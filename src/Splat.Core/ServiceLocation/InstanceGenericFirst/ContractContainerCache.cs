// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-type static cache for contract-based instance-scoped containers keyed by <see cref="ResolverState"/>.
/// </summary>
/// <remarks>
/// <para>
/// This cache is used by instance-scoped resolvers to store per-resolver contract registrations without requiring explicit cleanup.
/// Entries are removed automatically when the associated <see cref="ResolverState"/> becomes unreachable.
/// </para>
/// <para>
/// Per-contract registration lists are maintained as mutable lists guarded by an entry gate, with a published snapshot array for lock-free reads.
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContractContainerCache<T>
{
    /// <summary>
    /// The conditional weak table mapping resolver state to the per-resolver <see cref="ContractContainer"/>.
    /// </summary>
    /// <remarks>
    /// Using <see cref="ConditionalWeakTable{TKey,TValue}"/> avoids holding strong references to resolver state and prevents leaks.
    /// </remarks>
    private static readonly ConditionalWeakTable<ResolverState, ContractContainer> Containers = new();

    /// <summary>
    /// Gets or creates the contract container for the specified resolver state.
    /// </summary>
    /// <param name="state">The resolver state used as the cache key.</param>
    /// <returns>The per-resolver <see cref="ContractContainer"/> for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContractContainer Get(ResolverState state)
    {
        ArgumentExceptionHelper.ThrowIfNull(state);
        return Containers.GetOrCreateValue(state);
    }

    /// <summary>
    /// Per-resolver contract container for <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// Each contract maps to an independent registration entry with its own mutation gate, supporting concurrent usage across different contracts.
    /// </remarks>
    internal sealed class ContractContainer
    {
        /// <summary>
        /// Contract-to-entry map holding per-contract registration state.
        /// </summary>
        /// <remarks>
        /// Uses ordinal contract comparison for predictable behavior and best performance.
        /// </remarks>
        private readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> _entries =
            new(StringComparer.Ordinal);

        /// <summary>
        /// Returns whether a contract currently has one or more registrations.
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <returns><see langword="true"/> when the contract exists and has at least one registration; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public bool HasRegistrations(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry))
            {
                return false;
            }

            // Count is derived from the snapshot/versioned list; fast-path via snapshot if present.
            var snapshot = Volatile.Read(ref entry.Snapshot);
            if (snapshot is not null)
            {
                return snapshot.Length != 0;
            }

            // Fall back to list count under lock if no snapshot yet.
            lock (entry.Gate)
            {
                return entry.List.Count != 0;
            }
        }

        /// <summary>
        /// Gets the number of registrations for a contract.
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <returns>The number of registrations for the contract.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public int GetCount(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry))
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
        /// Attempts to resolve the most recent registration for a contract (last registration wins).
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <param name="instance">Receives the resolved instance.</param>
        /// <returns>
        /// <see langword="true"/> if a registration exists and resolves to a non-null value; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method does not hold locks while invoking user factories.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

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

            var last = registrations[registrations.Length - 1];

            if (last.TryGetFactory(out var factory))
            {
                instance = factory.Invoke()!;
                return instance is not null;
            }

            instance = last.GetInstance();
            return instance is not null;
        }

        /// <summary>
        /// Resolves all registrations for a contract.
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <returns>An array of resolved values; empty if no registrations exist.</returns>
        /// <remarks>
        /// Factories are invoked during materialization.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public T[] GetAll(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry))
            {
                return [];
            }

            var registrations = EnsureSnapshot(entry);
            return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
        }

        /// <summary>
        /// Registers a constant instance under the specified contract.
        /// </summary>
        /// <param name="service">The instance to register.</param>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public void Add(T service, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            var entry = _entries.GetOrAdd(contract, static _ => new());

            lock (entry.Gate)
            {
                entry.List.Add(Registration<T>.FromInstance(service));
                entry.Version++;

                // Snapshot is now stale; it will be rebuilt lazily.
            }
        }

        /// <summary>
        /// Registers a factory under the specified contract.
        /// </summary>
        /// <param name="factory">Factory used to produce instances.</param>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> or <paramref name="contract"/> is <see langword="null"/>.</exception>
        public void Add(Func<T?> factory, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(factory);
            ArgumentExceptionHelper.ThrowIfNull(contract);

            var entry = _entries.GetOrAdd(contract, static _ => new());

            lock (entry.Gate)
            {
                entry.List.Add(Registration<T>.FromFactory(factory));
                entry.Version++;
            }
        }

        /// <summary>
        /// Removes the most recent registration for a contract (if any).
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <remarks>
        /// If removal empties the contract, the contract entry is removed from the dictionary.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public void RemoveCurrent(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry))
            {
                return;
            }

            var shouldRemove = false;

            lock (entry.Gate)
            {
                var list = entry.List;
                if (list.Count == 0)
                {
                    return;
                }

                list.RemoveAt(list.Count - 1);
                entry.Version++;

                shouldRemove = list.Count == 0;
            }

            // Remove after releasing the gate to keep lock scope minimal.
            if (shouldRemove)
            {
                _entries.TryRemove(contract, out _);
            }
        }

        /// <summary>
        /// Removes all registrations for a contract.
        /// </summary>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        public void Clear(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);
            _entries.TryRemove(contract, out _);
        }

        /// <summary>
        /// Removes all registrations for all contracts in this container.
        /// </summary>
        public void ClearAll() => _entries.Clear();

        /// <summary>
        /// Ensures the snapshot for an entry is current, rebuilding it if needed.
        /// </summary>
        /// <param name="entry">The per-contract entry to snapshot.</param>
        /// <returns>A snapshot array representing the current registration list.</returns>
        /// <remarks>
        /// Fast path is lock-free: if the published snapshot version matches the current version, the snapshot is returned.
        /// Otherwise, the snapshot is rebuilt under <c>lock(entry.Gate)</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entry"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Registration<T>[] EnsureSnapshot(ArrayHelpers.Entry<Registration<T>> entry)
        {
            ArgumentExceptionHelper.ThrowIfNull(entry);

            var version = Volatile.Read(ref entry.Version);
            var snapshot = Volatile.Read(ref entry.Snapshot);

            if (snapshot is not null && Volatile.Read(ref entry.SnapshotVersion) == version)
            {
                return snapshot;
            }

            lock (entry.Gate)
            {
                snapshot = entry.Snapshot;

                // Read versions non-volatile under the gate.
                var currentVersion = entry.Version;
                if (snapshot is null || entry.SnapshotVersion != currentVersion)
                {
                    // Avoid allocating for empty.
                    Registration<T>[] newSnapshot = entry.List.Count == 0 ? [] : [.. entry.List];

                    entry.Snapshot = newSnapshot;
                    entry.SnapshotVersion = currentVersion;

                    return newSnapshot;
                }

                return snapshot;
            }
        }
    }
}
