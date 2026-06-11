// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>Provides extension methods for converting between native bitmap types and the cross-platform IBitmap interface.</summary>
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
            return ((BitmapSourceBitmap)value).Inner ?? throw new InvalidOperationException("There is not a valid bitmap");
        }
    }
}
