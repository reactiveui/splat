using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tizen.Multimedia.Util;

namespace Splat
{
    class PlatformBitmapLoader : IBitmapLoader
    {
        public IBitmap Create(float width, float height)
        {
            TizenBitmap bitmap = new TizenBitmap(BitmapHelper.EmptyImageBinary);
            return bitmap;
        }

        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return Task.Run(() =>
            {
                var ret = new TizenBitmap();
                ret.SetImage(((MemoryStream)sourceStream).ToArray(), desiredWidth, desiredHeight);
                return (IBitmap)ret;
            });
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {            
            return Task.Run(() =>
            {
                var ret = new TizenBitmap();
                ret.SetImage(source, desiredWidth, desiredHeight);
                return (IBitmap)ret;
            });
        }        
    }

    class TizenBitmap : IBitmap
    {
        internal BitmapFrame inner;

        public TizenBitmap()
        {
            inner = null;
        }

        public TizenBitmap(byte[] image)
        {
            this.inner = GetBitmapFrame(image); ;
        }

        public TizenBitmap(BitmapFrame image)
        {
            this.inner = image;
        }

        public void Dispose()
        {
            inner = null;
        }

        public float Width
        {
            get { return inner == null ? 0 : inner.Size.Width; }
        }

        public float Height
        {
            get { return inner == null ? 0 : inner.Size.Height; }
        }

        public void SetImage(string image, float? desiredWidth, float? desiredHeight)
        {
            inner = null;
            inner = GetBitmapFrame(File.ReadAllBytes(image));
        }

        public void SetImage(byte[] image, float? desiredWidth, float? desiredHeight)
        {
            inner = null;
            inner = GetBitmapFrame(image);
        }

        private BitmapFrame GetBitmapFrame(byte[] imageBuffer)
        {
            BitmapFrame result = null;
            List<ImageDecoder> decoderList = new List<ImageDecoder>{
                new JpegDecoder(), new PngDecoder(), new BmpDecoder(), new GifDecoder()
            };
            foreach (var decoder in decoderList)
            {
                try
                {
                    result = (decoder.DecodeAsync(imageBuffer).Result).First();
                    break;
                }
                catch (Tizen.Multimedia.FileFormatException)
                {
                    continue;
                }
            }
            return result;            
        }    

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            ImageEncoder encoder = null;
            int qualityPercent = (int)(100 * quality);
            switch (format)
            {
                case CompressedBitmapFormat.Jpeg:
                    encoder = new JpegEncoder();
                    ((JpegEncoder)encoder).Quality = qualityPercent;
                    break;
                case CompressedBitmapFormat.Png:
                    encoder = new PngEncoder();
                    if (qualityPercent == 100) ((PngEncoder)encoder).Compression = PngCompression.None;
                    else if (qualityPercent < 10) ((PngEncoder)encoder).Compression = PngCompression.Level1;
                    else ((PngEncoder)encoder).Compression = (PngCompression)(qualityPercent / 10);
                    break;
            }
            encoder.SetResolution(new Tizen.Multimedia.Size((int)Width, (int)Height));
            return encoder.EncodeAsync(inner.Buffer, target);
        }
    }

    public static class BitmapMixins
    {
        public static IBitmap FromNative(this BitmapFrame This)
        {
            return new TizenBitmap(This);
        }
        public static BitmapFrame ToNative(this IBitmap This)
        {
            return ((TizenBitmap)This).inner;
        }
    }
}
