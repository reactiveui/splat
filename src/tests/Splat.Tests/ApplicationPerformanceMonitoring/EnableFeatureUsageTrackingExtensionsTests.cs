// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

[NotInParallel] // ensure this logical group doesn't run alongside others
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Deliberate nesting for the tests")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberately passing exception for the tests")]
public static class EnableFeatureUsageTrackingExtensionsTests
{
    static EnableFeatureUsageTrackingExtensionsTests() => Locator.CurrentMutable.InitializeSplat();

    public sealed class TestObjectThatSupportsFeatureUsageTracking : IEnableFeatureUsageTracking;

    public sealed class FeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task ReturnsInstance()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            const string expected = "featureName";
            using var result = instance.FeatureUsageTrackingSession(expected);
            await Assert.That(result).IsNotNull();
            await Assert.That(result).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            await Assert.That(result.FeatureName).IsEqualTo(expected);
        }

        /// <summary>
        /// Test to ensure a default feature usage tracking session handles an exception.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task HandleOnException()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            await Assert.That(result).IsNotNull();
            await Assert.That(result).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            result.OnException(new("Test"));
        }

        /// <summary>
        /// Test to make sure a sub-feature is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task ReturnsSubFeatureInstance()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            await Assert.That(result).IsNotNull();
            await Assert.That(result).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            const string expected = "Sub-feature Name";
            using var subFeature = result.SubFeature(expected);
            await Assert.That(subFeature).IsNotNull();
            await Assert.That(subFeature).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            await Assert.That(subFeature.FeatureName).IsEqualTo(expected);
        }

        /// <summary>
        /// Test to make sure a sub-feature handles an Exception.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task SubFeatureHandlesOnException()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            await Assert.That(result).IsNotNull();
            await Assert.That(result).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            using var subFeature = result.SubFeature("Sub-feature Name");
            await Assert.That(subFeature).IsNotNull();
            await Assert.That(subFeature).IsTypeOf<DefaultFeatureUsageTrackingSession>();
            subFeature.OnException(new("Sub-feature"));
        }
    }

    public sealed class WithFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task HandlesAction()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                _ => handled = true);

            await Assert.That(handled).IsTrue();
        }
    }

    public sealed class WithSubFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task HandlesAction()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                session => session.WithSubFeatureUsageTrackingSession("SubFeature", _ => handled = true));

            await Assert.That(handled).IsTrue();
        }
    }
}
