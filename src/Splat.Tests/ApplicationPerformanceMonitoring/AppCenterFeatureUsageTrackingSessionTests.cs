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
        /// <inheritdoc />
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<AppCenterFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new AppCenterFeatureUsageTrackingSession(featureName);
            }
        }

        /// <inheritdoc />
        public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<AppCenterFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new AppCenterFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
