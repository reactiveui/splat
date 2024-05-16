// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Extension methods which extend the point mathematics.
/// </summary>
public static class PointMathExtensions
{
    /// <summary>
    /// Floor the specified point (i.e. round it to integer values).
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <returns>The point that has been floored.</returns>
    public static PointF Floor(this Point value)
        => new((float)Math.Floor((double)value.X), (float)Math.Ceiling((double)value.Y));

    /// <summary>
    /// Determines whether two points are within 'epsilon' of each other.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="other">The point to compare against.</param>
    /// <param name="epsilon">The tolerated epsilon value.</param>
    /// <returns>If the value is equal based on the epsilon.</returns>
    public static bool WithinEpsilonOf(this PointF value, PointF other, float epsilon) => value.DistanceTo(other) < epsilon;

    /// <summary>
    /// Calculates the Dot product of two points.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="other">The point to perform the dot product against.</param>
    /// <returns>The calculated dot product.</returns>
    public static float DotProduct(this PointF value, PointF other) => (value.X * other.X) + (value.Y * other.Y);

    /// <summary>
    /// Scales a PointF by a scalar factor.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="factor">The amount to scale by.</param>
    /// <returns>The scaled point.</returns>
    public static PointF ScaledBy(this PointF value, float factor) => new(value.X * factor, value.Y * factor);

    /// <summary>
    /// Calculates the magnitude of a point from (0,0).
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <returns>The length of the point.</returns>
    public static float Length(this PointF value) => PointF.Empty.DistanceTo(value);

    /// <summary>
    /// Normalize the specified PointF (i.e. makes its magnitude = 1.0f).
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <returns>The normalized point.</returns>
    public static PointF Normalize(this PointF value) => (float)value.Length() switch
    {
        0.0f => value,
        _ => new(value.X / (float)value.Length(), value.Y / (float)value.Length())
    };

    /// <summary>
    /// Calculates the angle in degrees of a PointF.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <returns>The angle that has been generated.</returns>
    public static float AngleInDegrees(this PointF value) => (float)(Math.Atan2(value.Y, value.X) * 180.0f / Math.PI);

    /// <summary>
    /// Projects a PointF along a specified direction.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="direction">The point containing the direction.</param>
    /// <returns>The projected point.</returns>
    public static PointF ProjectAlong(this PointF value, PointF direction)
    {
        var normalDirection = direction.Normalize();
        var dist = value.DotProduct(normalDirection);

        return normalDirection.ScaledBy(dist);
    }

    /// <summary>
    /// Projects a PointF along a specified angle.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="angleInDegrees">The angle in degrees to perform the projection against.</param>
    /// <returns>The point that has been projected.</returns>
    public static PointF ProjectAlongAngle(this PointF value, float angleInDegrees)
    {
        var rads = angleInDegrees * Math.PI / 180.0f;
        var direction = new PointF((float)Math.Cos(rads), (float)Math.Sin(rads));

        return value.ProjectAlong(direction);
    }

    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="value">The point value to use for the calculation.</param>
    /// <param name="other">The other point to generate for.</param>
    /// <returns>The distance to the other point.</returns>
    public static float DistanceTo(this PointF value, PointF other)
    {
        var deltaX = other.X - value.X;
        var deltaY = other.Y - value.Y;
        return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }
}
