using System;
using System.IO;
using System.Threading.Tasks;

using Gdk;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() => {
                if (desiredWidth != null && desiredHeight != null)
                    return (IBitmap)new PixbufBitmap(new Pixbuf(sourceStream, (int)desiredWidth, (int)desiredHeight));

                return (IBitmap)new PixbufBitmap(new Pixbuf (sourceStream));
            });
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() => {
                return (IBitmap)new PixbufBitmap(Pixbuf.LoadFromResource(source));
            });
        }

        public IBitmap Create(float width, float height)
        {
            return new PixbufBitmap(new Pixbuf(Colorspace.Rgb, true, 32, (int)width, (int)height));
        }
    }

    public sealed class PixbufBitmap : IBitmap
    {
        internal Pixbuf inner;

        public PixbufBitmap(Pixbuf inner)
        {
            this.inner = inner;
        }

        #region IBitmap implementation
        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() => {
                var formatName = format == CompressedBitmapFormat.Jpeg ? "jpeg" : "png";
                var param = format == CompressedBitmapFormat.Jpeg ? "quality" : "compression";
                byte[] buffer = inner.SaveToBuffer(formatName, new [] { param }, new [] { quality.ToString() });
                using (var ms = new MemoryStream(buffer)) {
                    ms.CopyTo(target);
                }
            });
        }

        public float Width
        {
            get {
                return inner.Width;
            }
        }

        public float Height
        {
            get {
                return inner.Height;
            }
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            inner.Dispose();
        }
        #endregion
    }

    public static class BitmapMixins
    {
        public static Pixbuf ToNative(this IBitmap This)
        {
            return ((PixbufBitmap)This).inner;
        }

        public static IBitmap FromNative(this Pixbuf This, bool copy = false)
        {
            if (copy) return new PixbufBitmap((Pixbuf)This.Copy());

            return new PixbufBitmap(This);
        }
    }
}
