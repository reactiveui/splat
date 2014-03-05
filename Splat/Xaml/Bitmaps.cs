using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
#if SILVERLIGHT
            return Deployment.Current.Dispatcher.InvokeAsync(() => {
#else
            return Task.Run(() => {
#endif
                var ret = new BitmapImage();

                withInit(ret, source => {
                    if (desiredWidth != null) {
                        source.DecodePixelWidth = (int)desiredWidth;
                        source.DecodePixelHeight = (int)desiredHeight;
                    }

#if SILVERLIGHT
                    source.SetSource(sourceStream);
#else
                    source.StreamSource = sourceStream;
                    source.CacheOption = BitmapCacheOption.OnLoad;
#endif
                });

                return (IBitmap) new BitmapSourceBitmap(ret);
            });
        }

        public Task<IBitmap> LoadFromResource(string resource, float? desiredWidth, float? desiredHeight)
        {
#if SILVERLIGHT
            return Deployment.Current.Dispatcher.InvokeAsync(() => {
#else
            return Task.Run(() => {
#endif
                var ret = new BitmapImage();
                withInit(ret, x => {
                    if (desiredWidth != null) {
                        x.DecodePixelWidth = (int)desiredWidth;
                        x.DecodePixelHeight = (int)desiredHeight;
                    }

                    x.UriSource = new Uri(resource);
                });

                return (IBitmap) new BitmapSourceBitmap(ret);
            });
        }

        public IBitmap Create(float width, float height)
        {
#if SILVERLIGHT
            return (IBitmap)new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height));
#else
            return (IBitmap) new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, 96, 96, PixelFormats.Default, null));
#endif
        }

        void withInit(BitmapImage source, Action<BitmapImage> block)
        {
#if SILVERLIGHT
            block(source);
#else
            source.BeginInit();
            block(source);
            source.EndInit();
            source.Freeze();
#endif
        }
    }

    class BitmapSourceBitmap : IBitmap
    {
        internal BitmapSource inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public BitmapSourceBitmap(BitmapSource bitmap)
        {
            inner = bitmap;
#if SILVERLIGHT
            Width = (float)inner.PixelWidth;
            Height = (float)inner.PixelHeight;
#else
            Width = (float)inner.Width;
            Height = (float)inner.Height;
#endif
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() => {
#if SILVERLIGHT
                if (format == CompressedBitmapFormat.Png) {
                    throw new PlatformNotSupportedException("WP8 can't save PNGs.");
                }

                var wb = new WriteableBitmap(inner);
                wb.SaveJpeg(target, wb.PixelWidth, wb.PixelHeight, 0, (int)(quality * 100.0f));
#else
                var encoder = format == CompressedBitmapFormat.Jpeg ?
                    (BitmapEncoder)new JpegBitmapEncoder() { QualityLevel = (int)(quality * 100.0f) } :
                    (BitmapEncoder)new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(inner));
                encoder.Save(target);
#endif
            });
        }
        public void Dispose()
        {
            inner = null;
        }
    }

    public static class BitmapMixins
    {
        public static IBitmap FromNative(this BitmapSource This)
        {
            return new BitmapSourceBitmap(This);
        }

        public static BitmapSource ToNative(this IBitmap This)
        {
            return ((BitmapSourceBitmap)This).inner;
        }
    }

    static class DispatcherMixin
    {
        public static Task<T> InvokeAsync<T>(this Dispatcher This, Func<T> block)
        {
            var tcs = new TaskCompletionSource<T>();

            This.BeginInvoke(new Action(() => {
                try {
                    tcs.SetResult(block());
                } catch (Exception ex) {
                    tcs.SetException(ex);
                }
            }));

            return tcs.Task;
        }
    }
}