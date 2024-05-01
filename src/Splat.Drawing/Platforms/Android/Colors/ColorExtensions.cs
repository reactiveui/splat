// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>
/// Provides extension methods for interacting with colors, to and from the android colors.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Converts a <see cref="System.Drawing.Color"/> to a android native color.
    /// </summary>
    /// <param name="other">The System.Drawing.Color to convert.</param>
    /// <returns>A native android color.</returns>
    public static Color ToNative(this System.Drawing.Color other) => new(other.R, other.G, other.B, other.A);

    /// <summary>
    /// Converts from a android native color to a <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <param name="other">The android native color to convert.</param>
    /// <returns>A System.Drawing.Color.</returns>
    public static System.Drawing.Color FromNative(this Color other) => System.Drawing.Color.FromArgb(other.A, other.R, other.G, other.B);
}
