// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp;

/// <summary>An <see cref="IBitmapLoader"/> that decodes and encodes through Skia.</summary>
/// <remarks>
/// The plain .NET target frameworks have no imaging stack of their own, so this is the loader for
/// server, console, container and Linux hosts. It reads whatever the native library was built with
/// - PNG, JPEG, WebP and GIF everywhere, and more besides on some platforms.
/// </remarks>
public sealed class SkiaBitmapLoader : IBitmapLoader
{
    /// <summary>The buffer size used when opening a file, matching the framework's own default.</summary>
    private const int FileBufferSize = 4096;

    /// <summary>The sampling used for resizing when the caller does not choose one.</summary>
    private static readonly SKSamplingOptions _defaultSampling = new(SKFilterMode.Linear, SKMipmapMode.Linear);

    /// <summary>The sampling this loader resizes with.</summary>
    private readonly SKSamplingOptions _sampling;

    /// <summary>Initializes a new instance of the <see cref="SkiaBitmapLoader"/> class.</summary>
    public SkiaBitmapLoader()
        : this(_defaultSampling)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="SkiaBitmapLoader"/> class.</summary>
    /// <param name="sampling">The sampling to resize with. Skia offers nearest, linear and cubic resamplers.</param>
    public SkiaBitmapLoader(SKSamplingOptions sampling) => _sampling = sampling;

    /// <summary>Gets the sampling this loader resizes with.</summary>
    public SKSamplingOptions Sampling => _sampling;

    /// <inheritdoc />
    /// <remarks>
    /// Supplying both dimensions fits the image inside that box without distorting it; supplying one
    /// derives the other from the image's proportions. Only some codecs - JPEG in practice - can decode
    /// straight to a smaller size, so the rest are decoded whole and then resized.
    /// </remarks>
    public async Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
    {
        ArgumentExceptionHelper.ThrowIfNull(sourceStream);

        using var data = await ReadAsync(sourceStream).ConfigureAwait(false);

        return Decode(data, desiredWidth, desiredHeight, _sampling);
    }

    /// <inheritdoc />
    /// <remarks>
    /// There is no bundle or resource URI scheme on these target frameworks, so the source names a file:
    /// either an absolute path, or one relative to the directory the application was loaded from.
    /// </remarks>
    public async Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(source);

