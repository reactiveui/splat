// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between <see cref="System.Drawing.Size"/>, <see cref="System.Drawing.SizeF"/>, and the Android native
/// Size structure.
/// </summary>
/// <remarks>These methods facilitate interoperability between .NET drawing types and Android's native size
/// representation. They are intended for use in cross-platform scenarios where size values need to be converted between
/// different frameworks.</remarks>
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
