// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Generic container for service registrations without contracts.
/// Uses versioned Entry pattern for O(1) registration and lazy snapshot rebuild.
/// </summary>
/// <typeparam name="T">The service type.</typeparam>
internal static class Container<T>
{
#if NET9_0_OR_GREATER
    private static readonly Lock LockInstance = new();
#else
    private static readonly object LockInstance = new();
#endif
    private static readonly ArrayHelpers.Entry<Registration<T>> Entries = new();
    private static int _count;

    static Container()
    {
        GlobalGenericFirstDependencyResolver.RegisterClearAction(Clear);
    }

    /// <summary>
    /// Gets a value indicating whether this container has any registrations.
    /// Fast path using count check to avoid snapshot materialization.
    /// </summary>
    public static bool HasRegistrations => Volatile.Read(ref _count) > 0;

    /// <summary>
    /// Gets the count of registrations without invoking any factories.
    /// </summary>
    /// <returns>The number of registrations.</returns>
    public static int GetCount() => Volatile.Read(ref _count);

    /// <summary>
    /// Adds a service instance to the container.
    /// Snapshot rebuild is deferred until first read - O(1) operation.
    /// </summary>
    /// <param name="service">The service instance to add.</param>
    public static void Add(T service)
    {
        lock (LockInstance)
        {
            Entries.List.Add(Registration<T>.FromInstance(service));
            Entries.Version++; // Under lock, simple increment is fine
            Volatile.Write(ref _count, Entries.List.Count);
        }
    }

    /// <summary>
    /// Adds a service factory to the container.
    /// Snapshot rebuild is deferred until first read - O(1) operation.
    /// </summary>
    /// <param name="factory">The factory function to add.</param>
    public static void Add(Func<T?> factory)
    {
        lock (LockInstance)
        {
            Entries.List.Add(Registration<T>.FromFactory(factory));
            Entries.Version++; // Under lock, simple increment is fine
            Volatile.Write(ref _count, Entries.List.Count);
        }
    }

    /// <summary>
    /// Tries to get the most recent service registration.
    /// Rebuilds snapshot lazily if stale.
    /// </summary>
    /// <param name="instance">The resolved service instance.</param>
    /// <returns>True if a service was found; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet([MaybeNullWhen(false)] out T instance)
    {
        // Fast path: avoid snapshot materialization if empty
        if (Volatile.Read(ref _count) == 0)
        {
            instance = default;
            return false;
        }

        var registrations = EnsureSnapshot();

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
    /// Gets all registered services as a materialized array.
    /// Rebuilds snapshot lazily if stale.
    /// </summary>
    /// <returns>An array of all registered services.</returns>
    public static T[] GetAll()
    {
        var registrations = EnsureSnapshot();
        return ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>
    /// Removes the most recent service registration.
    /// Snapshot rebuild is deferred until first read.
    /// </summary>
    public static void RemoveCurrent()
    {
        lock (LockInstance)
        {
            if (Entries.List.Count > 0)
            {
                Entries.List.RemoveAt(Entries.List.Count - 1);
                Entries.Version++; // Under lock, simple increment is fine
                Volatile.Write(ref _count, Entries.List.Count);
            }
        }
    }

    /// <summary>
    /// Clears all service registrations from this container.
    /// Publishes an empty snapshot with proper volatile semantics to ensure visibility.
    /// Concurrent readers will see either the complete old snapshot or the new empty snapshot.
    /// </summary>
    public static void Clear()
    {
        lock (LockInstance)
        {
            Entries.List.Clear();

            // Publish empty snapshot with proper ordering: Version first, then Snapshot, then SnapshotVersion, then Count
            var newVersion = Entries.Version + 1;
            Volatile.Write(ref Entries.Version, newVersion);
            Volatile.Write(ref Entries.Snapshot, []);
            Volatile.Write(ref Entries.SnapshotVersion, newVersion);
            Volatile.Write(ref _count, 0);
        }
    }

    /// <summary>
    /// Ensures the snapshot is current, rebuilding if stale.
    /// Uses volatile semantics for lock-free fast path.
    /// Returns the current snapshot array.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Registration<T>[] EnsureSnapshot()
    {
        // Lock-free fast path: check if snapshot is current
        var snapshot = Volatile.Read(ref Entries.Snapshot);
        var version = Volatile.Read(ref Entries.Version);
        var snapshotVersion = Volatile.Read(ref Entries.SnapshotVersion);

        if (snapshot != null && snapshotVersion == version)
        {
            return snapshot;
        }

        // Snapshot is stale, rebuild under lock
        lock (LockInstance)
        {
            // Double-check after acquiring lock
            snapshot = Volatile.Read(ref Entries.Snapshot);
            version = Volatile.Read(ref Entries.Version);
            snapshotVersion = Volatile.Read(ref Entries.SnapshotVersion);

            if (snapshot == null || snapshotVersion != version)
            {
                Registration<T>[] newSnapshot = [..Entries.List];

                // Publish snapshot with proper memory ordering
                Volatile.Write(ref Entries.Snapshot, newSnapshot);
                Volatile.Write(ref Entries.SnapshotVersion, version);

                return newSnapshot;
            }

            return snapshot;
        }
    }
}
