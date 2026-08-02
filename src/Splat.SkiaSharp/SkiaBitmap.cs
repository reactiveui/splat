// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp;

/// <summary>An <see cref="IBitmap"/> whose pixels are held by Skia.</summary>
/// <remarks>
/// The instance owns the bitmap it is given and releases it on <see cref="Dispose"/>, so a caller
/// that wants to keep drawing with the Skia bitmap afterwards has to hand over a copy.
/// </remarks>
public sealed class SkiaBitmap : IBitmap
{
    /// <summary>The percentage Skia's encoders express quality in.</summary>
    private const float EncoderQualityScale = 100F;

    /// <summary>The bitmap being wrapped, cleared once it has been disposed.</summary>
    private SKBitmap? _inner;

    /// <summary>Initializes a new instance of the <see cref="SkiaBitmap"/> class.</summary>
    /// <param name="bitmap">The bitmap to take ownership of.</param>
    public SkiaBitmap(SKBitmap bitmap)
        : this(bitmap, SKEncodedOrigin.TopLeft)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="SkiaBitmap"/> class.</summary>
    /// <param name="bitmap">The bitmap to take ownership of, already in its upright orientation.</param>
    /// <param name="encodedOrigin">The orientation the source image recorded.</param>
    public SkiaBitmap(SKBitmap bitmap, SKEncodedOrigin encodedOrigin)
    {
        ArgumentExceptionHelper.ThrowIfNull(bitmap);

        _inner = bitmap;
        EncodedOrigin = encodedOrigin;
    }

    /// <inheritdoc />
    public float Width => _inner?.Width ?? 0;

    /// <inheritdoc />
    public float Height => _inner?.Height ?? 0;

    /// <summary>Gets the orientation the source image recorded, which the loader has already applied to the pixels.</summary>
    /// <remarks>
    /// Anything other than <see cref="SKEncodedOrigin.TopLeft"/> means the encoded pixels were stored rotated or
    /// mirrored; the value is kept so a caller re-encoding the image can decide what orientation to record.
    /// </remarks>
    public SKEncodedOrigin EncodedOrigin { get; }

    /// <summary>Gets the Skia bitmap backing this instance, or <see langword="null"/> once it has been disposed.</summary>
    public SKBitmap? Inner => _inner;

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target) =>
        Save(format == CompressedBitmapFormat.Jpeg ? SKEncodedImageFormat.Jpeg : SKEncodedImageFormat.Png, quality, target);

    /// <summary>Saves the image in any format Skia can encode, rather than only the two <see cref="CompressedBitmapFormat"/> names.</summary>
    /// <param name="format">The encoder to use. Which ones are built into the native library varies by platform.</param>
    /// <param name="quality">A factor between 0 and 1, where 1 is the best quality. Lossless encoders ignore it.</param>
    /// <param name="target">The stream to write the encoded image to.</param>
    /// <returns>A task that completes once the encoded image has been written.</returns>
    /// <exception cref="BitmapLoaderException">The native library has no encoder for the requested format.</exception>
    public Task Save(SKEncodedImageFormat format, float quality, Stream target)
    {
        ArgumentExceptionHelper.ThrowIfNull(target);

        var bitmap = _inner;
        ObjectDisposedExceptionHelper.ThrowIf(bitmap is null, this);

        return SaveCore(bitmap, format, quality, target);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _inner?.Dispose();
        _inner = null;
    }

    /// <summary>Encodes the bitmap and writes it to the target stream.</summary>
    /// <param name="bitmap">The bitmap to encode.</param>
    /// <param name="format">The encoder to use.</param>
    /// <param name="quality">A factor between 0 and 1, where 1 is the best quality.</param>
    /// <param name="target">The stream to write the encoded image to.</param>
    /// <returns>A task that completes once the encoded image has been written.</returns>
    private static async Task SaveCore(SKBitmap bitmap, SKEncodedImageFormat format, float quality, Stream target)
    {
        using var data = bitmap.Encode(format, ToEncoderQuality(quality))
            ?? throw new BitmapLoaderException($"The native library has no encoder for {format}.");
        await using var source = data.AsStream();

        await source.CopyToAsync(target).ConfigureAwait(false);
    }

    /// <summary>Converts the interface's 0-to-1 quality factor to the percentage Skia's encoders take.</summary>
    /// <param name="quality">The quality factor to convert.</param>
    /// <returns>The quality as a percentage.</returns>
    private static int ToEncoderQuality(float quality) => (int)MathF.Round(Math.Clamp(quality, 0F, 1F) * EncoderQualityScale);
}
