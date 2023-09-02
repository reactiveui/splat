// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for the IEnableFeatureUsageTracking Extensions.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate Usage")]
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
                result.OnException(new("Test"));
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
                    subFeature.OnException(new("Sub-feature"));
                }
            }
        }
    }

    /// <summary>
    /// Unit tests for the WithFeatureUsageTrackingSession Method.
    /// </summary>
    public sealed class WithFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        [Fact]
        public void HandlesAction()
        {
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                _ => handled = true);

            Assert.True(handled);
        }
    }

    /// <summary>
    /// Unit tests for the WithSubFeatureUsageTrackingSession Method.
    /// </summary>
    public sealed class WithSubFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        [Fact]
        public void HandlesAction()
        {
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                session => session.WithSubFeatureUsageTrackingSession("SubFeature", _ => handled = true));

            Assert.True(handled);
        }
    }
}
