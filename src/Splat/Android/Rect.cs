using System;
using Android.Graphics;

namespace Splat
{
    public static class RectExtensions
    {
        public static Rect ToNative(this System.Drawing.Rectangle This)
        {
            return new Rect(This.X, This.Y, This.X + This.Width, This.Y + This.Height);
        }

        public static RectF ToNative(this System.Drawing.RectangleF This)
        {
            return new RectF(This.X, This.Y, This.X + This.Width, This.Y + This.Height);
        }

        public static System.Drawing.Rectangle FromNative(this Rect This)
        {
            return new System.Drawing.Rectangle(This.Left, This.Top, This.Width(), This.Height());
        }

        public static System.Drawing.RectangleF FromNative(this RectF This)
        {
            return new System.Drawing.RectangleF(This.Left, This.Top, This.Width(), This.Height());
        }
    }
}