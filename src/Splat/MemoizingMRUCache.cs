using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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
    public class MemoizingMRUCache<TParam, TVal>
    {
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

            if (_cacheEntries.TryGetValue(key, out found))
            {
                _cacheMRUList.Remove(found.Item1);
                _cacheMRUList.AddFirst(found.Item1);
                return found.Item2;
            }

            var result = _calculationFunction(key, context);

            var node = new LinkedListNode<TParam>(key);
            _cacheMRUList.AddFirst(node);
            _cacheEntries[key] = new Tuple<LinkedListNode<TParam>, TVal>(node, result);
            MaintainCache();

            return result;
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
            var ret = _cacheEntries.TryGetValue(key, out output);
            if (ret && output != null)
            {
                _cacheMRUList.Remove(output.Item1);
                _cacheMRUList.AddFirst(output.Item1);
                result = output.Item2;
            }
            else
            {
                result = default(TVal);
            }

            return ret;
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

            if (!_cacheEntries.TryGetValue(key, out to_remove))
            {
                return;
            }

            _releaseFunction?.Invoke(to_remove.Item2);

            _cacheMRUList.Remove(to_remove.Item1);
            _cacheEntries.Remove(key);
        }

        /// <summary>
        /// Invalidate all the items in the cache.
        /// </summary>
        public void InvalidateAll()
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

            // We have to remove them one-by-one to call the release function
            // We ToArray() this so we don't get a "modifying collection while
            // enumerating" exception.
            foreach (var v in _cacheEntries.Keys.ToArray())
            {
                Invalidate(v);
            }
        }

        /// <summary>
        /// Returns all values currently in the cache.
        /// </summary>
        /// <returns>The values in the cache.</returns>
        public IEnumerable<TVal> CachedValues()
        {
            return _cacheEntries.Select(x => x.Value.Item2);
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

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(_cacheEntries.Count == _cacheMRUList.Count);
            Contract.Invariant(_cacheEntries.Count <= _maxCacheSize);
        }
    }
}
