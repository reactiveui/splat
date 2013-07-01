using System;
using MonoTouch.UIKit;

namespace Splat
{
    public static class ColorExtensions
    {
        public static UIColor ToNative(System.Drawing.Color This)
        {
            return new UIColor((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static System.Drawing.Color FromNative(UIColor This)
        {
            float r,g,b,a;

            This.GetRGBA(out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f), (int)(a * 255.0f));
        }
    }
}