using Android.Graphics;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for interacting with colors, to and from the android colors.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to a android native color.
        /// </summary>
        /// <param name="other">The System.Drawing.Color to convert.</param>
        /// <returns>A native android color.</returns>
        public static Color ToNative(this System.Drawing.Color other)
        {
            return new Color(other.R, other.G, other.B, other.A);
        }

        /// <summary>
        /// Converts from a android native color to a <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="other">The android native color to convert.</param>
        /// <returns>A System.Drawing.Color.</returns>
        public static System.Drawing.Color FromNative(this Color other)
        {
            return System.Drawing.Color.FromArgb(other.A, other.R, other.G, other.B);
        }
    }
}
