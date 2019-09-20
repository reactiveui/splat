// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;

#if UIKIT
using UIKit;
#else
using AppKit;
using CoreGraphics;
using Foundation;

using UIImage = AppKit.NSImage;
#endif

namespace Splat
{
    /// <summary>
    /// Wraps a cocoa native bitmap into the splat <see cref="IBitmap"/>.
    /// </summary>
    internal sealed class CocoaBitmap : IBitmap
    {
        private UIImage _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CocoaBitmap"/> class.
        /// </summary>
        /// <param name="inner">The native image we are wrapping.</param>
        public CocoaBitmap(UIImage inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public float Width => (float)_inner.Size.Width;

        /// <inheritdoc />
        public float Height => (float)_inner.Size.Height;

        /// <summary>
        /// Gets the native image.
        /// </summary>
        internal UIImage Inner => _inner;

        /// <inheritdoc />
        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() =>
            {
#if UIKIT
                var data = format == CompressedBitmapFormat.Jpeg ? _inner.AsJPEG((float)quality) : _inner.AsPNG();
                data.AsStream().CopyTo(target);

#else

                var rect = CGRect.Empty;

                var cgImage = _inner.AsCGImage(ref rect, null, null);
                var imageRep = new NSBitmapImageRep(cgImage);

                var props = format == CompressedBitmapFormat.Png ?
                    new NSDictionary() :
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
            if (disp != null)
            {
                disp.Dispose();
            }
        }
    }
}
