// Copyright (c) 2026 ReactiveUI. All rights reserved.
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
/// Global, generic-first implementation of <see cref="IDependencyResolver"/> optimized for AOT compilation.
/// </summary>
/// <remarks>
/// <para>
/// This resolver uses static generic containers for type-safe, reflection-free service resolution:
/// <list type="bullet">
///   <item><description><see cref="GetService{T}()"/> and <see cref="GetServices{T}()"/> are the preferred fast paths.</description></item>
///   <item><description>Non-generic APIs (<see cref="GetService(Type)"/> / <see cref="GetServices(Type)"/>) are supported via an internal type registry.</description></item>
///   <item><description>Contracts allow named variants of the same service type.</description></item>
/// </list>
/// </para>
/// <para>
/// Registration semantics:
/// <list type="bullet">
///   <item><description><see cref="GetService{T}()"/> returns the most recently registered instance for service type (last registration wins).</description></item>
///   <item><description><see cref="GetServices{T}()"/> returns all registrations for registrations of that type.</description></item>
/// </list>
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>Resolution is lock-free within this type (uses published snapshots and static container concurrency).</description></item>
///   <item><description>Callback and disposal bookkeeping is thread-safe (protected by internal gates).</description></item>
/// </list>
/// </para>
/// <para>
/// Disposal:
/// <list type="bullet">
///   <item><description>Constants and lazy singletons that implement <see cref="IDisposable"/> are disposed when this resolver instance is disposed.</description></item>
///   <item><description>Callback and disposal exceptions are suppressed during <see cref="Dispose()"/>.</description></item>
/// </list>
/// </para>
/// <para>
/// <see cref="Clear()"/> clears global registrations and static containers for the entire process.
/// </para>
/// </remarks>
public sealed class GlobalGenericFirstDependencyResolver : IDependencyResolver
{
    /// <summary>
    /// Global clear actions registered by static generic containers.
    /// </summary>
    /// <remarks>
    /// Containers register their own clear action on first access. <see cref="Clear()"/> invokes all actions.
    /// </remarks>
    private static readonly ConcurrentBag<Action> _clearActions = [];

    /// <summary>
    /// Tracks whether the global resolver has ever had any registrations.
    /// </summary>
    /// <remarks>
    /// 0 = never registered; 1 = has registrations. Stored as <see cref="int"/> for atomic operations.
    /// </remarks>
    private static int _hasAnyRegistrations;

    /// <summary>
    /// Gate protecting <see cref="_callbackChanged"/> and publication of <see cref="_callbackSnapshot"/>.
    /// </summary>
    private readonly object _callbackGate = new();

    /// <summary>
    /// Registration-change callbacks. Mutated under <see cref="_callbackGate"/>.
    /// </summary>
    private readonly List<Action> _callbackChanged = [];

    /// <summary>
    /// Gate protecting <see cref="_disposalActions"/> additions/enumeration/clearing.
    /// </summary>
    private readonly object _disposalGate = new();

    /// <summary>
    /// Actions executed during <see cref="Dispose()"/> to dispose constants and lazy singletons.
    /// </summary>
    private readonly List<Action> _disposalActions = [];

    /// <summary>
    /// Published snapshot of callbacks for lock-free invocation.
    /// </summary>
    /// <remarks>
    /// Written under <see cref="_callbackGate"/>, read via a Volatile Read.
    /// </remarks>
    private Action[] _callbackSnapshot = [];

    /// <summary>
    /// Disposal flag for this resolver instance: 0 = not disposed, 1 = disposed.
    /// </summary>
    private int _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalGenericFirstDependencyResolver"/> class.
    /// </summary>
    public GlobalGenericFirstDependencyResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalGenericFirstDependencyResolver"/> class with bulk registration support.
    /// </summary>
    /// <param name="configure">
    /// Optional delegate that performs registrations against this instance. Intended for startup-time bulk registration.
    /// </param>
    /// <example>
    /// <code>
    /// var resolver = new GlobalGenericFirstDependencyResolver(r =>
    /// {
    ///     r.Register&lt;IService&gt;(() => new ServiceA());
    ///     r.Register&lt;ILogger&gt;(() => new ConsoleLogger());
    ///     r.RegisterConstant(configuration);
    /// });
    /// </code>
    /// </example>
    public GlobalGenericFirstDependencyResolver(Action<IMutableDependencyResolver>? configure) => configure?.Invoke(this);

