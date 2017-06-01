using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Splat
{
    public static class RectExtensions
    {
        public static Rect ToNative(this System.Drawing.Rectangle This)
        {
            return new Rect(This.X, This.Y, This.Width, This.Height);
        }

        public static Rect ToNative(this System.Drawing.RectangleF This)
        {
            return new Rect(This.X, This.Y, This.Width, This.Height);
        }

        public static System.Drawing.RectangleF FromNative(this Rect This)
        {
            return new System.Drawing.RectangleF((float)This.X, (float)This.Y, (float)This.Width, (float)This.Height);
        }
    }
}
