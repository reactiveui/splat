// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>
/// A set of extension methods which will convert between System.Drawing size's and a native size classes.
/// </summary>
public static class SizeExtensions
{
    /// <summary>
    /// Convert a <see cref="System.Drawing.Size"/> to the android native <see cref="Size"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Size"/> of the value.</returns>
    public static Size ToNative(this System.Drawing.Size value) => new(value.Width, value.Height);

    /// <summary>
    /// Convert a <see cref="System.Drawing.SizeF"/> to the android native <see cref="Size"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Size"/> of the value.</returns>
    public static Size ToNative(this System.Drawing.SizeF value) => new(value.Width, value.Height);

    /// <summary>
    /// Converts a <see cref="Size"/> to a <see cref="System.Drawing.SizeF"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="System.Drawing.SizeF"/> of the value.</returns>
    public static System.Drawing.SizeF FromNative(this Size value) => new((float)value.Width, (float)value.Height);
}
