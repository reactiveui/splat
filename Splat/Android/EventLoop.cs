using Android.OS;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

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
                if (Looper.MyLooper() == null) {
                    Looper.Prepare();
                }

                Looper looper = Looper.MyLooper();

                if (looper == Looper.MainLooper) {
                    return EventLoop.MainLoop;
                } 

                return new AndroidEventLoop(looper);
            });
                
        private static readonly IEventLoop mainloop = new AndroidEventLoop(Looper.MainLooper);

        public static IEventLoop Current { get { return current.Value; } }

        public static IEventLoop MainLoop { get { return mainloop; } }
 
        public static Task<SynchronizationContext> Spawn()
        {
            var retval = new TaskCompletionSource<SynchronizationContext>();
            new Thread(() => 
                {
                    Thread.CurrentThread.IsBackground = true;
                    IEventLoop eventLoop = EventLoop.Current;

                    SynchronizationContext.SetSynchronizationContext(eventLoop.Context);
                    retval.SetResult(eventLoop.Context);
                    Looper.Loop();
                }).Start();

            return retval.Task;
        }

        private sealed class AndroidEventLoop : IEventLoop
        {
            private readonly Looper looper;
            private readonly SynchronizationContext context;

            internal AndroidEventLoop(Looper looper)
            {
                this.looper = looper;
                this.context = new HandlerSynchronizationContext(new Handler(looper));
            }

            public SynchronizationContext Context { get { return context; } }

            public void Stop()
            {
                // FIXME: Try catch QuitSafely for Nosuchmethod first.
                looper.Quit();
            }
        }

        private sealed class HandlerSynchronizationContext : SynchronizationContext
        {
            private readonly Handler handler;

            internal HandlerSynchronizationContext(Handler handler)
            {
                this.handler = handler;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Contract.Requires(d != null);

                handler.Post(() => d(state));
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException();
            }
        }
    }
}

