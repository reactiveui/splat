// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-type static cache for instance-scoped containers using ConditionalWeakTable.
/// Maps resolver state to per-resolver Container&lt;T&gt; instances.
/// </summary>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContainerCache<T>
{
    private static readonly ConditionalWeakTable<ResolverState, Container> Containers = new();

    /// <summary>
    /// Gets or creates the container for the specified resolver state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Container Get(ResolverState state) => Containers.GetOrCreateValue(state);

    /// <summary>
    /// Per-resolver container for type T.
    /// </summary>
    internal sealed class Container
    {
        private readonly ArrayHelpers.Entry<Registration<T>> _entries = new();
        private int _count;

        public bool HasRegistrations => Volatile.Read(ref _count) > 0;

        public int GetCount() => Volatile.Read(ref _count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet([MaybeNullWhen(false)] out T instance)
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

            // Get the most recent registration
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

        public T[] GetAll()
        {
            var registrations = EnsureSnapshot();
            return ArrayHelpers.MaterializeRegistrations(registrations);
        }

        public void Add(T service)
        {
            lock (_entries)
            {
                _entries.List.Add(Registration<T>.FromInstance(service));
                _entries.Version++;
                Volatile.Write(ref _count, _entries.List.Count);
            }
        }

        public void Add(Func<T?> factory)
        {
            lock (_entries)
            {
                _entries.List.Add(Registration<T>.FromFactory(factory));
                _entries.Version++;
                Volatile.Write(ref _count, _entries.List.Count);
            }
        }

        public void RemoveCurrent()
        {
            lock (_entries)
            {
                if (_entries.List.Count > 0)
                {
                    _entries.List.RemoveAt(_entries.List.Count - 1);
                    _entries.Version++;
                    Volatile.Write(ref _count, _entries.List.Count);
                }
            }
        }

        public void Clear()
        {
            lock (_entries)
            {
                _entries.List.Clear();
                _entries.Version++;
                Volatile.Write(ref _count, 0);

                // Publish empty snapshot to avoid rebuild cost on next read
                Volatile.Write(ref _entries.Snapshot, []);
                Volatile.Write(ref _entries.SnapshotVersion, _entries.Version);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Registration<T>[] EnsureSnapshot()
        {
            // Lock-free fast path: check if snapshot is current
            var snapshot = Volatile.Read(ref _entries.Snapshot);
            var version = Volatile.Read(ref _entries.Version);
            var snapshotVersion = Volatile.Read(ref _entries.SnapshotVersion);

            if (snapshot != null && snapshotVersion == version)
            {
                return snapshot;
            }

            // Snapshot is stale, rebuild under lock
            lock (_entries)
            {
                // Double-check after acquiring lock
                snapshot = Volatile.Read(ref _entries.Snapshot);
                version = Volatile.Read(ref _entries.Version);
                snapshotVersion = Volatile.Read(ref _entries.SnapshotVersion);

                if (snapshot == null || snapshotVersion != version)
                {
                    Registration<T>[] newSnapshot = [.. _entries.List];

                    // Publish snapshot with proper memory ordering
                    Volatile.Write(ref _entries.Snapshot, newSnapshot);
                    Volatile.Write(ref _entries.SnapshotVersion, version);

                    return newSnapshot;
                }

                return snapshot;
            }
        }
    }
}
