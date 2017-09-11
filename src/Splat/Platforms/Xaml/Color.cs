using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Splat
{
    public static class ColorExtensions
    {
        public static Color ToNative(this System.Drawing.Color This)
        {
            return Color.FromArgb(This.A, This.R, This.G, This.B);
        }

        public static SolidColorBrush ToNativeBrush(this System.Drawing.Color This)
        {
            var ret = new SolidColorBrush(This.ToNative());
            ret.Freeze();
            return ret;
        }

        public static System.Drawing.Color FromNative(this Color This)
        {
            return System.Drawing.Color.FromArgb(This.A, This.R, This.G, This.B);
        }
    }
}
