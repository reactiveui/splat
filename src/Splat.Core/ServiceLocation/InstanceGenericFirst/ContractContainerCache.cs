// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-type static cache for contract-based instance-scoped containers using ConditionalWeakTable.
/// Maps resolver state to per-resolver contract containers for type T.
/// </summary>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContractContainerCache<T>
{
    private static readonly ConditionalWeakTable<ResolverState, ContractContainer> Containers = new();

    /// <summary>
    /// Gets or creates the contract container for the specified resolver state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContractContainer Get(ResolverState state) => Containers.GetOrCreateValue(state);

    /// <summary>
    /// Per-resolver contract container for type T.
    /// </summary>
    internal sealed class ContractContainer
    {
        private readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> _entries = new();
        private readonly ConcurrentDictionary<string, int> _counts = new();

        public bool HasRegistrations(string contract) => _counts.TryGetValue(contract, out var count) && count > 0;

        public int GetCount(string contract) => _counts.TryGetValue(contract, out var count) ? count : 0;

        public bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
        {
            // Fast path: check count first to avoid dictionary lookup if empty
            if (_counts.TryGetValue(contract, out var count) && count == 0)
            {
                instance = default;
                return false;
            }

            if (!_entries.TryGetValue(contract, out var entry))
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

        public T[] GetAll(string contract)
        {
            if (!_entries.TryGetValue(contract, out var entry))
            {
                return [];
            }

            var registrations = EnsureSnapshot(entry);
            return ArrayHelpers.MaterializeRegistrations(registrations);
        }

        public void Add(T service, string contract)
        {
            var entry = _entries.GetOrAdd(contract, _ => new());

            lock (entry)
            {
                entry.List.Add(Registration<T>.FromInstance(service));
                entry.Version++;
                _counts[contract] = entry.List.Count;
            }
        }

        public void Add(Func<T?> factory, string contract)
        {
            var entry = _entries.GetOrAdd(contract, _ => new());

            lock (entry)
            {
                entry.List.Add(Registration<T>.FromFactory(factory));
                entry.Version++;
                _counts[contract] = entry.List.Count;
            }
        }

        public void RemoveCurrent(string contract)
        {
            if (!_entries.TryGetValue(contract, out var entry))
            {
                return;
            }

            lock (entry)
            {
                if (entry.List.Count > 0)
                {
                    entry.List.RemoveAt(entry.List.Count - 1);
                    entry.Version++;

                    if (entry.List.Count == 0)
                    {
                        _entries.TryRemove(contract, out _);
                        _counts.TryRemove(contract, out _);
                    }
                    else
                    {
                        _counts[contract] = entry.List.Count;
                    }
                }
            }
        }

        public void Clear(string contract)
        {
            _entries.TryRemove(contract, out _);
            _counts.TryRemove(contract, out _);
        }

        public void ClearAll()
        {
            _entries.Clear();
            _counts.Clear();
        }

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
}
