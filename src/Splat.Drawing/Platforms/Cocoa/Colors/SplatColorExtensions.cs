// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using UIKit;
#else
using AppKit;
#endif

namespace Splat;

/// <summary>
/// Extension methods associated with the <see cref="SplatColor"/> struct.
/// </summary>
public static class SplatColorExtensions
{
#if UIKIT
    /// <summary>
    /// Converts a <see cref="SplatColor"/> into the cocoa native <see cref="UIColor"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="UIColor"/> generated value.</returns>
    public static UIColor ToNative(this SplatColor value) =>
        new(value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f);

    /// <summary>
    /// Converts a <see cref="UIColor"/> into the cocoa native <see cref="SplatColor"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="SplatColor"/> generated.</returns>
    public static SplatColor FromNative(this UIColor value)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value);
#else
        if (value == null)
        {
            throw new System.ArgumentNullException(nameof(value));
        }
#endif

        value.GetRGBA(out var r, out var g, out var b, out var a);
        return SplatColor.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
    }
#else
    /// <summary>
    /// Converts a <see cref="SplatColor"/> into the cocoa native <see cref="NSColor"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="NSColor"/> generated.</returns>
    public static NSColor ToNative(this SplatColor value) =>
        NSColor.FromSrgb(value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f);

    /// <summary>
    /// Converts a <see cref="NSColor"/> into the cocoa native <see cref="SplatColor"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    /// <returns>The <see cref="SplatColor"/> generated.</returns>
    public static SplatColor FromNative(this NSColor value)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value);
#else
        if (value == null)
        {
            throw new System.ArgumentNullException(nameof(value));
        }
#endif

        value.GetRgba(out var r, out var g, out var b, out var a);
        return SplatColor.FromArgb((int)(a * 255.0f), (int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
    }
#endif
}
