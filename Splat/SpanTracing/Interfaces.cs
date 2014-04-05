using System;

namespace Splat
{
    public interface IProfilerPlatformOperations
    {
        IDisposable Initialize();
        int GetThreadIdentifier();
    }

    interface IUiThreadDispatcherHook
    {
        IDisposable RegisterHook(
            Action<int> dispatcherQueued,
            Action<int> dispatcherItemStarted,
            Action<int> dispatcherItemFinished,
            Action<int> dispatcherItemCancelled);
    }
}
