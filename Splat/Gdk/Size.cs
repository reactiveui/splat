using System;

using Gdk;

namespace Splat
{
    public static class SizeExtensions
    {
        public static Size ToNative(this System.Drawing.Size This)
        {
            return new Size(This.Width, This.Height);
        }

        public static System.Drawing.Size FromNative(this Size This)
        {
            return new System.Drawing.Size(This.Width, This.Height);
        }
    }
}
