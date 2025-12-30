// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests;

/// <summary>
/// Tests to confirm that the locator is working.
/// </summary>
[TestFixture]
[NonParallelizable]
public class LocatorTests
{
    /// <summary>
    /// Should the resolve nulls.
    /// </summary>
    [Test]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var container = new InternalLocator();

        const int foo = 5;
        container.CurrentMutable.Register(() => foo, null);

        const int bar = 4;
        const string contract = "foo";
        container.CurrentMutable.Register(() => bar, null, contract);

        Assert.That(container.CurrentMutable.HasRegistration(null), Is.True);
        var value = container.Current.GetService(null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(value, Is.EqualTo(foo));

            Assert.That(container.CurrentMutable.HasRegistration(null, contract), Is.True);
        }

        value = container.Current.GetService(null, contract);
        Assert.That(value, Is.EqualTo(bar));

        var values = container.Current.GetServices(null);
        Assert.That(values.Count(), Is.EqualTo(1));

        container.CurrentMutable.UnregisterCurrent(null);
        var valuesNC = container.Current.GetServices(null).ToList();
        Assert.That(valuesNC, Is.Empty);
        var valuesC = container.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.EqualTo(1));

        container.CurrentMutable.UnregisterAll(null);
        valuesNC = container.Current.GetServices(null).ToList();
        Assert.That(valuesNC, Is.Empty);

        container.CurrentMutable.UnregisterAll(null, contract);
        valuesC = container.Current.GetServices(null, contract);
        Assert.That(valuesC.Count(), Is.Zero);
    }

    /// <summary>
    /// Tests if the registrations are not empty on no external registrations.
    /// </summary>
    [Test]
    public void InitializeSplat_RegistrationsNotEmptyNoRegistrations()
    {
        // this is using the internal constructor
        var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService(typeof(ILogManager));
        var logger = testLocator.Current.GetService(typeof(ILogger));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logManager, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.TypeOf<DebugLogger>());
            Assert.That(logManager, Is.TypeOf<DefaultLogManager>());
        }
    }

    /// <summary>
    /// Tests that if we use a contract it returns null entries for that type.
    /// </summary>
    [Test]
    public void InitializeSplat_ContractRegistrationsNullNoRegistration()
    {
        var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService(typeof(ILogManager), "test");
        var logger = testLocator.Current.GetService(typeof(ILogger), "test");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logManager, Is.Null);
            Assert.That(logger, Is.Null);
        }
    }

    /// <summary>
    /// Tests that if we use a contract it returns null entries for that type.
    /// </summary>
    [Test]
    public void InitializeSplat_ContractRegistrationsExtensionMethodsNullNoRegistration()
    {
        var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService<ILogManager>("test");
        var logger = testLocator.Current.GetService<ILogger>("test");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logManager, Is.Null);
            Assert.That(logger, Is.Null);
        }
    }

    /// <summary>
    /// Tests using the extension methods that the retrieving of the default InitializeSplat() still work.
    /// </summary>
    [Test]
    public void InitializeSplat_ExtensionMethodsNotNull()
    {
        var testLocator = new InternalLocator();
        var logManager = testLocator.Current.GetService<ILogManager>();
        var logger = testLocator.Current.GetService<ILogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logManager, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.TypeOf<DebugLogger>());
            Assert.That(logManager, Is.TypeOf<DefaultLogManager>());
        }
    }

    /// <summary>
    /// Tests to make sure that the locator's fire the resolver changed notifications.
    /// </summary>
    [Test]
    public void WithoutSuppress_NotificationsHappen()
    {
        var testLocator = new InternalLocator();
        var originalLocator = testLocator.Internal;

        var numberNotifications = 0;
        void NotificationAction() => numberNotifications++;

        testLocator.RegisterResolverCallbackChanged(NotificationAction);

        testLocator.SetLocator(new ModernDependencyResolver());
        testLocator.SetLocator(new ModernDependencyResolver());

        // 2 for the changes, 1 for the callback being immediately called.
        Assert.That(numberNotifications, Is.EqualTo(3));

        testLocator.SetLocator(originalLocator);
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if they are suppressed.
    /// </summary>
    [Test]
    public void WithSuppression_NotificationsDontHappen()
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

            Assert.That(numberNotifications, Is.Zero);

            testLocator.SetLocator(originalLocator);
        }
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
    /// </summary>
    [Test]
    public void WithResolver_NotificationsDontHappen()
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
        Assert.That(numberNotifications, Is.EqualTo(1));
    }

    /// <summary>
    /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
    /// </summary>
    [Test]
    public void WithResolver_NotificationsNotSuppressedHappen()
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
        Assert.That(numberNotifications, Is.EqualTo(5));
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    [Test]
    public void ModernDependencyResolver_UnregisterAll_WithValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();

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
            var items = currentMutable.GetServices<IDummyInterface>(testContract);

            Assert.That(items, Is.EquivalentTo(new IDummyInterface[] { dummy1, dummy2, dummy3 }));

            currentMutable.UnregisterAll<IDummyInterface>(testContract);

            items = currentMutable.GetServices<IDummyInterface>(testContract);

            Assert.That(items, Is.Empty);
        }
    }

    /// <summary>
    /// Nullables the type.
    /// </summary>
    [Test]
    public void RegisterAndResolveANullableTypeWithValue()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(() => new());
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        Assert.That(doc, Is.TypeOf<DummyObjectClass1>());
    }

    /// <summary>
    /// Nullables the type.
    /// </summary>
    [Test]
    public void RegisterAndResolveANullableTypeWithNull()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(() => null);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        Assert.That(doc, Is.Null);
    }

    /// <summary>
    /// Nullables the type.
    /// </summary>
    [Test]
    public void RegisterAndResolveANullableTypeWithValueLocatorDisposed()
    {
        var currentMutable = new ModernDependencyResolver();
        currentMutable.Register<DummyObjectClass1?>(() => new());
        currentMutable.Dispose();
        var doc = currentMutable.GetService<DummyObjectClass1?>();
        Assert.That(doc, Is.Null);
    }

    /// <summary>
    /// Nullables the type.
    /// </summary>
    [Test]
    public void RegisterAndResolveANullableTypeWithDefault()
    {
        Locator.CurrentMutable.Register<DummyObjectClass1?>(() => null);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        Assert.That(doc, Is.Null);
    }

    /// <summary>
    /// Nullables the type.
    /// </summary>
    [Test]
    public void RegisterAndResolveANullableTypeWithNulledInstance()
    {
        DummyObjectClass1? dummy = null;
        Locator.CurrentMutable.Register(() => dummy);
        var doc = Locator.Current.GetService<DummyObjectClass1?>();
        Assert.That(doc, Is.Null);
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    [Test]
    public void ModernDependencyResolver_UnregisterAll_NoValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();

        var items = currentMutable.GetServices<IDummyInterface>();

        Assert.That(items, Is.Empty);

        currentMutable.UnregisterAll<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        Assert.That(items, Is.Empty);
    }

    /// <summary>
    /// Tests tomake sure that the unregister current functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    [Test]
    public void ModernDependencyResolver_UnregisterCurrent_WithValuesWorks()
    {
        var dummy1 = new DummyObjectClass1();
        var dummy2 = new DummyObjectClass2();
        var dummy3 = new DummyObjectClass3();

        var currentMutable = new ModernDependencyResolver();

        var testContracts = new[] { string.Empty, "test" };

        foreach (var testContract in testContracts)
        {
            currentMutable.RegisterConstant<IDummyInterface>(dummy1, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy2, testContract);
            currentMutable.RegisterConstant<IDummyInterface>(dummy3, testContract);
        }

        foreach (var testContract in testContracts)
        {
            var items = currentMutable.GetServices<IDummyInterface>(testContract);

            Assert.That(items, Is.EquivalentTo(new IDummyInterface[] { dummy1, dummy2, dummy3 }));

            currentMutable.UnregisterCurrent<IDummyInterface>(testContract);

            items = currentMutable.GetServices<IDummyInterface>(testContract);

            Assert.That(items, Is.EquivalentTo(new IDummyInterface[] { dummy1, dummy2 }));
        }
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    [Test]
    public void ModernDependencyResolver_UnregisterCurrent_NoValuesWorks()
    {
        var currentMutable = new ModernDependencyResolver();
        var items = currentMutable.GetServices<IDummyInterface>();

        Assert.That(items, Is.Empty);

        currentMutable.UnregisterCurrent<IDummyInterface>();

        items = currentMutable.GetServices<IDummyInterface>();

        Assert.That(items, Is.Empty);
    }

    /// <summary>
    /// Tests to make sure that the unregister all functions correctly.
    /// This is a test when there are values not registered.
    /// </summary>
    [Test]
    public void FuncDependencyResolver_UnregisterAll()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        var currentMutable = new FuncDependencyResolver(
            (_, _) => [],
            unregisterAll: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract!;
                type = passedType;
            });

        currentMutable.UnregisterAll<IDummyInterface>();
        using (Assert.EnterMultipleScope())
        {
            // 'type' is a System.Type; compare to typeof(...)
            Assert.That(type, Is.EqualTo(typeof(IDummyInterface)));
            Assert.That(contract, Is.Null);
            Assert.That(unregisterAllCalled, Is.True);
        }

        unregisterAllCalled = false;
        currentMutable.UnregisterAll<IEnableLogger>("test");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(type, Is.EqualTo(typeof(IEnableLogger)));
            Assert.That(contract, Is.EqualTo("test"));
            Assert.That(unregisterAllCalled, Is.True);
        }
    }

    /// <summary>
    /// Tests tomake sure that the unregister current functions correctly.
    /// This is a test when there are values registered.
    /// </summary>
    [Test]
    public void FuncDependencyResolver_UnregisterCurrent()
    {
        var unregisterAllCalled = false;
        Type? type = null;
        string? contract = null;

        var currentMutable = new FuncDependencyResolver(
            (_, _) => [],
            unregisterCurrent: (passedType, passedContract) =>
            {
                unregisterAllCalled = true;
                contract = passedContract!;
                type = passedType;
            });

        currentMutable.UnregisterCurrent<IDummyInterface>();
        using (Assert.EnterMultipleScope())
        {
            // 'type' is a System.Type; compare to typeof(...)
            Assert.That(type, Is.EqualTo(typeof(IDummyInterface)));
            Assert.That(contract, Is.Null);
            Assert.That(unregisterAllCalled, Is.True);
        }

        unregisterAllCalled = false;
        currentMutable.UnregisterCurrent<IEnableLogger>("test");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(type, Is.EqualTo(typeof(IEnableLogger)));
            Assert.That(contract, Is.EqualTo("test"));
            Assert.That(unregisterAllCalled, Is.True);
        }
    }
}
