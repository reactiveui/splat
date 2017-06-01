using System;
using Android.Graphics;

namespace Splat
{
    public static class ColorExtensions
    {
        public static Color ToNative(this System.Drawing.Color This)
        {
            return new Color(This.R, This.G, This.B, This.A);
        }

        public static System.Drawing.Color FromNative(this Color This)
        {
            return System.Drawing.Color.FromArgb(This.A, This.R, This.G, This.B);
        }
    }
}
