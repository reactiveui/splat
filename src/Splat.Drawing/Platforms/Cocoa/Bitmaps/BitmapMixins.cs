// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if UIKIT
using UIKit;
#else
using UIImage = AppKit.NSImage;
#endif

namespace Splat
{
    /// <summary>
    /// Extension methods to assist with dealing with Bitmaps.
    /// </summary>
    public static class BitmapMixins
    {
        /// <summary>
        /// Converts <see cref="IBitmap"/> to a native type.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="UIImage"/> bitmap.</returns>
        public static UIImage ToNative(this IBitmap value)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            return ((CocoaBitmap)value).Inner;
        }

        /// <summary>
        /// Converts a <see cref="UIImage"/> to a splat <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The native bitmap to convert from.</param>
        /// <param name="copy">Whether to copy the android bitmap or not.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static IBitmap FromNative(this UIImage value, bool copy = false)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            if (copy)
            {
                return new CocoaBitmap((UIImage)value.Copy());
            }

            return new CocoaBitmap(value);
        }
    }
}
