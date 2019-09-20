// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Graphics;

namespace Splat
{
    /// <summary>
    /// A set of extension methods which will convert between System.Drawing rectangle's and a native rectangle classes.
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Convert a <see cref="System.Drawing.Rectangle"/> to the android native <see cref="Rect"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Rect"/> of the value.</returns>
        public static Rect ToNative(this System.Drawing.Rectangle value)
        {
            return new Rect(value.X, value.Y, value.X + value.Width, value.Y + value.Height);
        }

        /// <summary>
        /// Convert a <see cref="System.Drawing.RectangleF"/> to the android native <see cref="RectF"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="RectF"/> of the value.</returns>
        public static RectF ToNative(this System.Drawing.RectangleF value)
        {
            return new RectF(value.X, value.Y, value.X + value.Width, value.Y + value.Height);
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> to a <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="System.Drawing.Rectangle"/> of the value.</returns>
        public static System.Drawing.Rectangle FromNative(this Rect value)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            return new System.Drawing.Rectangle(value.Left, value.Top, value.Width(), value.Height());
        }

        /// <summary>
        /// Converts a <see cref="RectF"/> to a <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="System.Drawing.RectangleF"/> of the value.</returns>
        public static System.Drawing.RectangleF FromNative(this RectF value)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            return new System.Drawing.RectangleF(value.Left, value.Top, value.Width(), value.Height());
        }
    }
}
