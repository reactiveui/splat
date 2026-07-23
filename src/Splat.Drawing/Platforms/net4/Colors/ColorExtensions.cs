// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace Splat;

/// <summary>
/// Provides extension methods for converting between System.Drawing.Color and XAML color types, including Color and
/// SolidColorBrush.
/// </summary>
/// <remarks>These methods facilitate interoperability between System.Drawing and XAML-based color
/// representations, enabling seamless conversion when working with graphics or UI elements across different
/// frameworks.</remarks>
public static class ColorExtensions
{
    /// <summary>Extension members for <see cref="Color"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(Color value)
    {
        /// <summary>Converts a <see cref="SolidColorBrush"/> into the XAML <see cref="System.Drawing.Color"/>.</summary>
        /// <returns>The <see cref="System.Drawing.Color"/> generated.</returns>
        public System.Drawing.Color FromNative() => System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
    }

    /// <summary>Extension members for <see cref="System.Drawing.Color"/>.</summary>
    /// <param name="value">The value the extension members operate on.</param>
    extension(System.Drawing.Color value)
    {
        /// <summary>Converts a <see cref="System.Drawing.Color"/> to a XAML native color.</summary>
        /// <returns>A native XAML color.</returns>
        public Color ToNative() => Color.FromArgb(value.A, value.R, value.G, value.B);

        /// <summary>Converts a <see cref="System.Drawing.Color"/> into the cocoa native <see cref="SolidColorBrush"/>.</summary>
        /// <returns>The <see cref="SolidColorBrush"/> generated.</returns>
        public SolidColorBrush ToNativeBrush()
        {
            var ret = new SolidColorBrush(value.ToNative());
            ret.Freeze();
            return ret;
        }
    }
}
