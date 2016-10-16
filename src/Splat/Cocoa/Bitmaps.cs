using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#if UIKIT && !UNIFIED
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#elif UNIFIED && UIKIT
using UIKit;
using Foundation;
#elif UNIFIED && !UIKIT
using AppKit;
using Foundation;

using UIImage = AppKit.NSImage;
using UIApplication = AppKit.NSApplication;
#else
using System.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;

using UIImage = MonoMac.AppKit.NSImage;
using UIApplication = MonoMac.AppKit.NSApplication;
#endif

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            var data = NSData.FromStream(sourceStream);

            var tcs = new TaskCompletionSource<IBitmap>();
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                try {
#if UIKIT
                    var bitmap = UIImage.LoadFromData(data);
                    if (bitmap == null) {
                        throw new Exception("Failed to load image");
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
#else
                    tcs.TrySetResult(new CocoaBitmap(new UIImage(data)));
#endif
                } catch (Exception ex) {
                    tcs.TrySetException(ex);
                }
            });
            return tcs.Task;
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            var tcs = new TaskCompletionSource<IBitmap>();
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                try {
#if UIKIT
                    var bitmap = UIImage.FromBundle(source);
#else
                    var bitmap = UIImage.ImageNamed(source);
#endif
                    if (bitmap == null) {
                        throw new Exception("Failed to load image from resource: " + source);
                    }

                    tcs.TrySetResult(new CocoaBitmap(bitmap));
                } catch (Exception ex) {
                    tcs.TrySetException(ex);
                }
            });
            return tcs.Task;
        }

        public IBitmap Create(float width, float height)
        {
            throw new NotImplementedException();
        }
    }

    sealed class CocoaBitmap : IBitmap
    {
        internal UIImage inner;
        public CocoaBitmap(UIImage inner)
        {
            this.inner = inner;
        }

        public float Width {
            get { return (float)inner.Size.Width; }
        }

        public float Height {
            get { return (float)inner.Size.Height; }
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() => {
#if UIKIT
                var data = format == CompressedBitmapFormat.Jpeg ? inner.AsJPEG((float)quality) : inner.AsPNG();
                data.AsStream().CopyTo(target);

#else

#if UNIFIED
                var rect = new CoreGraphics.CGRect();
#else
                var rect = new RectangleF();
#endif

                var cgImage = inner.AsCGImage(ref rect, null, null);
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

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref inner, null);
            if (disp != null) disp.Dispose();
        }
    }

    public static class BitmapMixins
    {
        public static UIImage ToNative(this IBitmap This)
        {
            return ((CocoaBitmap)This).inner;
        }

        public static IBitmap FromNative(this UIImage This, bool copy = false)
        {
            if (copy) return new CocoaBitmap((UIImage)This.Copy());

            return new CocoaBitmap(This);
        }
    }
}
