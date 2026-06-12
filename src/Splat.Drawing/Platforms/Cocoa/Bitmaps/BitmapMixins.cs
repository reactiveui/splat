// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using UIKit;
#else
using UIImage = AppKit.NSImage;
#endif

namespace Splat;

/// <summary>Provides extension methods for converting between IBitmap and native UIImage types.</summary>
/// <remarks>These methods enable interoperability between platform-agnostic bitmap representations and native iOS
/// image types. They are intended for use in scenarios where image data needs to be transferred between cross-platform
/// and platform-specific components.</remarks>
public static class BitmapMixins
{
    /// <summary>Extension members for <see cref="IBitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(IBitmap value)
    {
        /// <summary>Converts <see cref="IBitmap"/> to a native type.</summary>
        /// <returns>A <see cref="UIImage"/> bitmap.</returns>
        public UIImage ToNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return value is CocoaBitmap cocoaBitmap
                ? cocoaBitmap.Inner
                : throw new InvalidCastException($"Unable to convert {value.GetType()} to a {nameof(UIImage)}.");
        }
    }

    /// <summary>Extension members for <see cref="UIImage"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(UIImage value)
    {
        /// <summary>Converts a <see cref="UIImage"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public IBitmap FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new CocoaBitmap(value);
        }

        /// <summary>Converts a <see cref="UIImage"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <param name="copy">Whether to copy the native image or not.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public IBitmap FromNative(bool copy)
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return copy ? new CocoaBitmap((UIImage)value.Copy()) : new CocoaBitmap(value);
        }
    }
}
