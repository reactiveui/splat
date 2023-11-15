// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat
{
    /// <summary>
    /// A set of extension methods which will convert between System.Drawing point's and a native point classes.
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public static Point ToNative(this System.Drawing.Point value) => new(value.X, value.Y);

        /// <summary>
        /// Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="Point"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="Point"/> of the value.</returns>
        public static Point ToNative(this System.Drawing.PointF value) => new(value.X, value.Y);

        /// <summary>
        /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.PointF"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
        public static System.Drawing.PointF FromNative(this Point value) => new((float)value.X, (float)value.Y);
    }
}
