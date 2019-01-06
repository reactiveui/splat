using System;
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
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            var data = NSData.FromStream(sourceStream);

            var tcs = new TaskCompletionSource<IBitmap>();
            NSRunLoop.Main.BeginInvokeOnMainThread(() =>
            {
                try
                {
#if UIKIT
                    var bitmap = UIImage.LoadFromData(data);
                    if (bitmap == null)
                    {
                        throw new Exception("Failed to load image");
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
#else
                    tcs.TrySetResult(new CocoaBitmap(new UIImage(data)));
#endif
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <inheritdoc />
        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            var tcs = new TaskCompletionSource<IBitmap>();
            NSRunLoop.Main.BeginInvokeOnMainThread(() =>
            {
                try
                {
#if UIKIT
                    var bitmap = UIImage.FromBundle(source);
#else
                    var bitmap = UIImage.ImageNamed(source);
#endif
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
            return tcs.Task;
        }

        /// <inheritdoc />
        public IBitmap Create(float width, float height)
        {
            throw new NotImplementedException();
        }
    }
}
