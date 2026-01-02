// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="ResolverMixins"/> class.
/// </summary>
[NotInParallel]
public sealed class ResolverMixinsTests
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
    public async Task RegisterAnd_TService_ShouldRegisterAndReturnResolver()
    {
        var resolver = AppLocator.CurrentMutable;

        var result = resolver.RegisterAnd<TestService>();

        await Assert.That(result).IsEqualTo(resolver);
        var service = AppLocator.GetService<TestService>();
        await Assert.That(service).IsNotNull();
        await Assert.That(service).IsTypeOf<TestService>();
    }

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

    private sealed class TestService : ITestService
    {
    }
}
