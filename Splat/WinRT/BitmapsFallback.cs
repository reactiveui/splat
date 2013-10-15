using Windows.Storage.Streams;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Splat
{
    class FallbackBitmapLoader : IBitmapLoader
    {
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return new BitmapImageBitmap(await ByteArrayToBitmapImage(sourceStream));
        }

        private static async Task<BitmapImage> ByteArrayToBitmapImage(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(await ConvertToRandomAccessStream(stream));
            return bitmapImage;
        }

        private static async Task<InMemoryRandomAccessStream> ConvertToRandomAccessStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            var ras = new InMemoryRandomAccessStream();
            var writer = ras.AsStreamForWrite();

            await stream.CopyToAsync(writer);
            await writer.FlushAsync();

            ras.Seek(0);
            return ras;
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            throw new NotImplementedException();
        }

        public IBitmap Create(float width, float height)
        {
            throw new NotImplementedException();
        }
    }
}
