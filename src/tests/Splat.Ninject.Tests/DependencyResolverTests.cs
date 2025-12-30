// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

using Splat.Common.Test;

namespace Splat.Ninject.Tests;

[NotInParallel]
public class DependencyResolverTests
{
/// <summary>
    /// Should resolve views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_Resolve_Views()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelOne>>().To<ViewOne>();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));
        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        await Assert.That(viewOne).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(viewOne).IsTypeOf<ViewOne>();
            await Assert.That(viewTwo).IsNotNull();
        }

        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should return null when no binding exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_Return_Null()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));

        await Assert.That(viewOne).IsNull();
    }

    /// <summary>
    /// GetServices should return an empty collection when no bindings exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_GetServices_Should_Return_Empty_Collection()
    {
        var container = new StandardKernel();
        container.UseNinjectDependencyResolver();

        var viewOne = AppLocator.Current.GetServices(typeof(IViewFor<ViewModelOne>));

        await Assert.That(viewOne).IsEmpty();
    }

    /// <summary>
    /// Should resolve views.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_Resolve_Named_View()
    {
        var container = new StandardKernel();
        container.Bind<IViewFor<ViewModelTwo>>().To<ViewTwo>();
        container.UseNinjectDependencyResolver();

        var viewTwo = AppLocator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

        await Assert.That(viewTwo).IsNotNull();
        await Assert.That(viewTwo).IsTypeOf<ViewTwo>();
    }

    /// <summary>
    /// Should resolve view models.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_Resolve_View_Models()
    {
        var container = new StandardKernel();
        container.Bind<ViewModelOne>().ToSelf();
        container.Bind<ViewModelTwo>().ToSelf();
        container.UseNinjectDependencyResolver();

        var vmOne = AppLocator.Current.GetService<ViewModelOne>();
        var vmTwo = AppLocator.Current.GetService<ViewModelTwo>();

        using (Assert.Multiple())
        {
            await Assert.That(vmOne).IsNotNull();
            await Assert.That(vmTwo).IsNotNull();
        }
    }

    /// <summary>
    /// Should resolve screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_Resolve_Screen()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>
    /// Should throw an exception if UnregisterCurrent is called. (Pending verification).
    /// </summary>
    [Test]
    [Skip("Further testing required")]
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NinjectDependencyResolver_Should_UnregisterAll()
    {
        var container = new StandardKernel();
        container.Bind<IScreen>().ToConstant(new MockScreen());
        container.UseNinjectDependencyResolver();

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();

        AppLocator.CurrentMutable.UnregisterAll(typeof(IScreen));

        var result = AppLocator.Current.GetService<IScreen>();
        await Assert.That(result).IsNull();
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
