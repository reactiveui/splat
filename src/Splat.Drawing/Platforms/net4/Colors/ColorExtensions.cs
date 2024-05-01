// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace Splat;

/// <summary>
/// Provides extension methods for interacting with colors, to and from the XAML colors.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Converts a <see cref="System.Drawing.Color"/> to a XAML native color.
    /// </summary>
    /// <param name="value">The System.Drawing.Color to convert.</param>
    /// <returns>A native XAML color.</returns>
    public static Color ToNative(this System.Drawing.Color value) => Color.FromArgb(value.A, value.R, value.G, value.B);

    /// <summary>
    /// Converts a <see cref="System.Drawing.Color"/> into the cocoa native <see cref="SolidColorBrush"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="SolidColorBrush"/> generated.</returns>
    public static SolidColorBrush ToNativeBrush(this System.Drawing.Color value)
    {
        var ret = new SolidColorBrush(value.ToNative());
        ret.Freeze();
        return ret;
    }

    /// <summary>
    /// Converts a <see cref="SolidColorBrush"/> into the XAML <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="System.Drawing.Color"/> generated.</returns>
    public static System.Drawing.Color FromNative(this Color value) => System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
}
