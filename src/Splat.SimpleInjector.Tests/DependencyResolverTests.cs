// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NUnit.Framework;

using SimpleInjector;

using Splat.Common.Test;
using Splat.SimpleInjector;

namespace Splat.Simplnjector;

/// <summary>
/// Tests to show the <see cref="SimpleInjectorDependencyResolver"/> works correctly.
/// </summary>
[TestFixture]
public class DependencyResolverTests
{
    /// <summary>
    /// Simples the injector dependency resolver should resolve a view model.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Should_Resolve_View_Model()
    {
        var container = new Container();
        container.Register<ViewModelOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var viewModel = AppLocator.Current.GetService(typeof(ViewModelOne));

        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.TypeOf<ViewModelOne>());
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve a view model.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Should_Resolve_View_Model_Directly()
    {
        var container = new SimpleInjectorInitializer();
        container.Register(() => new ViewModelOne());

        var viewModel = container.GetService<ViewModelOne>();

        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.TypeOf<ViewModelOne>());
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve a view.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Should_Resolve_View()
    {
        var container = new Container();
        container.Register<IViewFor<ViewModelOne>, ViewOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var view = AppLocator.Current.GetService(typeof(IViewFor<ViewModelOne>));

        Assert.That(view, Is.Not.Null);
        Assert.That(view, Is.TypeOf<ViewOne>());
    }

    /// <summary>
    /// Simples the injector dependency resolver should resolve the screen.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Should_Resolve_Screen()
    {
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        container.UseSimpleInjectorDependencyResolver(new());

        var screen = AppLocator.Current.GetService(typeof(IScreen));

        Assert.That(screen, Is.Not.Null);
        Assert.That(screen, Is.TypeOf<MockScreen>());
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
    [Test]
    public void SimpleInjectorDependencyResolver_ShouldResolveSplatRegisteredDependency()
    {
        Container container = new();
        SimpleInjectorInitializer initializer = new();

        AppLocator.SetLocator(initializer);
        AppLocator.CurrentMutable.InitializeSplat();
        container.UseSimpleInjectorDependencyResolver(initializer);

        var dependency = AppLocator.Current.GetService(typeof(ILogger)) as ILogger;
        Assert.NotNull(dependency);
    }

    /// <summary>
    /// Should resolve dependency registered during Splat initialization.
    /// </summary>
    [Test]
    public void SimpleInjectorDependencyResolver_CollectionShouldNeverReturnNull()
    {
        var container = new Container();
        container.UseSimpleInjectorDependencyResolver(new());

        var views = AppLocator.Current.GetServices(typeof(ViewOne));
        Assert.NotNull(views);
    }
}