        var path = Path.IsPathRooted(source) ? source : Path.Combine(AppContext.BaseDirectory, source);

        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, FileBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);

        return await Load(stream, desiredWidth, desiredHeight).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public IBitmap Create(float width, float height)
    {
        ArgumentOutOfRangeExceptionHelper.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeExceptionHelper.ThrowIfNegativeOrZero(height);

        return new SkiaBitmap(new SKBitmap(Math.Max(1, (int)width), Math.Max(1, (int)height)));
    }

    /// <summary>Works out the size to decode to, honouring the image's proportions.</summary>
    /// <remarks>
    /// Supplying both dimensions asks for a fit rather than a stretch, so the constraint that produces the
    /// smaller scale factor binds and the other is derived from it.
    /// </remarks>
    /// <param name="source">The size of the image as it will be displayed.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The size to produce, never smaller than one pixel in either direction.</returns>
    internal static SKSizeI ChooseTargetSize(SKSizeI source, float? desiredWidth, float? desiredHeight)
    {
        var width = desiredWidth is null ? 0 : Math.Max(1, (int)desiredWidth.Value);
        var height = desiredHeight is null ? 0 : Math.Max(1, (int)desiredHeight.Value);

        if (width == 0 && height == 0)
        {
            return source;
        }

        if (width == 0)
        {
            return new(ScaleDimension(source.Width, height, source.Height), height);
        }

        if (height == 0)
        {
            return new(width, ScaleDimension(source.Height, width, source.Width));
        }

        return (double)width / source.Width <= (double)height / source.Height
            ? new SKSizeI(width, ScaleDimension(source.Height, width, source.Width))
            : new SKSizeI(ScaleDimension(source.Width, height, source.Height), height);
    }

    /// <summary>Reports whether an orientation exchanges the image's width and height.</summary>
    /// <param name="origin">The orientation to test.</param>
    /// <returns><see langword="true"/> when the upright image is the encoded one turned on its side.</returns>
    internal static bool SwapsDimensions(SKEncodedOrigin origin) =>
        origin is SKEncodedOrigin.LeftTop or SKEncodedOrigin.RightTop or SKEncodedOrigin.RightBottom or SKEncodedOrigin.LeftBottom;

    /// <summary>Turns the decoded pixels the right way up.</summary>
    /// <remarks>
    /// Skia hands back the pixels exactly as they were stored and reports the orientation separately, so a
    /// photograph taken side-on decodes side-on unless this is applied. Ownership of <paramref name="source"/>
    /// passes to this method: it is either handed straight back or released once its pixels have been copied.
    /// </remarks>
    /// <param name="source">The decoded bitmap, in the orientation it was encoded in.</param>
    /// <param name="origin">The orientation the source image recorded.</param>
    /// <param name="sampling">The sampling to draw with.</param>
    /// <returns>The upright bitmap.</returns>
    internal static SKBitmap ApplyEncodedOrigin(SKBitmap source, SKEncodedOrigin origin, SKSamplingOptions sampling)
    {
        if (origin == SKEncodedOrigin.TopLeft)
        {
            return source;
        }

        var swaps = SwapsDimensions(origin);
        var upright = new SKBitmap(source.Info.WithSize(swaps ? source.Height : source.Width, swaps ? source.Width : source.Height));
        var matrix = CreateOriginMatrix(origin, source.Width, source.Height);

        using (source)
        {
            using var canvas = new SKCanvas(upright);
            canvas.SetMatrix(in matrix);
            canvas.DrawBitmap(source, 0, 0, sampling);
        }

        return upright;
    }

    /// <summary>Builds the transform that maps encoded pixel positions to upright ones.</summary>
    /// <remarks>
    /// The eight orientations are the four rotations and their mirror images, so each is a signed
    /// permutation of the two axes with a translation that brings the result back into the first quadrant.
    /// </remarks>
    /// <param name="origin">The orientation the source image recorded.</param>
    /// <param name="width">The encoded width.</param>
    /// <param name="height">The encoded height.</param>
    /// <returns>The transform to draw the encoded pixels through.</returns>
    internal static SKMatrix CreateOriginMatrix(SKEncodedOrigin origin, int width, int height) => origin switch
    {
        SKEncodedOrigin.TopRight => new(-1, 0, width, 0, 1, 0, 0, 0, 1),
        SKEncodedOrigin.BottomRight => new(-1, 0, width, 0, -1, height, 0, 0, 1),
        SKEncodedOrigin.BottomLeft => new(1, 0, 0, 0, -1, height, 0, 0, 1),
        SKEncodedOrigin.LeftTop => new(0, 1, 0, 1, 0, 0, 0, 0, 1),
        SKEncodedOrigin.RightTop => new(0, -1, height, 1, 0, 0, 0, 0, 1),
        SKEncodedOrigin.RightBottom => new(0, -1, height, -1, 0, width, 0, 0, 1),
        SKEncodedOrigin.LeftBottom => new(0, 1, 0, -1, 0, width, 0, 0, 1),
        _ => SKMatrix.CreateIdentity(),
    };

    /// <summary>Decodes encoded image data to the requested size.</summary>
    /// <param name="data">The encoded image.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="sampling">The sampling to resize with.</param>
    /// <returns>The decoded image, or <see langword="null"/> when no codec recognised the data.</returns>
    internal static SkiaBitmap? Decode(SKData data, float? desiredWidth, float? desiredHeight, SKSamplingOptions sampling)
    {
        using var codec = CreateCodec(data);
        if (codec is null)
        {
            return null;
        }

        var origin = codec.EncodedOrigin;
        var encoded = codec.Info.Size;
        var oriented = SwapsDimensions(origin) ? new SKSizeI(encoded.Height, encoded.Width) : encoded;
        var target = ChooseTargetSize(oriented, desiredWidth, desiredHeight);

        var decoded = DecodeAtLeast(codec, encoded, oriented, target);

        return new(ResizeTo(ApplyEncodedOrigin(decoded, origin, sampling), target, sampling), origin);
    }

    /// <summary>Creates a codec for encoded image data.</summary>
    /// <remarks>
    /// Skia annotates this as always succeeding, but it hands back nothing at all when no codec recognises
    /// the data, which is the ordinary answer for a stream that does not hold an image.
    /// </remarks>
    /// <param name="data">The encoded image.</param>
    /// <returns>The codec, or <see langword="null"/> when no codec recognised the data.</returns>
    private static SKCodec? CreateCodec(SKData data) => SKCodec.Create(data);

    /// <summary>Decodes at the closest size at or above the target that the codec can produce directly.</summary>
    /// <remarks>
    /// Only some codecs - JPEG in practice - support a scaled decode; the rest report their full size and
    /// are resized afterwards. Asking for a size the codec did not offer makes it refuse to decode at all,
    /// so the size has to come from the codec rather than from the caller.
    /// </remarks>
    /// <param name="codec">The codec to decode with.</param>
    /// <param name="encoded">The encoded pixel dimensions.</param>
    /// <param name="oriented">The encoded dimensions as they will be displayed.</param>
    /// <param name="target">The size wanted, in display orientation.</param>
    /// <returns>The decoded bitmap.</returns>
    private static SKBitmap DecodeAtLeast(SKCodec codec, SKSizeI encoded, SKSizeI oriented, SKSizeI target)
    {
        // Orientation only exchanges the two axes, so the linear scale factor is the same either way round.
        var scale = (float)target.Width / oriented.Width;
        var scaled = scale >= 1F ? encoded : codec.GetScaledDimensions(scale);

        return SKBitmap.Decode(codec, codec.Info.WithSize(scaled));
    }

    /// <summary>Resizes to the target size, when the bitmap is not already that size.</summary>
    /// <remarks>Ownership of <paramref name="source"/> passes to this method.</remarks>
    /// <param name="source">The bitmap to resize.</param>
    /// <param name="target">The size wanted.</param>
    /// <param name="sampling">The sampling to resize with.</param>
    /// <returns>The bitmap at the target size.</returns>
    private static SKBitmap ResizeTo(SKBitmap source, SKSizeI target, SKSamplingOptions sampling)
    {
        if (source.Width == target.Width && source.Height == target.Height)
        {
            return source;
        }

        using (source)
        {
            return source.Resize(target, sampling);
        }
    }

    /// <summary>Reads a stream into memory Skia can decode from.</summary>
    /// <param name="source">The stream to read.</param>
    /// <returns>The stream's remaining content.</returns>
    private static async Task<SKData> ReadAsync(Stream source)
    {
        await using var buffer = new MemoryStream();

        await source.CopyToAsync(buffer).ConfigureAwait(false);

        return SKData.CreateCopy(buffer.GetBuffer().AsSpan(0, (int)buffer.Length));
    }

    /// <summary>Scales one dimension by the ratio another was scaled by.</summary>
    /// <param name="value">The dimension to scale.</param>
    /// <param name="numerator">The scaled size of the other dimension.</param>
    /// <param name="denominator">The original size of the other dimension.</param>
    /// <returns>The scaled dimension, never below one pixel.</returns>
    private static int ScaleDimension(int value, int numerator, int denominator) =>
        Math.Max(1, (int)Math.Round((double)value * numerator / denominator));
}
