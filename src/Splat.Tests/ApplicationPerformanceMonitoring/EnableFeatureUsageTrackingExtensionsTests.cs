// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for the IEnableFeatureUsageTracking Extensions.
/// </summary>
[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate Usage")]
[SuppressMessage("StyleCop", "CA1034: Nested types should not be visible", Justification = "Test Class")]
[TestFixture]
public static class EnableFeatureUsageTrackingExtensionsTests
{
    static EnableFeatureUsageTrackingExtensionsTests() => Locator.CurrentMutable.InitializeSplat();

    /// <summary>
    /// Dummy object for testing IEnableFeatureUsageTracking.
    /// </summary>
    [TestFixture]
    public sealed class TestObjectThatSupportsFeatureUsageTracking : IEnableFeatureUsageTracking;

    /// <summary>
    /// Unit tests for the FeatureUsageTrackingExtensionMethod.
    /// </summary>
    [TestFixture]
    public sealed class FeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        [Test]
        public void ReturnsInstance()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            const string expected = "featureName";
            using var result = instance.FeatureUsageTrackingSession(expected);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            Assert.That(result.FeatureName, Is.EqualTo(expected));
        }

        /// <summary>
        /// Test to ensure a default feature usage tracking session handles an exception.
        /// </summary>
        [Test]
        public void HandleOnException()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            result.OnException(new("Test"));
        }

        /// <summary>
        /// Test to make sure a sub-feature is returned.
        /// </summary>
        [Test]
        public void ReturnsSubFeatureInstance()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            const string expected = "Sub-feature Name";
            using var subFeature = result.SubFeature(expected);
            Assert.That(subFeature, Is.Not.Null);
            Assert.That(subFeature, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            Assert.That(subFeature.FeatureName, Is.EqualTo(expected));
        }

        /// <summary>
        /// Test to make sure a sub-feature handles an Exception.
        /// </summary>
        [Test]
        public void SubFeatureHandlesOnException()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            using var result = instance.FeatureUsageTrackingSession("featureName");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            using var subFeature = result.SubFeature("Sub-feature Name");
            Assert.That(subFeature, Is.Not.Null);
            Assert.That(subFeature, Is.TypeOf<DefaultFeatureUsageTrackingSession>());
            subFeature.OnException(new("Sub-feature"));
        }
    }

    /// <summary>
    /// Unit tests for the WithFeatureUsageTrackingSession Method.
    /// </summary>
    [TestFixture]
    public sealed class WithFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        [Test]
        public void HandlesAction()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                _ => handled = true);

            Assert.That(handled, Is.True);
        }
    }

    /// <summary>
    /// Unit tests for the WithSubFeatureUsageTrackingSession Method.
    /// </summary>
    [TestFixture]
    public sealed class WithSubFeatureUsageTrackingSessionMethod
    {
        /// <summary>
        /// Test to ensure a default feature usage tracking session is set up.
        /// </summary>
        [Test]
        public void HandlesAction()
        {
            Locator.CurrentMutable.InitializeSplat();
            var instance = new TestObjectThatSupportsFeatureUsageTracking();
            var handled = false;

            instance.WithFeatureUsageTrackingSession(
                "FeatureName",
                session => session.WithSubFeatureUsageTrackingSession("SubFeature", _ => handled = true));

            Assert.That(handled, Is.True);
        }
    }
}
