// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;

using Splat.Exceptionless;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Coverage tests for <see cref="MutableDependencyResolverExtensions"/>.</summary>
public sealed class MutableDependencyResolverExtensionsCoverageTests
{
    /// <summary>Verifies that the extension registers an <see cref="ILogManager"/> on a fresh resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseExceptionlessWithWrappingFullLogger_Should_Register_LogManager()
    {
        using var resolver = new ModernDependencyResolver();
        var client = CreateClient();

        resolver.UseExceptionlessWithWrappingFullLogger(client);

        var logManager = resolver.GetService<ILogManager>();
        await Assert.That(logManager).IsNotNull();
    }

    /// <summary>Verifies the registered log manager produces a logger for the requested type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseExceptionlessWithWrappingFullLogger_Registered_LogManager_Should_Provide_Logger()
    {
        using var resolver = new ModernDependencyResolver();
        var client = CreateClient();

        resolver.UseExceptionlessWithWrappingFullLogger(client);

        var logManager = resolver.GetService<ILogManager>();
        var logger = logManager!.GetLogger(typeof(MutableDependencyResolverExtensionsCoverageTests));
        await Assert.That(logger).IsNotNull();
    }

    /// <summary>Creates a configured Exceptionless client that does not submit events to the network.</summary>
    /// <returns>The configured <see cref="ExceptionlessClient"/>.</returns>
    private static ExceptionlessClient CreateClient()
    {
        var client = new ExceptionlessClient();
        client.Configuration.ApiKey = "someapikey";
        client.Configuration.AddPlugin("cancel", static context => context.Cancel = true);
        return client;
    }
}

#endif
