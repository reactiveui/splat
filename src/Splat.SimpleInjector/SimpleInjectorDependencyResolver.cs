// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>
/// Provides an implementation of the IDependencyResolver interface using a SimpleInjector container for dependency
/// resolution.
/// </summary>
/// <remarks>
/// <para>
/// This resolver adapts a SimpleInjector Container to the IDependencyResolver abstraction, enabling integration with
/// frameworks or components that expect this interface.
/// </para>
/// <para>
/// SimpleInjector has no keyed registrations, so a contract cannot be expressed inside the container. Contract
/// registrations are held beside the container instead and are only ever returned to a caller asking for the same
/// contract; a contract-less lookup never sees them, and two implementations registered under different contracts
/// stay distinct.
/// </para>
/// <para>
/// Registration goes straight into the container, so anything registered here is also injectable by SimpleInjector.
/// SimpleInjector locks its container on the first resolution, so a registration attempted after the first service
/// has been resolved fails with SimpleInjector's own exception. Register everything up front - through
/// <see cref="SimpleInjectorInitializer"/> or against the container - before resolving.
/// </para>
/// <para>
/// Service unregistration and registration callbacks are not supported, as SimpleInjector does not provide mechanisms
/// for removing registrations or observing registration events after initial configuration. The resolver manages the
/// lifetime of the underlying container and disposes it when the resolver is disposed.
/// </para>
/// </remarks>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "Generic parameter is the caller-supplied service/implementation type for these IDependencyResolver APIs and cannot become a method parameter without changing the contract.")]
public class SimpleInjectorDependencyResolver : IDependencyResolver
{
    /// <summary>The underlying SimpleInjector container used for registration and resolution.</summary>
    private readonly Container _container;

    /// <summary>The factories registered against a contract, which SimpleInjector itself cannot express.</summary>
    private readonly ContractRegistrations _contractFactories = new();

    /// <summary>Serializes access to <see cref="_collectionServiceTypes"/>.</summary>
    private readonly Lock _lockObject = new();

    /// <summary>The service types this resolver has added factories to the container's collection for.</summary>
    /// <remarks>SimpleInjector does not report a collection registration from <c>GetCurrentRegistrations</c> until
    /// the container has been locked, so these are tracked here to keep <see cref="HasRegistration(Type)"/> honest
    /// before the first resolution.</remarks>
    private readonly HashSet<Type> _collectionServiceTypes = [];

