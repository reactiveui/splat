// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.NLog;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the NLog <see cref="MutableDependencyResolverExtensions"/> registration helpers.</summary>
public class MutableDependencyResolverExtensionsCoverageTests
{
    /// <summary>Verifies that calling the extension registers an <see cref="ILogManager"/> on the resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseNLogWithWrappingFullLogger_Registers_ILogManager()
    {
        var resolver = new ModernDependencyResolver();

        resolver.UseNLogWithWrappingFullLogger();

        var logManager = resolver.GetService(typeof(ILogManager));

        await Assert.That(logManager).IsNotNull();
    }

    /// <summary>Verifies that the registered <see cref="ILogManager"/> yields a working logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseNLogWithWrappingFullLogger_Resolves_Working_Logger()
    {
        var resolver = new ModernDependencyResolver();

        resolver.UseNLogWithWrappingFullLogger();

        var logManager = (ILogManager)resolver.GetService(typeof(ILogManager))!;
        var logger = logManager.GetLogger(typeof(MutableDependencyResolverExtensionsCoverageTests));

        await Assert.That(logger).IsNotNull();
        await Assert.That(logger).IsTypeOf<NLogLogger>();
    }

    /// <summary>Verifies that the extension throws when invoked on a null resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseNLogWithWrappingFullLogger_Null_Resolver_Throws()
    {
        IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.UseNLogWithWrappingFullLogger()).Throws<ArgumentNullException>();
    }
}
