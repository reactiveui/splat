// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Helper methods for array operations and snapshot-based registration patterns used by GenericFirst resolvers.
/// </summary>
/// <remarks>
/// <para>
/// This type centralizes performance-sensitive patterns used throughout the resolver implementation:
/// <list type="bullet">
///   <item><description>Small array mutation helpers for legacy array-based storage.</description></item>
///   <item><description>Materialization of <see cref="Registration{T}"/> into concrete instances.</description></item>
///   <item><description>Snapshotting patterns for O(1) writes and amortized O(n) reads using <see cref="Entry{TValue}"/>.</description></item>
/// </list>
/// </para>
/// <para>
/// Threading: the dictionary-based methods assume the caller holds an appropriate lock for the provided dictionary.
/// The <see cref="Entry{TValue}.Gate"/> exists for per-entry synchronization when used in concurrent dictionaries.
/// </para>
/// </remarks>
internal static class ArrayHelpers
{
    /// <summary>
    /// Appends a registration to an existing array, creating a new array if the input is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">Service type for the registration.</typeparam>
    /// <param name="current">Existing array of registrations, or <see langword="null"/>.</param>
    /// <param name="newItem">Registration to append.</param>
    /// <returns>A new array containing all prior items followed by <paramref name="newItem"/>.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Registration<T>[] AppendNullable<T>(Registration<T>[]? current, Registration<T> newItem)
    {
        if (current is null)
        {
            return [newItem];
        }

        var len = current.Length;
        var newArray = new Registration<T>[len + 1];
        Array.Copy(current, newArray, len);
        newArray[len] = newItem;
        return newArray;
    }

    /// <summary>
    /// Removes the last element from an array, returning a new array.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="current">Current array, or <see langword="null"/>.</param>
    /// <returns>
    /// An empty array when <paramref name="current"/> is <see langword="null"/>, empty, or length 1;
    /// otherwise, a new array containing all elements except the last.
    /// </returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] RemoveLast<T>(T[]? current)
    {
        if (current is null || current.Length <= 1)
        {
            return [];
        }

        var newLen = current.Length - 1;
        var newArray = new T[newLen];
        Array.Copy(current, newArray, newLen);
        return newArray;
    }

    /// <summary>
    /// Materializes registrations by invoking factories and collecting non-null results.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="registrations">Registrations to materialize.</param>
    /// <returns>An array of materialized, non-null instances.</returns>
    /// <remarks>
    /// <para>
    /// Factories are invoked exactly once per factory registration.
    /// </para>
    /// <para>
    /// Any exception thrown by a factory is not caught and will propagate to the caller.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="registrations"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] MaterializeRegistrations<T>(Registration<T>[] registrations)
    {
        ArgumentExceptionHelper.ThrowIfNull(registrations);

        // Fast exit.
        if (registrations.Length == 0)
        {
            return [];
        }

        // Single-pass materialization:
        // - Invoke each factory at most once.
        // - Avoid List<T> allocations.
        // - Allocate a temporary array at the maximum possible size, then shrink.
        var tmp = new T[registrations.Length];
        var idx = 0;

        for (var i = 0; i < registrations.Length; i++)
        {
            ref readonly var reg = ref registrations[i];

            // Prefer TryGetFactory for clarity and to avoid null-forgiving in the caller.
            if (reg.TryGetFactory(out var factory))
            {
                var value = factory.Invoke();
                if (value is not null)
                {
                    tmp[idx++] = value;
                }

                continue;
            }

            // Instance registration path.
            var instance = reg.GetInstance();
            if (instance is not null)
            {
                tmp[idx++] = instance;
            }
        }

        if (idx == 0)
        {
            return [];
        }

        // If all registrations produced non-null values, return the buffer as-is.
        if (idx == tmp.Length)
        {
            return tmp;
        }

        // Shrink to exact size.
        var result = new T[idx];
        Array.Copy(tmp, result, idx);
        return result;
    }

    /// <summary>
    /// Adds an item to an entry's list and increments the version.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type stored in the entry list.</typeparam>
    /// <param name="entries">Dictionary of entries.</param>
    /// <param name="key">Key under which to add the value.</param>
    /// <param name="value">Value to add.</param>
    /// <remarks>
    /// This method assumes the caller holds an appropriate lock for <paramref name="entries"/>.
    /// The snapshot is not rebuilt; it is rebuilt lazily on read.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void Add<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        ArgumentExceptionHelper.ThrowIfNull(entries);
        ArgumentExceptionHelper.ThrowIfNull(key);

