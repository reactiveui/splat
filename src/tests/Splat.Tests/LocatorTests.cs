// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;
using Splat.Tests.Mocks;

namespace Splat.Tests;

/// <summary>Tests for the <see cref="Locator"/>.</summary>
[NotInParallel]
public class LocatorTests
{
    /// <summary>The number of registered dummy services in the multi-registration tests.</summary>
    private const int RegisteredDummyCount = 3;

    /// <summary>The number of dummy services remaining after one is unregistered.</summary>
    private const int RemainingDummyCount = 2;

    /// <summary>The app locator scope used to isolate each test.</summary>
    private AppLocatorScope? _appLocatorScope;

    /// <summary>Setup method to initialize AppLocatorScope before each test.</summary>
    [Before(HookType.Test)]
    public void SetUpAppLocatorScope() => _appLocatorScope = new();

    /// <summary>Teardown method to dispose AppLocatorScope after each test.</summary>
    [After(HookType.Test)]
    public void TearDownAppLocatorScope()
    {
        _appLocatorScope?.Dispose();
        _appLocatorScope = null;
    }

    /// <summary>Verifies that <see cref="Locator.SetLocator"/> replaces the resolver returned by <see cref="Locator.GetLocator"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_ReplacesCurrentResolver()
    {
        using var resolver = new ModernDependencyResolver();

        Locator.SetLocator(resolver);

        await Assert.That(Locator.GetLocator()).IsSameReferenceAs(resolver);
    }

