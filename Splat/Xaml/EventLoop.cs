using System;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public static partial class EventLoop
    {
        private static readonly ThreadLocal<IEventLoop> current = new ThreadLocal<IEventLoop>(() =>
            {
                return new DispatcherEventLoop(Dispatcher.CurrentDispatcher);
            });

        public static IEventLoop Current { get { return current.Value; } }

        public static IEventLoop MainLoop
        {
            get
            {
                throw new PlatformNotSupportedException("WPF doesn't have a mainloop");
            }
        }

        public static Task<IEventLoop> Spawn()
        {
            var completer = new TaskCompletionSource<IEventLoop>();
            new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    IEventLoop eventLoop = EventLoop.Current;

                    completer.SetResult(eventLoop);
                    Dispatcher.Run();
                }).Start();

            return completer.Task;
        }

        private sealed class DispatcherEventLoop : IEventLoop
        {
            private readonly Dispatcher dispatcher;

            internal DispatcherEventLoop(Dispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            public Task PostAsync(Action block)
            {
                return dispatcher.InvokeAsync(block).Task;
            }

            public Task StopAsync()
            {
                var completer = new TaskCompletionSource<object>();
                dispatcher.ShutdownFinished += (o, e) => 
                    {
                        completer.SetResult(null);
                    };

                dispatcher.InvokeShutdown();
                return completer.Task;
            }
        }
    }
}
