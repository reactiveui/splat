// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-resolver registry for non-generic service types accessed via <see cref="Type"/> parameters.
/// </summary>
/// <remarks>
/// <para>
/// This cache is used by instance-scoped resolvers to support the non-generic service APIs
/// (for example, <c>GetService(Type)</c> and <c>GetServices(Type)</c>) without leaking resolver state.
/// </para>
/// <para>
/// Registries are keyed by <see cref="ResolverState"/> using <see cref="ConditionalWeakTable{TKey,TValue}"/>,
/// so all per-resolver registrations are eligible for garbage collection when the resolver state is no longer referenced.
/// </para>
/// </remarks>
internal static class ServiceTypeRegistryCache
{
    /// <summary>
    /// Maps resolver state to its per-resolver registry.
    /// </summary>
    private static readonly ConditionalWeakTable<ResolverState, Registry> Registries = new();

    /// <summary>
    /// Gets or creates the registry for the specified resolver state.
    /// </summary>
    /// <param name="state">Resolver state used as the cache key.</param>
    /// <returns>The per-resolver <see cref="Registry"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Registry Get(ResolverState state)
    {
        ArgumentExceptionHelper.ThrowIfNull(state);
        return Registries.GetOrCreateValue(state);
    }

    /// <summary>
    /// Per-resolver registry for non-generic service registrations keyed by (<see cref="Type"/>, contract).
    /// </summary>
    internal sealed class Registry
    {
        /// <summary>
        /// Gate protecting mutation and snapshot publication of <see cref="_nonGenericRegistrationSet"/> /
        /// <see cref="_nonGenericRegistrations"/>.
        /// </summary>
        /// <remarks>
        /// On .NET 9+, <c>lock(_nonGenericGate)</c> benefits from the fast-lock path when this is a Lock instance.
        /// </remarks>
#if NET9_0_OR_GREATER
        private readonly Lock _nonGenericGate = new();
#else
        private readonly object _nonGenericGate = new();
#endif

        /// <summary>
        /// Registration storage by (service type, contract).
        /// </summary>
        /// <remarks>
        /// Each value is an entry containing a mutable list of factories and a lazily-built snapshot array.
        /// </remarks>
        private readonly ConcurrentDictionary<(Type ServiceType, string? Contract), ArrayHelpers.Entry<Func<object?>>> _entries = new();

        /// <summary>
        /// Mutable set of tracked non-generic registrations; guarded by <see cref="_nonGenericGate"/>.
        /// </summary>
        private readonly HashSet<(Type ServiceType, string? Contract)> _nonGenericRegistrationSet = [];

        /// <summary>
        /// Published snapshot of tracked non-generic registrations for lock-free reads.
        /// </summary>
        /// <remarks>
        /// Readers use Volatile Read and perform <see cref="HashSet{T}.Contains(T)"/> without taking locks.
        /// Writers publish a new snapshot under <see cref="_nonGenericGate"/>.
        /// </remarks>
        private HashSet<(Type ServiceType, string? Contract)> _nonGenericRegistrations = [];

        /// <summary>
        /// Returns whether any non-generic registrations exist for the specified type and contract.
        /// </summary>
        /// <param name="serviceType">Service type to test.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <returns><see langword="true"/> if registrations exist; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasNonGenericRegistrations(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            // Single volatile read; empty set fast path.
            var snapshot = Volatile.Read(ref _nonGenericRegistrations);
            return snapshot.Count != 0 && snapshot.Contains((serviceType, contract));
        }

