// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using UIKit;
#else
using AppKit;
#endif

namespace Splat;

/// <summary>Provides extension methods for converting between <see cref="SplatColor"/> and native Cocoa color types.</summary>
/// <remarks>These methods enable seamless interoperability between SplatColor and platform-specific color
/// representations, such as UIColor on iOS or NSColor on macOS. Use these extensions to convert colors when working
/// with native UI frameworks.</remarks>
public static class SplatColorExtensions
{
    /// <summary>The maximum value of a single 8-bit colour component, used to scale between byte and normalized float channels.</summary>
    private const float ByteComponentMax = 255F;

#if UIKIT
    /// <summary>Extension members for <see cref="SplatColor"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(SplatColor value)
    {
        /// <summary>Converts a <see cref="SplatColor"/> into the cocoa native <see cref="UIColor"/>.</summary>
        /// <returns>The <see cref="UIColor"/> generated value.</returns>
        public UIColor ToNative() =>
            new(value.R / ByteComponentMax, value.G / ByteComponentMax, value.B / ByteComponentMax, value.A / ByteComponentMax);
    }

    /// <summary>Extension members for <see cref="UIColor"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(UIColor value)
    {
        /// <summary>Converts a <see cref="UIColor"/> into the cocoa native <see cref="SplatColor"/>.</summary>
        /// <returns>The <see cref="SplatColor"/> generated.</returns>
        public SplatColor FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            value.GetRGBA(out var r, out var g, out var b, out var a);
            return SplatColor.FromArgb((int)(a * ByteComponentMax), (int)(r * ByteComponentMax), (int)(g * ByteComponentMax), (int)(b * ByteComponentMax));
        }
    }
#else
    /// <summary>Extension members for <see cref="NSColor"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(NSColor value)
    {
        /// <summary>Converts a <see cref="NSColor"/> into the cocoa native <see cref="SplatColor"/>.</summary>
        /// <returns>The <see cref="SplatColor"/> generated.</returns>
        public SplatColor FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            value.GetRgba(out var r, out var g, out var b, out var a);
            return SplatColor.FromArgb((int)(a * ByteComponentMax), (int)(r * ByteComponentMax), (int)(g * ByteComponentMax), (int)(b * ByteComponentMax));
        }
    }

    /// <summary>Extension members for <see cref="SplatColor"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(SplatColor value)
    {
        /// <summary>Converts a <see cref="SplatColor"/> into the cocoa native <see cref="NSColor"/>.</summary>
        /// <returns>The <see cref="NSColor"/> generated.</returns>
        public NSColor ToNative() =>
            NSColor.FromSrgb(value.R / ByteComponentMax, value.G / ByteComponentMax, value.B / ByteComponentMax, value.A / ByteComponentMax);
    }
#endif
}
