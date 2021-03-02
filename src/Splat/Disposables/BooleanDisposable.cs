// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// Represents a disposable resource that can be checked for disposal status.
    /// Based on the System.Reactive.Disposable class.
    /// </summary>
    internal sealed class BooleanDisposable : IDisposable
    {
        private volatile bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanDisposable"/> class.
        /// </summary>
        public BooleanDisposable()
        {
        }

        private BooleanDisposable(bool isDisposed)
        {
            _isDisposed = isDisposed;
        }

        /// <summary>
        /// Gets a value indicating whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _isDisposed;

        /// <summary>
        /// Sets the status to disposed, which can be observer through the <see cref="IsDisposed"/> property.
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
