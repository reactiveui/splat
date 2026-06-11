// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for converting between System.Drawing.Size, System.Drawing.SizeF, and the Android
    /// native Size structure.
    /// </summary>
    /// <remarks>These methods simplify interoperability between .NET drawing types and Android's native size
    /// representation. They are intended for use in cross-platform scenarios where size values need to be converted
    /// between different frameworks.</remarks>
    public static class SizeExtensions
    {
        /// <summary>Extension members for <see cref="System.Drawing.Size"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(System.Drawing.Size value)
        {
            /// <summary>
            /// Convert a <see cref="System.Drawing.Size"/> to the android native <see cref="Size"/>.
            /// </summary>
            /// <returns>A <see cref="Size"/> of the value.</returns>
            public Size ToNative() => new(value.Width, value.Height);
        }

        /// <summary>Extension members for <see cref="System.Drawing.SizeF"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(System.Drawing.SizeF value)
        {
            /// <summary>
            /// Convert a <see cref="System.Drawing.SizeF"/> to the android native <see cref="Size"/>.
            /// </summary>
            /// <returns>A <see cref="Size"/> of the value.</returns>
            public Size ToNative() => new(value.Width, value.Height);
        }

        /// <summary>Extension members for <see cref="Size"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(Size value)
        {
            /// <summary>
            /// Converts a <see cref="Size"/> to a <see cref="System.Drawing.SizeF"/>.
            /// </summary>
            /// <returns>A <see cref="System.Drawing.SizeF"/> of the value.</returns>
            public System.Drawing.SizeF FromNative() => new((float)value.Width, (float)value.Height);
        }
    }
}
