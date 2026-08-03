// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.App;
using Android.Graphics;

namespace Splat;

/// <summary>
/// Provides helper methods for loading and creating bitmaps from streams, drawable resources, and Android drawables on
/// the current platform.
/// </summary>
/// <remarks>This class is intended for internal use to facilitate bitmap operations in platform-specific
/// scenarios, such as decoding images from streams or resources and handling stream termination issues that may arise
/// with certain image formats. All methods are static and thread safety depends on the usage of the provided streams
/// and resources.</remarks>
internal static class PlatformBitmapLoaderHelpers
{
    /// <summary>The minimum number of bytes a stream must contain to be considered a valid image.</summary>
    private const int MinimumImageStreamLength = 2;

    /// <summary>The first byte of the JPEG end-of-image (EOI) marker (0xFF).</summary>
    private const byte JpegEndOfImageMarkerByte1 = 0xFF;

    /// <summary>The second byte of the JPEG end-of-image (EOI) marker (0xD9).</summary>
    private const byte JpegEndOfImageMarkerByte2 = 0xD9;

    /// <summary>Loads a bitmap from a stream with optional desired dimensions.</summary>
    /// <param name="sourceStream">The stream to decode the bitmap from.</param>
    /// <param name="desiredWidth">The desired width of the bitmap, or <see langword="null"/> to use the source width.</param>
    /// <param name="desiredHeight">The desired height of the bitmap, or <see langword="null"/> to use the source height.</param>
    /// <param name="logger">The logger used to report any stream correction warnings, or <see langword="null"/> to suppress logging.</param>
    /// <returns>A task that resolves to the loaded <see cref="IBitmap"/>.</returns>
    internal static async Task<IBitmap?> LoadFromStream(Stream sourceStream, float? desiredWidth, float? desiredHeight, IEnableLogger? logger)
    {
        ArgumentExceptionHelper.ThrowIfNull(sourceStream);

        // this is a rough check to do with the termination check for #479
        if (sourceStream.Length < MinimumImageStreamLength)
        {
            throw new ArgumentException("The source stream is not a valid image file.", nameof(sourceStream));
        }

        if (!HasCorrectStreamEnd(sourceStream))
        {
            AttemptStreamByteCorrection(sourceStream, logger);
        }

        var bitmap = await Task.Run(() => Decode(sourceStream, desiredWidth, desiredHeight)).ConfigureAwait(false);

        return bitmap switch
        {
            null => throw new IOException("Failed to load bitmap from source stream"),
            _ => bitmap.FromNative()
        };
    }

    /// <summary>Loads a bitmap from a drawable resource ID.</summary>
    /// <param name="resourceId">The integer resource ID of the drawable to load.</param>
    /// <returns>The loaded <see cref="IBitmap"/>, or <see langword="null"/> if the drawable could not be resolved.</returns>
    internal static IBitmap? LoadFromDrawableId(int resourceId)
    {
        var res = Application.Context.Resources;
        var theme = Application.Context.Theme;

        if (res is null)
        {
            throw new InvalidOperationException("No resources found in the application.");
        }

        return GetFromDrawable(res.GetDrawable(resourceId, theme));
    }

    /// <summary>Creates a new bitmap with the specified dimensions.</summary>
    /// <param name="width">The width of the bitmap to create, in pixels.</param>
    /// <param name="height">The height of the bitmap to create, in pixels.</param>
    /// <returns>The newly created <see cref="IBitmap"/>.</returns>
    internal static IBitmap? CreateBitmap(float width, float height)
    {
        var config = Bitmap.Config.Argb8888 ?? throw new InvalidOperationException("The ARGB8888 bitmap format is unavailable");
        return Bitmap.CreateBitmap((int)width, (int)height, config).FromNative();
    }

    /// <summary>Converts an Android drawable to a Splat bitmap.</summary>
    /// <param name="drawable">The Android drawable to wrap, or <see langword="null"/>.</param>
    /// <returns>A <see cref="DrawableBitmap"/> wrapping the drawable, or <see langword="null"/> if <paramref name="drawable"/> is <see langword="null"/>.</returns>
    internal static DrawableBitmap? GetFromDrawable(Android.Graphics.Drawables.Drawable? drawable) =>
        drawable is null ? null : new DrawableBitmap(drawable);

