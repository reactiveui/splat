using Android.OS;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public static partial class EventLoop
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

                return new AndroidEventLoop(new Handler(looper));
            });
                
        private static readonly IEventLoop mainloop = new AndroidEventLoop(new Handler(Looper.MainLooper));

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
                    Looper.Loop();
                }).Start();

            return completer.Task;
        }

        private sealed class AndroidEventLoop : IEventLoop
        {
            private readonly Handler handler;

            internal AndroidEventLoop(Handler handler)
            {
                this.handler = handler;
            }
                
            public Task PostAsync(Action block)
            {
                var completer = new TaskCompletionSource<object>();
                handler.Post(() => 
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
                    });
                return completer.Task;
            }

            public Task StopAsync()
            {
                return PostAsync(() => 
                    {
                        // FIXME: Try catch QuitSafely for Nosuchmethod first.
                        Looper.MyLooper().Quit();
                    });
            }
        }
    }
}

