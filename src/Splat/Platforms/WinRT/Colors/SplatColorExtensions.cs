using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Splat.Platforms.WinRT.Colors
{
    /// <summary>
    /// Extension methods associated with the <see cref="SplatColor"/> struct.
    /// </summary>
    public static class SplatColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="SplatColor"/> into the XAML color.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The XAML color generated.</returns>
        public static Color ToNative(this SplatColor value)
        {
            return Color.FromArgb(value.A, value.R, value.G, value.B);
        }

        /// <summary>
        /// Converts a <see cref="SplatColor"/> into the XAML <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The <see cref="SolidColorBrush"/> generated.</returns>
        public static SolidColorBrush ToNativeBrush(this SplatColor value)
        {
            return new SolidColorBrush(value.ToNative());
        }

        /// <summary>
        /// Converts a XAML color into the XAML <see cref="SplatColor"/>.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The <see cref="SplatColor"/> generated.</returns>
        public static SplatColor FromNative(this Color value)
        {
            return SplatColor.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}
