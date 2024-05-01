// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>
/// Extension methods associated with the <see cref="SplatColor"/> struct.
/// </summary>
public static class SplatColorExtensions
{
    /// <summary>
    /// Converts a <see cref="SplatColor"/> into the android native <see cref="Color"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="Color"/> generated.</returns>
    public static Color ToNative(this SplatColor value) => new(value.R, value.G, value.B, value.A);

    /// <summary>
    /// Converts a <see cref="Color"/> into the android native <see cref="SplatColor"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="SplatColor"/> generated.</returns>
    public static SplatColor FromNative(this Color value) => SplatColor.FromArgb(value.A, value.R, value.G, value.B);
}
