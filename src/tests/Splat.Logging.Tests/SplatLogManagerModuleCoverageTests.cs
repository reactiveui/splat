// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Builder;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="SplatLogManagerModule"/> class.</summary>
public class SplatLogManagerModuleCoverageTests
{
    /// <summary>Test that Configure registers an ILogManager on an empty resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_On_Empty_Resolver_Registers_LogManager()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register<ILogger>(() => new TextLogger());

        new SplatLogManagerModule(resolver).Configure(resolver);

        await Assert.That(resolver.HasRegistration<ILogManager>()).IsTrue();
    }

    /// <summary>Test that Configure registers a DefaultLogManager on an empty resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_On_Empty_Resolver_Registers_DefaultLogManager()
    {
        var resolver = new ModernDependencyResolver();
        resolver.Register<ILogger>(() => new TextLogger());

        new SplatLogManagerModule(resolver).Configure(resolver);

        await Assert.That(resolver.GetService<ILogManager>()).IsTypeOf<DefaultLogManager>();
    }

    /// <summary>Test that Configure does not replace an already registered ILogManager.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Does_Not_Replace_Existing_LogManager()
    {
        var resolver = new ModernDependencyResolver();
        var existing = new FuncLogManager(_ => new WrappingFullLogger(new TextLogger()));
        resolver.Register<ILogManager>(() => existing);

        new SplatLogManagerModule(resolver).Configure(resolver);

        await Assert.That(resolver.GetService<ILogManager>()).IsSameReferenceAs(existing);
    }

    /// <summary>Test that Configure throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_With_Null_Resolver_Throws()
    {
        var current = new ModernDependencyResolver();

        await Assert.That(() => new SplatLogManagerModule(current).Configure(null!)).Throws<ArgumentNullException>();
    }
}
