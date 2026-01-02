// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Generic container for service registrations with contracts (named services).
/// Uses versioned Entry pattern for O(1) registration and lazy snapshot rebuild.
/// Lock-free reads via ConcurrentDictionary, per-entry locks for snapshot rebuilds.
/// </summary>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContractContainer<T>
{
    private static readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> Entries = new(StringComparer.Ordinal);
    private static readonly ConcurrentDictionary<string, int> Counts = [];

    static ContractContainer() => GlobalGenericFirstDependencyResolver.RegisterClearAction(ClearAll);

    /// <summary>
    /// Adds a service instance with a contract.
    /// Snapshot rebuild is deferred until first read - O(1) operation.
    /// </summary>
    /// <param name="service">The service instance to add.</param>
    /// <param name="contract">The contract name.</param>
    public static void Add(T service, string? contract)
    {
        var key = contract ?? string.Empty;
        var entry = Entries.GetOrAdd(key, _ => new());

        lock (entry)
        {
            entry.List.Add(Registration<T>.FromInstance(service));
            entry.Version++; // Under lock, simple increment is fine
            Counts[key] = entry.List.Count;
        }
    }

    /// <summary>
    /// Adds a service factory with a contract.
    /// Snapshot rebuild is deferred until first read - O(1) operation.
    /// </summary>
    /// <param name="factory">The factory function to add.</param>
    /// <param name="contract">The contract name.</param>
    public static void Add(Func<T?> factory, string? contract)
    {
        var key = contract ?? string.Empty;
        var entry = Entries.GetOrAdd(key, _ => new());

        lock (entry)
        {
            entry.List.Add(Registration<T>.FromFactory(factory));
            entry.Version++; // Under lock, simple increment is fine
            Counts[key] = entry.List.Count;
        }
    }

    /// <summary>
    /// Tries to get the most recent service registration for a contract.
    /// Rebuilds snapshot lazily if stale. Lock-free fast path for reads.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    /// <param name="instance">The resolved service instance.</param>
    /// <returns>True if a service was found; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
    {
        // Fast path: check count first to avoid dictionary lookup if empty
        if (Counts.TryGetValue(contract, out var count) && count == 0)
        {
            instance = default;
            return false;
        }

        if (!Entries.TryGetValue(contract, out var entry))
        {
            instance = default;
            return false;
        }

        var registrations = EnsureSnapshot(entry);

        if (registrations.Length == 0)
        {
            instance = default;
            return false;
        }

        var last = registrations[registrations.Length - 1];
        if (last.IsFactory)
        {
            var factory = last.GetFactory()!;
            instance = factory.Invoke()!;
            return instance is not null;
        }

        instance = last.GetInstance()!;
        return instance is not null;
    }

    /// <summary>
    /// Gets all registered services for a contract as a materialized array.
    /// Rebuilds snapshot lazily if stale. Lock-free fast path for reads.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    /// <returns>An array of all registered services for the contract.</returns>
    public static T[] GetAll(string contract)
    {
        if (!Entries.TryGetValue(contract, out var entry))
        {
            return [];
        }

        var registrations = EnsureSnapshot(entry);
        return ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>
    /// Removes the most recent service registration for a contract.
    /// Snapshot rebuild is deferred until first read.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    public static void RemoveCurrent(string contract)
    {
        if (!Entries.TryGetValue(contract, out var entry))
        {
            return;
        }

        lock (entry)
        {
            if (entry.List.Count > 0)
            {
                entry.List.RemoveAt(entry.List.Count - 1);
                entry.Version++; // Under lock, simple increment is fine

                var newCount = entry.List.Count;
                Counts[contract] = newCount;

                // Remove empty entries
                if (newCount == 0)
                {
                    Entries.TryRemove(contract, out _);
                    Counts.TryRemove(contract, out _);
                }
            }
        }
    }

    /// <summary>
    /// Clears all service registrations for a specific contract.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    public static void Clear(string contract)
    {
        Entries.TryRemove(contract, out _);
        Counts.TryRemove(contract, out _);
    }

    /// <summary>
    /// Clears all service registrations for all contracts.
    /// Note: Threads that have already obtained an Entry reference from Entries.TryGetValue
    /// may continue to resolve from the old entry snapshot until they attempt a new lookup.
    /// This is acceptable for DI containers where Clear operations are typically stop-the-world
    /// operations invoked during teardown or test cleanup.
    /// </summary>
    public static void ClearAll()
    {
        Entries.Clear();
        Counts.Clear();
    }

    /// <summary>
    /// Checks if there are any registrations for a specific contract.
    /// Fast path using count check to avoid lock acquisition.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    /// <returns>True if registrations exist for the contract; otherwise false.</returns>
    public static bool HasRegistrations(string contract) => Counts.TryGetValue(contract, out var count) && count > 0;

    /// <summary>
    /// Gets the count of registrations for a specific contract without invoking any factories.
    /// </summary>
    /// <param name="contract">The contract name.</param>
    /// <returns>The number of registrations for the contract.</returns>
    public static int GetCount(string contract) => Counts.TryGetValue(contract, out var count) ? count : 0;

    /// <summary>
    /// Ensures the snapshot is current, rebuilding if stale.
    /// Uses volatile semantics for lock-free fast path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Registration<T>[] EnsureSnapshot(ArrayHelpers.Entry<Registration<T>> entry)
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
                Registration<T>[] newSnapshot = [.. entry.List];

                // Publish snapshot with proper memory ordering
                Volatile.Write(ref entry.Snapshot, newSnapshot);
                Volatile.Write(ref entry.SnapshotVersion, version);

                return newSnapshot;
            }

            return snapshot;
        }
    }
}
