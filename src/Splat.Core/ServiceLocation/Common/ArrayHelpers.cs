// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Helper methods for array operations and concurrent registry management in the GenericFirst resolver.
/// Provides optimized implementations for array manipulation and snapshot-based registration patterns.
/// </summary>
internal static class ArrayHelpers
{
    /// <summary>
    /// Appends a new registration to an existing array, creating a new array if null.
    /// This is used for legacy array-based registration patterns.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="current">The current array of registrations, or null.</param>
    /// <param name="newItem">The new registration to append.</param>
    /// <returns>A new array containing all previous items plus the new item.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static Registration<T>[] AppendNullable<T>(Registration<T>[]? current, Registration<T> newItem)
    {
        if (current == null)
        {
            return [newItem];
        }

        // Create new array with space for one more item
        var newArray = new Registration<T>[current.Length + 1];
        Array.Copy(current, newArray, current.Length);
        newArray[newArray.Length - 1] = newItem;
        return newArray;
    }

    /// <summary>
    /// Removes the last item from an array, returning a new array.
    /// Returns an empty array if the input is null, empty, or has only one item.
    /// This is used for legacy array-based unregistration patterns.
    /// </summary>
    /// <typeparam name="T">The array element type.</typeparam>
    /// <param name="current">The current array, or null.</param>
    /// <returns>A new array with the last item removed, or an empty array.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] RemoveLast<T>(T[]? current)
    {
        if (current is null || current.Length == 0)
        {
            return current ?? [];
        }

        if (current.Length == 1)
        {
            return [];
        }

        // Copy all but the last item
        var newArray = new T[current.Length - 1];
        Array.Copy(current, newArray, current.Length - 1);
        return newArray;
    }

    /// <summary>
    /// Materializes all registrations by invoking factories and collecting non-null instances.
    /// This converts an array of registrations (which can be instances or factories) into
    /// an array of actual service instances, filtering out any null results.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="registrations">The array of registrations to materialize.</param>
    /// <returns>An array of non-null service instances.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static T[] MaterializeRegistrations<T>(Registration<T>[] registrations)
    {
        var results = new List<T>(registrations.Length);
        foreach (var reg in registrations)
        {
            if (reg.IsFactory)
            {
                // Invoke factory and collect result if non-null
                var factory = reg.GetFactory()!;
                var value = factory.Invoke();
                if (value is not null)
                {
                    results.Add(value);
                }
            }
            else
            {
                // Get instance directly if non-null
                var instance = reg.GetInstance();
                if (instance is not null)
                {
                    results.Add(instance);
                }
            }
        }

        return [.. results];
    }

    /// <summary>
    /// Adds an item to an entry's list and increments the version.
    /// The snapshot is NOT rebuilt immediately - it will be lazily rebuilt on first read.
    /// This makes registration O(1) instead of O(n).
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type being registered.</typeparam>
    /// <param name="entries">The dictionary of versioned entries.</param>
    /// <param name="key">The key to register under.</param>
    /// <param name="value">The value to add.</param>
    /// <remarks>
    /// This method must be called under a lock to ensure thread-safety.
    /// </remarks>
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
        // Get or create the entry for this key
        if (!entries.TryGetValue(key, out var entry))
        {
            entry = new();
            entries[key] = entry;
        }

        // Add to list and increment version - O(1)
        entry.List.Add(value);
        entry.Version++;

        // Snapshot is NOT rebuilt here - deferred until read
    }

    /// <summary>
    /// Gets the snapshot array for a key, lazily rebuilding if stale.
    /// This is where the O(n) cost is paid - only when reading, and only once per version.
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type being retrieved.</typeparam>
    /// <param name="entries">The dictionary of versioned entries.</param>
    /// <param name="key">The key to get the snapshot for.</param>
    /// <returns>The immutable snapshot array, or empty array if key not found.</returns>
    /// <remarks>
    /// This method must be called under a lock to ensure thread-safety during snapshot rebuild.
    /// </remarks>
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
        if (!entries.TryGetValue(key, out var entry))
        {
            return [];
        }

        // Check if snapshot needs rebuild
        if (entry.Snapshot == null || entry.SnapshotVersion != entry.Version)
        {
            // Rebuild snapshot from current list - O(n) but only once per version
            entry.Snapshot = [.. entry.List];
            entry.SnapshotVersion = entry.Version;
        }

        return entry.Snapshot;
    }

    /// <summary>
    /// Removes the last item from an entry's list and increments the version.
    /// If the list becomes empty, the entry is removed entirely.
    /// The snapshot will be rebuilt lazily on next read.
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type being unregistered.</typeparam>
    /// <param name="entries">The dictionary of versioned entries.</param>
    /// <param name="key">The key to unregister from.</param>
    /// <remarks>
    /// This method must be called under a lock to ensure thread-safety.
    /// </remarks>
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
        if (entries.TryGetValue(key, out var entry) && entry.List.Count > 0)
        {
            // Remove last item and increment version
            entry.List.RemoveAt(entry.List.Count - 1);
            entry.Version++;

            // Remove empty entries
            if (entry.List.Count == 0)
            {
                entries.Remove(key);
            }

            // Snapshot will be rebuilt lazily on next read
        }
    }

    /// <summary>
    /// Removes all registrations for a key by removing the entry entirely.
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type being unregistered.</typeparam>
    /// <param name="entries">The dictionary of versioned entries.</param>
    /// <param name="key">The key to remove all registrations for.</param>
    /// <remarks>
    /// This method must be called under a lock to ensure thread-safety.
    /// </remarks>
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
        entries.Remove(key);
    }

    /// <summary>
    /// Clears all registrations from the entries dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="entries">The dictionary of versioned entries to clear.</param>
    /// <remarks>
    /// This method must be called under a lock to ensure thread-safety.
    /// </remarks>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void Clear<TKey, TValue>(
        Dictionary<TKey, Entry<TValue>> entries)
        where TKey : notnull
    {
        entries.Clear();
    }

    /// <summary>
    /// Entry that tracks a mutable list with a lazily-updated immutable snapshot.
    /// Uses versioning to defer snapshot rebuilds until read - O(1) writes, amortized O(n) reads.
    /// This is a mutable class for performance - one allocation per key, not per mutation.
    /// Fields (not properties) to support Volatile.Read/Write with ref.
    /// </summary>
    /// <typeparam name="TValue">The type of values stored in the entry.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needed for volatile read/writes")]
    internal sealed class Entry<TValue>
    {
        /// <summary>
        /// The mutable list for O(1) append operations.
        /// </summary>
        public readonly List<TValue> List = new(4);

        /// <summary>
        /// The immutable snapshot array for lock-free reads.
        /// Null if snapshot has never been built.
        /// </summary>
        public TValue[]? Snapshot;

        /// <summary>
        /// The current version, incremented on every mutation.
        /// </summary>
        public int Version;

        /// <summary>
        /// The version when the snapshot was last built.
        /// </summary>
        public int SnapshotVersion;
    }
}
