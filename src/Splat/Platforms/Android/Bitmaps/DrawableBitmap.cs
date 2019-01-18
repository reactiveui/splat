// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics.Drawables;

namespace Splat
{
    internal sealed class DrawableBitmap : IBitmap
    {
        private Drawable _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableBitmap"/> class.
        /// </summary>
        /// <param name="inner">The drawable bitmap to wrap.</param>
        public DrawableBitmap(Drawable inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public float Width => Inner.IntrinsicWidth;

        /// <inheritdoc />
        public float Height => Inner.IntrinsicHeight;

        /// <summary>
        /// Gets the internal Drawable we are wrapping.
        /// </summary>
        internal Drawable Inner => _inner;

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            throw new NotSupportedException("You can't save resources");
        }

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref _inner, null);
            if (disp != null)
            {
                disp.Dispose();
            }
        }
    }
}
