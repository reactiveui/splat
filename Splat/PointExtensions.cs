using System;
using System.Drawing;

namespace Splat
{
    public static class PointMathExtensions
    {
        /// <summary>
        /// Floor the specified point (i.e. round it to integer values)
        /// </summary>
        public static PointF Floor(this Point This)
        {
#if UIKIT
            // NB: I have no idea but Archimedes does this, soooooooo....
            return new PointF((float)Math.Floor((double)This.X), (float)Math.Floor((double)This.Y));
#else
            return new PointF((float)Math.Floor((double)This.X), (float)Math.Ceiling((double)This.Y));
#endif
        }

        /// <summary>
        /// Determines whether two points are within 'epsilon' of each other
        /// </summary>
        public static bool WithinEpsilonOf(this PointF This, PointF other, float epsilon)
        {
            return This.DistanceTo(other) < epsilon;
        }

        /// <summary>
        /// Calculates the Dot product of two points
        /// </summary>
        public static float DotProduct(this PointF This, PointF other)
        {
            return (This.X * other.X + This.Y * other.Y);
        }

        /// <summary>
        /// Scales a PointF by a scalar factor
        /// </summary>
        public static PointF ScaledBy(this PointF This, float factor)
        {
            return new PointF(This.X * factor, This.Y * factor);
        }

        /// <summary>
        /// Calculates the magnitude of a point from (0,0)
        /// </summary>
        public static float Length(this PointF This)
        {
            return PointF.Empty.DistanceTo(This);
        }

        /// <summary>
        /// Normalize the specified PointF (i.e. makes its magnitude = 1.0f)
        /// </summary>
        public static PointF Normalize(this PointF This)
        {
            var length = This.Length();
            if (length == 0.0f) return This;

            return new PointF(This.X / length, This.Y / length);
        }

        /// <summary>
        /// Calculates the angle in degrees of a PointF
        /// </summary>
        public static float AngleInDegrees(this PointF This)
        {
            return (float)(Math.Atan2(This.Y, This.X) * 180.0f / Math.PI);
        }

        /// <summary>
        /// Projects a PointF along a specified direction
        /// </summary>
        public static PointF ProjectAlong(this PointF This, PointF direction)
        {
            var normalDirection = direction.Normalize();
            var dist = This.DotProduct(normalDirection);

            return normalDirection.ScaledBy(dist);
        }
                
        /// <summary>
        /// Projects a PointF along a specified angle
        /// </summary>
        public static PointF ProjectAlongAngle(this PointF This, float angleInDegrees)
        {
            var rads = angleInDegrees * Math.PI / 180.0f;
            var direction = new PointF((float)Math.Cos(rads), (float)Math.Sin(rads));

            return This.ProjectAlong(direction);
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        public static float DistanceTo(this PointF This, PointF other)
        {
            var deltaX = other.X - This.X;
            var deltaY = other.Y - This.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}

