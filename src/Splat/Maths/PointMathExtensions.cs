// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>Provides extension methods for performing common mathematical operations on Point and PointF structures.</summary>
/// <remarks>These methods enable vector-like operations such as scaling, normalization, dot product, projection,
/// and distance calculations directly on Point and PointF instances. They are intended to simplify geometric
/// computations in graphical or mathematical applications.</remarks>
public static class PointMathExtensions
{
    /// <summary>The number of degrees in half a turn, used to convert radians to degrees (degrees = radians * <see cref="DegreesPerHalfTurn"/> / pi).</summary>
    private const float DegreesPerHalfTurn = 180.0f;

    /// <summary>Extension members for mathematical operations on <see cref="Point"/>.</summary>
    /// <param name="value">The point the extension members operate on.</param>
    extension(Point value)
    {
        /// <summary>
        /// Returns a new PointF whose coordinates are the largest integers less than or equal to the corresponding
        /// coordinates of the specified Point.
        /// </summary>
        /// <returns>A PointF whose X and Y values are the result of applying the floor operation to the X and Y values of the input
        /// Point.</returns>
        public PointF Floor()
            => new((float)Math.Floor((double)value.X), (float)Math.Floor((double)value.Y));
    }

    /// <summary>Extension members for mathematical operations on <see cref="PointF"/>.</summary>
    /// <param name="value">The point the extension members operate on.</param>
    extension(PointF value)
    {
        /// <summary>Determines whether the distance between two points is less than the specified epsilon value.</summary>
        /// <param name="other">The point to compare to this point.</param>
        /// <param name="epsilon">The maximum allowed distance between the two points for them to be considered within epsilon. Must be
        /// non-negative.</param>
        /// <returns>true if the distance between this point and <paramref name="other"/> is less than <paramref
        /// name="epsilon"/>; otherwise, false.</returns>
        public bool WithinEpsilonOf(PointF other, float epsilon) => value.DistanceTo(other) < epsilon;

        /// <summary>Calculates the dot product of two 2D points represented by <see cref="PointF"/> instances.</summary>
        /// <remarks>The dot product is calculated as (value.X * other.X) + (value.Y * other.Y). This operation is
        /// commonly used in vector mathematics to determine the angle or projection between two vectors.</remarks>
        /// <param name="other">The second point to use in the dot product calculation.</param>
        /// <returns>A single-precision floating-point value representing the dot product of the two points.</returns>
        public float DotProduct(PointF other) => (value.X * other.X) + (value.Y * other.Y);

        /// <summary>Returns a new PointF whose coordinates are scaled by the specified factor.</summary>
        /// <param name="factor">The multiplier applied to both the X and Y coordinates of the point.</param>
        /// <returns>A new PointF whose X and Y values are equal to those of the original point multiplied by the specified factor.</returns>
        public PointF ScaledBy(float factor) => new(value.X * factor, value.Y * factor);

        /// <summary>Calculates the distance from the origin to the specified point.</summary>
        /// <returns>The distance from the origin (0,0) to the specified point, measured in the same units as the point's
        /// coordinates.</returns>
        public float Length() => PointF.Empty.DistanceTo(value);

        /// <summary>Returns a new PointF representing the normalized (unit length) vector of the specified point.</summary>
        /// <remarks>If the input point has a length of zero, the original point is returned unchanged to avoid
        /// division by zero.</remarks>
        /// <returns>A PointF with the same direction as the input but with a length of 1.0, or the original point if its length is
        /// zero.</returns>
        public PointF Normalize() => value.Length() switch
        {
            0.0f => value,
            _ => new(value.X / value.Length(), value.Y / value.Length())
        };

        /// <summary>Calculates the angle, in degrees, between the positive X-axis and the vector represented by the specified point.</summary>
        /// <remarks>A positive result indicates a counterclockwise angle from the X-axis, while a negative result
        /// indicates a clockwise angle. If this point is (0, 0), the result is 0.</remarks>
        /// <returns>The angle, in degrees, between the positive X-axis and the vector from the origin to this point.
        /// The result is in the range -180 to 180 degrees.</returns>
        public float AngleInDegrees() => (float)(Math.Atan2(value.Y, value.X) * DegreesPerHalfTurn / Math.PI);

        /// <summary>
        /// Projects the specified point onto the given direction vector, returning the component of the point that lies
        /// along that direction.
        /// </summary>
        /// <remarks>If <paramref name="direction"/> is the zero vector, the result will be a zero vector. The
        /// returned vector has the same direction as <paramref name="direction"/> and a magnitude equal to the component of
        /// this point along that direction.</remarks>
        /// <param name="direction">The direction vector along which to project the point. Does not need to be normalized.</param>
        /// <returns>A new <see cref="PointF"/> representing the projection of this point onto <paramref
        /// name="direction"/>.</returns>
        public PointF ProjectAlong(PointF direction)
        {
            var normalDirection = direction.Normalize();
            var dist = value.DotProduct(normalDirection);

            return normalDirection.ScaledBy(dist);
        }

        /// <summary>Projects the specified point onto a vector defined by the given angle in degrees.</summary>
        /// <param name="angleInDegrees">The angle, in degrees, specifying the direction of the projection. Measured counterclockwise from the positive
        /// X-axis.</param>
        /// <returns>A new <see cref="PointF"/> representing the projection of the original point along the specified angle.</returns>
        public PointF ProjectAlongAngle(float angleInDegrees)
        {
            var rads = angleInDegrees * Math.PI / 180.0f;
            var direction = new PointF((float)Math.Cos(rads), (float)Math.Sin(rads));

            return value.ProjectAlong(direction);
        }

        /// <summary>Calculates the Euclidean distance between two points represented by <see cref="PointF"/> structures.</summary>
        /// <param name="other">The second point to which the distance is measured.</param>
        /// <returns>The straight-line distance between this point and <paramref name="other"/>, measured in the same
        /// units as the point coordinates.</returns>
        public float DistanceTo(PointF other)
        {
            var deltaX = other.X - value.X;
            var deltaY = other.Y - value.Y;
            return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        }
    }
}
