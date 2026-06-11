// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
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
    /// <summary>Extension members for <see cref="IBitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(IBitmap value)
    {
        /// <summary>Converts <see cref="IBitmap"/> to a native type.</summary>
        /// <returns>A <see cref="Drawable"/> bitmap.</returns>
        public Drawable ToNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return value switch
            {
                AndroidBitmap androidBitmap => new BitmapDrawable(Application.Context.Resources, androidBitmap.Inner),
                DrawableBitmap drawableBitmap => drawableBitmap.Inner,
                _ => throw new InvalidCastException($"Unable to convert {value.GetType()} to a {nameof(Drawable)}."),
            };
        }
    }

    /// <summary>Extension members for <see cref="Bitmap"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Bitmap value)
    {
        /// <summary>Converts a <see cref="Bitmap"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public IBitmap FromNative()
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            return new AndroidBitmap(value);
        }

        /// <summary>Converts a <see cref="Bitmap"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <param name="copy">Whether to copy the android bitmap or not.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public IBitmap FromNative(bool copy)
        {
            ArgumentExceptionHelper.ThrowIfNull(value);

            if (!copy)
            {
                return new AndroidBitmap(value);
            }

            var copiedBitmap = value.Copy(value.GetConfig(), true) ?? throw new InvalidOperationException("The bitmap does not have a valid reference.");
            return new AndroidBitmap(copiedBitmap);
        }
    }

    /// <summary>Extension members for <see cref="Drawable"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Drawable value)
    {
        /// <summary>Converts a <see cref="Drawable"/> to a splat <see cref="IBitmap"/>.</summary>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public IBitmap FromNative() => new DrawableBitmap(value);
    }
}
