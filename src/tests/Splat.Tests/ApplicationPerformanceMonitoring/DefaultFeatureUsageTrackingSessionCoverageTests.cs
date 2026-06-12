// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;
using Splat.Common.Test;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="DefaultFeatureUsageTrackingSession"/>.</summary>
[NotInParallel] // The session constructor resolves ILogManager from the global locator, so each test isolates it with AppLocatorScope.
public sealed class DefaultFeatureUsageTrackingSessionCoverageTests
{
    /// <summary>Verifies the constructor populates a root session with the supplied feature name and no parent.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_SetsRootSessionProperties()
    {
        using var scope = new AppLocatorScope();
        var featureName = Guid.NewGuid().ToString();
        using var instance = new DefaultFeatureUsageTrackingSession(featureName);

        using (Assert.Multiple())
        {
            await Assert.That(instance.FeatureName).IsEqualTo(featureName);
            await Assert.That(instance.FeatureReference).IsNotEqualTo(Guid.Empty);
            await Assert.That(instance.ParentReference).IsEqualTo(Guid.Empty);
        }
    }

    /// <summary>Verifies the constructor throws when the feature name is null or whitespace.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ThrowsForInvalidFeatureName() =>
        await Assert.That(() => new DefaultFeatureUsageTrackingSession("  ")).Throws<ArgumentException>();

    /// <summary>Verifies that SubFeature returns a child session referencing the parent feature.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubFeature_ReturnsChildSessionWithParentReference()
    {
        using var scope = new AppLocatorScope();
        var featureName = Guid.NewGuid().ToString();
        using var instance = new DefaultFeatureUsageTrackingSession(featureName);

        var subFeatureName = Guid.NewGuid().ToString();
        using var subFeature = instance.SubFeature(subFeatureName);
        var typedSubFeature = subFeature as IFeatureUsageTrackingSession<Guid>;

        await Assert.That(typedSubFeature).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(typedSubFeature?.FeatureName).IsEqualTo(subFeatureName);
            await Assert.That(typedSubFeature?.FeatureReference).IsNotEqualTo(Guid.Empty);
            await Assert.That(typedSubFeature?.ParentReference).IsEqualTo(instance.FeatureReference);
        }
    }

    /// <summary>Verifies that OnException does not throw when reporting an exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task OnException_DoesNotThrow()
    {
        using var scope = new AppLocatorScope();
        using var instance = new DefaultFeatureUsageTrackingSession(Guid.NewGuid().ToString());

        await Assert.That(() => instance.OnException(new InvalidOperationException("boom"))).ThrowsNothing();
    }

    /// <summary>Verifies that Dispose does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DoesNotThrow()
    {
        using var scope = new AppLocatorScope();
        var instance = new DefaultFeatureUsageTrackingSession(Guid.NewGuid().ToString());

        await Assert.That(() => instance.Dispose()).ThrowsNothing();
    }
}
