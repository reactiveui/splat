using System;
using System.Collections.Generic;
using System.Text;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for Exceptionless Feature Usage Tracking.
    /// </summary>
    public static class ExceptionlessFeatureUsageTrackingSessionTests
    {
        /// <inheritdoc />>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ExceptionlessFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override ExceptionlessFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new ExceptionlessFeatureUsageTrackingSession(featureName);
            }
        }

        /// <inheritdoc />>
        public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<ExceptionlessFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override ExceptionlessFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new ExceptionlessFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
