// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace Splat;

/// <summary>Provides extension methods for performing mathematical operations on <see cref="SizeF"/> instances.</summary>
/// <remarks>These methods enable common size-related calculations, such as scaling and approximate equality
/// checks, to be performed directly on <see cref="SizeF"/> objects. All methods are static and can be used as extension
/// methods for improved readability and convenience.</remarks>
public static class SizeMathExtensions
{
    /// <summary>Extension members for mathematical operations on <see cref="SizeF"/>.</summary>
    /// <param name="value">The size the extension members operate on.</param>
    extension(SizeF value)
    {
        /// <summary>Determines whether the current size is within a specified distance (epsilon) of another size.</summary>
        /// <param name="other">The size to compare to.</param>
        /// <param name="epsilon">The maximum allowed distance between the two sizes. Must be non-negative.</param>
        /// <returns>true if the Euclidean distance between the two sizes is less than epsilon; otherwise, false.</returns>
        public bool WithinEpsilonOf(SizeF other, float epsilon)
        {
            var deltaW = other.Width - value.Width;
            var deltaH = other.Height - value.Height;
            return Math.Sqrt((deltaW * deltaW) + (deltaH * deltaH)) < epsilon;
        }

        /// <summary>Returns a new SizeF whose width and height are multiplied by the specified scaling factor.</summary>
        /// <param name="factor">The factor by which to scale the width and height of the size.</param>
        /// <returns>A new SizeF whose dimensions are the original width and height multiplied by the specified factor.</returns>
        public SizeF ScaledBy(float factor) => new(value.Width * factor, value.Height * factor);
    }
}
