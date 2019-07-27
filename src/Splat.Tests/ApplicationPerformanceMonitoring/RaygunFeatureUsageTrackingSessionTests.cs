using System;
using System.Collections.Generic;
using System.Text;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for Raygun Feature Usage Tracking.
    /// </summary>
    public static class RaygunFeatureUsageTrackingSessionTests
    {
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return new RaygunFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
