// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

// Inspired by ReactiveMarbles.Locator.DefaultServiceLocator
// https://github.com/reactivemarbles/Locator
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Generic-first implementation of IDependencyResolver optimized for AOT compilation.
/// Uses static generic containers for type-safe, reflection-free service resolution.
/// </summary>
public sealed class GlobalGenericFirstDependencyResolver : IDependencyResolver
{
    private static readonly ConcurrentBag<Action> _clearActions = [];

    /// <summary>
    /// Tracks whether the global resolver has ever had any registrations.
    /// 0 = False (never registered), 1 = True (has registrations).
    /// Using int allows for atomic Interlocked operations without locking.
    /// </summary>
    private static int _hasAnyRegistrations;

    private readonly List<Action> _callbackChanged = [];
    private Action[] _callbackSnapshot = [];
    private int _disposed; // 0 = not disposed, 1 = disposed

    static GlobalGenericFirstDependencyResolver()
    {
        // Register clear actions for static containers
        // Note: Container<T> registers itself on first access via static constructor
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalGenericFirstDependencyResolver"/> class.
    /// </summary>
    public GlobalGenericFirstDependencyResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalGenericFirstDependencyResolver"/> class
    /// with bulk registration support.
    /// Allows efficient initialization-time service registration - all Register() calls
    /// are O(1) and snapshots are built lazily on first read.
    /// </summary>
    /// <param name="configure">Optional configuration delegate for bulk service registration.</param>
    /// <example>
    /// <code>
    /// var resolver = new GlobalGenericFirstDependencyResolver(r =>
    /// {
    ///     r.Register&lt;IService&gt;(() => new ServiceA());
    ///     r.Register&lt;ILogger&gt;(() => new ConsoleLogger());
    ///     r.RegisterConstant(configuration);
    ///     // 500+ more registrations - all O(1), snapshots built on first GetService()
    /// });
    /// </code>
    /// </example>
    public GlobalGenericFirstDependencyResolver(Action<IMutableDependencyResolver>? configure) => configure?.Invoke(this);

    /// <summary>
    /// Clears all registered service types and tracked container instances from the registry.
    /// </summary>
    public static void Clear()
    {
        ServiceTypeRegistry.Clear();
        foreach (var clearAction in _clearActions)
        {
            clearAction();
        }

        // Reset the registration flag
        Interlocked.Exchange(ref _hasAnyRegistrations, 0);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>()
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return default;
        }

        if (Container<T>.TryGet(out var service))
        {
            return service;
        }

        if (ServiceTypeRegistry.HasNonGenericRegistrations(TypeCache<T>.Type))
        {
            return TryCastAndUnwrap<T>(ServiceTypeRegistry.GetService(TypeCache<T>.Type));
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
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return default;
        }

        if (ContractContainer<T>.TryGet(contract, out var contractService))
        {
            return contractService;
        }

        if (ServiceTypeRegistry.HasNonGenericRegistrations(TypeCache<T>.Type, contract))
        {
            return TryCastAndUnwrap<T>(ServiceTypeRegistry.GetService(TypeCache<T>.Type, contract));
        }

        return default;
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var result = ServiceTypeRegistry.GetService(serviceType);
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
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var result = ServiceTypeRegistry.GetService(serviceType, contract);
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
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return [];
        }

        var genericResults = Container<T>.GetAll();

        if (ServiceTypeRegistry.HasNonGenericRegistrations(TypeCache<T>.Type))
        {
            var fallbackResults = ServiceTypeRegistry.GetServices(TypeCache<T>.Type);
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
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return [];
        }

        var contractResults = ContractContainer<T>.GetAll(contract);

        if (ServiceTypeRegistry.HasNonGenericRegistrations(TypeCache<T>.Type, contract))
        {
            var fallbackResults = ServiceTypeRegistry.GetServices(TypeCache<T>.Type, contract);
            return CombineResults(contractResults, fallbackResults);
        }

