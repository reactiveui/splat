// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="ResolverMixins"/> class.</summary>
[NotInParallel]
public sealed class ResolverMixinsTests
{
    /// <summary>The locator scope created for the duration of each test.</summary>
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

    /// <summary>Verifies that RegisterAnd registers the service (TService) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TService_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterAnd<TestService>();

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<TestService>();
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

    /// <summary>Verifies that RegisterAnd registers the service (TService, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TService_WithContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterAnd<TestService>("test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<TestService>("test");
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

    /// <summary>Verifies that RegisterAnd registers the service (TService, WithFactory) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TService_WithFactory_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterAnd<ITestService>(() => instance);

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterAnd registers the service (TService, WithFactoryAndContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TService_WithFactoryAndContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterAnd<ITestService>(() => instance, "test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterAnd registers the service (TServiceTImplementation) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TServiceTImplementation_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterAnd<ITestService, TestService>();

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

    /// <summary>Verifies that RegisterAnd registers the service (TServiceTImplementation, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TServiceTImplementation_WithContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterAnd<ITestService, TestService>("test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

    /// <summary>Verifies that RegisterAnd registers the service (TServiceTImplementation, WithFactory) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TServiceTImplementation_WithFactory_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterAnd<ITestService, TestService>(() => instance);

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterAnd registers the service (TServiceTImplementation, WithFactoryAndContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_TServiceTImplementation_WithFactoryAndContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterAnd<ITestService, TestService>(() => instance, "test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TypeBased) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TypeBased_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterConstantAnd(instance, typeof(ITestService));

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TypeBased, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TypeBased_WithContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterConstantAnd(instance, typeof(ITestService), "test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TService) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TService_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterConstantAnd<TestService>();

        await Assert.That(result).IsEqualTo(resolver);
        var service1 = AppLocator.GetService<TestService>();
        var service2 = AppLocator.GetService<TestService>();
        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TService, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TService_WithContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterConstantAnd<TestService>("test");

        await Assert.That(result).IsEqualTo(resolver);
        var service1 = AppLocator.GetService<TestService>("test");
        var service2 = AppLocator.GetService<TestService>("test");
        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TService, WithValue) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TService_WithValue_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterConstantAnd<ITestService>(instance);

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>();
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterConstantAnd registers the service (TService, WithValueAndContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstantAnd_TService_WithValueAndContract_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var instance = new TestService();

        var result = resolver.RegisterConstantAnd<ITestService>(instance, "test");

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(service).IsEqualTo(instance);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TypeBased) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TypeBased_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        var result = resolver.RegisterLazySingletonAnd(
            () =>
            {
                callCount++;
                return new TestService();
            },
            typeof(ITestService));

        await Assert.That(result).IsEqualTo(resolver);
        await Assert.That(callCount).IsEqualTo(0);

        var service1 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);

        var service2 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TypeBased, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TypeBased_WithContract_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        var result = resolver.RegisterLazySingletonAnd(
            () =>
            {
                callCount++;
                return new TestService();
            },
            typeof(ITestService),
            "test");

        await Assert.That(result).IsEqualTo(resolver);
        await Assert.That(callCount).IsEqualTo(0);

        _ = AppLocator.GetService<ITestService>("test");
        await Assert.That(callCount).IsEqualTo(1);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TService) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TService_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterLazySingletonAnd<TestService>();

        await Assert.That(result).IsEqualTo(resolver);

        var service1 = AppLocator.GetService<TestService>();
        var service2 = AppLocator.GetService<TestService>();
        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TService, WithContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TService_WithContract_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterLazySingletonAnd<TestService>("test");

        await Assert.That(result).IsEqualTo(resolver);

        var service1 = AppLocator.GetService<TestService>("test");
        var service2 = AppLocator.GetService<TestService>("test");
        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TService, WithFactory) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TService_WithFactory_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        var result = resolver.RegisterLazySingletonAnd<ITestService>(
            () =>
            {
                callCount++;
                return new TestService();
            });

        await Assert.That(result).IsEqualTo(resolver);
        await Assert.That(callCount).IsEqualTo(0);

        var service1 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);
        await Assert.That(service1).IsNotNull();
        await Assert.That(service1).IsTypeOf<TestService>();

        var service2 = AppLocator.GetService<ITestService>();
        await Assert.That(callCount).IsEqualTo(1);
        await Assert.That(service2).IsEqualTo(service1);
    }

    /// <summary>Verifies that RegisterLazySingletonAnd registers lazily the service (TService, WithFactoryAndContract) and returns the resolver for chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingletonAnd_TService_WithFactoryAndContract_ShouldRegisterLazilyAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;
        var callCount = 0;

        var result = resolver.RegisterLazySingletonAnd<ITestService>(
            () =>
            {
                callCount++;
                return new TestService();
            },
            "test");

        await Assert.That(result).IsEqualTo(resolver);
        await Assert.That(callCount).IsEqualTo(0);

        var service = AppLocator.GetService<ITestService>("test");
        await Assert.That(callCount).IsEqualTo(1);
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

    /// <summary>Verifies that the RegisterAnd fluent API supports chaining.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterAnd_SupportsFluentChaining()
    {
        var resolver = AppLocator.CurrentMutable;

        resolver
            .RegisterAnd<TestService>()
            .RegisterConstantAnd<ITestService>(new TestService())
            .RegisterLazySingletonAnd<ITestService>(() => new TestService(), "lazy");

        var service1 = AppLocator.GetService<TestService>();
        var service2 = AppLocator.GetService<ITestService>();
        var service3 = AppLocator.GetService<ITestService>("lazy");

        await Assert.That(service1).IsNotNull();
        await Assert.That(service2).IsNotNull();
        await Assert.That(service3).IsNotNull();
    }

    /// <summary>Concrete test service implementation.</summary>
    private sealed class TestService : ITestService;
}
