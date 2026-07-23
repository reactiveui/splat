// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Tests for the <see cref="FuncDependencyResolver"/> class.</summary>
[NotInParallel]
[InheritsTests]
public class FuncDependencyResolverTests : BaseDependencyResolverTests<FuncDependencyResolver>
{
    /// <summary>Contract name used for the first registration in these tests.</summary>
    private const string First = "first";

    /// <summary>Contract name used for the second registration in these tests.</summary>
    private const string Second = "second";

    /// <summary>Contract name used for the third registration in these tests.</summary>
    private const string Third = "third";

    /// <summary>Contract name used when registering against a custom contract.</summary>
    private const string MyContract = "mycontract";

    /// <summary>The number of services expected after registering three factories.</summary>
    private const int ExpectedServiceCount = 3;

    /// <summary>Zero-based index of the third item.</summary>
    private const int ThirdIndex = 2;

    /// <summary>The value produced by a <see cref="NullServiceType"/>-wrapped registration once unwrapped.</summary>
    private const string UnwrappedValue = "unwrapped";

    /// <summary>A plain, non-wrapped service value.</summary>
    private const string PlainValue = "plain";

    /// <summary>The number of services yielded by the mixed wrapped/plain sequence.</summary>
    private const int MixedServiceCount = 2;

    /// <summary>Marker interface used by the tests.</summary>
    private interface ITestInterface;

    /// <summary>Verifies that the constructor accepts only the minimal required parameters.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ShouldAcceptMinimalParameters()
    {
        // Arrange & Act
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Assert
        await Assert.That(resolver).IsNotNull();
    }

    /// <summary>Verifies that the constructor accepts all optional delegates and the disposable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_ShouldAcceptAllParameters()
    {
        // Arrange
        var disposable = new TestDisposable();

        // Act
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { },
            unregisterCurrent: static (_, _) => { },
            unregisterAll: static (_, _) => { },
            toDispose: disposable);

        // Assert
        await Assert.That(resolver).IsNotNull();
        await Assert.That(disposable.IsDisposed).IsFalse();

        resolver.Dispose();
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    /// <summary>Verifies that GetService returns the last service when multiple are registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_ShouldReturnLastService_WhenMultipleRegistered()
    {
        // Arrange
        var services = new List<object> { First, Second, Third };
        using var resolver = new FuncDependencyResolver((_, _) => services);

        // Act
        var result = resolver.GetService<object>();

        // Assert
        await Assert.That(result).IsEqualTo(Third);
    }

    /// <summary>Verifies that GetService returns null when no services are registered.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_ShouldReturnNull_WhenNoServicesRegistered()
    {
        // Arrange
        using var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act
        var result = resolver.GetService<string>();

        // Assert
        await Assert.That(result).IsNull();
    }

    /// <summary>Verifies that GetService with a contract returns the last matching service.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_WithContract_ShouldReturnLastService()
    {
        // Arrange
        var services = new List<object> { First, Second };
        using var resolver = new FuncDependencyResolver((_, contract) =>
            contract == "test" ? services : []);

        // Act
        var result = resolver.GetService<object>("test");

        // Assert
        await Assert.That(result).IsEqualTo(Second);
    }

    /// <summary>Verifies that GetServices returns an empty list when the delegate returns null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_ShouldReturnEmptyList_WhenGetAllServicesReturnsNull()
    {
        // Arrange
        using var resolver = new FuncDependencyResolver(static (_, _) => null!);

        // Act
        var result = resolver.GetServices<string>().ToList();

        // Assert
        await Assert.That(result).IsEmpty();
    }

    /// <summary>Verifies that GetServices returns all registered services.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_ShouldReturnAllServices()
    {
        // Arrange
        var services = new List<object> { First, Second, Third };
        using var resolver = new FuncDependencyResolver((_, _) => services);

        // Act
        var result = resolver.GetServices<string>().ToList();

        // Assert
        await Assert.That(result.Count).IsEqualTo(ExpectedServiceCount);
        await Assert.That(result[0]).IsEqualTo(First);
        await Assert.That(result[1]).IsEqualTo(Second);
        await Assert.That(result[ThirdIndex]).IsEqualTo(Third);
    }

