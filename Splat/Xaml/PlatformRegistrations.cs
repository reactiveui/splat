using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    static class PlatformRegistrations
    {
        internal static void Register(IMutableDependencyResolver This)
        {
            This.RegisterConstant(new PlatformBitmapLoader(), typeof(IBitmapLoader));
            This.RegisterConstant(new PlatformModeDetector(), typeof(IModeDetector));
            This.RegisterConstant(new UiThreadDispatcherHook(), typeof(IUiThreadDispatcherHook));
            This.RegisterConstant(new ProfilerPlatformOperations(), typeof(IProfilerPlatformOperations));
        }
    }
}
