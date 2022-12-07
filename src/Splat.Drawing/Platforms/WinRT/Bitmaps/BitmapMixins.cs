﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Windows.UI.Xaml.Media.Imaging;

namespace Splat;

/// <summary>
/// Extension methods to assist with dealing with Bitmaps.
/// </summary>
public static class BitmapMixins
{
    /// <summary>
    /// Converts <see cref="BitmapImage"/> to a <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static IBitmap FromNative(this BitmapImage value)
    {
        return new BitmapImageBitmap(value);
    }

    /// <summary>
    /// Converts <see cref="WriteableBitmap"/> to a <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static IBitmap FromNative(this WriteableBitmap value)
    {
        return new WriteableBitmapImageBitmap(value);
    }

    /// <summary>
    /// Converts <see cref="IBitmap"/> to a <see cref="BitmapSource"/>.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="BitmapSource"/> bitmap.</returns>
    public static BitmapSource ToNative(this IBitmap value)
    {
        if (value is null)
        {
            throw new System.ArgumentNullException(nameof(value));
        }

        if (value is WriteableBitmapImageBitmap wbib)
        {
            return wbib.Inner ?? throw new InvalidOperationException("The bitmap has been disposed");
        }

        if (value is BitmapImageBitmap bitmapImage)
        {
            return bitmapImage.Inner ?? throw new InvalidOperationException("The bitmap has been disposed");
        }

        throw new InvalidOperationException("The bitmap type is unsupported");
    }
}
