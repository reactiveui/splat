// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>Provides extension methods for converting between <see cref="SplatColor"/> and the Android native Color type.</summary>
public static class SplatColorExtensions
{
    /// <summary>Extension members for <see cref="SplatColor"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(SplatColor value)
    {
        /// <summary>Converts a <see cref="SplatColor"/> into the android native <see cref="Color"/>.</summary>
        /// <returns>The <see cref="Color"/> generated.</returns>
        public Color ToNative() => new(value.R, value.G, value.B, value.A);
    }

    /// <summary>Extension members for <see cref="Color"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Color value)
    {
        /// <summary>Converts a <see cref="Color"/> into the android native <see cref="SplatColor"/>.</summary>
        /// <returns>The <see cref="SplatColor"/> generated.</returns>
        public SplatColor FromNative() => SplatColor.FromArgb(value.A, value.R, value.G, value.B);
    }
}
