using System.Windows;

namespace Splat
{
    /// <summary>
    /// A set of extension methods which will convert between System.Drawing size's and a native size classes.
    /// </summary>
    public static class SizeExtensions
    {
        /// <summary>
        /// Convert a <see cref="System.Drawing.Size"/> to the android native <see cref="Size"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Size"/> of the value.</returns>
        public static Size ToNative(this System.Drawing.Size value)
        {
            return new Size(value.Width, value.Height);
        }

        /// <summary>
        /// Convert a <see cref="System.Drawing.SizeF"/> to the android native <see cref="Size"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Size"/> of the value.</returns>
        public static Size ToNative(this System.Drawing.SizeF value)
        {
            return new Size(value.Width, value.Height);
        }

        /// <summary>
        /// Converts a <see cref="Size"/> to a <see cref="System.Drawing.SizeF"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="System.Drawing.SizeF"/> of the value.</returns>
        public static System.Drawing.SizeF FromNative(this Size value)
        {
            return new System.Drawing.SizeF((float)value.Width, (float)value.Height);
        }
    }
}
