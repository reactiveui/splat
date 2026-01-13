// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using Foundation;

using UIKit;
#else
using Foundation;

using UIApplication = AppKit.NSApplication;
using UIImage = AppKit.NSImage;
#endif

namespace Splat;

/// <summary>
/// Provides platform-specific functionality for loading and creating bitmap images.
/// </summary>
/// <remarks>This class implements the <see cref="IBitmapLoader"/> interface to support loading bitmaps from streams and
/// resources on the current platform. It is intended for internal use by image handling components that require
/// platform abstraction. Thread safety and performance characteristics may vary depending on the underlying platform
/// implementation.</remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Existing API")]
public class PlatformBitmapLoader : IBitmapLoader
{
    /// <inheritdoc />
    public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
    {
        var data = NSData.FromStream(sourceStream);

        var tcs = new TaskCompletionSource<IBitmap?>();
#if UIKIT
        NSRunLoop.InvokeInBackground(() =>
        {
            try
            {
                if (data is null)
                {
                    throw new InvalidOperationException("Failed to load stream");
                }

                var bitmap = UIImage.LoadFromData(data) ?? throw new InvalidOperationException("Failed to load image");
                tcs.TrySetResult(new CocoaBitmap(bitmap));
            }
            catch (Exception ex)
            {
                LogHost.Default.Debug(ex.ToString(), "Unable to parse bitmap from byte stream.");
                tcs.TrySetException(ex);
            }
        });
#else

        try
        {
            if (data is null)
            {
                throw new InvalidOperationException("Failed to load stream");
            }

            tcs.TrySetResult(new CocoaBitmap(new(data)));
        }
        catch (Exception ex)
        {
            LogHost.Default.Debug(ex, "Unable to parse bitmap from byte stream.");
            tcs.TrySetException(ex);
        }
#endif

        return tcs.Task;
    }

    /// <inheritdoc />
    public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
    {
        var tcs = new TaskCompletionSource<IBitmap?>();

#if UIKIT
        NSRunLoop.InvokeInBackground(() =>
        {
            try
            {
                var bitmap = UIImage.FromBundle(source) ?? throw new InvalidOperationException("Failed to load image from resource: " + source);
                tcs.TrySetResult(new CocoaBitmap(bitmap));
            }
            catch (Exception ex)
            {
                LogHost.Default.Debug(ex.ToString(), "Unable to parse bitmap from resource.");
                tcs.TrySetException(ex);
            }
        });
#else
        NSRunLoop.Main.BeginInvokeOnMainThread(() =>
        {
            try
            {
                var bitmap = UIImage.ImageNamed(source);
                if (bitmap is null)
                {
                    throw new InvalidOperationException("Failed to load image from resource: " + source);
                }

                tcs.TrySetResult(new CocoaBitmap(bitmap));
            }
            catch (Exception ex)
            {
                LogHost.Default.Debug(ex, "Unable to parse bitmap from resource.");
                tcs.TrySetException(ex);
            }
        });
#endif
        return tcs.Task;
    }

    /// <inheritdoc />
    public IBitmap Create(float width, float height) => throw new NotImplementedException();
}
