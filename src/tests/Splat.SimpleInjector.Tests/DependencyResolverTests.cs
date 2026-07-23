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

    /// <summary>The initializer's non-generic GetService should return the last registered factory's result.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServiceByType_ReturnsLastRegisteredFactoryResult()
    {
        using var initializer = new SimpleInjectorInitializer();
        var first = new ViewModelOne();
        var second = new ViewModelOne();
        initializer.Register((Func<object?>)(() => first), typeof(IViewModelOne));
        initializer.Register((Func<object?>)(() => second), typeof(IViewModelOne));

        var service = initializer.GetService(typeof(IViewModelOne));

        await Assert.That(service).IsSameReferenceAs(second);
    }

    /// <summary>The initializer's non-generic GetService with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServiceByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register((Func<object?>)(() => instance), typeof(IViewModelOne));

        var service = initializer.GetService(typeof(IViewModelOne), contract);

        await Assert.That(service).IsSameReferenceAs(instance);
    }

    /// <summary>Registering under a null service type should round-trip through the NullServiceType wrapper via the non-generic GetService.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServiceByNullType_ResolvesNullServiceTypeWrapper()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register((Func<object?>)(() => instance), (Type?)null);

        var service = initializer.GetService((Type?)null);

        await Assert.That(service).IsTypeOf<NullServiceType>();
    }

    /// <summary>The initializer's generic GetService with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServiceGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register<IViewModelOne>(() => instance);

        var service = initializer.GetService<IViewModelOne>(contract);

        await Assert.That(service).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's generic GetService should return the default value when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServiceGeneric_WithoutRegistration_ReturnsDefault()
    {
        using var initializer = new SimpleInjectorInitializer();

        var service = initializer.GetService<IViewModelOne>();

        await Assert.That(service).IsNull();
    }

    /// <summary>The initializer's non-generic GetServices with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register((Func<object?>)(() => instance), typeof(IViewModelOne));

        var services = initializer.GetServices(typeof(IViewModelOne), contract).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(1);
            await Assert.That(services[0]).IsSameReferenceAs(instance);
        }
    }

    /// <summary>Registering under a null service type should be resolvable through the non-generic GetServices.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesByNullType_ResolvesNullServiceTypeWrapper()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register((Func<object?>)(() => instance), (Type?)null);

        var services = initializer.GetServices((Type?)null).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(1);
            await Assert.That(services[0]).IsTypeOf<NullServiceType>();
        }
    }

    /// <summary>The initializer's generic GetServices with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register<IViewModelOne>(() => instance);

        var services = initializer.GetServices<IViewModelOne>(contract).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(1);
            await Assert.That(services[0]).IsSameReferenceAs(instance);
        }
    }

    /// <summary>The initializer's non-generic HasRegistration should reflect whether a factory has been registered for a type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_HasRegistrationByType_ReflectsRegistrationState()
    {
        using var initializer = new SimpleInjectorInitializer();

        var before = initializer.HasRegistration(typeof(IViewModelOne));
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));
        var after = initializer.HasRegistration(typeof(IViewModelOne));

        using (Assert.Multiple())
        {
            await Assert.That(before).IsFalse();
            await Assert.That(after).IsTrue();
        }
    }

    /// <summary>The initializer's non-generic HasRegistration should return false for a null service type when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_HasRegistrationByNullType_ReturnsFalseWhenUnregistered()
    {
        using var initializer = new SimpleInjectorInitializer();

        var result = initializer.HasRegistration((Type?)null);

        await Assert.That(result).IsFalse();
    }

    /// <summary>The initializer's non-generic HasRegistration with a contract should ignore the contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_HasRegistrationByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));

        var result = initializer.HasRegistration(typeof(IViewModelOne), contract);

        await Assert.That(result).IsTrue();
    }

    /// <summary>The initializer's generic HasRegistration with a contract should ignore the contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_HasRegistrationGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>();

        var result = initializer.HasRegistration<IViewModelOne>(contract);

        await Assert.That(result).IsTrue();
    }

    /// <summary>The initializer's non-generic Register with a contract should ignore the contract and still register the factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterByTypeWithContract_IgnoresContractAndRegisters()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register((Func<object?>)(() => instance), typeof(IViewModelOne), contract);

        await Assert.That(initializer.GetService(typeof(IViewModelOne))).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's generic Register with a contract should ignore the contract and still register the factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterGenericWithContract_IgnoresContractAndRegisters()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register<IViewModelOne>(() => instance, contract);

        await Assert.That(initializer.GetService<IViewModelOne>()).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's service/implementation Register with a contract should ignore the contract and resolve the implementation.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterServiceImplementationWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();

        initializer.Register<IViewModelOne, ViewModelOne>(contract);

        await Assert.That(initializer.GetService<IViewModelOne>()).IsTypeOf<ViewModelOne>();
    }

    /// <summary>The initializer does not support unregistering the current registration by type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterCurrentByType_Throws()
    {
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.UnregisterCurrent(typeof(IViewModelOne))).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support unregistering the current registration by type with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterCurrentByTypeWithContract_Throws()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.UnregisterCurrent(typeof(IViewModelOne), contract)).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support unregistering the current registration generically.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterCurrentGeneric_Throws()
    {
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.UnregisterCurrent<IViewModelOne>()).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support unregistering the current registration generically with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterCurrentGenericWithContract_Throws()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.UnregisterCurrent<IViewModelOne>(contract)).Throws<NotSupportedException>();
    }

    /// <summary>The initializer's non-generic UnregisterAll should remove every factory for the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByType_RemovesRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));

        initializer.UnregisterAll(typeof(IViewModelOne));

        await Assert.That(initializer.HasRegistration(typeof(IViewModelOne))).IsFalse();
    }

    /// <summary>The initializer's non-generic UnregisterAll should remove factories registered under a null service type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByNullType_RemovesRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), (Type?)null);

        initializer.UnregisterAll((Type?)null);

        await Assert.That(initializer.HasRegistration((Type?)null)).IsFalse();
    }

    /// <summary>The initializer's non-generic UnregisterAll with a contract should ignore the contract and remove registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByTypeWithContract_RemovesRegistrations()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));

        initializer.UnregisterAll(typeof(IViewModelOne), contract);

        await Assert.That(initializer.HasRegistration(typeof(IViewModelOne))).IsFalse();
    }

    /// <summary>The initializer's generic UnregisterAll should remove every factory for the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllGeneric_RemovesRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>();

        initializer.UnregisterAll<IViewModelOne>();

        await Assert.That(initializer.HasRegistration<IViewModelOne>()).IsFalse();
    }

    /// <summary>The initializer's generic UnregisterAll with a contract should ignore the contract and remove registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllGenericWithContract_RemovesRegistrations()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>();

        initializer.UnregisterAll<IViewModelOne>(contract);

        await Assert.That(initializer.HasRegistration<IViewModelOne>()).IsFalse();
    }

    /// <summary>The initializer does not support service registration callbacks by type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ServiceRegistrationCallbackByType_Throws()
    {
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.ServiceRegistrationCallback(typeof(IViewModelOne), static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support service registration callbacks by type with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ServiceRegistrationCallbackByTypeWithContract_Throws()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.ServiceRegistrationCallback(typeof(IViewModelOne), contract, static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support service registration callbacks generically.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ServiceRegistrationCallbackGeneric_Throws()
    {
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.ServiceRegistrationCallback<IViewModelOne>(static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The initializer does not support service registration callbacks generically with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ServiceRegistrationCallbackGenericWithContract_Throws()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();

        await Assert.That(() => initializer.ServiceRegistrationCallback<IViewModelOne>(contract, static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The initializer's RegisterConstant with a contract should ignore the contract and register the constant.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterConstantWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.RegisterConstant(instance, contract);

        await Assert.That(initializer.GetService<ViewModelOne>()).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's RegisterLazySingleton with a contract should ignore the contract and resolve the same instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterLazySingletonWithContract_IgnoresContract()
    {
        const string contract = "contract";
        using var initializer = new SimpleInjectorInitializer();
        initializer.RegisterLazySingleton(static () => new ViewModelOne(), contract);

        var first = initializer.GetService<ViewModelOne>();
        var second = initializer.GetService<ViewModelOne>();

        await Assert.That(first).IsNotNull();
        await Assert.That(second).IsSameReferenceAs(first);
    }

    /// <summary>The resolver's non-generic GetService should fall back to the registered collection when no single registration exists.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceByType_FallsBackToCollection()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), initializer);

        var service = resolver.GetService(typeof(IViewModelOne));

        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<ViewModelOne>();
    }

    /// <summary>The resolver's non-generic GetService should return null when the service type cannot be resolved.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceByType_UnresolvableReturnsNull()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        var service = resolver.GetService(typeof(IViewModelOne));

        await Assert.That(service).IsNull();
    }

    /// <summary>The resolver's non-generic GetService should treat a null service type as the null service type and return null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceByNullType_ReturnsNull()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        var service = resolver.GetService((Type?)null);

        await Assert.That(service).IsNull();
    }

    /// <summary>The resolver's non-generic GetService with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        var service = resolver.GetService(typeof(IScreen), contract);

        await Assert.That(service).IsTypeOf<MockScreen>();
    }

    /// <summary>The resolver's generic GetService with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        var service = resolver.GetService<IScreen>(contract);

        await Assert.That(service).IsTypeOf<MockScreen>();
    }

    /// <summary>The resolver's non-generic GetServices should return the single registration when no collection registration exists.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServicesByType_ReturnsSingleRegistration()
    {
        const int expectedCount = 1;
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        var services = resolver.GetServices(typeof(IScreen)).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(expectedCount);
            await Assert.That(services[0]).IsTypeOf<MockScreen>();
        }
    }

    /// <summary>The resolver's non-generic GetServices should return an empty sequence for a null service type when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServicesByNullType_ReturnsEmpty()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        var services = resolver.GetServices((Type?)null).ToList();

        await Assert.That(services).Count().IsEqualTo(0);
    }

    /// <summary>The resolver's non-generic GetServices with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServicesByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        const int expectedCount = 1;
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        var services = resolver.GetServices(typeof(IScreen), contract).ToList();

        await Assert.That(services).Count().IsEqualTo(expectedCount);
    }

    /// <summary>The resolver's generic GetServices with a contract should ignore the contract and resolve normally.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServicesGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        const int expectedCount = 1;
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        var services = resolver.GetServices<IScreen>(contract).ToList();

        await Assert.That(services).Count().IsEqualTo(expectedCount);
    }

    /// <summary>The resolver's non-generic HasRegistration should reflect whether the container has a registration for a type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_HasRegistrationByType_ReflectsContainerState()
    {
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IScreen))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(IViewModelOne))).IsFalse();
        }
    }

    /// <summary>The resolver's non-generic HasRegistration should return false for a null service type when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_HasRegistrationByNullType_ReturnsFalse()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(resolver.HasRegistration((Type?)null)).IsFalse();
    }

    /// <summary>The resolver's non-generic HasRegistration with a contract should ignore the contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_HasRegistrationByTypeWithContract_IgnoresContract()
    {
        const string contract = "contract";
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        await Assert.That(resolver.HasRegistration(typeof(IScreen), contract)).IsTrue();
    }

    /// <summary>The resolver's generic HasRegistration with a contract should ignore the contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_HasRegistrationGenericWithContract_IgnoresContract()
    {
        const string contract = "contract";
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        await Assert.That(resolver.HasRegistration<IScreen>(contract)).IsTrue();
    }

    /// <summary>The resolver's service/implementation Register overload should register directly into the container and resolve the implementation.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterServiceImplementation_ResolvesImplementation()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.Register<IViewModelOne, ViewModelOne>();

        await Assert.That(resolver.GetService<IViewModelOne>()).IsTypeOf<ViewModelOne>();
    }

    /// <summary>The resolver does not support unregistering the current registration by type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterCurrentByType_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterCurrent(typeof(IViewModelOne))).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering the current registration by type with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterCurrentByTypeWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterCurrent(typeof(IViewModelOne), contract)).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering the current registration generically.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterCurrentGeneric_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterCurrent<IViewModelOne>()).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering the current registration generically with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterCurrentGenericWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterCurrent<IViewModelOne>(contract)).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering all registrations by type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterAllByType_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterAll(typeof(IViewModelOne))).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering all registrations by type with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterAllByTypeWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterAll(typeof(IViewModelOne), contract)).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering all registrations generically.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterAllGeneric_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterAll<IViewModelOne>()).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support unregistering all registrations generically with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_UnregisterAllGenericWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.UnregisterAll<IViewModelOne>(contract)).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support service registration callbacks by type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ServiceRegistrationCallbackByType_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(IViewModelOne), static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support service registration callbacks by type with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ServiceRegistrationCallbackByTypeWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(IViewModelOne), contract, static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support service registration callbacks generically.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ServiceRegistrationCallbackGeneric_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.ServiceRegistrationCallback<IViewModelOne>(static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The resolver does not support service registration callbacks generically with a contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ServiceRegistrationCallbackGenericWithContract_Throws()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        await Assert.That(() => resolver.ServiceRegistrationCallback<IViewModelOne>(contract, static _ => { })).Throws<NotSupportedException>();
    }

    /// <summary>The resolver's RegisterConstant with a contract is a compatibility no-op that registers nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterConstantWithContract_DoesNotRegister()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.RegisterConstant(new ViewModelOne(), contract);

        await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsFalse();
    }

    /// <summary>The resolver's RegisterLazySingleton with a contract is a compatibility no-op that registers nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterLazySingletonWithContract_DoesNotRegister()
    {
        const string contract = "contract";
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.RegisterLazySingleton(static () => new ViewModelOne(), contract);

        await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsFalse();
    }

    /// <summary>Disposing the resolver without disposing managed resources should leave the underlying container usable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_DisposeWithoutManagedResources_LeavesContainerUsable()
    {
        var container = new Container();
        container.Register<ViewModelOne>();
        using var resolver = new DisposeProbeResolver(container, new SimpleInjectorInitializer());

        resolver.InvokeDispose(false);

        await Assert.That(container.GetInstance<ViewModelOne>()).IsNotNull();
    }

    /// <summary>Test-only resolver that exposes the protected dispose to exercise the non-managed dispose branch.</summary>
    /// <param name="container">The container the resolver adapts.</param>
    /// <param name="initializer">The initializer supplying the factory registrations.</param>
    private sealed class DisposeProbeResolver(Container container, SimpleInjectorInitializer initializer)
        : SimpleInjectorDependencyResolver(container, initializer)
    {
        /// <summary>Invokes the protected dispose with the supplied flag.</summary>
        /// <param name="disposing">Whether managed resources should be released.</param>
        public void InvokeDispose(bool disposing) => Dispose(disposing);
    }
}
