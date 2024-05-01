// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;

using Splat.Common.Test;

namespace Splat.Prism.Tests;

/// <summary>
/// Tests to show the <see cref="PrismDependencyResolver"/> works correctly.
/// </summary>
public class DependencyResolverTests
{
    /// <summary>
    /// Tracks CreateScope not being implemented in case it's changed in future.
    /// </summary>
    [Fact]
    public void CreateScope_Throws_NotImplementedException()
    {
        using var container = new SplatContainerExtension();
        Assert.Throws<NotSupportedException>(() => container.CreateScope());
    }

    /// <summary>
    /// Tracks RegisterScoped not being implemented in case it's changed in future.
    /// </summary>
    [Fact]
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
    [Fact]
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
    [Fact]
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
    [Fact]
    public void Resolve_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>));

        Assert.NotNull(instance);
    }

    /// <summary>
    /// Test to ensure a resolve with name succeeds.
    /// </summary>
    [Fact]
    public void Resolve_With_Name_Succeeds()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var instance = container.Resolve(typeof(IViewFor<ViewModelOne>), "name");

        Assert.NotNull(instance);
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    [Fact]
    public void IsRegistered_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));

        var instance = container.IsRegistered(typeof(IViewFor<ViewModelOne>));

        Assert.True(instance);
    }

    /// <summary>
    /// Test to ensure a simple is registered check succeeds.
    /// </summary>
    [Fact]
    public void IsRegistered_With_Name_Returns_True()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne), "name");

        var instance = container.IsRegistered(typeof(IViewFor<ViewModelOne>), "name");

        Assert.True(instance);
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_Resolve_Views()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo));

        var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        viewOne.Should().NotBeNull();
        viewOne.Should().BeOfType<ViewOne>();
        viewTwo.Should().NotBeNull();
        viewTwo.Should().BeOfType<ViewTwo>();
    }

    /// <summary>
    /// Should resolve the views.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_Resolve_Named_View()
    {
        using var container = new SplatContainerExtension();
        container.Register(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo), "Other");

        var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

        viewTwo.Should().NotBeNull();
        viewTwo.Should().BeOfType<ViewTwo>();
    }

    /// <summary>
    /// Should resolve the view models.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_Resolve_View_Models()
    {
        using var container = new SplatContainerExtension();

        container.Register(typeof(ViewModelOne), typeof(ViewModelOne));
        container.Register(typeof(ViewModelTwo), typeof(ViewModelTwo));

        var vmOne = Locator.Current.GetService<ViewModelOne>();
        var vmTwo = Locator.Current.GetService<ViewModelTwo>();

        vmOne.Should().NotBeNull();
        vmTwo.Should().NotBeNull();
    }

    /// <summary>
    /// Should resolve the screen.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_Resolve_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        var screen = Locator.Current.GetService<IScreen>();

        screen.Should().NotBeNull();
        screen.Should().BeOfType<MockScreen>();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_UnregisterCurrent_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        Locator.Current.GetService<IScreen>().Should().NotBeNull();

        Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

        Locator.Current.GetService<IScreen>().Should().BeNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_UnregisterCurrent_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        Locator.Current.GetService<IScreen>(nameof(MockScreen)).Should().NotBeNull();

        Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen), nameof(MockScreen));

        Locator.Current.GetService<IScreen>(nameof(MockScreen)).Should().BeNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_UnregisterAll_Screen()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen));

        Locator.Current.GetService<IScreen>().Should().NotBeNull();

        Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

        Locator.Current.GetService<IScreen>().Should().BeNull();
    }

    /// <summary>
    /// Should unregister the screen.
    /// </summary>
    [Fact]
    public void PrismDependencyResolver_Should_UnregisterAll_Screen_With_Contract()
    {
        using var builder = new SplatContainerExtension();
        builder.RegisterSingleton(typeof(IScreen), typeof(MockScreen), nameof(MockScreen));

        Locator.Current.GetService<IScreen>(nameof(MockScreen)).Should().NotBeNull();

        Locator.CurrentMutable.UnregisterAll(typeof(IScreen), nameof(MockScreen));

        Locator.Current.GetService<IScreen>(nameof(MockScreen)).Should().BeNull();
    }

    /// <summary>
    /// Check to ensure the correct logger is returned.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Fact]
    public void PrismDependencyResolver_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.Register(typeof(ILogger), typeof(ConsoleLogger));
        Locator.CurrentMutable.RegisterConstant(
            new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())),
            typeof(ILogManager));

        var d = Splat.Locator.Current.GetService<ILogManager>();
        Assert.IsType<FuncLogManager>(d);
    }

    /// <summary>
    /// Test that a pre-init logger isn't overriden.
    /// </summary>
    /// <remarks>
    /// Introduced for Splat #331.
    /// </remarks>
    [Fact]
    public void PrismDependencyResolver_PreInit_Should_ReturnRegisteredLogger()
    {
        using var c = new SplatContainerExtension();
        c.RegisterInstance(typeof(ILogManager), new FuncLogManager(type => new WrappingFullLogger(new ConsoleLogger())));

        var d = Splat.Locator.Current.GetService<ILogManager>();
        Assert.IsType<FuncLogManager>(d);
    }
}
