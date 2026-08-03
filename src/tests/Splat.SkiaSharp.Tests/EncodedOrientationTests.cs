// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for turning encoded pixels the right way up.</summary>
public sealed class EncodedOrientationTests
{
    /// <summary>The width of the bitmap the orientations are applied to.</summary>
    private const int SourceWidth = 4;

    /// <summary>The height of the bitmap the orientations are applied to.</summary>
    private const int SourceHeight = 2;

    /// <summary>The sampling used when redrawing, chosen so a pixel keeps its exact colour.</summary>
    private static readonly SKSamplingOptions _nearest = new(SKFilterMode.Nearest);

    /// <summary>Verifies which orientations exchange the width and the height.</summary>
    /// <param name="origin">The orientation to test.</param>
    /// <param name="expected">Whether the orientation turns the image on its side.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(SKEncodedOrigin.TopLeft, false)]
    [Arguments(SKEncodedOrigin.TopRight, false)]
    [Arguments(SKEncodedOrigin.BottomRight, false)]
    [Arguments(SKEncodedOrigin.BottomLeft, false)]
    [Arguments(SKEncodedOrigin.LeftTop, true)]
    [Arguments(SKEncodedOrigin.RightTop, true)]
    [Arguments(SKEncodedOrigin.RightBottom, true)]
    [Arguments(SKEncodedOrigin.LeftBottom, true)]
    public async Task SwapsDimensions_ReportsTheSidewaysOrientations(SKEncodedOrigin origin, bool expected) =>
        await Assert.That(SkiaBitmapLoader.SwapsDimensions(origin)).IsEqualTo(expected);

    /// <summary>Verifies that an image already the right way up is left untouched.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ApplyEncodedOrigin_TopLeft_ReturnsTheSameBitmap()
    {
        using var source = TestImages.CreateCornerMarked(SourceWidth, SourceHeight);

        var upright = SkiaBitmapLoader.ApplyEncodedOrigin(source, SKEncodedOrigin.TopLeft, _nearest);

        await Assert.That(upright).IsSameReferenceAs(source);
    }

    /// <summary>Verifies that an upright orientation asks for no transform at all.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CreateOriginMatrix_TopLeft_IsTheIdentity()
    {
        var matrix = SkiaBitmapLoader.CreateOriginMatrix(SKEncodedOrigin.TopLeft, SourceWidth, SourceHeight);

        await Assert.That(matrix).IsEqualTo(SKMatrix.CreateIdentity());
    }

    /// <summary>Verifies that each orientation moves the marked corner where the standard says it belongs.</summary>
    /// <remarks>
    /// The four rotations and their mirror images each send the encoded top left corner to a different
    /// corner of the upright image, so the marker's position identifies the transform on its own.
    /// </remarks>
    /// <param name="origin">The orientation to apply.</param>
    /// <param name="expectedWidth">The width the upright image should have.</param>
    /// <param name="expectedHeight">The height the upright image should have.</param>
    /// <param name="expectedX">The column the marked corner should end up in.</param>
    /// <param name="expectedY">The row the marked corner should end up in.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(SKEncodedOrigin.TopLeft, SourceWidth, SourceHeight, 0, 0)]
    [Arguments(SKEncodedOrigin.TopRight, SourceWidth, SourceHeight, 3, 0)]
    [Arguments(SKEncodedOrigin.BottomRight, SourceWidth, SourceHeight, 3, 1)]
    [Arguments(SKEncodedOrigin.BottomLeft, SourceWidth, SourceHeight, 0, 1)]
    [Arguments(SKEncodedOrigin.LeftTop, SourceHeight, SourceWidth, 0, 0)]
    [Arguments(SKEncodedOrigin.RightTop, SourceHeight, SourceWidth, 1, 0)]
    [Arguments(SKEncodedOrigin.RightBottom, SourceHeight, SourceWidth, 1, 3)]
    [Arguments(SKEncodedOrigin.LeftBottom, SourceHeight, SourceWidth, 0, 3)]
    public async Task ApplyEncodedOrigin_MovesTheMarkedCorner(SKEncodedOrigin origin, int expectedWidth, int expectedHeight, int expectedX, int expectedY)
    {
        using var upright = SkiaBitmapLoader.ApplyEncodedOrigin(TestImages.CreateCornerMarked(SourceWidth, SourceHeight), origin, _nearest);

        using (Assert.Multiple())
        {
            await Assert.That(upright.Width).IsEqualTo(expectedWidth);
            await Assert.That(upright.Height).IsEqualTo(expectedHeight);
            await Assert.That(TestImages.FindMarker(upright)).IsEqualTo((expectedX, expectedY));
        }
    }
}
