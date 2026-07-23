// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

using Splat.Common.Test;
using Splat.SimpleInjector;

namespace Splat.Simplnjector;

/// <summary>Tests for the SimpleInjector dependency resolver.</summary>
[NotInParallel]
public class DependencyResolverTests
{
    /// <summary>Simples the injector dependency resolver should resolve a view model.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View_Model()
    {
        await using var container = new Container();
        container.Register<ViewModelOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var viewModel = AppLocator.Current.GetService<ViewModelOne>();

        await Assert.That(viewModel).IsNotNull();
        await Assert.That(viewModel).IsTypeOf<ViewModelOne>();
    }

    /// <summary>Simples the injector dependency resolver should resolve a view model.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View_Model_Directly()
    {
        using var container = new SimpleInjectorInitializer();
        container.Register(static () => new ViewModelOne());

        var viewModel = container.GetService<ViewModelOne>();

        await Assert.That(viewModel).IsNotNull();
        await Assert.That(viewModel).IsTypeOf<ViewModelOne>();
    }

    /// <summary>Simples the injector dependency resolver should resolve a view.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_View()
    {
        await using var container = new Container();
        container.Register<IViewFor<ViewModelOne>, ViewOne>();
        container.UseSimpleInjectorDependencyResolver(new());

        var view = AppLocator.Current.GetService<IViewFor<ViewModelOne>>();

        await Assert.That(view).IsNotNull();
        await Assert.That(view).IsTypeOf<ViewOne>();
    }

    /// <summary>Simples the injector dependency resolver should resolve the screen.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_Should_Resolve_Screen()
    {
        await using var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        container.UseSimpleInjectorDependencyResolver(new());

        var screen = AppLocator.Current.GetService<IScreen>();

        await Assert.That(screen).IsNotNull();
        await Assert.That(screen).IsTypeOf<MockScreen>();
    }

    /// <summary>Should not throw during initialization of ReactiveUI.</summary>
    [Test]
    public void SimpleInjectorDependencyResolver_Splat_Initialization_ShouldNotThrow()
    {
        using Container container = new();
        SimpleInjectorInitializer initializer = new();

        Locator.SetLocator(initializer);
        AppLocator.CurrentMutable.InitializeSplat();
        container.UseSimpleInjectorDependencyResolver(initializer);
    }

    /// <summary>Should resolve dependency registered during Splat initialization.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ShouldResolveSplatRegisteredDependency()
    {
        await using Container container = new();
        SimpleInjectorInitializer initializer = new();

        AppLocator.SetLocator(initializer);
        AppLocator.CurrentMutable.InitializeSplat();
        container.UseSimpleInjectorDependencyResolver(initializer);

        var dependency = AppLocator.Current.GetService<ILogger>();
        await Assert.That(dependency).IsNotNull();
    }

    /// <summary>Should resolve dependency registered during Splat initialization.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_CollectionShouldNeverReturnNull()
    {
        await using var container = new Container();
        container.UseSimpleInjectorDependencyResolver(new());

        var views = AppLocator.Current.GetServices<ViewOne>();
        await Assert.That(views).IsNotNull();
    }

    /// <summary>The initializer's non-generic Register overload should store every factory and expose them from GetServices.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_NonGenericRegister_ReturnsAllServicesFromGetServices()
    {
        const int expectedServiceCount = 2;
        using var initializer = new SimpleInjectorInitializer();
        var first = new ViewModelOne();
        var second = new ViewModelOne();
        initializer.Register((Func<object?>)(() => first), typeof(IViewModelOne));
        initializer.Register((Func<object?>)(() => second), typeof(IViewModelOne));

        var services = initializer.GetServices(typeof(IViewModelOne)).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(expectedServiceCount);
            await Assert.That(services[0]).IsSameReferenceAs(first);
            await Assert.That(services[1]).IsSameReferenceAs(second);
        }
    }

    /// <summary>The initializer's generic GetServices should return the registered instances when a registration exists.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesGeneric_WithRegistration_ReturnsInstances()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register((Func<object?>)(() => instance), typeof(ViewModelOne));

        var services = initializer.GetServices<ViewModelOne>().ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(1);
            await Assert.That(services[0]).IsSameReferenceAs(instance);
        }
    }

    /// <summary>The initializer's generic GetServices should return an empty sequence when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesGeneric_WithoutRegistration_ReturnsEmpty()
    {
        using var initializer = new SimpleInjectorInitializer();

        var services = initializer.GetServices<ViewModelOne>().ToList();

        await Assert.That(services).Count().IsEqualTo(0);
    }

    /// <summary>The initializer's service/implementation Register overload should resolve to the implementation type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterServiceImplementation_ResolvesImplementation()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>();

        var service = initializer.GetService<IViewModelOne>();

        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<ViewModelOne>();
    }

    /// <summary>The initializer's RegisterLazySingleton should resolve the same instance on every resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterLazySingleton_ResolvesSameInstance()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.RegisterLazySingleton(static () => new ViewModelOne());

        var first = initializer.GetService<ViewModelOne>();
        var second = initializer.GetService<ViewModelOne>();

        await Assert.That(first).IsNotNull();
        await Assert.That(second).IsSameReferenceAs(first);
    }

    /// <summary>The resolver's service/implementation Register overload with a contract is a compatibility no-op that registers nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterServiceImplementationWithContract_DoesNotRegister()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.Register<IViewModelOne, ViewModelOne>(contract);

        await Assert.That(resolver.HasRegistration(typeof(IViewModelOne))).IsFalse();
    }

    /// <summary>The resolver's RegisterLazySingleton should resolve the same singleton instance on repeated resolutions.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterLazySingleton_ResolvesSameSingleton()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        resolver.RegisterLazySingleton(static () => new ViewModelOne());

        var first = resolver.GetService<ViewModelOne>();
        var second = resolver.GetService<ViewModelOne>();

        await Assert.That(first).IsNotNull();
        await Assert.That(second).IsSameReferenceAs(first);
    }
}
