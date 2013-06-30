using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Splat
{
    class BitmapLoader : IBitmapLoader
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

#if SILVERLIGHT
                    source.SetSource(sourceStream);
#else
                    source.StreamSource = sourceStream;
#endif
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
        BitmapSource inner;

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
}