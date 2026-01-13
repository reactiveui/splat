// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using UIKit;
#else
using AppKit;

using CoreGraphics;

using Foundation;

using UIImage = AppKit.NSImage;
#endif

namespace Splat;

/// <summary>
/// Represents a bitmap image backed by a native Cocoa UIImage instance.
/// </summary>
/// <remarks>This class provides a platform-specific implementation of the IBitmap interface for Cocoa-based
/// environments. It manages the lifetime of the underlying UIImage and ensures proper disposal. Instances of this class
/// are not thread-safe.</remarks>
/// <param name="inner">The native UIImage that provides the underlying image data for the bitmap. Cannot be null.</param>
internal sealed class CocoaBitmap(UIImage inner) : IBitmap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Is Disposed using Interlocked method")]
    private UIImage? _inner = inner;

    /// <inheritdoc />
    public float Width => (float)(_inner?.Size.Width ?? 0);

    /// <inheritdoc />
    public float Height => (float)(_inner?.Size.Height ?? 0);

    /// <summary>
    /// Gets the native image.
    /// </summary>
    internal UIImage Inner => _inner ?? throw new InvalidOperationException("Inner bitmap is no longer valid");

    /// <inheritdoc />
    public Task Save(CompressedBitmapFormat format, float quality, Stream target)
    {
        if (_inner is null)
        {
            return Task.CompletedTask;
        }

        return Task.Run(() =>
        {
#if UIKIT
            var data = (format == CompressedBitmapFormat.Jpeg ? _inner.AsJPEG(quality) : _inner.AsPNG())!;
            data.AsStream().CopyTo(target);

#else

            var rect = CGRect.Empty;

            var cgImage = _inner.AsCGImage(ref rect, null, null);
            var imageRep = new NSBitmapImageRep(cgImage);

            var props = format == CompressedBitmapFormat.Png ?
                new() :
                new NSDictionary(new NSNumber(quality), new NSString("NSImageCompressionFactor"));

            var type = format == CompressedBitmapFormat.Png ? NSBitmapImageFileType.Png : NSBitmapImageFileType.Jpeg;

            var outData = imageRep.RepresentationUsingTypeProperties(type, props);
            if (outData is null)
            {
                throw new InvalidOperationException("Failed to create bitmap representation");
            }

            outData.AsStream().CopyTo(target);
#endif
        });
    }

    /// <inheritdoc />
    public void Dispose()
    {
        var disp = Interlocked.Exchange(ref _inner, null);
        disp?.Dispose();
    }
}
