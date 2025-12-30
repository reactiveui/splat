// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Builder.Tests;

[NotInParallel]
public class SplatBuilderExtensionsTests
{
    private AppBuilderScope? _appBuilderScope;

    /// <summary>
    /// Setup method to initialize AppBuilderScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpAppBuilderScope() => _appBuilderScope = new AppBuilderScope();

    /// <summary>
    /// Teardown method to dispose AppBuilderScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownAppBuilderScope()
    {
        _appBuilderScope?.Dispose();
        _appBuilderScope = null;
    }

    /// <summary>
    /// Applies the throws on null module.
    /// </summary>
    [Test]
    public void ApplyThrowsOnNullModule()
    {
        IModule module = null!;
        Assert.Throws<ArgumentNullException>(() => module.Apply());
    }

    /// <summary>
    /// Creates the splat builder throws on null resolver.
    /// </summary>
    [Test]
    public void CreateSplatBuilderThrowsOnNullResolver()
    {
        IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
        Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder());
    }

    /// <summary>
    /// Creates the splat builder returns application builder.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderReturnsAppBuilder()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder();
        await Assert.That(builder).IsNotNull();
        resolver.Dispose();
    }

    /// <summary>
    /// Creates the splat builder with configure action throws on null resolver.
    /// </summary>
    [Test]
    public void CreateSplatBuilderWithConfigureActionThrowsOnNullResolver()
    {
        IMutableDependencyResolver resolver = (IMutableDependencyResolver)null!;
        Assert.Throws<ArgumentNullException>(() => resolver.CreateSplatBuilder(_ => { }));
    }

    /// <summary>
    /// Creates the splat builder with configure action returns application builder.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderWithConfigureActionReturnsAppBuilder()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder(r => r.Register<string>(() => "Hello"))
            .Build();
        await Assert.That(builder).IsNotNull();
        var hello = resolver.Current.GetService<string>();
        await Assert.That(hello).IsEqualTo("Hello");
        resolver.Dispose();
    }

    /// <summary>
    /// Creates the splat builder with configure action returns application builder.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateSplatBuilderWithConfigureActionReturnsAppBuilderNonGeneric()
    {
        var resolver = new InternalLocator();
        var builder = resolver.CurrentMutable.CreateSplatBuilder(r => r.Register(() => "Hello", typeof(string)))
            .Build();
        await Assert.That(builder).IsNotNull();
        var hello = resolver.Current.GetService<string>();
        await Assert.That(hello).IsEqualTo("Hello");
        resolver.Dispose();
    }
}
