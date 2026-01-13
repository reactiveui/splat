// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// Instance-scoped implementation using ConditionalWeakTable for per-resolver state
namespace Splat;

/// <summary>
/// Generic-first implementation of <see cref="IDependencyResolver"/> optimized for AOT compilation,
/// with per-resolver instance isolation using resolver-local state.
/// </summary>
/// <remarks>
/// <para>
/// This resolver favors the generic API surface (<see cref="GetService{T}()"/>, <see cref="GetServices{T}()"/>)
/// for performance, while still supporting non-generic APIs (<see cref="GetService(Type)"/>, <see cref="GetServices(Type)"/>)
/// through an internal type registry.
/// </para>
/// <para>
/// Registration semantics:
/// <list type="bullet">
///   <item><description><see cref="GetService{T}()"/> returns the most recently registered service for the type (last registration wins).</description></item>
///   <item><description><see cref="GetServices{T}()"/> returns all registrations (order is implementation-defined by the underlying containers/registry).</description></item>
///   <item><description>Contracts provide named variants of the same service type.</description></item>
/// </list>
/// </para>
/// <para>
/// Disposal semantics:
/// <list type="bullet">
///   <item><description>Constants and lazy singletons that implement <see cref="IDisposable"/> are disposed when the resolver is disposed.</description></item>
///   <item><description>Exceptions thrown from callbacks or disposal actions are suppressed during <see cref="Dispose()"/>.</description></item>
/// </list>
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>Registration-change callbacks and disposal action bookkeeping are thread-safe.</description></item>
///   <item><description>Resolve operations avoid additional locking within this type; underlying containers/registry are expected to be concurrency-safe for their usage patterns.</description></item>
/// </list>
/// </para>
/// </remarks>
public sealed class InstanceGenericFirstDependencyResolver : IDependencyResolver
{
    /// <summary>
    /// Gate protecting <see cref="_callbackChanged"/> and publication of <see cref="_callbackSnapshot"/>.
    /// </summary>
    /// <remarks>
    /// Do not lock on the list instance itself; list mutations (including <c>Clear</c>) would make that pattern fragile.
    /// </remarks>
    private readonly object _callbackGate = new();

    /// <summary>
    /// Registered callbacks notified when registrations change.
    /// </summary>
    /// <remarks>
    /// The list is mutated under <see cref="_callbackGate"/> and a snapshot is published to <see cref="_callbackSnapshot"/>
    /// for lock-free invocation.
    /// </remarks>
    private readonly List<Action> _callbackChanged = [];

    /// <summary>
    /// Gate protecting <see cref="_disposalActions"/> additions/enumeration/clearing.
    /// </summary>
    private readonly object _disposalGate = new();

    /// <summary>
    /// Actions executed during <see cref="Dispose()"/> to clean up registered disposables (constants and lazy singletons).
    /// </summary>
    /// <remarks>
    /// Mutated under <see cref="_disposalGate"/>. Enumerated and cleared during dispose using a snapshot to avoid holding locks while running user code.
    /// </remarks>
    private readonly List<Action> _disposalActions = [];

    /// <summary>
    /// Resolver-local state used by generic-first caches.
    /// </summary>
    /// <remarks>
    /// During <see cref="Dispose()"/>, this is swapped for a fresh state to allow the previous state to become unreachable.
    /// </remarks>
    private ResolverState _state = new();

    /// <summary>
    /// Snapshot of callbacks published for lock-free iteration.
    /// </summary>
    /// <remarks>
    /// Written under <see cref="_callbackGate"/> and read via Volatile Read.
    /// </remarks>
    private Action[] _callbackSnapshot = [];

