// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Windows.Media.Imaging;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for converting between platform-native bitmap types and the cross-platform IBitmap
    /// interface.
    /// </summary>
    /// <remarks>These methods enable interoperability between WPF's BitmapSource and the IBitmap abstraction
    /// used in cross-platform scenarios. They are intended to simplify bitmap conversions when working with image
    /// processing or rendering code that targets multiple platforms.</remarks>
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

            return ((BitmapSourceBitmap)value).Inner ?? throw new InvalidOperationException("The bitmap has been disposed");
        }
    }
}
