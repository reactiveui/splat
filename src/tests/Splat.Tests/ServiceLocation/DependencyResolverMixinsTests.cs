// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="DependencyResolverMixins"/> class.
/// </summary>
[NotInParallel]
public sealed class DependencyResolverMixinsTests
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
    public async Task WithResolver_WithNullResolver_ShouldThrow()
    {
        IDependencyResolver resolver = null!;

        await Assert.That(() => resolver.WithResolver()).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task WithResolver_ShouldTemporarilyOverrideResolver()
    {
        var originalResolver = AppLocator.GetLocator();
        var testResolver = new InstanceGenericFirstDependencyResolver();

        using (testResolver.WithResolver())
        {
            await Assert.That(AppLocator.GetLocator()).IsEqualTo(testResolver);
        }

        await Assert.That(AppLocator.GetLocator()).IsEqualTo(originalResolver);
    }

    [Test]
    public async Task WithResolver_WithSuppressTrue_ShouldSuppressCallbacks()
    {
        var callbackInvoked = false;
        AppLocator.RegisterResolverCallbackChanged(() => callbackInvoked = true);
        callbackInvoked = false; // Reset

        var testResolver = new InstanceGenericFirstDependencyResolver();

        using (testResolver.WithResolver(suppressResolverCallback: true))
        {
            // Callback should not be invoked when suppressed
        }

        await Assert.That(callbackInvoked).IsFalse();
    }

    [Test]
    public async Task WithResolver_WithSuppressFalse_ShouldInvokeCallbacks()
    {
        var callbackInvoked = false;
        AppLocator.RegisterResolverCallbackChanged(() => callbackInvoked = true);
        callbackInvoked = false; // Reset

        var testResolver = new InstanceGenericFirstDependencyResolver();

        using (testResolver.WithResolver(suppressResolverCallback: false))
        {
            // Callback should be invoked
        }

        await Assert.That(callbackInvoked).IsTrue();
    }

    [Test]
    public async Task RegisterConstant_TypeBased_WithNullResolver_ShouldThrow()
    {
        IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.RegisterConstant(new TestService(), typeof(ITestService)))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task RegisterConstant_TypeBased_ShouldRegisterInstance()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        resolver.RegisterConstant(instance, typeof(ITestService));

        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    [Test]
    public async Task RegisterConstant_TypeBased_WithContract_ShouldRegisterInstance()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        resolver.RegisterConstant(instance, typeof(ITestService), "test");

        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    [Test]
    public async Task RegisterLazySingleton_TypeBased_WithNullResolver_ShouldThrow()
    {
        IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.RegisterLazySingleton(() => new TestService(), typeof(ITestService)))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task RegisterLazySingleton_TypeBased_ShouldRegisterLazily()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        resolver.RegisterLazySingleton(
            () =>
            {
                callCount++;
                return new TestService();
            },
            typeof(ITestService));

        await Assert.That(callCount).IsEqualTo(0);

        var service1 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);

        var service2 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1); // Still 1
        await Assert.That(service2).IsEqualTo(service1);
    }

    [Test]
    public async Task RegisterLazySingleton_TypeBased_WithContract_ShouldRegisterLazily()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        resolver.RegisterLazySingleton(
            () =>
            {
                callCount++;
                return new TestService();
            },
            typeof(ITestService),
            "test");

        await Assert.That(callCount).IsEqualTo(0);

        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(callCount).IsEqualTo(1);
    }

    [Test]
    public async Task Register_TypeBased_ShouldCreateNewInstanceEachTime()
    {
        var resolver = AppLocator.CurrentMutable;

        resolver.Register<ITestService, TestService>();

        var service1 = AppLocator.GetService<ITestService>();
        var service2 = AppLocator.GetService<ITestService>();

        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsNotNull();
        await Assert.That(ReferenceEquals(service1, service2)).IsFalse();
    }

    private sealed class TestService : ITestService
    {
    }
}
