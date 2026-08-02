// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using SkiaSharp;

namespace Splat.SkiaSharp.Tests;

/// <summary>Unit tests for <see cref="SkiaBitmapLoader"/>.</summary>
public sealed class SkiaBitmapLoaderTests
{
    /// <summary>The width of the images the tests decode.</summary>
    private const int SourceWidth = 400;

    /// <summary>The height of the images the tests decode.</summary>
    private const int SourceHeight = 200;

    /// <summary>How far a decoded image's proportions may drift from the source's.</summary>
    private const float AspectTolerance = 0.01F;

    /// <summary>Verifies that a decode with no requested size keeps the image's own dimensions.</summary>
    /// <param name="format">The encoder the fixture is written with.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(SKEncodedImageFormat.Png)]
    [Arguments(SKEncodedImageFormat.Jpeg)]
    [Arguments(SKEncodedImageFormat.Webp)]
    public async Task Load_WithoutADesiredSize_KeepsTheImageSize(SKEncodedImageFormat format)
    {
        var loader = new SkiaBitmapLoader();
        await using var source = TestImages.OpenStream(SourceWidth, SourceHeight, format);

        using var bitmap = await loader.Load(source, null, null);

        using (Assert.Multiple())
        {
            await Assert.That(bitmap!.Width).IsEqualTo((float)SourceWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)SourceHeight);
        }
    }

    /// <summary>Verifies that a requested size is honoured without distorting the image.</summary>
    /// <remarks>
    /// A PNG has to be decoded whole and resized, while a JPEG can be decoded straight to a smaller
    /// size, so both encoders are exercised through the same expectations.
    /// </remarks>
    /// <param name="format">The encoder the fixture is written with.</param>
    /// <param name="desiredWidth">The requested width, or a negative number for no constraint.</param>
    /// <param name="desiredHeight">The requested height, or a negative number for no constraint.</param>
    /// <param name="expectedWidth">The width the decode should produce.</param>
    /// <param name="expectedHeight">The height the decode should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(SKEncodedImageFormat.Png, 100F, 100F, 100, 50)]
    [Arguments(SKEncodedImageFormat.Png, 100F, -1F, 100, 50)]
    [Arguments(SKEncodedImageFormat.Png, -1F, 50F, 100, 50)]
    [Arguments(SKEncodedImageFormat.Png, 1000F, 50F, 100, 50)]
    [Arguments(SKEncodedImageFormat.Png, 800F, 800F, 800, 400)]
    [Arguments(SKEncodedImageFormat.Jpeg, 100F, 100F, 100, 50)]
    [Arguments(SKEncodedImageFormat.Jpeg, 200F, -1F, 200, 100)]
    [Arguments(SKEncodedImageFormat.Jpeg, -1F, 25F, 50, 25)]
    [Arguments(SKEncodedImageFormat.Webp, 60F, 60F, 60, 30)]
    public async Task Load_WithADesiredSize_KeepsTheAspectRatio(
        SKEncodedImageFormat format,
        float desiredWidth,
        float desiredHeight,
        int expectedWidth,
        int expectedHeight)
    {
        var loader = new SkiaBitmapLoader();
        await using var source = TestImages.OpenStream(SourceWidth, SourceHeight, format);

        using var bitmap = await loader.Load(source, Requested(desiredWidth), Requested(desiredHeight));

        using (Assert.Multiple())
        {
            await Assert.That(bitmap!.Width).IsEqualTo((float)expectedWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)expectedHeight);
            await Assert.That(bitmap.Width / bitmap.Height).IsEqualTo((float)SourceWidth / SourceHeight).Within(AspectTolerance);
        }
    }

    /// <summary>Verifies that an image stored on its side comes back the right way up.</summary>
    /// <remarks>
    /// A camera records the orientation rather than rotating the pixels, so the decoded size is the
    /// encoded one with its axes exchanged, and a requested size describes the upright image.
    /// </remarks>
    /// <param name="orientation">The orientation to record, numbered as the metadata standard numbers them.</param>
    /// <param name="expectedOrigin">The orientation the decoded bitmap should report.</param>
    /// <param name="desiredWidth">The requested width, or a negative number for no constraint.</param>
    /// <param name="expectedWidth">The width the decode should produce.</param>
    /// <param name="expectedHeight">The height the decode should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(1, SKEncodedOrigin.TopLeft, -1F, 400, 200)]
    [Arguments(3, SKEncodedOrigin.BottomRight, -1F, 400, 200)]
    [Arguments(6, SKEncodedOrigin.RightTop, -1F, 200, 400)]
    [Arguments(6, SKEncodedOrigin.RightTop, 100F, 100, 200)]
    [Arguments(8, SKEncodedOrigin.LeftBottom, 50F, 50, 100)]
    public async Task Load_WithARecordedOrientation_TurnsTheImageUpright(
        int orientation,
        SKEncodedOrigin expectedOrigin,
        float desiredWidth,
        int expectedWidth,
        int expectedHeight)
    {
        var loader = new SkiaBitmapLoader();
        await using var source = new MemoryStream(TestImages.EncodeWithOrientation(SourceWidth, SourceHeight, orientation));

        using var bitmap = await loader.Load(source, Requested(desiredWidth), null);

        using (Assert.Multiple())
        {
            await Assert.That(bitmap!.Width).IsEqualTo((float)expectedWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)expectedHeight);
            await Assert.That(((SkiaBitmap)bitmap).EncodedOrigin).IsEqualTo(expectedOrigin);
        }
    }

    /// <summary>Verifies that a stream holding no image reports nothing rather than failing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Load_WithoutAnImage_ReturnsNothing()
    {
        var loader = new SkiaBitmapLoader();
        await using var source = new MemoryStream("this is not an image"u8.ToArray());

        var bitmap = await loader.Load(source, null, null);

        await Assert.That(bitmap).IsNull();
    }

    /// <summary>Verifies that the loader rejects a missing stream.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Load_WithoutAStream_Throws()
    {
        var loader = new SkiaBitmapLoader();

        await Assert.That(async () => await loader.Load(null!, null, null)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies that the sampling the loader was built with is the one it reports.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Sampling_ReportsTheChosenResampler()
    {
        var chosen = new SKSamplingOptions(SKFilterMode.Nearest);
        var fallback = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);

        using (Assert.Multiple())
        {
            await Assert.That(new SkiaBitmapLoader(chosen).Sampling).IsEqualTo(chosen);
            await Assert.That(new SkiaBitmapLoader().Sampling).IsEqualTo(fallback);
        }
    }

    /// <summary>Verifies that a resource path relative to the application directory is resolved.</summary>
    /// <param name="desiredWidth">The requested width.</param>
    /// <param name="expectedWidth">The width the decode should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(100F, 100)]
    public async Task LoadFromResource_WithARelativePath_ResolvesAgainstTheApplicationDirectory(float desiredWidth, int expectedWidth)
    {
        var name = $"{Guid.NewGuid():N}.png";
        var path = Path.Combine(AppContext.BaseDirectory, name);
        await File.WriteAllBytesAsync(path, TestImages.Encode(SourceWidth, SourceHeight, SKEncodedImageFormat.Png));

        try
        {
            var loader = new SkiaBitmapLoader();

            using var bitmap = await loader.LoadFromResource(name, desiredWidth, null);

            await Assert.That(bitmap!.Width).IsEqualTo((float)expectedWidth);
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>Verifies that a resource named by an absolute path is read from that path.</summary>
    /// <param name="desiredHeight">The requested height.</param>
    /// <param name="expectedWidth">The width the decode should produce.</param>
    /// <param name="expectedHeight">The height the decode should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(50F, 100, 50)]
    public async Task LoadFromResource_WithAnAbsolutePath_ReadsThatFile(float desiredHeight, int expectedWidth, int expectedHeight)
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        await File.WriteAllBytesAsync(path, TestImages.Encode(SourceWidth, SourceHeight, SKEncodedImageFormat.Png));

        try
        {
            var loader = new SkiaBitmapLoader();

            using var bitmap = await loader.LoadFromResource(path, null, desiredHeight);

            using (Assert.Multiple())
            {
                await Assert.That(bitmap!.Width).IsEqualTo((float)expectedWidth);
                await Assert.That(bitmap.Height).IsEqualTo((float)expectedHeight);
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>Verifies that the loader rejects a resource name that identifies nothing.</summary>
    /// <param name="source">The resource name to reject.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task LoadFromResource_WithoutAName_Throws(string? source)
    {
        var loader = new SkiaBitmapLoader();

        await Assert.That(async () => await loader.LoadFromResource(source!, null, null)).Throws<ArgumentException>();
    }

    /// <summary>Verifies that an empty canvas is created at the requested size.</summary>
    /// <param name="width">The width to ask for.</param>
    /// <param name="height">The height to ask for.</param>
    /// <param name="expectedWidth">The width the canvas should have.</param>
    /// <param name="expectedHeight">The height the canvas should have.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(64F, 32F, 64, 32)]
    [Arguments(0.5F, 0.5F, 1, 1)]
    public async Task Create_ProducesACanvasOfTheRequestedSize(float width, float height, int expectedWidth, int expectedHeight)
    {
        var loader = new SkiaBitmapLoader();

        using var bitmap = loader.Create(width, height);

        using (Assert.Multiple())
        {
            await Assert.That(bitmap.Width).IsEqualTo((float)expectedWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)expectedHeight);
        }
    }

    /// <summary>Verifies that a canvas with no area is rejected rather than silently produced.</summary>
    /// <param name="width">The width to ask for.</param>
    /// <param name="height">The height to ask for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(0F, 10F)]
    [Arguments(10F, 0F)]
    [Arguments(-1F, 10F)]
    [Arguments(10F, -1F)]
    public async Task Create_WithoutAnArea_Throws(float width, float height)
    {
        var loader = new SkiaBitmapLoader();

        await Assert.That(() => loader.Create(width, height)).Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>Verifies that a bitmap decoded from a stream reaches call sites through the static accessor.</summary>
    /// <param name="desiredWidth">The requested width.</param>
    /// <param name="expectedWidth">The width the decode should produce.</param>
    /// <param name="expectedHeight">The height the decode should produce.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(40F, 40, 20)]
    [NotInParallel] // Mutates the global BitmapLoader.Current static state.
    public async Task BitmapLoaderCurrent_OnceRegistered_DecodesThroughThisLoader(float desiredWidth, int expectedWidth, int expectedHeight)
    {
        BitmapLoader.Current = new SkiaBitmapLoader();
        await using var source = TestImages.OpenStream(SourceWidth, SourceHeight, SKEncodedImageFormat.Png);

        using var bitmap = await BitmapLoader.Current.Load(source, desiredWidth, null);

        using (Assert.Multiple())
        {
            await Assert.That(bitmap!.Width).IsEqualTo((float)expectedWidth);
            await Assert.That(bitmap.Height).IsEqualTo((float)expectedHeight);
        }
    }

    /// <summary>Turns a negative test argument into the absence of a constraint.</summary>
    /// <param name="value">The value from the test case.</param>
    /// <returns>The requested dimension, or <see langword="null"/> when the case asked for none.</returns>
    private static float? Requested(float value) => value < 0 ? null : value;
}
