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
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ExceptionlessFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override ExceptionlessFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new ExceptionlessFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
