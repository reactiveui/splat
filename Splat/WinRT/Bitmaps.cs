using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(async () => {
                using (var rwStream = new InMemoryRandomAccessStream()) {
                    var writer = rwStream.AsStreamForWrite();
                    await sourceStream.CopyToAsync(writer);
                    await writer.FlushAsync();
                    rwStream.Seek(0);

                    var decoder = await BitmapDecoder.CreateAsync(rwStream);

                    var transform = new BitmapTransform
                    {
                        ScaledWidth = (uint) (desiredWidth ?? decoder.OrientedPixelWidth),
                        ScaledHeight = (uint) (desiredHeight ?? decoder.OrientedPixelHeight),
                        InterpolationMode = BitmapInterpolationMode.Fant
                    };

                    var pixelData = await decoder.GetPixelDataAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
                    var pixels = pixelData.DetachPixelData();

                    var bmp = new WriteableBitmap((int)transform.ScaledWidth, (int)transform.ScaledHeight);
                    using (var bmpStream = bmp.PixelBuffer.AsStream()) {
                        bmpStream.Seek(0, SeekOrigin.Begin);
                        bmpStream.Write(pixels, 0, (int)bmpStream.Length);
                        return (IBitmap) new WriteableBitmapImageBitmap(bmp);
                    }
                }
            });
        }

        public async Task<IBitmap> LoadFromResource(string resource, float? desiredWidth, float? desiredHeight)
        {
            return await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(async () => {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(resource));
                using (var stream = await file.OpenAsync(FileAccessMode.Read)) {
                    return await Load(stream.AsStreamForRead(), desiredWidth, desiredHeight);
                }
            });
        }

        public IBitmap Create(float width, float height)
        {
            return new WriteableBitmapImageBitmap(new WriteableBitmap((int)width, (int)height));
        }
    }

    class WriteableBitmapImageBitmap : IBitmap
    {
        internal WriteableBitmap inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public WriteableBitmapImageBitmap(WriteableBitmap bitmap)
        {
            inner = bitmap;
            Width = (float)inner.PixelWidth;
            Height = (float)inner.PixelHeight;
        }

        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            // NB: Due to WinRT's brain-dead design, we're copying this image 
            // like three times. Let Dreams Soar.
            var rwTarget = new InMemoryRandomAccessStream();
            var fmt = format == CompressedBitmapFormat.Jpeg ? BitmapEncoder.JpegEncoderId : BitmapEncoder.PngEncoderId;
            var encoder = await BitmapEncoder.CreateAsync(fmt, rwTarget, new[] { new KeyValuePair<string, BitmapTypedValue>("ImageQuality", new BitmapTypedValue(quality, PropertyType.Single)) });

            var pixels = new byte[inner.PixelBuffer.Length];
            await inner.PixelBuffer.AsStream().ReadAsync(pixels, 0, (int)inner.PixelBuffer.Length);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)inner.PixelWidth, (uint)inner.PixelHeight, 96, 96, pixels);
            await encoder.FlushAsync();
            await rwTarget.AsStream().CopyToAsync(target);
        }

        public void Dispose()
        {
            inner = null;
        }       
    }

    class BitmapImageBitmap : IBitmap
    {
        internal BitmapImage inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public BitmapImageBitmap(BitmapImage bitmap)
        {
            inner = bitmap;
            Width = (float)inner.PixelWidth;
            Height = (float)inner.PixelHeight;
        }

        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            string installedFolderImageSourceUri = inner.UriSource.OriginalString.Replace("ms-appx:/", "");
            var wb = new WriteableBitmap(inner.PixelWidth, inner.PixelHeight);
            var file = await StorageFile.GetFileFromPathAsync(inner.UriSource.OriginalString);
            await wb.SetSourceAsync(await file.OpenReadAsync());

            await (new WriteableBitmapImageBitmap(wb).Save(format, quality, target));
        }

        public void Dispose()
        {
            inner = null;
        }
    }

    public static class BitmapMixins
    {
        public static IBitmap FromNative(this BitmapImage This)
        {
            return new BitmapImageBitmap(This);
        }

        public static IBitmap FromNative(this WriteableBitmap This)
        {
            return new WriteableBitmapImageBitmap(This);
        }

        public static BitmapSource ToNative(this IBitmap This)
        {
            var wbib = This as WriteableBitmapImageBitmap;
            if (wbib != null) {
                return wbib.inner;
            }

            return ((BitmapImageBitmap)This).inner;
        }
    }

    static class DispatcherMixin
    {
        public static Task<T> RunAsync<T>(this CoreDispatcher This, Func<Task<T>> func, CoreDispatcherPriority prio = CoreDispatcherPriority.Normal)
        {
            var tcs = new TaskCompletionSource<T>();

            This.RunAsync(prio, () => {
                func().ContinueWith(t => {
                    if (t.IsFaulted) {
                        tcs.SetException(t.Exception);
                    } else {
                        tcs.SetResult(t.Result);
                    }
                });
            });

            return tcs.Task;
        }
    }
}
