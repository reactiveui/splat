using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Splat
{
    public static class PointExtensions
    {
        public static Point ToNative(this System.Drawing.Point This)
        {
            return new Point(This.X, This.Y);
        }

        public static Point ToNative(this System.Drawing.PointF This)
        {
            return new Point(This.X, This.Y);
        }

        public static Size ToNative(this System.Drawing.Size This)
        {
            return new Size(This.Width, This.Height);
        }

        public static Size ToNative(this System.Drawing.SizeF This)
        {
            return new Size(This.Width, This.Height);
        }

        public static System.Drawing.PointF FromNative(this Point This)
        {
            return new System.Drawing.PointF((float)This.X, (float)This.Y);
        }

        public static System.Drawing.SizeF FromNative(this Size This)
        {
            return new System.Drawing.SizeF((float)This.Width, (float)This.Height);
        }
    }
}