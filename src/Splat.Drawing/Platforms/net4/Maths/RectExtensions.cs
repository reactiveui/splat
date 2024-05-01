// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>
/// A set of extension methods which will convert between System.Drawing rectangle's and a native rectangle classes.
/// </summary>
public static class RectExtensions
{
    /// <summary>
    /// Convert a <see cref="System.Drawing.Rectangle"/> to the android native <see cref="Rect"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Rect"/> of the value.</returns>
    public static Rect ToNative(this System.Drawing.Rectangle value) => new(value.X, value.Y, value.Width, value.Height);

    /// <summary>
    /// Convert a <see cref="System.Drawing.RectangleF"/> to the android native <see cref="Rect"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Rect"/> of the value.</returns>
    public static Rect ToNative(this System.Drawing.RectangleF value) => new(value.X, value.Y, value.Width, value.Height);

    /// <summary>
    /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.RectangleF"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="System.Drawing.RectangleF"/> of the value.</returns>
    public static System.Drawing.RectangleF FromNative(this Rect value) => new((float)value.X, (float)value.Y, (float)value.Width, (float)value.Height);
}
