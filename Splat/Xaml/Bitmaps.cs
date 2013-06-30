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
                var source = new BitmapImage();

                source.BeginInit();
                source.StreamSource = sourceStream;
                if (desiredWidth != null)
                {
                    source.DecodePixelWidth = (int)desiredWidth;
                    source.DecodePixelHeight = (int)desiredHeight;
                }
                source.EndInit();
                source.Freeze();

                return new BitmapSourceBitmap(source);
            });
        }

        public IBitmap Create(float width, float height)
        {
            return new BitmapSourceBitmap(new WriteableBitmap((int)width, (int)height, 96, 96, PixelFormats.Default, null));
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
}