        if (!entries.TryGetValue(key, out var entry))
        {
            entry = new();
            entries[key] = entry;
        }

        entry.List.Add(value);
        entry.Version++;
    }

    /// <summary>
    /// Gets the immutable snapshot array for a key, rebuilding it if stale.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="entries">Dictionary of entries.</param>
    /// <param name="key">Key to retrieve.</param>
    /// <returns>The snapshot array for the key, or an empty array when the key is not present.</returns>
    /// <remarks>
    /// This method assumes the caller holds an appropriate lock for <paramref name="entries"/>.
    /// Snapshot rebuild cost is O(n) but occurs only once per version.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static TValue[] GetSnapshot<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries,
        TKey key)
        where TKey : notnull
    {
        ArgumentExceptionHelper.ThrowIfNull(entries);
        ArgumentExceptionHelper.ThrowIfNull(key);

        if (!entries.TryGetValue(key, out var entry))
        {
            return [];
        }

        if (entry.Snapshot is null || entry.SnapshotVersion != entry.Version)
        {
            entry.Snapshot = entry.List.Count == 0 ? [] : [.. entry.List];
            entry.SnapshotVersion = entry.Version;
        }

        return entry.Snapshot;
    }

    /// <summary>
    /// Removes the last item from an entry list for a key. If the list becomes empty, removes the key entry.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="entries">Dictionary of entries.</param>
    /// <param name="key">Key to remove the current registration from.</param>
    /// <remarks>
    /// This method assumes the caller holds an appropriate lock for <paramref name="entries"/>.
    /// Snapshot is not rebuilt; it will be rebuilt lazily on next read.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void RemoveCurrent<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries,
        TKey key)
        where TKey : notnull
    {
        ArgumentExceptionHelper.ThrowIfNull(entries);
        ArgumentExceptionHelper.ThrowIfNull(key);

        if (!entries.TryGetValue(key, out var entry))
        {
            return;
        }

        var list = entry.List;
        var count = list.Count;
        if (count == 0)
        {
            return;
        }

        list.RemoveAt(count - 1);
        entry.Version++;

        if (list.Count == 0)
        {
            entries.Remove(key);
        }
    }

    /// <summary>
    /// Removes all registrations for a key by removing the entry entirely.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="entries">Dictionary of entries.</param>
    /// <param name="key">Key to remove.</param>
    /// <remarks>
    /// This method assumes the caller holds an appropriate lock for <paramref name="entries"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void RemoveAll<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries,
        TKey key)
        where TKey : notnull
    {
        ArgumentExceptionHelper.ThrowIfNull(entries);
        ArgumentExceptionHelper.ThrowIfNull(key);

        entries.Remove(key);
    }

    /// <summary>
    /// Clears all registrations from the entries dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="entries">Dictionary to clear.</param>
    /// <remarks>
    /// This method assumes the caller holds an appropriate lock for <paramref name="entries"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void Clear<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries)
        where TKey : notnull
    {
        ArgumentExceptionHelper.ThrowIfNull(entries);
        entries.Clear();
    }

    /// <summary>
    /// Entry that tracks a mutable list with a lazily-updated immutable snapshot.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes are O(1): callers append/remove items and increment <see cref="Version"/> under the appropriate synchronization.
    /// Reads can be O(n) when rebuilding the snapshot, but only once per version.
    /// </para>
    /// <para>
    /// When used in concurrent containers, <see cref="Gate"/> should be used with <c>lock(Gate)</c>.
    /// On .NET 9+ the runtime will use fast-locking when <see cref="Gate"/> is a Lock class.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValue">The value type stored in the entry.</typeparam>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "Needed for by-ref and volatile patterns; encapsulation would add overhead.")]
    internal sealed class Entry<TValue>
    {
        /// <summary>
        /// Per-entry gate used for synchronizing mutations and snapshot rebuilds.
        /// </summary>
#if NET9_0_OR_GREATER
        public readonly Lock Gate = new();
#else
        public readonly object Gate = new();
#endif

        /// <summary>
        /// Mutable list of values. Mutate only under the associated lock.
        /// </summary>
        public readonly List<TValue> List = new(4);

        /// <summary>
        /// Immutable snapshot of <see cref="List"/>. May be <see langword="null"/> until first built.
        /// </summary>
        public TValue[]? Snapshot;

        /// <summary>
        /// Current version, incremented on every mutation.
        /// </summary>
        public int Version;

        /// <summary>
        /// Version corresponding to the current <see cref="Snapshot"/>.
        /// </summary>
        public int SnapshotVersion;
    }
}
