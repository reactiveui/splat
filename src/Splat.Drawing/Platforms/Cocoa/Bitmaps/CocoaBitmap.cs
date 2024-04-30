// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
/// Wraps a cocoa native bitmap into the splat <see cref="IBitmap"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CocoaBitmap"/> class.
/// </remarks>
/// <param name="inner">The native image we are wrapping.</param>
internal sealed class CocoaBitmap(UIImage inner) : IBitmap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed in a Interlocked method")]
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
            var data = (format == CompressedBitmapFormat.Jpeg ? _inner.AsJPEG((float)quality) : _inner.AsPNG())!;
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
