// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.SimpleInjector;

/// <summary>
/// Provides a simple dependency resolver implementation using a factory-based registration model compatible with Simple
/// Injector patterns.
/// </summary>
/// <remarks>This class enables registration and resolution of services by type, supporting multiple factories per
/// service. Contract registrations are held separately from the contract-less ones, so a contract registration is only
/// ever returned to a caller that asks for the same contract and never overwrites the contract-less registration for
/// the same service type. Thread safety is ensured for all registration and resolution operations. This implementation
/// is suitable for scenarios where lightweight, in-memory dependency resolution is required without advanced features
/// such as scopes.</remarks>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "Generic service-location API; the service type is supplied explicitly by callers, so type inference cannot apply by design.")]
[SuppressMessage(
    "StyleSharp",
    "SST1452:A generic type parameter is used only as a marker",
    Justification = "Generic marker API; the type parameter identifies the service and is applied via typeof(T) in the implementation.")]
public class SimpleInjectorInitializer : IDependencyResolver
{
    /// <summary>Serializes access to the registered-factory collection.</summary>
    private readonly Lock _lockObject = new();

    /// <summary>Gets dictionary of registered factories.</summary>
    public Dictionary<Type, List<Func<object?>>> RegisteredFactories { get; }
        = [];

    /// <summary>Gets the factories that were registered against a contract.</summary>
    /// <remarks>Simple Injector has no keyed registrations, so these are kept apart from <see cref="RegisteredFactories"/>
    /// and are handed to the resolver when the container is wired up.</remarks>
    internal ContractRegistrations ContractFactories { get; } = new();

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(serviceType, out var factories))
            {
                return null;
            }

            return factories.Count == 0 ? null : factories[^1].Invoke()!;
        }
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>
        contract is null
            ? GetService(serviceType)
            : ContractFactories.ResolveLast(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    public T? GetService<T>()
    {
        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                return default;
            }

            return factories.Count == 0 ? default : (T?)factories[^1].Invoke();
        }
    }

    /// <inheritdoc/>
    public T? GetService<T>(string? contract)
    {
        if (contract is null)
        {
            return GetService<T>();
        }

        var service = ContractFactories.ResolveLast(typeof(T), contract);
        return service is null ? default : (T?)service;
    }

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(serviceType, out var factories))
            {
                return [];
            }

            var services = new List<object>(factories.Count);
            foreach (var factory in factories)
            {
                services.Add(factory()!);
            }

            return services;
        }
    }

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>
        contract is null
            ? GetServices(serviceType)
            : ContractFactories.ResolveAll(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>()
    {
        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                return [];
            }

            var services = new List<T>(factories.Count);
            foreach (var factory in factories)
            {
                services.Add((T)factory()!);
            }

            return services;
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
            return RegisteredFactories.TryGetValue(serviceType, out var values)
                   && values.Count > 0;
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract) =>
        contract is null
            ? HasRegistration(serviceType)
            : ContractFactories.Contains(serviceType ?? NullServiceType.CachedType, contract);

    /// <inheritdoc/>
    public bool HasRegistration<T>()
    {
        lock (_lockObject)
        {
            return RegisteredFactories.TryGetValue(typeof(T), out var values)
                   && values.Count > 0;
        }
    }

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        contract is null ? HasRegistration<T>() : ContractFactories.Contains(typeof(T), contract);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var value = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(RegisteredFactories, serviceType, out _);
            value ??= [];
#else
            if (!RegisteredFactories.TryGetValue(serviceType, out var value))
            {
                value = [];
                RegisteredFactories.Add(serviceType, value);
            }
#endif

            value.Add(() =>
                isNull
                    ? new NullServiceType(factory)
                    : factory());
        }
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            Register(factory, serviceType);
            return;
        }

        var isNull = serviceType is null;

        ContractFactories.Add(
            serviceType ?? NullServiceType.CachedType,
            contract,
            () => isNull ? new NullServiceType(factory) : factory());
    }

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var value = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(RegisteredFactories, typeof(T), out _);
            value ??= [];
#else
            if (!RegisteredFactories.TryGetValue(typeof(T), out var value))
            {
                value = [];
                RegisteredFactories.Add(typeof(T), value);
            }
#endif

            value.Add(() => factory());
        }
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

        ContractFactories.Add(typeof(T), contract, () => factory());
    }

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var value = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(RegisteredFactories, typeof(TService), out _);
            value ??= [];
#else
            if (!RegisteredFactories.TryGetValue(typeof(TService), out var value))
            {
                value = [];
                RegisteredFactories.Add(typeof(TService), value);
            }
#endif

            value.Add(static () => new TImplementation());
        }
    }

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

        ContractFactories.Add(typeof(TService), contract, static () => new TImplementation());
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) => throw new NotSupportedException();

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() => throw new NotSupportedException();

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) => throw new NotSupportedException();

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            _ = RegisteredFactories.Remove(serviceType);
        }
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            UnregisterAll(serviceType);
            return;
        }

        ContractFactories.RemoveAll(serviceType ?? NullServiceType.CachedType, contract);
    }

    /// <inheritdoc/>
    public void UnregisterAll<T>()
    {
        lock (_lockObject)
        {
            _ = RegisteredFactories.Remove(typeof(T));
        }
    }

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract)
    {
        if (contract is null)
        {
            UnregisterAll<T>();
            return;
        }

        ContractFactories.RemoveAll(typeof(T), contract);
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) => throw new NotSupportedException();

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotSupportedException();

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        throw new NotSupportedException("Simple Injector does not support the Service Registration Callbacks");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        throw new NotSupportedException("Simple Injector does not support the Service Registration Callbacks");

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var factories = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(RegisteredFactories, typeof(T), out _);
            factories ??= [];
#else
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                factories = [];
                RegisteredFactories.Add(typeof(T), factories);
            }
#endif

            factories.Add(() => value);
        }
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

        ContractFactories.Add(typeof(T), contract, () => value);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var factories = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(RegisteredFactories, typeof(T), out _);
            factories ??= [];
#else
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                factories = [];
                RegisteredFactories.Add(typeof(T), factories);
            }
#endif

            factories.Add(() => lazy.Value);
        }
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

        ContractFactories.Add(typeof(T), contract, () => lazy.Value);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool isDisposing)
    {
    }
}
