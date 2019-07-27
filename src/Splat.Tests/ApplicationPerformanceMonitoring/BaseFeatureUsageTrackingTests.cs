using System;
using System.Collections.Generic;
using System.Text;
using Splat.ApplicationPerformanceMonitoring;
using Xunit;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Common Unit Tests for Feature Usage Tracking.
    /// </summary>
    public static class BaseFeatureUsageTrackingTests
    {
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        /// <typeparam name="TFeatureUsageTracking">Type of Feature Usage Tracking Session Class to test.</typeparam>
        public abstract class BaseConstructorTests<TFeatureUsageTracking>
            where TFeatureUsageTracking : IFeatureUsageTrackingSession<Guid>
        {
            /// <summary>
            /// Test to make sure a root tracking session is set up correctly.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var featureName = Guid.NewGuid().ToString();
                var instance = GetFeatureUsageTrackingSession(featureName);
                Assert.NotNull(instance);
                Assert.Equal(featureName, instance.FeatureName);
                Assert.NotEqual(Guid.Empty, instance.FeatureReference);
                Assert.Equal(Guid.Empty, instance.ParentReference);
            }

            /// <summary>
            /// Gets a Feature Usage Tracking Session.
            /// </summary>
            /// <param name="featureName">Name of the feature being tracked.</param>
            /// <returns>Feature Usage Tracking Session.</returns>
            protected abstract TFeatureUsageTracking GetFeatureUsageTrackingSession(string featureName);
        }
    }
}
