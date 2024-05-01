// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Tizen.Multimedia.Util;

namespace Splat;

/// <summary>
/// Wraps a tizen native bitmap into the splat <see cref="IBitmap"/>.
/// </summary>
internal sealed class TizenBitmap : IBitmap
{
    private static readonly ImageDecoder[] _decoderList =
    [
        new JpegDecoder(),
        new PngDecoder(),
        new BmpDecoder(),
        new GifDecoder(),
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="TizenBitmap"/> class.
    /// </summary>
    /// <param name="image">The image in bytes we are wrapping. Will generate a native platform bitmap for us.</param>
    public TizenBitmap(byte[] image) => Inner = GetBitmapFrame(image);

    /// <summary>
    /// Initializes a new instance of the <see cref="TizenBitmap"/> class.
    /// </summary>
    /// <param name="imageResourceName">The name of the image resource to load.</param>
    public TizenBitmap(string imageResourceName) => Inner = GetBitmapFrame(File.ReadAllBytes(imageResourceName));

    public TizenBitmap(BitmapFrame image) => Inner = image;

    /// <inheritdoc />
    public float Width => Inner?.Size.Width ?? 0;

    /// <inheritdoc />
    public float Height => Inner?.Size.Height ?? 0;

    /// <summary>
    /// Gets the native bitmap.
    /// </summary>
    internal BitmapFrame? Inner { get; private set; }

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target)
    {
        if (Inner is null)
        {
            return Task.CompletedTask;
        }

        ImageEncoder? encoder = null;
        try
        {
            var qualityPercent = (int)(100 * quality);
            switch (format)
            {
                case CompressedBitmapFormat.Jpeg:
                    encoder = new JpegEncoder();
                    ((JpegEncoder)encoder).Quality = qualityPercent;
                    break;
                case CompressedBitmapFormat.Png:
                    encoder = new PngEncoder();
                    ((PngEncoder)encoder).Compression = qualityPercent switch
                    {
                        100 => PngCompression.None,
                        < 10 => PngCompression.Level1,
                        _ => (PngCompression)(qualityPercent / 10),
                    };
                    break;
            }

            if (encoder is null)
            {
                return Task.CompletedTask;
            }

            encoder.SetResolution(new((int)Width, (int)Height));
            return encoder.EncodeAsync(Inner.Buffer, target);
        }
        finally
        {
            encoder?.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose() => Inner = null;

    private static BitmapFrame? GetBitmapFrame(byte[] imageBuffer)
    {
        BitmapFrame? result = null;

        foreach (var decoder in _decoderList)
        {
            try
            {
                result = decoder.DecodeAsync(imageBuffer).Result.First();
                break;
            }
            catch (Tizen.Multimedia.FileFormatException)
            {
            }
        }

        return result;
    }
}
