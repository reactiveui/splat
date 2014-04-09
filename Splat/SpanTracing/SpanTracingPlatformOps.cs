using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    class ProfilerPlatformOperations : IProfilerPlatformOperations
    {
        public void Initialize()
        {
            (new RecordingTaskScheduler()).InstallScheduler();
        }

        public ulong GetSpanContextIdentifier()
        {
            var ret = RecordingDispatcherSchedulerHook.Current != null ?
                RecordingDispatcherSchedulerHook.Current.GetThreadIdentifier() :
                default(ulong);

            return (ret != 0 ? ret : RecordingTaskScheduler.GetThreadIdentifier());
        }

        public int GetRealThreadIdentifier()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        public bool IsContextSpanNotScheduled(ulong spanContext)
        {
            return RecordingTaskScheduler.ContextIsNotScheduled(spanContext);
        }
    }
}
