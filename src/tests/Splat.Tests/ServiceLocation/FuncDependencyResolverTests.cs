// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Tests for the <see cref="FuncDependencyResolver"/> class.
/// </summary>
[NotInParallel]
[InheritsTests]
public class FuncDependencyResolverTests : BaseDependencyResolverTests<FuncDependencyResolver>
{
    private interface ITestInterface
    {
    }

    [Test]
    public async Task Constructor_ShouldAcceptMinimalParameters()
    {
        // Arrange & Act
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Assert
        await Assert.That(resolver).IsNotNull();
    }

    [Test]
    public async Task Constructor_ShouldAcceptAllParameters()
    {
        // Arrange
        var disposable = new TestDisposable();

        // Act
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { },
            unregisterCurrent: (_, _) => { },
            unregisterAll: (_, _) => { },
            toDispose: disposable);

        // Assert
        await Assert.That(resolver).IsNotNull();
        await Assert.That(disposable.IsDisposed).IsFalse();

        resolver.Dispose();
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    [Test]
    public async Task GetService_ShouldReturnLastService_WhenMultipleRegistered()
    {
        // Arrange
        var services = new List<object> { "first", "second", "third" };
        var resolver = new FuncDependencyResolver((_, _) => services);

        // Act
        var result = resolver.GetService<object>();

        // Assert
        await Assert.That(result).IsEqualTo("third");
    }

    [Test]
    public async Task GetService_ShouldReturnNull_WhenNoServicesRegistered()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act
        var result = resolver.GetService<string>();

        // Assert
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetService_WithContract_ShouldReturnLastService()
    {
        // Arrange
        var services = new List<object> { "first", "second" };
        var resolver = new FuncDependencyResolver((type, contract) =>
            contract == "test" ? services : []);

        // Act
        var result = resolver.GetService<object>("test");

        // Assert
        await Assert.That(result).IsEqualTo("second");
    }

