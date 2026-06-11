// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="AppLocator"/> class.</summary>
[NotInParallel]
public sealed class AppLocatorTests
{
    private AppLocatorScope? _scope;

    /// <summary>Marker service interface used by the tests.</summary>
    private interface ITestService;

    /// <summary>Creates a fresh locator scope before each test.</summary>
    [Before(Test)]
    public void SetUp() => _scope = new();

    /// <summary>Disposes the locator scope after each test.</summary>
    [After(Test)]
    public void TearDown()
    {
        _scope?.Dispose();
        _scope = null;
    }

    /// <summary>Verifies that Current returns a non-null read-only resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Current_ShouldReturnReadonlyResolver()
    {
        var resolver = AppLocator.Current;

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    /// <summary>Verifies that CurrentMutable returns a non-null mutable resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CurrentMutable_ShouldReturnMutableResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    /// <summary>Verifies that GetLocator returns the current resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetLocator_ShouldReturnResolver()
    {
        var resolver = AppLocator.GetLocator();

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    /// <summary>Verifies that SetLocator throws when given a null resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_WithNullResolver_ShouldThrow() => await Assert.That(() => AppLocator.SetLocator(null!)).ThrowsExactly<ArgumentNullException>();

    /// <summary>Verifies that SetLocator updates the active resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SetLocator_ShouldUpdateResolver()
    {
        var newResolver = new FuncDependencyResolver(
            (_, _) => null!,
            (_, _, _) => { },
            (_, _) => { },
            (_, _) => { });

        AppLocator.SetLocator(newResolver);

        await Assert.That(AppLocator.GetLocator()).IsEqualTo(newResolver);
    }

    /// <summary>Verifies that Register registers a service factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_ShouldRegisterService()
    {
        var service = new TestService();
        AppLocator.Register(() => (ITestService)service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that Register with a contract registers a service factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithContract_ShouldRegisterService()
    {
        var service = new TestService();
        AppLocator.Register(() => (ITestService)service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that RegisterConstant registers a constant instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_ShouldRegisterInstance()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that RegisterConstant with a contract registers a constant instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_WithContract_ShouldRegisterInstance()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that RegisterLazySingleton defers creation until first resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_ShouldRegisterLazily()
    {
        var callCount = 0;
        AppLocator.RegisterLazySingleton(() =>
        {
            callCount++;
            return (ITestService)new TestService();
        });

        await Assert.That(callCount).IsEqualTo(0);

        var service1 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);

        var service2 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1); // Still 1, singleton
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterLazySingleton with a contract defers creation until first resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_WithContract_ShouldRegisterLazily()
    {
        var callCount = 0;
        AppLocator.RegisterLazySingleton(
            () =>
            {
                callCount++;
                return (ITestService)new TestService();
            },
            "test");

        await Assert.That(callCount).IsEqualTo(0);

        _ = AppLocator.GetService<ITestService>("test");
        await Assert.That(callCount).IsEqualTo(1);
    }

    /// <summary>Verifies that GetService returns the registered service.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_ShouldReturnRegisteredService()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that GetService with a contract returns the registered service.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithContract_ShouldReturnRegisteredService()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

    /// <summary>Verifies that GetService returns null when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithNoRegistration_ShouldReturnNull()
    {
        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsNull();
    }

    /// <summary>Verifies that GetServices returns all registered services.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_ShouldReturnAllRegisteredServices()
    {
        var service1 = new TestService();
        var service2 = new TestService();
        AppLocator.RegisterConstant<ITestService>(service1);
        AppLocator.RegisterConstant<ITestService>(service2);

        var services = AppLocator.GetServices<ITestService>();
        var serviceList = services.ToList();

        await Assert.That(serviceList.Count).IsEqualTo(2);
        await Assert.That(serviceList).Contains(service1);
        await Assert.That(serviceList).Contains(service2);
    }

    /// <summary>Verifies that GetServices with a contract returns the registered services.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithContract_ShouldReturnRegisteredServices()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service, "test");

        var services = AppLocator.GetServices<ITestService>("test");
        var serviceList = services.ToList();

        await Assert.That(serviceList.Count).IsEqualTo(1);
        await Assert.That(serviceList[0]).IsEqualTo(service);
    }

    /// <summary>Verifies that HasRegistration returns true when a service is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WithRegisteredService_ShouldReturnTrue()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService());

        var hasRegistration = AppLocator.HasRegistration<ITestService>();

        await Assert.That(hasRegistration).IsTrue();
    }

    /// <summary>Verifies that HasRegistration returns false when nothing is registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WithNoRegistration_ShouldReturnFalse()
    {
        var hasRegistration = AppLocator.HasRegistration<ITestService>();

        await Assert.That(hasRegistration).IsFalse();
    }

    /// <summary>Verifies that HasRegistration with a contract returns true when registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WithContract_ShouldReturnTrue()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");

        await Assert.That(hasRegistration).IsTrue();
    }

    /// <summary>Verifies that UnregisterCurrent removes the most recent registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_ShouldRemoveLastRegistration()
    {
        var service1 = new TestService();
        var service2 = new TestService();
        AppLocator.RegisterConstant<ITestService>(service1);
        AppLocator.RegisterConstant<ITestService>(service2);

        AppLocator.UnregisterCurrent<ITestService>();

        var resolved = AppLocator.GetService<ITestService>();
        await Assert.That(resolved).IsEqualTo(service1);
    }

    /// <summary>Verifies that UnregisterCurrent with a contract removes the most recent registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_WithContract_ShouldRemoveLastRegistration()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        AppLocator.UnregisterCurrent<ITestService>("test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");
        await Assert.That(hasRegistration).IsTrue();
    }

    /// <summary>Verifies that UnregisterAll removes every registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_ShouldRemoveAllRegistrations()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService());
        AppLocator.RegisterConstant<ITestService>(new TestService());

        AppLocator.UnregisterAll<ITestService>();

        var hasRegistration = AppLocator.HasRegistration<ITestService>();
        await Assert.That(hasRegistration).IsFalse();
    }

