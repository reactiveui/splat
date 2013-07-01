using System;

#if UIKIT
using MonoTouch.UIKit;
#else
using MonoMac.AppKit;
#endif

namespace Splat
{
#if UIKIT
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
#else
    public static class ColorExtensions
    {
        public static NSColor ToNative(System.Drawing.Color This)
        {
            return NSColor.FromSrgb((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static System.Drawing.Color FromNative(NSColor This)
        {
            float r,g,b,a;

            This.GetRgba(out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f), (int)(a * 255.0f));
        }
    }
#endif
}