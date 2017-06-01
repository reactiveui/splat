using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat.WinForms
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() => {
                var ret = new Bitmap(sourceStream);

                if (desiredWidth != null) {
                    ret = new Bitmap(ret, (int)desiredWidth, (int)desiredHeight);
                }

                return (IBitmap)new BitmapBitmap(ret);
            });
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() => {
                var ret = new Bitmap(source);

                if (desiredWidth != null) {
                    ret = new Bitmap(ret, (int)desiredWidth, (int)desiredHeight);
                }

                return (IBitmap)new BitmapBitmap(ret);
            });
        }

        public IBitmap Create(float width, float height)
        {
            return (IBitmap)new BitmapBitmap(new Bitmap((int)width, (int)height));
        }
    }

    class BitmapBitmap : IBitmap
    {
        internal Bitmap inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public BitmapBitmap(Bitmap bitmap)
        {
            inner = bitmap;
            Width = (float)inner.Width;
            Height = (float)inner.Height;
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            return Task.Run(() => {
                if (format == CompressedBitmapFormat.Jpeg) {
                    var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    var encoderParams = new EncoderParameters(1);
                    var encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (int)(quality * 100.0f));
                    encoderParams.Param[0] = encoderParam;
                    inner.Save(target, jpgEncoder, encoderParams);
                } else {
                    inner.Save(target, ImageFormat.Png);
                }
            });
        }

        public void Dispose()
        {
            inner.Dispose();
        }

        ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();

            foreach (var codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }

            return null;
        }
    }

    public static class BitmapMixins
    {
        public static IBitmap FromNative(this Bitmap This)
        {
            return new BitmapBitmap(This);
        }

        public static Bitmap ToNative(this IBitmap This)
        {
            return ((BitmapBitmap)This).inner;
        }
    }
}
