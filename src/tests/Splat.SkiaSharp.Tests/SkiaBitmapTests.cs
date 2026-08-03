// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for <see cref="SkiaBitmap"/>.</summary>
public sealed class SkiaBitmapTests
{
    /// <summary>The width of the bitmaps the tests encode.</summary>
    private const int BitmapWidth = 20;

    /// <summary>The height of the bitmaps the tests encode.</summary>
    private const int BitmapHeight = 10;

    /// <summary>Verifies that the wrapper reports the dimensions of the bitmap it holds.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dimensions_ComeFromTheWrappedBitmap()
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));

        using (Assert.Multiple())
        {
            await Assert.That(bitmap.Width).IsEqualTo((float)BitmapWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)BitmapHeight);
        }
    }

    /// <summary>Verifies that a disposed bitmap reports no size and hands out nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_ReleasesTheWrappedBitmap()
    {
        var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));

        bitmap.Dispose();
        bitmap.Dispose();

        using (Assert.Multiple())
        {
            await Assert.That(bitmap.Width).IsEqualTo(0F);
            await Assert.That(bitmap.Height).IsEqualTo(0F);
            await Assert.That(bitmap.Inner).IsNull();
        }
    }

    /// <summary>Verifies that a bitmap has to be supplied.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithoutABitmap_Throws() =>
        await Assert.That(static () => new SkiaBitmap(null!)).Throws<ArgumentNullException>();

    /// <summary>Verifies that a bitmap with no recorded orientation reports the upright one.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task EncodedOrigin_DefaultsToUpright()
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));

        await Assert.That(bitmap.EncodedOrigin).IsEqualTo(SKEncodedOrigin.TopLeft);
    }

    /// <summary>Verifies that the orientation the source recorded is kept.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task EncodedOrigin_KeepsWhatTheSourceRecorded()
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight), SKEncodedOrigin.RightTop);

        await Assert.That(bitmap.EncodedOrigin).IsEqualTo(SKEncodedOrigin.RightTop);
    }

    /// <summary>Verifies that the two <see cref="CompressedBitmapFormat"/> names produce readable images.</summary>
    /// <param name="format">The format to save in.</param>
    /// <param name="quality">The quality factor to save with.</param>
    /// <param name="expected">The encoder the saved bytes should identify as.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(CompressedBitmapFormat.Png, 0.8F, SKEncodedImageFormat.Png)]
    [Arguments(CompressedBitmapFormat.Jpeg, 0.8F, SKEncodedImageFormat.Jpeg)]
    public async Task Save_WritesTheRequestedCompressedFormat(CompressedBitmapFormat format, float quality, SKEncodedImageFormat expected)
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));
        await using var target = new MemoryStream();

        await bitmap.Save(format, quality, target);

        await AssertRoundTrips(target, expected);
    }

    /// <summary>Verifies that the additive overload reaches formats the shared enumeration has no name for.</summary>
    /// <param name="format">The encoder to save with.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(SKEncodedImageFormat.Webp)]
    [Arguments(SKEncodedImageFormat.Png)]
    [Arguments(SKEncodedImageFormat.Jpeg)]
    public async Task Save_WritesAnyFormatTheNativeLibraryCanEncode(SKEncodedImageFormat format)
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));
        await using var target = new MemoryStream();

        await bitmap.Save(format, 1F, target);

        await AssertRoundTrips(target, format);
    }

    /// <summary>Verifies that a quality outside the interface's range is brought back into it.</summary>
    /// <param name="quality">The quality factor to save with.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(-1F)]
    [Arguments(0F)]
    [Arguments(0.5F)]
    [Arguments(1F)]
    [Arguments(5F)]
    public async Task Save_WithAQualityOutsideTheRange_StillEncodes(float quality)
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));
        await using var target = new MemoryStream();

        await bitmap.Save(CompressedBitmapFormat.Jpeg, quality, target);

        await AssertRoundTrips(target, SKEncodedImageFormat.Jpeg);
    }

    /// <summary>Verifies that a format the native library only reads is reported rather than written empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Save_WithoutAnEncoder_Throws()
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));
        await using var target = new MemoryStream();

        await Assert.That(async () => await bitmap.Save(SKEncodedImageFormat.Astc, 1F, target)).Throws<BitmapLoaderException>();
    }

    /// <summary>Verifies that a target stream has to be supplied.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Save_WithoutATarget_Throws()
    {
        using var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));

        await Assert.That(async () => await bitmap.Save(CompressedBitmapFormat.Png, 1F, null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that saving a released bitmap says so rather than writing nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Save_AfterDispose_Throws()
    {
        var bitmap = new SkiaBitmap(TestImages.CreateCornerMarked(BitmapWidth, BitmapHeight));
        bitmap.Dispose();

        await using var target = new MemoryStream();

        await Assert.That(async () => await bitmap.Save(CompressedBitmapFormat.Png, 1F, target)).Throws<ObjectDisposedException>();
    }

    /// <summary>Asserts that the written bytes decode back to the bitmap that was saved.</summary>
    /// <param name="target">The stream the image was written to.</param>
    /// <param name="expected">The encoder the bytes should identify as.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertRoundTrips(MemoryStream target, SKEncodedImageFormat expected)
    {
        using var data = SKData.CreateCopy(target.GetBuffer().AsSpan(0, (int)target.Length));
        using var codec = SKCodec.Create(data);

        using (Assert.Multiple())
        {
            await Assert.That(codec.EncodedFormat).IsEqualTo(expected);
            await Assert.That(codec.Info.Width).IsEqualTo(BitmapWidth);
            await Assert.That(codec.Info.Height).IsEqualTo(BitmapHeight);
        }
    }
}
