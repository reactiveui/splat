﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace Splat;

/// <summary>
/// Extension methods to assist with dealing with Bitmaps.
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
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value);
#else
        if (value == null)
        {
            throw new System.ArgumentNullException(nameof(value));
        }
#endif

        return ((BitmapSourceBitmap)value).Inner ?? throw new InvalidOperationException("The bitmap is not longer valid");
    }
}
