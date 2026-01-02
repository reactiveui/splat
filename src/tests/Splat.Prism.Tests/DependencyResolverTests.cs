// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Prism.Tests;

[NotInParallel]
public class DependencyResolverTests
{
    /// <summary>
    /// Tracks CreateScope not being implemented in case it's changed in future.
    /// </summary>
    [Test]
    public void CreateScope_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        Assert.Throws<NotSupportedException>(() => container.CreateScope());
    }

    /// <summary>
    /// Tracks RegisterScoped not being implemented in case it's changed in future.
    /// </summary>
    [Test]
    public void RegisterScoped_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        Assert.Throws<NotSupportedException>(() => container.RegisterScoped(
            typeof(IViewFor<ViewModelOne>),
            () => new ViewOne()));
    }

    /// <summary>
    /// Tracks RegisterManySingleton not being implemented in case it's changed in future.
    /// </summary>
    [Test]
    public void RegisterManySingleton_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        Assert.Throws<NotSupportedException>(() => container.RegisterManySingleton(
            typeof(IViewFor<ViewModelOne>),
            typeof(ViewOne)));
    }

    /// <summary>
    /// Test to ensure register many succeeds.
    /// </summary>
    [Test]
    public void RegisterMany_Succeeds()
    {
        using var container = new SplatContainerExtension();

        container.RegisterMany(
            typeof(IViewFor<ViewModelOne>),
            typeof(ViewOne));
    }

    /// <summary>
    /// Test to ensure a simple resolve succeeds.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Resolve_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>));

        await Assert.That(instance).IsNotNull();
    }

    /// <summary>
    /// Test to ensure a resolve with name succeeds.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Resolve_With_Name_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>), "name");

        await Assert.That(instance).IsNotNull();
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsRegistered_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var isRegistered = container.IsRegistered(typeof(IViewFor<ViewModelOne>));

        await Assert.That(isRegistered).IsTrue();
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsRegistered_With_Name_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var isRegistered = container.IsRegistered(typeof(IViewFor<ViewModelOne>), "name");

        await Assert.That(isRegistered).IsTrue();
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_Resolve_Views()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo));

        var viewOne = AppLocator.Current.GetService<IViewFor<ViewModelOne>>();
        var viewTwo = AppLocator.Current.GetService<IViewFor<ViewModelTwo>>();

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_Resolve_Named_View()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo), "Other");

        var viewTwo = AppLocator.Current.GetService<IViewFor<ViewModelTwo>>("Other");

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve the view models.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_Resolve_View_Models()
    {
        using var container = new SplatContainerExtension();

        container.Register(typeof(ViewModelOne), typeof(ViewModelOne));
        container.Register(typeof(ViewModelTwo), typeof(ViewModelTwo));

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.Multiple())
        {
            await Assert.That(vmOne).IsNotNull();
            await Assert.That(vmTwo).IsNotNull();
        }
    }

    /// <summary>
    /// Should resolve the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_Resolve_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_UnregisterCurrent_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();

        AppLocator.CurrentMutable.UnregisterCurrent<IScreen>();

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNotNull();

        AppLocator.CurrentMutable.UnregisterCurrent<IScreen>(nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_UnregisterAll_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();

        AppLocator.CurrentMutable.UnregisterAll<IScreen>();

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNotNull();

        AppLocator.CurrentMutable.UnregisterAll<IScreen>(nameof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>(nameof(MockScreen))).IsNull();
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.Register(typeof(ILogger), typeof(ConsoleLogger));
        AppLocator.CurrentMutable.RegisterConstant<ILogManager>(
            new FuncLogManager(_ => new WrappingFullLogger(new ConsoleLogger())));

        var d = AppLocator.Current.GetService<ILogManager>();

        await Assert.That(d).IsTypeOf<FuncLogManager>();
    }

    /// <summary>
    /// Test that a pre-init logger isn't overridden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PrismDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.RegisterInstance(
            typeof(ILogManager),
            new FuncLogManager(_ => new WrappingFullLogger(new ConsoleLogger())));

        var d = AppLocator.Current.GetService<ILogManager>();

        await Assert.That(d).IsTypeOf<FuncLogManager>();
    }
}
