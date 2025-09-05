// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NUnit.Framework;

using Splat.Common.Test;

namespace Splat.Prism.Tests;

/// <summary>
/// Tests to show the <see cref="PrismDependencyResolver"/> works correctly.
/// </summary>
[TestFixture]
[NonParallelizable]
public class DependencyResolverTests
{
    /// <summary>
    /// Tracks CreateScope not being implemented in case it's changed in future.
    /// </summary>
    [Test]
    public void CreateScope_Throws_NotImplementedException()
    {
        using var container = new SplatContainerExtension();
        Assert.Throws<NotSupportedException>(() => container.CreateScope());
    }

    /// <summary>
    /// Tracks RegisterScoped not being implemented in case it's changed in future.
    /// </summary>
    [Test]
    public void RegisterScoped_Throws_NotImplementedException()
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
    public void RegisterManySingleton_Throws_NotImplementedException()
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
    [Test]
    public void Resolve_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>));

        Assert.That(instance, Is.Not.Null);
    }

    /// <summary>
    /// Test to ensure a resolve with name succeeds.
    /// </summary>
    [Test]
    public void Resolve_With_Name_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>), "name");

        Assert.That(instance, Is.Not.Null);
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    [Test]
    public void IsRegistered_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var instance = container.IsRegistered(typeof(IViewFor<ViewModelOne>));

        Assert.That(instance, Is.True);
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    [Test]
    public void IsRegistered_With_Name_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var instance = container.IsRegistered(typeof(IViewFor<ViewModelOne>), "name");

        Assert.That(instance, Is.True);
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_Resolve_Views()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo));

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        Assert.Multiple(() =>
        {
            Assert.That(viewOne, Is.Not.Null);
            Assert.That(viewOne, Is.TypeOf<ViewOne>());
            Assert.That(viewTwo, Is.Not.Null);
            Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
        });
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_Resolve_Named_View()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo), "Other");

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        Assert.Multiple(() =>
        {
            Assert.That(viewTwo, Is.Not.Null);
            Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
        });
    }

    /// <summary>
    /// Should resolve the view models.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_Resolve_View_Models()
    {
        using var container = new SplatContainerExtension();

        container.Register(typeof(ViewModelOne), typeof(ViewModelOne));
        container.Register(typeof(ViewModelTwo), typeof(ViewModelTwo));

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        Assert.Multiple(() =>
        {
            Assert.That(vmOne, Is.Not.Null);
            Assert.That(vmTwo, Is.Not.Null);
        });
    }

    /// <summary>
    /// Should resolve the screen.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_Resolve_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        var screen = AppLocator.Current.GetService<IScreen>();

        Assert.Multiple(() =>
        {
            Assert.That(screen, Is.Not.Null);
            Assert.That(screen, Is.TypeOf<MockScreen>());
        });
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_UnregisterCurrent_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_UnregisterAll_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(), Is.Null);
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Test]
    public void PrismDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Not.Null);

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

        AppLocator.Current.Assert.That(GetService<IScreen>(nameof(MockScreen)), Is.Null);
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void PrismDependencyResolver_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.Register(typeof(ILogger), typeof(ConsoleLogger));
        AppLocator.CurrentMutable.RegisterConstant<ILogManager>(new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

        var d = Splat.AppLocator.Current.GetService<ILogManager>();
        Assert.That(d, Is.TypeOf<FuncLogManager>());
    }

    /// <summary>
    /// Test that a pre-init logger isn't overriden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Test]
    public void PrismDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.RegisterInstance(typeof(ILogManager), new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

        var d = Splat.AppLocator.Current.GetService<ILogManager>();
        Assert.That(d, Is.TypeOf<FuncLogManager>());
    }
}
