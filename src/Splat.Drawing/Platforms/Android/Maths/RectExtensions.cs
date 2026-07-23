// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>Provides extension methods for converting between <see cref="System.Drawing.Rectangle"/> types and Android native rectangle types.</summary>
/// <remarks>These methods enable seamless interoperability between .NET drawing rectangles and Android's native
/// Rect and RectF structures. All methods are static and intended to be used as extension methods for convenient syntax
/// when working with rectangle conversions in cross-platform scenarios.</remarks>
public static class RectExtensions
{
    /// <summary>Extension members for <see cref="Rect"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Rect value)
    {
        /// <summary>Converts a <see cref="Rect"/> to a <see cref="System.Drawing.Rectangle"/>.</summary>
        /// <returns>A <see cref="System.Drawing.Rectangle"/> of the value.</returns>
        public System.Drawing.Rectangle FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new(value.Left, value.Top, value.Width(), value.Height());
        }
    }

    /// <summary>Extension members for <see cref="RectF"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(RectF value)
    {
        /// <summary>Converts a <see cref="RectF"/> to a <see cref="System.Drawing.RectangleF"/>.</summary>
        /// <returns>A <see cref="System.Drawing.RectangleF"/> of the value.</returns>
        public System.Drawing.RectangleF FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new(value.Left, value.Top, value.Width(), value.Height());
        }
    }

    /// <summary>Extension members for <see cref="System.Drawing.Rectangle"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.Rectangle value)
    {
        /// <summary>Convert a <see cref="System.Drawing.Rectangle"/> to the android native <see cref="Rect"/>.</summary>
        /// <returns>A <see cref="Rect"/> of the value.</returns>
        public Rect ToNative() => new(value.X, value.Y, value.X + value.Width, value.Y + value.Height);
    }

    /// <summary>Extension members for <see cref="System.Drawing.RectangleF"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.RectangleF value)
    {
        /// <summary>Convert a <see cref="System.Drawing.RectangleF"/> to the android native <see cref="RectF"/>.</summary>
        /// <returns>A <see cref="RectF"/> of the value.</returns>
        public RectF ToNative() => new(value.X, value.Y, value.X + value.Width, value.Y + value.Height);
    }
}
