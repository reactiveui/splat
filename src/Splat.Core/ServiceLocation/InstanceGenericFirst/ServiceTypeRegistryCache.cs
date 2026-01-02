// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-resolver registry for non-generic service types accessed via Type parameters.
/// Uses ConditionalWeakTable to map resolver state to per-resolver registries.
/// </summary>
internal static class ServiceTypeRegistryCache
{
    private static readonly ConditionalWeakTable<ResolverState, Registry> Registries = new();

    /// <summary>
    /// Gets or creates the registry for the specified resolver state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Registry Get(ResolverState state) => Registries.GetOrCreateValue(state);

    /// <summary>
    /// Per-resolver non-generic type registry.
    /// </summary>
    internal sealed class Registry
    {
#if NET9_0_OR_GREATER
        private readonly Lock _nonGenericLock = new();
#else
        private readonly object _nonGenericLock = new();
#endif
        private readonly ConcurrentDictionary<(Type, string?), ArrayHelpers.Entry<Func<object?>>> _entries = new();
        private readonly HashSet<(Type, string?)> _nonGenericRegistrationSet = [];

        private HashSet<(Type, string?)> _nonGenericRegistrations = [];
        private bool _hasAnyNonGenericRegistrations;

        public bool HasNonGenericRegistrations(Type serviceType, string? contract = null)
        {
            // Fast path: avoid HashSet lookup if there are no non-generic registrations at all
            if (!Volatile.Read(ref _hasAnyNonGenericRegistrations))
            {
                return false;
            }

            var snapshot = Volatile.Read(ref _nonGenericRegistrations);
            return snapshot.Contains((serviceType, contract));
        }

        public void TrackNonGenericRegistration(Type serviceType, string? contract = null)
        {
            lock (_nonGenericLock)
            {
                _nonGenericRegistrationSet.Add((serviceType, contract));
                RefreshNonGenericSnapshot();
                Volatile.Write(ref _hasAnyNonGenericRegistrations, true);
            }
        }

        public void Register(Type serviceType, Func<object?> factory, string? contract = null)
        {
            var key = (serviceType, contract);
            var entry = _entries.GetOrAdd(key, _ => new());

            lock (entry)
            {
                entry.List.Add(factory);
                entry.Version++;
            }
        }

        public object? GetService(Type serviceType, string? contract = null)
        {
            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return null;
            }

            var factories = EnsureSnapshot(entry);

            if (factories.Length == 0)
            {
                return null;
            }

            return factories[factories.Length - 1]();
        }

        public object[] GetServices(Type serviceType, string? contract = null)
        {
            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return [];
            }

            var factories = EnsureSnapshot(entry);
            return Materialize(factories);
        }

        public bool HasRegistration(Type serviceType, string? contract = null)
        {
            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return false;
            }

            lock (entry)
            {
                return entry.List.Count > 0;
            }
        }

        public int GetCount(Type serviceType, string? contract = null)
        {
            if (!_entries.TryGetValue((serviceType, contract), out var entry))
            {
                return 0;
            }

            lock (entry)
            {
                return entry.List.Count;
            }
        }

        public void UnregisterCurrent(Type serviceType, string? contract = null)
        {
            var key = (serviceType, contract);
            if (!_entries.TryGetValue(key, out var entry))
            {
                return;
            }

            bool shouldRemoveTracking = false;

            lock (entry)
            {
                if (entry.List.Count > 0)
                {
                    entry.List.RemoveAt(entry.List.Count - 1);
                    entry.Version++;

                    if (entry.List.Count == 0)
                    {
                        _entries.TryRemove(key, out _);
                        shouldRemoveTracking = true;
                    }
                }
            }

            if (shouldRemoveTracking)
            {
                lock (_nonGenericLock)
                {
                    _nonGenericRegistrationSet.Remove(key);
                    RefreshNonGenericSnapshot();
                }
            }
        }

        public void UnregisterAll(Type serviceType, string? contract = null)
        {
            var key = (serviceType, contract);
            bool removed = _entries.TryRemove(key, out _);

            if (removed)
            {
                lock (_nonGenericLock)
                {
                    _nonGenericRegistrationSet.Remove(key);
                    RefreshNonGenericSnapshot();
                }
            }
        }

        public void Clear()
        {
            _entries.Clear();

            lock (_nonGenericLock)
            {
                _nonGenericRegistrationSet.Clear();
                Volatile.Write(ref _nonGenericRegistrations, []);
                Volatile.Write(ref _hasAnyNonGenericRegistrations, false);
            }
        }

        public IEnumerable<Func<object?>> GetAllFactoriesForDisposal()
        {
            // Take a snapshot of entries to avoid issues if dictionary is modified during disposal
            KeyValuePair<(Type, string?), ArrayHelpers.Entry<Func<object?>>>[] entriesSnapshot = [.. _entries];

            var allFactories = new List<Func<object?>>(entriesSnapshot.Length * 2);

            foreach (var kvp in entriesSnapshot)
            {
                var entry = kvp.Value;
                lock (entry)
                {
                    allFactories.AddRange(entry.List);
                }
            }

            return allFactories;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object?>[] EnsureSnapshot(ArrayHelpers.Entry<Func<object?>> entry)
        {
            // Lock-free fast path: check if snapshot is current
            var snapshot = Volatile.Read(ref entry.Snapshot);
            var version = Volatile.Read(ref entry.Version);
            var snapshotVersion = Volatile.Read(ref entry.SnapshotVersion);

            if (snapshot != null && snapshotVersion == version)
            {
                return snapshot;
            }

            // Snapshot is stale, rebuild under lock
            lock (entry)
            {
                // Double-check after acquiring lock
                snapshot = Volatile.Read(ref entry.Snapshot);
                version = Volatile.Read(ref entry.Version);
                snapshotVersion = Volatile.Read(ref entry.SnapshotVersion);

                if (snapshot == null || snapshotVersion != version)
                {
                    Func<object?>[] newSnapshot = [.. entry.List];

                    // Publish snapshot with proper memory ordering
                    Volatile.Write(ref entry.Snapshot, newSnapshot);
                    Volatile.Write(ref entry.SnapshotVersion, version);

                    return newSnapshot;
                }

                return snapshot;
            }
        }

        private static object[] Materialize(Func<object?>[] factories)
        {
            var results = new List<object>(factories.Length);
            foreach (var factory in factories)
            {
                var value = factory();
                if (value is not null)
                {
                    results.Add(value);
                }
            }

            return [.. results];
        }

        private void RefreshNonGenericSnapshot() => Volatile.Write(ref _nonGenericRegistrations, [.. _nonGenericRegistrationSet]);
    }
}
