// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

public static class BaseFeatureUsageTrackingTests
{
    public abstract class BaseConstructorTests<TFeatureUsageTracking>
        where TFeatureUsageTracking : class, IFeatureUsageTrackingSession<Guid>
    {
        /// <summary>
        /// Test to make sure a root tracking session is set up correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task ReturnsInstance()
        {
            var featureName = Guid.NewGuid().ToString();
            var instance = GetFeatureUsageTrackingSession(featureName);
            await Assert.That(instance).IsNotNull();
            using (Assert.Multiple())
            {
                await Assert.That(instance.FeatureName).IsEqualTo(featureName);
                await Assert.That(instance.FeatureReference).IsNotEqualTo(Guid.Empty);
                await Assert.That(instance.ParentReference).IsEqualTo(Guid.Empty);
            }
        }

        /// <summary>
        /// Gets a Feature Usage Tracking Session.
        /// </summary>
        /// <param name="featureName">Name of the feature being tracked.</param>
        /// <returns>Feature Usage Tracking Session.</returns>
        protected abstract TFeatureUsageTracking GetFeatureUsageTrackingSession(string featureName);
    }

    public abstract class BaseSubFeatureMethodTests<TFeatureUsageTracking>
        where TFeatureUsageTracking : class, IFeatureUsageTrackingSession<Guid>
    {
        /// <summary>
        /// Test to make sure a sub-feature tracking session is set up correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task ReturnsInstance()
        {
            var featureName = Guid.NewGuid().ToString();
            var instance = GetFeatureUsageTrackingSession(featureName);
            await Assert.That(instance).IsNotNull();
            using (Assert.Multiple())
            {
                await Assert.That(instance.FeatureName).IsEqualTo(featureName);
                await Assert.That(instance.FeatureReference).IsNotEqualTo(Guid.Empty);
                await Assert.That(instance.ParentReference).IsEqualTo(Guid.Empty);
            }

            var subfeatureName = Guid.NewGuid().ToString();
            var subfeature = instance.SubFeature(subfeatureName);
            await Assert.That(instance).IsNotNull();

            var genericSubfeature = subfeature as IFeatureUsageTrackingSession<Guid>;
            await Assert.That(genericSubfeature).IsNotNull();
            using (Assert.Multiple())
            {
                await Assert.That(genericSubfeature?.FeatureName).IsEqualTo(subfeatureName);
                await Assert.That(genericSubfeature?.FeatureReference).IsNotEqualTo(Guid.Empty);
                await Assert.That(genericSubfeature?.ParentReference).IsEqualTo(instance.FeatureReference);
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
