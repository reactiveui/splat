// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;

using Splat.Builder;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="ExceptionlessSplatModule"/>.</summary>
public sealed class ExceptionlessSplatModuleCoverageTests
{
    /// <summary>Verifies the constructor throws when the Exceptionless client is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Should_Throw_When_Client_Is_Null() =>
        await Assert.That(() => new ExceptionlessSplatModule(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies <see cref="ExceptionlessSplatModule.Configure(IMutableDependencyResolver)"/> registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Should_Register_LogManager_On_Fresh_Resolver()
    {
        var resolver = new ModernDependencyResolver();
        var client = CreateClient();
        var module = new ExceptionlessSplatModule(client);

        module.Configure(resolver);

        var logManager = resolver.GetService<ILogManager>();
        await Assert.That(logManager).IsNotNull();
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
