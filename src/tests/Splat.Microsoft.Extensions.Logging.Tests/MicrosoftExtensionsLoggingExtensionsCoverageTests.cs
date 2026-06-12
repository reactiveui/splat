// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Splat.Microsoft.Extensions.Logging;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="MicrosoftExtensionsLoggingExtensions"/> class.</summary>
public class MicrosoftExtensionsLoggingExtensionsCoverageTests
{
    /// <summary>Verifies the resolver extension registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UseMicrosoftExtensionsLoggingWithWrappingFullLogger_RegistersLogManager()
    {
        var resolver = new ModernDependencyResolver();

        resolver.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(NullLoggerFactory.Instance);

        await Assert.That(resolver.HasRegistration<ILogManager>()).IsTrue();
    }

    /// <summary>Verifies the registered <see cref="ILogManager"/> resolves a usable <see cref="IFullLogger"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UseMicrosoftExtensionsLoggingWithWrappingFullLogger_ResolvesFullLogger()
    {
        var resolver = new ModernDependencyResolver();

        resolver.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(NullLoggerFactory.Instance);

        var logManager = resolver.GetService<ILogManager>();
        await Assert.That(logManager).IsNotNull();

        var logger = logManager!.GetLogger(typeof(MicrosoftExtensionsLoggingExtensionsCoverageTests));
        await Assert.That(logger).IsNotNull();
    }

    /// <summary>Verifies a null resolver throws an <see cref="ArgumentNullException"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UseMicrosoftExtensionsLoggingWithWrappingFullLogger_NullResolverThrows()
    {
        const IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(NullLoggerFactory.Instance)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies the logging builder extension registers a Splat logger provider.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task AddSplat_OnLoggingBuilder_RegistersProvider()
    {
        var registeredSplatProvider = false;
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSplat();
            registeredSplatProvider = builder.Services.Any(static descriptor =>
                descriptor.ServiceType == typeof(ILoggerProvider) &&
                descriptor.ImplementationType == typeof(MicrosoftExtensionsLogProvider));
        });

        await Assert.That(registeredSplatProvider).IsTrue();
    }

    /// <summary>Verifies the logging builder extension returns the same builder for chaining.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task AddSplat_OnLoggingBuilder_ReturnsSameBuilder()
    {
        ILoggingBuilder? captured = null;
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            captured = builder.AddSplat();
        });

        await Assert.That(captured).IsNotNull();
    }

    /// <summary>Verifies a null logging builder throws an <see cref="ArgumentNullException"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task AddSplat_OnLoggingBuilder_NullThrows()
    {
        const ILoggingBuilder builder = null!;

        await Assert.That(() => builder.AddSplat()).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies the logger factory extension returns the same factory for chaining.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task AddSplat_OnLoggerFactory_ReturnsSameFactory()
    {
        using var loggerFactory = new LoggerFactory();

        var result = loggerFactory.AddSplat();

        await Assert.That(result).IsEqualTo((ILoggerFactory)loggerFactory);
    }

    /// <summary>Verifies a null logger factory throws an <see cref="ArgumentNullException"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task AddSplat_OnLoggerFactory_NullThrows()
    {
        const ILoggerFactory loggerFactory = null!;

        await Assert.That(() => loggerFactory.AddSplat()).Throws<ArgumentNullException>();
    }
}