    [Test]
    public async Task GetServices_ShouldReturnEmptyList_WhenGetAllServicesReturnsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => null!);

        // Act
        var result = resolver.GetServices<string>().ToList();

        // Assert
        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task GetServices_ShouldReturnAllServices()
    {
        // Arrange
        var services = new List<object> { "first", "second", "third" };
        var resolver = new FuncDependencyResolver((_, _) => services);

        // Act
        var result = resolver.GetServices<string>().ToList();

        // Assert
        await Assert.That(result.Count).IsEqualTo(3);
        await Assert.That(result[0]).IsEqualTo("first");
        await Assert.That(result[1]).IsEqualTo("second");
        await Assert.That(result[2]).IsEqualTo("third");
    }

    [Test]
    public async Task GetServices_WithContract_ShouldPassContractToGetAllServices()
    {
        // Arrange
        string? capturedContract = null;
        var resolver = new FuncDependencyResolver((_, contract) =>
        {
            capturedContract = contract;
            return [];
        });

        // Act
        _ = resolver.GetServices<string>("mycontract").ToList();

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    [Test]
    public async Task GetServices_WithNullServiceType_ShouldUseNullServiceType()
    {
        // Arrange
        Type? capturedType = null;
        var resolver = new FuncDependencyResolver((type, _) =>
        {
            capturedType = type;
            return [];
        });

        // Act
        _ = resolver.GetServices(serviceType: null).ToList();

        // Assert
        await Assert.That(capturedType).IsEqualTo(NullServiceType.CachedType);
    }

    [Test]
    public async Task HasRegistration_ShouldReturnTrue_WhenGetAllServicesReturnsNonNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act
        var result = resolver.HasRegistration<string>();

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task HasRegistration_ShouldReturnFalse_WhenGetAllServicesReturnsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => null!);

        // Act
        var result = resolver.HasRegistration<string>();

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task HasRegistration_WithContract_ShouldPassContractToGetAllServices()
    {
        // Arrange
        string? capturedContract = null;
        var resolver = new FuncDependencyResolver((_, contract) =>
        {
            capturedContract = contract;
            return [];
        });

        // Act
        _ = resolver.HasRegistration<string>("mycontract");

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    [Test]
    public async Task Register_ShouldThrowNotImplementedException_WhenRegisterDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.Register(() => "test", typeof(string)))
            .Throws<NotImplementedException>()
            .WithMessageContaining("Register is not implemented", StringComparison.Ordinal);
    }

    [Test]
    public async Task Register_ShouldCallRegisterDelegate_WhenProvided()
    {
        // Arrange
        Func<object?>? capturedFactory = null;
        Type? capturedType = null;
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedFactory = factory;
                capturedType = type;
                capturedContract = contract;
            });

        // Act
        resolver.Register(() => "test", typeof(string));

        // Assert
        await Assert.That(capturedFactory).IsNotNull();
        await Assert.That(capturedType).IsEqualTo(typeof(string));
        await Assert.That(capturedContract).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Register_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedContract = contract;
            });

        // Act
        resolver.Register(() => "test", typeof(string), "mycontract");

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    [Test]
    public async Task Register_WithNullServiceType_ShouldWrapInNullServiceType()
    {
        // Arrange
        object? capturedValue = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedValue = factory();
            });

        // Act
        resolver.Register(() => "test", serviceType: null);

        // Assert
        await Assert.That(capturedValue).IsNotNull();
        await Assert.That(capturedValue).IsTypeOf<NullServiceType>();
    }

    [Test]
    public async Task Register_Generic_ShouldThrowArgumentNullException_WhenFactoryIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        // Act & Assert
        await Assert.That(() => resolver.Register<string>(null!))
            .Throws<ArgumentNullException>();
    }

    [Test]
    public async Task RegisterConstant_ShouldRegisterValue()
    {
        // Arrange
        object? capturedValue = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedValue = factory();
            });

        var testValue = new TestClass();

        // Act
        resolver.RegisterConstant(testValue);

        // Assert
        await Assert.That(ReferenceEquals(capturedValue, testValue)).IsTrue();
    }

    [Test]
    public async Task RegisterLazySingleton_ShouldThrowArgumentNullException_WhenFactoryIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        // Act & Assert
        await Assert.That(() => resolver.RegisterLazySingleton<TestClass>(null!))
            .Throws<ArgumentNullException>();
    }

    [Test]
    public async Task RegisterLazySingleton_ShouldRegisterLazyValue()
    {
        // Arrange
        var callCount = 0;
        Func<object?>? capturedFactory = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => null!, // Return null to avoid triggering factories during registration callbacks
            register: (factory, type, contract) =>
            {
                capturedFactory = factory;
            });

        // Act
        resolver.RegisterLazySingleton<TestClass>(() =>
        {
            callCount++;
            return new();
        });

        // Assert - factory should be captured
        await Assert.That(capturedFactory).IsNotNull();

        // Call the factory twice to verify lazy singleton behavior
        var value1 = capturedFactory!();
        var value2 = capturedFactory();

        // Should only create once (lazy singleton)
        await Assert.That(callCount).IsEqualTo(1);
        await Assert.That(ReferenceEquals(value1, value2)).IsTrue();
    }

    [Test]
    public async Task UnregisterCurrent_ShouldThrowNotImplementedException_WhenDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.UnregisterCurrent<string>())
            .Throws<NotImplementedException>()
            .WithMessageContaining("UnregisterCurrent is not implemented", StringComparison.Ordinal);
    }

    [Test]
    public async Task UnregisterCurrent_ShouldCallDelegate_WhenProvided()
    {
        // Arrange
        Type? capturedType = null;
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            unregisterCurrent: (type, contract) =>
            {
                capturedType = type;
                capturedContract = contract;
            });

        // Act
        resolver.UnregisterCurrent<string>();

        // Assert
        await Assert.That(capturedType).IsEqualTo(typeof(string));
        await Assert.That(capturedContract).IsNull();
    }

    [Test]
    public async Task UnregisterCurrent_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            unregisterCurrent: (type, contract) =>
            {
                capturedContract = contract;
            });

        // Act
        resolver.UnregisterCurrent<string>("mycontract");

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    [Test]
    public async Task UnregisterAll_ShouldThrowNotImplementedException_WhenDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.UnregisterAll<string>())
            .Throws<NotImplementedException>();
    }

    [Test]
    public async Task UnregisterAll_ShouldCallDelegate_WhenProvided()
    {
        // Arrange
        Type? capturedType = null;
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            unregisterAll: (type, contract) =>
            {
                capturedType = type;
                capturedContract = contract;
            });

        // Act
        resolver.UnregisterAll<string>();

        // Assert
        await Assert.That(capturedType).IsEqualTo(typeof(string));
        await Assert.That(capturedContract).IsNull();
    }

    [Test]
    public async Task UnregisterAll_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            unregisterAll: (type, contract) =>
            {
                capturedContract = contract;
            });

        // Act
        resolver.UnregisterAll<string>("mycontract");

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    [Test]
    public async Task ServiceRegistrationCallback_ShouldRegisterCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        // Act
        var disposable = resolver.ServiceRegistrationCallback<string>(disp => callbackInvoked = true);

        // Register a service to trigger callbacks
        resolver.Register(() => "test", typeof(string));

        // Assert
        await Assert.That(callbackInvoked).IsTrue();
        await Assert.That(disposable).IsNotNull();
    }

    [Test]
    public async Task ServiceRegistrationCallback_ShouldThrowArgumentNullException_WhenCallbackIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.ServiceRegistrationCallback<string>(null!))
            .Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ServiceRegistrationCallback_Dispose_ShouldRemoveCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        var disposable = resolver.ServiceRegistrationCallback<string>(disp => callbackInvoked = true);

        // Act
        disposable.Dispose();
        resolver.Register(() => "test", typeof(string));

        // Assert - callback should not be invoked after disposal
        await Assert.That(callbackInvoked).IsFalse();
    }

    [Test]
    public async Task ServiceRegistrationCallback_WithContract_ShouldRegisterCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        // Act
        _ = resolver.ServiceRegistrationCallback<string>("mycontract", _ => callbackInvoked = true);
        resolver.Register(() => "test", typeof(string), "mycontract");

        // Assert
        await Assert.That(callbackInvoked).IsTrue();
    }

    [Test]
    public async Task ServiceRegistrationCallback_ShouldNotInvokeCallback_ForDifferentContract()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        // Act
        _ = resolver.ServiceRegistrationCallback<string>("contract1", _ => callbackInvoked = true);
        resolver.Register(() => "test", typeof(string), "contract2");

        // Assert - callback should not be invoked for different contract
        await Assert.That(callbackInvoked).IsFalse();
    }

    [Test]
    public async Task ServiceRegistrationCallback_ShouldRemoveCallbackAfterDisposal()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (_, _, _) => { });

        var callbackInvoked = false;
        var disposable = resolver.ServiceRegistrationCallback<string>(disp =>
        {
            callbackInvoked = true;
            disp.Dispose(); // Signal to remove this callback
        });

        // Act - first registration should invoke callback
        resolver.Register(() => "test1", typeof(string));
        var firstInvocation = callbackInvoked;

        callbackInvoked = false;

        // Second registration should not invoke callback (it was removed)
        resolver.Register(() => "test2", typeof(string));

        // Assert
        await Assert.That(firstInvocation).IsTrue();
        await Assert.That(callbackInvoked).IsFalse();
    }

    [Test]
    public async Task Dispose_ShouldDisposeInnerDisposable()
    {
        // Arrange
        var disposable = new TestDisposable();
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            toDispose: disposable);

        // Act
        resolver.Dispose();

        // Assert
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Dispose_ShouldBeIdempotent()
    {
        // Arrange
        var disposeCount = 0;
        var disposable = new TestDisposable(() => disposeCount++);
        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            toDispose: disposable);

        // Act
        resolver.Dispose();
        resolver.Dispose();
        resolver.Dispose();

        // Assert - should only dispose once
        await Assert.That(disposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Dispose_ShouldNotThrow_WhenInnerDisposableIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver((_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.Dispose()).ThrowsNothing();
    }

    /// <summary>
    /// FuncDependencyResolver is a lightweight wrapper that doesn't track instances.
    /// Service disposal is the responsibility of the external getAllServices function provider.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override Task Dispose_DisposesRegisteredServices() =>

        // FuncDependencyResolver doesn't track instances, so it can't dispose them
        Task.CompletedTask;

    [Test]
    public async Task Register_TwoTypeParameters_ShouldCreateNewInstance()
    {
        // Arrange
        Func<object?>? capturedFactory = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedFactory = factory;
            });

        // Act
        resolver.Register<ITestInterface, TestClass>();

        // Assert
        await Assert.That(capturedFactory).IsNotNull();
        var instance1 = capturedFactory!();
        var instance2 = capturedFactory();

        await Assert.That(instance1).IsTypeOf<TestClass>();
        await Assert.That(ReferenceEquals(instance1, instance2)).IsFalse();
    }

    [Test]
    public async Task Register_TwoTypeParametersWithContract_ShouldPassContract()
    {
        // Arrange
        string? capturedContract = null;

        var resolver = new FuncDependencyResolver(
            getAllServices: (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedContract = contract;
            });

        // Act
        resolver.Register<ITestInterface, TestClass>("mycontract");

        // Assert
        await Assert.That(capturedContract).IsEqualTo("mycontract");
    }

    /// <inheritdoc/>
    protected override FuncDependencyResolver GetDependencyResolver()
    {
        var services = new Dictionary<(Type? type, string contract), List<Func<object?>>>();

        return new(
            getAllServices: (type, contract) =>
            {
                type ??= NullServiceType.CachedType;

                // Normalize contract: null -> string.Empty
                contract ??= string.Empty;
                if (services.TryGetValue((type, contract), out var list) && list.Count > 0)
                {
                    return [.. list.Select(f => f()!)];
                }

                return null!; // Return null to indicate no services registered
            },
            register: (factory, type, contract) =>
            {
                type ??= NullServiceType.CachedType;

                // Normalize contract: null -> string.Empty
                contract ??= string.Empty;
                var key = (type, contract);
                if (!services.TryGetValue(key, out var value))
                {
                    value = [];
                    services[key] = value;
                }

                value.Add(factory);
            },
            unregisterCurrent: (type, contract) =>
            {
                type ??= NullServiceType.CachedType;

                // Normalize contract: null -> string.Empty
                contract ??= string.Empty;
                var key = (type, contract);
                if (services.TryGetValue(key, out var list) && list.Count > 0)
                {
                    list.RemoveAt(list.Count - 1);
                    if (list.Count == 0)
                    {
                        services.Remove(key);
                    }
                }
            },
            unregisterAll: (type, contract) =>
            {
                type ??= NullServiceType.CachedType;

                // Normalize contract: null -> string.Empty
                contract ??= string.Empty;
                services.Remove((type, contract));
            });
    }

    private sealed class TestClass : ITestInterface
    {
    }

    private sealed class TestDisposable : IDisposable
    {
        private readonly Action? _onDispose;

        public TestDisposable(Action? onDispose = null) => _onDispose = onDispose;

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                _onDispose?.Invoke();
            }
        }
    }
}
