// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

using Splat.Common.Test;
using Splat.SimpleInjector;

namespace Splat.Simplnjector;

/// <summary>Tests that the SimpleInjector adapter keys registrations by contract instead of dropping the contract.</summary>
public class ContractRegistrationTests
{
    /// <summary>The contract used by tests that register a single named service.</summary>
    private const string Contract = "contract";

    /// <summary>The first of two contracts used by the tests that prove contracts stay distinct.</summary>
    private const string LeftContract = "left";

    /// <summary>The second of two contracts used by the tests that prove contracts stay distinct.</summary>
    private const string RightContract = "right";

    /// <summary>The number of services expected when two are registered under the same contract.</summary>
    private const int ExpectedContractServiceCount = 2;

    /// <summary>A contract registration on the initializer must not answer a contract-less lookup.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ContractRegistration_IsInvisibleToContractlessLookup()
    {
        using var initializer = new SimpleInjectorInitializer();

        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne), Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne))).IsFalse();
            await Assert.That(initializer.GetService(typeof(IViewModelOne))).IsNull();
            await Assert.That(initializer.GetServices(typeof(IViewModelOne))).IsEmpty();
        }
    }

    /// <summary>A contract-less registration on the initializer must not answer a contract lookup.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ContractlessRegistration_IsInvisibleToContractLookup()
    {
        using var initializer = new SimpleInjectorInitializer();

        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne), Contract)).IsFalse();
            await Assert.That(initializer.HasRegistration<IViewModelOne>(Contract)).IsFalse();
            await Assert.That(initializer.GetService(typeof(IViewModelOne), Contract)).IsNull();
            await Assert.That(initializer.GetService<IViewModelOne>(Contract)).IsNull();
            await Assert.That(initializer.GetServices(typeof(IViewModelOne), Contract)).IsEmpty();
            await Assert.That(initializer.GetServices<IViewModelOne>(Contract)).IsEmpty();
        }
    }

    /// <summary>The initializer must keep two implementations registered under different contracts apart.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_DifferentContracts_ResolveDistinctServices()
    {
        using var initializer = new SimpleInjectorInitializer();
        var left = new ViewModelOne();
        var right = new ViewModelOne();

        initializer.Register<IViewModelOne>(() => left, LeftContract);
        initializer.Register<IViewModelOne>(() => right, RightContract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.GetService<IViewModelOne>(LeftContract)).IsSameReferenceAs(left);
            await Assert.That(initializer.GetService<IViewModelOne>(RightContract)).IsSameReferenceAs(right);
        }
    }

    /// <summary>The initializer's non-generic Register with a contract should resolve under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterByTypeWithContract_ResolvesUnderThatContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register((Func<object?>)(() => instance), typeof(IViewModelOne), Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne), Contract)).IsTrue();
            await Assert.That(initializer.GetService(typeof(IViewModelOne), Contract)).IsSameReferenceAs(instance);
        }
    }

    /// <summary>The initializer's generic Register with a contract should resolve under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterGenericWithContract_ResolvesUnderThatContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register<IViewModelOne>(() => instance, Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration<IViewModelOne>(Contract)).IsTrue();
            await Assert.That(initializer.GetService<IViewModelOne>(Contract)).IsSameReferenceAs(instance);
        }
    }

    /// <summary>The initializer's service/implementation Register with a contract should resolve under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterServiceImplementationWithContract_ResolvesUnderThatContract()
    {
        using var initializer = new SimpleInjectorInitializer();

        initializer.Register<IViewModelOne, ViewModelOne>(Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.GetService<IViewModelOne>(Contract)).IsTypeOf<ViewModelOne>();
            await Assert.That(initializer.GetService<IViewModelOne>()).IsNull();
        }
    }

    /// <summary>The initializer's RegisterConstant with a contract should resolve the same instance under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterConstantWithContract_ResolvesUnderThatContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.RegisterConstant(instance, Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.GetService<ViewModelOne>(Contract)).IsSameReferenceAs(instance);
            await Assert.That(initializer.GetService<ViewModelOne>()).IsNull();
        }
    }

    /// <summary>The initializer's RegisterLazySingleton with a contract should resolve the same instance every time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterLazySingletonWithContract_ResolvesSameInstance()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.RegisterLazySingleton(static () => new ViewModelOne(), Contract);

        var first = initializer.GetService<ViewModelOne>(Contract);
        var second = initializer.GetService<ViewModelOne>(Contract);

        using (Assert.Multiple())
        {
            await Assert.That(first).IsNotNull();
            await Assert.That(second).IsSameReferenceAs(first);
            await Assert.That(initializer.GetService<ViewModelOne>()).IsNull();
        }
    }

    /// <summary>The initializer's GetServices with a contract should return every registration made under it, in order.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_GetServicesWithContract_ReturnsEveryContractRegistration()
    {
        using var initializer = new SimpleInjectorInitializer();
        var first = new ViewModelOne();
        var second = new ViewModelOne();
        initializer.Register<IViewModelOne>(() => first, Contract);
        initializer.Register<IViewModelOne>(() => second, Contract);

        var services = initializer.GetServices(typeof(IViewModelOne), Contract).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(ExpectedContractServiceCount);
            await Assert.That(services[0]).IsSameReferenceAs(first);
            await Assert.That(services[1]).IsSameReferenceAs(second);
        }
    }

    /// <summary>The initializer's non-generic UnregisterAll with a contract should leave the contract-less registrations alone.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByTypeWithContract_LeavesContractlessRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne), Contract);

        initializer.UnregisterAll(typeof(IViewModelOne), Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne), Contract)).IsFalse();
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne))).IsTrue();
        }
    }

    /// <summary>The initializer's generic UnregisterAll with a contract should remove only that contract's registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllGenericWithContract_RemovesOnlyThatContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>(LeftContract);
        initializer.Register<IViewModelOne, ViewModelOne>(RightContract);

        initializer.UnregisterAll<IViewModelOne>(LeftContract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration<IViewModelOne>(LeftContract)).IsFalse();
            await Assert.That(initializer.HasRegistration<IViewModelOne>(RightContract)).IsTrue();
        }
    }

    /// <summary>A null contract on the initializer means "no contract" and must register into the contract-less table.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterWithNullContract_RegistersWithoutContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register((Func<object?>)(() => instance), typeof(IViewModelOne), null);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration(typeof(IViewModelOne), null)).IsTrue();
            await Assert.That(initializer.GetService(typeof(IViewModelOne), null)).IsSameReferenceAs(instance);
            await Assert.That(initializer.GetServices(typeof(IViewModelOne), null)).IsNotEmpty();
        }
    }

    /// <summary>A contract registration under a null service type should round-trip through the NullServiceType wrapper.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterByNullTypeWithContract_ResolvesNullServiceTypeWrapper()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register((Func<object?>)(() => instance), (Type?)null, Contract);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration((Type?)null, Contract)).IsTrue();
            await Assert.That(initializer.GetService((Type?)null, Contract)).IsTypeOf<NullServiceType>();
            await Assert.That(initializer.GetServices((Type?)null, Contract)).IsNotEmpty();
        }
    }

    /// <summary>The initializer's generic UnregisterAll with a null contract should remove the contract-less registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllGenericWithNullContract_RemovesContractlessRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register<IViewModelOne, ViewModelOne>();

        initializer.UnregisterAll<IViewModelOne>(null);

        await Assert.That(initializer.HasRegistration<IViewModelOne>()).IsFalse();
    }

    /// <summary>The initializer's non-generic UnregisterAll with a contract should remove registrations made under a null service type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByNullTypeWithContract_RemovesContractRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), (Type?)null, Contract);

        initializer.UnregisterAll((Type?)null, Contract);

        await Assert.That(initializer.HasRegistration((Type?)null, Contract)).IsFalse();
    }

    /// <summary>The initializer's generic reads with a null contract must fall through to the contract-less lookup.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_ReadGenericWithNullContract_DelegatesToContractlessLookup()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register<IViewModelOne>(() => instance);

        using (Assert.Multiple())
        {
            await Assert.That(initializer.HasRegistration<IViewModelOne>(null)).IsTrue();
            await Assert.That(initializer.GetService<IViewModelOne>(null)).IsSameReferenceAs(instance);
            await Assert.That(initializer.GetServices<IViewModelOne>(null)).IsNotEmpty();
        }
    }

    /// <summary>The initializer's generic Register with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterGenericWithNullContract_RegistersWithoutContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.Register<IViewModelOne>(() => instance, null);

        await Assert.That(initializer.GetService<IViewModelOne>()).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's service/implementation Register with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterServiceImplementationWithNullContract_RegistersWithoutContract()
    {
        using var initializer = new SimpleInjectorInitializer();

        initializer.Register<IViewModelOne, ViewModelOne>(null);

        await Assert.That(initializer.GetService<IViewModelOne>()).IsTypeOf<ViewModelOne>();
    }

    /// <summary>The initializer's RegisterConstant with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterConstantWithNullContract_RegistersWithoutContract()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();

        initializer.RegisterConstant(instance, null);

        await Assert.That(initializer.GetService<ViewModelOne>()).IsSameReferenceAs(instance);
    }

    /// <summary>The initializer's RegisterLazySingleton with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterLazySingletonWithNullContract_RegistersWithoutContract()
    {
        using var initializer = new SimpleInjectorInitializer();

        initializer.RegisterLazySingleton(static () => new ViewModelOne(), null);

        var first = initializer.GetService<ViewModelOne>();
        var second = initializer.GetService<ViewModelOne>();

        await Assert.That(first).IsSameReferenceAs(second);
    }

    /// <summary>The initializer's non-generic UnregisterAll with a null contract must remove the contract-less registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_UnregisterAllByTypeWithNullContract_RemovesContractlessRegistrations()
    {
        using var initializer = new SimpleInjectorInitializer();
        initializer.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne));

        initializer.UnregisterAll(typeof(IViewModelOne), null);

        await Assert.That(initializer.HasRegistration(typeof(IViewModelOne))).IsFalse();
    }

    /// <summary>The contract registrations staged on the initializer must survive the handover to the resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_InheritsContractRegistrationsFromInitializer()
    {
        using var initializer = new SimpleInjectorInitializer();
        var instance = new ViewModelOne();
        initializer.Register<IViewModelOne>(() => instance, Contract);
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), initializer);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration<IViewModelOne>(Contract)).IsTrue();
            await Assert.That(resolver.GetService<IViewModelOne>(Contract)).IsSameReferenceAs(instance);
        }
    }

    /// <summary>A contract registration on the resolver must not answer a contract-less lookup.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ContractRegistration_IsInvisibleToContractlessLookup()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.Register((Func<object?>)(static () => new ViewModelOne()), typeof(IViewModelOne), Contract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IViewModelOne))).IsFalse();
            await Assert.That(resolver.GetService(typeof(IViewModelOne))).IsNull();
        }
    }

    /// <summary>A registration made straight against the container must not answer a contract lookup.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_ContainerRegistration_IsInvisibleToContractLookup()
    {
        var container = new Container();
        container.RegisterSingleton<IScreen, MockScreen>();
        using var resolver = new SimpleInjectorDependencyResolver(container, new SimpleInjectorInitializer());

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IScreen), Contract)).IsFalse();
            await Assert.That(resolver.HasRegistration<IScreen>(Contract)).IsFalse();
            await Assert.That(resolver.GetService(typeof(IScreen), Contract)).IsNull();
            await Assert.That(resolver.GetService<IScreen>(Contract)).IsNull();
            await Assert.That(resolver.GetServices(typeof(IScreen), Contract)).IsEmpty();
            await Assert.That(resolver.GetServices<IScreen>(Contract)).IsEmpty();
        }
    }

    /// <summary>The resolver must keep two implementations registered under different contracts apart.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_DifferentContracts_ResolveDistinctServices()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var left = new ViewModelOne();
        var right = new ViewModelOne();

        resolver.Register<IViewModelOne>(() => left, LeftContract);
        resolver.Register<IViewModelOne>(() => right, RightContract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetService<IViewModelOne>(LeftContract)).IsSameReferenceAs(left);
            await Assert.That(resolver.GetService<IViewModelOne>(RightContract)).IsSameReferenceAs(right);
        }
    }

    /// <summary>The resolver's non-generic Register with a contract should resolve under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterByTypeWithContract_ResolvesUnderThatContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.Register((Func<object?>)(() => instance), typeof(IViewModelOne), Contract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IViewModelOne), Contract)).IsTrue();
            await Assert.That(resolver.GetService(typeof(IViewModelOne), Contract)).IsSameReferenceAs(instance);
        }
    }

    /// <summary>The resolver's service/implementation Register with a contract should resolve under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterServiceImplementationWithContract_ResolvesUnderThatContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.Register<IViewModelOne, ViewModelOne>(Contract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetService<IViewModelOne>(Contract)).IsTypeOf<ViewModelOne>();
            await Assert.That(resolver.HasRegistration(typeof(IViewModelOne))).IsFalse();
        }
    }

    /// <summary>The resolver's RegisterConstant with a contract should resolve the same instance under that contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterConstantWithContract_ResolvesUnderThatContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.RegisterConstant(instance, Contract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetService<ViewModelOne>(Contract)).IsSameReferenceAs(instance);
            await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsFalse();
        }
    }

    /// <summary>The resolver's RegisterLazySingleton with a contract should resolve the same instance every time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterLazySingletonWithContract_ResolvesSameInstance()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        resolver.RegisterLazySingleton(static () => new ViewModelOne(), Contract);

        var first = resolver.GetService<ViewModelOne>(Contract);
        var second = resolver.GetService<ViewModelOne>(Contract);

        using (Assert.Multiple())
        {
            await Assert.That(first).IsNotNull();
            await Assert.That(second).IsSameReferenceAs(first);
        }
    }

    /// <summary>The resolver's GetServices with a contract should return every registration made under it, in order.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServicesWithContract_ReturnsEveryContractRegistration()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var first = new ViewModelOne();
        var second = new ViewModelOne();
        resolver.Register<IViewModelOne>(() => first, Contract);
        resolver.Register<IViewModelOne>(() => second, Contract);

        var services = resolver.GetServices<IViewModelOne>(Contract).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(ExpectedContractServiceCount);
            await Assert.That(services[0]).IsSameReferenceAs(first);
            await Assert.That(services[1]).IsSameReferenceAs(second);
        }
    }

    /// <summary>The resolver's contract lookups should return nothing when the contract was never registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_GetServiceWithUnknownContract_ReturnsNull()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        resolver.Register<IViewModelOne>(static () => new ViewModelOne(), LeftContract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetService<IViewModelOne>(RightContract)).IsNull();
            await Assert.That(resolver.GetService(typeof(IViewModelOne), RightContract)).IsNull();
        }
    }

    /// <summary>The resolver's contract registrations should reject a null factory instead of accepting it silently.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterWithContract_NullFactory_Throws()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        using (Assert.Multiple())
        {
            await Assert.That(() => resolver.Register(null!, typeof(IViewModelOne), Contract)).Throws<ArgumentNullException>();
            await Assert.That(() => resolver.Register((Func<IViewModelOne?>)null!, Contract)).Throws<ArgumentNullException>();
            await Assert.That(() => resolver.RegisterLazySingleton((Func<ViewModelOne?>)null!, Contract)).Throws<ArgumentNullException>();
            await Assert.That(() => resolver.RegisterConstant((ViewModelOne?)null, Contract)).Throws<ArgumentNullException>();
        }
    }

    /// <summary>A null contract on the resolver means "no contract" and must register into the container.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterWithNullContract_RegistersWithoutContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.Register((Func<object?>)(() => instance), typeof(IViewModelOne), null);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(IViewModelOne), null)).IsTrue();
            await Assert.That(resolver.GetService(typeof(IViewModelOne), null)).IsSameReferenceAs(instance);
            await Assert.That(resolver.GetServices(typeof(IViewModelOne), null)).IsNotEmpty();
        }
    }

    /// <summary>The resolver's generic registrations with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterGenericWithNullContract_RegistersWithoutContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.Register<IViewModelOne>(() => instance, null);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration<IViewModelOne>(null)).IsTrue();
            await Assert.That(resolver.GetService<IViewModelOne>(null)).IsSameReferenceAs(instance);
            await Assert.That(resolver.GetServices<IViewModelOne>(null)).IsNotEmpty();
        }
    }

    /// <summary>The resolver's constant and singleton registrations with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterConstantWithNullContract_RegistersWithoutContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.RegisterConstant(instance, null);

        await Assert.That(resolver.GetService<ViewModelOne>()).IsSameReferenceAs(instance);
    }

    /// <summary>The resolver's lazy singleton with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterLazySingletonWithNullContract_RegistersWithoutContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.RegisterLazySingleton(static () => new ViewModelOne(), null);

        var first = resolver.GetService<ViewModelOne>();
        var second = resolver.GetService<ViewModelOne>();

        await Assert.That(first).IsSameReferenceAs(second);
    }

    /// <summary>The resolver's service/implementation registration with a null contract must land on the contract-less path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterServiceImplementationWithNullContract_RegistersWithoutContract()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());

        resolver.Register<IViewModelOne, ViewModelOne>(null);

        await Assert.That(resolver.GetService<IViewModelOne>()).IsTypeOf<ViewModelOne>();
    }

    /// <summary>A contract registration on the resolver under a null service type should round-trip through the NullServiceType wrapper.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorDependencyResolver_RegisterByNullTypeWithContract_ResolvesNullServiceTypeWrapper()
    {
        using var resolver = new SimpleInjectorDependencyResolver(new Container(), new SimpleInjectorInitializer());
        var instance = new ViewModelOne();

        resolver.Register((Func<object?>)(() => instance), (Type?)null, Contract);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration((Type?)null, Contract)).IsTrue();
            await Assert.That(resolver.GetService((Type?)null, Contract)).IsTypeOf<NullServiceType>();
            await Assert.That(resolver.GetServices((Type?)null, Contract)).IsNotEmpty();
        }
    }

    /// <summary>The initializer's contract registrations should reject a null factory instead of accepting it silently.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SimpleInjectorInitializer_RegisterWithContract_NullFactory_Throws()
    {
        using var initializer = new SimpleInjectorInitializer();

        using (Assert.Multiple())
        {
            await Assert.That(() => initializer.Register((Func<IViewModelOne?>)null!, Contract)).Throws<ArgumentNullException>();
            await Assert.That(() => initializer.RegisterLazySingleton((Func<ViewModelOne?>)null!, Contract)).Throws<ArgumentNullException>();
            await Assert.That(() => initializer.RegisterConstant((ViewModelOne?)null, Contract)).Throws<ArgumentNullException>();
        }
    }
}
