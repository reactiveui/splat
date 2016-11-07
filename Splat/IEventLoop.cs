using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    public interface IEventLoop
    {
        Task PostAsync(Action block);
        Task StopAsync();
    }

    public static partial class EventLoop
    {
        public static SynchronizationContext AsSynchronizationContext(this IEventLoop This) {
            return new EventLoopSynchronizationContext(This);
        }

        private sealed class EventLoopSynchronizationContext : SynchronizationContext
        {
            private readonly IEventLoop eventLoop;

            internal EventLoopSynchronizationContext(IEventLoop eventLoop)
            {
                this.eventLoop = eventLoop;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Contract.Requires(d != null);

                eventLoop.PostAsync(() => d(state));
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException();
            }
        }
    }
}

