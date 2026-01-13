// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between <see cref="System.Drawing.Point"/> types and the Android native Point type.
/// </summary>
/// <remarks>These methods enable seamless conversion between <see cref="System.Drawing.Point"/>, <see cref="System.Drawing.PointF"/>, and the
/// Android native Point structure. They are intended to simplify interoperability when working with graphics or UI code
/// that uses both .NET and Android types.</remarks>
public static class PointExtensions
{
    /// <summary>
    /// Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Point"/> of the value.</returns>
    public static Point ToNative(this System.Drawing.Point value) => new(value.X, value.Y);

    /// <summary>
    /// Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="Point"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Point"/> of the value.</returns>
    public static Point ToNative(this System.Drawing.PointF value) => new(value.X, value.Y);

    /// <summary>
    /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.PointF"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
    public static System.Drawing.PointF FromNative(this Point value) => new((float)value.X, (float)value.Y);
}
