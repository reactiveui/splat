// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>Provides extension methods for performing advanced mathematical and geometric operations on <see cref="RectangleF"/> instances.</summary>
/// <remarks>This static class includes methods for calculating the center point, dividing rectangles (with or
/// without padding), inverting rectangles within a containing rectangle, and creating modified copies of rectangles.
/// These utilities are intended to simplify common rectangle manipulations in graphical and layout scenarios.</remarks>
public static class RectangleMathExtensions
{
    /// <summary>The divisor used to obtain the midpoint of a dimension when locating a rectangle's center.</summary>
    private const float CenterDivisor = 2.0f;

    /// <summary>Extension members for mathematical and geometric operations on <see cref="RectangleF"/>.</summary>
    /// <param name="value">The rectangle the extension members operate on.</param>
    extension(RectangleF value)
    {
        /// <summary>Calculates the center point of the specified rectangle.</summary>
        /// <returns>A <see cref="PointF"/> representing the center of the specified rectangle.</returns>
        public PointF Center() => new(value.X + (value.Width / CenterDivisor), value.Y + (value.Height / CenterDivisor));

        /// <summary>Divides the specified rectangle into two rectangles by splitting off a region of the given size from one edge.</summary>
        /// <remarks>If the specified amount is zero, the first rectangle will have zero width or height,
        /// depending on the edge, and the second rectangle will be equal to the original. If the amount equals the
        /// rectangle's width or height (as appropriate), the second rectangle will have zero width or height. Throws an
        /// exception if the amount is negative or exceeds the rectangle's dimension along the specified edge.</remarks>
        /// <param name="amount">The size, in the same units as the rectangle, of the region to split from the specified edge. Must be
        /// non-negative and less than or equal to the corresponding dimension of the rectangle.</param>
        /// <param name="fromEdge">The edge from which to split the region. Specifies which side of the rectangle the split is performed from.</param>
        /// <returns>A tuple containing two rectangles: the first is the region split from the specified edge with the given size,
        /// and the second is the remainder of the original rectangle. The sum of their areas equals the area of the
        /// original rectangle.</returns>
        public Tuple<RectangleF, RectangleF> Divide(float amount, RectEdge fromEdge)
        {
            switch (fromEdge)
            {
                case RectEdge.Left:
                    return Tuple.Create(
                        value.Copy(new() { Width = amount }),
                        value.Copy(new() { X = value.Left + amount, Width = value.Width - amount }));
                case RectEdge.Top:
                    return Tuple.Create(
                        value.Copy(new() { Height = amount }),
                        value.Copy(new() { Y = value.Top + amount, Height = value.Height - amount }));
                case RectEdge.Right:
                    return Tuple.Create(
                        value.Copy(new() { X = value.Right - amount, Width = amount }),
                        value.Copy(new() { Width = value.Width - amount }));
                case RectEdge.Bottom:
                    return Tuple.Create(
                        value.Copy(new() { Y = value.Bottom - amount, Height = amount }),
                        value.Copy(new() { Height = value.Height - amount }));
                default:
                    throw new ArgumentException($"Invalid edge: {fromEdge}", nameof(fromEdge));
            }
        }

        /// <summary>Divides the specified rectangle into two regions along the given edge, separating them by a specified padding.</summary>
        /// <remarks>If the sum of sliceAmount and padding exceeds the corresponding dimension of the rectangle,
        /// the resulting rectangles may have zero or negative size. The method does not modify the original
        /// rectangle.</remarks>
        /// <param name="sliceAmount">The size, in the same units as the rectangle, of the first region to slice from the specified edge. Must be
        /// non-negative.</param>
        /// <param name="padding">The size, in the same units as the rectangle, of the padding to insert between the two resulting regions. Must
        /// be non-negative.</param>
        /// <param name="fromEdge">The edge of the rectangle from which to perform the division.</param>
        /// <returns>A tuple containing two rectangles: the first is the sliced region, and the second is the remaining region after
        /// the slice and padding have been removed.</returns>
        public Tuple<RectangleF, RectangleF> DivideWithPadding(float sliceAmount, float padding, RectEdge fromEdge)
        {
            var slice = value.Divide(sliceAmount, fromEdge);
            var paddingRect = value.Divide(padding, fromEdge);
            return Tuple.Create(slice.Item1, paddingRect.Item2);
        }

        /// <summary>Returns a new rectangle that is vertically inverted within the specified containing rectangle.</summary>
        /// <remarks>The method inverts the Y-coordinate of the rectangle so that its position is mirrored
        /// vertically within the containing rectangle. This is commonly used to convert between coordinate systems with
        /// different vertical origins.</remarks>
        /// <param name="containingRect">The rectangle that defines the bounds within which to invert the position of this rectangle.</param>
        /// <returns>A <see cref="RectangleF"/> representing the vertically inverted position of this rectangle within
        /// <paramref name="containingRect"/>. The size of the rectangle remains unchanged.</returns>
        public RectangleF InvertWithin(RectangleF containingRect) =>
            value with { Y = containingRect.Height - value.Bottom };

        /// <summary>Creates a copy of the specified rectangle, optionally overriding one or more of its position or size components.</summary>
        /// <remarks>If both <see cref="RectangleCopyOptions.Y"/> and <see cref="RectangleCopyOptions.Top"/> are specified, or both
        /// <see cref="RectangleCopyOptions.Height"/> and <see cref="RectangleCopyOptions.Bottom"/> are specified, an exception is thrown
        /// due to conflicting overrides.</remarks>
        /// <param name="options">The set of component overrides to apply. Any component left <see langword="null"/> keeps the original value.</param>
        /// <returns>A new <see cref="RectangleF"/> instance with the specified components replaced as provided.</returns>
        public RectangleF Copy(RectangleCopyOptions options)
        {
            var newRect = new RectangleF(value.Location, value.Size);

            if (options.X.HasValue)
            {
                newRect.X = options.X.Value;
            }

            if (options.Y.HasValue)
            {
                newRect.Y = options.Y.Value;
            }

            if (options.Width.HasValue)
            {
                newRect.Width = options.Width.Value;
            }

            if (options.Height.HasValue)
            {
                newRect.Height = options.Height.Value;
            }

            if (options.Top.HasValue)
            {
                ArgumentGuard.ThrowIf(options.Y.HasValue, "Conflicting Copy arguments Y and Top");

                newRect.Y = options.Top.Value;
            }

            if (options.Bottom.HasValue)
            {
                ArgumentGuard.ThrowIf(options.Height.HasValue, "Conflicting Copy arguments Height and Bottom");

                newRect.Height = newRect.Y + options.Bottom.Value;
            }

            return newRect;
        }
    }
}
