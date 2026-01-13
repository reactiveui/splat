// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>
/// A set of extension methods which will convert between System.Drawing point's and a native point classes.
/// </summary>
public static class PointExtensions
{
    /// <summary>
    /// Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Point"/> of the value.</returns>
    public static Point ToNative(this System.Drawing.Point value) => new(value.X, value.Y);

    /// <summary>
    /// Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="PointF"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="PointF"/> of the value.</returns>
    public static PointF ToNative(this System.Drawing.PointF value) => new(value.X, value.Y);

    /// <summary>
    /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.Point"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="System.Drawing.Point"/> of the value.</returns>
    public static System.Drawing.Point FromNative(this Point value)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        return new(value.X, value.Y);
    }

    /// <summary>
    /// Converts a <see cref="PointF"/> to a <see cref="System.Drawing.PointF"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
    public static System.Drawing.PointF FromNative(this PointF value)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        return new(value.X, value.Y);
    }
}
