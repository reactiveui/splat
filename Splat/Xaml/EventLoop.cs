using System;
using System.Windows.Threading;
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

        public static Task<SynchronizationContext> Spawn()
        {
            var retval = new TaskCompletionSource<SynchronizationContext>();
            (new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    IEventLoop eventLoop = EventLoop.Current;

                    retval.SetResult(eventLoop.Context);
                    Dispatcher.Run();
                })).Start();

            return retval.Task;
        }

        private sealed class DispatcherEventLoop : IEventLoop
        {
            private readonly Dispatcher dispatcher;
            private readonly SynchronizationContext context;

            internal DispatcherEventLoop(Dispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
                this.context = new DispatcherSynchronizationContext(dispatcher);
            }

            public SynchronizationContext Context { get { return context; } }

            public void Stop()
            {
                dispatcher.InvokeShutdown();
            }
        }
    }
}
