using System;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using System.Threading;
using Android.Content;
using Android.App;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using System.Linq;

using Path = System.IO.Path;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        static readonly Dictionary<string, int> drawableList;

        static PlatformBitmapLoader()
        {
            // NB: This is some hacky shit, but on MonoAndroid at the moment, 
            // this is always the entry assembly.
            var assm = AppDomain.CurrentDomain.GetAssemblies()[1];

            var resources = assm.GetModules().SelectMany(x => x.GetTypes()).First(x => x.Name == "Resource");

            drawableList = resources.GetNestedType("Drawable").GetFields()
                .Where(x => x.FieldType == typeof(int))
                .ToDictionary(k => k.Name, v => (int)v.GetRawConstantValue());
        }

        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            Bitmap bitmap = null;
            
            if (desiredWidth == null) {
                bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream));
            } else {
                var opts = new BitmapFactory.Options() {
                    OutWidth = (int)desiredWidth.Value,
                    OutHeight = (int)desiredHeight.Value,
                };

                var noPadding = new Rect(0, 0, 0, 0);
                bitmap = await Task.Run(() => BitmapFactory.DecodeStream(sourceStream, noPadding, opts));
            }

            if (bitmap == null) {
                throw new IOException("Failed to load bitmap from source stream");
            }

            return bitmap.FromNative();
        }

        public Task<IBitmap> LoadFromResource(string source, float? desiredWidth, float? desiredHeight)
        {
            var res = Application.Context.Resources;

            var id = default(int);
            if (Int32.TryParse(source, out id)) {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(id)));
            }

            if (drawableList.ContainsKey(source)) {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(drawableList[source])));
            }

            // NB: On iOS, you have to pass the extension, but on Android it's 
            // stripped - try stripping the extension to see if there's a Drawable.
            var key = Path.GetFileNameWithoutExtension(source);
            if (drawableList.ContainsKey(key)) {
                return Task.Run(() => (IBitmap)new DrawableBitmap(res.GetDrawable(drawableList[key])));
            }

            throw new ArgumentException("Either pass in an integer ID cast to a string, or the name of a drawable resource");
        }

        public IBitmap Create(float width, float height)
        {
            return Bitmap.CreateBitmap((int)width, (int)height, Bitmap.Config.Argb8888).FromNative();
        }
    }

    sealed class DrawableBitmap : IBitmap
    {
        internal Drawable inner;

        public DrawableBitmap(Drawable inner)
        {
            this.inner = inner;
        }

        public float Width {
            get { return (float)inner.IntrinsicWidth; }
        }

        public float Height {
            get { return (float)inner.IntrinsicHeight; }
        }

        public Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            throw new NotSupportedException("You can't save resources");
        }

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref inner, null);
            if (disp != null) disp.Dispose();
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
        public static Drawable ToNative(this IBitmap This)
        {
            var androidBitmap = This as AndroidBitmap;
            if (androidBitmap != null) {
                return new BitmapDrawable(((AndroidBitmap)This).inner);
            } else {
                return ((DrawableBitmap)This).inner;
            }
        }

        public static IBitmap FromNative(this Bitmap This, bool copy = false)
        {
            if (copy) return new AndroidBitmap(This.Copy(This.GetConfig(), true));
            return new AndroidBitmap(This);
        }

        public static IBitmap FromNative(this Drawable This)
        {
            return new DrawableBitmap(This);
        }
    }
}
