// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;
using Splat.Tests.Mocks;

namespace Splat.Tests;

[NotInParallel]
public class LocatorSerialRegisterTests
{
    private InternalLocatorScope? _locatorScope;

    /// <summary>
    /// Setup method to initialize InternalLocatorScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpLocatorScope() => _locatorScope = new();

    /// <summary>
    /// Teardown method to dispose InternalLocatorScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownLocatorScope()
    {
        _locatorScope?.Dispose();
        _locatorScope = null;
    }

    /// <summary>
    /// Tests if the registrations are not empty on no external registrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_RegistrationsNotEmptyNoRegistrations()
    {
        // this is using the internal constructor
        var testLocator = new InternalLocator();
        testLocator.CurrentMutable.InitializeSplat();
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

    /// <summary>
    /// Tests that if we use a contract it returns null entries for that type.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_ContractRegistrationsNullNoRegistration()
    {
        var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService<ILogManager>("test");
        var logger = testLocator.Current.GetService<ILogger>("test");

        using (Assert.Multiple())
        {
            await Assert.That(logManager).IsNull();
            await Assert.That(logger).IsNull();
        }
    }

    /// <summary>
    /// Tests using the extension methods that the retrieving of the default InitializeSplat() still work.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InitializeSplat_ExtensionMethodsNotNull()
    {
        var testLocator = new InternalLocator();
        testLocator.CurrentMutable.InitializeSplat();
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

    /// <summary>
    /// Tests to make sure that the locator's fire the resolver changed notifications.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithoutSuppress_NotificationsHappen()
    {
        var testLocator = new InternalLocator();
        var originalLocator = testLocator.Internal;

        var numberNotifications = 0;
        void NotificationAction() => numberNotifications++;

        testLocator.RegisterResolverCallbackChanged(NotificationAction);

        testLocator.SetLocator(new ModernDependencyResolver());
        testLocator.SetLocator(new ModernDependencyResolver());

        // 2 for the changes, 1 for the callback being immediately called.
        await Assert.That(numberNotifications).IsEqualTo(3);

        testLocator.SetLocator(originalLocator);
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if they are suppressed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithSuppression_NotificationsDontHappen()
    {
        var testLocator = new InternalLocator();
        var originalLocator = testLocator.Internal;

        using (testLocator.SuppressResolverCallbackChangedNotifications())
        {
            var numberNotifications = 0;
            void NotificationAction() => numberNotifications++;

            testLocator.RegisterResolverCallbackChanged(NotificationAction);

            testLocator.SetLocator(new ModernDependencyResolver());
            testLocator.SetLocator(new ModernDependencyResolver());

            await Assert.That(numberNotifications).IsEqualTo(0);

            testLocator.SetLocator(originalLocator);
        }
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_NotificationsDontHappen()
    {
        var numberNotifications = 0;
        void NotificationAction() => numberNotifications++;

        var testLocator = new InternalLocator();
        testLocator.RegisterResolverCallbackChanged(NotificationAction);

        using (testLocator.Internal.WithResolver())
        using (testLocator.Internal.WithResolver())
        {
        }

        // 1 due to the fact the callback is called when we register.
        await Assert.That(numberNotifications).IsEqualTo(1);
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_NotificationsNotSuppressedHappen()
    {
        var numberNotifications = 0;
        void NotificationAction() => numberNotifications++;

        Locator.RegisterResolverCallbackChanged(NotificationAction);

        using (Locator.GetLocator().WithResolver(false))
        using (Locator.GetLocator().WithResolver(false))
        {
        }

        // 1 due to the fact the callback is called when we register.
        // 2 for, 1 for change to resolver, 1 for change back
        // 2 for, 1 for change to resolver, 1 for change back
        await Assert.That(numberNotifications).IsEqualTo(5);
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterAll_WithValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();

        var dummy1 = new DummyObjectClass1();
        var dummy2 = new DummyObjectClass2();
        var dummy3 = new DummyObjectClass3();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterConstantAnd<IDummyInterface>(dummy1, testContract)
                .RegisterConstantAnd<IDummyInterface>(dummy2, testContract)
                .RegisterConstant<IDummyInterface>(dummy3, testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract).ToList();

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(3);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
                await Assert.That(items).Contains(dummy3);
            }

            currentMutable.UnregisterAll<IDummyInterface>(testContract);

            items = [.. currentMutable.GetServices<IDummyInterface>(testContract)];

            await Assert.That(items).IsEmpty();
        }
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterAll_NoValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();

        var items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();

        currentMutable.UnregisterAll<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();
    }

    /// <summary>
    /// Tests tomake sure that the unregister current functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_ConstantUnregisterCurrent_WithValuesWorks()
    {
        var dummy1 = new DummyObjectClass1();
        var dummy2 = new DummyObjectClass2();
        var dummy3 = new DummyObjectClass3();

        var currentMutable = new ModernDependencyResolver();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterConstantAnd<IDummyInterface>(dummy1, testContract)
                .RegisterConstantAnd<IDummyInterface>(dummy2, testContract)
                .RegisterConstant<IDummyInterface>(dummy3, testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract).ToList();

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(3);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
                await Assert.That(items).Contains(dummy3);
            }

            currentMutable.UnregisterCurrent<IDummyInterface>(testContract);

            items = [.. currentMutable.GetServices<IDummyInterface>(testContract)];

            using (Assert.Multiple())
            {
                await Assert.That(items).Count().IsEqualTo(2);
                await Assert.That(items).Contains(dummy1);
                await Assert.That(items).Contains(dummy2);
            }
        }
    }

    /// <summary>
    /// Tests tomake sure that the unregister current functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterCurrent_WithValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterAnd<IDummyInterface, DummyObjectClass1>(testContract)
                .RegisterAnd<IDummyInterface, DummyObjectClass2>(testContract)
                .Register<IDummyInterface, DummyObjectClass3>(testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract).ToList();

            await Assert.That(items).Count().IsEqualTo(3);

            currentMutable.UnregisterCurrent<IDummyInterface>(testContract);

            items = [.. currentMutable.GetServices<IDummyInterface>(testContract)];

            await Assert.That(items).Count().IsEqualTo(2);
        }
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModernDependencyResolver_UnregisterCurrent_NoValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();
        var items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();

        currentMutable.UnregisterCurrent<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        await Assert.That(items).IsEmpty();
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FuncDependencyResolver_UnregisterAll()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        var currentMutable = new FuncDependencyResolver(
            (_, _) => [],
            unregisterAll: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract;
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

    /// <summary>
    /// Tests tomake sure that the unregister current functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FuncDependencyResolver_UnregisterCurrent()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        var currentMutable = new FuncDependencyResolver(
            (_, _) => [],
            unregisterCurrent: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract;
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
