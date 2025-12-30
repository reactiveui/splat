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
    /// Tests for ensuring hasregistration behaves when using contracts.
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
    /// Registers the and tests.
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
            return new ViewModelOne();
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

        resolver.RegisterLazySingleton<ViewModelOne>(() => new ViewModelOne(), contract);

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

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            callbackInvoked = true;
        });

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

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            callbackInvoked = true;
        });

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

        using var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(contract, _ =>
        {
            callbackInvoked = true;
        });

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

        using var subscription = resolver.ServiceRegistrationCallback(typeof(ViewModelOne), _ =>
        {
            callbackInvoked = true;
        });

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

        using var subscription = resolver.ServiceRegistrationCallback(typeof(ViewModelOne), contract, _ =>
        {
            callbackInvoked = true;
        });

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

        var subscription = resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            callbackCount++;
        });

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
            return new DisposableTestService();
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

        resolver.ServiceRegistrationCallback<ViewModelOne>(_ =>
        {
            callbackInvoked = true;
        });

        resolver.Dispose();

        await Assert.That(callbackInvoked).IsTrue();
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

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