    /// <summary>Verifies that GetServices passes the contract through to the delegate.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithContract_ShouldPassContractToGetAllServices()
    {
        // Arrange
        string? capturedContract = null;
        using var resolver = new FuncDependencyResolver((_, contract) =>
        {
            capturedContract = contract;
            return [];
        });

        // Act
        _ = resolver.GetServices<string>(MyContract).ToList();

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>Verifies that GetServices substitutes the null service type when none is supplied.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithNullServiceType_ShouldUseNullServiceType()
    {
        // Arrange
        Type? capturedType = null;
        using var resolver = new FuncDependencyResolver((type, _) =>
        {
            capturedType = type;
            return [];
        });

        // Act
        _ = resolver.GetServices(serviceType: null).ToList();

        // Assert
        await Assert.That(capturedType).IsEqualTo(NullServiceType.CachedType);
    }

    /// <summary>Verifies that HasRegistration returns true when the delegate returns a non-null result.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_ShouldReturnTrue_WhenGetAllServicesReturnsNonNull()
    {
        // Arrange
        using var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act
        var result = resolver.HasRegistration<string>();

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>Verifies that HasRegistration returns false when the delegate returns null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_ShouldReturnFalse_WhenGetAllServicesReturnsNull()
    {
        // Arrange
        using var resolver = new FuncDependencyResolver(static (_, _) => null!);

        // Act
        var result = resolver.HasRegistration<string>();

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>Verifies that HasRegistration passes the contract through to the delegate.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_WithContract_ShouldPassContractToGetAllServices()
    {
        // Arrange
        string? capturedContract = null;
        using var resolver = new FuncDependencyResolver((_, contract) =>
        {
            capturedContract = contract;
            return [];
        });

        // Act
        _ = resolver.HasRegistration<string>(MyContract);

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>Verifies that Register throws NotSupportedException when no register delegate is provided.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_ShouldThrowNotSupportedException_WhenRegisterDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.Register(static () => "test", typeof(string)))
            .Throws<NotSupportedException>()
            .WithMessageContaining("Register is not supported", StringComparison.Ordinal);
    }

    /// <summary>Verifies that Register invokes the register delegate with the expected arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_ShouldCallRegisterDelegate_WhenProvided()
    {
        // Arrange
        Func<object?>? capturedFactory = null;
        Type? capturedType = null;
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (factory, type, contract) =>
            {
                capturedFactory = factory;
                capturedType = type;
                capturedContract = contract;
            });

        // Act
        resolver.Register(static () => "test", typeof(string));

        // Assert
        await Assert.That(capturedFactory).IsNotNull();
        await Assert.That(capturedType).IsEqualTo(typeof(string));
        await Assert.That(capturedContract).IsEqualTo(string.Empty);
    }

    /// <summary>Verifies that Register passes the contract through to the register delegate.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (_, _, contract) => capturedContract = contract);

        // Act
        resolver.Register(static () => "test", typeof(string), MyContract);

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>Verifies that Register wraps a null service type in the null service type.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithNullServiceType_ShouldWrapInNullServiceType()
    {
        // Arrange
        object? capturedValue = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (factory, _, _) => capturedValue = factory());

        // Act
        resolver.Register(static () => "test", serviceType: null);

        // Assert
        await Assert.That(capturedValue).IsNotNull();
        await Assert.That(capturedValue).IsTypeOf<NullServiceType>();
    }

