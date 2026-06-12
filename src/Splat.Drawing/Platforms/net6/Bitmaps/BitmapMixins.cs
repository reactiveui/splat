// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>Provides extension methods for converting between <see cref="IBitmap"/> and native <see cref="BitmapSource"/> types.</summary>
/// <remarks>These methods enable interoperability between platform-agnostic bitmap representations and native WPF
/// bitmap types. They are intended to simplify conversion scenarios when working with image data across different
/// frameworks.</remarks>
public static class BitmapMixins
{
    /// <summary>Extension members for <see cref="BitmapSource"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(BitmapSource value)
    {
        /// <summary>Converts <see cref="IBitmap"/> to a native type.</summary>
        /// <returns>A <see cref="BitmapSource"/> bitmap.</returns>
        public IBitmap FromNative() => new BitmapSourceBitmap(value);
    }

    /// <summary>Extension members for <see cref="IBitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(IBitmap value)
    {
        /// <summary>Converts a <see cref="BitmapSource"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public BitmapSource ToNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            if (value is not BitmapSourceBitmap bitmapSourceBitmap)
            {
                throw new InvalidCastException($"Unable to convert {value.GetType()} to a {nameof(BitmapSource)}.");
            }

            return bitmapSourceBitmap.Inner ?? throw new InvalidOperationException("The bitmap is not longer valid");
        }
    }
}
