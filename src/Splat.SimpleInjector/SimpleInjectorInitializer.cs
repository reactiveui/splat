// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.SimpleInjector;

/// <summary>
/// Initializer for SimpleInjector dependency resolver.
/// </summary>
public class SimpleInjectorInitializer : IDependencyResolver
{
    private readonly object _lockObject = new();

    /// <summary>
    /// Gets dictionary of registered factories.
    /// </summary>
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
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            return RegisteredFactories[serviceType]
                .Select(n => n()!);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetServices(serviceType);

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

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(serviceType, out var value))
            {
                value = [];
                RegisteredFactories.Add(serviceType, value);
            }

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

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) => throw new NotImplementedException();

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract) => throw new NotImplementedException();

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

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) => throw new NotImplementedException();

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException();

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
            return fact != null ? (T?)fact.Invoke() : default;
        }
    }

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetService<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>()
    {
        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                return Enumerable.Empty<T>();
            }

            return factories.Select(factory => (T)factory()!);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        GetServices<T>();

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

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var value))
            {
                value = [];
                RegisteredFactories.Add(typeof(T), value);
            }

            value.Add(() => factory());
        }
    }

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract) =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        Register(factory);

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() => throw new NotImplementedException();

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) => throw new NotImplementedException();

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

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) => throw new NotImplementedException("Simple Injector does not support the Service Registration Callbacks");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) => throw new NotImplementedException("Simple Injector does not support the Service Registration Callbacks");

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(TService), out var value))
            {
                value = [];
                RegisteredFactories.Add(typeof(TService), value);
            }

            value.Add(() => new TImplementation());
        }
    }

    /// <inheritdoc/>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        Register<TService, TImplementation>();

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                factories = [];
                RegisteredFactories.Add(typeof(T), factories);
            }

            factories.Add(() => value);
        }
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        RegisterConstant(value);

    /// <inheritdoc/>
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_lockObject)
        {
            if (!RegisteredFactories.TryGetValue(typeof(T), out var factories))
            {
                factories = [];
                RegisteredFactories.Add(typeof(T), factories);
            }

            factories.Add(() => lazy.Value);
        }
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class =>

        // SimpleInjectorInitializer doesn't support contracts, so we treat contract-based calls the same as non-contract
        RegisterLazySingleton(valueFactory);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool isDisposing)
    {
    }
}
