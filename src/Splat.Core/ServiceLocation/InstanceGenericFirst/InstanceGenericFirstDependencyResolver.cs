// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ReactiveUI.Primitives.Disposables;

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
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "Generic service-location API; the service type is supplied explicitly by callers, so type inference cannot apply by design.")]
public sealed class InstanceGenericFirstDependencyResolver : IDependencyResolver
{
    /// <summary>Gate protecting <see cref="_callbackRegistry"/> structure and <see cref="_callbackCount"/> updates.</summary>
    /// <remarks>
    /// Held only while mutating the registry dictionary or reading an entry reference; user callbacks always run outside this gate.
    /// </remarks>
    private readonly Lock _callbackGate = new();

    /// <summary>Gate protecting <see cref="_disposalActions"/> additions/enumeration/clearing.</summary>
    private readonly Lock _disposalGate = new();

    /// <summary>Actions executed during <see cref="Dispose()"/> to clean up registered disposables (constants and lazy singletons).</summary>
    /// <remarks>
    /// Mutated under <see cref="_disposalGate"/>. Enumerated and cleared during dispose using a snapshot to avoid holding locks while running user code.
    /// </remarks>
    private readonly List<Action> _disposalActions = [];

    /// <summary>Resolver-local state used by generic-first caches.</summary>
    /// <remarks>
    /// During <see cref="Dispose()"/>, this is swapped for a fresh state to allow the previous state to become unreachable.
    /// </remarks>
    private ResolverState _state = new();

    /// <summary>Registration-change callbacks keyed by <c>(service type, contract)</c>; <see langword="null"/> until the first subscription.</summary>
    /// <remarks>
    /// <para>
    /// Lazily created so resolvers that never subscribe (the common case) allocate no callback state.
    /// The dictionary structure is mutated only under <see cref="_callbackGate"/>; each per-key
    /// <see cref="ArrayHelpers.Entry{TValue}"/> publishes an immutable snapshot that is invoked lock-free outside the gate.
    /// </para>
    /// <para>
    /// Callbacks are scoped to their <c>(type, contract)</c> key and fire only on registration (not unregistration),
    /// matching <see cref="ModernDependencyResolver"/> semantics.
    /// </para>
    /// </remarks>
    private Dictionary<(Type ServiceType, string? Contract), ArrayHelpers.Entry<Action<IDisposable>>>? _callbackRegistry;

    /// <summary>Total number of live registration callbacks; enables a lock-free fast exit from <see cref="NotifyCallbackChanged"/> when zero.</summary>
    /// <remarks>Written under <see cref="_callbackGate"/> via <see cref="Volatile.Write(ref int, int)"/>; read lock-free on the registration hot path.</remarks>
    private int _callbackCount;

    /// <summary>Disposal flag: 0 = not disposed, 1 = disposed.</summary>
    private int _disposed;

    /// <summary>Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class.</summary>
    public InstanceGenericFirstDependencyResolver()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="InstanceGenericFirstDependencyResolver"/> class with bulk registration support.</summary>
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
    public InstanceGenericFirstDependencyResolver(Action<IMutableDependencyResolver>? configure) => ApplyConfiguration(configure);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetService<T>()
    {
        if (HasAnyRegistrations())
        {
            return default;
        }

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        if (container.TryGet(out var service))
        {
            return service;
        }

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        return !registry.HasNonGenericRegistrations(TypeCache<T>.Type) ? default : TryCastAndUnwrap<T>(registry.GetService(TypeCache<T>.Type));
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        if (contractContainer.TryGet(contract, out var contractService))
        {
            return contractService;
        }

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        return !registry.HasNonGenericRegistrations(TypeCache<T>.Type, contract) ? default : TryCastAndUnwrap<T>(registry.GetService(TypeCache<T>.Type, contract));
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType)
    {
        if (HasAnyRegistrations())
        {
            return null;
        }

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        return UnwrapNullServiceType(registry.GetService(serviceType, contract));
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>()
    {
        if (HasAnyRegistrations())
        {
            return [];
        }

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        var genericResults = container.GetAll();

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        var contractResults = contractContainer.GetAll(contract);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        return UnwrapResults(registry.GetServices(serviceType, contract));
    }

    /// <inheritdoc />
    public bool HasRegistration<T>()
    {
        if (HasAnyRegistrations())
        {
            return false;
        }

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        return registry.HasRegistration(serviceType, contract);
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        MarkRegistered();

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        container.Add(factory);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type) can find it.
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<T>.Type, () => factory()!);

