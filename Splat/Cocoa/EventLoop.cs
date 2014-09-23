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
    public static partial class EventLoop
    {
        private static readonly ThreadLocal<IEventLoop> current = new ThreadLocal<IEventLoop>(() => 
            {   
                NSThread thread = NSThread.Current;

                if (thread == NSThread.MainThread) {
                    return EventLoop.MainLoop;
                } 

                return new NSRunLoopEventLoop(thread);
            });

        private static readonly IEventLoop mainloop = new NSMainThreadEventLoop();
                
        public static IEventLoop Current { get { return current.Value; } }

        public static IEventLoop MainLoop { get { return mainloop; } }

        public static Task<IEventLoop> Spawn()
        {
            var completer = new TaskCompletionSource<IEventLoop>();

            new Thread(() => 
                {
                    Thread.CurrentThread.IsBackground = true;
                    IEventLoop eventLoop = EventLoop.Current;

                    SynchronizationContext.SetSynchronizationContext(eventLoop.AsSynchronizationContext());
                    completer.SetResult(eventLoop);

                    ((NSRunLoopEventLoop) eventLoop).Run();
                }).Start();

            return completer.Task;
        }

        private sealed class NSRunLoopEventLoop : NSObject, IEventLoop
        {
            private readonly NSThread thread;
            private volatile bool stopped = false;

            internal NSRunLoopEventLoop(NSThread thread)
            {
                this.thread = thread;
            }

            public Task PostAsync(Action block)
            {
                var completer = new TaskCompletionSource<object>();
                var callback = new NSSelectableAction(block, completer);
                callback.PerformSelector(new Selector("Invoke"), thread, NSNull.Null, false);
                return completer.Task;
            }

            internal void Run()
            {
                if (NSThread.Current != thread) {
                    throw new NotSupportedException("Run called on a different thread");
                }

                stopped = false;
                while (!stopped) {
                    NSRunLoop.Current.RunUntil(NSRunLoopMode.Default, NSDate.DistantFuture);
                }
            }

            public Task StopAsync()
            {
                return PostAsync(() => 
                    {
                        stopped = true;
                        NSRunLoop.Current.Stop();
                    });
            }
        }

        private sealed class NSMainThreadEventLoop : NSObject, IEventLoop
        {
            public Task PostAsync(Action block)
            {
                var completer = new TaskCompletionSource<object>();
                var callback = new NSSelectableAction(block, completer);
                callback.PerformSelector(new Selector("Invoke"), NSThread.MainThread, NSNull.Null, false);
                return completer.Task;
            }

            public Task StopAsync()
            {
#if UIKIT
                throw new NotSupportedException("Cannot call Stop() on the main loop on iOS");
#else
                return PostAsync(() =>
                    {
                        NSApplication.Stop(this);
                    }
#endif
            }
        }

        private sealed class NSSelectableAction : NSObject 
        {
            private readonly Action block;
            private readonly TaskCompletionSource<object> completer;

            internal NSSelectableAction (Action block, TaskCompletionSource<object> completer)
            {
                this.block = block;
                this.completer = completer;
            }

            [Export ("Invoke")]
            public void Invoke()
            {
                try
                {
                    block();
                    completer.SetResult(null);
                } 
                catch (Exception e) 
                {
                    completer.SetException(e);
                }
            }
        }
    }
}

