// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// A XAML based platform bitmap loader which will load our bitmaps for us.
    /// </summary>
    public class PlatformBitmapLoader : IBitmapLoader
    {
        /// <inheritdoc />
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return GetDispatcher().RunTaskAsync(async () =>
            {
                using (var rwStream = new InMemoryRandomAccessStream())
                {
                    var writer = rwStream.AsStreamForWrite();
                    await sourceStream.CopyToAsync(writer).ConfigureAwait(true);
                    await writer.FlushAsync().ConfigureAwait(true);
                    rwStream.Seek(0);

                    var decoder = await BitmapDecoder.CreateAsync(rwStream);

                    var transform = new BitmapTransform
                    {
                        ScaledWidth = (uint)(desiredWidth ?? decoder.OrientedPixelWidth),
                        ScaledHeight = (uint)(desiredHeight ?? decoder.OrientedPixelHeight),
                        InterpolationMode = BitmapInterpolationMode.Fant
                    };

                    var pixelData = await decoder.GetPixelDataAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
                    var pixels = pixelData.DetachPixelData();

                    var bmp = new WriteableBitmap((int)transform.ScaledWidth, (int)transform.ScaledHeight);
                    using (var bmpStream = bmp.PixelBuffer.AsStream())
                    {
                        bmpStream.Seek(0, SeekOrigin.Begin);
                        bmpStream.Write(pixels, 0, (int)bmpStream.Length);
                        return (IBitmap)new WriteableBitmapImageBitmap(bmp);
                    }
                }
            });
        }

        /// <inheritdoc />
        public Task<IBitmap> LoadFromResource(string resource, float? desiredWidth, float? desiredHeight)
        {
            return GetDispatcher().RunTaskAsync(async () =>
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(resource));
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    return await Load(stream.AsStreamForRead(), desiredWidth, desiredHeight).ConfigureAwait(false);
                }
            });
        }

        /// <inheritdoc />
        public IBitmap Create(float width, float height)
        {
            return new WriteableBitmapImageBitmap(new WriteableBitmap((int)width, (int)height));
        }

        private static CoreDispatcher GetDispatcher()
        {
            CoreWindow currentThreadWindow = CoreWindow.GetForCurrentThread();

            return currentThreadWindow == null ? CoreApplication.MainView.CoreWindow.Dispatcher : currentThreadWindow.Dispatcher;
        }
    }
}
