using Windows.UI.Xaml.Media.Imaging;

namespace Splat.Platforms.WinRT.Bitmaps
{
    /// <summary>
    /// Extension methods to assist with dealing with Bitmaps.
    /// </summary>
    public static class BitmapMixins
    {
        /// <summary>
        /// Converts <see cref="BitmapImage"/> to a <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static IBitmap FromNative(this BitmapImage value)
        {
            return new BitmapImageBitmap(value);
        }

        /// <summary>
        /// Converts <see cref="WriteableBitmap"/> to a <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static IBitmap FromNative(this WriteableBitmap value)
        {
            return new WriteableBitmapImageBitmap(value);
        }

        /// <summary>
        /// Converts <see cref="IBitmap"/> to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="BitmapSource"/> bitmap.</returns>
        public static BitmapSource ToNative(this IBitmap value)
        {
            var wbib = value as WriteableBitmapImageBitmap;
            if (wbib != null)
            {
                return wbib.Inner;
            }

            return ((BitmapImageBitmap)value).Inner;
        }
    }
}
