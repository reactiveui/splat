// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>
/// Provides extension methods for performing mathematical operations on <see cref="SizeF"/> instances.
/// </summary>
/// <remarks>These methods enable common size-related calculations, such as scaling and approximate equality
/// checks, to be performed directly on <see cref="SizeF"/> objects. All methods are static and can be used as extension
/// methods for improved readability and convenience.</remarks>
public static class SizeMathExtensions
{
    /// <summary>
    /// Determines whether the current size is within a specified distance (epsilon) of another size.
    /// </summary>
    /// <param name="value">The size to compare from.</param>
    /// <param name="other">The size to compare to.</param>
    /// <param name="epsilon">The maximum allowed distance between the two sizes. Must be non-negative.</param>
    /// <returns>true if the Euclidean distance between the two sizes is less than epsilon; otherwise, false.</returns>
    public static bool WithinEpsilonOf(this SizeF value, SizeF other, float epsilon)
    {
        var deltaW = other.Width - value.Width;
        var deltaH = other.Height - value.Height;
        return Math.Sqrt((deltaW * deltaW) + (deltaH * deltaH)) < epsilon;
    }

    /// <summary>
    /// Returns a new SizeF whose width and height are multiplied by the specified scaling factor.
    /// </summary>
    /// <param name="value">The original SizeF to scale.</param>
    /// <param name="factor">The factor by which to scale the width and height of the size.</param>
    /// <returns>A new SizeF whose dimensions are the original width and height multiplied by the specified factor.</returns>
    public static SizeF ScaledBy(this SizeF value, float factor) => new(value.Width * factor, value.Height * factor);
}
