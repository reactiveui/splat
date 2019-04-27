// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace Splat
{
    /// <summary>
    /// This data structure is a representation of a memoizing cache - i.e. a
    /// class that will evaluate a function, but keep a cache of recently
    /// evaluated parameters.
    ///
    /// Since this is a memoizing cache, it is important that this function be a
    /// "pure" function in the mathematical sense - that a key *always* maps to
    /// a corresponding return value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter to the calculation function.</typeparam>
    /// <typeparam name="TVal">The type of the value returned by the calculation
    /// function.</typeparam>
    public sealed class MemoizingMRUCache<TParam, TVal>
    {
        private readonly object _lockObject = new object();
        private readonly Func<TParam, object, TVal> _calculationFunction;
        private readonly Action<TVal> _releaseFunction;
        private readonly int _maxCacheSize;

        private LinkedList<TParam> _cacheMRUList;
        private Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>> _cacheEntries;

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
        public MemoizingMRUCache(Func<TParam, object, TVal> calculationFunc, int maxSize, Action<TVal> onRelease = null)
        {
            Contract.Requires(calculationFunc != null);
            Contract.Requires(maxSize > 0);

            _calculationFunction = calculationFunc;
            _releaseFunction = onRelease;
            _maxCacheSize = maxSize;
            InvalidateAll();
        }

        /// <summary>
        /// Gets the value from the specified key.
        /// </summary>
        /// <param name="key">The value to pass to the calculation function.</param>
        /// <returns>The value that we have got.</returns>
        public TVal Get(TParam key)
        {
            return Get(key, null);
        }

        /// <summary>
        /// Evaluates the function provided, returning the cached value if possible.
        /// </summary>
        /// <param name="key">The value to pass to the calculation function.</param>
        /// <param name="context">An additional optional user-specific parameter.</param>
        /// <returns>The value that we have got.</returns>
        public TVal Get(TParam key, object context = null)
        {
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> found;

            lock (_lockObject)
            {
                if (_cacheEntries.TryGetValue(key, out found))
                {
                    RefreshEntry(found.Item1);

                    return found.Item2;
                }

                var result = _calculationFunction(key, context);

                var node = new LinkedListNode<TParam>(key);
                _cacheMRUList.AddFirst(node);
                _cacheEntries[key] = new Tuple<LinkedListNode<TParam>, TVal>(node, result);
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
        public bool TryGet(TParam key, out TVal result)
        {
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> output;

            lock (_lockObject)
            {
                var ret = _cacheEntries.TryGetValue(key, out output);
                if (ret && output != null)
                {
                    RefreshEntry(output.Item1);
                    result = output.Item2;
                }
                else
                {
                    result = default(TVal);
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
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> to_remove;

            lock (_lockObject)
            {
                if (!_cacheEntries.TryGetValue(key, out to_remove))
                {
                    return;
                }

                var releaseVar = to_remove.Item2;

                _cacheMRUList.Remove(to_remove.Item1);
                _cacheEntries.Remove(key);

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
        public void InvalidateAll(bool aggregateReleaseExceptions = false)
        {
            Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>> oldCacheToClear = null;
            lock (_lockObject)
            {
                if (_releaseFunction == null || _cacheEntries == null)
                {
                    _cacheMRUList = new LinkedList<TParam>();
                    _cacheEntries = new Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>>();
                    return;
                }

                if (_cacheEntries.Count == 0)
                {
                    return;
                }

                // by moving to a temp variable
                // can free up the lock quicker for other calls to MRU
                if (_releaseFunction != null)
                {
                    // no point doing it, if nothing to release
                    oldCacheToClear = _cacheEntries;
                }

                _cacheMRUList = new LinkedList<TParam>();
                _cacheEntries = new Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>>();
            }

            if (oldCacheToClear == null)
            {
                return;
            }

            if (aggregateReleaseExceptions)
            {
                var exceptions = new List<Exception>(oldCacheToClear.Count);
                foreach (var item in oldCacheToClear)
                {
                    try
                    {
                        _releaseFunction?.Invoke(item.Value.Item2);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }

                if (exceptions.Count > 0)
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
                _releaseFunction?.Invoke(item.Value.Item2);
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
                return _cacheEntries.Select(x => x.Value.Item2);
            }
        }

        private void MaintainCache()
        {
            while (_cacheMRUList.Count > _maxCacheSize)
            {
                var to_remove = _cacheMRUList.Last.Value;
                _releaseFunction?.Invoke(_cacheEntries[to_remove].Item2);

                _cacheEntries.Remove(_cacheMRUList.Last.Value);
                _cacheMRUList.RemoveLast();
            }
        }

        private void RefreshEntry(LinkedListNode<TParam> item)
        {
            // only juggle entries if more than 1 of them.
            if (_cacheEntries.Count > 1)
            {
                _cacheMRUList.Remove(item);
                _cacheMRUList.AddFirst(item);
            }
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(_cacheEntries.Count == _cacheMRUList.Count);
            Contract.Invariant(_cacheEntries.Count <= _maxCacheSize);
        }
    }
}
