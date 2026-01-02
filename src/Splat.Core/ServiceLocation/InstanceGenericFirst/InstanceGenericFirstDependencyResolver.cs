// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

// Instance-scoped implementation using ConditionalWeakTable for per-resolver state
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Generic-first implementation of IDependencyResolver optimized for AOT compilation
/// with proper per-resolver instance isolation using ConditionalWeakTable.
/// Each resolver instance maintains its own containers while preserving the generic-first architecture.
/// </summary>
public sealed class InstanceGenericFirstDependencyResolver : IDependencyResolver
{
    private readonly List<Action> _callbackChanged = [];
    private ResolverState _state = new();
    private Action[] _callbackSnapshot = [];
    private int _disposed; // 0 = not disposed, 1 = disposed

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class.
    /// </summary>
    public InstanceGenericFirstDependencyResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class
    /// with bulk registration support.
    /// Allows efficient initialization-time service registration - all Register() calls
    /// are O(1) and snapshots are built lazily on first read.
    /// </summary>
    /// <param name="configure">Optional configuration delegate for bulk service registration.</param>
    /// <example>
    /// <code>
    /// var resolver = new InstanceGenericFirstDependencyResolver(r =>
    /// {
    ///     r.Register&lt;IService&gt;(() => new ServiceA());
    ///     r.Register&lt;ILogger&gt;(() => new ConsoleLogger());
    ///     r.RegisterConstant(configuration);
    ///     // 500+ more registrations - all O(1), snapshots built on first GetService()
    /// });
    /// </code>
    /// </example>
    public InstanceGenericFirstDependencyResolver(Action<IMutableDependencyResolver>? configure) => configure?.Invoke(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>()
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return default;
        }

