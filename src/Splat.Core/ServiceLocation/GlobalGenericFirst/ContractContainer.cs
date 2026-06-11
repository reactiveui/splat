// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>Global generic container for service registrations indexed by contract (named services).</summary>
/// <remarks>
/// <para>
/// Registrations are stored per contract key and exposed through a lazily rebuilt snapshot array per contract.
/// Writes are O(1) (append/remove under a per-contract gate). Reads are lock-free when the snapshot is current.
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>The contract map is a <see cref="ConcurrentDictionary{TKey,TValue}"/>.</description></item>
///   <item><description>Each contract has an independent <see cref="ArrayHelpers.Entry{TValue}"/> that encapsulates its own gate.</description></item>
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
    /// <summary>Contract-to-entry map storing per-contract registrations.</summary>
    private static readonly ConcurrentDictionary<string, ArrayHelpers.Entry<Registration<T>>> Entries =
        new(StringComparer.Ordinal);

    /// <summary>Initializes static members of the <see cref="ContractContainer{T}"/> class. Registers the clear action with the global resolver.</summary>
    static ContractContainer() => GlobalGenericFirstDependencyResolver.RegisterClearAction(ClearAll);

    /// <summary>Adds a constant instance registration for a contract.</summary>
    /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
    /// <param name="contract">The contract name. When <see langword="null"/>, the default contract is used.</param>
    public static void Add(T service, string? contract)
    {
        var entry = Entries.GetOrAdd(NormalizeContract(contract), static _ => new());
        entry.Add(Registration<T>.FromInstance(service));
    }

    /// <summary>Adds a factory registration for a contract.</summary>
    /// <param name="factory">Factory delegate used to produce instances.</param>
    /// <param name="contract">The contract name. When <see langword="null"/>, the default contract is used.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static void Add(Func<T?> factory, string? contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var entry = Entries.GetOrAdd(NormalizeContract(contract), static _ => new());
        entry.Add(Registration<T>.FromFactory(factory));
    }

    /// <summary>Attempts to resolve the most recent registration for a contract (last registration wins).</summary>
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
        if (!Entries.TryGetValue(NormalizeContract(contract), out var entry) || !entry.HasItems)
        {
            instance = default;
            return false;
        }

        var registrations = entry.GetSnapshot();
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

    /// <summary>Resolves all registrations for a contract.</summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns>
    /// An array of resolved instances (excluding nulls). Returns an empty array when no registrations exist.
    /// </returns>
    public static T[] GetAll(string contract)
    {
        if (!Entries.TryGetValue(NormalizeContract(contract), out var entry))
        {
            return [];
        }

        var registrations = entry.GetSnapshot();
        return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>Removes the most recent registration for a contract, if any.</summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    public static void RemoveCurrent(string contract)
    {
        var key = NormalizeContract(contract);

        if (!Entries.TryGetValue(key, out var entry) || !entry.RemoveCurrent())
        {
            return;
        }

        Entries.TryRemove(key, out _);
    }

    /// <summary>Removes all registrations for a specific contract.</summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    public static void Clear(string contract) => Entries.TryRemove(NormalizeContract(contract), out _);

    /// <summary>Clears all registrations for all contracts.</summary>
    public static void ClearAll() => Entries.Clear();

    /// <summary>Returns whether a contract has any registrations.</summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns><see langword="true"/> when the contract has at least one registration; otherwise <see langword="false"/>.</returns>
    public static bool HasRegistrations(string contract) =>
        Entries.TryGetValue(NormalizeContract(contract), out var entry) && entry.HasItems;

    /// <summary>Gets the number of registrations for a contract without invoking any factories.</summary>
    /// <param name="contract">
    /// The contract name. For compatibility, <see langword="null"/> is treated as the default contract.
    /// </param>
    /// <returns>The number of registrations for the contract.</returns>
    public static int GetCount(string contract) =>
        Entries.TryGetValue(NormalizeContract(contract), out var entry) ? entry.Count : 0;

    /// <summary>Normalizes a contract key for internal storage and lookup.</summary>
    /// <param name="contract">Contract string, possibly <see langword="null"/>.</param>
    /// <returns>The normalized contract key. Never <see langword="null"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string NormalizeContract(string? contract) => contract ?? string.Empty;
}
