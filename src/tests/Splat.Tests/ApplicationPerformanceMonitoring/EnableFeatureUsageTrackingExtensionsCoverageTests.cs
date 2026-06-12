// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;
using Splat.Common.Test;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="EnableFeatureUsageTrackingExtensions"/>.</summary>
[NotInParallel]
public sealed class EnableFeatureUsageTrackingExtensionsCoverageTests
{
    /// <summary>Verifies the default manager path returns a session with the requested feature name.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FeatureUsageTrackingSession_ReturnsSessionWithFeatureName()
    {
        using var scope = new AppLocatorScope();
        var fixture = new TrackingEnabledFixture();
        var featureName = Guid.NewGuid().ToString();

        using var session = fixture.FeatureUsageTrackingSession(featureName);

        await Assert.That(session).IsNotNull();
        await Assert.That(session.FeatureName).IsEqualTo(featureName);
    }

    /// <summary>Verifies that the default manager produces a <see cref="DefaultFeatureUsageTrackingSession"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FeatureUsageTrackingSession_ReturnsDefaultSessionType()
    {
        using var scope = new AppLocatorScope();
        var fixture = new TrackingEnabledFixture();

        using var session = fixture.FeatureUsageTrackingSession(Guid.NewGuid().ToString());

        await Assert.That(session).IsTypeOf<DefaultFeatureUsageTrackingSession>();
    }

    /// <summary>Verifies that the session throws when no manager is registered with the locator.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FeatureUsageTrackingSession_ThrowsWhenManagerMissing()
    {
        using var scope = new AppLocatorScope();
        AppLocator.CurrentMutable.UnregisterCurrent<IFeatureUsageTrackingManager>();
        var fixture = new TrackingEnabledFixture();

        await Assert.That(() => fixture.FeatureUsageTrackingSession("feature")).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that WithFeatureUsageTrackingSession invokes the supplied action.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithFeatureUsageTrackingSession_InvokesAction()
    {
        using var scope = new AppLocatorScope();
        var fixture = new TrackingEnabledFixture();
        var featureName = Guid.NewGuid().ToString();
        IFeatureUsageTrackingSession? captured = null;

        fixture.WithFeatureUsageTrackingSession(featureName, session => captured = session);

        await Assert.That(captured).IsNotNull();
        await Assert.That(captured?.FeatureName).IsEqualTo(featureName);
    }

    /// <summary>Verifies that WithFeatureUsageTrackingSession throws when the action is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithFeatureUsageTrackingSession_ThrowsForNullAction()
    {
        using var scope = new AppLocatorScope();
        var fixture = new TrackingEnabledFixture();

        await Assert.That(() => fixture.WithFeatureUsageTrackingSession("feature", null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that WithFeatureUsageTrackingSession reports exceptions and rethrows them.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithFeatureUsageTrackingSession_ReportsAndRethrows()
    {
        using var scope = new AppLocatorScope();
        var fixture = new TrackingEnabledFixture();

        await Assert.That(() => fixture.WithFeatureUsageTrackingSession(
            "feature",
            _ => throw new InvalidOperationException("boom"))).Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that WithSubFeatureUsageTrackingSession invokes the supplied action with a sub-feature session.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithSubFeatureUsageTrackingSession_InvokesActionWithSubFeature()
    {
        using var scope = new AppLocatorScope();
        using var session = new DefaultFeatureUsageTrackingSession(Guid.NewGuid().ToString());
        var subFeatureName = Guid.NewGuid().ToString();
        IFeatureUsageTrackingSession? captured = null;

        session.WithSubFeatureUsageTrackingSession(subFeatureName, sub => captured = sub);

        await Assert.That(captured).IsNotNull();
        await Assert.That(captured?.FeatureName).IsEqualTo(subFeatureName);
    }

    /// <summary>Verifies that WithSubFeatureUsageTrackingSession throws when the action is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithSubFeatureUsageTrackingSession_ThrowsForNullAction()
    {
        using var scope = new AppLocatorScope();
        using var session = new DefaultFeatureUsageTrackingSession(Guid.NewGuid().ToString());

        await Assert.That(() => session.WithSubFeatureUsageTrackingSession("sub", null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that WithSubFeatureUsageTrackingSession reports exceptions and rethrows them.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithSubFeatureUsageTrackingSession_ReportsAndRethrows()
    {
        using var scope = new AppLocatorScope();
        using var session = new DefaultFeatureUsageTrackingSession(Guid.NewGuid().ToString());

        await Assert.That(() => session.WithSubFeatureUsageTrackingSession(
            "sub",
            _ => throw new InvalidOperationException("boom"))).Throws<InvalidOperationException>();
    }

    /// <summary>A test type that opts in to feature usage tracking via the marker interface.</summary>
    private sealed class TrackingEnabledFixture : IEnableFeatureUsageTracking;
}