    /// <summary>Verifies that the generic Register throws when the factory is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_Generic_ShouldThrowArgumentNullException_WhenFactoryIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        // Act & Assert
        await Assert.That(() => resolver.Register<string>(null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that RegisterConstant registers the supplied value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterConstant_ShouldRegisterValue()
    {
        // Arrange
        object? capturedValue = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (factory, _, _) => capturedValue = factory());

        var testValue = new TestClass();

        // Act
        resolver.RegisterConstant(testValue);

        // Assert
        await Assert.That(ReferenceEquals(capturedValue, testValue)).IsTrue();
    }

    /// <summary>Verifies that RegisterLazySingleton throws when the factory is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_ShouldThrowArgumentNullException_WhenFactoryIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        // Act & Assert
        await Assert.That(() => resolver.RegisterLazySingleton<TestClass>(null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that RegisterLazySingleton registers a factory that produces a single instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterLazySingleton_ShouldRegisterLazyValue()
    {
        // Arrange
        var callCount = 0;
        Func<object?>? capturedFactory = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => null!, // Return null to avoid triggering factories during registration callbacks
            register: (factory, _, _) => capturedFactory = factory);

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

    /// <summary>Verifies that UnregisterCurrent throws NotSupportedException when no delegate is provided.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_ShouldThrowNotSupportedException_WhenDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.UnregisterCurrent<string>())
            .Throws<NotSupportedException>()
            .WithMessageContaining("UnregisterCurrent is not supported", StringComparison.Ordinal);
    }

    /// <summary>Verifies that UnregisterCurrent invokes the delegate with the expected arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_ShouldCallDelegate_WhenProvided()
    {
        // Arrange
        Type? capturedType = null;
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
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

    /// <summary>Verifies that UnregisterCurrent passes the contract through to the delegate.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterCurrent_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            unregisterCurrent: (_, contract) => capturedContract = contract);

        // Act
        resolver.UnregisterCurrent<string>(MyContract);

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>Verifies that UnregisterAll throws NotSupportedException when no delegate is provided.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_ShouldThrowNotSupportedException_WhenDelegateIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.UnregisterAll<string>())
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that UnregisterAll invokes the delegate with the expected arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_ShouldCallDelegate_WhenProvided()
    {
        // Arrange
        Type? capturedType = null;
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
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

    /// <summary>Verifies that UnregisterAll passes the contract through to the delegate.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnregisterAll_WithContract_ShouldPassContractToDelegate()
    {
        // Arrange
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            unregisterAll: (_, contract) => capturedContract = contract);

        // Act
        resolver.UnregisterAll<string>(MyContract);

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>Verifies that ServiceRegistrationCallback registers a callback invoked on registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_ShouldRegisterCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        // Act
        var disposable = resolver.ServiceRegistrationCallback<string>(disp => callbackInvoked = true);

        // Register a service to trigger callbacks
        resolver.Register(static () => "test", typeof(string));

        // Assert
        await Assert.That(callbackInvoked).IsTrue();
        await Assert.That(disposable).IsNotNull();
    }

    /// <summary>Verifies that ServiceRegistrationCallback throws when the callback is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_ShouldThrowArgumentNullException_WhenCallbackIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.ServiceRegistrationCallback<string>(null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that disposing the subscription removes the registration callback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_Dispose_ShouldRemoveCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        var disposable = resolver.ServiceRegistrationCallback<string>(disp => callbackInvoked = true);

        // Act
        disposable.Dispose();
        resolver.Register(static () => "test", typeof(string));

        // Assert - callback should not be invoked after disposal
        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>Verifies that ServiceRegistrationCallback registers a callback for the given contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_WithContract_ShouldRegisterCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        // Act
        _ = resolver.ServiceRegistrationCallback<string>(MyContract, _ => callbackInvoked = true);
        resolver.Register(static () => "test", typeof(string), MyContract);

        // Assert
        await Assert.That(callbackInvoked).IsTrue();
    }

    /// <summary>Verifies that the registration callback is not invoked for a different contract.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_ShouldNotInvokeCallback_ForDifferentContract()
    {
        // Arrange
        var callbackInvoked = false;
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        // Act
        _ = resolver.ServiceRegistrationCallback<string>("contract1", _ => callbackInvoked = true);
        resolver.Register(static () => "test", typeof(string), "contract2");

        // Assert - callback should not be invoked for different contract
        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>Verifies that the registration callback is removed after it disposes itself.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallback_ShouldRemoveCallbackAfterDisposal()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: static (_, _, _) => { });

        var callbackInvoked = false;
        _ = resolver.ServiceRegistrationCallback<string>(disp =>
        {
            callbackInvoked = true;
            disp.Dispose(); // Signal to remove this callback
        });

        // Act - first registration should invoke callback
        resolver.Register(static () => "test1", typeof(string));
        var firstInvocation = callbackInvoked;

        callbackInvoked = false;

        // Second registration should not invoke callback (it was removed)
        resolver.Register(static () => "test2", typeof(string));

        // Assert
        await Assert.That(firstInvocation).IsTrue();
        await Assert.That(callbackInvoked).IsFalse();
    }

    /// <summary>Verifies that Dispose disposes the inner disposable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_ShouldDisposeInnerDisposable()
    {
        // Arrange
        var disposable = new TestDisposable();
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            toDispose: disposable);

        // Act
        resolver.Dispose();

        // Assert
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    /// <summary>Verifies that Dispose only disposes the inner disposable once.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_ShouldBeIdempotent()
    {
        // Arrange
        var disposeCount = 0;
        var disposable = new TestDisposable(() => disposeCount++);
        var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            toDispose: disposable);

        // Act
        resolver.Dispose();
        resolver.Dispose();
        resolver.Dispose();

