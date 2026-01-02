// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
    private readonly Lock _gate = new();
#else
    private readonly object _gate = new();
#endif

    /// <summary>
    /// Stores all registration callbacks keyed by (service type, contract).
    /// </summary>
    private readonly Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>> _callbackRegistry;

    /// <summary>
    /// Stores all registered factory delegates keyed by (service type, contract).
    /// This is a copy-on-write snapshot to allow lock-free reads.
    /// </summary>
    private Snapshot? _snapshot;

    /// <summary>
    /// Tracks whether this resolver has already been disposed.
    /// </summary>
    private bool _isDisposed;

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

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);

        List<Action<IDisposable>>? callbacksToRun = null;

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            // Copy-on-write update of the registry for this key only.
            var newRegistry = CloneRegistryShallow(snap.Registry);

            if (!newRegistry.TryGetValue(pair, out var value))
            {
                value = new(4);
                newRegistry[pair] = value;
            }
            else
            {
                // The list is mutable; clone it before mutating.
                value = [.. value];
                newRegistry[pair] = value;
            }

            value.Add(() =>
                isNull
                    ? new NullServiceType(factory)
                    : factory());

            // Capture callbacks for this key (run outside the lock).
            if (_callbackRegistry.TryGetValue(pair, out var callbackList) && callbackList.Count > 0)
            {
                callbacksToRun = [.. callbackList];
            }

            _snapshot = snap with { Registry = newRegistry };
        }

        if (callbacksToRun is null)
        {
            return;
        }

        RunCallbacks(callbacksToRun, pair);
    }

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory) => Register<T>(factory, null);

    /// <inheritdoc />
    public void Register<T>(Func<T?> factory, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var pair = GetKey(typeof(T), contract);

        List<Action<IDisposable>>? callbacksToRun = null;

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            // Copy-on-write update of the registry for this key only.
            var newRegistry = CloneRegistryShallow(snap.Registry);

            if (!newRegistry.TryGetValue(pair, out var value))
            {
                value = new(4);
                newRegistry[pair] = value;
            }
            else
            {
                // The list is mutable; clone it before mutating.
                value = [.. value];
                newRegistry[pair] = value;
            }

            value.Add(() => factory());

            // Capture callbacks for this key (run outside the lock).
            if (_callbackRegistry.TryGetValue(pair, out var callbackList) && callbackList.Count > 0)
            {
                callbacksToRun = [.. callbackList];
            }

            _snapshot = snap with { Registry = newRegistry };
        }

        if (callbacksToRun is null)
        {
            return;
        }

        RunCallbacks(callbacksToRun, pair);
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
            return default;
        }

        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);
        if (!snap.Registry.TryGetValue(pair, out var value))
        {
            return default;
        }

        var ret = value.LastOrDefault();
        object? returnValue = default;
        if (ret != null)
        {
            returnValue = ret();
            if (returnValue is Lazy<object?> lazy)
            {
                return lazy.Value;
            }

            if (returnValue is NullServiceType nullServiceType)
            {
                return nullServiceType.Factory()!;
            }
        }

        return returnValue;
    }

    /// <inheritdoc />
    public T? GetService<T>() => GetService<T>(null);

    /// <inheritdoc />
    public T? GetService<T>(string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return default;
        }

        var pair = GetKey(typeof(T), contract);
        if (!snap.Registry.TryGetValue(pair, out var value))
        {
            return default;
        }

        var ret = value.LastOrDefault();
        if (ret == null)
        {
            return default;
        }

        var returnValue = ret();

        // Unwrap Lazy<T> registered by RegisterLazySingleton
        if (returnValue is Lazy<T> lazyT)
        {
            return lazyT.Value;
        }

        // Fallback for Lazy<object?> from non-generic registrations
        if (returnValue is Lazy<object?> lazy)
        {
            return (T?)lazy.Value;
        }

        return (T?)returnValue;
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
            return [];
        }

        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);
        return !snap.Registry.TryGetValue(pair, out var value) ? [] : value.ConvertAll(x =>
        {
            var v = x()!;
            return v is Lazy<object?> lazy ? lazy.Value! : v;
        });
    }

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>() => GetServices<T>(null);

    /// <inheritdoc />
    public IEnumerable<T> GetServices<T>(string? contract)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return [];
        }

        var pair = GetKey(typeof(T), contract);
        if (!snap.Registry.TryGetValue(pair, out var value))
        {
            return [];
        }

        var result = new List<T>(value.Count);
        foreach (var factory in value)
        {
            var v = factory();

            // Unwrap Lazy<T> registered by RegisterLazySingleton
            if (v is Lazy<T> lazyT)
            {
                result.Add(lazyT.Value!);
            }
            else if (v is Lazy<object?> lazy)
            {
                result.Add((T)lazy.Value!);
            }
            else
            {
                result.Add((T)v!);
            }
        }

        return result;
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) => UnregisterCurrent(serviceType, null);

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            if (!snap.Registry.TryGetValue(pair, out var list))
            {
                return;
            }

            var position = list.Count - 1;
            if (position < 0)
            {
                return;
            }

            // Copy-on-write: clone only the list being modified and publish a new snapshot.
            var newRegistry = CloneRegistryShallow(snap.Registry);
            List<Func<object?>> newList = [.. list];
            newList.RemoveAt(position);
            newRegistry[pair] = newList;

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterCurrent<T>() => UnregisterCurrent<T>(null);

    /// <inheritdoc />
    public void UnregisterCurrent<T>(string? contract)
    {
        var pair = GetKey(typeof(T), contract);

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            if (!snap.Registry.TryGetValue(pair, out var list))
            {
                return;
            }

            var position = list.Count - 1;
            if (position < 0)
            {
                return;
            }

            // Copy-on-write: clone only the list being modified and publish a new snapshot.
            var newRegistry = CloneRegistryShallow(snap.Registry);
            List<Func<object?>> newList = [.. list];
            newList.RemoveAt(position);
            newRegistry[pair] = newList;

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType) => UnregisterAll(serviceType, null);

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        var pair = GetKey(serviceType, contract);

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            // Copy-on-write: publish a new snapshot with an empty list for this key.
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
        var pair = GetKey(typeof(T), contract);

        lock (_gate)
        {
            var snap = _snapshot;
            if (snap is null)
            {
                return;
            }

            // Copy-on-write: publish a new snapshot with an empty list for this key.
            var newRegistry = CloneRegistryShallow(snap.Registry);
            newRegistry[pair] = [];

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) => ServiceRegistrationCallback(serviceType, null, callback);

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

            if (!_callbackRegistry.TryGetValue(pair, out var value))
            {
                value = new(4);
                _callbackRegistry[pair] = value;
            }

            value.Add(callback);
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
        if (snap is null || !snap.Registry.TryGetValue(pair, out var callbackList))
        {
            return disp;
        }

        foreach (var unused in callbackList)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) => ServiceRegistrationCallback<T>(null, callback);

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

            if (!_callbackRegistry.TryGetValue(pair, out var value))
            {
                value = new(4);
                _callbackRegistry[pair] = value;
            }

            value.Add(callback);
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
        if (snap is null || !snap.Registry.TryGetValue(pair, out var callbackList))
        {
            return disp;
        }

        foreach (var unused in callbackList)
        {
            callback(disp);
        }

        return disp;
    }

    /// <inheritdoc />
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() => Register(() => new TImplementation(), typeof(TService), null);

    /// <inheritdoc />
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() => Register(() => new TImplementation(), typeof(TService), contract);

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value)
        where T : class => Register(() => value, typeof(T), null);

    /// <inheritdoc />
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class => Register(() => value, typeof(T), contract);

    /// <inheritdoc />
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        var val = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Register the Lazy object itself to avoid triggering evaluation during disposal
        Register(() => val, typeof(T), null);
    }

    /// <inheritdoc />
    public void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);
        var val = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        // Register the Lazy object itself to avoid triggering evaluation during disposal
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
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            // Atomically prevent any further operations.
            var snap = Interlocked.Exchange(ref _snapshot, null);

            // Dispose of all IDisposable callbacks
            foreach (var pair in _callbackRegistry)
            {
                foreach (var callback in pair.Value)
                {
                    using var disp = new BooleanDisposable();
                    callback(disp);
                }
            }

            _callbackRegistry.Clear();

            // Clear the registry and dispose of all factory registrations
            if (snap is not null)
            {
                foreach (var pair in snap.Registry.Values)
                {
                    foreach (var factory in pair)
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
                                // Skip if the lazy value is not created yet, don't create it just for disposal :)
                                continue;
                            }
                        }

                        if (item is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
        }

        _isDisposed = true;
    }

    /// <summary>
    /// Generates a duplicate of the registry with all the current registrations.
    /// </summary>
    private static Dictionary<(Type serviceType, string? contract), List<Func<object?>>> CloneRegistryShallow(
        Dictionary<(Type serviceType, string? contract), List<Func<object?>>> source)
    {
        // Shallow-clone dictionary: lists are cloned only when mutated.
        var clone = new Dictionary<(Type serviceType, string? contract), List<Func<object?>>>(source.Count);
        foreach (var kvp in source)
        {
            clone[kvp.Key] = kvp.Value;
        }

        return clone;
    }

    private static (Type type, string contract) GetKey(
        Type? serviceType,
        string? contract = null) =>
        (serviceType!, contract ?? string.Empty);

    private void RunCallbacks(List<Action<IDisposable>> callbacksToRun, (Type type, string contract) pair)
    {
        List<Action<IDisposable>>? toRemove = null;

        foreach (var callback in callbacksToRun)
        {
            using var disp = new BooleanDisposable();

            callback(disp);

            if (disp.IsDisposed)
            {
                (toRemove ??= []).Add(callback);
            }
        }

        if (toRemove is not null)
        {
            lock (_gate)
            {
                foreach (var c in toRemove)
                {
                    if (_callbackRegistry.TryGetValue(pair, out var list))
                    {
                        _ = list.Remove(c);
                    }
                }
            }
        }
    }

    /// <summary>
    /// A copy-on-write snapshot of the registry.
    /// </summary>
    /// <param name="Registry">A registry of services.</param>
    private sealed record Snapshot(Dictionary<(Type serviceType, string? contract), List<Func<object?>>> Registry);
}
