// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

#if UIKIT
using Foundation;
using UIKit;
#else
using Foundation;

using UIApplication = AppKit.NSApplication;
using UIImage = AppKit.NSImage;
#endif

namespace Splat
{
    /// <summary>
    /// A <see cref="IBitmapLoader"/> which will load Cocoa based bitmaps.
    /// </summary>
    public class PlatformBitmapLoader : IBitmapLoader
    {
        /// <inheritdoc />
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Logs message but a non-failing operation.")]
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose handled by user.")]
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            var data = NSData.FromStream(sourceStream);

            var tcs = new TaskCompletionSource<IBitmap>();
#if UIKIT
            NSRunLoop.InvokeInBackground(() =>
            {
                try
                {
                    var bitmap = UIImage.LoadFromData(data);
                    if (bitmap == null)
                    {
                        throw new Exception("Failed to load image");
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
                }
                catch (Exception ex)
                {
                    LogHost.Default.Debug(ex, "Unable to parse the known colour name.");
                    tcs.TrySetException(ex);
                }
            });
#else
            tcs.TrySetResult(new CocoaBitmap(new UIImage(data)));
#endif
            return tcs.Task;
        }

        /// <inheritdoc />
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Logs message but a non-failing operation.")]
        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            var tcs = new TaskCompletionSource<IBitmap>();

#if UIKIT
            NSRunLoop.InvokeInBackground(() =>
            {
                try
                {
                    var bitmap = UIImage.FromBundle(source);
                    if (bitmap == null)
                    {
                        throw new Exception("Failed to load image from resource: " + source);
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
                }
                catch (Exception ex)
                {
                    LogHost.Default.Debug(ex, "Unable to parse the known colour name.");
                    tcs.TrySetException(ex);
                }
            });
#else
            NSRunLoop.Main.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var bitmap = UIImage.ImageNamed(source);
                    if (bitmap == null)
                    {
                        throw new Exception("Failed to load image from resource: " + source);
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
#endif
            return tcs.Task;
        }

        /// <inheritdoc />
        public IBitmap Create(float width, float height)
        {
            throw new NotImplementedException();
        }
    }
}