    /// <summary>Verifies that <see cref="Locator.SuppressResolverCallbackChangedNotifications"/> disables notifications until disposed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SuppressResolverCallbackChangedNotifications_DisablesNotificationsUntilDisposed()
    {
        await Assert.That(Locator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();

        using (Locator.SuppressResolverCallbackChangedNotifications())
        {
            await Assert.That(Locator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();
        }

        await Assert.That(Locator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();
    }

    /// <summary>Should the resolve nulls.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Can_Register_And_Resolve_Null_Types()
    {
        using var container = new InternalLocator();

        const int foo = 5;

        // Explicitly cast to call the non-generic Register method with null service type
        container.CurrentMutable.Register(static () => foo, serviceType: null);

        const int bar = 4;
        const string contract = "foo";
        container.CurrentMutable.Register(static () => bar, serviceType: null, contract: contract);

        await Assert.That(container.CurrentMutable.HasRegistration(null)).IsTrue();
        var value = container.Current.GetService(null);
        using (Assert.Multiple())
        {
            await Assert.That(value).IsEqualTo(foo);

            await Assert.That(container.CurrentMutable.HasRegistration(null, contract)).IsTrue();
        }

        value = container.Current.GetService(null, contract);
        await Assert.That(value).IsEqualTo(bar);

        var values = container.Current.GetServices(null);
        await Assert.That(values.Count()).IsEqualTo(1);

        container.CurrentMutable.UnregisterCurrent(null);
        var valuesNC = container.Current.GetServices(null).ToList();
        await Assert.That(valuesNC).IsEmpty();
        var valuesC = container.Current.GetServices(null, contract);
        await Assert.That(valuesC.Count()).IsEqualTo(1);

        container.CurrentMutable.UnregisterAll(null);
        valuesNC = [.. container.Current.GetServices(null)];
        await Assert.That(valuesNC).IsEmpty();

        container.CurrentMutable.UnregisterAll(null, contract);
        valuesC = container.Current.GetServices(null, contract);
        await Assert.That(valuesC.Count()).IsEqualTo(0);
    }

    /// <summary>Tests if the registrations are not empty on no external registrations.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_RegistrationsNotEmptyNoRegistrations()
    {
        // this is using the internal constructor and the Type-based GetService overload
        using var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService(typeof(ILogManager));
        var logger = testLocator.Current.GetService(typeof(ILogger));

        using (Assert.Multiple())
        {
            await Assert.That(logManager).IsNotNull();
            await Assert.That(logger).IsNotNull();
        }

        using (Assert.Multiple())
        {
            await Assert.That(logger).IsTypeOf<DebugLogger>();
            await Assert.That(logManager).IsTypeOf<DefaultLogManager>();
        }
    }

    /// <summary>Tests that if we use a contract it returns null entries for that type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_ContractRegistrationsNullNoRegistration()
    {
        using var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService(typeof(ILogManager), "test");
        var logger = testLocator.Current.GetService(typeof(ILogger), "test");

        using (Assert.Multiple())
        {
            await Assert.That(logManager).IsNull();
            await Assert.That(logger).IsNull();
        }
    }

    /// <summary>Tests that if we use a contract it returns null entries for that type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_ContractRegistrationsExtensionMethodsNullNoRegistration()
    {
        using var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService<ILogManager>("test");
        var logger = testLocator.Current.GetService<ILogger>("test");

        using (Assert.Multiple())
        {
            await Assert.That(logManager).IsNull();
            await Assert.That(logger).IsNull();
        }
    }

    /// <summary>Tests using the extension methods that the retrieving of the default InitializeSplat() still work.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_ExtensionMethodsNotNull()
    {
        using var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService<ILogManager>();
        var logger = testLocator.Current.GetService<ILogger>();

        using (Assert.Multiple())
        {
            await Assert.That(logManager).IsNotNull();
            await Assert.That(logger).IsNotNull();
        }

        using (Assert.Multiple())
        {
            await Assert.That(logger).IsTypeOf<DebugLogger>();
            await Assert.That(logManager).IsTypeOf<DefaultLogManager>();
        }
    }

    /// <summary>Tests to make sure that the locator's fire the resolver changed notifications.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithoutSuppress_NotificationsHappen()
    {
        var testLocator = new InternalLocator();
        var originalLocator = testLocator.Internal;

        var numberNotifications = 0;
        void NotificationAction() => Interlocked.Increment(ref numberNotifications);

        _ = testLocator.RegisterResolverCallbackChanged(NotificationAction);

        testLocator.SetLocator(new ModernDependencyResolver());
        testLocator.SetLocator(new ModernDependencyResolver());

        // 1 for the callback being immediately called when registered, plus 2 for the two SetLocator changes.
        const int immediateCallbackCount = 1;
        const int setLocatorChangeCount = 2;
        await Assert.That(numberNotifications).IsEqualTo(immediateCallbackCount + setLocatorChangeCount);

        testLocator.SetLocator(originalLocator);
    }

    /// <summary>Tests to make sure that the locator's don't fire the resolver changed notifications if they are suppressed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithSuppression_NotificationsDontHappen()
    {
        var testLocator = new InternalLocator();
        var originalLocator = testLocator.Internal;

        using (testLocator.SuppressResolverCallbackChangedNotifications())
        {
            var numberNotifications = 0;
            void NotificationAction() => Interlocked.Increment(ref numberNotifications);

            _ = testLocator.RegisterResolverCallbackChanged(NotificationAction);

            testLocator.SetLocator(new ModernDependencyResolver());
            testLocator.SetLocator(new ModernDependencyResolver());

            await Assert.That(numberNotifications).IsEqualTo(0);

            testLocator.SetLocator(originalLocator);
        }
    }

    /// <summary>Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_NotificationsDontHappen()
    {
        var numberNotifications = 0;
        void NotificationAction() => Interlocked.Increment(ref numberNotifications);

        var testLocator = new InternalLocator();
        _ = testLocator.RegisterResolverCallbackChanged(NotificationAction);

        var outerResolver = testLocator.Internal.WithResolver();
        var innerResolver = testLocator.Internal.WithResolver();
        innerResolver.Dispose();
        outerResolver.Dispose();

        // 1 due to the fact the callback is called when we register.
        await Assert.That(numberNotifications).IsEqualTo(1);
    }

    /// <summary>Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_NotificationsNotSuppressedHappen()
    {
        var numberNotifications = 0;
        void NotificationAction() => Interlocked.Increment(ref numberNotifications);

        _ = Locator.RegisterResolverCallbackChanged(NotificationAction);

        var outerResolver = Locator.GetLocator().WithResolver(false);
        var innerResolver = Locator.GetLocator().WithResolver(false);
        innerResolver.Dispose();
        outerResolver.Dispose();

        // 1 for the callback being called immediately when we register, then each WithResolver(false)
        // scope fires twice (once for the change to the resolver and once for the change back).
        const int immediateCallbackCount = 1;
        const int notificationsPerScope = 2;
        const int scopeCount = 2;
        await Assert.That(numberNotifications).IsEqualTo(immediateCallbackCount + (notificationsPerScope * scopeCount));
    }

    /// <summary>Tests to make sure that the unregister all functions correctly. This is a test when there are values registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterAll_WithValuesWorks()
    {
        using var currentMutable = new ModernDependencyResolver();

        var dummy1 = new DummyObjectClass1();
        var dummy2 = new DummyObjectClass2();
        var dummy3 = new DummyObjectClass3();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterConstant<IDummyInterface>(dummy1, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy2, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy3, testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract).ToList();

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(RegisteredDummyCount);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
                await Assert.That(items).Contains(dummy3);
            }

            currentMutable.UnregisterAll<IDummyInterface>(testContract);

            items = [.. currentMutable.GetServices<IDummyInterface>(testContract)];

            await Assert.That(items).IsEmpty();
        }
    }

    /// <summary>Nullables the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndResolveANullableTypeWithValue()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(static () => new());
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        await Assert.That(doc).IsTypeOf<DummyObjectClass1>();
    }

