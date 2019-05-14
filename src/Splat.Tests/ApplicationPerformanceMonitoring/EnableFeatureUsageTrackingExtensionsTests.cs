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
                const string expected = "featureName";
                using (var result = instance.FeatureUsageTrackingSession(expected))
                {
                    Assert.NotNull(result);
                    Assert.IsType<DefaultFeatureUsageTrackingSession>(result);
                    Assert.Equal(expected, result.FeatureName);
                }
            }

            /// <summary>
            /// Test to ensure a default feature usage tracking session handles an exception.
            /// </summary>
            [Fact]
            public void HandleOnException()
            {
                var instance = new TestObjectThatSupportsFeatureUsageTracking();
                using (var result = instance.FeatureUsageTrackingSession("featureName"))
                {
                    Assert.NotNull(result);
                    Assert.IsType<DefaultFeatureUsageTrackingSession>(result);
                    result.OnException(new Exception("Test"));
                }
            }

            /// <summary>
            /// Test to make sure a sub-feature is returned.
            /// </summary>
            [Fact]
            public void ReturnsSubFeatureInstance()
            {
                var instance = new TestObjectThatSupportsFeatureUsageTracking();
                using (var result = instance.FeatureUsageTrackingSession("featureName"))
                {
                    Assert.NotNull(result);
                    Assert.IsType<DefaultFeatureUsageTrackingSession>(result);
                    const string expected = "Sub-feature Name";
                    using (var subFeature = result.SubFeature(expected))
                    {
                        Assert.NotNull(subFeature);
                        Assert.IsType<DefaultFeatureUsageTrackingSession>(subFeature);
                        Assert.Equal(expected, subFeature.FeatureName);
                    }
                }
            }

            /// <summary>
            /// Test to make sure a sub-feature handles an Exception.
            /// </summary>
            [Fact]
            public void SubFeatureHandlesOnException()
            {
                var instance = new TestObjectThatSupportsFeatureUsageTracking();
                using (var result = instance.FeatureUsageTrackingSession("featureName"))
                {
                    Assert.NotNull(result);
                    Assert.IsType<DefaultFeatureUsageTrackingSession>(result);
                    using (var subFeature = result.SubFeature("Sub-feature Name"))
                    {
                        Assert.NotNull(subFeature);
                        Assert.IsType<DefaultFeatureUsageTrackingSession>(subFeature);
                        subFeature.OnException(new Exception("Sub-feature"));
                    }
                }
            }
        }
    }
}
