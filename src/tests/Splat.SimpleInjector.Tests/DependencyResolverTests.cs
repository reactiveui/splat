// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

using Splat.Common.Test;
using Splat.SimpleInjector;

namespace Splat.Simplnjector;

[NotInParallel]
public class DependencyResolverTests
{
    /// <summary>
    /// Simples the injector dependency resolver should resolve a view model.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View_Model()
    {
        var container = new Container();
        container.Register<ViewModelOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var viewModel = AppLocator.Current.GetService<ViewModelOne>();

        await Assert.That(viewModel).IsNotNull();
        await Assert.That(viewModel).IsTypeOf<ViewModelOne>();
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve a view model.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View_Model_Directly()
    {
        var container = new SimpleInjectorInitializer();
        container.Register(() => new ViewModelOne());

        var viewModel = container.GetService<ViewModelOne>();

        await Assert.That(viewModel).IsNotNull();
        await Assert.That(viewModel).IsTypeOf<ViewModelOne>();
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve a view.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelOne>, ViewOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var view = AppLocator.Current.GetService<IViewFor<ViewModelOne>>();

        await Assert.That(view).IsNotNull();
        await Assert.That(view).IsTypeOf<ViewOne>();
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve the screen.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_Screen()
    {
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        container.UseSimpleInjectorDependencyResolver(new());

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>
    /// Should not throw during initialization of ReactiveUI.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Splat_Initialization_ShouldNotThrow()
    {
        Container container = new();
        SimpleInjectorInitializer initializer = new();

        Locator.SetLocator(initializer);
        AppLocator.CurrentMutable.InitializeSplat();
        container.UseSimpleInjectorDependencyResolver(initializer);
    }

    /// <summary>
    /// Should resolve dependency registered during Splat initialization.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ShouldResolveSplatRegisteredDependency()
    {
        Container container = new();
        SimpleInjectorInitializer initializer = new();

        AppLocator.SetLocator(initializer);
        AppLocator.CurrentMutable.InitializeSplat();
        container.UseSimpleInjectorDependencyResolver(initializer);

        var dependency = AppLocator.Current.GetService<ILogger>() as ILogger;
        await Assert.That(dependency).IsNotNull();
    }

    /// <summary>
    /// Should resolve dependency registered during Splat initialization.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_CollectionShouldNeverReturnNull()
    {
        var container = new Container();
        container.UseSimpleInjectorDependencyResolver(new());

        var views = AppLocator.Current.GetServices<ViewOne>();
        await Assert.That(views).IsNotNull();
    }
}
