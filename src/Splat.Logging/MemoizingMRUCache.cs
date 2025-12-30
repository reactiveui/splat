// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.Contracts;

#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Splat;

/// <summary>
/// <para>
/// This data structure is a representation of a memoizing cache - i.e. a
/// class that will evaluate a function, but keep a cache of recently
/// evaluated parameters.
/// </para>
/// <para>
/// Since this is a memoizing cache, it is important that this function be a
/// "pure" function in the mathematical sense - that a key *always* maps to
/// a corresponding return value.
/// </para>
/// </summary>
/// <typeparam name="TParam">The type of the parameter to the calculation function.</typeparam>
/// <typeparam name="TVal">The type of the value returned by the calculation
/// function.</typeparam>
public sealed class MemoizingMRUCache<TParam, TVal>
    where TParam : notnull
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// The synchronization gate for all mutations of the MRU list and cache dictionary.
    /// </summary>
    private readonly Lock _lockObject = new();
#else
    /// <summary>
    /// The synchronization gate for all mutations of the MRU list and cache dictionary.
    /// </summary>
    private readonly object _lockObject = new();
#endif

    /// <summary>
    /// The function whose results you want to cache,
    /// which is provided the key value, and an Tag object that is
    /// user-defined.
    /// </summary>
    private readonly Func<TParam, object?, TVal> _calculationFunction;

    /// <summary>
    /// A function to call when a result gets
    /// evicted from the cache (i.e. because Invalidate was called or the
    /// cache is full).
    /// </summary>
    private readonly Action<TVal>? _releaseFunction;

    /// <summary>
    /// The size of the cache to maintain, after which old
    /// items will start to be thrown out.
    /// </summary>
    private readonly int _maxCacheSize;

    /// <summary>
    /// A comparer for the parameter.
    /// </summary>
    private readonly IEqualityComparer<TParam> _comparer;

    /// <summary>
    /// The MRU list of cached keys. Front is most recently used; back is least recently used.
    /// </summary>
    private LinkedList<TParam> _cacheMRUList;

    /// <summary>
    /// The cache entries. Each key maps to a tuple of:
    /// - the corresponding node in <see cref="_cacheMRUList"/>
    /// - the cached value.
    /// </summary>
    private Dictionary<TParam, (LinkedListNode<TParam> param, TVal value)> _cacheEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The function whose results you want to cache,
    /// which is provided the key value, and an Tag object that is
    /// user-defined.</param>
    /// <param name="maxSize">The size of the cache to maintain, after which old
    /// items will start to be thrown out.</param>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize)
        : this(calculationFunc, maxSize, null, EqualityComparer<TParam>.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The function whose results you want to cache,
    /// which is provided the key value, and an Tag object that is
    /// user-defined.</param>
    /// <param name="maxSize">The size of the cache to maintain, after which old
    /// items will start to be thrown out.</param>
    /// <param name="onRelease">A function to call when a result gets
    /// evicted from the cache (i.e. because Invalidate was called or the
    /// cache is full).</param>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize, Action<TVal> onRelease)
        : this(calculationFunc, maxSize, onRelease, EqualityComparer<TParam>.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The function whose results you want to cache,
    /// which is provided the key value, and an Tag object that is
    /// user-defined.</param>
    /// <param name="maxSize">The size of the cache to maintain, after which old
    /// items will start to be thrown out.</param>
    /// <param name="paramComparer">A comparer for the parameter.</param>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize, IEqualityComparer<TParam> paramComparer)
        : this(calculationFunc, maxSize, null, paramComparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingMRUCache{TParam, TVal}"/> class.
    /// </summary>
    /// <param name="calculationFunc">The function whose results you want to cache,
    /// which is provided the key value, and an Tag object that is
    /// user-defined.</param>
    /// <param name="maxSize">The size of the cache to maintain, after which old
    /// items will start to be thrown out.</param>
    /// <param name="onRelease">A function to call when a result gets
    /// evicted from the cache (i.e. because Invalidate was called or the
    /// cache is full).</param>
    /// <param name="paramComparer">A comparer for the parameter.</param>
    public MemoizingMRUCache(Func<TParam, object?, TVal> calculationFunc, int maxSize, Action<TVal>? onRelease, IEqualityComparer<TParam> paramComparer)
    {
        ArgumentExceptionHelper.ThrowIfLessThanOrEqual(maxSize, 0);

        ArgumentExceptionHelper.ThrowIfNull(calculationFunc);

        _calculationFunction = calculationFunc;
        _releaseFunction = onRelease;
        _maxCacheSize = maxSize;
        _comparer = paramComparer ?? EqualityComparer<TParam>.Default;
        _cacheMRUList = [];
        _cacheEntries = new(_comparer);
    }

    /// <summary>
    /// Gets the value from the specified key.
    /// </summary>
    /// <param name="key">The value to pass to the calculation function.</param>
    /// <returns>The value that we have got.</returns>
    public TVal Get(TParam key) => Get(key, null);

    /// <summary>
    /// Evaluates the function provided, returning the cached value if possible.
    /// </summary>
    /// <param name="key">The value to pass to the calculation function.</param>
    /// <param name="context">An additional optional user-specific parameter.</param>
    /// <returns>The value that we have got.</returns>
    public TVal Get(TParam key, object? context = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

#if NET9_0_OR_GREATER
        // Lock modernization: opportunistic fast path under contention.
        if (_lockObject.TryEnter())
        {
            try
            {
                if (_cacheEntries.TryGetValue(key, out var found))
                {
                    RefreshEntry(found.param);

                    return found.value;
                }
            }
            finally
            {
                _lockObject.Exit();
            }
        }
#endif

        // Lock minimisation: do not compute under the lock.
        lock (_lockObject)
        {
            if (_cacheEntries.TryGetValue(key, out var found))
            {
                RefreshEntry(found.param);

                return found.value;
            }
        }

        var result = _calculationFunction(key, context);

        lock (_lockObject)
        {
            // Double-check in case another thread populated it while we computed.
            if (_cacheEntries.TryGetValue(key, out var found))
            {
                RefreshEntry(found.param);

                return found.value;
            }

            var node = new LinkedListNode<TParam>(key);
            _cacheMRUList.AddFirst(node);
            _cacheEntries[key] = (node, result);
            MaintainCache();

            return result;
        }
    }

    /// <summary>
    /// Tries to get the value if it's available.
    /// </summary>
    /// <param name="key">The input value of the key to use.</param>
    /// <param name="result">The result if available, otherwise it will be the default value.</param>
    /// <returns>If we were able to retrieve the value or not.</returns>
    public bool TryGet(TParam key, out TVal? result)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

        lock (_lockObject)
        {
            var ret = _cacheEntries.TryGetValue(key, out var output);
            if (ret)
            {
                RefreshEntry(output.param);
                result = output.value;
            }
            else
            {
                result = default;
            }

            return ret;
        }
    }

    /// <summary>
    /// Ensure that the next time this key is queried, the calculation
    /// function will be called.
    /// </summary>
    /// <param name="key">The key to invalidate the value for.</param>
    public void Invalidate(TParam key)
    {
        ArgumentExceptionHelper.ThrowIfNull(key);

        lock (_lockObject)
        {
            if (!_cacheEntries.TryGetValue(key, out var toRemove))
            {
                return;
            }

            var releaseVar = toRemove.value;

            _cacheMRUList.Remove(toRemove.param);
            _ = _cacheEntries.Remove(key);

            // moved down to allow removal from list
            // even if the release call fails.
            _releaseFunction?.Invoke(releaseVar);
        }
    }

    /// <summary>
    /// Invalidate all the items in the cache.
    /// </summary>
    /// <param name="aggregateReleaseExceptions">
    /// Flag to indicate whether Exceptions during the resource Release call should not fail on the first item.
    /// But should try all items then throw an aggregate exception.
    /// </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Aggregates the exceptions")]
    public void InvalidateAll(bool aggregateReleaseExceptions = false)
    {
        Dictionary<TParam, (LinkedListNode<TParam> param, TVal value)>? oldCacheToClear = null;
        lock (_lockObject)
        {
            if (_releaseFunction is null || _cacheEntries is null)
            {
                _cacheMRUList = [];
                _cacheEntries = new(_comparer);
                return;
            }

            if (_cacheEntries.Count == 0)
            {
                return;
            }

            // by moving to a temp variable
            // can free up the lock quicker for other calls to MRU
            // no point doing it, if nothing to release
            oldCacheToClear = _cacheEntries;

            _cacheMRUList = [];
            _cacheEntries = new(_comparer);
        }

        if (aggregateReleaseExceptions)
        {
            // Allocation minimisation: only allocate if an exception actually occurs.
            List<Exception>? exceptions = null;

            foreach (var item in oldCacheToClear)
            {
                try
                {
                    _releaseFunction?.Invoke(item.Value.value);
                }
                catch (Exception e)
                {
                    (exceptions ??= new()).Add(e);
                }
            }

            if (exceptions is not null && exceptions.Count > 0)
            {
                throw new AggregateException("Exceptions throw during MRU Cache Invalidate All Item Release.", exceptions);
            }

            return;
        }

        // release mechanism that will throw on first failure.
        // but they've still been removed from the active cache
        // as the cache field was reassigned.
        foreach (var item in oldCacheToClear)
        {
            _releaseFunction?.Invoke(item.Value.value);
        }
    }

    /// <summary>
    /// Returns all values currently in the cache.
    /// </summary>
    /// <returns>The values in the cache.</returns>
    public IEnumerable<TVal> CachedValues()
    {
        lock (_lockObject)
        {
            // Materialise inside the lock to avoid a deferred iterator escaping the lock.
            return _cacheEntries.Select(x => x.Value.value).ToArray();
        }
    }

    /// <summary>
    /// Ensures the cache does not exceed the configured maximum size, evicting least-recently-used entries as needed.
    /// </summary>
    private void MaintainCache()
    {
        while (_cacheMRUList.Count > _maxCacheSize)
        {
            if (_cacheMRUList.Last is null)
            {
                continue;
            }

            var toRemove = _cacheMRUList.Last.Value;

#if NET8_0_OR_GREATER
            // Modernisation: avoid repeated dictionary lookups in eviction path.
            // We expect the key to exist; if it does not, fall back to the safe path.
            ref var entry = ref CollectionsMarshal.GetValueRefOrNullRef(_cacheEntries, toRemove);
            if (!Unsafe.IsNullRef(ref entry))
            {
                _releaseFunction?.Invoke(entry.value);
            }
#else
            _releaseFunction?.Invoke(_cacheEntries[toRemove].value);
#endif

            _ = _cacheEntries.Remove(_cacheMRUList.Last.Value);
            _cacheMRUList.RemoveLast();
        }
    }

    /// <summary>
    /// Moves the specified node to the front of the MRU list.
    /// </summary>
    /// <param name="item">The MRU node to refresh.</param>
    private void RefreshEntry(LinkedListNode<TParam> item)
    {
        // only juggle entries if more than 1 of them.
        if (_cacheEntries.Count > 1)
        {
            _cacheMRUList.Remove(item);
            _cacheMRUList.AddFirst(item);
        }
    }

    /// <summary>
    /// Ensures cache invariants are maintained.
    /// </summary>
    [ContractInvariantMethod]
    private void Invariants()
    {
        Contract.Invariant(_cacheEntries.Count == _cacheMRUList.Count);
        Contract.Invariant(_cacheEntries.Count <= _maxCacheSize);
    }
}
