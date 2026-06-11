// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat;

/// <summary>Provides extension methods for converting between System.Drawing.Color and native Android color representations.</summary>
/// <remarks>These methods enable seamless interoperability between .NET color types and Android's native color
/// structures when developing cross-platform applications. All methods are static and intended for use as extension
/// methods.</remarks>
public static class ColorExtensions
{
    /// <summary>Extension members for <see cref="System.Drawing.Color"/>.</summary>
    /// <param name="other">The value the extension members operate on.</param>
    extension(System.Drawing.Color other)
    {
        /// <summary>Converts a <see cref="System.Drawing.Color"/> to a android native color.</summary>
        /// <returns>A native android color.</returns>
        public Color ToNative() => new(other.R, other.G, other.B, other.A);
    }

    /// <summary>Extension members for <see cref="Color"/>.</summary>
    /// <param name="other">The value the extension members operate on.</param>
    extension(Color other)
    {
        /// <summary>Converts from a android native color to a <see cref="System.Drawing.Color"/>.</summary>
        /// <returns>A System.Drawing.Color.</returns>
        public System.Drawing.Color FromNative() => System.Drawing.Color.FromArgb(other.A, other.R, other.G, other.B);
    }
}
