// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Common tests for Dependency Resolver interaction with Splat.
/// </summary>
/// <typeparam name="T">The dependency resolver to test.</typeparam>
[NotInParallel]
public abstract class BaseDependencyResolverTests<T>
    where T : IDependencyResolver
{
    private AppLocatorScope? _appLocatorScope;

    /// <summary>
    /// Setup method to initialize AppLocatorScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpAppLocatorScope() => _appLocatorScope = new AppLocatorScope();

    /// <summary>
    /// Teardown method to dispose AppLocatorScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownAppLocatorScope()
    {
        _appLocatorScope?.Dispose();
        _appLocatorScope = null;
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(type);
            resolver.UnregisterCurrent(type);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure UnregisterCurrent removes last entry.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Remove_Last()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new FuncLogManager(_ => new WrappingFullLogger(new DebugLogger())), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        var service = resolver.GetService(type);
        await Assert.That(service).IsTypeOf<FuncLogManager>();

        resolver.UnregisterCurrent(type);

        service = resolver.GetService(type);
        await Assert.That(service).IsTypeOf<DefaultLogManager>();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        await Assert.That(() =>
        {
            resolver.UnregisterCurrent(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, "named");

        await Assert.That(() =>
        {
            resolver.UnregisterAll(type);
            resolver.UnregisterCurrent(type);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
    {
        var resolver = GetDependencyResolver();
        var type = typeof(ILogManager);
        const string contract = "named";

        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type);
        resolver.Register(() => new DefaultLogManager(AppLocator.Current), type, contract);

        await Assert.That(() =>
        {
            resolver.UnregisterAll(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Ensures <see cref="IReadonlyDependencyResolver.GetServices(Type, string)"/> never returns null.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_Should_Never_Return_Null()
    {
        var resolver = GetDependencyResolver();

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetServices<string>()).IsNotNull();
            await Assert.That(resolver.GetServices<string>("Landscape")).IsNotNull();
        }
    }

    /// <summary>
    /// Tests for ensuring Has Registration method behaves when using contracts.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration()
    {
        var type = typeof(string);
        const string contractOne = "ContractOne";
        const string contractTwo = "ContractTwo";
        var resolver = GetDependencyResolver();

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.Register(() => "unnamed", type);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.UnregisterAll(type);

        resolver.Register(() => contractOne, type, contractOne);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsTrue();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsFalse();
        }

        resolver.UnregisterAll(type, contractOne);

        resolver.Register(() => contractTwo, type, contractTwo);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(type)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractOne)).IsFalse();
            await Assert.That(resolver.HasRegistration(type, contractTwo)).IsTrue();
        }
    }

    /// <summary>
    /// Nulls the resolver tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NullResolverTests()
    {
        IMutableDependencyResolver? resolver1 = null;
        IDependencyResolver? resolver2 = null;

        await Assert.That(() => resolver2!.WithResolver().Dispose()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingleton(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd<ViewModelOne>("eight")).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterLazySingletonAnd<DefaultLogManager>(() => new(AppLocator.Current), "seven")).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd<ViewModelOne>()).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd(() => new DefaultLogManager(AppLocator.Current))).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterAnd<IViewModelOne>(() => new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd(new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd(new ViewModelOne())).Throws<ArgumentNullException>();
        await Assert.That(() => resolver1!.RegisterConstantAnd<ViewModelOne>()).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Registers the resolver and tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndTests()
    {
        var resolver = GetDependencyResolver();

        await Assert.That(() => resolver.RegisterAnd<IViewModelOne>(null!)).Throws<ArgumentNullException>();

        resolver.RegisterAnd<ViewModelOne>("one")
                .RegisterAnd<IViewModelOne, ViewModelOne>("two")
                .RegisterAnd(() => new DefaultLogManager(AppLocator.Current), "three")
                .RegisterAnd<IViewModelOne>(() => new ViewModelOne(), "four")
                .RegisterConstantAnd<ViewModelOne>("five")
                .RegisterConstantAnd(new ViewModelOne(), "six")
                .RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), typeof(ILogManager), "seven")
                .RegisterLazySingletonAnd<ViewModelOne>("eight")
                .RegisterLazySingletonAnd(() => new DefaultLogManager(AppLocator.Current), "seven")
                .Register<IViewModelOne, ViewModelOne>();
    }

    /// <summary>
    /// Test generic GetService without contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_Generic_WithoutContract_ReturnsService()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test generic GetService with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_Generic_WithContract_ReturnsService()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.RegisterConstant(new ViewModelOne(), contract);

        var result = resolver.GetService<ViewModelOne>(contract);

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test generic GetService returns null for unregistered service.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_Generic_Unregistered_ReturnsNull()
    {
        var resolver = GetDependencyResolver();

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test generic GetServices returns all registered services.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_Generic_ReturnsAllServices()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        var results = resolver.GetServices<ViewModelOne>().ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(2);
    }

    /// <summary>
    /// Test generic GetServices with contract returns only contract services.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_Generic_WithContract_ReturnsContractServices()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne(), contract);

        var results = resolver.GetServices<ViewModelOne>(contract).ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Test generic HasRegistration returns true when registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_Generic_WithoutContract_ReturnsTrueWhenRegistered()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());

        var result = resolver.HasRegistration<ViewModelOne>();

        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test generic HasRegistration with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_Generic_WithContract_ReturnsTrueWhenRegistered()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.RegisterConstant(new ViewModelOne(), contract);

        var result = resolver.HasRegistration<ViewModelOne>(contract);

        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test generic HasRegistration returns false when not registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_Generic_NotRegistered_ReturnsFalse()
    {
        var resolver = GetDependencyResolver();

        var result = resolver.HasRegistration<ViewModelOne>();

        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Test generic Register with factory function.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithFactory_RegistersService()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test generic Register with factory and contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithFactoryAndContract_RegistersService()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.Register(() => new ViewModelOne(), contract);

        var result = resolver.GetService<ViewModelOne>(contract);

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test generic Register with TService and TImplementation.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithServiceAndImplementation_RegistersService()
    {
        var resolver = GetDependencyResolver();
        resolver.Register<IViewModelOne, ViewModelOne>();

        var result = resolver.GetService<IViewModelOne>();

        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsTypeOf<ViewModelOne>();
    }

    /// <summary>
    /// Test generic Register with TService, TImplementation, and contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithServiceImplementationAndContract_RegistersService()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.Register<IViewModelOne, ViewModelOne>(contract);

        var result = resolver.GetService<IViewModelOne>(contract);

        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsTypeOf<ViewModelOne>();
    }

    /// <summary>
    /// Test generic RegisterConstant creates singleton.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterConstant_Generic_CreatesSingleton()
    {
        var resolver = GetDependencyResolver();
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance);

        var result1 = resolver.GetService<ViewModelOne>();
        var result2 = resolver.GetService<ViewModelOne>();

        await Assert.That(result1).IsSameReferenceAs(instance);
        await Assert.That(result2).IsSameReferenceAs(instance);
    }

    /// <summary>
    /// Test generic RegisterConstant with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterConstant_Generic_WithContract_CreatesSingleton()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance, contract);

        var result = resolver.GetService<ViewModelOne>(contract);

        await Assert.That(result).IsSameReferenceAs(instance);
    }

    /// <summary>
    /// Test generic RegisterLazySingleton creates singleton on first access.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterLazySingleton_Generic_CreatesSingletonOnFirstAccess()
    {
        var resolver = GetDependencyResolver();
        var callCount = 0;

        resolver.RegisterLazySingleton<ViewModelOne>(() =>
        {
            callCount++;
            return new();
        });

        await Assert.That(callCount).IsEqualTo(0); // Not called yet

        var result1 = resolver.GetService<ViewModelOne>();
        await Assert.That(callCount).IsEqualTo(1); // Called once
        await Assert.That(result1).IsNotNull();

        var result2 = resolver.GetService<ViewModelOne>();
        await Assert.That(callCount).IsEqualTo(1); // Not called again

        await Assert.That(result1).IsSameReferenceAs(result2);
    }

    /// <summary>
    /// Test generic RegisterLazySingleton with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterLazySingleton_Generic_WithContract_CreatesSingleton()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";

        resolver.RegisterLazySingleton<ViewModelOne>(() => new(), contract);

        var result1 = resolver.GetService<ViewModelOne>(contract);
        var result2 = resolver.GetService<ViewModelOne>(contract);

        await Assert.That(result1).IsNotNull();
        await Assert.That(result1).IsSameReferenceAs(result2);
    }

    /// <summary>
    /// Test generic UnregisterCurrent removes last registration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Generic_RemovesLastRegistration()
    {
        var resolver = GetDependencyResolver();
        var instance1 = new ViewModelOne();
        var instance2 = new ViewModelOne();

        resolver.RegisterConstant(instance1);
        resolver.RegisterConstant(instance2);

        var beforeUnregister = resolver.GetService<ViewModelOne>();
        await Assert.That(beforeUnregister).IsSameReferenceAs(instance2);

        resolver.UnregisterCurrent<ViewModelOne>();

        var afterUnregister = resolver.GetService<ViewModelOne>();
        await Assert.That(afterUnregister).IsSameReferenceAs(instance1);
    }

    /// <summary>
    /// Test generic UnregisterCurrent with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Generic_WithContract_RemovesRegistration()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.RegisterConstant(new ViewModelOne(), contract);

        await Assert.That(resolver.HasRegistration<ViewModelOne>(contract)).IsTrue();

        resolver.UnregisterCurrent<ViewModelOne>(contract);

        await Assert.That(resolver.HasRegistration<ViewModelOne>(contract)).IsFalse();
    }

    /// <summary>
    /// Test generic UnregisterAll removes all registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_Generic_RemovesAllRegistrations()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsTrue();

        resolver.UnregisterAll<ViewModelOne>();

        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsFalse();
    }

    /// <summary>
    /// Test generic UnregisterAll with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_Generic_WithContract_RemovesAllContractRegistrations()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.RegisterConstant(new ViewModelOne(), contract);
        resolver.Register(() => new ViewModelOne(), contract);

        await Assert.That(resolver.HasRegistration<ViewModelOne>(contract)).IsTrue();

        resolver.UnregisterAll<ViewModelOne>(contract);

        await Assert.That(resolver.HasRegistration<ViewModelOne>(contract)).IsFalse();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback generic version is invoked when service is registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_Generic_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        var callbackInvoked = false;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsFalse();

        resolver.Register(() => new ViewModelOne());

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback generic version with existing registration invokes immediately.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_Generic_WithExistingRegistration_InvokesImmediately()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());

        var callbackInvoked = false;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback generic with contract version is invoked.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_Generic_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        var callbackInvoked = false;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(contract, _ => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsFalse();

        resolver.Register(() => new ViewModelOne(), contract);

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback non-generic version is invoked.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_NonGeneric_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        var callbackInvoked = false;

        using var subscription = resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsFalse();

        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback non-generic with contract version is invoked.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_NonGeneric_WithContract_InvokedWhenServiceRegistered()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        var callbackInvoked = false;

        using var subscription = resolver.ServiceRegistrationCallback(
            typeof(ViewModelOne),
            contract,
            _ => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsFalse();

        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne), contract);

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test ServiceRegistrationCallback disposal stops receiving notifications.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_Disposal_StopsReceivingNotifications()
    {
        var resolver = GetDependencyResolver();
        var callbackCount = 0;

        var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackCount++);

        resolver.Register(() => new ViewModelOne());
        await Assert.That(callbackCount).IsEqualTo(1);

        subscription.Dispose();

        resolver.Register(() => new ViewModelOne());
        await Assert.That(callbackCount).IsEqualTo(1); // Should not increment
    }

    /// <summary>
    /// Test ServiceRegistrationCallback with null callback throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_NullCallback_Throws()
    {
        var resolver = GetDependencyResolver();

        await Assert.That(() => resolver.ServiceRegistrationCallback<ViewModelOne>(null!))
            .Throws<ArgumentNullException>();

        await Assert.That(() => resolver.ServiceRegistrationCallback(typeof(ViewModelOne), null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test Dispose can be called multiple times without throwing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_DoubleDispose_DoesNotThrow()
    {
        var resolver = GetDependencyResolver();

        await Assert.That(() =>
        {
            resolver.Dispose();
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test Dispose disposes registered IDisposable services.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_DisposesRegisteredServices()
    {
        var resolver = GetDependencyResolver();
        var disposableService = new DisposableTestService();

        resolver.RegisterConstant(disposableService);

        await Assert.That(disposableService.IsDisposed).IsFalse();

        resolver.Dispose();

        await Assert.That(disposableService.IsDisposed).IsTrue();
    }

    /// <summary>
    /// Test Dispose does not create lazy services just to dispose them.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_WithLazySingleton_DoesNotCreateIfNotAccessed()
    {
        var resolver = GetDependencyResolver();
        var factoryCalled = false;

        resolver.RegisterLazySingleton<DisposableTestService>(() =>
        {
            factoryCalled = true;
            return new();
        });

        await Assert.That(factoryCalled).IsFalse();

        resolver.Dispose();

        // Factory should not be called during disposal if lazy value was never accessed
        await Assert.That(factoryCalled).IsFalse();
    }

    /// <summary>
    /// Test Dispose invokes registered callbacks.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_InvokesCallbacks()
    {
        var resolver = GetDependencyResolver();
        var callbackInvoked = false;

        resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackInvoked = true);

        resolver.Dispose();

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>
    /// Test GetService with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance);

        var result = resolver.GetService<ViewModelOne>(null);

        await Assert.That(result).IsSameReferenceAs(instance);
    }

    /// <summary>
    /// Test GetService non-generic with null contract returns null when no registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_NonGeneric_WithContract_ReturnsNullWhenEmpty()
    {
        var resolver = GetDependencyResolver();

        var result = resolver.GetService(typeof(ViewModelOne), "test");

        await Assert.That(result).IsNull();
    }

    /// <summary>
    /// Test GetServices with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());

        var results = resolver.GetServices<ViewModelOne>(null).ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Test GetServices non-generic with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        var results = resolver.GetServices(typeof(ViewModelOne), null).ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Test GetServices non-generic with contract returns empty when no registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_NonGeneric_WithContract_ReturnsEmptyWhenNoRegistrations()
    {
        var resolver = GetDependencyResolver();

        var results = resolver.GetServices(typeof(ViewModelOne), "test").ToList();

        await Assert.That(results).IsEmpty();
    }

    /// <summary>
    /// Test HasRegistration with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());

        var result = resolver.HasRegistration<ViewModelOne>(null);

        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test HasRegistration generic with contract returns false when no registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_Generic_WithContract_ReturnsFalseWhenEmpty()
    {
        var resolver = GetDependencyResolver();

        var result = resolver.HasRegistration<ViewModelOne>("test");

        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Test HasRegistration non-generic with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        var result = resolver.HasRegistration(typeof(ViewModelOne), null);

        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test Register with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), null);

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test Register non-generic with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne), null);

        var result = resolver.GetService(typeof(ViewModelOne));

        await Assert.That(result).IsNotNull();
    }

    /// <summary>
    /// Test Register with TService and TImplementation and null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_Generic_WithServiceImplementationAndNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register<IViewModelOne, ViewModelOne>(null);

        var result = resolver.GetService<IViewModelOne>();

        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsTypeOf<ViewModelOne>();
    }

    /// <summary>
    /// Test RegisterConstant with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterConstant_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance, null);

        var result = resolver.GetService<ViewModelOne>();

        await Assert.That(result).IsSameReferenceAs(instance);
    }

    /// <summary>
    /// Test RegisterLazySingleton with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task RegisterLazySingleton_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterLazySingleton<ViewModelOne>(() => new(), null);

        var result1 = resolver.GetService<ViewModelOne>();
        var result2 = resolver.GetService<ViewModelOne>();

        await Assert.That(result1).IsNotNull();
        await Assert.That(result1).IsSameReferenceAs(result2);
    }

    /// <summary>
    /// Test UnregisterCurrent with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        var instance1 = new ViewModelOne();
        var instance2 = new ViewModelOne();

        resolver.RegisterConstant(instance1);
        resolver.RegisterConstant(instance2);

        resolver.UnregisterCurrent<ViewModelOne>(null);

        var result = resolver.GetService<ViewModelOne>();
        await Assert.That(result).IsSameReferenceAs(instance1);
    }

    /// <summary>
    /// Test UnregisterCurrent non-generic with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        resolver.UnregisterCurrent(typeof(ViewModelOne), null);

        await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsTrue();
    }

    /// <summary>
    /// Test UnregisterAll with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_Generic_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.RegisterConstant(new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        resolver.UnregisterAll<ViewModelOne>(null);

        await Assert.That(resolver.HasRegistration<ViewModelOne>()).IsFalse();
    }

    /// <summary>
    /// Test UnregisterAll non-generic with null contract delegates to non-contract version.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_NonGeneric_WithNullContract_DelegatesToNonContract()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        resolver.UnregisterAll(typeof(ViewModelOne), null);

        await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsFalse();
    }

    /// <summary>
    /// Test GetServices combines generic and non-generic registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_Generic_CombinesGenericAndNonGenericResults()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne));

        var results = resolver.GetServices<ViewModelOne>().ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(2);
    }

    /// <summary>
    /// Test GetServices with contract combines generic and non-generic registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_Generic_WithContract_CombinesGenericAndNonGenericResults()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        resolver.Register(() => new ViewModelOne(), contract);
        resolver.Register(() => new ViewModelOne(), typeof(ViewModelOne), contract);

        var results = resolver.GetServices<ViewModelOne>(contract).ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(2);
    }

    /// <summary>
    /// Test ServiceRegistrationCallback invokes callback for each existing registration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task ServiceRegistrationCallback_Generic_InvokesForEachExistingRegistration()
    {
        var resolver = GetDependencyResolver();
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());
        resolver.Register(() => new ViewModelOne());

        var callbackCount = 0;

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackCount++);

        await Assert.That(callbackCount).IsGreaterThanOrEqualTo(3);
    }

    /// <summary>
    /// Test Dispose handles exceptions from callbacks gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_SuppressesExceptionsFromCallbacks()
    {
        var resolver = GetDependencyResolver();

        resolver.ServiceRegistrationCallback<ViewModelOne>(_ => throw new InvalidOperationException("Test exception"));

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test Dispose handles exceptions from service disposal gracefully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_SuppressesExceptionsFromServiceDisposal()
    {
        var resolver = GetDependencyResolver();
        var throwingService = new ThrowingDisposableService();

        resolver.RegisterConstant(throwingService);

        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test registration callbacks are not invoked after disposal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Register_AfterDispose_DoesNotInvokeCallbacks()
    {
        var resolver = GetDependencyResolver();
        var callbackCount = 0;

        resolver.ServiceRegistrationCallback<ViewModelOne>(_ => callbackCount++);

        resolver.Dispose();

        var countAfterDispose = callbackCount;

        await Assert.That(() =>
        {
            resolver.Register(() => new ViewModelOne());
        }).Throws<ObjectDisposedException>();

        await Assert.That(callbackCount).IsEqualTo(countAfterDispose);
    }

    /// <summary>
    /// Test RegisterLazySingleton unwrapping in non-generic GetService.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_NonGeneric_UnwrapsLazySingleton()
    {
        var resolver = GetDependencyResolver();
        var factoryCalled = false;

        resolver.RegisterLazySingleton<ViewModelOne>(() =>
        {
            factoryCalled = true;
            return new();
        });

        await Assert.That(factoryCalled).IsFalse();

        var result1 = resolver.GetService(typeof(ViewModelOne));
        await Assert.That(factoryCalled).IsTrue();
        await Assert.That(result1).IsNotNull();

        var result2 = resolver.GetService(typeof(ViewModelOne));
        await Assert.That(result1).IsSameReferenceAs(result2);
    }

    /// <summary>
    /// Test RegisterLazySingleton unwrapping in non-generic GetService with contract.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_NonGeneric_WithContract_UnwrapsLazySingleton()
    {
        var resolver = GetDependencyResolver();
        const string contract = "test";
        var factoryCalled = false;

        resolver.RegisterLazySingleton<ViewModelOne>(
            () =>
            {
                factoryCalled = true;
                return new();
            },
            contract);

        await Assert.That(factoryCalled).IsFalse();

        var result1 = resolver.GetService(typeof(ViewModelOne), contract);
        await Assert.That(factoryCalled).IsTrue();
        await Assert.That(result1).IsNotNull();

        var result2 = resolver.GetService(typeof(ViewModelOne), contract);
        await Assert.That(result1).IsSameReferenceAs(result2);
    }

    /// <summary>
    /// Test Dispose checks Lazy.IsValueCreated before disposing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_WithAccessedLazySingleton_DisposesValue()
    {
        var resolver = GetDependencyResolver();
        var disposableService = new DisposableTestService();

        resolver.RegisterLazySingleton<DisposableTestService>(() => disposableService);

        var retrieved = resolver.GetService<DisposableTestService>();
        await Assert.That(retrieved).IsSameReferenceAs(disposableService);
        await Assert.That(disposableService.IsDisposed).IsFalse();

        resolver.Dispose();

        await Assert.That(disposableService.IsDisposed).IsTrue();
    }

    /// <summary>
    /// Test that Dispose does NOT create transient services.
    /// Transient services should never be instantiated during disposal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_WithTransientServices_DoesNotCreateInstances()
    {
        var resolver = GetDependencyResolver();
        var transientCreatedCount = 0;
        var disposableCreatedCount = 0;

        // Register transient non-disposable services
        resolver.Register(() =>
        {
            transientCreatedCount++;
            return new ViewModelOne();
        });

        // Register transient disposable services
        resolver.Register<DisposableTestService>(() =>
        {
            disposableCreatedCount++;
            return new DisposableTestService();
        });

        // Get some instances to verify registration works
        _ = resolver.GetService<ViewModelOne>();
        _ = resolver.GetService<DisposableTestService>();

        await Assert.That(transientCreatedCount).IsEqualTo(1);
        await Assert.That(disposableCreatedCount).IsEqualTo(1);

        // Dispose the resolver - should NOT create any new transient instances
        resolver.Dispose();

        // Counts should remain the same - no new instances created during disposal
        await Assert.That(transientCreatedCount).IsEqualTo(1);
        await Assert.That(disposableCreatedCount).IsEqualTo(1);
    }

    /// <summary>
    /// Test GetService with null serviceType uses NullServiceType.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetService_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        var instance = new ViewModelOne();

        resolver.Register((Func<object?>)(() => instance), (Type?)null);

        var result = resolver.GetService(null);

        await Assert.That(result).IsSameReferenceAs(instance);
    }

    /// <summary>
    /// Test GetServices with null serviceType uses NullServiceType.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task GetServices_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        resolver.Register((Func<object?>)(() => new ViewModelOne()), (Type?)null);

        var results = resolver.GetServices(null).ToList();

        await Assert.That(results).Count().IsGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Test HasRegistration with null serviceType uses NullServiceType.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task HasRegistration_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        resolver.Register((Func<object?>)(() => new ViewModelOne()), (Type?)null);

        var result = resolver.HasRegistration(null);

        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Test UnregisterCurrent with null serviceType uses NullServiceType.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterCurrent_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        resolver.Register((Func<object?>)(() => new ViewModelOne()), (Type?)null);
        resolver.Register((Func<object?>)(() => new ViewModelOne()), (Type?)null);

        resolver.UnregisterCurrent(null);

        await Assert.That(resolver.HasRegistration(null)).IsTrue();
    }

    /// <summary>
    /// Test UnregisterAll with null serviceType uses NullServiceType.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task UnregisterAll_NonGeneric_WithNullType_HandlesNullServiceType()
    {
        var resolver = GetDependencyResolver();
        resolver.Register((Func<object?>)(() => new ViewModelOne()), (Type?)null);

        resolver.UnregisterAll(null);

        await Assert.That(resolver.HasRegistration(null)).IsFalse();
    }

    /// <summary>
    /// Test that when a resolver is disposed while a lazy singleton is under construction,
    /// the service should be disposed after construction and ObjectDisposedException should be thrown to caller.
    /// This matches Microsoft.Extensions.DependencyInjection behavior.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public virtual async Task Dispose_WhileLazySingletonUnderConstruction_DisposesServiceAndThrowsException()
    {
        var resolver = GetDependencyResolver();

        // Register a slow service that takes 3 seconds to construct
        resolver.RegisterLazySingleton<SlowDisposableService>(() => new SlowDisposableService());

        // Register a fast service for comparison
        resolver.RegisterLazySingleton<FastDisposableService>(() => new FastDisposableService());

        // Start constructing the slow service in a background task
        var slowTask = Task.Run(() => resolver.GetService<SlowDisposableService>());

        // Wait for construction to start
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Start constructing the fast service in a background task
        var fastTask = Task.Run(() => resolver.GetService<FastDisposableService>());

        // Wait a bit to ensure fast service is constructed
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Dispose the resolver while slow service is still being constructed
        resolver.Dispose();

        // The fast service should have been disposed during resolver disposal
        FastDisposableService? fastService = null;
        try
        {
            fastService = await fastTask;

            // If we get the service, it should be disposed
            if (fastService is not null)
            {
                await Assert.That(fastService.IsDisposed).IsTrue();
            }
        }
        catch (ObjectDisposedException)
        {
            // This is also acceptable - throwing ObjectDisposedException
        }

        // The slow service construction should complete, then be disposed,
        // and an ObjectDisposedException should be thrown to the caller
        SlowDisposableService? slowService = null;
        var exceptionThrown = false;

        try
        {
            slowService = await slowTask;

            // If we got a service instance back, it MUST be disposed
            // (Microsoft DI behavior: construct, dispose, then throw)
            if (slowService is not null)
            {
                await Assert.That(slowService.IsDisposed).IsTrue();

                // Getting a disposed service is not ideal, but if we do,
                // subsequent calls should throw ObjectDisposedException
                await Assert.That(() => resolver.GetService<SlowDisposableService>())
                    .Throws<ObjectDisposedException>();
            }
        }
        catch (ObjectDisposedException)
        {
            exceptionThrown = true;
        }

        // Ideally, we should either:
        // 1. Get the service back but it must be disposed, OR
        // 2. Get an ObjectDisposedException
        // The current bug is that we get the service back and it's NOT disposed
        if (slowService is not null)
        {
            await Assert.That(slowService.IsDisposed).IsTrue();
        }
        else
        {
            await Assert.That(exceptionThrown).IsTrue();
        }
    }

    /// <summary>
    /// Gets an instance of a dependency resolver to test.
    /// </summary>
    /// <returns>Dependency Resolver.</returns>
    protected abstract T GetDependencyResolver();

    /// <summary>
    /// Disposable test service for testing disposal.
    /// </summary>
    protected sealed class DisposableTestService : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }

    /// <summary>
    /// Disposable test service that throws exceptions during disposal.
    /// </summary>
    protected sealed class ThrowingDisposableService : IDisposable
    {
        public void Dispose() => throw new InvalidOperationException("Disposal exception");
    }

    /// <summary>
    /// Disposable test service that takes time to construct.
    /// </summary>
    protected sealed class SlowDisposableService : IDisposable
    {
        public SlowDisposableService()
        {
            ConstructionStarted = true;
            Thread.Sleep(TimeSpan.FromSeconds(3));
            ConstructionCompleted = true;
        }

        public bool IsDisposed { get; private set; }

        public bool ConstructionStarted { get; private set; }

        public bool ConstructionCompleted { get; private set; }

        public void Dispose() => IsDisposed = true;
    }

    /// <summary>
    /// Disposable test service that constructs quickly.
    /// </summary>
    protected sealed class FastDisposableService : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }
}
