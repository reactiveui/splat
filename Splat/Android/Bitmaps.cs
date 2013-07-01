using System;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using System.Threading;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            if (desiredWidth == null) {
                return Task.Run(() => BitmapFactory.DecodeStream(sourceStream).FromNative());
            }

            var opts = new BitmapFactory.Options() {
                OutWidth = (int)desiredWidth.Value,
                OutHeight = (int)desiredHeight.Value,
            };
            var noPadding = new Rect(0, 0, 0, 0);
            return Task.Run(() => BitmapFactory.DecodeStream(sourceStream, noPadding, opts).FromNative());
        }

        public IBitmap Create(float width, float height)
        {
            return Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb8888).FromNative();
        }
    }

    sealed class AndroidBitmap : IBitmap
    {
        internal Bitmap inner;
        public AndroidBitmap(Bitmap inner)
        {
            this.inner = inner;
        }

        public float Width {
            get { return inner.Width; }
        }

        public float Height {
            get { return inner.Height; }
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            var fmt = format == CompressedBitmapFormat.Jpeg ? Bitmap.CompressFormat.Jpeg : Bitmap.CompressFormat.Png;
            return Task.Run(() => { inner.Compress(fmt, (int)quality * 100, target); });
        }

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref inner, null);
            if (disp != null) disp.Dispose();
        }
    }

    public static class BitmapMixins
    {
        public static Bitmap ToNative(this IBitmap This)
        {
            return ((AndroidBitmap)This).inner;
        }

        public static IBitmap FromNative(this Bitmap This, bool copy = false)
        {
            if (copy) return new AndroidBitmap(This.Copy(This.GetConfig(), true));
            return new AndroidBitmap(This);
        }
    }
}
