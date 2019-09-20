// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Tizen.Multimedia.Util;

namespace Splat
{
    /// <summary>
    /// Extension methods to assist with dealing with Bitmaps.
    /// </summary>
    public static class BitmapMixins
    {
        /// <summary>
        /// Converts <see cref="BitmapFrame"/> to a <see cref="IBitmap"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="IBitmap"/> bitmap.</returns>
        public static IBitmap FromNative(this BitmapFrame value)
        {
            return new TizenBitmap(value);
        }

        /// <summary>
        /// Converts <see cref="IBitmap"/> to a <see cref="BitmapFrame"/>.
        /// </summary>
        /// <param name="value">The bitmap to convert.</param>
        /// <returns>A <see cref="BitmapFrame"/> bitmap.</returns>
        public static BitmapFrame ToNative(this IBitmap value)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            return ((TizenBitmap)value).Inner;
        }
    }
}
