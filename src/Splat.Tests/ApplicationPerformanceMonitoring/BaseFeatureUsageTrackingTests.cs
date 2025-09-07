// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Common Unit Tests for Feature Usage Tracking.
/// </summary>
[TestFixture]
public static class BaseFeatureUsageTrackingTests
{
    /// <summary>
    /// Unit Tests for the constructor.
    /// </summary>
    /// <typeparam name="TFeatureUsageTracking">Type of Feature Usage Tracking Session Class to test.</typeparam>
    [TestFixture]
    public abstract class BaseConstructorTests<TFeatureUsageTracking>
        where TFeatureUsageTracking : IFeatureUsageTrackingSession<Guid>
    {
        /// <summary>
        /// Test to make sure a root tracking session is set up correctly.
        /// </summary>
        [Test]
        public void ReturnsInstance()
        {
            var featureName = Guid.NewGuid().ToString();
            var instance = GetFeatureUsageTrackingSession(featureName);
            Assert.That(instance, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(instance.FeatureName, Is.EqualTo(featureName));
                Assert.That(instance.FeatureReference, Is.Not.EqualTo(Guid.Empty));
                Assert.That(instance.ParentReference, Is.EqualTo(Guid.Empty));
            }
        }

        /// <summary>
        /// Gets a Feature Usage Tracking Session.
        /// </summary>
        /// <param name="featureName">Name of the feature being tracked.</param>
        /// <returns>Feature Usage Tracking Session.</returns>
        protected abstract TFeatureUsageTracking GetFeatureUsageTrackingSession(string featureName);
    }

    /// <summary>
    /// Unit Tests for the sub-feature method.
    /// </summary>
    /// <typeparam name="TFeatureUsageTracking">Type of Feature Usage Tracking Session Class to test.</typeparam>
    [TestFixture]
    public abstract class BaseSubFeatureMethodTests<TFeatureUsageTracking>
        where TFeatureUsageTracking : IFeatureUsageTrackingSession<Guid>
    {
        /// <summary>
        /// Test to make sure a sub-feature tracking session is set up correctly.
        /// </summary>
        [Test]
        public void ReturnsInstance()
        {
            var featureName = Guid.NewGuid().ToString();
            var instance = GetFeatureUsageTrackingSession(featureName);
            Assert.That(instance, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(instance.FeatureName, Is.EqualTo(featureName));
                Assert.That(instance.FeatureReference, Is.Not.EqualTo(Guid.Empty));
                Assert.That(instance.ParentReference, Is.EqualTo(Guid.Empty));
            }

            var subfeatureName = Guid.NewGuid().ToString();
            var subfeature = instance.SubFeature(subfeatureName);
            Assert.That(instance, Is.Not.Null);

            var genericSubfeature = subfeature as IFeatureUsageTrackingSession<Guid>;
            Assert.That(genericSubfeature, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(genericSubfeature?.FeatureName, Is.EqualTo(subfeatureName));
                Assert.That(genericSubfeature?.FeatureReference, Is.Not.EqualTo(Guid.Empty));
                Assert.That(genericSubfeature?.ParentReference, Is.EqualTo(instance.FeatureReference));
            }
        }

        /// <summary>
        /// Gets a Feature Usage Tracking Session.
        /// </summary>
        /// <param name="featureName">Name of the feature being tracked.</param>
        /// <returns>Feature Usage Tracking Session.</returns>
        protected abstract TFeatureUsageTracking GetFeatureUsageTrackingSession(string featureName);
    }
}