        /// <summary>
        /// Tracks the existence of a non-generic registration for a given type and contract.
        /// </summary>
        /// <param name="serviceType">Service type being registered.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public void TrackNonGenericRegistration(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            lock (_nonGenericGate)
            {
                _nonGenericRegistrationSet.Add((serviceType, contract));
                PublishNonGenericSnapshot_NoThrow();
            }
        }

        /// <summary>
        /// Registers a factory for a service type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type being registered.</param>
        /// <param name="factory">Factory delegate that produces instances.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> or <paramref name="factory"/> is <see langword="null"/>.</exception>
        public void Register(Type serviceType, Func<object?> factory, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);
            ArgumentExceptionHelper.ThrowIfNull(factory);

            var key = (serviceType, contract);
            var entry = _entries.GetOrAdd(key, static _ => new());

            lock (entry.Gate)
            {
                entry.List.Add(factory);
                entry.Version++;
            }
        }

        /// <summary>
        /// Resolves the most recently registered service for a type and optional contract (last registration wins).
        /// </summary>
        /// <param name="serviceType">Service type to resolve.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <returns>The resolved instance, or <see langword="null"/> when no registration exists or the factory returns <see langword="null"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public object? GetService(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return null;
            }

            var factories = EnsureSnapshot(entry);
            if (factories.Length == 0)
            {
                return null;
            }

            // Invoke user code outside locks.
            return factories[factories.Length - 1].Invoke();
        }

        /// <summary>
        /// Resolves all services for a type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type to resolve.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <returns>An array of resolved services (excluding nulls). Returns an empty array when none exist.</returns>
        /// <remarks>
        /// Factories are invoked during materialization; exceptions propagate to the caller.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public object[] GetServices(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return [];
            }

            var factories = EnsureSnapshot(entry);
            return factories.Length == 0 ? [] : Materialize(factories);
        }

        /// <summary>
        /// Returns whether any registrations exist for a type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type to test.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <returns><see langword="true"/> if at least one registration exists; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public bool HasRegistration(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return false;
            }

            // Cheap path: snapshot length when available; otherwise list count under gate.
            var snapshot = Volatile.Read(ref entry.Snapshot);
            if (snapshot is not null)
            {
                return snapshot.Length != 0;
            }

            lock (entry.Gate)
            {
                return entry.List.Count != 0;
            }
        }

        /// <summary>
        /// Gets the number of registrations for a type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type to count.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <returns>Number of registrations.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public int GetCount(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return 0;
            }

            var snapshot = Volatile.Read(ref entry.Snapshot);
            if (snapshot is not null)
            {
                return snapshot.Length;
            }

            lock (entry.Gate)
            {
                return entry.List.Count;
            }
        }

        /// <summary>
        /// Unregisters the most recent registration for a type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type to unregister.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public void UnregisterCurrent(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            var key = (serviceType, contract);
            if (!_entries.TryGetValue(key, out var entry))
            {
                return;
            }

            var becameEmpty = false;

            lock (entry.Gate)
            {
                var list = entry.List;
                var count = list.Count;
                if (count == 0)
                {
                    return;
                }

                list.RemoveAt(count - 1);
                entry.Version++;

                becameEmpty = list.Count == 0;
            }

            if (becameEmpty)
            {
                _entries.TryRemove(key, out _);

                lock (_nonGenericGate)
                {
                    _nonGenericRegistrationSet.Remove(key);
                    PublishNonGenericSnapshot_NoThrow();
                }
            }
        }

        /// <summary>
        /// Unregisters all registrations for a type and optional contract.
        /// </summary>
        /// <param name="serviceType">Service type to unregister.</param>
        /// <param name="contract">Optional contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
        public void UnregisterAll(Type serviceType, string? contract = null)
        {
            ArgumentExceptionHelper.ThrowIfNull(serviceType);

            var key = (serviceType, contract);
            if (!_entries.TryRemove(key, out _))
            {
                return;
            }

            lock (_nonGenericGate)
            {
                _nonGenericRegistrationSet.Remove(key);
                PublishNonGenericSnapshot_NoThrow();
            }
        }

        /// <summary>
        /// Clears all registrations and tracked non-generic registrations from this registry.
        /// </summary>
        public void Clear()
        {
            _entries.Clear();

            lock (_nonGenericGate)
            {
                _nonGenericRegistrationSet.Clear();
                Volatile.Write(ref _nonGenericRegistrations, []);
            }
        }

        /// <summary>
        /// Returns a snapshot of all registered factories, intended for disposal enumeration.
        /// </summary>
        /// <returns>
        /// An array containing all registered factories across all entries at the time of the call.
        /// </returns>
        /// <remarks>
        /// This method does not invoke factories. It snapshots the current lists under each entry gate.
        /// </remarks>
        public Func<object?>[] GetAllFactoriesForDisposal()
        {
            // Snapshot dictionary entries once (avoids enumerator invalidation if the dictionary is modified concurrently).
            var entriesSnapshot = _entries.ToArray();

            // First pass: count total factories.
            var total = 0;
            for (var i = 0; i < entriesSnapshot.Length; i++)
            {
                var entry = entriesSnapshot[i].Value;

                lock (entry.Gate)
                {
                    total += entry.List.Count;
                }
            }

            if (total == 0)
            {
                return [];
            }

            // Second pass: fill array.
            var result = new Func<object?>[total];
            var idx = 0;

            for (var i = 0; i < entriesSnapshot.Length; i++)
            {
                var entry = entriesSnapshot[i].Value;

                lock (entry.Gate)
                {
                    var list = entry.List;
                    for (var j = 0; j < list.Count; j++)
                    {
                        result[idx++] = list[j];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ensures the snapshot for an entry is current, rebuilding it if stale.
        /// </summary>
        /// <param name="entry">Entry containing the mutable list and snapshot metadata.</param>
        /// <returns>A snapshot of the current factory list.</returns>
        /// <remarks>
        /// Fast path is lock-free. Snapshot rebuild occurs under <c>lock(entry.Gate)</c>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object?>[] EnsureSnapshot(ArrayHelpers.Entry<Func<object?>> entry)
        {
            var version = Volatile.Read(ref entry.Version);
            var snapshot = Volatile.Read(ref entry.Snapshot);

            if (snapshot is not null && Volatile.Read(ref entry.SnapshotVersion) == version)
            {
                return snapshot;
            }

            lock (entry.Gate)
            {
                snapshot = entry.Snapshot;
                var currentVersion = entry.Version;

                if (snapshot is null || entry.SnapshotVersion != currentVersion)
                {
                    Func<object?>[] newSnapshot = entry.List.Count == 0 ? [] : [.. entry.List];
                    entry.Snapshot = newSnapshot;
                    entry.SnapshotVersion = currentVersion;
                    return newSnapshot;
                }

                return snapshot;
            }
        }

        /// <summary>
        /// Materializes an array of factories, invoking each factory once and excluding null results.
        /// </summary>
        /// <param name="factories">Factories to invoke.</param>
        /// <returns>An array of non-null results.</returns>
        /// <remarks>
        /// Exceptions thrown by factories are not caught and propagate to the caller.
        /// </remarks>
        private static object[] Materialize(Func<object?>[] factories)
        {
            // Two-pass: count then allocate exactly once (invokes factories twice would be incorrect),
            // so we do single-pass into a max-sized buffer and shrink.
            var tmp = new object[factories.Length];
            var idx = 0;

            for (var i = 0; i < factories.Length; i++)
            {
                var value = factories[i].Invoke();
                if (value is not null)
                {
                    tmp[idx++] = value;
                }
            }

            if (idx == 0)
            {
                return [];
            }

            if (idx == tmp.Length)
            {
                return tmp;
            }

            var result = new object[idx];
            Array.Copy(tmp, result, idx);
            return result;
        }

        /// <summary>
        /// Publishes a new snapshot set from <see cref="_nonGenericRegistrationSet"/>.
        /// </summary>
        /// <remarks>
        /// Caller must hold <see cref="_nonGenericGate"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PublishNonGenericSnapshot_NoThrow()
        {
            // HashSet copy is required to publish an immutable snapshot for lock-free readers.
            Volatile.Write(ref _nonGenericRegistrations, [.. _nonGenericRegistrationSet]);
        }
    }
}
