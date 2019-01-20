// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;

namespace Splat
{
    /// <summary>
    /// Wraps a android native bitmap into the splat <see cref="IBitmap"/>.
    /// </summary>
    internal sealed class AndroidBitmap : IBitmap
    {
        private Bitmap _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidBitmap"/> class.
        /// </summary>
        /// <param name="inner">The bitmap we are wrapping.</param>
        public AndroidBitmap(Bitmap inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public float Width => _inner.Width;

        /// <inheritdoc />
        public float Height => _inner.Height;

        /// <summary>
        /// Gets the internal bitmap we are wrapping.
        /// </summary>
        internal Bitmap Inner => _inner;

        /// <inheritdoc />
        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            var fmt = format == CompressedBitmapFormat.Jpeg ? Bitmap.CompressFormat.Jpeg : Bitmap.CompressFormat.Png;
            return Task.Run(() => _inner.Compress(fmt, (int)(quality * 100), target));
        }

        /// <inheritdoc />
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
