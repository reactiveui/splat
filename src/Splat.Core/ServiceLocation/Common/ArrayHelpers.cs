// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>Helper methods for array operations and snapshot-based registration patterns used by GenericFirst resolvers.</summary>
/// <remarks>
/// <para>
/// This type centralizes performance-sensitive patterns used throughout the resolver implementation:
/// <list type="bullet">
///   <item><description>Small array mutation helpers for legacy array-based storage.</description></item>
///   <item><description>Materialization of <see cref="Registration{T}"/> into concrete instances.</description></item>
///   <item><description>The <see cref="Entry{TValue}"/> snapshot primitive for O(1) writes and amortized O(n) reads.</description></item>
/// </list>
/// </para>
/// </remarks>
internal static class ArrayHelpers
{
    /// <summary>Appends a registration to an existing array, creating a new array if the input is <see langword="null"/>.</summary>
    /// <typeparam name="T">Service type for the registration.</typeparam>
    /// <param name="current">Existing array of registrations, or <see langword="null"/>.</param>
    /// <param name="newItem">Registration to append.</param>
    /// <returns>A new array containing all prior items followed by <paramref name="newItem"/>.</returns>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static Registration<T>[] AppendNullable<T>(Registration<T>[]? current, Registration<T> newItem)
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

    /// <summary>Removes the last element from an array, returning a new array.</summary>
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
    internal static T[] RemoveLast<T>(T[]? current)
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

