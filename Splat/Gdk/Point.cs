using System;

using Gdk;

namespace Splat
{
    public static class PointExtensions
    {
        public static Point ToNative(this System.Drawing.Point This)
        {
            return new Point(This.X, This.Y);
        }

        public static System.Drawing.Point FromNative(this Point This)
        {
            return new System.Drawing.Point(This.X, This.Y);
        }
    }
}
