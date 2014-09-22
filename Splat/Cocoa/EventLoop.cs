using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

#if UIKIT && !UNIFIED
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#elif UNIFIED && UIKIT
using UIKit;
using Foundation;
using ObjCRuntime;
#elif UNIFIED && !UIKIT
using AppKit;
using Foundation;
using ObjCRuntime;
#else
using MonoMac.AppKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#endif

namespace Splat
{
    public interface IEventLoop
    {
        SynchronizationContext Context { get; }
        void Stop();
    }

    public static class EventLoop
    {
        private static readonly ThreadLocal<IEventLoop> current = new ThreadLocal<IEventLoop>(() => 
            {   
                NSRunLoop runLoop = NSRunLoop.Current;

                if (runLoop == NSRunLoop.Main) {
                    return EventLoop.MainLoop;
                } 

                return new NSRunLoopEventLoop(runLoop, NSThread.Current);
            });

        private static readonly IEventLoop mainloop = new NSMainThreadEventLoop();
                
        public static IEventLoop Current { get { return current.Value; } }

        public static IEventLoop MainLoop { get { return mainloop; } }

        public static Task<SynchronizationContext> Spawn()
        {
            var retval = new TaskCompletionSource<SynchronizationContext>();

            (new Thread(() => 
                {
                    Thread.CurrentThread.IsBackground = true;
                    IEventLoop eventLoop = EventLoop.Current;

                    SynchronizationContext.SetSynchronizationContext(eventLoop.Context);
                    retval.SetResult(eventLoop.Context);

                    ((NSRunLoopEventLoop) eventLoop).Run();
                })).Start();

            return retval.Task;
        }

        private sealed class NSMainThreadEventLoop : NSObject, IEventLoop
        {
            private readonly SynchronizationContext context = new NSThreadSynchronizationContext(NSThread.MainThread);

            public SynchronizationContext Context { get { return context; } }

            public void Stop()
            {
#if UIKIT
                throw new NotSupportedException("Cannot call Stop() on the main loop on iOS");
#else
                NSApplication.Stop(this);
#endif
            }
        }

        private sealed class NSRunLoopEventLoop : IEventLoop
        {
            private readonly NSRunLoop runLoop;
            private readonly SynchronizationContext context;
            private volatile bool stopped = false;

            internal NSRunLoopEventLoop(NSRunLoop runLoop, NSThread thread)
            {
                this.runLoop = runLoop;
                this.context = new NSThreadSynchronizationContext(thread);
            }

            public SynchronizationContext Context { get { return context; } }

            internal void Run()
            {
                stopped = false;
                while (!stopped) {
                    runLoop.RunUntil(NSRunLoopMode.Default, NSDate.DistantFuture);
                }
            }

            public void Stop()
            {
                stopped = true;
                runLoop.Stop();
            }
        }

        private sealed class NSThreadSynchronizationContext : SynchronizationContext
        {
            private readonly NSThread thread;

            internal NSThreadSynchronizationContext(NSThread thread)
            {
                this.thread = thread;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Contract.Requires(d != null);

                var callback = new NSSendOrPostCallback(d, state);
                callback.PerformSelector(new Selector("Invoke"), thread, NSNull.Null, false);
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class NSSendOrPostCallback : NSObject
        {
            private readonly SendOrPostCallback d;
            private readonly object state;

            internal NSSendOrPostCallback(SendOrPostCallback d, object state)
            {
                this.d = d;
                this.state = state;
            }

            [Export ("Invoke")]
            public void Invoke()
            {
                d(state);
            }
        }
    }
}

