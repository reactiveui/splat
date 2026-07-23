// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Builder.Tests;

/// <summary>Unit tests for the <see cref="SplatBuilderExtensions"/> class.</summary>
[NotInParallel]
public class SplatBuilderExtensionsTests
{
    /// <summary>The sample string value registered and resolved during the builder tests.</summary>
    private const string HelloValue = "Hello";

    /// <summary>The scope that isolates the <see cref="AppBuilder"/> state for each test.</summary>
    private AppBuilderScope? _appBuilderScope;

    /// <summary>Setup method to initialize AppBuilderScope before each test.</summary>
    [Before(HookType.Test)]
    public void SetUpAppBuilderScope() => _appBuilderScope = new();

    /// <summary>Teardown method to dispose AppBuilderScope after each test.</summary>
    [After(HookType.Test)]
    public void TearDownAppBuilderScope()
    {
        _appBuilderScope?.Dispose();
        _appBuilderScope = null;
    }

    /// <summary>Applies the throws on null module.</summary>
    [Test]
    public void ApplyThrowsOnNullModule()
    {
        const IModule module = null!;
        _ = Assert.Throws<ArgumentNullException>(static () => module!.Apply());
    }

    /// <summary>Creates the splat builder throws on null resolver.</summary>
    [Test]
    public void CreateSplatBuilderThrowsOnNullResolver()
    {
        const IMutableDependencyResolver resolver = null!;
        _ = Assert.Throws<ArgumentNullException>(static () => resolver!.CreateSplatBuilder());
    }

    /// <summary>Creates the splat builder returns application builder.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderReturnsAppBuilder()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder();
        await Assert.That(builder).IsNotNull();
        resolver.Dispose();
    }

    /// <summary>Creates the splat builder with configure action throws on null resolver.</summary>
    [Test]
    public void CreateSplatBuilderWithConfigureActionThrowsOnNullResolver()
    {
        const IMutableDependencyResolver resolver = null!;
        _ = Assert.Throws<ArgumentNullException>(static () => resolver!.CreateSplatBuilder(static _ => { }));
    }

    /// <summary>Creates the splat builder with configure action returns application builder.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderWithConfigureActionReturnsAppBuilder()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder(static r => r.Register(static () => HelloValue))
            .Build();
        await Assert.That(builder).IsNotNull();
        var hello = resolver.Current.GetService<string>();
        await Assert.That(hello).IsEqualTo(HelloValue);
        resolver.Dispose();
    }

    /// <summary>Creates the splat builder with configure action returns application builder.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderWithConfigureActionReturnsAppBuilderNonGeneric()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder(static r => r.Register(static () => HelloValue, typeof(string)))
            .Build();
        await Assert.That(builder).IsNotNull();
        var hello = resolver.Current.GetService<string>();
        await Assert.That(hello).IsEqualTo(HelloValue);
        resolver.Dispose();
    }
}
