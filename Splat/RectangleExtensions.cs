using System;
using System.Drawing;

namespace Splat
{
    public enum RectEdge {
        Left, Top,  // Left / Top
        Right, Bottom,  // Right / Bottom
    }

    public static class RectangleExtensions
    {
        /// <summary>
        /// Determine the center of a Rectangle
        /// </summary>
        public static PointF Center(this RectangleF This)
        {
            return new PointF(This.X + This.Width / 2.0f, This.Y + This.Height / 2.0f);
        }

        /// <summary>
        /// Divide the specified Rectangle into two component rectangles
        /// </summary>
        /// <param name="amount">Amount to move away from the given edge</param>
        /// <param name="fromEdge">The edge to create the slice from.</param>
        public static Tuple<RectangleF, RectangleF> Divide(this RectangleF This, float amount, RectEdge fromEdge)
        {
            var delta = default(float);

            switch (fromEdge) {
            case RectEdge.Left:
                delta = Math.Max(This.Width, amount);
                return Tuple.Create(
                    new RectangleF(This.Left, This.Top, delta, This.Height),
                    new RectangleF(This.Left + delta, This.Top, This.Width - delta, This.Height));
            case RectEdge.Top:
                delta = Math.Max(This.Height, amount);
                return Tuple.Create(
                    new RectangleF(This.Left, This.Top, This.Width, amount),
                    new RectangleF(This.Left, This.Top + delta, This.Width, This.Height - delta));
            case RectEdge.Right:
                delta = Math.Max(This.Width, amount);
                return Tuple.Create(
                    new RectangleF(This.Right - delta, This.Top, delta, This.Height),
                    new RectangleF(This.Left, This.Top, This.Width - delta, This.Height));
            case RectEdge.Bottom:
                delta = Math.Max(This.Height, amount);
                return Tuple.Create(
                    new RectangleF(This.Left, This.Bottom - delta, This.Width, delta),
                    new RectangleF(This.Left, This.Top, This.Width, This.Height - delta));
            default:
                throw new ArgumentException("edge");
            }
        }

        /// <summary>
        /// Vertically inverts the coordinates of the rectangle within containingRect 
        ///
        /// This can effectively be used to change the coordinate system of a rectangle.
        /// For example, if `rect` is defined for a coordinate system starting at the
        /// top-left, the result will be a rectangle relative to the bottom-left.
        /// </summary>
        public static RectangleF InvertWithin(this RectangleF This, RectangleF containingRect)
        {
            return new RectangleF(This.X, containingRect.Height - This.Bottom, This.Width, This.Height);
        }
    }
}