        return contractResults;
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, don't bother with cache lookups
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var results = ServiceTypeRegistry.GetServices(serviceType);
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
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var results = ServiceTypeRegistry.GetServices(serviceType, contract);
        return UnwrapResults(results);
    }

    /// <inheritdoc />
    public bool HasRegistration<T>()
    {
        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return false;
        }

        return Container<T>.HasRegistrations || ServiceTypeRegistry.HasRegistration(TypeCache<T>.Type);
    }

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract)
    {
        if (contract is null)
        {
            return HasRegistration<T>();
        }

        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return false;
        }

        return ContractContainer<T>.HasRegistrations(contract) || ServiceTypeRegistry.HasRegistration(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return false;
        }

        serviceType ??= NullServiceType.CachedType;
        return ServiceTypeRegistry.HasRegistration(serviceType);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return HasRegistration(serviceType);
        }

        // FAST PATH: If we have never registered anything, return false immediately
        if (Volatile.Read(ref _hasAnyRegistrations) == 0)
        {
            return false;
        }

        serviceType ??= NullServiceType.CachedType;

        return ServiceTypeRegistry.HasRegistration(serviceType, contract);
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        Container<T>.Add(factory);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => factory()!);

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
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        ContractContainer<T>.Add(factory, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => factory()!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        // Internal wrapper for null type support if needed, though standard API requires Type.
        // If users pass null, it's invalid unless we support "null" type key which usually means generic untyped?
        // But for "ServiceTypeRegistry", we need a key.
        // Legacy code wrapped it in NullServiceType if null passed.
        // We will assume serviceType is valid or handle null if legacy required.
        // Actually interface says Type (not nullable in new definition? No, I made it non-nullable in method signature but caller might pass null if nullable context disabled?)
        // The interface uses "Type serviceType" (non-nullable in signature I wrote).
        // But let's be safe.
        ServiceTypeRegistry.Register(serviceType, factory);
        ServiceTypeRegistry.TrackNonGenericRegistration(serviceType);
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
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        ServiceTypeRegistry.Register(serviceType, factory, contract);
        ServiceTypeRegistry.TrackNonGenericRegistration(serviceType, contract);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        Container<TService>.Add(static () => new TImplementation());

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        ServiceTypeRegistry.Register(TypeCache<TService>.Type, static () => new TImplementation());

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        ContractContainer<TService>.Add(static () => new TImplementation(), contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        ServiceTypeRegistry.Register(TypeCache<TService>.Type, static () => new TImplementation(), contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        Container<T>.Add(value!);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => value!);

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        ContractContainer<T>.Add(value!, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => value!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        Container<T>.Add(() => lazy.Value);

        // Register the Lazy object itself in ServiceTypeRegistry for proper disposal handling
        // This allows disposal code to check IsValueCreated without triggering evaluation
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => lazy);

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
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // FAST PATH: Mark that this resolver is no longer "virgin" (atomic operation)
        if (_hasAnyRegistrations == 0)
        {
            Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        ContractContainer<T>.Add(() => lazy.Value, contract);

        // Register the Lazy object itself in ServiceTypeRegistry for proper disposal handling
        // This allows disposal code to check IsValueCreated without triggering evaluation
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => lazy, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        Container<T>.RemoveCurrent();

        // Always unregister from ServiceTypeRegistry since we now register in both places
        ServiceTypeRegistry.UnregisterCurrent(TypeCache<T>.Type);

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        ContractContainer<T>.RemoveCurrent(contract);

        // Always unregister from ServiceTypeRegistry since we now register in both places
        ServiceTypeRegistry.UnregisterCurrent(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        ServiceTypeRegistry.UnregisterCurrent(serviceType);
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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        ServiceTypeRegistry.UnregisterCurrent(serviceType, contract);
        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        Container<T>.Clear();

        // Always unregister from ServiceTypeRegistry since we now register in both places
        ServiceTypeRegistry.UnregisterAll(TypeCache<T>.Type);

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        ContractContainer<T>.Clear(contract);

        // Always unregister from ServiceTypeRegistry since we now register in both places
        ServiceTypeRegistry.UnregisterAll(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        ServiceTypeRegistry.UnregisterAll(serviceType);
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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        ServiceTypeRegistry.UnregisterAll(serviceType, contract);
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
        var count = contract == null
            ? Container<T>.GetCount() + ServiceTypeRegistry.GetCount(TypeCache<T>.Type)
            : ContractContainer<T>.GetCount(contract) + ServiceTypeRegistry.GetCount(TypeCache<T>.Type, contract);

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
        var count = ServiceTypeRegistry.GetCount(serviceType, contract);

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
        var factories = ServiceTypeRegistry.GetAllFactoriesForDisposal();
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

        ServiceTypeRegistry.Clear();
    }

    /// <summary>
    /// Registers a clear action for a static container.
    /// </summary>
    /// <param name="clearAction">The clear action to register.</param>
    internal static void RegisterClearAction(Action clearAction) => _clearActions.Add(clearAction);

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
