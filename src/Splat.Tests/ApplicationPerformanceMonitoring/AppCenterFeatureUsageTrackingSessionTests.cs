using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for App Center Feature Usage Tracking.
    /// </summary>
    public static class AppCenterFeatureUsageTrackingSessionTests
    {
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<AppCenterFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new AppCenterFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
