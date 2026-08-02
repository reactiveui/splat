// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>Verifies the dimensions worked out for a caller who asks for a bitmap at a particular size.</summary>
/// <remarks>
/// The platform loaders each hand a decoder a different kind of instruction - a subsampling factor here, a bound on
/// the longer edge there - but all of them derive it from this arithmetic, so pinning it down here pins down the
/// behaviour on platforms whose decoders cannot be run in a test.
/// </remarks>
public sealed class BitmapDecodeSizeCalculationTests
{
    /// <summary>The width, in pixels, of the landscape source image the calculations are made against.</summary>
    private const int LandscapeWidth = 4000;

    /// <summary>The height, in pixels, of the landscape source image the calculations are made against.</summary>
    private const int LandscapeHeight = 2000;

    /// <summary>The edge, in pixels, of the square box the image is asked to fit inside.</summary>
    private const int BoxEdge = 200;

    /// <summary>Half the box edge; the landscape source fitted to <see cref="BoxEdge"/> lands on this height.</summary>
    private const int HalfBoxEdge = 100;

    /// <summary>A width that matches the landscape source proportions when paired with <see cref="BoxEdge"/>.</summary>
    private const int AspectMatchingWidth = 400;

    /// <summary>A request smaller than a single pixel, used to check the lower clamp.</summary>
    private const float SubPixelWidth = 0.4F;

    /// <summary>The smallest image a decoder can be asked for.</summary>
    private const int SinglePixel = 1;

    /// <summary>A requested height taller than the box is wide, so the height is what stops the subsampling.</summary>
    private const int TallBoxHeight = 1000;

    /// <summary>The subsampling factor for a box twenty times smaller than the source in both directions.</summary>
    private const int TwentyFoldSampleSize = 16;

    /// <summary>The subsampling factor for a request whose height leaves only one step of headroom.</summary>
    private const int SingleStepSampleSize = 2;

    /// <summary>The subsampling factor that keeps every pixel the decoder reads.</summary>
    private const int NoSubsampling = 1;

    /// <summary>An orientation code for pixels stored the way they are meant to be shown.</summary>
    private const int UprightOrientation = 1;

    /// <summary>An orientation code for pixels stored a quarter turn away from how they are meant to be shown.</summary>
    private const int QuarterTurnedOrientation = 6;

    /// <summary>An orientation code past the ones a container can record.</summary>
    private const int OrientationPastTheKnownCodes = 9;

    /// <summary>A degenerate source dimension, as a decoder reports when it cannot read a header.</summary>
    private const int Unmeasurable = 0;

    /// <summary>
    /// The height derived for a landscape source whose requested width was capped at the largest addressable one;
    /// the source is twice as wide as it is tall, so the height comes out at half the cap.
    /// </summary>
    private const int HalfTheLargestPixelCount = int.MaxValue / 2;

    /// <summary>Verifies an unconstrained request leaves the image at its source size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NeitherDimensionRequested_LeavesTheSourceSize()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, null, null);

