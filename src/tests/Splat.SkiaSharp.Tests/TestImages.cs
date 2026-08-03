// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Builds the images the tests decode, so no binary fixtures have to be checked in.</summary>
internal static class TestImages
{
    /// <summary>The encoder quality the fixtures are written at.</summary>
    private const int FixtureQuality = 90;

    /// <summary>The offset of the orientation value inside <see cref="_orientationSegment"/>.</summary>
    private const int OrientationValueOffset = 28;

    /// <summary>The offset a marker segment is spliced in at, which is just past the start-of-image marker.</summary>
    private const int SegmentOffset = 2;

    /// <summary>
    /// A JPEG application segment holding the smallest well-formed metadata block that records an orientation:
    /// the Exif identifier, a little-endian header, and a single directory entry for the orientation tag.
    /// </summary>
    private static readonly byte[] _orientationSegment =
    [
        0xFF, 0xE1, 0x00, 0x22,
        0x45, 0x78, 0x69, 0x66, 0x00, 0x00,
        0x49, 0x49, 0x2A, 0x00, 0x08, 0x00, 0x00, 0x00,
        0x01, 0x00,
        0x12, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
    ];

    /// <summary>Creates a white bitmap with a single red pixel in its top left corner.</summary>
    /// <remarks>The marked corner is what makes a rotation or a mirroring visible in an assertion.</remarks>
    /// <param name="width">The width to create.</param>
    /// <param name="height">The height to create.</param>
    /// <returns>The bitmap.</returns>
    internal static SKBitmap CreateCornerMarked(int width, int height)
    {
        var bitmap = new SKBitmap(width, height);

        using var canvas = new SKCanvas(bitmap);
        using var paint = new SKPaint { Color = SKColors.Red };

        canvas.Clear(SKColors.White);
        canvas.DrawRect(0, 0, 1, 1, paint);

        return bitmap;
    }

    /// <summary>Encodes a corner-marked bitmap.</summary>
    /// <param name="width">The width to create.</param>
    /// <param name="height">The height to create.</param>
    /// <param name="format">The encoder to use.</param>
    /// <returns>The encoded image.</returns>
    internal static byte[] Encode(int width, int height, SKEncodedImageFormat format)
    {
        using var bitmap = CreateCornerMarked(width, height);
        using var data = bitmap.Encode(format, FixtureQuality);

        return data.ToArray();
    }

    /// <summary>Encodes a corner-marked JPEG that records the given orientation.</summary>
    /// <remarks>
    /// No encoder writes this metadata, so it is spliced in: the segment goes immediately after the
    /// start-of-image marker, which is where a decoder looks for it.
    /// </remarks>
    /// <param name="width">The width to create.</param>
    /// <param name="height">The height to create.</param>
    /// <param name="orientation">The orientation to record, numbered as the metadata standard numbers them.</param>
    /// <returns>The encoded image.</returns>
    internal static byte[] EncodeWithOrientation(int width, int height, int orientation)
    {
        var jpeg = Encode(width, height, SKEncodedImageFormat.Jpeg);
        var segment = _orientationSegment.AsSpan().ToArray();
        segment[OrientationValueOffset] = (byte)orientation;

        var tagged = new byte[jpeg.Length + segment.Length];
        jpeg.AsSpan(0, SegmentOffset).CopyTo(tagged);
        segment.CopyTo(tagged.AsSpan(SegmentOffset));
        jpeg.AsSpan(SegmentOffset).CopyTo(tagged.AsSpan(SegmentOffset + segment.Length));

        return tagged;
    }

    /// <summary>Opens a stream over an encoded corner-marked bitmap.</summary>
    /// <param name="width">The width to create.</param>
    /// <param name="height">The height to create.</param>
    /// <param name="format">The encoder to use.</param>
    /// <returns>The stream.</returns>
    internal static MemoryStream OpenStream(int width, int height, SKEncodedImageFormat format) =>
        new(Encode(width, height, format));

    /// <summary>Finds the single red pixel a corner-marked bitmap was built with.</summary>
    /// <param name="bitmap">The bitmap to search.</param>
    /// <returns>The pixel position, or <see langword="null"/> when nothing in the bitmap is red.</returns>
    internal static (int X, int Y)? FindMarker(SKBitmap bitmap)
    {
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                if (bitmap.GetPixel(x, y) == SKColors.Red)
                {
                    return (x, y);
                }
            }
        }

        return null;
    }
}
