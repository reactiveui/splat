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
            var factories = RegisteredFactories[serviceType];
            return factories.Count == 0 ? null : factories[^1].Invoke()!;
        }
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract) =>
        GetService(serviceType); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
    public T? GetService<T>(string? contract) =>
        GetService<T>(); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var factories = RegisteredFactories[serviceType];
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
        GetServices(serviceType); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
    public IEnumerable<T> GetServices<T>(string? contract) =>
        GetServices<T>(); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        HasRegistration(serviceType); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        HasRegistration<T>(); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        Register(factory, serviceType); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        Register(factory); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        Register<TService, TImplementation>(); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        UnregisterAll(serviceType); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        UnregisterAll<T>(); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        RegisterConstant(value); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
        RegisterLazySingleton(valueFactory); // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract

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
