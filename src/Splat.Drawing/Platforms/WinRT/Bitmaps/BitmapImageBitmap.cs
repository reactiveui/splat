// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// A bitmap that wraps a <see cref="BitmapImage"/>.
    /// </summary>
    internal sealed class BitmapImageBitmap : IBitmap
    {
        private BitmapImage _inner;

        public BitmapImageBitmap(BitmapImage bitmap)
        {
            _inner = bitmap;
        }

        /// <inheritdoc />
        public float Width => Inner.PixelWidth;

        /// <inheritdoc />
        public float Height => Inner.PixelHeight;

        /// <summary>
        /// Gets the platform <see cref="BitmapImage"/>.
        /// </summary>
        public BitmapSource Inner => _inner;

        /// <inheritdoc />
        [SuppressMessage("Globalization", "CA1307: Use IFormatProvider", Justification = "string.Replace does not have a IFormatProvider on all .NET platforms")]
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "WriteableBitmapImageBitMap will handle of disposing.")]
        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            string installedFolderImageSourceUri = _inner.UriSource.OriginalString.Replace("ms-appx:/", string.Empty);
            var wb = new WriteableBitmap(_inner.PixelWidth, _inner.PixelHeight);
            var file = await StorageFile.GetFileFromPathAsync(_inner.UriSource.OriginalString);
            await wb.SetSourceAsync(await file.OpenReadAsync());

            await new WriteableBitmapImageBitmap(wb).Save(format, quality, target).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _inner = null;
        }
    }
}
