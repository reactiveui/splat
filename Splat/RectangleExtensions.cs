using System;
using System.Drawing;

namespace Splat
{
    public enum RectEdge {
        Left, Top,  // Left / Top
        Right, Bottom,  // Right / Bottom
    }

    public static class RectangleMathExtensions
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
                    This.Copy(Width: delta),
                    This.Copy(X: This.Left + delta, Width: This.Width - delta));
            case RectEdge.Top:
                delta = Math.Max(This.Height, amount);
                return Tuple.Create(
                    This.Copy(Height: amount),
                    This.Copy(Y: This.Top + delta, Height: This.Height - delta));
            case RectEdge.Right:
                delta = Math.Max(This.Width, amount);
                return Tuple.Create(
                    This.Copy(X: This.Right - delta, Width: delta),
                    This.Copy(Width: This.Width - delta));
            case RectEdge.Bottom:
                delta = Math.Max(This.Height, amount);
                return Tuple.Create(
                    This.Copy(Y: This.Bottom - delta, Height: delta),
                    This.Copy(Height: This.Height - delta));
            default:
                throw new ArgumentException("edge");
            }
        }

        /// <summary>
        /// Divide the specified Rectangle into two component rectangles, adding 
        /// a padding between them.
        /// </summary>
        /// <param name="amount">Amount to move away from the given edge</param>
        /// <param name="padding">The amount of padding that is in neither rectangle.</param>
        /// <param name="fromEdge">The edge to create the slice from.</param>
        public static Tuple<RectangleF, RectangleF> DivideWithPadding(this RectangleF This, float sliceAmount, float padding, RectEdge fromEdge)
        {
            var slice = This.Divide(sliceAmount, fromEdge);
            var pad = This.Divide(padding, fromEdge);
            return Tuple.Create(slice.Item1, pad.Item2);
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

        /// <summary>
        /// Creates a new RectangleF as a Copy of an existing one 
        ///
        /// This is useful when you have a rectangle that is almost what you
        /// want, but you just want to change a couple properties.
        /// </summary>
        public static RectangleF Copy(this RectangleF rect,
            float? X = null, float? Y = null,
            float? Width = null, float? Height = null,
            float? Top = null, float? Bottom = null)
        {
            var newRect = new RectangleF(rect.Location, rect.Size);

            if (X.HasValue) {
                newRect.X = X.Value;
            }
            if (Y.HasValue) {
                newRect.Y = Y.Value;
            }

            if (Width.HasValue) {
                newRect.Width = Width.Value;
            }
            if (Height.HasValue) {
                newRect.Height = Height.Value;
            }
            if (Top.HasValue) {
                if (Y.HasValue) {
                    throw new ArgumentException("Conflicting Copy arguments Y and Top");
                }
                newRect.Y = Top.Value;
            }
            if (Bottom.HasValue) {
                if (Height.HasValue) {
                    throw new ArgumentException("Conflicting Copy arguments Height and Bottom");
                }
                newRect.Height = newRect.Y + Bottom.Value;
            }

            return newRect;
        }
    }
}
