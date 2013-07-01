using System;
using Android.Graphics;

namespace Splat
{
    public static class PointExtensions
    {
        public static Point ToNative(this System.Drawing.Point This)
        {
            return new Point(This.X, This.Y);
        }

        public static PointF ToNative(this System.Drawing.PointF This)
        {
            return new PointF(This.X, This.Y);
        }

        public static System.Drawing.Point FromNative(this Point This)
        {
            return new System.Drawing.Point(This.X, This.Y);
        }

        public static System.Drawing.PointF FromNative(this PointF This)
        {
            return new System.Drawing.PointF(This.X, This.Y);
        }
    }
}