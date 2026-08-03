// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SkiaSharp;

namespace Splat.SkiaSharp;

/// <summary>Converts between <see cref="IBitmap"/> and the Skia bitmap behind it.</summary>
/// <remarks>
/// Skia is not the host's imaging stack, and these conversions do not pretend otherwise: what comes
/// back is an <see cref="SKBitmap"/>, not a platform bitmap type.
/// </remarks>
public static class BitmapMixins
{
    /// <summary>Extension members for <see cref="IBitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(IBitmap value)
    {
        /// <summary>Gets the Skia bitmap behind an <see cref="IBitmap"/> this package produced.</summary>
        /// <returns>The Skia bitmap, which the <see cref="IBitmap"/> still owns.</returns>
        /// <exception cref="InvalidOperationException">The bitmap has been disposed.</exception>
        /// <exception cref="InvalidCastException">The bitmap came from a different loader.</exception>
        public SKBitmap ToNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return ((SkiaBitmap)value).Inner ?? throw new InvalidOperationException("The bitmap has been disposed");
        }
    }

    /// <summary>Extension members for <see cref="SKBitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(SKBitmap value)
    {
        /// <summary>Wraps a Skia bitmap as an <see cref="IBitmap"/>, taking ownership of it.</summary>
        /// <returns>The wrapped bitmap.</returns>
        public IBitmap FromNative() => new SkiaBitmap(value);
    }
}
