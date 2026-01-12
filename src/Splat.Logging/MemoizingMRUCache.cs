// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Splat;

/// <summary>
/// Represents a memoizing Most-Recently-Used (MRU) cache.
/// </summary>
/// <remarks>
/// <para>
/// This cache memoizes a calculation function: given the same key, it returns the cached value without recomputing.
/// Because the cache assumes memoization, the calculation function should be a "pure" function in the mathematical sense:
/// the same key must always map to the same value for the cache to be correct.
/// </para>
/// <para>
/// Thread-safety: all structural mutations of the dictionary and MRU list are synchronized via a single gate.
/// The cache intentionally does not attempt to be lock-free because dictionary and MRU list updates must be coordinated.
/// </para>
/// </remarks>
/// <typeparam name="TParam">The key type.</typeparam>
/// <typeparam name="TVal">The cached value type.</typeparam>
public sealed class MemoizingMRUCache<TParam, TVal>
    where TParam : notnull
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// Synchronization gate for all mutations of the MRU list and cache dictionary.
    /// </summary>
    private readonly Lock _gate = new();
#else
    /// <summary>
    /// Synchronization gate for all mutations of the MRU list and cache dictionary.
    /// </summary>
    private readonly object _gate = new();
#endif

    /// <summary>
    /// The calculation function used to produce values for missing keys.
    /// </summary>
    private readonly Func<TParam, object?, TVal> _calculationFunction;

    /// <summary>
    /// Optional callback invoked when an entry is evicted or explicitly invalidated.
    /// </summary>
    private readonly Action<TVal>? _releaseFunction;

    /// <summary>
    /// Maximum number of entries retained in the cache.
    /// </summary>
    private readonly int _maxCacheSize;

    /// <summary>
    /// Equality comparer for keys.
    /// </summary>
    private readonly IEqualityComparer<TParam> _comparer;

    /// <summary>
    /// MRU list of keys. Head is most-recently-used; tail is least-recently-used.
    /// </summary>
    private LinkedList<TParam> _mruList;

    /// <summary>
    /// Dictionary from key to its MRU node and cached value.
    /// </summary>
    private Dictionary<TParam, (LinkedListNode<TParam> node, TVal value)> _entries;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The calculation function used to produce values for missing keys.</param>
    /// <param name="maxSize">The maximum number of entries retained in the cache. Must be &gt; 0.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculationFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxSize"/> is less than or equal to zero.</exception>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize)
        : this(calculationFunc, maxSize, onRelease: null, EqualityComparer<TParam>.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The calculation function used to produce values for missing keys.</param>
    /// <param name="maxSize">The maximum number of entries retained in the cache. Must be &gt; 0.</param>
    /// <param name="onRelease">Callback invoked when an entry is evicted or invalidated.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculationFunc"/> or <paramref name="onRelease"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxSize"/> is less than or equal to zero.</exception>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize, Action<TVal> onRelease)
        : this(calculationFunc, maxSize, onRelease, EqualityComparer<TParam>.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The calculation function used to produce values for missing keys.</param>
    /// <param name="maxSize">The maximum number of entries retained in the cache. Must be &gt; 0.</param>
    /// <param name="paramComparer">Equality comparer for keys.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculationFunc"/> or <paramref name="paramComparer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxSize"/> is less than or equal to zero.</exception>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize, IEqualityComparer<TParam> paramComparer)
        : this(calculationFunc, maxSize, onRelease: null, paramComparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The calculation function used to produce values for missing keys.</param>
    /// <param name="maxSize">The maximum number of entries retained in the cache. Must be &gt; 0.</param>
    /// <param name="onRelease">Optional callback invoked when an entry is evicted or invalidated.</param>
    /// <param name="paramComparer">Equality comparer for keys.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculationFunc"/> or <paramref name="paramComparer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxSize"/> is less than or equal to zero.</exception>
    public MemoizingMRUCache(
        Func<TParam, object?, TVal> calculationFunc,
        int maxSize,
        Action<TVal>? onRelease,
        IEqualityComparer<TParam> paramComparer)
    {
        ArgumentExceptionHelper.ThrowIfLessThanOrEqual(maxSize, 0);
        ArgumentExceptionHelper.ThrowIfNull(calculationFunc);
        ArgumentExceptionHelper.ThrowIfNull(paramComparer);

        _calculationFunction = calculationFunc;
        _releaseFunction = onRelease;
        _maxCacheSize = maxSize;
        _comparer = paramComparer;

        _mruList = [];
        _entries = new(_comparer);
    }

    /// <summary>
    /// Gets the cached value for <paramref name="key"/>, computing and caching it if necessary.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <returns>The cached or computed value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    public TVal Get(TParam key) => Get(key, context: null);

    /// <summary>
    /// Gets the cached value for <paramref name="key"/>, computing and caching it if necessary.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="context">Optional context passed to the calculation function.</param>
    /// <returns>The cached or computed value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    public TVal Get(TParam key, object? context = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

        // First, try to hit in-cache under the gate.
        lock (_gate)
        {
            if (_entries.TryGetValue(key, out var found))
            {
                Refresh_NoThrow(found.node);
                return found.value;
            }
        }

        // Miss: compute outside lock.
        var computed = _calculationFunction(key, context);

        // Insert under the gate (double-check in case another thread filled it).
        TVal? evictedToRelease = default;
        List<TVal>? evictedBatch = null;

        lock (_gate)
        {
            if (_entries.TryGetValue(key, out var found))
            {
                Refresh_NoThrow(found.node);
                return found.value;
            }

            var node = new LinkedListNode<TParam>(key);
            _mruList.AddFirst(node);
            _entries[key] = (node, computed);

            // Maintain size under lock, but do not invoke release under lock.
            if (_entries.Count > _maxCacheSize)
            {
                EvictToSize_NoThrow(ref evictedToRelease, ref evictedBatch);
            }
        }

        // Invoke release callbacks outside the lock.
        if (_releaseFunction is not null)
        {
            if (evictedBatch is not null)
            {
                for (var i = 0; i < evictedBatch.Count; i++)
                {
                    _releaseFunction(evictedBatch[i]);
                }
            }
            else if (!EqualityComparer<TVal>.Default.Equals(evictedToRelease!, default!))
            {
                _releaseFunction(evictedToRelease!);
            }
        }

        return computed;
    }

    /// <summary>
    /// Attempts to get the cached value without computing it.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="result">Receives the cached value when available; otherwise the default value.</param>
    /// <returns><see langword="true"/> if the key was cached; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    public bool TryGet(TParam key, [MaybeNullWhen(false)] out TVal result)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

        lock (_gate)
        {
            if (_entries.TryGetValue(key, out var found))
            {
                Refresh_NoThrow(found.node);
                result = found.value;
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Ensures that the next time this key is queried, the calculation function will be called.
    /// </summary>
    /// <param name="key">The key to invalidate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    public void Invalidate(TParam key)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

        TVal? toRelease = default;
        var hasRelease = false;

        lock (_gate)
        {
            if (!_entries.TryGetValue(key, out var found))
            {
                return;
            }

            _mruList.Remove(found.node);
            _ = _entries.Remove(key);

            if (_releaseFunction is not null)
            {
                toRelease = found.value;
                hasRelease = true;
            }
        }

        // Release outside lock.
        if (hasRelease)
        {
            _releaseFunction!.Invoke(toRelease!);
        }
    }

    /// <summary>
    /// Invalidates all items in the cache.
    /// </summary>
    /// <param name="aggregateReleaseExceptions">
    /// When <see langword="true"/>, release exceptions are collected and rethrown as an <see cref="AggregateException"/>
    /// after all entries have been processed.
    /// </param>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Aggregates exceptions when requested.")]
    public void InvalidateAll(bool aggregateReleaseExceptions = false)
    {
        Dictionary<TParam, (LinkedListNode<TParam> node, TVal value)>? oldEntries = null;

        lock (_gate)
        {
            if (_entries.Count == 0)
            {
                return;
            }

            // Swap caches quickly under lock.
            oldEntries = _entries;
            _mruList = [];
            _entries = new(_comparer);
        }

        if (_releaseFunction is null || oldEntries is null)
        {
            return;
        }

        if (!aggregateReleaseExceptions)
        {
            foreach (var item in oldEntries)
            {
                _releaseFunction(item.Value.value);
            }

            return;
        }

        List<Exception>? exceptions = null;

        foreach (var item in oldEntries)
        {
            try
            {
                _releaseFunction(item.Value.value);
            }
            catch (Exception e)
            {
                (exceptions ??= new()).Add(e);
            }
        }

        if (exceptions is not null && exceptions.Count != 0)
        {
            throw new AggregateException("Exceptions thrown during MRU Cache InvalidateAll item release.", exceptions);
        }
    }

    /// <summary>
    /// Returns a snapshot of all values currently in the cache.
    /// </summary>
    /// <returns>An immutable snapshot of cached values.</returns>
    public IEnumerable<TVal> CachedValues()
    {
        lock (_gate)
        {
            if (_entries.Count == 0)
            {
                return [];
            }

            var result = new TVal[_entries.Count];
            var i = 0;

            foreach (var entry in _entries)
            {
                result[i++] = entry.Value.value;
            }

            return result;
        }
    }

    /// <summary>
    /// Records an evicted value into either the single slot or a batch list.
    /// </summary>
    /// <param name="value">The value that was evicted.</param>
    /// <param name="singleEvicted">Single-value slot.</param>
    /// <param name="batchEvicted">Batch list for multiple evictions.</param>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static void RecordEvictedValueNoThrow(TVal value, ref TVal? singleEvicted, ref List<TVal>? batchEvicted)
    {
        if (batchEvicted is not null)
        {
            batchEvicted.Add(value);
            return;
        }

        // First eviction goes to the single slot.
        if (EqualityComparer<TVal>.Default.Equals(singleEvicted!, default!))
        {
            singleEvicted = value;
            return;
        }

        // Second eviction upgrades to a list.
        batchEvicted = new(capacity: 4) { singleEvicted!, value };
        singleEvicted = default;
    }

    /// <summary>
    /// Moves a node to the front of the MRU list.
    /// </summary>
    /// <param name="node">The node to refresh.</param>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private void Refresh_NoThrow(LinkedListNode<TParam> node)
    {
        // Avoid list juggling when there is only one entry.
        if (_entries.Count <= 1)
        {
            return;
        }

        _mruList.Remove(node);
        _mruList.AddFirst(node);
    }

    /// <summary>
    /// Evicts least-recently-used entries until the cache size is within <see cref="_maxCacheSize"/>.
    /// </summary>
    /// <param name="singleEvicted">Receives a single evicted value when exactly one eviction occurs.</param>
    /// <param name="batchEvicted">Receives evicted values when multiple evictions occur.</param>
    /// <remarks>
    /// Caller must hold <c>lock(_gate)</c>. This method does not invoke the release callback.
    /// </remarks>
#if NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private void EvictToSize_NoThrow(ref TVal? singleEvicted, ref List<TVal>? batchEvicted)
    {
        while (_entries.Count > _maxCacheSize)
        {
            var lastNode = _mruList.Last;
            if (lastNode is null)
            {
                return;
            }

            var key = lastNode.Value;

#if NET8_0_OR_GREATER
            ref var entry = ref CollectionsMarshal.GetValueRefOrNullRef(_entries, key);
            if (!Unsafe.IsNullRef(ref entry))
            {
                RecordEvictedValueNoThrow(entry.value, ref singleEvicted, ref batchEvicted);
            }
#else
            RecordEvictedValueNoThrow(_entries[key].value, ref singleEvicted, ref batchEvicted);
#endif

            _ = _entries.Remove(key);
            _mruList.RemoveLast();
        }
    }

    /// <summary>
    /// Ensures cache invariants are maintained.
    /// </summary>
    [ContractInvariantMethod]
    private void Invariants()
    {
        Contract.Invariant(_entries.Count == _mruList.Count);
        Contract.Invariant(_entries.Count <= _maxCacheSize);
    }
}
