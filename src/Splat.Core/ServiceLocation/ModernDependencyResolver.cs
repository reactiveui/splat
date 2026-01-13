// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// <para>
/// This class is a dependency resolver written for modern C# 5.0 times.
/// It implements all registrations via a Factory method. With the power
/// of Closures, you can actually implement most lifetime styles (i.e.
/// construct per call, lazy construct, singleton) using this.
/// </para>
/// <para>
/// Unless you have a very compelling reason not to, this is the only class
/// you need in order to do dependency resolution, don't bother with using
/// a full IoC container.
/// </para>
/// </summary>
public class ModernDependencyResolver : IDependencyResolver
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// Synchronization primitive guarding mutations to the resolver's internal state.
    /// </summary>
    /// <remarks>
    /// This protects updates to <see cref="_snapshot"/>, <see cref="_callbackRegistry"/>, and <see cref="_disposables"/>.
    /// Reads resolve from <see cref="_snapshot"/> without locking.
    /// </remarks>
    private readonly Lock _gate = new();
#else
    /// <summary>
    /// Synchronization primitive guarding mutations to the resolver's internal state.
    /// </summary>
    /// <remarks>
    /// This protects updates to <see cref="_snapshot"/>, <see cref="_callbackRegistry"/>, and <see cref="_disposables"/>.
    /// Reads resolve from <see cref="_snapshot"/> without locking.
    /// </remarks>
    private readonly object _gate = new();
