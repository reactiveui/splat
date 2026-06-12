// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;
using Splat.Common.Test;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="FuncFeatureUsageTrackingManager"/>.</summary>
[NotInParallel] // The factory builds a DefaultFeatureUsageTrackingSession, which resolves ILogManager from the global locator.
public sealed class FuncFeatureUsageTrackingManagerCoverageTests
{
    /// <summary>Verifies the constructor throws when the supplied factory function is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ThrowsForNullFactory() =>
        await Assert.That(() => new FuncFeatureUsageTrackingManager(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies GetFeatureUsageTrackingSession returns the session created by the supplied factory function.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetFeatureUsageTrackingSession_ReturnsSessionFromFactory()
    {
        using var scope = new AppLocatorScope();
        var featureName = Guid.NewGuid().ToString();
        string? capturedName = null;
        var manager = new FuncFeatureUsageTrackingManager(name =>
        {
            capturedName = name;
            return new DefaultFeatureUsageTrackingSession(name);
        });

        using var session = manager.GetFeatureUsageTrackingSession(featureName);

        await Assert.That(session).IsNotNull();
        await Assert.That(capturedName).IsEqualTo(featureName);
        await Assert.That(session.FeatureName).IsEqualTo(featureName);
    }
}
