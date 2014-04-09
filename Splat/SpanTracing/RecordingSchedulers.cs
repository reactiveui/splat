using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
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

            _dispatcherThreadId = platformOps.GetRealThreadIdentifier();

            Current = this;
        }

        public ulong GetThreadIdentifier(int? dispatcherOperationId = null)
        {
            if (dispatcherOperationId != null) {
                return _dispatcherHighWord | (uint)dispatcherOperationId.Value;
            }

            if (_runningDispatcherOperation == 0 || _dispatcherThreadId != platformOps.GetRealThreadIdentifier()) {
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
                var ctx = platformOps.GetSpanContextIdentifier();
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

#if MONO
        internal const string defaultSchedulerBackingField = "defaultScheduler";
#else
        internal const string defaultSchedulerBackingField = "s_defaultTaskScheduler";
#endif

        TaskScheduler _inner;
        TaskFactory _factory;

        public RecordingTaskScheduler(TaskScheduler inner = null)
        {
            _inner = inner;

            if (_inner == null) {
                _inner = (TaskScheduler) typeof(TaskScheduler).GetField(defaultSchedulerBackingField, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            }

            _factory = new TaskFactory(_inner);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return (IEnumerable<Task>)_getScheduledTasks.Invoke(_inner, new object[0]);
        }

        protected override void QueueTask(Task task)
        {
            var oldId = _platformOps.GetSpanContextIdentifier();
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

        public void InstallScheduler()
        {
            var fi = typeof(TaskScheduler).GetField(defaultSchedulerBackingField, BindingFlags.Static | BindingFlags.NonPublic);
            fi.SetValue(null, this);
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
                return (_threadHighWord | (uint)_platformOps.GetRealThreadIdentifier());
            }
        }

        public static bool ContextIsNotScheduled(ulong ctx)
        {
            return (ctx & _threadHighWord) == _threadHighWord;
        }
    }
}