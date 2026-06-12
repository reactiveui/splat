// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for converting between System.Drawing point types and Android native Point types.
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>Extension members for <see cref="System.Drawing.Point"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(System.Drawing.Point value)
        {
            /// <summary>
            /// Convert a <see cref="System.Drawing.Point"/> to the android native <see cref="Point"/>.
            /// </summary>
            /// <returns>A <see cref="Point"/> of the value.</returns>
            public Point ToNative() => new(value.X, value.Y);
        }

        /// <summary>Extension members for <see cref="System.Drawing.PointF"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(System.Drawing.PointF value)
        {
            /// <summary>
            /// Convert a <see cref="System.Drawing.PointF"/> to the android native <see cref="Point"/>.
            /// </summary>
            /// <returns>A <see cref="Point"/> of the value.</returns>
            public Point ToNative() => new(value.X, value.Y);
        }

        /// <summary>Extension members for <see cref="Point"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(Point value)
        {
            /// <summary>
            /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.PointF"/>.
            /// </summary>
            /// <returns>A <see cref="System.Drawing.PointF"/> of the value.</returns>
            public System.Drawing.PointF FromNative() => new((float)value.X, (float)value.Y);
        }
    }
}
