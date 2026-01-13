// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="AppLocator"/> class.
/// </summary>
[NotInParallel]
public sealed class AppLocatorTests
{
    private AppLocatorScope? _scope;

    private interface ITestService
    {
    }

    [Before(HookType.Test)]
    public void SetUp() => _scope = new();

    [After(HookType.Test)]
    public void TearDown()
    {
        _scope?.Dispose();
        _scope = null;
    }

    [Test]
    public async Task Current_ShouldReturnReadonlyResolver()
    {
        var resolver = AppLocator.Current;

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    [Test]
    public async Task CurrentMutable_ShouldReturnMutableResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    [Test]
    public async Task GetLocator_ShouldReturnResolver()
    {
        var resolver = AppLocator.GetLocator();

        await Assert.That(resolver).IsNotNull();
        await Assert.That(resolver).IsTypeOf<InstanceGenericFirstDependencyResolver>();
    }

    [Test]
    public async Task SetLocator_WithNullResolver_ShouldThrow() => await Assert.That(() => AppLocator.SetLocator(null!)).ThrowsExactly<ArgumentNullException>();

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

    [Test]
    public async Task Register_ShouldRegisterService()
    {
        var service = new TestService();
        AppLocator.Register(() => (ITestService)service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    [Test]
    public async Task Register_WithContract_ShouldRegisterService()
    {
        var service = new TestService();
        AppLocator.Register(() => (ITestService)service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

    [Test]
    public async Task RegisterConstant_ShouldRegisterInstance()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    [Test]
    public async Task RegisterConstant_WithContract_ShouldRegisterInstance()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

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

    [Test]
    public async Task GetService_ShouldReturnRegisteredService()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service);

        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsEqualTo(service);
    }

    [Test]
    public async Task GetService_WithContract_ShouldReturnRegisteredService()
    {
        var service = new TestService();
        AppLocator.RegisterConstant<ITestService>(service, "test");

        var resolved = AppLocator.GetService<ITestService>("test");

        await Assert.That(resolved).IsEqualTo(service);
    }

    [Test]
    public async Task GetService_WithNoRegistration_ShouldReturnNull()
    {
        var resolved = AppLocator.GetService<ITestService>();

        await Assert.That(resolved).IsNull();
    }

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

    [Test]
    public async Task HasRegistration_WithRegisteredService_ShouldReturnTrue()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService());

        var hasRegistration = AppLocator.HasRegistration<ITestService>();

        await Assert.That(hasRegistration).IsTrue();
    }

    [Test]
    public async Task HasRegistration_WithNoRegistration_ShouldReturnFalse()
    {
        var hasRegistration = AppLocator.HasRegistration<ITestService>();

        await Assert.That(hasRegistration).IsFalse();
    }

    [Test]
    public async Task HasRegistration_WithContract_ShouldReturnTrue()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");

        await Assert.That(hasRegistration).IsTrue();
    }

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

    [Test]
    public async Task UnregisterCurrent_WithContract_ShouldRemoveLastRegistration()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        AppLocator.UnregisterCurrent<ITestService>("test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");
        await Assert.That(hasRegistration).IsTrue();
    }

    [Test]
    public async Task UnregisterAll_ShouldRemoveAllRegistrations()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService());
        AppLocator.RegisterConstant<ITestService>(new TestService());

        AppLocator.UnregisterAll<ITestService>();

        var hasRegistration = AppLocator.HasRegistration<ITestService>();
        await Assert.That(hasRegistration).IsFalse();
    }

    [Test]
    public async Task UnregisterAll_WithContract_ShouldRemoveAllRegistrations()
    {
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");
        AppLocator.RegisterConstant<ITestService>(new TestService(), "test");

        AppLocator.UnregisterAll<ITestService>("test");

        var hasRegistration = AppLocator.HasRegistration<ITestService>("test");
        await Assert.That(hasRegistration).IsFalse();
    }

    [Test]
    public async Task RegisterResolverCallbackChanged_WithNullCallback_ShouldThrow() => await Assert.That(() => AppLocator.RegisterResolverCallbackChanged(null!)).ThrowsExactly<ArgumentNullException>();

    [Test]
    public async Task RegisterResolverCallbackChanged_ShouldInvokeImmediately()
    {
        var callbackInvoked = false;

        AppLocator.RegisterResolverCallbackChanged(() => callbackInvoked = true);

        await Assert.That(callbackInvoked).IsTrue();
    }

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

    [Test]
    public async Task AreResolverCallbackChangedNotificationsEnabled_ByDefault_ShouldBeTrue()
    {
        var enabled = AppLocator.AreResolverCallbackChangedNotificationsEnabled();

        await Assert.That(enabled).IsTrue();
    }

    private sealed class TestService : ITestService
    {
    }
}
