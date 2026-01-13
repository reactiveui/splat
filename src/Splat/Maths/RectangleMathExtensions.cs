// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Provides extension methods for performing advanced mathematical and geometric operations on <see cref="RectangleF"/>
/// instances.
/// </summary>
/// <remarks>This static class includes methods for calculating the center point, dividing rectangles (with or
/// without padding), inverting rectangles within a containing rectangle, and creating modified copies of rectangles.
/// These utilities are intended to simplify common rectangle manipulations in graphical and layout scenarios.</remarks>
public static class RectangleMathExtensions
{
    /// <summary>
    /// Calculates the center point of the specified rectangle.
    /// </summary>
    /// <param name="value">The rectangle for which to determine the center point.</param>
    /// <returns>A <see cref="PointF"/> representing the center of the specified rectangle.</returns>
    public static PointF Center(this RectangleF value) => new(value.X + (value.Width / 2.0f), value.Y + (value.Height / 2.0f));

    /// <summary>
    /// Divides the specified rectangle into two rectangles by splitting off a region of the given size from one edge.
    /// </summary>
    /// <remarks>If the specified amount is zero, the first rectangle will have zero width or height,
    /// depending on the edge, and the second rectangle will be equal to the original. If the amount equals the
    /// rectangle's width or height (as appropriate), the second rectangle will have zero width or height. Throws an
    /// exception if the amount is negative or exceeds the rectangle's dimension along the specified edge.</remarks>
    /// <param name="value">The rectangle to divide.</param>
    /// <param name="amount">The size, in the same units as the rectangle, of the region to split from the specified edge. Must be
    /// non-negative and less than or equal to the corresponding dimension of the rectangle.</param>
    /// <param name="fromEdge">The edge from which to split the region. Specifies which side of the rectangle the split is performed from.</param>
    /// <returns>A tuple containing two rectangles: the first is the region split from the specified edge with the given size,
    /// and the second is the remainder of the original rectangle. The sum of their areas equals the area of the
    /// original rectangle.</returns>
    public static Tuple<RectangleF, RectangleF> Divide(this RectangleF value, float amount, RectEdge fromEdge)
    {
        switch (fromEdge)
        {
            case RectEdge.Left:
                return Tuple.Create(
                    value.Copy(width: amount),
                    value.Copy(x: value.Left + amount, width: value.Width - amount));
            case RectEdge.Top:
                return Tuple.Create(
                    value.Copy(height: amount),
                    value.Copy(y: value.Top + amount, height: value.Height - amount));
            case RectEdge.Right:
                return Tuple.Create(
                    value.Copy(x: value.Right - amount, width: amount),
                    value.Copy(width: value.Width - amount));
            case RectEdge.Bottom:
                return Tuple.Create(
                    value.Copy(y: value.Bottom - amount, height: amount),
                    value.Copy(height: value.Height - amount));
            default:
                ArgumentExceptionHelper.ThrowIf(true, $"Invalid edge: {fromEdge}", nameof(fromEdge));
                return null!; // unreachable
        }
    }

    /// <summary>
    /// Divides the specified rectangle into two regions along the given edge, separating them by a specified padding.
    /// </summary>
    /// <remarks>If the sum of sliceAmount and padding exceeds the corresponding dimension of the rectangle,
    /// the resulting rectangles may have zero or negative size. The method does not modify the original
    /// rectangle.</remarks>
    /// <param name="value">The rectangle to be divided.</param>
    /// <param name="sliceAmount">The size, in the same units as the rectangle, of the first region to slice from the specified edge. Must be
    /// non-negative.</param>
    /// <param name="padding">The size, in the same units as the rectangle, of the padding to insert between the two resulting regions. Must
    /// be non-negative.</param>
    /// <param name="fromEdge">The edge of the rectangle from which to perform the division.</param>
    /// <returns>A tuple containing two rectangles: the first is the sliced region, and the second is the remaining region after
    /// the slice and padding have been removed.</returns>
    public static Tuple<RectangleF, RectangleF> DivideWithPadding(this RectangleF value, float sliceAmount, float padding, RectEdge fromEdge)
    {
        var slice = value.Divide(sliceAmount, fromEdge);
        var paddingRect = value.Divide(padding, fromEdge);
        return Tuple.Create(slice.Item1, paddingRect.Item2);
    }

    /// <summary>
    /// Returns a new rectangle that is vertically inverted within the specified containing rectangle.
    /// </summary>
    /// <remarks>The method inverts the Y-coordinate of the rectangle so that its position is mirrored
    /// vertically within the containing rectangle. This is commonly used to convert between coordinate systems with
    /// different vertical origins.</remarks>
    /// <param name="value">The rectangle to invert within the containing rectangle.</param>
    /// <param name="containingRect">The rectangle that defines the bounds within which to invert the position of <paramref name="value"/>.</param>
    /// <returns>A <see cref="RectangleF"/> representing the vertically inverted position of <paramref name="value"/> within
    /// <paramref name="containingRect"/>. The size of the rectangle remains unchanged.</returns>
    public static RectangleF InvertWithin(this RectangleF value, RectangleF containingRect) =>
        value with { Y = containingRect.Height - value.Bottom };

    /// <summary>
    /// Creates a copy of the specified rectangle, optionally overriding one or more of its position or size components.
    /// </summary>
    /// <remarks>If both <paramref name="y"/> and <paramref name="top"/> are specified, or both <paramref
    /// name="height"/> and <paramref name="bottom"/> are specified, an exception is thrown due to conflicting
    /// arguments.</remarks>
    /// <param name="value">The rectangle to copy.</param>
    /// <param name="x">The value to use for the X coordinate of the new rectangle. If null, the original X value is used. Cannot be
    /// specified together with <paramref name="top"/>.</param>
    /// <param name="y">The value to use for the Y coordinate of the new rectangle. If null, the original Y value is used. Cannot be
    /// specified together with <paramref name="top"/>.</param>
    /// <param name="width">The value to use for the width of the new rectangle. If null, the original width is used. Cannot be specified
    /// together with <paramref name="bottom"/>.</param>
    /// <param name="height">The value to use for the height of the new rectangle. If null, the original height is used. Cannot be specified
    /// together with <paramref name="bottom"/>.</param>
    /// <param name="top">The value to use for the top (Y coordinate) of the new rectangle. If specified, <paramref name="y"/> must be
    /// null.</param>
    /// <param name="bottom">The value to use for the bottom edge of the new rectangle, relative to the Y coordinate. If specified, <paramref
    /// name="height"/> must be null.</param>
    /// <returns>A new <see cref="RectangleF"/> instance with the specified components replaced as provided.</returns>
    public static RectangleF Copy(
        this RectangleF value,
        float? x = null,
        float? y = null,
        float? width = null,
        float? height = null,
        float? top = null,
        float? bottom = null)
    {
        var newRect = new RectangleF(value.Location, value.Size);

        if (x.HasValue)
        {
            newRect.X = x.Value;
        }

        if (y.HasValue)
        {
            newRect.Y = y.Value;
        }

        if (width.HasValue)
        {
            newRect.Width = width.Value;
        }

        if (height.HasValue)
        {
            newRect.Height = height.Value;
        }

        if (top.HasValue)
        {
            ArgumentExceptionHelper.ThrowIf(y.HasValue, "Conflicting Copy arguments Y and Top");

            newRect.Y = top.Value;
        }

        if (bottom.HasValue)
        {
            ArgumentExceptionHelper.ThrowIf(height.HasValue, "Conflicting Copy arguments Height and Bottom");

            newRect.Height = newRect.Y + bottom.Value;
        }

        return newRect;
    }
}
