// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Extension methods to assist with the <see cref="SizeF"/> struct.
/// </summary>
public static class SizeMathExtensions
{
    /// <summary>
    /// Determines whether two sizes are within epsilon of each other.
    /// </summary>
    /// <param name="value">The size we are doing the operation against.</param>
    /// <param name="other">The size to compare for equality.</param>
    /// <param name="epsilon">The tolerated epsilon value.</param>
    /// <returns>If the value is equal based on the epsilon.</returns>
    public static bool WithinEpsilonOf(this SizeF value, SizeF other, float epsilon)
    {
        var deltaW = other.Width - value.Width;
        var deltaH = other.Height - value.Height;
        return Math.Sqrt((deltaW * deltaW) + (deltaH * deltaH)) < epsilon;
    }

    /// <summary>
    /// Scales a size by a scalar value.
    /// </summary>
    /// <param name="value">The size we are doing the operation against.</param>
    /// <param name="factor">The amount to scale by.</param>
    /// <returns>The scaled size.</returns>
    public static SizeF ScaledBy(this SizeF value, float factor) => new(value.Width * factor, value.Height * factor);
}
