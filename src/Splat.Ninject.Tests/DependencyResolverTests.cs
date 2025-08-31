// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;

using Ninject;

using Splat.Common.Test;

namespace Splat.Ninject.Tests;

/// <summary>
/// Tests to show the <see cref="NinjectDependencyResolver"/> works correctly.
/// </summary>
public class DependencyResolverTests
{
    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Resolve_Views()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelOne>>().To<ViewOne>();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        viewOne.Should().NotBeNull();
        viewOne.Should().BeOfType<ViewOne>();
        viewTwo.Should().NotBeNull();
        viewTwo.Should().BeOfType<ViewTwo>();
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Return_Null()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));

        viewOne.Should().BeNull();
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_GetServices_Should_Return_Empty_Collection()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetServices(typeof(IViewFor<ViewModelOne>));

        viewOne.Should().BeEmpty();
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Resolve_Named_View()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        viewTwo.Should().NotBeNull();
        viewTwo.Should().BeOfType<ViewTwo>();
    }

    /// <summary>
    /// Should resolve view models.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Resolve_View_Models()
    {
        var container = new StandardKernel();
        container.Bind<ViewModelOne>().ToSelf();
        container.Bind<ViewModelTwo>().ToSelf();
        container.UseNinjectDependencyResolver();

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        vmOne.Should().NotBeNull();
        vmTwo.Should().NotBeNull();
    }

    /// <summary>
    /// Should resolve screen.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Resolve_Screen()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        screen.Should().NotBeNull();
        screen.Should().BeOfType<MockScreen>();
    }

    /// <summary>
    /// Should throw an exception if service registration call back called.
    /// </summary>
    [Fact(Skip = "Further testing required")]
    public void NinjectDependencyResolver_Should_Throw_If_UnregisterCurrent_Called()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        Action result = () =>
            AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

        result.Should().Throw<NotImplementedException>();
    }

    /// <summary>
    /// Should unregister all.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_UnregisterAll()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        screen.Should().NotBeNull();
        screen.Should().BeOfType<MockScreen>();

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        var result = AppLocator.Current.GetService<IScreen>();
        result.Should().BeNull();
    }

    /// <summary>
    /// Should throw an exception if service registration call back called.
    /// </summary>
    [Fact]
    public void NinjectDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var result = Record.Exception(() =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

        result.Should().BeOfType<NotImplementedException>();
    }
}
