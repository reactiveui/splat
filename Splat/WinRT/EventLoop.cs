using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Splat
{
    public static partial class EventLoop
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

        public static Task<IEventLoop> Spawn()
        {
            throw new PlatformNotSupportedException("WinRT doesn't support creating threads");
        }

        private sealed class CoreDispatcherEventLoop : IEventLoop
        {
            public Task PostAsync(Action block)
            {
                return dispatcher.RunAsync(() =>
                    {
                        block();
                        return null;
                    });
            }

            public Task StopAsync()
            {
                throw new PlatformNotSupportedException("Cannot call StopAsync() on the main loop on WinRT");
            }
        }
    }
}
