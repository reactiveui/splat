// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Global generic container for service registrations indexed by contract (named services).
/// </summary>
/// <remarks>
/// <para>
/// Registrations are stored per contract key and exposed through a lazily rebuilt snapshot array per contract.
/// Writes are O(1) (append/remove under a per-contract gate). Reads are lock-free when the snapshot is current.
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>The contract map is a <see cref="ConcurrentDictionary{TKey,TValue}"/>.</description></item>
///   <item><description>Each contract has an independent gate (<c>lock(entry.Gate)</c>) for list mutation and snapshot rebuild.</description></item>
///   <item><description>User factories are invoked without holding internal locks.</description></item>
/// </list>
/// </para>
/// <para>
/// Contract normalization:
/// <list type="bullet">
///   <item><description><see langword="null"/> is treated as the default contract (<see cref="string.Empty"/>).</description></item>
/// </list>
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContractContainer<T>
{
    /// <summary>
    /// Contract-to-entry map storing per-contract registrations.
    /// </summary>
    private static readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> Entries =
        new(StringComparer.Ordinal);

    static ContractContainer() => GlobalGenericFirstDependencyResolver.RegisterClearAction(ClearAll);

    /// <summary>
    /// Adds a constant instance registration for a contract.
    /// </summary>
    /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
    /// <param name="contract">The contract name. When <see langword="null"/>, the default contract is used.</param>
    public static void Add(T service, string? contract)
    {
        var key = NormalizeContract(contract);
        var entry = Entries.GetOrAdd(key, static _ => new());

        lock (entry.Gate)
        {
            entry.List.Add(Registration<T>.FromInstance(service));
            entry.Version++;
        }
    }

    /// <summary>
    /// Adds a factory registration for a contract.
    /// </summary>
    /// <param name="factory">Factory delegate used to produce instances.</param>
    /// <param name="contract">The contract name. When <see langword="null"/>, the default contract is used.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static void Add(Func<T?> factory, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var key = NormalizeContract(contract);
        var entry = Entries.GetOrAdd(key, static _ => new());

        lock (entry.Gate)
        {
            entry.List.Add(Registration<T>.FromFactory(factory));
            entry.Version++;
        }
    }

    /// <summary>
    /// Attempts to resolve the most recent registration for a contract (last registration wins).
    /// </summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <param name="instance">Receives the resolved instance.</param>
    /// <returns>
    /// <see langword="true"/> if a registration exists and resolves to a non-null instance; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method does not hold internal locks while invoking user factories.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
    {
        // Signature is non-null for API compatibility, but we still normalize defensively.
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry))
        {
            instance = default;
            return false;
        }

        // Fast exit when we already have a published empty snapshot.
        var snapshot = Volatile.Read(ref entry.Snapshot);
        if (snapshot is not null && snapshot.Length == 0)
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
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns>
    /// An array of resolved instances (excluding nulls). Returns an empty array when no registrations exist.
    /// </returns>
    public static T[] GetAll(string contract)
    {
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry))
        {
            return [];
        }

        var registrations = EnsureSnapshot(entry);
        return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>
    /// Removes the most recent registration for a contract, if any.
    /// </summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    public static void RemoveCurrent(string contract)
    {
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry))
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

            if (becameEmpty)
            {
                // Publish an empty snapshot aligned with the new version so reads become lock-free fast-exit.
                entry.Snapshot = [];
                entry.SnapshotVersion = entry.Version;
            }
        }

        if (becameEmpty)
        {
            Entries.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Clears all registrations for a specific contract.
    /// </summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    public static void Clear(string contract)
    {
        var key = NormalizeContract(contract);
        Entries.TryRemove(key, out _);
    }

    /// <summary>
    /// Clears all registrations for all contracts.
    /// </summary>
    public static void ClearAll() => Entries.Clear();

    /// <summary>
    /// Returns whether a contract has any registrations.
    /// </summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns><see langword="true"/> when the contract has at least one registration; otherwise <see langword="false"/>.</returns>
    public static bool HasRegistrations(string contract)
    {
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry))
        {
            return false;
        }

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
    /// Gets the number of registrations for a contract without invoking any factories.
    /// </summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns>The number of registrations for the contract.</returns>
    public static int GetCount(string contract)
    {
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry))
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
    /// Ensures the snapshot for an entry is current, rebuilding it if stale.
    /// </summary>
    /// <param name="entry">Entry containing the mutable list and snapshot/version metadata.</param>
    /// <returns>A snapshot of the current registration list.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Registration<T>[] EnsureSnapshot(ArrayHelpers.Entry<Registration<T>> entry)
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
                Registration<T>[] newSnapshot = entry.List.Count == 0 ? [] : [.. entry.List];
                entry.Snapshot = newSnapshot;
                entry.SnapshotVersion = currentVersion;
                return newSnapshot;
            }

            return snapshot;
        }
    }

    /// <summary>
    /// Normalizes a contract key for internal storage and lookup.
    /// </summary>
    /// <param name="contract">Contract string, possibly <see langword="null"/>.</param>
    /// <returns>The normalized contract key. Never <see langword="null"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string NormalizeContract(string? contract) => contract ?? string.Empty;
}
