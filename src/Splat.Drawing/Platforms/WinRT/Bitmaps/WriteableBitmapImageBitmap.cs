// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// A bitmap that wraps a <see cref="WriteableBitmap"/>.
    /// </summary>
    internal sealed class WriteableBitmapImageBitmap : IBitmap
    {
        public WriteableBitmapImageBitmap(WriteableBitmap bitmap)
        {
            Inner = bitmap;
        }

        /// <inheritdoc />
        public float Width => Inner.PixelWidth;

        /// <inheritdoc />
        public float Height => Inner.PixelHeight;

        /// <summary>
        /// Gets the platform <see cref="WriteableBitmap"/>.
        /// </summary>
        public WriteableBitmap Inner { get; private set; }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "Local variable will be disposed.")]
        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            // NB: Due to WinRT's brain-dead design, we're copying this image
            // like three times. Let Dreams Soar.
            using var rwTarget = new InMemoryRandomAccessStream();
            var fmt = format == CompressedBitmapFormat.Jpeg ? BitmapEncoder.JpegEncoderId : BitmapEncoder.PngEncoderId;
            var encoder = await BitmapEncoder.CreateAsync(fmt, rwTarget, new[] { new KeyValuePair<string, BitmapTypedValue>("ImageQuality", new BitmapTypedValue(quality, PropertyType.Single)) });

            var pixels = new byte[Inner.PixelBuffer.Length];
            await Inner.PixelBuffer.AsStream().ReadAsync(pixels, 0, (int)Inner.PixelBuffer.Length).ConfigureAwait(true);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)Inner.PixelWidth, (uint)Inner.PixelHeight, 96, 96, pixels);
            await encoder.FlushAsync();
            await rwTarget.AsStream().CopyToAsync(target).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Inner = null;
        }
    }
}
