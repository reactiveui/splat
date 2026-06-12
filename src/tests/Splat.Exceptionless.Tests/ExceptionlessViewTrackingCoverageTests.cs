// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="ExceptionlessViewTracking"/>.</summary>
public sealed class ExceptionlessViewTrackingCoverageTests
{
    /// <summary>Verifies the constructor throws when the Exceptionless client is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Should_Throw_When_Client_Is_Null() =>
        await Assert.That(() => new ExceptionlessViewTracking(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies the constructor succeeds with a valid Exceptionless client.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Should_Create_Instance_With_Valid_Client()
    {
        var client = CreateClient();
        var tracking = new ExceptionlessViewTracking(client);

        await Assert.That(tracking).IsNotNull();
    }

    /// <summary>Verifies <see cref="ExceptionlessViewTracking.OnViewNavigation(string)"/> does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task OnViewNavigation_Should_Not_Throw()
    {
        var client = CreateClient();
        var tracking = new ExceptionlessViewTracking(client);

        await Assert.That(() => tracking.OnViewNavigation("TestView")).ThrowsNothing();
    }

    /// <summary>Creates a configured Exceptionless client that does not submit events to the network.</summary>
    /// <returns>The configured <see cref="ExceptionlessClient"/>.</returns>
    private static ExceptionlessClient CreateClient()
    {
        var client = new ExceptionlessClient();
        client.Configuration.ApiKey = "someapikey";
        client.Configuration.AddPlugin("cancel", context => context.Cancel = true);
        return client;
    }
}

#endif
