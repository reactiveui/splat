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
        var reg = registry is not null ?
            registry.ToDictionary(k => k.Key, v => v.Value.ToList()) :
            new Dictionary<(Type serviceType, string? contract), List<Func<object?>>>();

        _snapshot = new Snapshot(reg);

        _callbackRegistry = new Dictionary<(Type serviceType, string? contract), List<Action<IDisposable>>>();
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract = null)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return false;
        }

        serviceType ??= typeof(NullServiceType);

        var pair = GetKey(serviceType, contract);
        return snap.Registry.TryGetValue(pair, out var registrations) && registrations.Count > 0;
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var isNull = serviceType is null;

        serviceType ??= typeof(NullServiceType);

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
                value = [];
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

    /// <inheritdoc />
    public object? GetService(Type? serviceType, string? contract = null)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return default;
        }

        serviceType ??= typeof(NullServiceType);

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
    public IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        var snap = Volatile.Read(ref _snapshot);
        if (snap is null)
        {
            return [];
        }

        serviceType ??= typeof(NullServiceType);

        var pair = GetKey(serviceType, contract);
        return !snap.Registry.TryGetValue(pair, out var value) ? Array.Empty<object>() : value.ConvertAll(x =>
        {
            var v = x()!;
            return v is Lazy<object?> lazy ? lazy.Value! : v;
        });
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

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
            var newList = list.ToList();
            newList.RemoveAt(position);
            newRegistry[pair] = newList;

            _snapshot = snap with { Registry = newRegistry };
        }
    }

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

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
                value = [];
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

        foreach (var s in callbackList)
        {
            callback(disp);
        }

        return disp;
    }

    /// <summary>
    /// Generates a duplicate of the resolver with all the current registrations.
    /// Useful if you want to generate temporary resolver using the <see cref="DependencyResolverMixins.WithResolver(IDependencyResolver, bool)"/> method.
    /// </summary>
    /// <returns>The newly generated <see cref="ModernDependencyResolver"/> class with the current registrations.</returns>
    public ModernDependencyResolver Duplicate()
    {
        var snap = Volatile.Read(ref _snapshot);
        return snap is null ? new ModernDependencyResolver() : new ModernDependencyResolver(snap.Registry);
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

    /// <summary>
    /// A copy-on-write snapshot of the registry.
    /// </summary>
    /// <param name="Registry">A registry of services.</param>
    private sealed record Snapshot(Dictionary<(Type serviceType, string? contract), List<Func<object?>>> Registry);
}
