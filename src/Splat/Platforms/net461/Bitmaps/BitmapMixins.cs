using System.Windows.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// Extension methods to assist with dealing with Bitmaps.
    /// </summary>
    public static class BitmapMixins
    {
        /// <summary>
        /// Converts <see cref="IBitmap"/> to a native type.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="BitmapSource"/> bitmap.</returns>
        public static IBitmap FromNative(this BitmapSource value)
        {
            return new BitmapSourceBitmap(value);
        }

        /// <summary>
        /// Converts a <see cref="BitmapSource"/> to a splat <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The native bitmap to convert from.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static BitmapSource ToNative(this IBitmap value)
        {
            return ((BitmapSourceBitmap)value).Inner;
        }
    }
}
