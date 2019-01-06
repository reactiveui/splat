using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Multimedia.Util;

namespace Splat
{
    /// <summary>
    /// Extension methods to assist with dealing with Bitmaps.
    /// </summary>
    public static class BitmapMixins
    {
        /// <summary>
        /// Converts <see cref="BitmapFrame"/> to a <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static IBitmap FromNative(this BitmapFrame value)
        {
            return new TizenBitmap(value);
        }

        /// <summary>
        /// Converts <see cref="IBitmap"/> to a <see cref="BitmapFrame"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="BitmapFrame"/> bitmap.</returns>
        public static BitmapFrame ToNative(this IBitmap value)
        {
            return ((TizenBitmap)value).Inner;
        }
    }
}