        // Assert - should only dispose once
        await Assert.That(disposeCount).IsEqualTo(1);
    }

    /// <summary>Verifies that Dispose does not throw when there is no inner disposable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_ShouldNotThrow_WhenInnerDisposableIsNull()
    {
        // Arrange
        var resolver = new FuncDependencyResolver(static (_, _) => []);

        // Act & Assert
        await Assert.That(() => resolver.Dispose()).ThrowsNothing();
    }

    /// <summary>
    /// FuncDependencyResolver is a lightweight wrapper that doesn't track instances.
    /// Service disposal is the responsibility of the external getAllServices function provider.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public override async Task Dispose_DisposesRegisteredServices()
    {
        // FuncDependencyResolver does not track created instances, so disposing it must not
        // dispose services registered through it; disposal stays the responsibility of the
        // external getAllServices provider.
        var resolver = GetDependencyResolver();
        var disposableService = new DisposableTestService();
        resolver.RegisterConstant(disposableService);

        resolver.Dispose();

        await Assert.That(disposableService.IsDisposed).IsFalse();
    }

    /// <summary>Verifies that the two-type-parameter Register creates a new instance each time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_TwoTypeParameters_ShouldCreateNewInstance()
    {
        // Arrange
        Func<object?>? capturedFactory = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (factory, _, _) => capturedFactory = factory);

        // Act
        resolver.Register<ITestInterface, TestClass>();

        // Assert
        await Assert.That(capturedFactory).IsNotNull();
        var instance1 = capturedFactory!();
        var instance2 = capturedFactory();

        await Assert.That(instance1).IsTypeOf<TestClass>();
        await Assert.That(ReferenceEquals(instance1, instance2)).IsFalse();
    }

    /// <summary>Verifies that the two-type-parameter Register passes the contract through.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_TwoTypeParametersWithContract_ShouldPassContract()
    {
        // Arrange
        string? capturedContract = null;

        using var resolver = new FuncDependencyResolver(
            getAllServices: static (_, _) => [],
            register: (_, _, contract) => capturedContract = contract);

        // Act
        resolver.Register<ITestInterface, TestClass>(MyContract);

        // Assert
        await Assert.That(capturedContract).IsEqualTo(MyContract);
    }

    /// <summary>
    /// Verifies that resolving a null service type over a lazily-evaluated (non-collection) sequence unwraps
    /// <see cref="NullServiceType"/> markers by materializing a new list.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_WithNullServiceTypeOverNonCollection_UnwrapsMarkers()
    {
        // Arrange - the sequence is an iterator, so it is not an ICollection and takes the materializing unwrap path.
        using var resolver = new FuncDependencyResolver(static (_, _) => WrappedThenPlainServices());

        // Act
        var services = resolver.GetServices((Type?)null).ToList();

        // Assert - the wrapper is unwrapped to its factory value; the plain value passes through unchanged.
        using (Assert.Multiple())
        {
            await Assert.That(services.Count).IsEqualTo(MixedServiceCount);
            await Assert.That(services[0]).IsEqualTo(UnwrappedValue);
            await Assert.That(services[1]).IsEqualTo(PlainValue);
        }
    }

    /// <summary>Verifies that an exception thrown while disposing a created lazy singleton is suppressed on <see cref="FuncDependencyResolver.Dispose()"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenLazySingletonDisposalThrows_DoesNotPropagate()
    {
        // Arrange
        var resolver = GetDependencyResolver();
        resolver.RegisterLazySingleton(static () => new ThrowingDisposableService());

        // Force creation so the lazy value exists and is disposed during teardown.
        var service = resolver.GetService<ThrowingDisposableService>();
        await Assert.That(service).IsNotNull();

        // Act & Assert - the disposal exception from the created singleton must not escape.
        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();
    }

    /// <summary>Verifies that an exception thrown by a registered service during resolver disposal is suppressed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_WhenRegisteredServiceDisposalThrows_DoesNotPropagate()
    {
        // Arrange - the backing store surfaces a disposable that records the attempt, then throws.
        var throwing = new TestDisposable(static () => throw new InvalidOperationException("disposal failure"));
        var resolver = new FuncDependencyResolver((_, _) => new object[] { throwing });

        // Act & Assert
        await Assert.That(() =>
        {
            resolver.Dispose();
            return Task.CompletedTask;
        }).ThrowsNothing();

        await Assert.That(throwing.IsDisposed).IsTrue();
    }

    /// <inheritdoc/>
    protected override FuncDependencyResolver GetDependencyResolver()
    {
        var services = new Dictionary<(Type? type, string contract), List<Func<object?>>>();

        return new(
            getAllServices: (type, contract) => GetAllServices(services, type, contract),
            register: (factory, type, contract) => RegisterService(services, factory, type, contract),
            unregisterCurrent: (type, contract) => UnregisterCurrentService(services, type, contract),
            unregisterAll: (type, contract) => UnregisterAllServices(services, type, contract));
    }

    /// <summary>Normalizes a service type and contract into the dictionary key used by the backing store.</summary>
    /// <param name="type">The service type, or <see langword="null"/> for the null service type.</param>
    /// <param name="contract">The registration contract, or <see langword="null"/> for the default contract.</param>
    /// <returns>The normalized key.</returns>
    private static (Type Type, string Contract) NormalizeKey(Type? type, string? contract) =>
        (type ?? NullServiceType.CachedType, contract ?? string.Empty);

    /// <summary>Returns the materialized services registered for the given type and contract.</summary>
    /// <param name="services">The backing service store.</param>
    /// <param name="type">The service type.</param>
    /// <param name="contract">The registration contract.</param>
    /// <returns>The registered service instances, or <see langword="null"/> when none are registered.</returns>
    private static IEnumerable<object> GetAllServices(
        Dictionary<(Type? type, string contract), List<Func<object?>>> services,
        Type? type,
        string? contract)
    {
        var key = NormalizeKey(type, contract);
        return services.TryGetValue(key, out var list) && list.Count > 0 ? [.. list.Select(static f => f()!)] : null!;
    }

    /// <summary>Registers a factory for the given service type and contract.</summary>
    /// <param name="services">The backing service store.</param>
    /// <param name="factory">The factory that produces the service instance.</param>
    /// <param name="type">The service type.</param>
    /// <param name="contract">The registration contract.</param>
    private static void RegisterService(
        Dictionary<(Type? type, string contract), List<Func<object?>>> services,
        Func<object?> factory,
        Type? type,
        string? contract)
    {
        var key = NormalizeKey(type, contract);
#if NET6_0_OR_GREATER
        ref var value = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(services, key, out _);
        value ??= [];
#else
        if (!services.TryGetValue(key, out var value))
        {
            value = [];
            services[key] = value;
        }
#endif

        value.Add(factory);
    }

    /// <summary>Removes the most recently registered factory for the given service type and contract.</summary>
    /// <param name="services">The backing service store.</param>
    /// <param name="type">The service type.</param>
    /// <param name="contract">The registration contract.</param>
    private static void UnregisterCurrentService(
        Dictionary<(Type? type, string contract), List<Func<object?>>> services,
        Type? type,
        string? contract)
    {
        var key = NormalizeKey(type, contract);
        if (!services.TryGetValue(key, out var list) || list.Count == 0)
        {
            return;
        }

        list.RemoveAt(list.Count - 1);
        if (list.Count != 0)
        {
            return;
        }

        _ = services.Remove(key);
    }

    /// <summary>Removes all registered factories for the given service type and contract.</summary>
    /// <param name="services">The backing service store.</param>
    /// <param name="type">The service type.</param>
    /// <param name="contract">The registration contract.</param>
    private static void UnregisterAllServices(
        Dictionary<(Type? type, string contract), List<Func<object?>>> services,
        Type? type,
        string? contract) =>
        services.Remove(NormalizeKey(type, contract));

    /// <summary>
    /// Yields a <see cref="NullServiceType"/>-wrapped value followed by a plain value as a lazily evaluated
    /// (non-collection) sequence, forcing the null-service-type unwrap loop that materializes a new list.
    /// </summary>
    /// <returns>A non-materialized sequence of services.</returns>
    private static IEnumerable<object> WrappedThenPlainServices()
    {
        yield return new NullServiceType(static () => UnwrappedValue);
        yield return PlainValue;
    }

    /// <summary>Concrete implementation used as a test service.</summary>
    private sealed class TestClass : ITestInterface;

    /// <summary>Disposable test helper that records whether it has been disposed.</summary>
    private sealed class TestDisposable : IDisposable
    {
        /// <summary>The optional callback invoked when this instance is disposed.</summary>
        private readonly Action? _onDispose;

        /// <summary>Initializes a new instance of the <see cref="TestDisposable"/> class.</summary>
        /// <param name="onDispose">Optional callback invoked on disposal.</param>
        public TestDisposable(Action? onDispose = null) => _onDispose = onDispose;

        /// <summary>Gets a value indicating whether this instance has been disposed.</summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            _onDispose?.Invoke();
        }
    }
}
