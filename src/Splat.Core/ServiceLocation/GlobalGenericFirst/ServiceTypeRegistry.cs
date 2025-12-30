// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Non-generic registry for service types accessed via reflection or Type parameters.
/// Tracks which types have been registered via non-generic APIs for fallback lookups.
/// Uses versioned Entry pattern for O(1) registration and lazy snapshot rebuild.
/// Lock-free reads via ConcurrentDictionary, per-entry locks for snapshot rebuilds.
/// </summary>
internal static class ServiceTypeRegistry
{
#if NET9_0_OR_GREATER
    private static readonly Lock NonGenericLock = new();
#else
    private static readonly object NonGenericLock = new();
#endif
    private static readonly ConcurrentDictionary<(Type, string?), ArrayHelpers.Entry<Func<object?>>> Entries = [];
    private static readonly HashSet<(Type, string?)> NonGenericRegistrationSet = [];

    private static HashSet<(Type, string?)> NonGenericRegistrations = [];
    private static bool _hasAnyNonGenericRegistrations;

    /// <summary>
    /// Tracks that a type was registered via non-generic API.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    public static void TrackNonGenericRegistration(Type serviceType, string? contract = null)
    {
        lock (NonGenericLock)
        {
            NonGenericRegistrationSet.Add((serviceType, contract));
            RefreshNonGenericSnapshot();
            Volatile.Write(ref _hasAnyNonGenericRegistrations, true);
        }
    }

    /// <summary>
    /// Checks if a type has non-generic registrations.
    /// Fast path using global flag to avoid HashSet lookup when empty.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    /// <returns>True if the type was registered via non-generic API; otherwise false.</returns>
    public static bool HasNonGenericRegistrations(Type serviceType, string? contract = null)
    {
        // Fast path: avoid HashSet lookup if there are no non-generic registrations at all
        if (!Volatile.Read(ref _hasAnyNonGenericRegistrations))
        {
            return false;
        }

        var snapshot = Volatile.Read(ref NonGenericRegistrations);
        return snapshot.Contains((serviceType, contract));
    }

    /// <summary>
    /// Registers a factory for a service type.
    /// Snapshot rebuild is deferred until first read - O(1) operation.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="factory">The factory function.</param>
    /// <param name="contract">Optional contract name.</param>
    public static void Register(Type serviceType, Func<object?> factory, string? contract = null)
    {
        var key = (serviceType, contract);
        var entry = Entries.GetOrAdd(key, _ => new ArrayHelpers.Entry<Func<object?>>());

        lock (entry)
        {
            entry.List.Add(factory);
            entry.Version++; // Under lock, simple increment is fine
        }
    }

    /// <summary>
    /// Gets the most recent service registration.
    /// Rebuilds snapshot lazily if stale. Lock-free fast path for reads.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    /// <returns>The service instance or null.</returns>
    public static object? GetService(Type serviceType, string? contract = null)
    {
        if (!Entries.TryGetValue((serviceType, contract), out var entry))
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

    /// <summary>
    /// Gets all registered services as a materialized array.
    /// Rebuilds snapshot lazily if stale. Lock-free fast path for reads.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    /// <returns>An array of all registered services.</returns>
    public static object[] GetServices(Type serviceType, string? contract = null)
    {
        if (!Entries.TryGetValue((serviceType, contract), out var entry))
        {
            return [];
        }

        var factories = EnsureSnapshot(entry);
        return Materialize(factories);
    }

    /// <summary>
    /// Checks if there are any registrations for a service type.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    /// <returns>True if registrations exist; otherwise false.</returns>
    public static bool HasRegistration(Type serviceType, string? contract = null)
    {
        if (!Entries.TryGetValue((serviceType, contract), out var entry))
        {
            return false;
        }

        // Lock to avoid data race on List.Count
        lock (entry)
        {
            return entry.List.Count > 0;
        }
    }

    /// <summary>
    /// Gets the count of registrations for a service type without invoking any factories.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    /// <returns>The number of registrations.</returns>
    public static int GetCount(Type serviceType, string? contract = null)
    {
        if (!Entries.TryGetValue((serviceType, contract), out var entry))
        {
            return 0;
        }

        // Lock to avoid data race on List.Count
        lock (entry)
        {
            return entry.List.Count;
        }
    }

    /// <summary>
    /// Removes the most recent service registration.
    /// Snapshot rebuild is deferred until first read.
    /// Also removes from NonGenericRegistrationSet if entry becomes empty.
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    public static void UnregisterCurrent(Type serviceType, string? contract = null)
    {
        var key = (serviceType, contract);
        if (!Entries.TryGetValue(key, out var entry))
        {
            return;
        }

        bool shouldRemoveTracking = false;

        lock (entry)
        {
            if (entry.List.Count > 0)
            {
                entry.List.RemoveAt(entry.List.Count - 1);
                entry.Version++; // Under lock, simple increment is fine

                // Remove empty entries
                if (entry.List.Count == 0)
                {
                    Entries.TryRemove(key, out _);
                    shouldRemoveTracking = true;
                }
            }
        }

        // Remove from tracking set to avoid permanent "tax" on this type
        if (shouldRemoveTracking)
        {
            lock (NonGenericLock)
            {
                NonGenericRegistrationSet.Remove(key);
                RefreshNonGenericSnapshot();
            }
        }
    }

    /// <summary>
    /// Removes all service registrations for a type.
    /// Also removes from NonGenericRegistrationSet to avoid permanent "tax".
    /// </summary>
    /// <param name="serviceType">The service type.</param>
    /// <param name="contract">Optional contract name.</param>
    public static void UnregisterAll(Type serviceType, string? contract = null)
    {
        var key = (serviceType, contract);
        bool removed = Entries.TryRemove(key, out _);

        // Remove from tracking set to avoid permanent "tax" on this type
        if (removed)
        {
            lock (NonGenericLock)
            {
                NonGenericRegistrationSet.Remove(key);
                RefreshNonGenericSnapshot();
            }
        }
    }

    /// <summary>
    /// Clears all service registrations from the registry.
    /// Note: Threads that have already obtained an Entry reference from Entries.TryGetValue
    /// may continue to resolve from the old entry snapshot until they attempt a new lookup.
    /// The NonGenericRegistrations snapshot is published with volatile semantics for visibility.
    /// This is acceptable for DI containers where Clear operations are typically stop-the-world
    /// operations invoked during teardown or test cleanup.
    /// </summary>
    public static void Clear()
    {
        Entries.Clear();

        lock (NonGenericLock)
        {
            NonGenericRegistrationSet.Clear();
            Volatile.Write(ref NonGenericRegistrations, []);
            Volatile.Write(ref _hasAnyNonGenericRegistrations, false);
        }
    }

    /// <summary>
    /// Gets all registered factories for disposal purposes.
    /// Returns a snapshot of all factories at the time of call.
    /// </summary>
    /// <returns>An enumerable of all registered factories.</returns>
    public static IEnumerable<Func<object?>> GetAllFactoriesForDisposal()
    {
        var allFactories = new List<Func<object?>>();

        // Take a snapshot of entries to avoid issues if dictionary is modified during disposal
        var entriesSnapshot = Entries.ToArray();

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

    /// <summary>
    /// Ensures the snapshot is current, rebuilding if stale.
    /// Uses volatile semantics for lock-free fast path.
    /// </summary>
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

    private static void RefreshNonGenericSnapshot() => Volatile.Write(ref NonGenericRegistrations, [.. NonGenericRegistrationSet]);
}
