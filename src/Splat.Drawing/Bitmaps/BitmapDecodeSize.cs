// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Works out the pixel dimensions a platform decoder has to be asked for so that a caller-supplied width and height
/// are honoured.
/// </summary>
/// <remarks>
/// <para>
/// A requested width and height describe a box the image has to fit inside, not a shape it has to be stretched to.
/// When both are supplied the edge that produces the smaller scale factor binds and the other edge follows from the
/// source proportions; when one is supplied the other is derived the same way. That contract is shared by every
/// platform loader so callers see one behaviour.
/// </para>
/// <para>
/// Nothing here touches a platform imaging API, so the arithmetic is exercised directly by the tests while the
/// platform loaders stay thin wrappers around it.
/// </para>
/// </remarks>
internal static class BitmapDecodeSize
{
    /// <summary>The factor between one subsampling step and the next; decoders only accept powers of two.</summary>
    private const int SubsamplingStep = 2;

    /// <summary>The lowest orientation code, as recorded by an image container, that transposes the stored pixels.</summary>
    private const int FirstTransposingOrientation = 5;

    /// <summary>The highest orientation code, as recorded by an image container, that transposes the stored pixels.</summary>
    private const int LastTransposingOrientation = 8;

    /// <summary>Works out the exact pixel size to produce so the source fits the requested box without distortion.</summary>
    /// <param name="sourceWidth">The width of the source image, in pixels.</param>
    /// <param name="sourceHeight">The height of the source image, in pixels.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>
    /// The dimensions to produce, or <see langword="null"/> when the image should be produced at its source size
    /// because nothing was requested or because the source could not be measured.
    /// </returns>
    internal static (int Width, int Height)? ChooseFittedSize(int sourceWidth, int sourceHeight, float? desiredWidth, float? desiredHeight)
    {
        if (desiredWidth is null && desiredHeight is null)
        {
            return null;
        }

        if (Math.Min(sourceWidth, sourceHeight) <= 0)
        {
            return null;
        }

        var width = ToWholePixels(desiredWidth);
        var height = ToWholePixels(desiredHeight);

        // The edge that shrinks the image the most is the one that keeps all of it inside the requested box; cross
        // multiplying compares the two scale factors without dividing.
        return height is null || (width is { } requestedWidth && (long)requestedWidth * sourceHeight <= (long)height.Value * sourceWidth)
            ? (width!.Value, DeriveOppositeEdge(sourceHeight, width.Value, sourceWidth))
            : (DeriveOppositeEdge(sourceWidth, height.Value, sourceHeight), height.Value);
    }

    /// <summary>
    /// Works out the largest power-of-two subsampling factor a decoder can apply while still producing at least the
    /// requested dimensions.
    /// </summary>
    /// <remarks>
    /// Subsampling happens inside the decode, so the full-size pixels are never materialised. It only lands on powers
    /// of two, which is why an exact size still needs a final scale of whatever this leaves behind.
    /// </remarks>
    /// <param name="sourceWidth">The width of the source image, in pixels.</param>
    /// <param name="sourceHeight">The height of the source image, in pixels.</param>
    /// <param name="targetWidth">The width wanted from the decode, in pixels.</param>
    /// <param name="targetHeight">The height wanted from the decode, in pixels.</param>
    /// <returns>The subsampling factor, which is always at least one.</returns>
    internal static int ChooseSampleSize(int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
    {
        if (Math.Min(sourceWidth, sourceHeight) <= 0 || Math.Min(targetWidth, targetHeight) <= 0)
        {
            return 1;
        }

        var sampleSize = 1;
        while (sourceWidth / (sampleSize * SubsamplingStep) >= targetWidth
               && sourceHeight / (sampleSize * SubsamplingStep) >= targetHeight)
        {
            sampleSize *= SubsamplingStep;
        }

        return sampleSize;
    }

    /// <summary>Works out the longest edge a thumbnail decoder may produce so the image fits the requested box.</summary>
    /// <remarks>
    /// A thumbnail decoder is constrained by a single number bounding the longer edge, and it keeps the proportions
    /// itself, so the fitted box collapses to whichever of its edges is longer.
    /// </remarks>
    /// <param name="sourceWidth">The width of the source image, in pixels.</param>
    /// <param name="sourceHeight">The height of the source image, in pixels.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The longest permitted edge, or <see langword="null"/> when the image should be decoded at its source size.</returns>
    internal static int? ChooseThumbnailPixelSize(int sourceWidth, int sourceHeight, float? desiredWidth, float? desiredHeight) =>
        ChooseFittedSize(sourceWidth, sourceHeight, desiredWidth, desiredHeight) is { } fitted
            ? Math.Max(fitted.Width, fitted.Height)
            : null;

    /// <summary>Reports the dimensions an image presents once the orientation recorded alongside its pixels is applied.</summary>
    /// <remarks>
    /// A container may store a photograph rotated a quarter turn and record how to put it back. A decoder that applies
    /// that rotation hands back transposed dimensions, so the box has to be fitted against the transposed size rather
    /// than the stored one.
    /// </remarks>
    /// <param name="pixelWidth">The width of the stored pixels.</param>
    /// <param name="pixelHeight">The height of the stored pixels.</param>
    /// <param name="orientation">The orientation code recorded by the container.</param>
    /// <returns>The dimensions the image presents once oriented.</returns>
    internal static (int Width, int Height) OrientedPixelSize(int pixelWidth, int pixelHeight, int orientation) =>
        orientation is >= FirstTransposingOrientation and <= LastTransposingOrientation
            ? (pixelHeight, pixelWidth)
            : (pixelWidth, pixelHeight);

    /// <summary>Reduces a requested edge to a whole number of pixels that a decoder can act on.</summary>
    /// <param name="requested">The requested edge, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The edge in whole pixels, never below one, or <see langword="null"/> when nothing was requested.</returns>
    private static int? ToWholePixels(float? requested)
    {
        if (requested is not { } value)
        {
            return null;
        }

        return value >= int.MaxValue ? int.MaxValue : Math.Max(1, (int)value);
    }

    /// <summary>Derives the edge that was not requested from the one that binds, keeping the source proportions.</summary>
    /// <param name="sourceOppositeEdge">The source edge being derived, in pixels.</param>
    /// <param name="boundEdge">The requested edge that binds the result, in pixels.</param>
    /// <param name="sourceBoundEdge">The source edge matching <paramref name="boundEdge"/>, in pixels.</param>
    /// <returns>The derived edge, in whole pixels and never below one.</returns>
    private static int DeriveOppositeEdge(int sourceOppositeEdge, int boundEdge, int sourceBoundEdge)
    {
        // Rounding down keeps the result inside the requested box rather than a fraction of a pixel outside it.
        var scaled = (long)sourceOppositeEdge * boundEdge / sourceBoundEdge;
        return (int)Math.Min(int.MaxValue, Math.Max(1, scaled));
    }
}
