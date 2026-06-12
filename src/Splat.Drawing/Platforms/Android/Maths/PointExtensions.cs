// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>Provides extension methods for converting between System.Drawing point types and Android native point types.</summary>
/// <remarks>These methods enable seamless conversion between System.Drawing.Point and System.Drawing.PointF types
/// and their Android equivalents (Point and PointF). This is useful when interoperating between .NET drawing APIs and
/// Android graphics APIs in cross-platform applications.</remarks>
public static class PointExtensions
{
    /// <summary>Extension members for <see cref="System.Drawing.Point"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.Point value)
    {
        /// <summary>Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.</summary>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public Point ToNative() => new(value.X, value.Y);
    }

    /// <summary>Extension members for <see cref="System.Drawing.PointF"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.PointF value)
    {
        /// <summary>Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="PointF"/>.</summary>
        /// <returns>A <see cref="PointF"/> of the value.</returns>
        public PointF ToNative() => new(value.X, value.Y);
    }

    /// <summary>Extension members for <see cref="Point"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Point value)
    {
        /// <summary>Converts a <see cref="Point"/> to a <see cref="System.Drawing.Point"/>.</summary>
        /// <returns>A <see cref="System.Drawing.Point"/> of the value.</returns>
        public System.Drawing.Point FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new(value.X, value.Y);
        }
    }

    /// <summary>Extension members for <see cref="PointF"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(PointF value)
    {
        /// <summary>Converts a <see cref="PointF"/> to a <see cref="System.Drawing.PointF"/>.</summary>
        /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
        public System.Drawing.PointF FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new(value.X, value.Y);
        }
    }
}
