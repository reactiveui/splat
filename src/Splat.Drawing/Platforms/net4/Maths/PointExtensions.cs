// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>Provides extension methods for converting between <see cref="System.Drawing.Point"/> types and the Android native Point type.</summary>
/// <remarks>These methods enable seamless conversion between <see cref="System.Drawing.Point"/>, <see cref="System.Drawing.PointF"/>, and the
/// Android native Point structure. They are intended to simplify interoperability when working with graphics or UI code
/// that uses both .NET and Android types.</remarks>
public static class PointExtensions
{
    /// <summary>Extension members for <see cref="Point"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Point value)
    {
        /// <summary>Converts a <see cref="Point"/> to a <see cref="System.Drawing.PointF"/>.</summary>
        /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
        public System.Drawing.PointF FromNative() => new((float)value.X, (float)value.Y);
    }

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
        /// <summary>Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="Point"/>.</summary>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public Point ToNative() => new(value.X, value.Y);
    }
}
