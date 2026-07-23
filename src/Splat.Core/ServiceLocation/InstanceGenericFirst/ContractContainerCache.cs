// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>Per-type static cache for contract-based instance-scoped containers keyed by <see cref="ResolverState"/>.</summary>
/// <remarks>
/// <para>
/// This cache is used by instance-scoped resolvers to store per-resolver contract registrations without requiring explicit cleanup.
/// Entries are removed automatically when the associated <see cref="ResolverState"/> becomes unreachable.
/// </para>
/// <para>
/// Each contract maps to an <see cref="ArrayHelpers.Entry{TValue}"/> that guards its mutable list and publishes a snapshot array for lock-free reads.
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContractContainerCache<T>
{
    /// <summary>The conditional weak table mapping resolver state to the per-resolver <see cref="ContractContainer"/>.</summary>
    /// <remarks>
    /// Using <see cref="ConditionalWeakTable{TKey,TValue}"/> avoids holding strong references to resolver state and prevents leaks.
    /// </remarks>
    private static readonly ConditionalWeakTable<ResolverState, ContractContainer> Containers = new();

    /// <summary>Gets or creates the contract container for the specified resolver state.</summary>
    /// <param name="state">The resolver state used as the cache key.</param>
    /// <returns>The per-resolver <see cref="ContractContainer"/> for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ContractContainer Get(ResolverState state)
    {
        ArgumentExceptionHelper.ThrowIfNull(state);
        return Containers.GetOrCreateValue(state);
    }

    /// <summary>Per-resolver contract container for <typeparamref name="T"/>.</summary>
    /// <remarks>
    /// Each contract maps to an independent registration entry with its own mutation gate, supporting concurrent usage across different contracts.
    /// </remarks>
    internal sealed class ContractContainer
    {
        /// <summary>Contract-to-entry map holding per-contract registration state.</summary>
        /// <remarks>
        /// Uses ordinal contract comparison for predictable behavior and best performance.
        /// </remarks>
        private readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> _entries =
            new(StringComparer.Ordinal);

        /// <summary>Returns whether a contract currently has one or more registrations.</summary>
        /// <param name="contract">The contract key.</param>
        /// <returns><see langword="true"/> when the contract exists and has at least one registration; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal bool HasRegistrations(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            return _entries.TryGetValue(contract, out var entry) && entry.HasItems;
        }

        /// <summary>Gets the number of registrations for a contract.</summary>
        /// <param name="contract">The contract key.</param>
        /// <returns>The number of registrations for the contract.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal int GetCount(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            return _entries.TryGetValue(contract, out var entry) ? entry.Count : 0;
        }

        /// <summary>Attempts to resolve the most recent registration for a contract (last registration wins).</summary>
        /// <param name="contract">The contract key.</param>
        /// <param name="instance">Receives the resolved instance.</param>
        /// <returns>
        /// <see langword="true"/> if a registration exists and resolves to a non-null value; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method does not hold locks while invoking user factories.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry) || !entry.HasItems)
            {
                instance = default;
                return false;
            }

            return ResolveLast(entry.GetSnapshot(), out instance);
        }

        /// <summary>Resolves all registrations for a contract.</summary>
        /// <param name="contract">The contract key.</param>
        /// <returns>An array of resolved values; empty if no registrations exist.</returns>
        /// <remarks>
        /// Factories are invoked during materialization.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal T[] GetAll(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry))
            {
                return [];
            }

            var registrations = entry.GetSnapshot();
            return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
        }

        /// <summary>Registers a constant instance under the specified contract.</summary>
        /// <param name="service">The instance to register.</param>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal void Add(T service, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            var entry = _entries.GetOrAdd(contract, static _ => new());
            entry.Add(Registration<T>.FromInstance(service));
        }

        /// <summary>Registers a factory under the specified contract.</summary>
        /// <param name="factory">Factory used to produce instances.</param>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> or <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal void Add(Func<T?> factory, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(factory);
            ArgumentExceptionHelper.ThrowIfNull(contract);

            var entry = _entries.GetOrAdd(contract, static _ => new());
            entry.Add(Registration<T>.FromFactory(factory));
        }

        /// <summary>Removes the most recent registration for a contract (if any).</summary>
        /// <param name="contract">The contract key.</param>
        /// <remarks>
        /// If removal empties the contract, the contract entry is removed from the dictionary.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal void RemoveCurrent(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);

            if (!_entries.TryGetValue(contract, out var entry) || !entry.RemoveCurrent())
            {
                return;
            }

            _ = _entries.TryRemove(contract, out _);
        }

        /// <summary>Removes all registrations for a contract.</summary>
        /// <param name="contract">The contract key.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is <see langword="null"/>.</exception>
        internal void Clear(string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(contract);
            _ = _entries.TryRemove(contract, out _);
        }

        /// <summary>Removes all registrations for all contracts in this container.</summary>
        internal void ClearAll() => _entries.Clear();

        /// <summary>Resolves the most recent registration from <paramref name="registrations"/> (last registration wins).</summary>
        /// <param name="registrations">The immutable snapshot to resolve from.</param>
        /// <param name="instance">Receives the resolved instance when available.</param>
        /// <returns><see langword="true"/> when a non-null instance is produced; otherwise <see langword="false"/>.</returns>
        /// <remarks>
        /// The empty-snapshot arm is a copy-on-write race guard: the entry reported items but a concurrent
        /// <see cref="Clear"/>/<see cref="RemoveCurrent"/> drained the snapshot before it was read. That window cannot be
        /// reproduced by a single-threaded test, so the method is excluded from coverage.
        /// </remarks>
        [ExcludeFromCodeCoverage] // Defensive: the empty-snapshot arm only fires when a concurrent Clear/Remove drains the entry between the HasItems check and the snapshot read.
        private static bool ResolveLast(Registration<T>[] registrations, [MaybeNullWhen(false)] out T instance)
        {
            if (registrations.Length == 0)
            {
                instance = default;
                return false;
            }

            var last = registrations[^1];
            if (last.TryGetFactory(out var factory))
            {
                instance = factory.Invoke()!;
                return instance is not null;
            }

            instance = last.GetInstance();
            return instance is not null;
        }
    }
}
