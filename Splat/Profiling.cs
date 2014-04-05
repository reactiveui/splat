using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public interface IProfilerPlatformOperations
    {
        IDisposable Initialize();
        int GetThreadIdentifier();
    }

    public class Span
    {
        static int nextId;

        public Span Parent { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }

        static readonly Dictionary<ulong, Span> contextMap = new Dictionary<ulong, Span>();

        List<ulong> associatedThreads = new List<ulong>();
        int refCount = 1;

        protected Span() { }

        public static IDisposable EnterSpan(string message)
        {
            var ctx = Span.GetThreadIdentifier();

            var ret = new Span() {
                Message = message,
                Parent = GetSpanForContext(ctx),
                Id = Interlocked.Increment(ref nextId),
            };

            if (!RecordingTaskScheduler.ContextIsNotScheduled(ctx)) {
                ret.AddRef();
            }

            ret.AssociateSpanWithContext(ctx);

            Profiler.Write("Entering Span: " + message);
            return new ActionDisposable(() => {
                Profiler.Write("Exiting Span: " + message);
                //Console.WriteLine("Exiting Span " + ret.Id + ": " + message);
                ret.Release();
            });
        }

        public void AddRef()
        {
            Interlocked.Increment(ref refCount);
            //Console.WriteLine("AddRef: {0} - {1}", refCount, Message);
        }

        public void Release()
        {
            //Console.WriteLine("Release: {0} - {1}", refCount - 1, Message);

            if (Interlocked.Decrement(ref refCount) <= 0) {
                lock (contextMap) {
                    foreach(var v in associatedThreads) {
                        if (!contextMap.ContainsKey(v) || contextMap[v] != this) continue;

                        //Console.WriteLine("{0:x} disassociated from {1}", v, this.Message);
                        contextMap.Remove(v);
                    }
                };
            }
        }

        public void AssociateSpanWithContext(ulong ctx)
        {
            lock (contextMap) {
                //Console.WriteLine("{0:x} associated with {1}", ctx, this.Message);
                contextMap[ctx] = this;
                associatedThreads.Add(ctx);
            }
        }

        public static Span GetSpanForContext(ulong ctx)
        {
            lock (contextMap) {
                if (!contextMap.ContainsKey(ctx)) return null;
                return contextMap[ctx];
            }
        }

        public bool SpanIsAlive {
            get { return (associatedThreads != null); }
        }

        public bool SpanHasReferences {
            get { return refCount > 0; }
        }

        public static ulong GetThreadIdentifier()
        {
            var ret = RecordingDispatcherSchedulerHook.Current != null ?
                RecordingDispatcherSchedulerHook.Current.GetThreadIdentifier() :
                default(ulong);

            return (ret != 0 ? ret : RecordingTaskScheduler.GetThreadIdentifier());
        }
    }

    public static class Profiler
    {
        const string MarkersTypeName = "Microsoft.ConcurrencyVisualizer.Instrumentation.Markers, Microsoft.ConcurrencyVisualizer.Markers, Version=12.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        static Type markersType;
        static MethodInfo enterSpan;
        static MethodInfo writeFlag;
        static MethodInfo writeMessage;

        static Profiler()
        {
            // NB: If this fails, we're in production
            markersType = Type.GetType(MarkersTypeName, false);
            if (markersType == null) return;

            // public Span EnterSpan(Importance level, int category, string text)
            enterSpan = markersType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(x =>
            {
                if (x.Name != "EnterSpan") return false;
                var p = x.GetParameters();
                return p.Length == 3 && p[2].ParameterType == typeof(string);
            });

            // public static void WriteFlag(Importance level, string text)
            writeFlag = markersType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(x =>
            {
                if (x.Name != "WriteFlag") return false;
                var p = x.GetParameters();
                return p.Length == 2 && p[0].ParameterType != typeof(int) && p[1].ParameterType == typeof(string);
            });

            // public static void WriteMessage(string text)
            writeMessage = markersType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(x =>
            {
                if (x.Name != "WriteMessage") return false;
                var p = x.GetParameters();
                return p.Length == 1 && p[0].ParameterType == typeof(string);
            });
        }

        public static IDisposable EnterSpan(string message, Importance importance = Importance.Normal)
        {
            if (markersType == null) {
                return Span.EnterSpan(message);
            }

            return (IDisposable)enterSpan.Invoke(null, new object[] { importance, 1, message });
        }

        public static void Write(string message, Importance importance = Importance.Normal)
        {
            if (markersType == null) {
                var ctx = Span.GetThreadIdentifier();
                var span = Span.GetSpanForContext(ctx);

                //Console.WriteLine("Write: {0:x}", ctx);
                if (span != null) {
                    var parentSpan = span;
                    var indent = "";

                    while (parentSpan != null) {
                        if (parentSpan.SpanIsAlive || true) indent = indent + parentSpan.Id + "   ";
                        parentSpan = parentSpan.Parent;
                    }

                    Console.WriteLine(indent + message);
                } else {
                    Console.WriteLine(message);
                }

                return;
            }

            switch (importance)
            {
            case Importance.Critical:
            case Importance.High:
                writeFlag.Invoke(null, new object[] { importance, message });
                return;
            default:
                writeMessage.Invoke(null, new object[] { message });
                return;
            }
        }
    }

    public sealed class RecordingDispatcherSchedulerHook : IDisposable
    {
        IDisposable _inner;
        uint _runningDispatcherOperation;
        int _dispatcherThreadId;

        readonly IProfilerPlatformOperations platformOps;
        readonly Dictionary<int, ulong> _queuedOps = new Dictionary<int, ulong>();

        static ulong _dispatcherHighWord;

        static RecordingDispatcherSchedulerHook()
        {
            _dispatcherHighWord = ((ulong)"dispatcher".GetHashCode()) << 32;
        }

        public static RecordingDispatcherSchedulerHook Current { get; private set; }

        public RecordingDispatcherSchedulerHook()
        {
            var dispatcherHookImpl = Locator.Current.GetService<IUiThreadDispatcherHook>();
            platformOps = Locator.Current.GetService<IProfilerPlatformOperations>();

            _inner = dispatcherHookImpl.RegisterHook(
                operationQueued,
                operationStarted,
                operationCompleted,
                operationCanceled);

            _dispatcherThreadId = platformOps.GetThreadIdentifier();

            Current = this;
        }

        public ulong GetThreadIdentifier(int? dispatcherOperationId = null)
        {
            if (dispatcherOperationId != null) {
                return _dispatcherHighWord | (uint)dispatcherOperationId.Value;
            }

            if (_runningDispatcherOperation == 0 || _dispatcherThreadId != platformOps.GetThreadIdentifier()) {
                return 0;
            }

            return _dispatcherHighWord | _runningDispatcherOperation;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _inner, ActionDisposable.Empty).Dispose();
        }

        void operationCanceled(int op)
        {
            lock (_queuedOps) {
                if (!_queuedOps.ContainsKey(op)) return;

                var span = Span.GetSpanForContext(_queuedOps[op]);
                if (span != null) {
                    span.Release();
                }

                _queuedOps.Remove(op);
            }
        }

        void operationCompleted(int op)
        {
            lock (_queuedOps) {
                if (_queuedOps.ContainsKey(op)) {
                    var span = Span.GetSpanForContext(_queuedOps[op]);
                    if (span != null) {
                        span.Release();
                    }

                    _queuedOps.Remove(op);
                }

                _runningDispatcherOperation = 0;
            }
        }

        void operationStarted(int op)
        {
            lock (_queuedOps) {
                if (!_queuedOps.ContainsKey(op)) {
                    // NB: If you have a lot of dispatcher events queued up,
                    // you can end up rolling through a whole set of them at
                    // once, without queuing them
                    return;
                }

                var opItem = _queuedOps[op];
                //Console.WriteLine("Running dispatcher task: {0:x}", GetThreadIdentifier(e.Operation));
                _runningDispatcherOperation = (uint)op;
            }
        }

        void operationQueued(int op)
        {
            lock (_queuedOps) {
                var ctx = Span.GetThreadIdentifier();
                _queuedOps[op] = ctx;

                var opCtx = GetThreadIdentifier(op);
                //Console.WriteLine("Scheduling Dispatcher {0:x}=>{1:x}", ctx, opCtx);

                var span = Span.GetSpanForContext(ctx);
                if (span != null) {
                    span.AssociateSpanWithContext(opCtx);
                    span.AddRef();
                }
            }
        }
    }

    public class RecordingTaskScheduler : TaskScheduler
    {
        static readonly MethodInfo _getScheduledTasks;
        static readonly MethodInfo _queueTask;
        static readonly MethodInfo _tryExecuteTaskInline;
        static readonly IProfilerPlatformOperations _platformOps;

        static readonly ulong _taskHighWord;
        static readonly ulong _threadHighWord;

        static RecordingTaskScheduler()
        {
            var t = typeof(TaskScheduler);
            _getScheduledTasks = t.GetMethod("GetScheduledTasks", BindingFlags.NonPublic | BindingFlags.Instance);
            _queueTask = t.GetMethod("QueueTask", BindingFlags.NonPublic | BindingFlags.Instance);
            _tryExecuteTaskInline = t.GetMethod("TryExecuteTaskInline", BindingFlags.NonPublic | BindingFlags.Instance);
            _platformOps = Locator.Current.GetService<IProfilerPlatformOperations>();

            _taskHighWord = ((ulong)"task".GetHashCode()) << 32;
            _threadHighWord = ((ulong)"thread".GetHashCode()) << 32;
        }

        TaskScheduler _inner;
        TaskFactory _factory;

        public RecordingTaskScheduler(TaskScheduler inner = null)
        {
            _inner = inner;

            if (_inner == null) {
                _inner = (TaskScheduler) typeof(TaskScheduler).GetField("s_defaultTaskScheduler", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            }

            _factory = new TaskFactory(_inner);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return (IEnumerable<Task>)_getScheduledTasks.Invoke(_inner, new object[0]);
        }

        protected override void QueueTask(Task task)
        {
            var oldId = Span.GetThreadIdentifier();
            //Console.WriteLine("Scheduling {0:x}=>{1:x}", oldId, GetThreadIdentifier(task));
            var span = Span.GetSpanForContext(oldId);

            if (span != null && span.SpanIsAlive) {
                span.AssociateSpanWithContext(GetThreadIdentifier(task));
                span.AddRef();
            }

            _factory.StartNew(() => {
                //Console.WriteLine("Running task: {0:x} from {1:x}", GetThreadIdentifier(task), oldId);
                _tryExecuteTaskInline.Invoke(_inner, new object[] { task, false });

                //Console.WriteLine("Exiting task: {0:x} from {1:x}", GetThreadIdentifier(task), oldId);
                if (span != null) span.Release();
            });
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            //Console.WriteLine("Running task inline: " + GetThreadIdentifier(task));
            return (bool)_tryExecuteTaskInline.Invoke(_inner, new object[] { task, taskWasPreviouslyQueued });
        }

        public static ulong GetThreadIdentifier(Task task = null)
        {
            if (task != null) {
                return (_taskHighWord | (uint)task.Id);
            }

            if (Task.CurrentId != null) {
                return (_taskHighWord | (uint)Task.CurrentId.Value);
            } else {
                return (_threadHighWord | (uint)_platformOps.GetThreadIdentifier());
            }
        }

        public static bool ContextIsNotScheduled(ulong ctx)
        {
            return (ctx & _threadHighWord) == _threadHighWord;
        }
    }

    interface IUiThreadDispatcherHook
    {
        IDisposable RegisterHook(
            Action<int> dispatcherQueued,
            Action<int> dispatcherItemStarted,
            Action<int> dispatcherItemFinished,
            Action<int> dispatcherItemCancelled);
    }
}