        NotifyCallbackChanged(TypeCache<T>.Type, null);
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        contractContainer.Add(factory, contract);

        // Also register in ServiceTypeRegistry so non-generic GetService(Type, contract) can find it.
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<T>.Type, () => factory()!, contract);

        NotifyCallbackChanged(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;

        MarkRegistered();

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(serviceType, factory);
        registry.TrackNonGenericRegistration(serviceType);

        NotifyCallbackChanged(serviceType, null);
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

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(serviceType, factory, contract);
        registry.TrackNonGenericRegistration(serviceType, contract);

        NotifyCallbackChanged(serviceType, contract);
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        var container = ContainerCache<TService>.Get(Volatile.Read(ref _state));
        container.Add(static () => new TImplementation());

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<TService>.Type, static () => new TImplementation());

        NotifyCallbackChanged(TypeCache<TService>.Type, null);
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

        var contractContainer = ContractContainerCache<TService>.Get(Volatile.Read(ref _state));
        contractContainer.Add(static () => new TImplementation(), contract);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<TService>.Type, static () => new TImplementation(), contract);

        NotifyCallbackChanged(TypeCache<TService>.Type, contract);
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        AddDisposableIfNeeded(value);

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        container.Add(value!);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<T>.Type, () => value!);

        NotifyCallbackChanged(TypeCache<T>.Type, null);
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        contractContainer.Add(value!, contract);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.Register(TypeCache<T>.Type, () => value!, contract);

        NotifyCallbackChanged(TypeCache<T>.Type, contract);
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
        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
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

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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

        NotifyCallbackChanged(TypeCache<T>.Type, null);
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton(valueFactory);
            return;
        }

        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        MarkRegistered();

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Wrap lazy value access to dispose and throw if resolver was disposed during construction.
        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
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

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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

        NotifyCallbackChanged(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        container.RemoveCurrent();

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterCurrent(TypeCache<T>.Type);
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        contractContainer.RemoveCurrent(contract);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterCurrent(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterCurrent(serviceType);
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

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterCurrent(serviceType, contract);
    }

    /// <inheritdoc />
    public void UnregisterAll<T>()
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        container.Clear();

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterAll(TypeCache<T>.Type);
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

        var contractContainer = ContractContainerCache<T>.Get(Volatile.Read(ref _state));
        contractContainer.Clear(contract);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterAll(TypeCache<T>.Type, contract);
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterAll(serviceType);
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

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
        registry.UnregisterAll(serviceType, contract);
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback<T>(null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _disposed) != 0, this);

        var disp = AddCallback(TypeCache<T>.Type, contract, callback);

        // Invoke callback once per existing registration (matches ModernDependencyResolver behavior).
        // Use counts to avoid invoking factories (especially lazy singletons).
        var container = ContainerCache<T>.Get(Volatile.Read(ref _state));
        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));

        var count = contract is null
            ? container.GetCount() + registry.GetCount(TypeCache<T>.Type)
            : ContractContainerCache<T>.Get(Volatile.Read(ref _state)).GetCount(contract) + registry.GetCount(TypeCache<T>.Type, contract);

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

        var disp = AddCallback(serviceType, contract, callback);

        var registry = ServiceTypeRegistryCache.Get(Volatile.Read(ref _state));
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
        var callbacks = DrainCallbacks();

        Action[]? disposalActionsSnapshot;
        lock (_disposalGate)
        {
            disposalActionsSnapshot = _disposalActions.Count == 0 ? null : [.. _disposalActions];
            _disposalActions.Clear();
        }

        // Dispose registration callbacks (exceptions suppressed).
        for (var i = 0; i < callbacks.Length; i++)
        {
            var callback = callbacks[i];
            ResolverExceptionHelpers.RunSwallowingExceptions(() => callback(EmptyDisposable.Instance));
        }

        // Execute disposal actions for constants and lazy singletons (exceptions suppressed).
        if (disposalActionsSnapshot is not null)
        {
            for (var i = 0; i < disposalActionsSnapshot.Length; i++)
            {
                ResolverExceptionHelpers.RunSwallowingExceptions(disposalActionsSnapshot[i]);
            }
        }

        // Clear the non-generic registry associated with the old state.
        // This ensures non-generic registrations do not leak across resolver lifetimes.
        var registry = ServiceTypeRegistryCache.Get(oldState);
        registry.Clear();
    }

    /// <summary>Unwraps non-generic results and filters out nulls.</summary>
    /// <param name="results">The raw array returned by the type registry.</param>
    /// <returns>
    /// An array containing unwrapped non-null results. Returning an array avoids iterator allocations.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    private static object[] UnwrapResults(object[] results)
    {
        ArgumentExceptionHelper.ThrowIfNull(results);
        return UnwrapArray(results);
    }

    /// <summary>Unwraps and null-filters an array result, reusing the source array when no element changes.</summary>
    /// <param name="arr">The raw array returned by the type registry.</param>
    /// <returns>An array of unwrapped, non-null objects.</returns>
    private static object[] UnwrapArray(object[] arr)
    {
        var count = CountUnwrapped(arr);

        // Reuse the source array only when nothing is filtered and no element changes under unwrap.
        if (count == arr.Length && !AnyChangedUnderUnwrap(arr))
        {
            return arr;
        }

        if (count == 0)
        {
            return [];
        }

        var output = new object[count];
        var idx = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            var v = UnwrapNullServiceType(arr[i]);
            if (v is not null)
            {
                output[idx] = v;
                idx++;
            }
        }

        return output;
    }

    /// <summary>Counts the elements of <paramref name="arr"/> that survive unwrap/null-filtering.</summary>
    /// <param name="arr">The raw array returned by the type registry.</param>
    /// <returns>The number of non-null unwrapped elements.</returns>
    private static int CountUnwrapped(object[] arr)
    {
        var count = 0;
        for (var i = 0; i < arr.Length; i++)
        {
            if (UnwrapNullServiceType(arr[i]) is not null)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>Determines whether unwrapping any element of <paramref name="arr"/> yields a different reference.</summary>
    /// <param name="arr">The raw array returned by the type registry.</param>
    /// <returns><see langword="true"/> if any element changes under unwrap; otherwise <see langword="false"/>.</returns>
    private static bool AnyChangedUnderUnwrap(object[] arr)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            if (!ReferenceEquals(arr[i], UnwrapNullServiceType(arr[i])))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Unwraps a <see cref="NullServiceType"/> marker by invoking its factory; otherwise returns the value unchanged.</summary>
    /// <param name="value">The resolved value, possibly a <see cref="NullServiceType"/> marker.</param>
    /// <returns>The unwrapped value.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static object? UnwrapNullServiceType(object? value) => value is NullServiceType nullServiceType ? nullServiceType.Factory() : value;

    /// <summary>Unwraps a possible <see cref="NullServiceType"/> marker and casts the result to <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The expected service type.</typeparam>
    /// <param name="value">The resolved value to unwrap and cast.</param>
    /// <returns>The value cast to <typeparamref name="T"/>, or <see langword="default"/> when it is not compatible.</returns>
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
                combined[idx] = typed;
                idx++;
            }
        }

        return combined;
    }

    /// <summary>Applies constructor-supplied bulk registrations against this fully-constructed instance.</summary>
    /// <param name="configure">Optional delegate that performs registrations against this resolver.</param>
    private void ApplyConfiguration(Action<IMutableDependencyResolver>? configure) => configure?.Invoke(this);

    /// <summary>Returns <see langword="true"/> if the resolver has never had a registration added.</summary>
    /// <returns>
    /// <see langword="true"/> when no registrations have been added; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasAnyRegistrations() => !_state.HasAnyRegistrations;

    /// <summary>Marks the resolver as having at least one registration.</summary>
    /// <remarks>
    /// This is an optimization: many hot-path resolve operations can skip cache/registry lookups when the resolver is still "virgin".
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MarkRegistered()
    {
        if (_state.HasAnyRegistrations)
        {
            return;
        }

        _ = _state.MarkHasRegistrations();
    }

    /// <summary>Registers a callback under the supplied <c>(service type, contract)</c> key and returns a disposable that removes it.</summary>
    /// <param name="serviceType">The service type the callback is scoped to.</param>
    /// <param name="contract">The optional contract the callback is scoped to.</param>
    /// <param name="callback">The callback to add.</param>
    /// <returns>A disposable that unsubscribes the callback when disposed.</returns>
    private ActionDisposable AddCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        var key = (serviceType, contract);

        lock (_callbackGate)
        {
            _callbackRegistry ??= [];
            var entry = ResolverDictionaryHelpers.GetOrAddValue(_callbackRegistry, key);

            entry.Add(callback);
            Volatile.Write(ref _callbackCount, _callbackCount + 1);
        }

        return new(() => RemoveCallback(key, callback));
    }

    /// <summary>Atomically detaches and returns every registered callback for one-shot teardown invocation.</summary>
    /// <returns>A flattened snapshot of all registered callbacks; an empty array when none are subscribed.</returns>
    private Action<IDisposable>[] DrainCallbacks()
    {
        lock (_callbackGate)
        {
            try
            {
                if (_callbackRegistry is null || _callbackCount == 0)
                {
                    return [];
                }

                // Flatten every key's callbacks into a single list so each is invoked exactly once on teardown.
                var flattened = new List<Action<IDisposable>>(_callbackCount);
                foreach (var entry in _callbackRegistry.Values)
                {
                    entry.CopyItemsTo(flattened);
                }

                return [.. flattened];
            }
            finally
            {
                _callbackRegistry = null;
                Volatile.Write(ref _callbackCount, 0);
            }
        }
    }

    /// <summary>Removes a previously registered callback for the supplied key.</summary>
    /// <param name="key">The <c>(service type, contract)</c> key the callback was registered under.</param>
    /// <param name="callback">The callback to remove.</param>
    private void RemoveCallback((Type ServiceType, string? Contract) key, Action<IDisposable> callback)
    {
        lock (_callbackGate)
        {
            if (_callbackRegistry is null || !_callbackRegistry.TryGetValue(key, out var entry))
            {
                return;
            }

            if (!entry.Remove(callback))
            {
                return;
            }

            Volatile.Write(ref _callbackCount, _callbackCount - 1);

            if (entry.Count == 0)
            {
                _ = _callbackRegistry.Remove(key);
            }
        }
    }

    /// <summary>Notifies callbacks scoped to the registered <c>(service type, contract)</c> key that a registration changed.</summary>
    /// <param name="serviceType">The service type that was just registered.</param>
    /// <param name="contract">The contract that was just registered, if any.</param>
    /// <remarks>
    /// Returns immediately (lock-free) when the resolver is disposed or no callbacks are subscribed.
    /// Otherwise the matching key's immutable snapshot is invoked outside the gate; callback exceptions are suppressed.
    /// </remarks>
    [ExcludeFromCodeCoverage] // Defensive: disposed guard only fires under a concurrent Dispose; the notify body stays covered in NotifyCallbackChangedCore.
    private void NotifyCallbackChanged(Type serviceType, string? contract)
    {
        if (Volatile.Read(ref _disposed) != 0)
        {
            return;
        }

        NotifyCallbackChangedCore(serviceType, contract);
    }

    /// <summary>Invokes callbacks scoped to the <c>(service type, contract)</c> key; the disposed fast-exit is handled by the caller.</summary>
    /// <param name="serviceType">The service type that was just registered.</param>
    /// <param name="contract">The contract that was just registered, if any.</param>
    private void NotifyCallbackChangedCore(Type serviceType, string? contract)
    {
        // Fast path: the overwhelmingly common case has no subscribers, so avoid the gate and the dictionary lookup entirely.
        if (Volatile.Read(ref _callbackCount) == 0)
        {
            return;
        }

        Action<IDisposable>[] snapshot;
        lock (_callbackGate)
        {
            if (_callbackRegistry is null || !_callbackRegistry.TryGetValue((serviceType, contract), out var entry))
            {
                return;
            }

            snapshot = entry.GetSnapshot();
        }

        for (var i = 0; i < snapshot.Length; i++)
        {
            var callback = snapshot[i];
            ResolverExceptionHelpers.RunSwallowingExceptions(() => callback(EmptyDisposable.Instance));
        }
    }

    /// <summary>Adds a disposal action for a constant registration if it implements <see cref="IDisposable"/>.</summary>
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
            _disposalActions.Add(() => ResolverExceptionHelpers.RunSwallowingExceptions(disposable.Dispose));
        }
    }

    /// <summary>Adds a disposal action for a lazy singleton so that the created value is disposed if applicable.</summary>
    /// <typeparam name="T">The lazy singleton value type.</typeparam>
    /// <param name="lazy">The lazy wrapper.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="lazy"/> is <see langword="null"/>.</exception>
    private void AddLazyDisposal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Lazy<T?> lazy)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(lazy);

        lock (_disposalGate)
        {
            _disposalActions.Add(() => ResolverExceptionHelpers.RunSwallowingExceptions(() =>
            {
                if (!lazy.IsValueCreated || lazy.Value is not IDisposable disposable)
                {
                    return;
                }

                disposable.Dispose();
            }));
        }
    }
}
