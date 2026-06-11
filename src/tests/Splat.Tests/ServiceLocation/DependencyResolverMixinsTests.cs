// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="DependencyResolverMixins"/> class.</summary>
[NotInParallel]
public sealed class DependencyResolverMixinsTests
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

    /// <summary>Verifies that WithResolver throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WithResolver_WithNullResolver_ShouldThrow()
    {
        IDependencyResolver resolver = null!;

        await Assert.That(() => resolver.WithResolver()).ThrowsExactly<ArgumentNullException>();
    }

    /// <summary>Verifies that WithResolver temporarily overrides the active resolver.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that WithResolver suppresses callbacks when suppression is requested.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that WithResolver invokes callbacks when suppression is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that the type-based RegisterConstant throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_TypeBased_WithNullResolver_ShouldThrow()
    {
        IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.RegisterConstant(new TestService(), typeof(ITestService)))
            .ThrowsExactly<ArgumentNullException>();
    }

    /// <summary>Verifies that the type-based RegisterConstant registers a constant instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_TypeBased_ShouldRegisterInstance()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        resolver.RegisterConstant(instance, typeof(ITestService));

        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that the type-based RegisterConstant with a contract registers a constant instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_TypeBased_WithContract_ShouldRegisterInstance()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        resolver.RegisterConstant(instance, typeof(ITestService), "test");

        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that the type-based RegisterLazySingleton throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_TypeBased_WithNullResolver_ShouldThrow()
    {
        IMutableDependencyResolver resolver = null!;

        await Assert.That(() => resolver.RegisterLazySingleton(() => new TestService(), typeof(ITestService)))
            .ThrowsExactly<ArgumentNullException>();
    }

    /// <summary>Verifies that the type-based RegisterLazySingleton defers creation until first resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that the type-based RegisterLazySingleton with a contract defers creation until first resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Verifies that the type-based Register produces a new instance on each resolution.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

    /// <summary>Concrete test service implementation.</summary>
    private sealed class TestService : ITestService;
}