#endif

    /// <summary>
    /// Stores all registration callbacks keyed by (service type, contract).
    /// </summary>
    /// <remarks>
    /// Callbacks are invoked when a registration is added for the associated key.
    /// Callbacks may dispose the provided token to indicate they should be removed (one-shot semantics).
    /// </remarks>
    private readonly Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>> _callbackRegistry;

    /// <summary>
    /// Tracks singleton instances and lazy wrappers for disposal.
    /// </summary>
    /// <remarks>
    /// Only singletons (from <c>RegisterConstant</c> and <c>RegisterLazySingleton</c>) are tracked here.
    /// Transient services (from <c>Register</c>) are not tracked to avoid instantiating them during disposal.
    /// </remarks>
    private readonly List<object> _disposables;

    /// <summary>
    /// Stores all registered factory delegates keyed by (service type, contract).
    /// </summary>
    /// <remarks>
    /// This is a copy-on-write snapshot to allow lock-free reads:
    /// writers publish a new <see cref="Snapshot"/> instance via assignment under <see cref="_gate"/>;
    /// readers use Volatile Read without locking.
    /// </remarks>
    private Snapshot? _snapshot;

    /// <summary>
    /// Tracks whether this resolver has already been disposed.
    /// </summary>
    /// <remarks>
    /// Used to reject registrations after disposal and to prevent double dispose.
    /// Uses int instead of bool for thread-safe Interlocked operations (0 = not disposed, 1 = disposed).
    /// </remarks>
    private int _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModernDependencyResolver"/> class.
    /// </summary>
    public ModernDependencyResolver()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModernDependencyResolver"/> class.
    /// </summary>
    /// <param name="registry">A registry of services.</param>
    /// <remarks>
    /// When provided, the registry is deep-copied at the list level to avoid sharing mutable lists between resolvers.
    /// </remarks>
    protected ModernDependencyResolver(Dictionary<(Type serviceType, string? contract), List<Func<object?>>>? registry)
    {
        Dictionary<(Type serviceType, string? contract), List<Func<object?>>> reg;
        if (registry is not null)
        {
            reg = new(registry.Count);
            foreach (var kvp in registry)
            {
                reg[kvp.Key] = [.. kvp.Value];
            }
        }
        else
        {
            reg = new(32);
        }

        _snapshot = new(reg);
        _callbackRegistry = new(16);
        _disposables = new(16);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType) => HasRegistration(serviceType, null);

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return false;
        }

        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);
        return snap.Registry.TryGetValue(pair, out var registrations) && registrations.Count > 0;
    }

    /// <inheritdoc />
    public bool HasRegistration<T>() => HasRegistration<T>(null);

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return false;
        }

        var pair = GetKey(typeof(T), contract);
        return snap.Registry.TryGetValue(pair, out var registrations) && registrations.Count > 0;
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType) => Register(factory, serviceType, null);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);

        List<Action<IDisposable>>? callbacksToRun = null;

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            // Copy-on-write update of the registry for this key only.
            var newRegistry = CloneRegistryShallow(snap.Registry);

            if (!newRegistry.TryGetValue(pair, out var list))
            {
                list = new(4);
            }
            else
            {
                // The list is mutable; clone it before mutating.
                list = [.. list];
            }

            // Wrap null-type registrations using NullServiceType.
            list.Add(isNull ? () => new NullServiceType(factory) : factory);

            newRegistry[pair] = list;

            // Capture callbacks for this key (run outside the lock).
            if (_callbackRegistry.TryGetValue(pair, out var callbackList) && callbackList.Count > 0)
            {
                callbacksToRun = [.. callbackList];
            }

            _snapshot = snap with { Registry = newRegistry };
        }

        if (callbacksToRun is not null)
        {
            RunCallbacks(callbacksToRun, pair);
        }
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory) => Register<T>(factory, null);

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        var pair = GetKey(typeof(T), contract);
        List<Action<IDisposable>>? callbacksToRun = null;

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            var newRegistry = CloneRegistryShallow(snap.Registry);

            if (!newRegistry.TryGetValue(pair, out var list))
            {
                list = new(4);
            }
            else
            {
                list = [.. list];
            }

            // Avoid extra closure allocation: store the delegate directly.
            list.Add(() => factory());

            newRegistry[pair] = list;

            if (_callbackRegistry.TryGetValue(pair, out var callbackList) && callbackList.Count > 0)
            {
                callbacksToRun = [.. callbackList];
            }

            _snapshot = snap with { Registry = newRegistry };
        }

        if (callbacksToRun is not null)
        {
            RunCallbacks(callbacksToRun, pair);
        }
    }

    /// <inheritdoc />
    public object? GetService(Type? serviceType) => GetService(serviceType, null);

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            // Resolver has been disposed
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
            return default;
        }

        var pair = GetKey(serviceType, contract);
        if (!snap.Registry.TryGetValue(pair, out var list))
        {
            return default;
        }

        var factory = GetLastFactoryOrDefault(list);
        if (factory is null)
        {
            return default;
        }

        var result = factory();

        // Check again after factory execution as disposal may have occurred during construction
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        return Unwrap(result, allowNullServiceTypeUnwrap: true);
    }

    /// <inheritdoc />
    public T? GetService<T>() => GetService<T>(null);

    /// <inheritdoc />
    public T? GetService<T>(string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            // Resolver has been disposed
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
            return default;
        }

        var pair = GetKey(typeof(T), contract);
        if (!snap.Registry.TryGetValue(pair, out var list))
        {
            return default;
        }

        var factory = GetLastFactoryOrDefault(list);
        if (factory is null)
        {
            return default;
        }

        var result = factory();

        // Check again after factory execution as disposal may have occurred during construction
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        // For generic resolutions we still honor lazy and null-type wrappers to preserve behavior.
        return (T?)Unwrap(result, allowNullServiceTypeUnwrap: true);
    }

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType) => GetServices(serviceType, null);

    /// <inheritdoc />
    public IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            // Resolver has been disposed
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
            return [];
        }

        var pair = GetKey(serviceType, contract);
        if (!snap.Registry.TryGetValue(pair, out var list) || list.Count == 0)
        {
            return [];
        }

        // Preserve existing eager semantics (factory invocation happens during this call).
        // Pre-size to avoid internal List growth.
        var results = new List<object>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            results.Add(Unwrap(list[i](), allowNullServiceTypeUnwrap: true)!);
        }

        // Check again after factory execution as disposal may have occurred during construction
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        return results;
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>() => GetServices<T>(null);

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            // Resolver has been disposed
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);
            return [];
        }

        var pair = GetKey(typeof(T), contract);
        if (!snap.Registry.TryGetValue(pair, out var list) || list.Count == 0)
        {
            return [];
        }

        // Preserve existing eager semantics (factory invocation happens during this call).
        var results = new List<T>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            results.Add((T)Unwrap(list[i](), allowNullServiceTypeUnwrap: true)!);
        }

        // Check again after factory execution as disposal may have occurred during construction
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        return results;
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) => UnregisterCurrent(serviceType, null);

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        var pair = GetKey(serviceType, contract);

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            if (!snap.Registry.TryGetValue(pair, out var list) || list.Count == 0)
            {
                return;
            }

            var newRegistry = CloneRegistryShallow(snap.Registry);
            List<Func<object?>> newList = [.. list];
            newList.RemoveAt(newList.Count - 1);
            newRegistry[pair] = newList;

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>() => UnregisterCurrent<T>(null);

    /// <inheritdoc />
    public void UnregisterCurrent<T>(string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        var pair = GetKey(typeof(T), contract);

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            if (!snap.Registry.TryGetValue(pair, out var list) || list.Count == 0)
            {
                return;
            }

            var newRegistry = CloneRegistryShallow(snap.Registry);
            List<Func<object?>> newList = [.. list];
            newList.RemoveAt(newList.Count - 1);
            newRegistry[pair] = newList;

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType) => UnregisterAll(serviceType, null);

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        serviceType ??= NullServiceType.CachedType;
        var pair = GetKey(serviceType, contract);

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            var newRegistry = CloneRegistryShallow(snap.Registry);
            newRegistry[pair] = [];
            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterAll<T>() => UnregisterAll<T>(null);

    /// <inheritdoc />
    public void UnregisterAll<T>(string? contract)
    {
        ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

        var pair = GetKey(typeof(T), contract);

        lock (_gate)
        {
            ObjectDisposedExceptionHelper.ThrowIf(Volatile.Read(ref _isDisposed) != 0, this);

            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            var newRegistry = CloneRegistryShallow(snap.Registry);
            newRegistry[pair] = [];
            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(serviceType, null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(callback);

        if (Volatile.Read(ref _snapshot) is null)
        {
            return new ActionDisposable(() => { });
        }

        var pair = GetKey(serviceType, contract);

        lock (_gate)
        {
            if (Volatile.Read(ref _snapshot) is null)
            {
                return new ActionDisposable(() => { });
            }

            if (!_callbackRegistry.TryGetValue(pair, out var list))
            {
                list = new(4);
                _callbackRegistry[pair] = list;
            }

            list.Add(callback);
        }

        var disp = new ActionDisposable(() =>
        {
            lock (_gate)
            {
                if (_callbackRegistry.TryGetValue(pair, out var list))
                {
                    _ = list.Remove(callback);
                }
            }
        });

        // Preserve original behavior: invoke callback once per existing registration.
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null || !snap.Registry.TryGetValue(pair, out var registrations))
        {
            return disp;
        }

        for (var i = 0; i < registrations.Count; i++)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback<T>(null, callback);

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);

        if (Volatile.Read(ref _snapshot) is null)
        {
            return new ActionDisposable(() => { });
        }

        var pair = GetKey(typeof(T), contract);

        lock (_gate)
        {
            if (Volatile.Read(ref _snapshot) is null)
            {
                return new ActionDisposable(() => { });
            }

            if (!_callbackRegistry.TryGetValue(pair, out var list))
            {
                list = new(4);
                _callbackRegistry[pair] = list;
            }

            list.Add(callback);
        }

        var disp = new ActionDisposable(() =>
        {
            lock (_gate)
            {
                if (_callbackRegistry.TryGetValue(pair, out var list))
                {
                    _ = list.Remove(callback);
                }
            }
        });

        // Preserve original behavior: invoke callback once per existing registration.
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null || !snap.Registry.TryGetValue(pair, out var registrations))
        {
            return disp;
        }

        for (var i = 0; i < registrations.Count; i++)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() =>
        Register(static () => new TImplementation(), typeof(TService), null);

    /// <inheritdoc />
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() =>
        Register(static () => new TImplementation(), typeof(TService), contract);

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        if (value is not null)
        {
            lock (_gate)
            {
                _disposables.Add(value);
            }
        }

        Register(() => value, typeof(T), null);
    }

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        if (value is not null)
        {
            lock (_gate)
            {
                _disposables.Add(value);
            }
        }

        Register(() => value, typeof(T), contract);
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        // Use Lazy<object?> so non-generic GetService can unwrap without reflection.
        var val = new Lazy<object?>(() => valueFactory(), LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_gate)
        {
            _disposables.Add(val);
        }

        // Register the Lazy object itself so Dispose can check IsValueCreated.
        Register(() => val, typeof(T), null);
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var val = new Lazy<object?>(() => valueFactory(), LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_gate)
        {
            _disposables.Add(val);
        }

        Register(() => val, typeof(T), contract);
    }

    /// <summary>
    /// Generates a duplicate of the resolver with all the current registrations.
    /// Useful if you want to generate temporary resolver using the <see cref="DependencyResolverMixins.WithResolver(IDependencyResolver, bool)"/> method.
    /// </summary>
    /// <returns>The newly generated <see cref="ModernDependencyResolver"/> class with the current registrations.</returns>
    public ModernDependencyResolver Duplicate()
    {
        var snap = Volatile.Read(ref _snapshot);
        return snap is null ? new() : new ModernDependencyResolver(snap.Registry);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of all managed memory from this class.
    /// </summary>
    /// <param name="isDisposing">If we are currently disposing managed resources.</param>
    protected virtual void Dispose(bool isDisposing)
    {
        if (Volatile.Read(ref _isDisposed) != 0)
        {
            return;
        }

        if (isDisposing)
        {
            // Snapshot state under the gate to prevent concurrent mutation races,
            // but execute user code (callbacks / Dispose) outside the lock.
            Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>>? callbacksSnapshot;
            List<object>? disposablesSnapshot;

            lock (_gate)
            {
                // Use Interlocked to atomically set disposed flag and prevent double dispose
                var wasDisposed = Interlocked.Exchange(ref _isDisposed, 1);
                if (wasDisposed != 0)
                {
                    return;
                }

                // Set disposed flag BEFORE clearing snapshot to ensure GetService throws
                // (already set by Interlocked.Exchange above)

                // Atomically prevent any further operations on the registry.
                _ = Interlocked.Exchange(ref _snapshot, null);

                callbacksSnapshot = _callbackRegistry.Count == 0 ? null : new(_callbackRegistry);
                _callbackRegistry.Clear();

                disposablesSnapshot = _disposables.Count == 0 ? null : [.. _disposables];
                _disposables.Clear();
            }

            // Dispose of all IDisposable callbacks (exceptions suppressed).
            if (callbacksSnapshot is not null)
            {
                foreach (var kvp in callbacksSnapshot)
                {
                    var list = kvp.Value;
                    for (var i = 0; i < list.Count; i++)
                    {
                        try
                        {
                            using var disp = new BooleanDisposable();
                            list[i](disp);
                        }
                        catch
                        {
                            // Ignore exceptions during disposal.
                        }
                    }
                }
            }

            // Dispose only tracked singletons (exceptions suppressed).
            if (disposablesSnapshot is not null)
            {
                for (var i = 0; i < disposablesSnapshot.Count; i++)
                {
                    try
                    {
                        var item = disposablesSnapshot[i];

                        // Lazy wrapper from RegisterLazySingleton.
                        if (item is Lazy<object?> lazy)
                        {
                            // Only dispose if value was already created.
                            // Don't force creation just to dispose it.
                            // Services constructed during disposal are handled by the
                            // post-construction check in GetService/Unwrap.
                            if (lazy.IsValueCreated && lazy.Value is IDisposable disposable)
                            {
                                disposable.Dispose();
                            }

                            continue;
                        }

                        // Singleton instance from RegisterConstant.
                        (item as IDisposable)?.Dispose();
                    }
                    catch
                    {
                        // Ignore exceptions during disposal.
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a shallow clone of a registry dictionary.
    /// </summary>
    /// <param name="source">The source dictionary to clone.</param>
    /// <returns>
    /// A new dictionary instance containing the same keys and list references as <paramref name="source"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Lists are cloned only when mutated (copy-on-write at the list level).
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Dictionary<(Type serviceType, string? contract), List<Func<object?>>> CloneRegistryShallow(
        Dictionary<(Type serviceType, string? contract), List<Func<object?>>> source)
    {
        ArgumentExceptionHelper.ThrowIfNull(source);

        var clone = new Dictionary<(Type serviceType, string? contract), List<Func<object?>>>(source.Count);
        foreach (var kvp in source)
        {
            clone[kvp.Key] = kvp.Value;
        }

        return clone;
    }

    /// <summary>
    /// Produces the internal dictionary key for a given service type and contract.
    /// </summary>
    /// <param name="serviceType">The service type. Must not be <see langword="null"/>.</param>
    /// <param name="contract">The optional contract. If <see langword="null"/>, an empty string is used.</param>
    /// <returns>A normalized key consisting of the service type and normalized contract string.</returns>
    /// <exception cref="NullReferenceException">
    /// Thrown if <paramref name="serviceType"/> is <see langword="null"/>. Callers normalize null service types before calling.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (Type type, string contract) GetKey(Type? serviceType, string? contract = null) =>
        (serviceType!, contract ?? string.Empty);

    /// <summary>
    /// Returns the last factory delegate in a registration list, or <see langword="null"/> if the list is empty.
    /// </summary>
    /// <param name="list">The registration list.</param>
    /// <returns>The last factory in <paramref name="list"/>, or <see langword="null"/> if none exists.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Func<object?>? GetLastFactoryOrDefault(List<Func<object?>> list)
    {
        ArgumentExceptionHelper.ThrowIfNull(list);
        var count = list.Count;
        return count == 0 ? null : list[count - 1];
    }

    /// <summary>
    /// Unwraps wrapper objects produced by registration helpers.
    /// </summary>
    /// <param name="value">The raw value returned from a registration factory.</param>
    /// <param name="allowNullServiceTypeUnwrap">
    /// If <see langword="true"/>, unwraps <see cref="NullServiceType"/> by invoking <see cref="NullServiceType.Factory"/>.
    /// </param>
    /// <returns>
    /// The unwrapped value, or the original <paramref name="value"/> if no unwrapping applies.
    /// </returns>
    /// <remarks>
    /// This preserves the current behavior:
    /// <list type="bullet">
    /// <item><description>Unwraps <see cref="Lazy{T}"/> produced by <c>RegisterLazySingleton</c>.</description></item>
    /// <item><description>Unwraps <see cref="NullServiceType"/> produced by null-type registrations.</description></item>
    /// </list>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object? Unwrap(object? value, bool allowNullServiceTypeUnwrap)
    {
        if (value is Lazy<object?> lazy)
        {
            var result = lazy.Value;

            // If resolver was disposed during lazy construction, dispose the value and throw.
            // This matches Microsoft.Extensions.DependencyInjection behavior.
            // Use volatile read to ensure visibility across threads without locking.
            if (Volatile.Read(ref _isDisposed) != 0)
            {
                (result as IDisposable)?.Dispose();
                ObjectDisposedExceptionHelper.ThrowIf(true, this);
            }

            return result;
        }

        if (allowNullServiceTypeUnwrap && value is NullServiceType nst)
        {
            return nst.Factory()!;
        }

        return value;
    }

    /// <summary>
    /// Invokes registration callbacks and removes one-shot callbacks from the registry.
    /// </summary>
    /// <param name="callbacksToRun">The callbacks to invoke. This list must not be mutated by this method.</param>
    /// <param name="pair">The registry key the callbacks correspond to.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="callbacksToRun"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Callbacks are invoked outside the resolver gate to avoid deadlocks and to prevent user code
    /// from blocking registration writes. A callback may dispose the supplied token to indicate it should be removed.
    /// </remarks>
    private void RunCallbacks(List<Action<IDisposable>> callbacksToRun, (Type type, string contract) pair)
    {
        ArgumentExceptionHelper.ThrowIfNull(callbacksToRun);

        List<Action<IDisposable>>? toRemove = null;

        for (var i = 0; i < callbacksToRun.Count; i++)
        {
            var callback = callbacksToRun[i];

            using var disp = new BooleanDisposable();
            callback(disp);

            if (disp.IsDisposed)
            {
                (toRemove ??= []).Add(callback);
            }
        }

        if (toRemove is null)
        {
            return;
        }

        lock (_gate)
        {
            if (_callbackRegistry.TryGetValue(pair, out var list))
            {
                for (var i = 0; i < toRemove.Count; i++)
                {
                    _ = list.Remove(toRemove[i]);
                }
            }
        }
    }

    /// <summary>
    /// A copy-on-write snapshot of the registry.
    /// </summary>
    /// <param name="Registry">A registry of services keyed by (service type, contract).</param>
    /// <remarks>
    /// This type enables lock-free reads: readers access an immutable reference to the dictionary,
    /// while writers publish a new snapshot after applying mutations.
    /// </remarks>
    private sealed record Snapshot(Dictionary<(Type serviceType, string? contract), List<Func<object?>>> Registry);
}
