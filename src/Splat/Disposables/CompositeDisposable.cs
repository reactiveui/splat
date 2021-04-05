// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Splat
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// This is a simplified version based on System.Reactive's one.
    /// </summary>
    internal sealed class CompositeDisposable : IDisposable
    {
        // Default initial capacity of the _disposables list in case
        // The number of items is not known upfront
        private const int DefaultCapacity = 16;

        private bool _disposed;
        private List<IDisposable>? _disposables;
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with no disposables contained by it initially.
        /// </summary>
        public CompositeDisposable()
        {
            _disposables = new List<IDisposable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with the specified number of disposables.
        /// </summary>
        /// <param name="capacity">The number of disposables that the new CompositeDisposable can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public CompositeDisposable(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _disposables = new List<IDisposable>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Any of the disposables in the <paramref name="disposables"/> collection is <c>null</c>.</exception>
        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables is null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            _disposables = new List<IDisposable>(disposables.Length);
            Init(disposables);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Any of the disposables in the <paramref name="disposables"/> collection is <c>null</c>.</exception>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables is null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            // If the disposables is a collection, get its size
            // and use it as a capacity hint for the copy.
            if (disposables is ICollection<IDisposable> c)
            {
                _disposables = new List<IDisposable>(c.Count);
                Init(disposables);
            }
            else
            {
                _disposables = new List<IDisposable>(DefaultCapacity);

                // Unknown sized disposables, use the default capacity hint
                Init(disposables);
            }
        }

        /// <summary>
        /// Disposes all disposables in the group and removes them from the group.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            var disposables = Interlocked.Exchange(ref _disposables, null);

            if (disposables is not null)
            {
                foreach (var disposed in disposables)
                {
                    disposed.Dispose();
                }
            }

            Volatile.Write(ref _count, 0);
            Volatile.Write(ref _disposed, true);
        }

        /// <summary>
        /// Initialize the inner disposable list and count fields.
        /// </summary>
        /// <param name="disposables">The enumerable sequence of disposables.</param>
        private void Init(IEnumerable<IDisposable> disposables)
        {
            if (_disposables is null)
            {
                return;
            }

            // do the copy and null-check in one step to avoid a
            // second loop for just checking for null items
            foreach (var d in disposables)
            {
                if (d is null)
                {
                    throw new ArgumentException("disposables for some reason are null", nameof(disposables));
                }

                _disposables.Add(d);
            }

            // _count can be read by other threads and thus should be properly visible
            // also releases the _disposables contents so it becomes thread-safe
            Volatile.Write(ref _count, _disposables.Count);
        }
    }
}
