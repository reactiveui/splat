// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Diagnostics.CodeAnalysis;
#if !IS_SHARED_NET
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace Splat.Tests;

/// <summary>Verifies the decode dimensions chosen when a caller asks for a bitmap at a particular size.</summary>
/// <remarks>
/// The imaging layer keeps the aspect ratio only when exactly one decode dimension is set, so a request carrying both
/// has to be reduced to the single dimension that constrains the result. These tests pin that reduction.
/// </remarks>
[SuppressMessage(
    "StyleSharp",
    "SST1436:Add members to the type or remove it; an empty type is rarely intentional",
    Justification = "The loader under test only exists where the platform ships a codec, so the members compile away on the shared targets.")]
public sealed class BitmapDecodeSizeTests
{
#if !IS_SHARED_NET
    /// <summary>The width, in pixels, of the landscape source image the tests decode from.</summary>
    private const int LandscapeWidth = 4000;

    /// <summary>The height, in pixels, of the landscape source image the tests decode from.</summary>
    private const int LandscapeHeight = 2000;

    /// <summary>The edge, in pixels, of the square box the tests ask the image to fit inside.</summary>
    private const int BoxEdge = 200;

    /// <summary>Half the box edge, used where the request is deliberately not square.</summary>
    private const int HalfBoxEdge = 100;

    /// <summary>A width that matches the landscape source aspect ratio when paired with <see cref="BoxEdge"/>.</summary>
    private const int AspectMatchingWidth = 400;

    /// <summary>A request smaller than a single pixel, used to check the lower clamp.</summary>
    private const float SubPixelWidth = 0.4F;

    /// <summary>How much of the encoded image to keep when checking a header the codec cannot finish reading.</summary>
    private const int TruncatedImageLength = 40;

    /// <summary>The width of the image synthesized for the end-to-end decode; deliberately not equal to its height.</summary>
    private const int SourceImageWidth = 800;

    /// <summary>The height of the image synthesized for the end-to-end decode.</summary>
    private const int SourceImageHeight = 750;

    /// <summary>The dots-per-inch the synthesized image is authored at.</summary>
    private const double SourceImageDpi = 96;

    /// <summary>How far the decoded aspect ratio may drift, absorbing the rounding to whole pixels.</summary>
    private const float AspectTolerance = 0.02F;

    /// <summary>
    /// How far the decoded size may exceed the requested box, in device-independent units. The container stores
    /// resolution as whole pixels per metre, so the authored dots-per-inch does not round-trip exactly and the
    /// device-independent size lands a fraction above the pixel count that was actually decoded.
    /// </summary>
    private const float BoxFitTolerance = 1F;

    /// <summary>Verifies an unconstrained request decodes at the source size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NeitherDimensionRequested_DecodesAtSourceSize()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), null, null);

