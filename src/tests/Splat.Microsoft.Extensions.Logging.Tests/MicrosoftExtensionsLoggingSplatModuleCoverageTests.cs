// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging.Abstractions;

using Splat.Builder;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="MicrosoftExtensionsLoggingSplatModule"/> class.</summary>
public class MicrosoftExtensionsLoggingSplatModuleCoverageTests
{
    /// <summary>Verifies Configure registers an <see cref="ILogManager"/> on a fresh resolver.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Configure_RegistersLogManager()
    {
        var resolver = new ModernDependencyResolver();
        var module = new MicrosoftExtensionsLoggingSplatModule(NullLoggerFactory.Instance);

        module.Configure(resolver);

        await Assert.That(resolver.HasRegistration<ILogManager>()).IsTrue();
    }

    /// <summary>Verifies the registered <see cref="ILogManager"/> resolves and produces a logger.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Configure_RegistersResolvableLogManager()
    {
        var resolver = new ModernDependencyResolver();
        var module = new MicrosoftExtensionsLoggingSplatModule(NullLoggerFactory.Instance);

        module.Configure(resolver);

        var logManager = resolver.GetService<ILogManager>();
        await Assert.That(logManager).IsNotNull();
        await Assert.That(logManager!.GetLogger(typeof(MicrosoftExtensionsLoggingSplatModuleCoverageTests))).IsNotNull();
    }

    /// <summary>Verifies Configure can be invoked repeatedly without throwing.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Configure_IsIdempotent()
    {
        var resolver = new ModernDependencyResolver();
        var module = new MicrosoftExtensionsLoggingSplatModule(NullLoggerFactory.Instance);

        module.Configure(resolver);
        module.Configure(resolver);

        await Assert.That(resolver.HasRegistration<ILogManager>()).IsTrue();
    }

    /// <summary>Verifies a null resolver throws an <see cref="ArgumentNullException"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Configure_NullResolverThrows()
    {
        var module = new MicrosoftExtensionsLoggingSplatModule(NullLoggerFactory.Instance);

        await Assert.That(() => module.Configure(null!)).Throws<ArgumentNullException>();
    }
}
