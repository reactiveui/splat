// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Tizen.Multimedia.Util;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between <see cref="BitmapFrame"/> and <see cref="IBitmap"/>
/// representations.
/// </summary>
/// <remarks>These methods enable seamless interoperability between platform-specific bitmap types and the <see
/// cref="IBitmap"/> abstraction. They are intended to simplify bitmap conversions when working with image processing or
/// rendering APIs that require different bitmap formats.</remarks>
public static class BitmapMixins
{
    /// <summary>
    /// Converts <see cref="BitmapFrame"/> to a <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static IBitmap FromNative(this BitmapFrame value) => new TizenBitmap(value);

    /// <summary>
    /// Converts <see cref="IBitmap"/> to a <see cref="BitmapFrame"/>.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="BitmapFrame"/> bitmap.</returns>
    public static BitmapFrame ToNative(this IBitmap value)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);
        return (value as TizenBitmap)?.Inner ?? throw new InvalidOperationException("Bitmap has been disposed");
    }
}