        var container = ContainerCache<T>.Get(_state);
        if (container.TryGet(out var service))
        {
            return service;
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        if (registry.HasNonGenericRegistrations(TypeCache<T>.Type))
        {
            return TryCastAndUnwrap<T>(registry.GetService(TypeCache<T>.Type));
        }

        return default;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>(string? contract)
    {
        if (contract is null)
        {
            return GetService<T>();
        }

        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return default;
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        if (contractContainer.TryGet(contract, out var contractService))
        {
            return contractService;
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        if (registry.HasNonGenericRegistrations(TypeCache<T>.Type, contract))
        {
            return TryCastAndUnwrap<T>(registry.GetService(TypeCache<T>.Type, contract));
        }

        return default;
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        var result = registry.GetService(serviceType);
        result = UnwrapNullServiceType(result);

        // Unwrap Lazy objects (registered by RegisterLazySingleton)
        if (result is Lazy<object?> lazy)
        {
            return lazy.Value;
        }

        return result;
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract)
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        var result = registry.GetService(serviceType, contract);
        result = UnwrapNullServiceType(result);

        // Unwrap Lazy objects (registered by RegisterLazySingleton)
        if (result is Lazy<object?> lazy)
        {
            return lazy.Value;
        }

        return result;
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>()
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return [];
        }

        var container = ContainerCache<T>.Get(_state);
        var genericResults = container.GetAll();

        var registry = ServiceTypeRegistryCache.Get(_state);
        if (registry.HasNonGenericRegistrations(TypeCache<T>.Type))
        {
            var fallbackResults = registry.GetServices(TypeCache<T>.Type);
            return CombineResults(genericResults, fallbackResults);
        }

        return genericResults;
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        if (contract is null)
        {
            return GetServices<T>();
        }

        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return [];
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        var contractResults = contractContainer.GetAll(contract);

        var registry = ServiceTypeRegistryCache.Get(_state);
        if (registry.HasNonGenericRegistrations(TypeCache<T>.Type, contract))
        {
            var fallbackResults = registry.GetServices(TypeCache<T>.Type, contract);
            return CombineResults(contractResults, fallbackResults);
        }

        return contractResults;
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        var results = registry.GetServices(serviceType);
        return UnwrapResults(results);
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return GetServices(serviceType);
        }

        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        var results = registry.GetServices(serviceType, contract);
        return UnwrapResults(results);
    }

    /// <inheritdoc />
    public bool HasRegistration<T>()
    {
        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return false;
        }

        var container = ContainerCache<T>.Get(_state);
        var registry = ServiceTypeRegistryCache.Get(_state);
        return container.HasRegistrations || registry.HasRegistration(TypeCache<T>.Type);
    }

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract)
    {
        if (contract is null)
        {
            return HasRegistration<T>();
        }

        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return false;
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        var registry = ServiceTypeRegistryCache.Get(_state);
        return contractContainer.HasRegistrations(contract) || registry.HasRegistration(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return false;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return registry.HasRegistration(serviceType);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return HasRegistration(serviceType);
        }

        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _state.HasAnyRegistrations) == 0)
        {
            return false;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return registry.HasRegistration(serviceType, contract);
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var container = ContainerCache<T>.Get(_state);
        container.Add(factory);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => factory()!);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory, string? contract)
    {
        if (contract is null)
        {
            Register<T>(factory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(factory, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => factory()!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        serviceType ??= NullServiceType.CachedType;

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(serviceType, factory);
        registry.TrackNonGenericRegistration(serviceType);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            Register(factory, serviceType);
            return;
        }

        serviceType ??= NullServiceType.CachedType;
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(serviceType, factory, contract);
        registry.TrackNonGenericRegistration(serviceType, contract);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var container = ContainerCache<TService>.Get(_state);
        container.Add(static () => new TImplementation());

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<TService>.Type, static () => new TImplementation());

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (contract is null)
        {
            Register<TService, TImplementation>();
            return;
        }

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var contractContainer = ContractContainerCache<TService>.Get(_state);
        contractContainer.Add(static () => new TImplementation(), contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<TService>.Type, static () => new TImplementation(), contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var container = ContainerCache<T>.Get(_state);
        container.Add(value!);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => value!);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterConstant<T>(value);
            return;
        }

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(value!, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => value!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        var container = ContainerCache<T>.Get(_state);
        container.Add(() => lazy.Value);

        // Register the Lazy object itself in ServiceTypeRegistry for proper disposal handling
        // This allows disposal code to check IsValueCreated without triggering evaluation
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => lazy);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton<T>(valueFactory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_state.HasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(() => lazy.Value, contract);

        // Register the Lazy object itself in ServiceTypeRegistry for proper disposal handling
        // This allows disposal code to check IsValueCreated without triggering evaluation
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => lazy, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>()
    {
        var container = ContainerCache<T>.Get(_state);
        container.RemoveCurrent();

        // Always unregister from ServiceTypeRegistry since we now register in both places
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(TypeCache<T>.Type);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>(string? contract)
    {
        if (contract is null)
        {
            UnregisterCurrent<T>();
            return;
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.RemoveCurrent(contract);

        // Always unregister from ServiceTypeRegistry since we now register in both places
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(serviceType);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        if (contract is null)
        {
            UnregisterCurrent(serviceType);
            return;
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(serviceType, contract);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll<T>()
    {
        var container = ContainerCache<T>.Get(_state);
        container.Clear();

        // Always unregister from ServiceTypeRegistry since we now register in both places
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(TypeCache<T>.Type);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll<T>(string? contract)
    {
        if (contract is null)
        {
            UnregisterAll<T>();
            return;
        }

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Clear(contract);

        // Always unregister from ServiceTypeRegistry since we now register in both places
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(serviceType);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        if (contract is null)
        {
            UnregisterAll(serviceType);
            return;
        }

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(serviceType, contract);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) => ServiceRegistrationCallback<T>(null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        Action callbackWrapper = () => callback(ActionDisposable.Empty);

        lock (_callbackChanged)
        {
            _callbackChanged.Add(callbackWrapper);
        }

        RefreshCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            lock (_callbackChanged)
            {
                _callbackChanged.Remove(callbackWrapper);
            }

            RefreshCallbackSnapshot();
        });

        // Invoke callback once per existing registration to match ModernDependencyResolver behavior
        // Use count methods to avoid invoking factories (especially lazy singletons)
        var container = ContainerCache<T>.Get(_state);
        var registry = ServiceTypeRegistryCache.Get(_state);

        var count = contract == null
            ? container.GetCount() + registry.GetCount(TypeCache<T>.Type)
            : ContractContainerCache<T>.Get(_state).GetCount(contract) + registry.GetCount(TypeCache<T>.Type, contract);

        for (var i = 0; i < count; i++)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) => ServiceRegistrationCallback(serviceType, null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);
        Action callbackWrapper = () => callback(ActionDisposable.Empty);

        lock (_callbackChanged)
        {
            _callbackChanged.Add(callbackWrapper);
        }

        RefreshCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            lock (_callbackChanged)
            {
                _callbackChanged.Remove(callbackWrapper);
            }

            RefreshCallbackSnapshot();
        });

        // Invoke callback once per existing registration to match ModernDependencyResolver behavior
        // Use count method to avoid invoking factories (especially lazy singletons)
        var registry = ServiceTypeRegistryCache.Get(_state);
        var count = registry.GetCount(serviceType, contract);

        for (var i = 0; i < count; i++)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Atomically mark as disposed
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
        {
            return; // Already disposed
        }

        // Atomically replace state with a fresh empty one to make old state unreachable
        // This enables ConditionalWeakTable cleanup of all generic containers
        var oldState = Interlocked.Exchange(ref _state, new());

        // Take a snapshot of callbacks to avoid issues if modified during disposal
        var callbacksToInvoke = Volatile.Read(ref _callbackSnapshot);

        // Dispose all registered callbacks
        foreach (var callback in callbacksToInvoke)
        {
            try
            {
                callback();
            }
            catch
            {
                // Suppress exceptions during disposal
            }
        }

        _callbackChanged.Clear();

        // Dispose all registered services that implement IDisposable
        var registry = ServiceTypeRegistryCache.Get(oldState);
        var factories = registry.GetAllFactoriesForDisposal();
        foreach (var factory in factories)
        {
            try
            {
                var item = factory();
                if (item is Lazy<object?> lazy)
                {
                    if (lazy.IsValueCreated)
                    {
                        item = lazy.Value;
                    }
                    else
                    {
                        // Skip if the lazy value is not created yet, don't create it just for disposal
                        continue;
                    }
                }

                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch
            {
                // Suppress exceptions during disposal to avoid crashing the application
            }
        }

        registry.Clear();

        // oldState is now unreachable - ConditionalWeakTable will clean up all generic containers
    }

    private static IEnumerable<object> UnwrapResults(IEnumerable<object> results)
    {
        foreach (var result in results)
        {
            var value = UnwrapNullServiceType(result);
            if (value is not null)
            {
                yield return value;
            }
        }
    }

#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static object? UnwrapNullServiceType(object? value)
    {
        if (value is NullServiceType nullServiceType)
        {
            return nullServiceType.Factory();
        }

        return value;
    }

#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static T? TryCastAndUnwrap<T>(object? value)
    {
        var unwrapped = UnwrapNullServiceType(value);
        return unwrapped is T typedResult ? typedResult : default;
    }

    private static T[] CombineResults<T>(T[] genericResults, object[] fallbackResults)
    {
        if (fallbackResults.Length == 0)
        {
            return genericResults;
        }

        var results = new List<T>(genericResults.Length + fallbackResults.Length);
        results.AddRange(genericResults);

        foreach (var result in fallbackResults)
        {
            var unwrapped = UnwrapNullServiceType(result);
            if (unwrapped is T typedItem)
            {
                results.Add(typedItem);
            }
        }

        return [.. results];
    }

    private void NotifyCallbackChanged()
    {
        if (Volatile.Read(ref _disposed) != 0)
        {
            return;
        }

        var callbacks = Volatile.Read(ref _callbackSnapshot);
        foreach (var callback in callbacks)
        {
            callback();
        }
    }

    private void RefreshCallbackSnapshot()
    {
        lock (_callbackChanged)
        {
            Volatile.Write(ref _callbackSnapshot, [.. _callbackChanged]);
        }
    }
}
