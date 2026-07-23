// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>Provides platform-specific functionality for loading and creating bitmap images.</summary>
/// <remarks>This class implements the IBitmapLoader interface to support loading bitmaps from streams and
/// resources, as well as creating new bitmap instances. It is intended for use in scenarios where platform-dependent
/// image loading is required.</remarks>
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <summary>The default horizontal and vertical dots-per-inch used when creating a blank bitmap.</summary>
    private const double DefaultDpi = 96;

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
    public IBitmap Create(float width, float height) => new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, DefaultDpi, DefaultDpi, PixelFormats.Pbgra32, null));

    /// <summary>Runs the supplied initialization block on a <see cref="BitmapImage"/> between <c>BeginInit</c> and <c>EndInit</c>.</summary>
    /// <param name="source">The bitmap image to initialize.</param>
    /// <param name="block">The initialization actions to apply to <paramref name="source"/>.</param>
    private static void WithInit(BitmapImage source, Action<BitmapImage> block)
    {
        source.BeginInit();
        block(source);
        source.EndInit();

        if (!source.CanFreeze)
        {
            return;
        }

        source.Freeze();
    }
}
