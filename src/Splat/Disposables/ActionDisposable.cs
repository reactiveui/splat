// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Splat
{
    /// <summary>
    /// A disposable which will call the specified action.
    /// </summary>
    internal sealed class ActionDisposable : IDisposable
    {
        private Action _block;

        public ActionDisposable(Action block)
        {
            _block = block;
        }

        /// <summary>
        /// Gets a action disposable which does nothing.
        /// </summary>
        public static IDisposable Empty => new ActionDisposable(() => { });

        /// <inheritdoc />
        public void Dispose()
        {
            Interlocked.Exchange(ref _block, () => { })();
        }
    }
}
