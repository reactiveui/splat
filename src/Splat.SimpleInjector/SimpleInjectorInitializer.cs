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
/// service. Contract-based registrations are not supported; contract parameters are ignored. Thread safety is ensured
/// for all registration and resolution operations. This implementation is suitable for scenarios where lightweight,
/// in-memory dependency resolution is required without advanced features such as scopes or contract-based
/// resolution.</remarks>
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

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var fact = RegisteredFactories[serviceType].LastOrDefault();
            return fact?.Invoke()!;
        }
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetService(serviceType);

    /// <inheritdoc/>
    public T? GetService<T>()
    {
        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                return default;
            }

            var fact = factories.LastOrDefault();
            return fact is not null ? (T?)fact.Invoke() : default;
        }
    }

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetService<T>();

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            return RegisteredFactories[serviceType]
                .Select(static n => n()!);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetServices(serviceType);

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>()
    {
        lock (_lockObject)
        {
            return !RegisteredFactories.TryGetValue(typeof(T), out var factories) ? [] : factories.Select(static factory => (T)factory()!);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetServices<T>();

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

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        HasRegistration(serviceType);

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

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        HasRegistration<T>();

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
    public void Register(Func<object?> factory, Type? serviceType, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        Register(factory, serviceType);

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
    public void Register<T>(Func<T?> factory, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        Register(factory);

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
        where TImplementation : class, TService, new() =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        Register<TService, TImplementation>();

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
    public void UnregisterAll(Type? serviceType, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        UnregisterAll(serviceType);

    /// <inheritdoc/>
    public void UnregisterAll<T>()
    {
        lock (_lockObject)
        {
            _ = RegisteredFactories.Remove(typeof(T));
        }
    }

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        UnregisterAll<T>();

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
        where T : class =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        RegisterConstant(value);

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
        where T : class =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        RegisterLazySingleton(valueFactory);

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
