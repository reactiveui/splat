using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Splat
{
    public interface IEventLoop
    {
        SynchronizationContext Context { get; }
        void Stop();
    }

    public static class EventLoop
    {
        private static readonly CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
        private static readonly IEventLoop mainloop = new CoreDispatcherEventLoop();

        public static IEventLoop Current 
        { 
            get 
            {
                if (dispatcher.HasThreadAccess) {
                    return EventLoop.MainLoop;
                }

                throw new PlatformNotSupportedException("WinRT doesn't support adding event loops to task pool threads");
            } 
        }

        public static IEventLoop MainLoop { get { return mainloop; } }

        public static Task<SynchronizationContext> Spawn()
        {
            throw new PlatformNotSupportedException("WinRT doesn't support creating threads");
        }

        private sealed class CoreDispatcherEventLoop : IEventLoop
        {
            private SynchronizationContext context =  new CoreDispatcherSynchronizationContext();

            public SynchronizationContext Context { get { return context; } }

            public void Stop()
            {
                throw new PlatformNotSupportedException("Cannot call Stop() on the main loop on WinRT");
            }
        }

        private sealed class CoreDispatcherSynchronizationContext : SynchronizationContext
        {            
            public override void Post(SendOrPostCallback d, object state)
            {
                Contract.Requires(d != null);

                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => d(state));
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException();
            }
        }
    }
}
