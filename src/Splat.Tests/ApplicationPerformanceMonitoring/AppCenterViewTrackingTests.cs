using System;
using System.Collections.Generic;
using System.Text;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for App Center View Tracking.
    /// </summary>
    public static class AppCenterViewTrackingTests
    {
        /// <inheritdoc/>
        public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<AppCenterViewTracking>
        {
            /// <inheritdoc/>
            protected override AppCenterViewTracking GetViewTracking()
            {
                return new AppCenterViewTracking();
            }
        }
    }
}
