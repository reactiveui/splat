// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;
using Splat.Common.Test;

namespace Splat.Ninject.Tests;

/// <summary>
/// Contains unit tests for the dependency resolver functionality using Ninject.
/// </summary>
[TestFixture]
public class DependencyResolverTests
{
    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Resolve_Views()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelOne>>().To<ViewOne>();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        Assert.That(viewOne, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(viewOne, Is.TypeOf<ViewOne>());
            Assert.That(viewTwo, Is.Not.Null);
        }

        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Should return null when no binding exists.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Return_Null()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));

        Assert.That(viewOne, Is.Null);
    }

    /// <summary>
    /// GetServices should return an empty collection when no bindings exist.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_GetServices_Should_Return_Empty_Collection()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetServices(typeof(IViewFor<ViewModelOne>));

        Assert.That(viewOne, Is.Empty);
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Resolve_Named_View()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        Assert.That(viewTwo, Is.Not.Null);
        Assert.That(viewTwo, Is.TypeOf<ViewTwo>());
    }

    /// <summary>
    /// Should resolve view models.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Resolve_View_Models()
    {
        var container = new StandardKernel();
        container.Bind<ViewModelOne>().ToSelf();
        container.Bind<ViewModelTwo>().ToSelf();
        container.UseNinjectDependencyResolver();

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vmOne, Is.Not.Null);
            Assert.That(vmTwo, Is.Not.Null);
        }
    }

    /// <summary>
    /// Should resolve screen.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Resolve_Screen()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        Assert.That(screen, Is.Not.Null);
        Assert.That(screen, Is.TypeOf<MockScreen>());
    }

    /// <summary>
    /// Should throw an exception if UnregisterCurrent is called. (Pending verification).
    /// </summary>
    [Test]
    [Ignore("Further testing required")]
    public void NinjectDependencyResolver_Should_Throw_If_UnregisterCurrent_Called()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        Assert.Throws<NotImplementedException>(() =>
            AppLocator.CurrentMutable.UnregisterCurrent(typeof(IScreen)));
    }

    /// <summary>
    /// Should unregister all.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_UnregisterAll()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        Assert.That(screen, Is.Not.Null);
        Assert.That(screen, Is.TypeOf<MockScreen>());

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        var result = AppLocator.Current.GetService<IScreen>();
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Should throw an exception if service registration callback is called.
    /// </summary>
    [Test]
    public void NinjectDependencyResolver_Should_Throw_If_ServiceRegistionCallback_Called()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        Assert.Throws<NotImplementedException>(() =>
            AppLocator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), _ => { }));
    }
}
