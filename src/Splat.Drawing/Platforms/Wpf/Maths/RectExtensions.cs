// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>Provides extension methods for converting between System.Drawing rectangle types and Android native Rect structures.</summary>
/// <remarks>These methods facilitate interoperability between .NET drawing types and Android graphics by enabling
/// direct conversion of rectangle and point structures. All methods are static and intended for use as extension
/// methods.</remarks>
public static class RectExtensions
{
    /// <summary>Extension members for <see cref="Rect"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Rect value)
    {
        /// <summary>Converts a <see cref="Rect"/> to a <see cref="System.Drawing.RectangleF"/>.</summary>
        /// <returns>A <see cref="System.Drawing.RectangleF"/> of the value.</returns>
        public System.Drawing.RectangleF FromNative() => new((float)value.X, (float)value.Y, (float)value.Width, (float)value.Height);
    }

    /// <summary>Extension members for <see cref="System.Drawing.Rectangle"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.Rectangle value)
    {
        /// <summary>Convert a <see cref="System.Drawing.Rectangle"/> to the android native <see cref="Rect"/>.</summary>
        /// <returns>A <see cref="Rect"/> of the value.</returns>
        public Rect ToNative() => new(value.X, value.Y, value.Width, value.Height);
    }

    /// <summary>Extension members for <see cref="System.Drawing.RectangleF"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.RectangleF value)
    {
        /// <summary>Convert a <see cref="System.Drawing.RectangleF"/> to the android native <see cref="Rect"/>.</summary>
        /// <returns>A <see cref="Rect"/> of the value.</returns>
        public Rect ToNative() => new(value.X, value.Y, value.Width, value.Height);
    }
}