    /// <summary>
    /// Disposal flag: 0 = not disposed, 1 = disposed.
    /// </summary>
    private int _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class.
    /// </summary>
    public InstanceGenericFirstDependencyResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class with bulk registration support.
    /// </summary>
    /// <param name="configure">
    /// Optional delegate that performs registrations against this instance. Intended for startup-time bulk registration.
    /// </param>
    /// <example>
    /// <code>
    /// var resolver = new InstanceGenericFirstDependencyResolver(r =>
    /// {
    ///     r.Register&lt;IService&gt;(() => new ServiceA());
    ///     r.Register&lt;ILogger&gt;(() => new ConsoleLogger());
    ///     r.RegisterConstant(configuration);
    /// });
    /// </code>
    /// </example>
    public InstanceGenericFirstDependencyResolver(Action<IMutableDependencyResolver>? configure) => configure?.Invoke(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>()
    {
        if (HasAnyRegistrations())
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

        if (HasAnyRegistrations())
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
        if (HasAnyRegistrations())
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return UnwrapNullServiceType(registry.GetService(serviceType));
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract)
    {
        if (HasAnyRegistrations())
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return UnwrapNullServiceType(registry.GetService(serviceType, contract));
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>()
    {
        if (HasAnyRegistrations())
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

        if (HasAnyRegistrations())
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
        if (HasAnyRegistrations())
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return UnwrapResults(registry.GetServices(serviceType));
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return GetServices(serviceType);
        }

        if (HasAnyRegistrations())
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(_state);
        return UnwrapResults(registry.GetServices(serviceType, contract));
    }

    /// <inheritdoc />
    public bool HasRegistration<T>()
    {
        if (HasAnyRegistrations())
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

        if (HasAnyRegistrations())
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
        if (HasAnyRegistrations())
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

        if (HasAnyRegistrations())
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

        MarkRegistered();

        var container = ContainerCache<T>.Get(_state);
        container.Add(factory);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it.
        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => factory()!);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory, string? contract)
    {
        if (contract is null)
        {
            Register(factory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        MarkRegistered();

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(factory, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it.
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

        MarkRegistered();

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

        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;

        MarkRegistered();

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
        MarkRegistered();

        var container = ContainerCache<TService>.Get(_state);
        container.Add(static () => new TImplementation());

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        var contractContainer = ContractContainerCache<TService>.Get(_state);
        contractContainer.Add(static () => new TImplementation(), contract);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<TService>.Type, static () => new TImplementation(), contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        AddDisposableIfNeeded(value);

        var container = ContainerCache<T>.Get(_state);
        container.Add(value!);

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
            RegisterConstant(value);
            return;
        }

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        AddDisposableIfNeeded(value);

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(value!, contract);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () => value!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Wrap lazy value access to dispose and throw if resolver was disposed during construction.
        var container = ContainerCache<T>.Get(_state);
        container.Add(() =>
        {
            var value = lazy.Value;
            if (Volatile.Read(ref _disposed) != 0)
            {
                (value as IDisposable)?.Dispose();
                ObjectDisposedExceptionHelper.ThrowIf(true, this);
            }

            return value;
        });

        AddLazyDisposal(lazy);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(TypeCache<T>.Type, () =>
        {
            var value = lazy.Value;
            if (Volatile.Read(ref _disposed) != 0)
            {
                (value as IDisposable)?.Dispose();
                ObjectDisposedExceptionHelper.ThrowIf(true, this);
            }

            return value;
        });

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton<T>(valueFactory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Wrap lazy value access to dispose and throw if resolver was disposed during construction.
        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Add(
            () =>
            {
                var value = lazy.Value;
                if (Volatile.Read(ref _disposed) != 0)
                {
                    (value as IDisposable)?.Dispose();
                    ObjectDisposedExceptionHelper.ThrowIf(true, this);
                }

                return value;
            },
            contract);

        AddLazyDisposal(lazy);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.Register(
            TypeCache<T>.Type,
            () =>
            {
                var value = lazy.Value;
                if (Volatile.Read(ref _disposed) != 0)
                {
                    (value as IDisposable)?.Dispose();
                    ObjectDisposedExceptionHelper.ThrowIf(true, this);
                }

                return value;
            },
            contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var container = ContainerCache<T>.Get(_state);
        container.RemoveCurrent();

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.RemoveCurrent(contract);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterCurrent(serviceType, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var container = ContainerCache<T>.Get(_state);
        container.Clear();

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var contractContainer = ContractContainerCache<T>.Get(_state);
        contractContainer.Clear(contract);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(TypeCache<T>.Type, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

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

        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var registry = ServiceTypeRegistryCache.Get(_state);
        registry.UnregisterAll(serviceType, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback<T>(null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        // Wrap to normalize callback signature; the wrapper is what we store/remove.
        Action callbackWrapper = () => callback(ActionDisposable.Empty);

        AddCallback(callbackWrapper);
        RefreshCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            RemoveCallback(callbackWrapper);
            RefreshCallbackSnapshot();
        });

        // Invoke callback once per existing registration (matches ModernDependencyResolver behavior).
        // Use counts to avoid invoking factories (especially lazy singletons).
        var container = ContainerCache<T>.Get(_state);
        var registry = ServiceTypeRegistryCache.Get(_state);

        var count = contract is null
            ? container.GetCount() + registry.GetCount(TypeCache<T>.Type)
            : ContractContainerCache<T>.Get(_state).GetCount(contract) + registry.GetCount(TypeCache<T>.Type, contract);

        for (var i = 0; i < count; i++)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(serviceType, null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        Action callbackWrapper = () => callback(ActionDisposable.Empty);

        AddCallback(callbackWrapper);
        RefreshCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            RemoveCallback(callbackWrapper);
            RefreshCallbackSnapshot();
        });

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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Unwraps non-generic results and filters out nulls.
    /// </summary>
    /// <param name="results">The raw results sequence from the type registry.</param>
    /// <returns>
    /// An array containing unwrapped non-null results. Returning an array avoids iterator allocations.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    private static object[] UnwrapResults(IEnumerable<object> results)
    {
        ArgumentExceptionHelper.ThrowIfNull(results);

        // Most callers pass object[] from the registry; special-case to avoid enumerator allocations.
        if (results is object[] arr)
        {
            var count = 0;

            // First pass: count surviving items after unwrap/null-filter.
            for (var i = 0; i < arr.Length; i++)
            {
                var v = UnwrapNullServiceType(arr[i]);
                if (v is not null)
                {
                    count++;
                }
            }

            if (count == arr.Length)
            {
                // No nulls filtered and unwrap did not produce null; we can safely return original array
                // because we are returning IEnumerable<object> semantics and the source is already an array.
                // (If unwrap changed reference types, count would still equal, but we would have produced new objects;
                // so we only reuse the array when unwrapping did not change shape. To ensure correctness, we re-check quickly.)
                var changed = false;
                for (var i = 0; i < arr.Length; i++)
                {
                    if (!ReferenceEquals(arr[i], UnwrapNullServiceType(arr[i])))
                    {
                        changed = true;
                        break;
                    }
                }

                if (!changed)
                {
                    return arr;
                }
            }

            if (count == 0)
            {
                return [];
            }

            var output = new object[count];
            var idx = 0;

            // Second pass: fill output.
            for (var i = 0; i < arr.Length; i++)
            {
                var v = UnwrapNullServiceType(arr[i]);
                if (v is not null)
                {
                    output[idx++] = v;
                }
            }

            return output;
        }

        // General path: materialize into a list then return an array. This preserves semantics and avoids iterator allocations.
        var list = new List<object>();
        foreach (var item in results)
        {
            var v = UnwrapNullServiceType(item);
            if (v is not null)
            {
                list.Add(v);
            }
        }

        return list.Count == 0 ? [] : [.. list];
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

    /// <summary>
    /// Combines generic-first results with fallback non-generic results, preserving the existing behavior:
    /// fallback results are unwrapped and included only when they can be cast to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the combined results.</typeparam>
    /// <param name="genericResults">Results from the generic container.</param>
    /// <param name="fallbackResults">Results from the non-generic registry.</param>
    /// <returns>An array containing the combined results.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="genericResults"/> or <paramref name="fallbackResults"/> is <see langword="null"/>.
    /// </exception>
    private static T[] CombineResults<T>(T[] genericResults, object[] fallbackResults)
    {
        ArgumentExceptionHelper.ThrowIfNull(genericResults);
        ArgumentExceptionHelper.ThrowIfNull(fallbackResults);

        if (fallbackResults.Length == 0)
        {
            return genericResults;
        }

        // First pass: count additional typed items from fallback.
        var extra = 0;
        for (var i = 0; i < fallbackResults.Length; i++)
        {
            var unwrapped = UnwrapNullServiceType(fallbackResults[i]);
            if (unwrapped is T)
            {
                extra++;
            }
        }

        if (extra == 0)
        {
            return genericResults;
        }

        var combined = new T[genericResults.Length + extra];
        Array.Copy(genericResults, combined, genericResults.Length);

        // Second pass: fill additional elements.
        var idx = genericResults.Length;
        for (var i = 0; i < fallbackResults.Length; i++)
        {
            var unwrapped = UnwrapNullServiceType(fallbackResults[i]);
            if (unwrapped is T typed)
            {
                combined[idx++] = typed;
            }
        }

        return combined;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and, optionally, releases the managed resources.
    /// </summary>
    /// <remarks>
    /// This method is called by public Dispose methods and finalizers to perform resource cleanup.
    /// When isDisposing is true, the method can safely dispose managed objects. When isDisposing is false, the method
    /// should only release unmanaged resources. Override this method in a derived class to provide custom disposal
    /// logic. Always call the base class implementation to ensure proper cleanup.
    /// </remarks>
    /// <param name="isDisposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    private void Dispose(bool isDisposing)
    {
        // Ensure Dispose runs exactly once.
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
        {
            return;
        }

        // Swap state so any future cache lookups use the new empty state.
        // The old state becomes unreachable from this instance (allowing cleanup).
        var oldState = Interlocked.Exchange(ref _state, new());

        // Snapshot callbacks and disposal actions under their respective gates.
        // Run user code outside locks.
        var callbacks = Volatile.Read(ref _callbackSnapshot);

        Action[]? disposalActionsSnapshot;
        lock (_disposalGate)
        {
            disposalActionsSnapshot = _disposalActions.Count == 0 ? null : [.. _disposalActions];
            _disposalActions.Clear();
        }

        // Dispose registration callbacks (exceptions suppressed).
        for (var i = 0; i < callbacks.Length; i++)
        {
            try
            {
                callbacks[i]();
            }
            catch
            {
                // Suppress exceptions during disposal.
            }
        }

        // Clear callbacks under the correct gate (race fix vs. original).
        lock (_callbackGate)
        {
            _callbackChanged.Clear();
            Volatile.Write(ref _callbackSnapshot, []);
        }

        // Execute disposal actions for constants and lazy singletons (exceptions suppressed).
        if (disposalActionsSnapshot is not null)
        {
            for (var i = 0; i < disposalActionsSnapshot.Length; i++)
            {
                try
                {
                    disposalActionsSnapshot[i]();
                }
                catch
                {
                    // Suppress exceptions during disposal.
                }
            }
        }

        // Clear the non-generic registry associated with the old state.
        // This ensures non-generic registrations do not leak across resolver lifetimes.
        var registry = ServiceTypeRegistryCache.Get(oldState);
        registry.Clear();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the resolver has never had a registration added.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when no registrations have been added; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasAnyRegistrations() => Volatile.Read(ref _state.HasAnyRegistrations) == 0;

    /// <summary>
    /// Marks the resolver as having at least one registration.
    /// </summary>
    /// <remarks>
    /// This is an optimization: many hot-path resolve operations can skip cache/registry lookups when the resolver is still "virgin".
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MarkRegistered()
    {
        if (_state.HasAnyRegistrations == 0)
        {
            _ = Interlocked.CompareExchange(ref _state.HasAnyRegistrations, 1, 0);
        }
    }

    /// <summary>
    /// Adds a callback to the registration-change callback list.
    /// </summary>
    /// <param name="callback">The callback to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is <see langword="null"/>.</exception>
    private void AddCallback(Action callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        lock (_callbackGate)
        {
            _callbackChanged.Add(callback);
        }
    }

    /// <summary>
    /// Removes a callback from the registration-change callback list.
    /// </summary>
    /// <param name="callback">The callback to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is <see langword="null"/>.</exception>
    private void RemoveCallback(Action callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        lock (_callbackGate)
        {
            _callbackChanged.Remove(callback);
        }
    }

    /// <summary>
    /// Publishes a fresh snapshot of callbacks for lock-free invocation.
    /// </summary>
    /// <remarks>
    /// This allocates a new array containing the current callback list.
    /// The array is then published via <see cref="Volatile.Write{T}(ref T, T)"/>.
    /// </remarks>
    private void RefreshCallbackSnapshot()
    {
        lock (_callbackGate)
        {
            Volatile.Write(ref _callbackSnapshot, [.. _callbackChanged]);
        }
    }

    /// <summary>
    /// Invokes the current callback snapshot to signal that registrations have changed.
    /// </summary>
    /// <remarks>
    /// Suppresses exceptions thrown by callbacks. If the resolver is disposed, no callbacks are invoked.
    /// </remarks>
    private void NotifyCallbackChanged()
    {
        if (Volatile.Read(ref _disposed) != 0)
        {
            return;
        }

        var callbacks = Volatile.Read(ref _callbackSnapshot);
        for (var i = 0; i < callbacks.Length; i++)
        {
            try
            {
                callbacks[i]();
            }
            catch
            {
                // Suppress exceptions during normal notification to match existing behavior.
            }
        }
    }

    /// <summary>
    /// Adds a disposal action for a constant registration if it implements <see cref="IDisposable"/>.
    /// </summary>
    /// <typeparam name="T">The registered service type.</typeparam>
    /// <param name="value">The constant value; may be <see langword="null"/>.</param>
    private void AddDisposableIfNeeded<T>(T? value)
        where T : class
    {
        if (value is not IDisposable disposable)
        {
            return;
        }

        lock (_disposalGate)
        {
            _disposalActions.Add(() =>
            {
                try
                {
                    disposable.Dispose();
                }
                catch
                {
                    // Suppress exceptions during disposal.
                }
            });
        }
    }

    /// <summary>
    /// Adds a disposal action for a lazy singleton so that the created value is disposed if applicable.
    /// </summary>
    /// <typeparam name="T">The lazy singleton value type.</typeparam>
    /// <param name="lazy">The lazy wrapper.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="lazy"/> is <see langword="null"/>.</exception>
    private void AddLazyDisposal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Lazy<T?> lazy)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(lazy);

        lock (_disposalGate)
        {
            _disposalActions.Add(() =>
            {
                try
                {
                    if (lazy.IsValueCreated && lazy.Value is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
                catch
                {
                    // Suppress exceptions during disposal.
                }
            });
        }
    }
}