    /// <summary>Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.</summary>
    /// <param name="container">The container.</param>
    /// <param name="initializer">The initializer.</param>
    public SimpleInjectorDependencyResolver(Container container, SimpleInjectorInitializer initializer)
    {
        ArgumentExceptionHelper.ThrowIfNull(container);
        ArgumentExceptionHelper.ThrowIfNull(initializer);

        _container = container;
        RegisterFactories(initializer);
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        try
        {
            var registration = _container.GetRegistration(serviceType);
            if (registration is not null)
            {
                return registration.GetInstance();
            }

            object? last = null;
            foreach (var register in _container.GetAllInstances(serviceType))
            {
                last = register;
            }

            return last!;
        }
        catch (ActivationException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>
        contract is null
            ? GetService(serviceType)
            : _contractFactories.ResolveLast(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    public T? GetService<T>() =>
        (T?)GetService(typeof(T)); // SimpleInjector's generic methods require class constraint, so we always use the non-generic version

    /// <inheritdoc/>
    public T? GetService<T>(string? contract)
    {
        if (contract is null)
        {
            return GetService<T>();
        }

        var service = _contractFactories.ResolveLast(typeof(T), contract);
        return service is null ? default : (T?)service;
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        try
        {
            return _container.GetAllInstances(serviceType);
        }
        catch (ActivationException)
        {
            var registration = _container.GetRegistration(serviceType);
            return registration switch
            {
                not null => [registration.GetInstance()],
                _ => Array.Empty<object>()
            };
        }
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>
        contract is null
            ? GetServices(serviceType)
            : _contractFactories.ResolveAll(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    /// <remarks>SimpleInjector's generic methods require a class constraint, so the non-generic overload does the work.</remarks>
    public IEnumerable<T> GetServices<T>()
    {
        foreach (var service in GetServices(typeof(T)))
        {
            yield return (T)service;
        }
    }

    /// <inheritdoc/>
    /// <remarks>SimpleInjector's generic methods require a class constraint, so the non-generic overload does the work.</remarks>
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        foreach (var service in GetServices(typeof(T), contract))
        {
            yield return (T)service;
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (_collectionServiceTypes.Contains(serviceType))
            {
                return true;
            }
        }

        return Array.Exists(_container.GetCurrentRegistrations(), x => x.ServiceType == serviceType);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract) =>
        contract is null
            ? HasRegistration(serviceType)
            : _contractFactories.Contains(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    public bool HasRegistration<T>() =>
        HasRegistration(typeof(T));

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        contract is null ? HasRegistration<T>() : _contractFactories.Contains(typeof(T), contract);

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">The container has already resolved a service and can no longer be changed.</exception>
    public void Register(Func<object?> factory, Type? serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        AppendToCollection(
            serviceType,
            isNull
                ? () => new NullServiceType(factory)
                : factory);
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            Register(factory, serviceType);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(factory);

        var isNull = serviceType is null;

        _contractFactories.Add(
            serviceType ?? NullServiceType.CachedType,
            contract,
            () => isNull ? new NullServiceType(factory) : factory());
    }

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        Register(() => factory(), typeof(T));
    }

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract)
    {
        if (contract is null)
        {
            Register(factory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(factory);

        _contractFactories.Add(typeof(T), contract, () => factory());
    }

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() =>
        _container.Register<TService, TImplementation>();

    /// <inheritdoc/>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (contract is null)
        {
            Register<TService, TImplementation>();
            return;
        }

        _contractFactories.Add(typeof(TService), contract, static () => new TImplementation());
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) =>
        throw new NotSupportedException(
            "UnregisterCurrent is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not support removing individual registrations after they have been added.");

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract) =>
        throw new NotSupportedException(
            "UnregisterCurrent with contract is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not support removing individual registrations after they have been added.");

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() =>
        UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType) =>
        throw new NotSupportedException(
            "UnregisterAll is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not support removing registrations after they have been added.");

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract) =>
        throw new NotSupportedException(
            "UnregisterAll with contract is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not support removing registrations after they have been added.");

    /// <inheritdoc/>
    public void UnregisterAll<T>() =>
        UnregisterAll(typeof(T));

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotSupportedException(
            "ServiceRegistrationCallback is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not provide a mechanism for service registration callbacks.");

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotSupportedException(
            "ServiceRegistrationCallback with contract is not supported in the SimpleInjector dependency resolver. "
            + "SimpleInjector does not provide a mechanism for service registration callbacks.");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        _container.RegisterInstance(value);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterConstant(value);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(value);

        _contractFactories.Add(typeof(T), contract, () => value);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        _container.RegisterSingleton(() => valueFactory()!);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton(valueFactory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        _contractFactories.Add(typeof(T), contract, () => lazy.Value);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Disposes of the instance.</summary>
    /// <param name="disposing">Whether or not the instance is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _container.Dispose();
    }

    /// <summary>Registers the collection of factories captured by the initializer into the underlying container.</summary>
    /// <param name="initializer">The initializer containing the registered factories to apply to the container.</param>
    private void RegisterFactories(SimpleInjectorInitializer initializer)
    {
        foreach (var typeFactories in initializer.RegisteredFactories)
        {
            List<TransientSimpleInjectorRegistration> registrations = new(typeFactories.Value.Count);
            foreach (var factory in typeFactories.Value)
            {
                registrations.Add(new(_container, typeFactories.Key, factory));
            }

            _container.Collection.Register(typeFactories.Key, registrations);

            lock (_lockObject)
            {
                _ = _collectionServiceTypes.Add(typeFactories.Key);
            }
        }

        initializer.ContractFactories.CopyTo(_contractFactories);
    }

    /// <summary>Appends a factory to the container's collection for the supplied service type.</summary>
    /// <param name="serviceType">The service type the factory produces.</param>
    /// <param name="factory">The factory that produces the service.</param>
    private void AppendToCollection(Type serviceType, Func<object?> factory)
    {
        _container.Collection.Append(
            serviceType,
            new TransientSimpleInjectorRegistration(_container, serviceType, factory));

        lock (_lockObject)
        {
            _ = _collectionServiceTypes.Add(serviceType);
        }
    }
}
