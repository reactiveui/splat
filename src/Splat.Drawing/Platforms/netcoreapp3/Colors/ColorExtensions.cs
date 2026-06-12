// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for converting between System.Drawing.Color and XAML color types.
    /// </summary>
    /// <remarks>These methods facilitate interoperability between System.Drawing and XAML color
    /// representations, enabling seamless conversion for scenarios such as UI rendering or cross-platform color
    /// manipulation.</remarks>
    public static class ColorExtensions
    {
        /// <summary>Extension members for <see cref="System.Drawing.Color"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(System.Drawing.Color value)
        {
            /// <summary>
            /// Converts a <see cref="System.Drawing.Color"/> to a XAML native color.
            /// </summary>
            /// <returns>A native XAML color.</returns>
            public Color ToNative() =>
                Color.FromArgb(value.A, value.R, value.G, value.B);

            /// <summary>
            /// Converts a <see cref="System.Drawing.Color"/> into the cocoa native <see cref="SolidColorBrush"/>.
            /// </summary>
            /// <returns>The <see cref="SolidColorBrush"/> generated.</returns>
            public SolidColorBrush ToNativeBrush()
            {
                var ret = new SolidColorBrush(value.ToNative());
                ret.Freeze();
                return ret;
            }
        }

        /// <summary>Extension members for <see cref="Color"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(Color value)
        {
            /// <summary>
            /// Converts a <see cref="SolidColorBrush"/> into the XAML <see cref="System.Drawing.Color"/>.
            /// </summary>
            /// <returns>The <see cref="System.Drawing.Color"/> generated.</returns>
            public System.Drawing.Color FromNative() =>
                System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}
