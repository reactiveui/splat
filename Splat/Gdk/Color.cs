using System;

using Gdk;

namespace Splat
{
    public static class ColorExtensions
    {
#if GTK_SHARP_3
        public static RGBA ToNative(this System.Drawing.Color This)
        {
            return new RGBA () {
                Alpha = This.A / 255.0, Red = This.R / 255.0, Green = This.G / 255.0, Blue = This.B / 255.0
            };
        }

        public static System.Drawing.Color FromNative(this RGBA This)
        {
            return System.Drawing.Color.FromArgb((int)(This.Alpha * 255.0), (int)(This.Red * 255.0),
                (int)(This.Green * 255.0), (int)(This.Blue * 255.0));
        }
#else
        public static Color ToNative(this System.Drawing.Color This)
        {
            return new Color(This.R, This.G, This.B);
        }

        public static System.Drawing.Color FromNative(this Color This)
        {
            return System.Drawing.Color.FromArgb(This.Red, This.Green, This.Blue);
        }
#endif
    }
}
