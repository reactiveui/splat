using System;

using Gdk;

namespace Splat
{
    public static class RectangleExtensions
    {
        public static Rectangle ToNative(this System.Drawing.Rectangle This)
        {
            return new Rectangle(This.X, This.Y, This.Width, This.Height);
        }

        public static System.Drawing.Rectangle FromNative(this Rectangle This)
        {
            return new System.Drawing.Rectangle(This.X, This.Y, This.Width, This.Height);
        }
    }
}
