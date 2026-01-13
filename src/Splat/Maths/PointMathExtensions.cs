// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Provides extension methods for performing common mathematical operations on Point and PointF structures.
/// </summary>
/// <remarks>These methods enable vector-like operations such as scaling, normalization, dot product, projection,
/// and distance calculations directly on Point and PointF instances. They are intended to simplify geometric
/// computations in graphical or mathematical applications.</remarks>
public static class PointMathExtensions
{
    /// <summary>
    /// Returns a new PointF whose coordinates are the largest integers less than or equal to the corresponding
    /// coordinates of the specified Point.
    /// </summary>
    /// <param name="value">The Point whose coordinates are to be floored.</param>
    /// <returns>A PointF whose X and Y values are the result of applying the floor operation to the X and Y values of the input
    /// Point.</returns>
    public static PointF Floor(this Point value)
        => new((float)Math.Floor((double)value.X), (float)Math.Floor((double)value.Y));

    /// <summary>
    /// Determines whether the distance between two points is less than the specified epsilon value.
    /// </summary>
    /// <param name="value">The point to compare to <paramref name="other"/>.</param>
    /// <param name="other">The point to compare to <paramref name="value"/>.</param>
    /// <param name="epsilon">The maximum allowed distance between the two points for them to be considered within epsilon. Must be
    /// non-negative.</param>
    /// <returns>true if the distance between <paramref name="value"/> and <paramref name="other"/> is less than <paramref
    /// name="epsilon"/>; otherwise, false.</returns>
    public static bool WithinEpsilonOf(this PointF value, PointF other, float epsilon) => value.DistanceTo(other) < epsilon;

    /// <summary>
    /// Calculates the dot product of two 2D points represented by <see cref="PointF"/> instances.
    /// </summary>
    /// <remarks>The dot product is calculated as (value.X * other.X) + (value.Y * other.Y). This operation is
    /// commonly used in vector mathematics to determine the angle or projection between two vectors.</remarks>
    /// <param name="value">The first point to use in the dot product calculation.</param>
    /// <param name="other">The second point to use in the dot product calculation.</param>
    /// <returns>A single-precision floating-point value representing the dot product of the two points.</returns>
    public static float DotProduct(this PointF value, PointF other) => (value.X * other.X) + (value.Y * other.Y);

    /// <summary>
    /// Returns a new PointF whose coordinates are scaled by the specified factor.
    /// </summary>
    /// <param name="value">The PointF to scale.</param>
    /// <param name="factor">The multiplier applied to both the X and Y coordinates of the point.</param>
    /// <returns>A new PointF whose X and Y values are equal to those of the original point multiplied by the specified factor.</returns>
    public static PointF ScaledBy(this PointF value, float factor) => new(value.X * factor, value.Y * factor);

    /// <summary>
    /// Calculates the distance from the origin to the specified point.
    /// </summary>
    /// <param name="value">The point for which to calculate the distance from the origin.</param>
    /// <returns>The distance from the origin (0,0) to the specified point, measured in the same units as the point's
    /// coordinates.</returns>
    public static float Length(this PointF value) => PointF.Empty.DistanceTo(value);

    /// <summary>
    /// Returns a new PointF representing the normalized (unit length) vector of the specified point.
    /// </summary>
    /// <remarks>If the input point has a length of zero, the original point is returned unchanged to avoid
    /// division by zero.</remarks>
    /// <param name="value">The PointF to normalize.</param>
    /// <returns>A PointF with the same direction as the input but with a length of 1.0, or the original point if its length is
    /// zero.</returns>
    public static PointF Normalize(this PointF value) => value.Length() switch
    {
        0.0f => value,
        _ => new(value.X / value.Length(), value.Y / value.Length())
    };

    /// <summary>
    /// Calculates the angle, in degrees, between the positive X-axis and the vector represented by the specified point.
    /// </summary>
    /// <remarks>A positive result indicates a counterclockwise angle from the X-axis, while a negative result
    /// indicates a clockwise angle. If <paramref name="value"/> is (0, 0), the result is 0.</remarks>
    /// <param name="value">The point whose vector angle is to be calculated. The X and Y coordinates represent the vector components.</param>
    /// <returns>The angle, in degrees, between the positive X-axis and the vector from the origin to <paramref name="value"/>.
    /// The result is in the range -180 to 180 degrees.</returns>
    public static float AngleInDegrees(this PointF value) => (float)(Math.Atan2(value.Y, value.X) * 180.0f / Math.PI);

    /// <summary>
    /// Projects the specified point onto the given direction vector, returning the component of the point that lies
    /// along that direction.
    /// </summary>
    /// <remarks>If <paramref name="direction"/> is the zero vector, the result will be a zero vector. The
    /// returned vector has the same direction as <paramref name="direction"/> and a magnitude equal to the component of
    /// <paramref name="value"/> along that direction.</remarks>
    /// <param name="value">The point to be projected onto the direction vector.</param>
    /// <param name="direction">The direction vector along which to project the point. Does not need to be normalized.</param>
    /// <returns>A new <see cref="PointF"/> representing the projection of <paramref name="value"/> onto <paramref
    /// name="direction"/>.</returns>
    public static PointF ProjectAlong(this PointF value, PointF direction)
    {
        var normalDirection = direction.Normalize();
        var dist = value.DotProduct(normalDirection);

        return normalDirection.ScaledBy(dist);
    }

    /// <summary>
    /// Projects the specified point onto a vector defined by the given angle in degrees.
    /// </summary>
    /// <param name="value">The point to project.</param>
    /// <param name="angleInDegrees">The angle, in degrees, specifying the direction of the projection. Measured counterclockwise from the positive
    /// X-axis.</param>
    /// <returns>A new <see cref="PointF"/> representing the projection of the original point along the specified angle.</returns>
    public static PointF ProjectAlongAngle(this PointF value, float angleInDegrees)
    {
        var rads = angleInDegrees * Math.PI / 180.0f;
        var direction = new PointF((float)Math.Cos(rads), (float)Math.Sin(rads));

        return value.ProjectAlong(direction);
    }

    /// <summary>
    /// Calculates the Euclidean distance between two points represented by <see cref="PointF"/> structures.
    /// </summary>
    /// <param name="value">The first point from which to measure the distance.</param>
    /// <param name="other">The second point to which the distance is measured.</param>
    /// <returns>The straight-line distance between <paramref name="value"/> and <paramref name="other"/>, measured in the same
    /// units as the point coordinates.</returns>
    public static float DistanceTo(this PointF value, PointF other)
    {
        var deltaX = other.X - value.X;
        var deltaY = other.Y - value.Y;
        return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }
}
