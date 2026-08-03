// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using CoreGraphics;

using Foundation;

using ImageIO;

#if UIKIT
using UIKit;
#else
using UIImage = AppKit.NSImage;
#endif

namespace Splat;

/// <summary>Provides platform-specific functionality for loading and creating bitmap images.</summary>
/// <remarks>This class implements the <see cref="IBitmapLoader"/> interface to support loading bitmaps from streams and
/// resources on the current platform. It is intended for internal use by image handling components that require
/// platform abstraction. Thread safety and performance characteristics may vary depending on the underlying platform
/// implementation.</remarks>
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <summary>The index of the frame every load reads; only the primary image of a container is of interest.</summary>
    private const int PrimaryImageIndex = 0;

    /// <summary>The message logged when a stream does not yield an image.</summary>
    private const string StreamFailureMessage = "Unable to parse bitmap from byte stream.";

    /// <summary>The message logged when a resource does not yield an image.</summary>
    private const string ResourceFailureMessage = "Unable to parse bitmap from resource.";

    /// <summary>The message reported when the platform decoder does not hand back an image.</summary>
    private const string DecodeFailureMessage = "Failed to load image";

    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
    {
        var data = NSData.FromStream(sourceStream);

        var tcs = new TaskCompletionSource<IBitmap?>(TaskCreationOptions.RunContinuationsAsynchronously);
#if UIKIT
        NSRunLoop.InvokeInBackground(() => Publish(tcs, () => DecodeData(data, desiredWidth, desiredHeight), StreamFailureMessage));
#else
        Publish(tcs, () => DecodeData(data, desiredWidth, desiredHeight), StreamFailureMessage);
#endif

        return tcs.Task;
    }

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
    {
        var tcs = new TaskCompletionSource<IBitmap?>(TaskCreationOptions.RunContinuationsAsynchronously);

#if UIKIT
        NSRunLoop.InvokeInBackground(() => Publish(tcs, () => DecodeResource(source, desiredWidth, desiredHeight), ResourceFailureMessage));
#else
        NSRunLoop.Main.BeginInvokeOnMainThread(() => Publish(tcs, () => DecodeResource(source, desiredWidth, desiredHeight), ResourceFailureMessage));
#endif
        return tcs.Task;
    }

    /// <inheritdoc />
    public IBitmap Create(float width, float height) => throw new NotSupportedException("Creating an empty bitmap is not supported by the Cocoa platform bitmap loader.");

    /// <summary>Runs a decode and hands its outcome, successful or not, to the awaiting caller.</summary>
    /// <param name="completion">The completion source the caller is awaiting.</param>
    /// <param name="decode">The decode to run.</param>
    /// <param name="failureMessage">The message to log if the decode throws.</param>
    private static void Publish(TaskCompletionSource<IBitmap?> completion, Func<UIImage> decode, string failureMessage)
    {
        try
        {
            _ = completion.TrySetResult(new CocoaBitmap(decode()));
        }
        catch (Exception ex)
        {
            LogHost.Default.Debug(ex, failureMessage);
            _ = completion.TrySetException(ex);
        }
    }

    /// <summary>Decodes image data, shrinking it inside the decode when a size was asked for.</summary>
    /// <param name="data">The encoded image, or <see langword="null"/> when the stream could not be read.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The decoded image.</returns>
    private static UIImage DecodeData(NSData? data, float? desiredWidth, float? desiredHeight)
    {
        if (data is null)
        {
            throw new InvalidOperationException("Failed to load stream");
        }

        if (desiredWidth is null && desiredHeight is null)
        {
#if UIKIT
            return UIImage.LoadFromData(data) ?? throw new InvalidOperationException(DecodeFailureMessage);
#else
            return new(data);
#endif
        }

        using var imageSource = CGImageSource.FromData(data) ?? throw new InvalidOperationException(DecodeFailureMessage);

        return DecodeAtSize(imageSource, desiredWidth, desiredHeight);
    }

    /// <summary>Decodes a bundled image, shrinking it inside the decode when the bundle exposes it as a file.</summary>
    /// <remarks>
    /// An image the bundle only offers through its asset catalogue has no file to read a header from, so it is loaded
    /// by name at its natural size; everything else goes through the scaling decoder.
    /// </remarks>
    /// <param name="source">The resource to load, as a relative path or a resource name.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The decoded image.</returns>
    private static UIImage DecodeResource(string source, float? desiredWidth, float? desiredHeight)
    {
        if (desiredWidth is not null || desiredHeight is not null)
        {
            using var imageSource = OpenBundleResource(source);

            if (imageSource is not null)
            {
                return DecodeAtSize(imageSource, desiredWidth, desiredHeight);
            }
        }

#if UIKIT
        return UIImage.FromBundle(source) ?? throw new InvalidOperationException($"Failed to load image from resource: {source}");
#else
        return UIImage.ImageNamed(source) ?? throw new InvalidOperationException($"Failed to load image from resource: {source}");
#endif
    }

    /// <summary>Opens a bundled resource for decoding when the bundle exposes it as a file.</summary>
    /// <param name="source">The resource to load, as a relative path or a resource name.</param>
    /// <returns>The opened image, or <see langword="null"/> when the bundle has no file for the resource.</returns>
    private static CGImageSource? OpenBundleResource(string source)
    {
        var url = NSBundle.MainBundle.GetUrlForResource(
            Path.GetFileNameWithoutExtension(source),
            Path.GetExtension(source).TrimStart('.'));

        return url is null ? null : CGImageSource.FromUrl(url);
    }

    /// <summary>Decodes an image down to the size the caller's request works out to.</summary>
    /// <remarks>
    /// The thumbnail decoder reads the header, then produces only the pixels that survive the requested bound, so the
    /// full-size image is never materialised. It keeps the proportions itself, which is why a single bound expresses
    /// the whole request.
    /// </remarks>
    /// <param name="imageSource">The opened image to decode.</param>
    /// <param name="desiredWidth">The requested width, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <param name="desiredHeight">The requested height, or <see langword="null"/> when the caller did not constrain it.</param>
    /// <returns>The decoded image.</returns>
    private static UIImage DecodeAtSize(CGImageSource imageSource, float? desiredWidth, float? desiredHeight)
    {
        var (sourceWidth, sourceHeight) = ReadPixelSize(imageSource);

        return FromCoreGraphics(
            BitmapDecodeSize.ChooseThumbnailPixelSize(sourceWidth, sourceHeight, desiredWidth, desiredHeight) is { } maxPixelSize
                ? imageSource.CreateThumbnail(PrimaryImageIndex, ThumbnailOptions(maxPixelSize))
                : imageSource.CreateImage(PrimaryImageIndex, new()));
    }

    /// <summary>Builds the instruction that bounds a thumbnail decode.</summary>
    /// <remarks>
    /// The decoder is told to build the thumbnail from the image itself rather than reuse one the container happens
    /// to embed, which would be whatever size its author chose, and to apply the recorded orientation.
    /// </remarks>
    /// <param name="maxPixelSize">The longest edge the decoder may produce, in pixels.</param>
    /// <returns>The decoder options.</returns>
    private static CGImageThumbnailOptions ThumbnailOptions(int maxPixelSize) =>
        new() { CreateThumbnailFromImageAlways = true, CreateThumbnailWithTransform = true, MaxPixelSize = maxPixelSize };

    /// <summary>Reads the dimensions an image presents, from its header rather than its pixels.</summary>
    /// <param name="imageSource">The opened image to inspect.</param>
    /// <returns>The dimensions, which are non-positive when the header could not be read.</returns>
    private static (int Width, int Height) ReadPixelSize(CGImageSource imageSource)
    {
        var properties = imageSource.GetProperties(PrimaryImageIndex);

        // The decoder is asked to apply the recorded orientation, so a quarter-turned photograph comes back with its
        // stored dimensions transposed and the box has to be fitted against that.
        return BitmapDecodeSize.OrientedPixelSize(
            properties?.PixelWidth ?? 0,
            properties?.PixelHeight ?? 0,
            (int)(properties?.Orientation ?? 0));
    }

    /// <summary>Wraps a decoded Core Graphics image in the platform's image type.</summary>
    /// <param name="image">The decoded image, or <see langword="null"/> when the decoder produced none.</param>
    /// <returns>The platform image.</returns>
    private static UIImage FromCoreGraphics(CGImage? image)
    {
        if (image is null)
        {
            throw new InvalidOperationException(DecodeFailureMessage);
        }

        // The platform image retains the Core Graphics image, so this reference to it is finished with either way.
        using (image)
        {
#if UIKIT
            return new(image);
#else
            // An empty size tells the image to take the dimensions of the pixels it was handed.
            return new(image, CGSize.Empty);
#endif
        }
    }
}
