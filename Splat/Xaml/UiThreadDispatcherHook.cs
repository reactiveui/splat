using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Splat
{
    class UiThreadDispatcherHook : IUiThreadDispatcherHook
    {
        int nextDispatcherId = 1;
        readonly Dictionary<DispatcherOperation, int> dispatcherMap = new Dictionary<DispatcherOperation,int>();

        public IDisposable RegisterHook(Action<int> dispatcherQueued, Action<int> dispatcherItemStarted, Action<int> dispatcherItemFinished, Action<int> dispatcherItemCancelled)
        {
            var queued = new DispatcherHookEventHandler((o, e) => dispatcherQueued(getOrCreateDispatcherId(e.Operation)));
            var started = new DispatcherHookEventHandler((o, e) => dispatcherItemStarted(getOrCreateDispatcherId(e.Operation)));
            var completed = new DispatcherHookEventHandler((o, e) => dispatcherItemFinished(getOrCreateDispatcherId(e.Operation, true)));
            var cancelled = new DispatcherHookEventHandler((o, e) => dispatcherItemCancelled(getOrCreateDispatcherId(e.Operation, true)));

            Dispatcher.CurrentDispatcher.Hooks.OperationPosted += queued;
            Dispatcher.CurrentDispatcher.Hooks.OperationStarted += started;
            Dispatcher.CurrentDispatcher.Hooks.OperationCompleted += completed;
            Dispatcher.CurrentDispatcher.Hooks.OperationAborted += cancelled;

            return new ActionDisposable(() => {
                Dispatcher.CurrentDispatcher.Hooks.OperationPosted -= queued;
                Dispatcher.CurrentDispatcher.Hooks.OperationStarted -= started;
                Dispatcher.CurrentDispatcher.Hooks.OperationCompleted -= completed;
                Dispatcher.CurrentDispatcher.Hooks.OperationAborted -= cancelled;
            });
        }

        int getOrCreateDispatcherId(DispatcherOperation op, bool shouldRemove = false)
        {
            if (dispatcherMap.ContainsKey(op)) {
                var ret = dispatcherMap[op];
                if (shouldRemove) dispatcherMap.Remove(op);
                return ret;
            }

            var newId = Interlocked.Increment(ref nextDispatcherId);
            if (!shouldRemove) dispatcherMap[op] = newId;
            return newId;
        }
    }
}