    /// <summary>
    /// Clears all global registrations and tracked container instances for the entire process.
    /// </summary>
    /// <remarks>
    /// This affects all <see cref="GlobalGenericFirstDependencyResolver"/> instances because registrations are global/static.
    /// </remarks>
    public static void Clear()
    {
        ServiceTypeRegistry.Clear();

        foreach (var clearAction in _clearActions)
        {
            clearAction();
        }

        Interlocked.Exchange(ref _hasAnyRegistrations, 0);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>()
    {
        if (HasNeverRegistered())
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

        if (HasNeverRegistered())
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
        if (HasNeverRegistered())
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        return UnwrapNullServiceType(ServiceTypeRegistry.GetService(serviceType));
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract)
    {
        if (HasNeverRegistered())
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        return UnwrapNullServiceType(ServiceTypeRegistry.GetService(serviceType, contract));
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>()
    {
        if (HasNeverRegistered())
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

        if (HasNeverRegistered())
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
        if (HasNeverRegistered())
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        return UnwrapResults(ServiceTypeRegistry.GetServices(serviceType));
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return GetServices(serviceType);
        }

        if (HasNeverRegistered())
        {
            return [];
        }

        serviceType ??= NullServiceType.CachedType;
        return UnwrapResults(ServiceTypeRegistry.GetServices(serviceType, contract));
    }

    /// <inheritdoc />
    public bool HasRegistration<T>()
    {
        if (HasNeverRegistered())
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

        if (HasNeverRegistered())
        {
            return false;
        }

        return ContractContainer<T>.HasRegistrations(contract) || ServiceTypeRegistry.HasRegistration(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        if (HasNeverRegistered())
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

        if (HasNeverRegistered())
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

        MarkRegistered();

        Container<T>.Add(factory);
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => factory()!);

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

        ContractContainer<T>.Add(factory, contract);
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => factory()!, contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;

        MarkRegistered();

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

        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;

        MarkRegistered();

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
        MarkRegistered();

        Container<TService>.Add(static () => new TImplementation());
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
        MarkRegistered();

        ContractContainer<TService>.Add(static () => new TImplementation(), contract);
        ServiceTypeRegistry.Register(TypeCache<TService>.Type, static () => new TImplementation(), contract);

        NotifyCallbackChanged();
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        AddDisposableIfNeeded(value);

        Container<T>.Add(value!);
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => value!);

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

        ContractContainer<T>.Add(value!, contract);
        ServiceTypeRegistry.Register(TypeCache<T>.Type, () => value!, contract);

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
        Container<T>.Add(() =>
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

        ServiceTypeRegistry.Register(TypeCache<T>.Type, () =>
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
        ContractContainer<T>.Add(
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

        ServiceTypeRegistry.Register(
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

        Container<T>.RemoveCurrent();
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
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback<T>(null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        Action wrapper = () => callback(ActionDisposable.Empty);

        AddCallback(wrapper);
        PublishCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            RemoveCallback(wrapper);
            PublishCallbackSnapshot();
        });

        // Invoke once per existing registration (without invoking factories).
        var count = contract is null
            ? Container<T>.GetCount() + ServiceTypeRegistry.GetCount(TypeCache<T>.Type)
            : ContractContainer<T>.GetCount(contract) + ServiceTypeRegistry.GetCount(TypeCache<T>.Type, contract);

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

        Action wrapper = () => callback(ActionDisposable.Empty);

        AddCallback(wrapper);
        PublishCallbackSnapshot();

        var disp = new ActionDisposable(() =>
        {
            RemoveCallback(wrapper);
            PublishCallbackSnapshot();
        });

        // Invoke once per existing registration (without invoking factories).
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
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
        {
            return;
        }

        // Snapshot callbacks for lock-free invocation; clear lists under locks.
        var callbacks = Volatile.Read(ref _callbackSnapshot);

        Action[]? disposals;
        lock (_disposalGate)
        {
            disposals = _disposalActions.Count == 0 ? null : [.. _disposalActions];
            _disposalActions.Clear();
        }

        lock (_callbackGate)
        {
            _callbackChanged.Clear();
            Volatile.Write(ref _callbackSnapshot, []);
        }

        // Invoke callbacks (exceptions suppressed).
        for (var i = 0; i < callbacks.Length; i++)
        {
            try
            {
                callbacks[i]();
            }
            catch
            {
            }
        }

        // Run disposal actions (exceptions suppressed).
        if (disposals is not null)
        {
            for (var i = 0; i < disposals.Length; i++)
            {
                try
                {
                    disposals[i]();
                }
                catch
                {
                }
            }
        }

        // Clear global registry for non-generic lookups.
        ServiceTypeRegistry.Clear();
    }

    /// <summary>
    /// Registers a clear action for a static generic container.
    /// </summary>
    /// <param name="clearAction">Action that clears the container's registrations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="clearAction"/> is <see langword="null"/>.</exception>
    internal static void RegisterClearAction(Action clearAction)
    {
        ArgumentExceptionHelper.ThrowIfNull(clearAction);
        _clearActions.Add(clearAction);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the process has never observed a registration.
    /// </summary>
    /// <returns><see langword="true"/> if no registrations have occurred; otherwise <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool HasNeverRegistered() => Volatile.Read(ref _hasAnyRegistrations) == 0;

    /// <summary>
    /// Unwraps registry results by evaluating <see cref="NullServiceType"/> wrappers and filtering out nulls.
    /// </summary>
    /// <param name="results">Raw results returned by the type registry.</param>
    /// <returns>An array of unwrapped, non-null objects.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    private static object[] UnwrapResults(IEnumerable<object> results)
    {
        ArgumentExceptionHelper.ThrowIfNull(results);

        // Common case: registry returns an array.
        if (results is object[] arr)
        {
            var count = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                if (UnwrapNullServiceType(arr[i]) is not null)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return [];
            }

            // Fast path reuse: only when no nulls and no wrapper instances.
            if (count == arr.Length)
            {
                var hasWrapper = false;
                for (var i = 0; i < arr.Length; i++)
                {
                    if (arr[i] is NullServiceType)
                    {
                        hasWrapper = true;
                        break;
                    }
                }

                if (!hasWrapper)
                {
                    return arr;
                }
            }

            var output = new object[count];
            var idx = 0;

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
    private static object? UnwrapNullServiceType(object? value) =>
        value is NullServiceType nst ? nst.Factory() : value;

#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static T? TryCastAndUnwrap<T>(object? value)
    {
        var unwrapped = UnwrapNullServiceType(value);
        return unwrapped is T typed ? typed : default;
    }

    /// <summary>
    /// Combines generic results with fallback non-generic results, including only fallback values castable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="genericResults">Results returned by the generic container.</param>
    /// <param name="fallbackResults">Results returned by the non-generic registry.</param>
    /// <returns>A combined array.</returns>
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

        var extra = 0;
        for (var i = 0; i < fallbackResults.Length; i++)
        {
            if (UnwrapNullServiceType(fallbackResults[i]) is T)
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

        var idx = genericResults.Length;
        for (var i = 0; i < fallbackResults.Length; i++)
        {
            if (UnwrapNullServiceType(fallbackResults[i]) is T typed)
            {
                combined[idx++] = typed;
            }
        }

        return combined;
    }

    /// <summary>
    /// Marks the global resolver as having at least one registration.
    /// </summary>
    /// <remarks>
    /// This enables a fast resolve path that avoids unnecessary container/registry lookups when no registrations exist.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void MarkRegistered()
    {
        if (_hasAnyRegistrations == 0)
        {
            _ = Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0);
        }
    }

    /// <summary>
    /// Adds a registration-change callback.
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
    /// Removes a registration-change callback.
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
    /// Allocates an array containing the current callbacks and publishes it via <see cref="Volatile.Write{T}(ref T, T)"/>.
    /// </remarks>
    private void PublishCallbackSnapshot()
    {
        lock (_callbackGate)
        {
            Volatile.Write(ref _callbackSnapshot, [.. _callbackChanged]);
        }
    }

    /// <summary>
    /// Notifies registered callbacks that the registration set changed.
    /// </summary>
    /// <remarks>
    /// Uses a published snapshot and suppresses exceptions thrown by callback implementations.
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
            }
        }
    }

    /// <summary>
    /// Adds a disposal action for a constant registration if it implements <see cref="IDisposable"/>.
    /// </summary>
    /// <typeparam name="T">Registered service type.</typeparam>
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
                }
            });
        }
    }

    /// <summary>
    /// Adds a disposal action for a lazy singleton so the created value is disposed if applicable.
    /// </summary>
    /// <typeparam name="T">Lazy singleton value type.</typeparam>
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
                }
            });
        }
    }
}