        await Assert.That(fitted).IsNull();
    }

    /// <summary>Verifies a source whose header could not be read is left at its source size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnmeasurableSource_LeavesTheSourceSize()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(Unmeasurable, Unmeasurable, BoxEdge, BoxEdge);

        await Assert.That(fitted).IsNull();
    }

    /// <summary>Verifies a width-only request derives the height from the source proportions.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WidthOnly_DerivesTheHeight()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, BoxEdge, null);

        await Assert.That(fitted).IsEqualTo((BoxEdge, HalfBoxEdge));
    }

    /// <summary>Verifies a height-only request derives the width from the source proportions.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HeightOnly_DerivesTheWidth()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, null, BoxEdge);

        await Assert.That(fitted).IsEqualTo((AspectMatchingWidth, BoxEdge));
    }

    /// <summary>Verifies a source wider than the requested box is bound by its width, not stretched to both edges.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SourceWiderThanTheBox_IsBoundByWidth()
    {
        // 4000x2000 into a 200x200 box: the width scales by 0.05 and the height by 0.1, so the width binds and the
        // derived height lands on 100, inside the box.
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, BoxEdge, BoxEdge);

        await Assert.That(fitted).IsEqualTo((BoxEdge, HalfBoxEdge));
    }

    /// <summary>Verifies a source taller than the requested box is bound by its height.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SourceTallerThanTheBox_IsBoundByHeight()
    {
        // The portrait source is the landscape one rotated, so the height binds instead.
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeHeight, LandscapeWidth, BoxEdge, BoxEdge);

        await Assert.That(fitted).IsEqualTo((HalfBoxEdge, BoxEdge));
    }

    /// <summary>Verifies a request matching the source proportions reaches both requested edges exactly.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RequestMatchingTheSourceProportions_ReachesBothEdges()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, AspectMatchingWidth, BoxEdge);

        await Assert.That(fitted).IsEqualTo((AspectMatchingWidth, BoxEdge));
    }

    /// <summary>Verifies a request smaller than a pixel still leaves an image a decoder can produce.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RequestSmallerThanAPixel_LeavesASinglePixel()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, SubPixelWidth, null);

        await Assert.That(fitted).IsEqualTo((SinglePixel, SinglePixel));
    }

    /// <summary>Verifies a request larger than any addressable image is capped rather than wrapping round.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RequestLargerThanAnyAddressableImage_IsCapped()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeWidth, LandscapeHeight, float.MaxValue, null);

        await Assert.That(fitted).IsEqualTo((int.MaxValue, HalfTheLargestPixelCount));
    }

    /// <summary>Verifies a derived edge larger than any addressable image is capped as well.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DerivedEdgeLargerThanAnyAddressableImage_IsCapped()
    {
        var fitted = BitmapDecodeSize.ChooseFittedSize(LandscapeHeight, LandscapeWidth, float.MaxValue, null);

        await Assert.That(fitted).IsEqualTo((int.MaxValue, int.MaxValue));
    }

    /// <summary>Verifies a source whose header could not be read is decoded without subsampling.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubsamplingAnUnmeasurableSource_KeepsEveryPixel()
    {
        var sampleSize = BitmapDecodeSize.ChooseSampleSize(Unmeasurable, Unmeasurable, BoxEdge, HalfBoxEdge);

        await Assert.That(sampleSize).IsEqualTo(NoSubsampling);
    }

    /// <summary>Verifies an empty target size is decoded without subsampling rather than sampled without end.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubsamplingToAnEmptyTarget_KeepsEveryPixel()
    {
        var sampleSize = BitmapDecodeSize.ChooseSampleSize(LandscapeWidth, LandscapeHeight, Unmeasurable, Unmeasurable);

        await Assert.That(sampleSize).IsEqualTo(NoSubsampling);
    }

    /// <summary>Verifies a target far below the source is reached by repeated halving, stopping before it undershoots.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubsamplingToAMuchSmallerTarget_HalvesUntilAFurtherStepWouldUndershoot()
    {
        // Halving 4000x2000 five times would leave 125x62, below the requested 200x100, so it stops at four.
        var sampleSize = BitmapDecodeSize.ChooseSampleSize(LandscapeWidth, LandscapeHeight, BoxEdge, HalfBoxEdge);

        await Assert.That(sampleSize).IsEqualTo(TwentyFoldSampleSize);
    }

    /// <summary>Verifies the height stops the halving when it runs out of room before the width does.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubsamplingToATargetBoundByHeight_StopsOnTheHeight()
    {
        var sampleSize = BitmapDecodeSize.ChooseSampleSize(LandscapeWidth, LandscapeHeight, HalfBoxEdge, TallBoxHeight);

        await Assert.That(sampleSize).IsEqualTo(SingleStepSampleSize);
    }

    /// <summary>Verifies a target at the source size is decoded without subsampling.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubsamplingToTheSourceSize_KeepsEveryPixel()
    {
        var sampleSize = BitmapDecodeSize.ChooseSampleSize(LandscapeWidth, LandscapeHeight, LandscapeWidth, LandscapeHeight);

        await Assert.That(sampleSize).IsEqualTo(NoSubsampling);
    }

    /// <summary>Verifies an unconstrained request leaves a thumbnail decoder unbounded.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThumbnailWithNoRequestedSize_IsUnbounded()
    {
        var maxPixelSize = BitmapDecodeSize.ChooseThumbnailPixelSize(LandscapeWidth, LandscapeHeight, null, null);

        await Assert.That(maxPixelSize).IsNull();
    }

    /// <summary>Verifies a thumbnail of a landscape source is bounded by the fitted width.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThumbnailOfALandscapeSource_IsBoundedByTheFittedWidth()
    {
        var maxPixelSize = BitmapDecodeSize.ChooseThumbnailPixelSize(LandscapeWidth, LandscapeHeight, BoxEdge, BoxEdge);

        await Assert.That(maxPixelSize).IsEqualTo(BoxEdge);
    }

    /// <summary>Verifies a thumbnail of a portrait source is bounded by the fitted height.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ThumbnailOfAPortraitSource_IsBoundedByTheFittedHeight()
    {
        var maxPixelSize = BitmapDecodeSize.ChooseThumbnailPixelSize(LandscapeHeight, LandscapeWidth, BoxEdge, BoxEdge);

        await Assert.That(maxPixelSize).IsEqualTo(BoxEdge);
    }

    /// <summary>Verifies pixels stored the way they are shown keep their dimensions.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UprightPixels_KeepTheirDimensions()
    {
        var oriented = BitmapDecodeSize.OrientedPixelSize(LandscapeWidth, LandscapeHeight, UprightOrientation);

        await Assert.That(oriented).IsEqualTo((LandscapeWidth, LandscapeHeight));
    }

    /// <summary>Verifies pixels stored a quarter turn away are measured the way they will be shown.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task QuarterTurnedPixels_AreTransposed()
    {
        var oriented = BitmapDecodeSize.OrientedPixelSize(LandscapeWidth, LandscapeHeight, QuarterTurnedOrientation);

        await Assert.That(oriented).IsEqualTo((LandscapeHeight, LandscapeWidth));
    }

    /// <summary>Verifies an orientation nobody records leaves the stored dimensions alone.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PixelsWithAnUnrecognisedOrientation_KeepTheirDimensions()
    {
        var oriented = BitmapDecodeSize.OrientedPixelSize(LandscapeWidth, LandscapeHeight, OrientationPastTheKnownCodes);

        await Assert.That(oriented).IsEqualTo((LandscapeWidth, LandscapeHeight));
    }
}
