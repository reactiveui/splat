using System;

#if UIKIT
using UIKit;
#else
using AppKit;
#endif


namespace Splat
{
#if UIKIT
    public static class SplatColorExtensions
    {
        public static UIColor ToNative(this SplatColor This)
        {
            return new UIColor((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static SplatColor FromNative(this UIColor This)
        {
            nfloat r,g,b,a;

            This.GetRGBA(out r, out g, out b, out a);
            return SplatColor.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
        }
    }
#else
    public static class SplatColorExtensions
    {
        public static NSColor ToNative(this SplatColor This)
        {
            return NSColor.FromSrgb((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);
        }

        public static SplatColor FromNative(this NSColor This)
        {
            nfloat r,g,b,a;

            This.GetRgba(out r, out g, out b, out a);
            return SplatColor.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
        }
    }
#endif
}