    /// <summary>Nullables the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndResolveANullableTypeWithNull()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(static () => null);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        await Assert.That(doc).IsNull();
    }

    /// <summary>Test that GetService throws ObjectDisposedException after disposal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndResolveANullableTypeWithValueLocatorDisposed()
    {
        var currentMutable = new ModernDependencyResolver();
        currentMutable.Register<DummyObjectClass1?>(static () => new());
        currentMutable.Dispose();

        await Assert.That(() => currentMutable.GetService<DummyObjectClass1?>())
            .Throws<ObjectDisposedException>();
    }

    /// <summary>Nullables the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndResolveANullableTypeWithDefault()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(static () => default);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        await Assert.That(doc).IsNull();
    }

    /// <summary>Nullables the type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAndResolveANullableTypeWithNulledInstance()
    {
        const DummyObjectClass1? dummy = null;
        Locator.CurrentMutable.Register(static () => dummy);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        await Assert.That(doc).IsNull();
    }

    /// <summary>Tests to make sure that the unregister all functions correctly. This is a test when there are values not registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterAll_NoValuesWorks()
    {
        using var currentMutable = new ModernDependencyResolver();

        var items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();

        currentMutable.UnregisterAll<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();
    }

    /// <summary>Tests tomake sure that the unregister current functions correctly. This is a test when there are values registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterCurrent_WithValuesWorks()
    {
        var dummy1 = new DummyObjectClass1();
        var dummy2 = new DummyObjectClass2();
        var dummy3 = new DummyObjectClass3();

        using var currentMutable = new ModernDependencyResolver();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterConstant<IDummyInterface>(dummy1, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy2, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy3, testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract).ToList();

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(RegisteredDummyCount);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
                await Assert.That(items).Contains(dummy3);
            }

            currentMutable.UnregisterCurrent<IDummyInterface>(testContract);

            items = [.. currentMutable.GetServices<IDummyInterface>(testContract)];

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(RemainingDummyCount);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
            }
        }
    }

    /// <summary>Tests to make sure that the unregister all functions correctly. This is a test when there are values not registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterCurrent_NoValuesWorks()
    {
        using var currentMutable = new ModernDependencyResolver();
        var items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();

        currentMutable.UnregisterCurrent<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();
    }

    /// <summary>Tests to make sure that the unregister all functions correctly. This is a test when there are values not registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FuncDependencyResolver_UnregisterAll()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        using var currentMutable = new FuncDependencyResolver(
            static (_, _) => [],
            unregisterAll: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract!;
                type = passedType;
            });

        currentMutable.UnregisterAll<IDummyInterface>();
        using (Assert.Multiple())
        {
            // 'type' is a System.Type; compare to typeof(...)
            await Assert.That(type).IsEqualTo(typeof(IDummyInterface));
            await Assert.That(contract).IsNull();
            await Assert.That(unregisterAllCalled).IsTrue();
        }

        unregisterAllCalled = false;
        currentMutable.UnregisterAll<IEnableLogger>("test");
        using (Assert.Multiple())
        {
            await Assert.That(type).IsEqualTo(typeof(IEnableLogger));
            await Assert.That(contract).IsEqualTo("test");
            await Assert.That(unregisterAllCalled).IsTrue();
        }
    }

    /// <summary>Tests tomake sure that the unregister current functions correctly. This is a test when there are values registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FuncDependencyResolver_UnregisterCurrent()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        using var currentMutable = new FuncDependencyResolver(
            static (_, _) => [],
            unregisterCurrent: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract!;
                type = passedType;
            });

        currentMutable.UnregisterCurrent<IDummyInterface>();
        using (Assert.Multiple())
        {
            // 'type' is a System.Type; compare to typeof(...)
            await Assert.That(type).IsEqualTo(typeof(IDummyInterface));
            await Assert.That(contract).IsNull();
            await Assert.That(unregisterAllCalled).IsTrue();
        }

        unregisterAllCalled = false;
        currentMutable.UnregisterCurrent<IEnableLogger>("test");
        using (Assert.Multiple())
        {
            await Assert.That(type).IsEqualTo(typeof(IEnableLogger));
            await Assert.That(contract).IsEqualTo("test");
            await Assert.That(unregisterAllCalled).IsTrue();
        }
    }
}
