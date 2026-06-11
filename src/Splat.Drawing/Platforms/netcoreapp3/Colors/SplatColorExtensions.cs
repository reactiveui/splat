// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace Splat
{
    /// <summary>
    /// Provides extension methods for converting between SplatColor and XAML color types such as Color and
    /// SolidColorBrush.
    /// </summary>
    /// <remarks>These methods enable seamless interoperability between SplatColor and XAML color
    /// representations, allowing for easy conversion when working with UI elements in XAML-based
    /// applications.</remarks>
    public static class SplatColorExtensions
    {
        /// <summary>Extension members for <see cref="SplatColor"/>.</summary>
        /// <param name="value">The value the extension members operate on.</param>
        extension(SplatColor value)
        {
            /// <summary>
            /// Converts a <see cref="SplatColor"/> into the XAML <see cref="Color"/>.
            /// </summary>
            /// <returns>The <see cref="Color"/> generated.</returns>
            public Color ToNative() =>
                Color.FromArgb(value.A, value.R, value.G, value.B);

            /// <summary>
            /// Converts a <see cref="SplatColor"/> into the XAML <see cref="SolidColorBrush"/>.
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
            /// Converts a <see cref="Color"/> into the XAML <see cref="SplatColor"/>.
            /// </summary>
            /// <returns>The <see cref="SplatColor"/> generated.</returns>
            public SplatColor FromNative() =>
                SplatColor.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}
