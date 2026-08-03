// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for the size a requested width and height decode to.</summary>
public sealed class DecodeSizeTests
{
    /// <summary>The width of the image the requests are made against.</summary>
    private const int SourceWidth = 400;

    /// <summary>The height of the image the requests are made against.</summary>
    private const int SourceHeight = 200;

    /// <summary>The size of the image the requests are made against.</summary>
    private static readonly SKSizeI _source = new(SourceWidth, SourceHeight);

    /// <summary>Verifies that an unconstrained request keeps the image's own size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ChooseTargetSize_Unconstrained_KeepsTheSourceSize() =>
        await Assert.That(SkiaBitmapLoader.ChooseTargetSize(_source, null, null)).IsEqualTo(_source);

    /// <summary>Verifies that the dimension the caller left out is derived from the image's proportions.</summary>
    /// <param name="desiredWidth">The requested width, or a negative number for no constraint.</param>
    /// <param name="desiredHeight">The requested height, or a negative number for no constraint.</param>
    /// <param name="expectedWidth">The width the request should produce.</param>
    /// <param name="expectedHeight">The height the request should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(100F, -1F, 100, 50)]
    [Arguments(-1F, 50F, 100, 50)]
    [Arguments(300F, -1F, 300, 150)]
    [Arguments(-1F, 150F, 300, 150)]
    public async Task ChooseTargetSize_OneDimension_DerivesTheOther(float desiredWidth, float desiredHeight, int expectedWidth, int expectedHeight)
    {
        var target = SkiaBitmapLoader.ChooseTargetSize(_source, Requested(desiredWidth), Requested(desiredHeight));

        using (Assert.Multiple())
        {
            await Assert.That(target.Width).IsEqualTo(expectedWidth);
            await Assert.That(target.Height).IsEqualTo(expectedHeight);
        }
    }

    /// <summary>Verifies that asking for both dimensions fits the image inside that box rather than stretching it.</summary>
    /// <param name="desiredWidth">The requested width.</param>
    /// <param name="desiredHeight">The requested height.</param>
    /// <param name="expectedWidth">The width the request should produce.</param>
    /// <param name="expectedHeight">The height the request should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(100F, 100F, 100, 50)]
    [Arguments(1000F, 50F, 100, 50)]
    [Arguments(100F, 50F, 100, 50)]
    [Arguments(800F, 800F, 800, 400)]
    public async Task ChooseTargetSize_BothDimensions_FitsInsideTheBox(float desiredWidth, float desiredHeight, int expectedWidth, int expectedHeight)
    {
        var target = SkiaBitmapLoader.ChooseTargetSize(_source, desiredWidth, desiredHeight);

        using (Assert.Multiple())
        {
            await Assert.That(target.Width).IsEqualTo(expectedWidth);
            await Assert.That(target.Height).IsEqualTo(expectedHeight);
        }
    }

    /// <summary>Verifies that a request that would round away to nothing still produces a pixel.</summary>
    /// <param name="desiredWidth">The requested width.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(0.4F)]
    [Arguments(0F)]
    [Arguments(-5F)]
    [Arguments(1F)]
    public async Task ChooseTargetSize_BelowOnePixel_ClampsToOnePixel(float desiredWidth)
    {
        var target = SkiaBitmapLoader.ChooseTargetSize(_source, desiredWidth, null);

        using (Assert.Multiple())
        {
            await Assert.That(target.Width).IsEqualTo(1);
            await Assert.That(target.Height).IsEqualTo(1);
        }
    }

    /// <summary>Verifies that a proportion that does not divide evenly rounds to the nearest pixel.</summary>
    /// <param name="sourceWidth">The width of the image the request is made against.</param>
    /// <param name="sourceHeight">The height of the image the request is made against.</param>
    /// <param name="desiredWidth">The requested width.</param>
    /// <param name="expectedHeight">The height the request should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(3, 7, 2F, 5)]
    [Arguments(7, 3, 2F, 1)]
    public async Task ChooseTargetSize_UnevenProportions_RoundsToTheNearestPixel(int sourceWidth, int sourceHeight, float desiredWidth, int expectedHeight)
    {
        var target = SkiaBitmapLoader.ChooseTargetSize(new(sourceWidth, sourceHeight), desiredWidth, null);

        await Assert.That(target.Height).IsEqualTo(expectedHeight);
    }

    /// <summary>Turns a negative test argument into the absence of a constraint.</summary>
    /// <param name="value">The value from the test case.</param>
    /// <returns>The requested dimension, or <see langword="null"/> when the case asked for none.</returns>
    private static float? Requested(float value) => value < 0 ? null : value;
}
