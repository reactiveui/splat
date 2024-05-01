// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Extension methods to help with operations associated with the <see cref="RectangleF"/> struct.
/// </summary>
public static class RectangleMathExtensions
{
    /// <summary>
    /// Determine the center of a Rectangle.
    /// </summary>
    /// <param name="value">The rectangle to perform the calculation against.</param>
    /// <returns>The point of the center of the rectangle.</returns>
    public static PointF Center(this RectangleF value) => new(value.X + (value.Width / 2.0f), value.Y + (value.Height / 2.0f));

    /// <summary>
    /// Divide the specified Rectangle into two component rectangles.
    /// </summary>
    /// <param name="value">The rectangle to perform the calculation against.</param>
    /// <param name="amount">Amount to move away from the given edge.</param>
    /// <param name="fromEdge">The edge to create the slice from.</param>
    /// <returns>The set of rectnagles that are generated.</returns>
    public static Tuple<RectangleF, RectangleF> Divide(this RectangleF value, float amount, RectEdge fromEdge)
    {
        float delta;
        switch (fromEdge)
        {
        case RectEdge.Left:
            delta = Math.Max(value.Width, amount);
            return Tuple.Create(
                value.Copy(width: delta),
                value.Copy(x: value.Left + delta, width: value.Width - delta));
        case RectEdge.Top:
            delta = Math.Max(value.Height, amount);
            return Tuple.Create(
                value.Copy(height: amount),
                value.Copy(y: value.Top + delta, height: value.Height - delta));
        case RectEdge.Right:
            delta = Math.Max(value.Width, amount);
            return Tuple.Create(
                value.Copy(x: value.Right - delta, width: delta),
                value.Copy(width: value.Width - delta));
        case RectEdge.Bottom:
            delta = Math.Max(value.Height, amount);
            return Tuple.Create(
                value.Copy(y: value.Bottom - delta, height: delta),
                value.Copy(height: value.Height - delta));
        default:
            throw new ArgumentException("edge");
        }
    }

    /// <summary>
    /// Divide the specified Rectangle into two component rectangles, adding
    /// a padding between them.
    /// </summary>
    /// <param name="value">The rectangle to perform the calculation against.</param>
    /// <param name="sliceAmount">Amount to move away from the given edge.</param>
    /// <param name="padding">The amount of padding that is in neither rectangle.</param>
    /// <param name="fromEdge">The edge to create the slice from.</param>
    /// <returns>The set of rectangles that are generated.</returns>
    public static Tuple<RectangleF, RectangleF> DivideWithPadding(this RectangleF value, float sliceAmount, float padding, RectEdge fromEdge)
    {
        var slice = value.Divide(sliceAmount, fromEdge);
        var pad = value.Divide(padding, fromEdge);
        return Tuple.Create(slice.Item1, pad.Item2);
    }

    /// <summary>
    /// <para>Vertically inverts the coordinates of the rectangle within containingRect.</para>
    /// <para>
    /// value can effectively be used to change the coordinate system of a rectangle.
    /// For example, if `rect` is defined for a coordinate system starting at the
    /// top-left, the result will be a rectangle relative to the bottom-left.
    /// </para>
    /// </summary>
    /// <param name="value">The rectangle to perform the calculation against.</param>
    /// <param name="containingRect">The containing rectangle.</param>
    /// <returns>The inverted rectangle.</returns>
    public static RectangleF InvertWithin(this RectangleF value, RectangleF containingRect) =>
        value with { Y = containingRect.Height - value.Bottom };

    /// <summary>
    /// <para>Creates a new RectangleF as a Copy of an existing one .</para>
    /// <para>
    /// value is useful when you have a rectangle that is almost what you
    /// want, but you just want to change a couple properties.
    /// </para>
    /// </summary>
    /// <param name="value">The rectangle to perform the calculation against.</param>
    /// <param name="x">Optional new x coordinate of the rectangle to use.</param>
    /// <param name="y">Optional new y coordinate of the rectangle to use.</param>
    /// <param name="width">Optional new width of the rectangle to use.</param>
    /// <param name="height">Optional new height of the rectangle to use.</param>
    /// <param name="top">Optional new top of the rectangle to use.</param>
    /// <param name="bottom">Optional new bottom of the rectangle to use.</param>
    /// <returns>The copied rectangle.</returns>
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
            if (y.HasValue)
            {
                throw new ArgumentException("Conflicting Copy arguments Y and Top");
            }

            newRect.Y = top.Value;
        }

        if (bottom.HasValue)
        {
            if (height.HasValue)
            {
                throw new ArgumentException("Conflicting Copy arguments Height and Bottom");
            }

            newRect.Height = newRect.Y + bottom.Value;
        }

        return newRect;
    }
}
