// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between native bitmap types and the cross-platform IBitmap interface.
/// </summary>
public static class BitmapMixins
{
    /// <summary>
    /// Converts <see cref="IBitmap"/> to a native type.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="BitmapSource"/> bitmap.</returns>
    public static IBitmap FromNative(this BitmapSource value) => new BitmapSourceBitmap(value);

    /// <summary>
    /// Converts a <see cref="BitmapSource"/> to a splat <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The native bitmap to convert from.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static BitmapSource ToNative(this IBitmap value)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);
        return ((BitmapSourceBitmap)value).Inner ?? throw new InvalidOperationException("There is not a valid bitmap");
    }
}
