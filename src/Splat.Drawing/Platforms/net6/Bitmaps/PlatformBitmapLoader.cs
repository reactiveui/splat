// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>
/// A XAML based platform bitmap loader which will load our bitmaps for us.
/// </summary>
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) =>
        Task.Run<IBitmap?>(() =>
        {
            var ret = new BitmapImage();

            WithInit(ret, source =>
            {
                if (desiredWidth is not null)
                {
                    source.DecodePixelWidth = (int)desiredWidth;
                }

                if (desiredHeight is not null)
                {
                    source.DecodePixelHeight = (int)desiredHeight;
                }

                source.StreamSource = sourceStream;
                source.CacheOption = BitmapCacheOption.OnLoad;
            });

            return new BitmapSourceBitmap(ret);
        });

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight) =>
        Task.Run<IBitmap?>(() =>
        {
            var ret = new BitmapImage();
            WithInit(ret, x =>
            {
                if (desiredWidth is not null)
                {
                    x.DecodePixelWidth = (int)desiredWidth;
                }

                if (desiredHeight is not null)
                {
                    x.DecodePixelHeight = (int)desiredHeight;
                }

                x.UriSource = new(source, UriKind.RelativeOrAbsolute);
            });

            return new BitmapSourceBitmap(ret);
        });

    /// <inheritdoc />
    public IBitmap Create(float width, float height) =>
        /*
         * Taken from MSDN:
         *
         * The preferred values for pixelFormat are Bgr32 and Pbgra32.
         * These formats are natively supported and do not require a format conversion.
         * Other pixelFormat values require a format conversion for each frame update, which reduces performance.
         */
        new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32, null));

    private static void WithInit(BitmapImage source, Action<BitmapImage> block)
    {
        source.BeginInit();
        block(source);
        source.EndInit();

        if (source.CanFreeze)
        {
            source.Freeze();
        }
    }
}
