using System;
using System.Collections.Generic;
using System.Text;
using Splat.ApplicationPerformanceMonitoring;
using Xunit;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for the IEnableFeatureUsageTracking Extensions.
    /// </summary>
    public static class EnableFeatureUsageTrackingExtensionsTests
    {
        /// <summary>
        /// Dummy object for testing IEnableFeatureUsageTracking.
        /// </summary>
        public sealed class TestObjectThatSupportsFeatureUsageTracking : IEnableFeatureUsageTracking
        {
        }

        /// <summary>
        /// Unit tests for the FeatureUsageTrackingExtensionMethod.
        /// </summary>
        public sealed class FeatureUsageTrackingSessionMethod
        {
            /// <summary>
            /// Test to ensure a default feature usage tracking session is set up.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new TestObjectThatSupportsFeatureUsageTracking();
                var result = instance.FeatureUsageTrackingSession("featureName");
                Assert.NotNull(result);
                Assert.IsType<DefaultFeatureUsageTrackingSession>(result);
            }
        }
    }
}
