// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Builder;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="SplatLoggerModule"/> class.</summary>
public class SplatLoggerModuleCoverageTests
{
    /// <summary>Test that Configure registers an ILogger on an empty resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_On_Empty_Resolver_Registers_Logger()
    {
        var resolver = new ModernDependencyResolver();

        new SplatLoggerModule().Configure(resolver);

        await Assert.That(resolver.HasRegistration<ILogger>()).IsTrue();
    }

    /// <summary>Test that Configure registers a DebugLogger on an empty resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_On_Empty_Resolver_Registers_DebugLogger()
    {
        var resolver = new ModernDependencyResolver();

        new SplatLoggerModule().Configure(resolver);

        await Assert.That(resolver.GetService<ILogger>()).IsTypeOf<DebugLogger>();
    }

    /// <summary>Test that Configure does not replace an already registered ILogger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Does_Not_Replace_Existing_Logger()
    {
        var resolver = new ModernDependencyResolver();
        var existing = new TextLogger();
        resolver.Register<ILogger>(() => existing);

        new SplatLoggerModule().Configure(resolver);

        await Assert.That(resolver.GetService<ILogger>()).IsSameReferenceAs(existing);
    }

    /// <summary>Test that Configure throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_With_Null_Resolver_Throws()
        => await Assert.That(() => new SplatLoggerModule().Configure(null!)).Throws<ArgumentNullException>();
}
