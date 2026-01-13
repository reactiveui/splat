// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between platform-native bitmap types and the cross-platform IBitmap
/// interface.
/// </summary>
/// <remarks>These methods enable interoperability between Android native bitmap representations and the IBitmap
/// abstraction used in cross-platform code. They are intended to simplify conversion scenarios when working with image
/// data in applications targeting Android.</remarks>
public static class BitmapMixins
{
    /// <summary>
    /// Converts <see cref="IBitmap"/> to a native type.
    /// </summary>
    /// <param name="value">The bitmap to convert.</param>
    /// <returns>A <see cref="Drawable"/> bitmap.</returns>
    public static Drawable ToNative(this IBitmap value)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        return value switch
        {
            AndroidBitmap androidBitmap => new BitmapDrawable(Application.Context.Resources, androidBitmap.Inner),
            _ => ((DrawableBitmap)value).Inner,
        };
    }

    /// <summary>
    /// Converts a <see cref="Bitmap"/> to a splat <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The native bitmap to convert from.</param>
    /// <param name="copy">Whether to copy the android bitmap or not.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static IBitmap FromNative(this Bitmap value, bool copy = false)
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        if (copy)
        {
            var copiedBitmap = value.Copy(value.GetConfig(), true) ?? throw new InvalidOperationException("The bitmap does not have a valid reference.");
            return new AndroidBitmap(copiedBitmap);
        }

        return new AndroidBitmap(value);
    }

    /// <summary>
    /// Converts a <see cref="Drawable"/> to a splat <see cref="IBitmap"/>.
    /// </summary>
    /// <param name="value">The native bitmap to convert from.</param>
    /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
    public static IBitmap FromNative(this Drawable value) => new DrawableBitmap(value);
}
