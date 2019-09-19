// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Imaging;
using Windows.Storage;
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
                using (var randomAccessStream = sourceStream.AsRandomAccessStream())
                {
                    randomAccessStream.Seek(0);
                    var decoder = await BitmapDecoder.CreateAsync(randomAccessStream);

                    int targetWidth = (int)(desiredWidth ?? decoder.OrientedPixelWidth);
                    int targetHeight = (int)(desiredHeight ?? decoder.OrientedPixelHeight);

                    var transform = new BitmapTransform
                    {
                        ScaledWidth = (uint)targetWidth,
                        ScaledHeight = (uint)targetHeight,
                        InterpolationMode = BitmapInterpolationMode.Fant
                    };

                    if (decoder.OrientedPixelHeight != decoder.PixelHeight)
                    {
                        // if Exif orientation indicates 90 or 270 degrees rotation we swap width and height for the transformation.
                        transform.ScaledWidth = (uint)targetHeight;
                        transform.ScaledHeight = (uint)targetWidth;
                    }

                    var pixelData = await decoder.GetPixelDataAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
                    var pixels = pixelData.DetachPixelData();

                    var bmp = new WriteableBitmap(targetWidth, targetHeight);
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