        await Assert.That(size).IsEqualTo((0, 0));
    }

    /// <summary>Verifies a width-only request leaves the height to be derived.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WidthOnly_LeavesHeightDerived()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), BoxEdge, null);

        await Assert.That(size).IsEqualTo((BoxEdge, 0));
    }

    /// <summary>Verifies a height-only request leaves the width to be derived.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HeightOnly_LeavesWidthDerived()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), null, BoxEdge);

        await Assert.That(size).IsEqualTo((0, BoxEdge));
    }

    /// <summary>Verifies that a source wider than the requested box is constrained by its width.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SourceWiderThanRequestedBox_IsConstrainedByWidth()
    {
        // 4000x2000 into a 200x200 box: width scales by 0.05 and height by 0.1, so width binds and the
        // derived height lands on 100, inside the box.
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), BoxEdge, BoxEdge);

        await Assert.That(size).IsEqualTo((BoxEdge, 0));
    }

    /// <summary>Verifies that a source taller than the requested box is constrained by its height.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SourceTallerThanRequestedBox_IsConstrainedByHeight()
    {
        // The portrait source is the landscape one rotated, so now the height binds instead.
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeHeight, LandscapeWidth), BoxEdge, BoxEdge);

        await Assert.That(size).IsEqualTo((0, BoxEdge));
    }

    /// <summary>Verifies a request matching the source aspect ratio never sets both dimensions.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RequestMatchingSourceAspectRatio_SetsOnlyOneDimension()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), AspectMatchingWidth, BoxEdge);

        await Assert.That(size).IsEqualTo((AspectMatchingWidth, 0));
    }

    /// <summary>Verifies that an unreadable source falls back to the requested width rather than stretching.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UnknownSourceSize_FallsBackToWidth()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize(null, BoxEdge, HalfBoxEdge);

        await Assert.That(size).IsEqualTo((BoxEdge, 0));
    }

    /// <summary>Verifies a degenerate source size falls back to the requested width rather than dividing by zero.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DegenerateSourceSize_FallsBackToWidth()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((0, 0), BoxEdge, HalfBoxEdge);

        await Assert.That(size).IsEqualTo((BoxEdge, 0));
    }

    /// <summary>Verifies a sub-pixel request still decodes at least one pixel.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task SubPixelRequest_DecodesAtLeastOnePixel()
    {
        var size = PlatformBitmapLoader.ChooseDecodeSize((LandscapeWidth, LandscapeHeight), SubPixelWidth, null);

        await Assert.That(size).IsEqualTo((1, 0));
    }

    /// <summary>Verifies decoding into a box the source cannot fill keeps the source proportions.</summary>
    /// <remarks>
    /// This is the end-to-end counterpart to the arithmetic above: it decodes a real image through the platform
    /// codec, so it fails if the requested dimensions are applied in a way that stretches the result.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingIntoAMismatchedBox_PreservesSourceProportions()
    {
        var loader = new PlatformBitmapLoader();

        await using var scaledStream = CreateSourceImage();
        var scaled = await loader.Load(scaledStream, BoxEdge, BoxEdge);

        await Assert.That(scaled).IsNotNull();

        await Assert.That(scaled!.Width).IsLessThanOrEqualTo(BoxEdge + BoxFitTolerance);
        await Assert.That(scaled.Height).IsLessThanOrEqualTo(BoxEdge + BoxFitTolerance);
        await Assert.That(scaled.Width / scaled.Height)
            .IsEqualTo((float)SourceImageWidth / SourceImageHeight)
            .Within(AspectTolerance);
    }

    /// <summary>Verifies a height-only request derives the width from the source, and reaches the image.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingWithOnlyAHeightRequested_DerivesTheWidth()
    {
        var loader = new PlatformBitmapLoader();

        await using var stream = CreateSourceImage();
        var scaled = await loader.Load(stream, null, BoxEdge);

        await Assert.That(scaled).IsNotNull();
        await Assert.That(scaled!.Height).IsLessThanOrEqualTo(BoxEdge + BoxFitTolerance);
        await Assert.That(scaled.Width / scaled.Height)
            .IsEqualTo((float)SourceImageWidth / SourceImageHeight)
            .Within(AspectTolerance);
    }

    /// <summary>Verifies an unconstrained request decodes the image at its natural size.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingWithNoSizeRequested_UsesTheSourceSize()
    {
        var loader = new PlatformBitmapLoader();

        await using var stream = CreateSourceImage();
        var natural = await loader.Load(stream, null, null);

        await Assert.That(natural).IsNotNull();
        await Assert.That(natural!.Width).IsEqualTo(SourceImageWidth).Within(BoxFitTolerance);
    }

    /// <summary>Verifies the source dimensions are read from a stream and its position restored.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringASeekableStream_ReadsTheSourceSizeAndRewinds()
    {
        await using var stream = CreateSourceImage();

        var size = PlatformBitmapLoader.ReadPixelSize(stream);

        await Assert.That(size).IsEqualTo((SourceImageWidth, SourceImageHeight));
        await Assert.That(stream.Position).IsEqualTo(0L);
    }

    /// <summary>Verifies a stream that cannot be repositioned reports no size rather than consuming it.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringAForwardOnlyStream_ReportsNoSize()
    {
        await using var seekable = CreateSourceImage();
        await using var stream = new ForwardOnlyStream(seekable);

        await Assert.That(PlatformBitmapLoader.ReadPixelSize(stream)).IsNull();
    }

    /// <summary>Verifies content the codec cannot read reports no size rather than throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringContentThatIsNotAnImage_ReportsNoSize()
    {
        await using var stream = new MemoryStream([0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07]);

        await Assert.That(PlatformBitmapLoader.ReadPixelSize(stream)).IsNull();
    }

    /// <summary>Verifies a resource that is not there reports no size rather than throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringAResourceThatDoesNotExist_ReportsNoSize()
    {
        var missing = new Uri(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png"));

        await Assert.That(PlatformBitmapLoader.ReadPixelSize(missing)).IsNull();
    }

    /// <summary>Verifies a resource that is not a file is left unmeasured rather than fetched.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringAResourceThatIsNotAFile_ReportsNoSize() =>
        await Assert.That(PlatformBitmapLoader.ReadPixelSize(new Uri("http://example.invalid/image.png"))).IsNull();

    /// <summary>Verifies a resource identifier with no root is left unmeasured.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringARelativeResource_ReportsNoSize() =>
        await Assert.That(PlatformBitmapLoader.ReadPixelSize(new Uri("image.png", UriKind.Relative))).IsNull();

    /// <summary>Verifies a resource naming something that cannot be opened as a file reports no size.</summary>
    /// <remarks>A directory is addressable as a file resource but cannot be opened as one.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringAResourceThatCannotBeOpened_ReportsNoSize() =>
        await Assert.That(PlatformBitmapLoader.ReadPixelSize(new Uri(Path.GetTempPath()))).IsNull();

    /// <summary>Verifies an image whose header is cut short reports no size rather than throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MeasuringATruncatedImage_ReportsNoSize()
    {
        await using var whole = CreateSourceImage();
        await using var truncated = new MemoryStream(whole.ToArray()[..TruncatedImageLength]);

        await Assert.That(PlatformBitmapLoader.ReadPixelSize(truncated)).IsNull();
    }

    /// <summary>Verifies that content the codec cannot read is reported by the decode rather than swallowed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingContentThatIsNotAnImage_ReportsTheFailure()
    {
        var loader = new PlatformBitmapLoader();

        await using var stream = new MemoryStream([0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07]);

        await Assert.That(async () => await loader.Load(stream, BoxEdge, BoxEdge)).ThrowsException();
    }

    /// <summary>Verifies a resource is decoded into the requested box without distorting it.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingAResourceIntoAMismatchedBox_PreservesSourceProportions()
    {
        var loader = new PlatformBitmapLoader();
        var path = await WriteSourceImageToDisk();

        try
        {
            var scaled = await loader.LoadFromResource(new Uri(path).AbsoluteUri, BoxEdge, BoxEdge);

            await Assert.That(scaled).IsNotNull();
            await Assert.That(scaled!.Width).IsLessThanOrEqualTo(BoxEdge + BoxFitTolerance);
            await Assert.That(scaled.Width / scaled.Height)
                .IsEqualTo((float)SourceImageWidth / SourceImageHeight)
                .Within(AspectTolerance);
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>Verifies a resource that is not an image is reported by the decode rather than swallowed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingAResourceThatIsNotAnImage_ReportsTheFailure()
    {
        var loader = new PlatformBitmapLoader();
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        await File.WriteAllBytesAsync(path, [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07]);

        try
        {
            await Assert.That(async () => await loader.LoadFromResource(new Uri(path).AbsoluteUri, BoxEdge, BoxEdge))
                .ThrowsException();
        }
        finally
        {
            DeleteIfReleased(path);
        }
    }

    /// <summary>Verifies a resource that is not there is reported by the decode rather than swallowed.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DecodingAResourceThatDoesNotExist_ReportsTheFailure()
    {
        var loader = new PlatformBitmapLoader();
        var missing = new Uri(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png")).AbsoluteUri;

        await Assert.That(async () => await loader.LoadFromResource(missing, BoxEdge, BoxEdge)).ThrowsException();
    }

    /// <summary>Removes a temporary file, tolerating a decode that failed while still holding it.</summary>
    /// <remarks>
    /// A decode that throws part way through leaves the source open on some runtimes, and nothing the loader exposes
    /// can close it. The successful cases delete strictly, so a regression in releasing the file is still caught.
    /// </remarks>
    /// <param name="path">The file to remove.</param>
    private static void DeleteIfReleased(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            // The uniquely named temporary is left for the operating system to reclaim.
        }
    }

    /// <summary>Writes the synthesized image to a temporary file so it can be addressed by a resource identifier.</summary>
    /// <returns>The path the image was written to.</returns>
    private static async Task<string> WriteSourceImageToDisk()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        await using var source = CreateSourceImage();
        await File.WriteAllBytesAsync(path, source.ToArray());
        return path;
    }

    /// <summary>Encodes an image of known, non-square dimensions so the decode has real pixels to work from.</summary>
    /// <returns>A seekable <see cref="MemoryStream"/> holding the encoded image.</returns>
    private static MemoryStream CreateSourceImage()
    {
        var source = new WriteableBitmap(
            SourceImageWidth,
            SourceImageHeight,
            SourceImageDpi,
            SourceImageDpi,
            PixelFormats.Pbgra32,
            null);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(source));

        var stream = new MemoryStream();
        encoder.Save(stream);
        stream.Position = 0;

        return stream;
    }

    /// <summary>A read-only stream that reports itself as unable to seek, as a network stream would.</summary>
    /// <param name="inner">The stream supplying the bytes.</param>
    private sealed class ForwardOnlyStream(Stream inner) : Stream
    {
        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void Flush() => inner.Flush();

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void SetLength(long value) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
#endif
}
