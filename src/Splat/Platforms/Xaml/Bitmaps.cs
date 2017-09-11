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

            return Task.Run(() => {
                var ret = new BitmapImage();

                withInit(ret, source => {
                    if (desiredWidth != null) {
                        source.DecodePixelWidth = (int)desiredWidth;
                        source.DecodePixelHeight = (int)desiredHeight;
                    }
                    source.StreamSource = sourceStream;
                    source.CacheOption = BitmapCacheOption.OnLoad;
                });

                return (IBitmap) new BitmapSourceBitmap(ret);
            });
        }

        public Task<IBitmap> LoadFromResource(string resource, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() => {
                var ret = new BitmapImage();
                withInit(ret, x => {
                    if (desiredWidth != null) {
                        x.DecodePixelWidth = (int)desiredWidth;
                        x.DecodePixelHeight = (int)desiredHeight;
                    }

                    x.UriSource = new Uri(resource, UriKind.RelativeOrAbsolute);
                });

                return (IBitmap) new BitmapSourceBitmap(ret);
            });
        }

        public IBitmap Create(float width, float height)
        {
            return (IBitmap) new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, 96, 96, PixelFormats.Default, null));
        }

        void withInit(BitmapImage source, Action<BitmapImage> block)
        {
            source.BeginInit();
            block(source);
            source.EndInit();
            source.Freeze();
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
            Width = (float)inner.Width;
            Height = (float)inner.Height;
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() => {
                var encoder = format == CompressedBitmapFormat.Jpeg ?
                    (BitmapEncoder)new JpegBitmapEncoder() { QualityLevel = (int)(quality * 100.0f) } :
                    (BitmapEncoder)new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(inner));
                encoder.Save(target);
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