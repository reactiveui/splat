// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>Provides methods for loading and creating bitmap images from streams and resources on the current platform.</summary>
/// <remarks>This class implements the IBitmapLoader interface to support platform-specific bitmap loading and
/// creation. It enables loading bitmaps from data streams or resource identifiers, with optional resizing. All methods
/// are thread-safe and return bitmaps suitable for use in platform graphics APIs.</remarks>
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <summary>The default screen DPI used when creating a blank writeable bitmap.</summary>
    private const double DefaultDpi = 96;

    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) =>
        Task.Run<IBitmap?>(() =>
        {
            var sourceSize = ReadPixelSize(sourceStream);
            var ret = new BitmapImage();

            WithInit(ret, source =>
            {
                ApplyDecodeSize(source, sourceSize, desiredWidth, desiredHeight);

                source.StreamSource = sourceStream;
                source.CacheOption = BitmapCacheOption.OnLoad;
            });

            return new BitmapSourceBitmap(ret);
        });

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight) =>
        Task.Run<IBitmap?>(() =>
        {
            var uri = new Uri(source, UriKind.RelativeOrAbsolute);
            var sourceSize = ReadPixelSize(uri);
            var ret = new BitmapImage();

            WithInit(ret, x =>
            {
                ApplyDecodeSize(x, sourceSize, desiredWidth, desiredHeight);

                x.UriSource = uri;
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
        new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, DefaultDpi, DefaultDpi, PixelFormats.Pbgra32, null));

    /// <summary>Determines which single decode dimension reproduces the requested size without distorting the image.</summary>
    /// <remarks>
    /// The imaging layer preserves the aspect ratio when exactly one of the decode dimensions is set, and stretches
    /// the image to fit when both are. Requesting both therefore has to be expressed as the one dimension that binds:
    /// whichever produces the smaller scale factor fits the whole image inside the requested box.
    /// </remarks>
    /// <param name="sourceSize">The pixel dimensions of the source image, or <see langword="null"/> when they could not be read.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The width and height to decode at, at most one of which is non-zero; zero means "derive from the other".</returns>
    internal static (int Width, int Height) ChooseDecodeSize((int Width, int Height)? sourceSize, float? desiredWidth, float? desiredHeight)
    {
        var width = desiredWidth is null ? 0 : Math.Max(1, (int)desiredWidth.Value);
        var height = desiredHeight is null ? 0 : Math.Max(1, (int)desiredHeight.Value);

        if (width == 0 || height == 0)
        {
            return (width, height);
        }

        // Without the source dimensions there is no way to tell which constraint binds, so honour the width and
        // let the height follow rather than stretching to both.
        if (sourceSize is not { Width: > 0, Height: > 0 } source)
        {
            return (width, 0);
        }

        return (double)width / source.Width <= (double)height / source.Height
            ? (width, 0)
            : (0, height);
    }

    /// <summary>Applies the chosen decode dimension to the bitmap being initialized.</summary>
    /// <param name="target">The bitmap image to configure.</param>
    /// <param name="sourceSize">The pixel dimensions of the source image, or <see langword="null"/> when they could not be read.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    private static void ApplyDecodeSize(BitmapImage target, (int Width, int Height)? sourceSize, float? desiredWidth, float? desiredHeight)
    {
        var (width, height) = ChooseDecodeSize(sourceSize, desiredWidth, desiredHeight);

        if (width > 0)
        {
            target.DecodePixelWidth = width;
            return;
        }

        if (height <= 0)
        {
            return;
        }

        target.DecodePixelHeight = height;
    }

    /// <summary>Reads the pixel dimensions from an image stream without decoding its pixels, restoring the position.</summary>
    /// <param name="sourceStream">The stream to inspect.</param>
    /// <returns>The source dimensions, or <see langword="null"/> when the stream cannot be inspected.</returns>
    private static (int Width, int Height)? ReadPixelSize(Stream sourceStream)
    {
        if (!sourceStream.CanSeek)
        {
            return null;
        }

        var origin = sourceStream.Position;
        try
        {
            var decoder = BitmapDecoder.Create(sourceStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            return ReadFirstFrameSize(decoder);
        }
        catch (NotSupportedException)
        {
            // The codec could not read a header it will also reject during the real decode; let that path report it.
            return null;
        }
        catch (FileFormatException)
        {
            return null;
        }
        finally
        {
            sourceStream.Position = origin;
        }
    }

    /// <summary>Reads the pixel dimensions from an image resource without decoding its pixels.</summary>
    /// <param name="source">The resource to inspect.</param>
    /// <returns>The source dimensions, or <see langword="null"/> when the resource cannot be inspected.</returns>
    private static (int Width, int Height)? ReadPixelSize(Uri source)
    {
        try
        {
            var decoder = BitmapDecoder.Create(source, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            return ReadFirstFrameSize(decoder);
        }
        catch (NotSupportedException)
        {
            return null;
        }
        catch (FileFormatException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
    }

    /// <summary>Reads the pixel dimensions of a decoder's first frame.</summary>
    /// <param name="decoder">The decoder to read from.</param>
    /// <returns>The first frame's dimensions, or <see langword="null"/> when the image carries no frame.</returns>
    private static (int Width, int Height)? ReadFirstFrameSize(BitmapDecoder decoder)
    {
        if (decoder.Frames.Count == 0)
        {
            return null;
        }

        var frame = decoder.Frames[0];
        return (frame.PixelWidth, frame.PixelHeight);
    }

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
