// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="ExceptionlessFeatureUsageTrackingSession"/> branches not covered by the inherited base tests.</summary>
[NotInParallel]
public sealed class ExceptionlessFeatureUsageTrackingSessionCoverageTests
{
    /// <summary>Verifies the constructor throws when the feature name is null or whitespace.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Should_Throw_When_FeatureName_Is_Whitespace()
    {
        ConfigureDefaultClient();
        await Assert.That(() => new ExceptionlessFeatureUsageTrackingSession(" ")).Throws<ArgumentException>();
    }

    /// <summary>Verifies <see cref="ExceptionlessFeatureUsageTrackingSession.SubFeature(string)"/> creates a child session referencing its parent.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubFeature_Should_Create_Child_Session()
    {
        ConfigureDefaultClient();
        var session = new ExceptionlessFeatureUsageTrackingSession("parent");

        var subFeature = session.SubFeature("child");

        await Assert.That(subFeature).IsNotNull();
        await Assert.That(subFeature.FeatureName).IsEqualTo("child");
    }

    /// <summary>Verifies <see cref="ExceptionlessFeatureUsageTrackingSession.OnException(Exception)"/> does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task OnException_Should_Not_Throw()
    {
        ConfigureDefaultClient();
        var session = new ExceptionlessFeatureUsageTrackingSession("feature");

        await Assert.That(() => session.OnException(new InvalidOperationException("boom"))).ThrowsNothing();
    }

    /// <summary>Verifies <see cref="ExceptionlessFeatureUsageTrackingSession.Dispose"/> does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_Should_Not_Throw()
    {
        ConfigureDefaultClient();
        var session = new ExceptionlessFeatureUsageTrackingSession("feature");

        await Assert.That(() => session.Dispose()).ThrowsNothing();
    }

    /// <summary>Configures the default Exceptionless client so events are cancelled and never submitted to the network.</summary>
    private static void ConfigureDefaultClient()
    {
        var configuration = ExceptionlessClient.Default.Configuration;
        configuration.ApiKey = "someapikey";
        configuration.RemovePlugin("cancel-default");
        configuration.AddPlugin("cancel-default", context => context.Cancel = true);
    }
}

#endif
