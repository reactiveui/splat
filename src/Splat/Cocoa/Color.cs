using System;

#if UIKIT && !UNIFIED
using MonoTouch.UIKit;
#elif UNIFIED && UIKIT
using UIKit;
#elif UNIFIED && !UIKIT
using AppKit;
#else
using MonoMac.AppKit;
#endif

namespace Splat
{
#if UIKIT
    public static class ColorExtensions
    {
        public static UIColor ToNative(this System.Drawing.Color This)
        {
            return new UIColor((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static System.Drawing.Color FromNative(this UIColor This)
        {
#if UNIFIED
            nfloat r,g,b,a;
#else
            float r,g,b,a;
#endif

            This.GetRGBA(out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
        }
    }
#else
    public static class ColorExtensions
    {
        public static NSColor ToNative(this System.Drawing.Color This)
        {
            return NSColor.FromSrgb((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static System.Drawing.Color FromNative(this NSColor This)
        {
#if UNIFIED
            nfloat r,g,b,a;
#else
            float r,g,b,a;
#endif

            This.GetRgba(out r, out g, out b, out a);
            return System.Drawing.Color.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
        }
    }
#endif
}