    /// <summary>Materializes registrations by invoking factories and collecting non-null results.</summary>
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
    internal static T[] MaterializeRegistrations<T>(Registration<T>[] registrations)
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
                    tmp[idx] = value;
                    idx++;
                }

                continue;
            }

            // Instance registration path.
            var instance = reg.GetInstance();
            if (instance is not null)
            {
                tmp[idx] = instance;
                idx++;
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

    /// <summary>A concurrent, snapshot-backed list used as the per-registration storage primitive.</summary>
    /// <remarks>
    /// <para>
    /// Writes (<see cref="Add"/>, <see cref="RemoveCurrent"/>, <see cref="Clear"/>) are O(1) and run under a
    /// private gate; they invalidate the published snapshot, which is rebuilt lazily on the next read.
    /// Reads via <see cref="GetSnapshot"/> are lock-free when the snapshot is current.
    /// </para>
    /// <para>
    /// A volatile count is maintained on every mutation so <see cref="Count"/> and <see cref="HasItems"/>
    /// answer without materializing a snapshot or taking the gate.
    /// </para>
    /// <para>
    /// The gate is private to this type, so all synchronization is encapsulated here and never shared with callers.
    /// On .NET 9+ the <c>Lock</c> alias resolves to <c>System.Threading.Lock</c> (fast-path locking);
    /// on earlier targets it resolves to <see cref="object"/> and the <c>lock</c> statement falls back to <see cref="System.Threading.Monitor"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValue">The value type stored in the entry.</typeparam>
    internal sealed class Entry<TValue>
    {
        /// <summary>Serializes mutations so committed state and version counters advance atomically.</summary>
        private readonly Lock _gate = new();

        /// <summary>Backing storage for committed items; mutated only under <see cref="_gate"/>.</summary>
        private readonly List<TValue> _list = new(4);

        /// <summary>Cached immutable copy of <see cref="_list"/> handed to lock-free readers; rebuilt when stale.</summary>
        private TValue[]? _snapshot;

        /// <summary>Monotonically increasing version stamped on each committed mutation.</summary>
        private int _version;

        /// <summary>The <see cref="_version"/> captured when <see cref="_snapshot"/> was last rebuilt.</summary>
        private int _snapshotVersion;

        /// <summary>Number of committed items; read lock-free via <see cref="Count"/>.</summary>
        private int _count;

        /// <summary>Gets the number of committed items.</summary>
        /// <remarks>Lock-free volatile read; reflects the count after the most recent committed mutation.</remarks>
        internal int Count => Volatile.Read(ref _count);

        /// <summary>Gets a value indicating whether the entry currently holds any items.</summary>
        /// <remarks>Lock-free volatile read; equivalent to <c><see cref="Count"/> &gt; 0</c>.</remarks>
        internal bool HasItems => Volatile.Read(ref _count) > 0;

        /// <summary>Appends an item, invalidating the snapshot.</summary>
        /// <param name="value">The value to append.</param>
        internal void Add(TValue value)
        {
            lock (_gate)
            {
                _list.Add(value);
                _version++;
                Volatile.Write(ref _count, _list.Count);
            }
        }

        /// <summary>Removes the most recently added item, if any.</summary>
        /// <returns><see langword="true"/> if the entry became empty as a result; otherwise <see langword="false"/>.</returns>
        /// <remarks>When the entry drains to empty, an empty snapshot is published so subsequent reads fast-exit.</remarks>
        internal bool RemoveCurrent()
        {
            lock (_gate)
            {
                var listCount = _list.Count;
                if (listCount == 0)
                {
                    return false;
                }

                _list.RemoveAt(listCount - 1);
                _version++;

                var remaining = _list.Count;
                Volatile.Write(ref _count, remaining);

                if (remaining != 0)
                {
                    return false;
                }

                // Publish an empty snapshot aligned with the new version so readers fast-exit.
                _snapshot = [];
                _snapshotVersion = _version;
                return true;
            }
        }

        /// <summary>Removes the first occurrence of <paramref name="value"/>, if present.</summary>
        /// <param name="value">The value to remove, matched by the default equality comparer.</param>
        /// <returns><see langword="true"/> if an item was removed; otherwise <see langword="false"/>.</returns>
        /// <remarks>When the entry drains to empty, an empty snapshot is published so subsequent reads fast-exit.</remarks>
        internal bool Remove(TValue value)
        {
            lock (_gate)
            {
                if (!_list.Remove(value))
                {
                    return false;
                }

                _version++;
                var remaining = _list.Count;
                Volatile.Write(ref _count, remaining);

                if (remaining == 0)
                {
                    // Publish an empty snapshot aligned with the new version so readers fast-exit.
                    _snapshot = [];
                    _snapshotVersion = _version;
                }

                return true;
            }
        }

        /// <summary>Removes all items and publishes an empty snapshot.</summary>
        internal void Clear()
        {
            lock (_gate)
            {
                _list.Clear();
                _version++;
                _snapshot = [];
                _snapshotVersion = _version;
                Volatile.Write(ref _count, 0);
            }
        }

        /// <summary>Returns the current immutable snapshot, rebuilding it if stale.</summary>
        /// <returns>A snapshot array of the current items; never <see langword="null"/>.</returns>
        /// <remarks>
        /// Fast path is lock-free when the published snapshot version matches the list version.
        /// Otherwise the snapshot is rebuilt under the gate and allocates an array copy of the list.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal TValue[] GetSnapshot()
        {
            var version = Volatile.Read(ref _version);
            var snapshot = Volatile.Read(ref _snapshot);

            return snapshot is not null && Volatile.Read(ref _snapshotVersion) == version
                ? snapshot
                : RebuildSnapshot();
        }

        /// <summary>Appends all current items to <paramref name="destination"/> under the gate.</summary>
        /// <param name="destination">The list to append the current items to.</param>
        /// <remarks>Used by disposal enumeration, which needs every item without invoking factories.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="destination"/> is <see langword="null"/>.</exception>
        internal void CopyItemsTo(List<TValue> destination)
        {
            ArgumentExceptionHelper.ThrowIfNull(destination);

            lock (_gate)
            {
                destination.AddRange(_list);
            }
        }

        /// <summary>Rebuilds and publishes the snapshot under the gate after the lock-free fast path missed.</summary>
        /// <returns>The current immutable snapshot; never <see langword="null"/>.</returns>
        /// <remarks>
        /// The in-gate re-validation success (returning the cached snapshot without rebuilding) only happens when another thread
        /// rebuilt the snapshot between this thread's lock-free miss and its acquisition of the gate. That race cannot be
        /// reproduced by a single-threaded test, so the method is excluded from coverage.
        /// </remarks>
        [ExcludeFromCodeCoverage] // Defensive: the in-gate re-validation only succeeds when another thread rebuilt the snapshot between the lock-free miss and acquiring the gate.
        private TValue[] RebuildSnapshot()
        {
            lock (_gate)
            {
                var snapshot = _snapshot;
                var currentVersion = _version;

                if (snapshot is null || _snapshotVersion != currentVersion)
                {
                    TValue[] newSnapshot = _list.Count == 0 ? [] : [.. _list];
                    _snapshot = newSnapshot;
                    _snapshotVersion = currentVersion;
                    return newSnapshot;
                }

                return snapshot;
            }
        }
    }
}
