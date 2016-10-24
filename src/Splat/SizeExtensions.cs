using System;
using System.Drawing;

namespace Splat
{
    public static class SizeMathExtensions
    {
        /// <summary>
        /// Determines whether two sizes are within epsilon of each other
        /// </summary>
        public static bool WithinEpsilonOf(this SizeF This, SizeF other, float epsilon)
        {
            var deltaW = other.Width - This.Width;
            var deltaH = other.Height - This.Height;
            return Math.Sqrt(deltaW * deltaW + deltaH * deltaH) < epsilon;
        }

        /// <summary>
        /// Scales a size by a scalar value
        /// </summary>
        public static SizeF ScaledBy(this SizeF This, float factor)
        {
            return new SizeF(This.Width * factor, This.Height * factor);
        }
    }
}