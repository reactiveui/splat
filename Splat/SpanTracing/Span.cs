using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public class Span
    {
        static int nextId;

        public Span Parent { get; set; }
        public object Context { get; set; }
        public int Id { get; set; }

        static readonly Dictionary<ulong, Span> contextMap = new Dictionary<ulong, Span>();
        static readonly IProfilerPlatformOperations platformOps;

        List<ulong> associatedThreads = new List<ulong>();
        int refCount = 1;

        static Span()
        {
            platformOps = Locator.Current.GetService<IProfilerPlatformOperations>();
        }

        protected Span() { }

        public static IDisposable EnterSpan(object context)
        {
            var ctx = platformOps.GetSpanContextIdentifier();

            var ret = new Span() {
                Context = context,
                Parent = GetSpanForContext(ctx),
                Id = Interlocked.Increment(ref nextId),
            };

            if (!platformOps.IsContextSpanNotScheduled(ctx)) {
                ret.AddRef();
            }

            ret.AssociateSpanWithContext(ctx);

            return new ActionDisposable(ret.Release);
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

        public void IterateThroughAncestors(Func<Span, bool> iterator)
        {
            var parentSpan = this;

            while (parentSpan != null) {
                if (!iterator(parentSpan)) break;
                parentSpan = parentSpan.Parent;
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

        /* XXX: This goes to PlatformOps
        public static ulong GetThreadIdentifier()
        {
            var ret = RecordingDispatcherSchedulerHook.Current != null ?
                RecordingDispatcherSchedulerHook.Current.GetThreadIdentifier() :
                default(ulong);

            return (ret != 0 ? ret : RecordingTaskScheduler.GetThreadIdentifier());
        }
        */
    }
}
