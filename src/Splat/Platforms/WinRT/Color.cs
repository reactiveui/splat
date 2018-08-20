using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Splat
{
    public static class ColorExtensions
    {
        public static Color ToNative(this System.Drawing.Color This)
        {
            return Color.FromArgb(This.A, This.R, This.G, This.B);
        }

        public static SolidColorBrush ToNativeBrush(this System.Drawing.Color This)
        {
            return new SolidColorBrush(This.ToNative());
        }

        public static System.Drawing.Color FromNative(this Color This)
        {
            return System.Drawing.Color.FromArgb(This.A, This.R, This.G, This.B);
        }
    }

    public static class SplatColorExtensions
    {
        public static Color ToNative(this SplatColor This)
        {
            return Color.FromArgb(This.A, This.R, This.G, This.B);
        }

        public static SolidColorBrush ToNativeBrush(this SplatColor This)
        {
            return new SolidColorBrush(This.ToNative());
        }

        public static SplatColor FromNative(this Color This)
        {
            return SplatColor.FromArgb(This.A, This.R, This.G, This.B);
        }
    }
}