    /// <summary>
    /// Checks to make sure the last 2 bytes are as expected.
    /// issue #479 xamarin android can throw an objectdisposedexception on stream
    /// suggestion is it relates to https://forums.xamarin.com/discussion/16500/bitmap-decode-byte-array-skia-decoder-returns-false
    /// and truncated jpeg\png files.
    /// </summary>
    /// <param name="sourceStream">Input image source stream.</param>
    /// <returns>Whether the termination is correct.</returns>
    internal static bool HasCorrectStreamEnd(Stream sourceStream)
    {
        sourceStream.Position = sourceStream.Length - MinimumImageStreamLength;
        return sourceStream.ReadByte() == JpegEndOfImageMarkerByte1
               && sourceStream.ReadByte() == JpegEndOfImageMarkerByte2;
    }

    /// <summary>Attempts to correct stream byte termination if possible.</summary>
    /// <param name="sourceStream">The stream to inspect and, if writable, correct.</param>
    /// <param name="logger">Optional logger used to report when the stream cannot be corrected.</param>
    internal static void AttemptStreamByteCorrection(Stream sourceStream, IEnableLogger? logger)
    {
        if (!sourceStream.CanWrite)
        {
            logger?.Log().Warn("Stream missing terminating bytes but is read only.");
        }
        else
        {
            logger?.Log().Warn("Carrying out source stream byte correction.");
            sourceStream.Position = sourceStream.Length;
            sourceStream.Write([JpegEndOfImageMarkerByte1, JpegEndOfImageMarkerByte2]);
        }
    }

    /// <summary>Decodes the stream, shrinking the image inside the decode when a size was asked for.</summary>
    /// <remarks>
    /// The source dimensions are read from the header first so the decode can subsample: that keeps the full-size
    /// pixels from ever being allocated, which is several times cheaper than decoding everything and scaling after.
    /// Subsampling only lands on powers of two, so a final scale settles the image on the exact fitted size.
    /// </remarks>
    /// <param name="sourceStream">The stream to decode the bitmap from.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The decoded bitmap, or <see langword="null"/> when the stream does not hold an image the decoder accepts.</returns>
    private static Bitmap? Decode(Stream sourceStream, float? desiredWidth, float? desiredHeight)
    {
        var (sourceWidth, sourceHeight) = ReadPixelSize(sourceStream);

        if (BitmapDecodeSize.ChooseFittedSize(sourceWidth, sourceHeight, desiredWidth, desiredHeight) is not { } target)
        {
            return DecodeStream(sourceStream, null);
        }

        var sampleSize = BitmapDecodeSize.ChooseSampleSize(sourceWidth, sourceHeight, target.Width, target.Height);

        using var options = new BitmapFactory.Options { InSampleSize = sampleSize };

        var decoded = DecodeStream(sourceStream, options);

        return decoded is null ? null : ScaleToFittedSize(decoded, target);
    }

    /// <summary>Reads the source dimensions from the stream's header without allocating any pixels.</summary>
    /// <param name="sourceStream">The stream to inspect.</param>
    /// <returns>The source dimensions, which the decoder reports as non-positive when it cannot read the header.</returns>
    private static (int Width, int Height) ReadPixelSize(Stream sourceStream)
    {
        using var bounds = new BitmapFactory.Options { InJustDecodeBounds = true };

        // A bounds-only decode reports the dimensions through the options and hands back no bitmap.
        DecodeStream(sourceStream, bounds)?.Dispose();

        return (bounds.OutWidth, bounds.OutHeight);
    }

    /// <summary>Rewinds the stream and hands it to the decoder.</summary>
    /// <param name="sourceStream">The stream to decode the bitmap from.</param>
    /// <param name="options">The decoder options, or <see langword="null"/> to decode at the source size.</param>
    /// <returns>The decoded bitmap, or <see langword="null"/> when the decoder produced none.</returns>
    private static Bitmap? DecodeStream(Stream sourceStream, BitmapFactory.Options? options)
    {
        sourceStream.Position = 0;
        return BitmapFactory.DecodeStream(sourceStream, null, options);
    }

    /// <summary>Settles a subsampled bitmap on the exact fitted size, releasing the intermediate.</summary>
    /// <param name="decoded">The bitmap the decoder produced.</param>
    /// <param name="target">The dimensions the caller's request works out to.</param>
    /// <returns>The bitmap at the fitted size, or <see langword="null"/> when the scale produced none.</returns>
    private static Bitmap? ScaleToFittedSize(Bitmap decoded, (int Width, int Height) target)
    {
        if (decoded.Width == target.Width && decoded.Height == target.Height)
        {
            return decoded;
        }

        var scaled = Bitmap.CreateScaledBitmap(decoded, target.Width, target.Height, true);

        decoded.Recycle();
        decoded.Dispose();

        return scaled;
    }
}