    /// <summary>Verifies that UnregisterAll with a contract removes every registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_WithContract_ShouldRemoveAllRegistrations()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        AppLocator.UnregisterAll<ITestService>("test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");
        await Assert.That(hasRegistration).IsFalse();
    }

    /// <summary>Verifies that RegisterResolverCallbackChanged throws when the callback is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterResolverCallbackChanged_WithNullCallback_ShouldThrow() => await Assert.That(() => AppLocator.RegisterResolverCallbackChanged(null!)).ThrowsExactly<ArgumentNullException>();

    /// <summary>Verifies that RegisterResolverCallbackChanged invokes the callback immediately.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterResolverCallbackChanged_ShouldInvokeImmediately()
    {
        var callbackInvoked = false;

        AppLocator.RegisterResolverCallbackChanged(() => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>Verifies that SuppressResolverCallbackChangedNotifications suppresses callbacks within the scope.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SuppressResolverCallbackChangedNotifications_ShouldSuppressCallbacks()
    {
        await Assert.That(AppLocator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();

        using (AppLocator.SuppressResolverCallbackChangedNotifications())
        {
            await Assert.That(AppLocator.AreResolverCallbackChangedNotificationsEnabled()).IsFalse();
        }

        await Assert.That(AppLocator.AreResolverCallbackChangedNotificationsEnabled()).IsTrue();
    }

    /// <summary>Verifies that resolver callback notifications are enabled by default.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AreResolverCallbackChangedNotificationsEnabled_ByDefault_ShouldBeTrue()
    {
        var enabled = AppLocator.AreResolverCallbackChangedNotificationsEnabled();

        await Assert.That(enabled).IsTrue();
    }

    /// <summary>Concrete test service implementation.</summary>
    private sealed class TestService : ITestService;
}
