using System.Windows;

namespace Splat
{
    /// <summary>
    /// A set of extension methods which will convert between System.Drawing point's and a native point classes.
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public static Point ToNative(this System.Drawing.Point value)
        {
            return new Point(value.X, value.Y);
        }

        /// <summary>
        /// Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="Point"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public static Point ToNative(this System.Drawing.PointF value)
        {
            return new Point(value.X, value.Y);
        }

        /// <summary>
        /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.PointF"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
        public static System.Drawing.PointF FromNative(this Point value)
        {
            return new System.Drawing.PointF((float)value.X, (float)value.Y);
        }
    }
}
