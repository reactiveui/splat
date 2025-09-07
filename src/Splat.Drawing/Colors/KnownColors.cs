// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Provides a constant mapping between <see cref="KnownColor"/> enum values and
/// their ARGB values (in <c>0xAARRGGBB</c> order), plus utilities to convert to/from
/// <see cref="SplatColor"/>.
/// </summary>
/// <remarks>
/// <para>
/// The mapping is stored in <see cref="ArgbValues"/> where the array index is the
/// numeric value of the corresponding <see cref="KnownColor"/>.
/// Example: <c>ArgbValues[(int)KnownColor.Blue] == 0xFF0000FF</c>.
/// </para>
/// <para>
/// This type also supports overriding known colors at runtime via <see cref="Update(int,int)"/>.
/// When a color is updated, the reverse lookup index used by <see cref="FindColorMatch(SplatColor)"/>
/// is refreshed to keep matches O(1).
/// </para>
/// <para>
/// Original authors (Mono):
///   Gonzalo Paniagua Javier (gonzalo@ximian.com)
///   Peter Dennis Bartok (pbartok@novell.com)
///   Sebastien Pouliot (sebastien@ximian.com)
///
/// Adapted for Splat, modernized for .NET 8 / .NET Standard.
/// Copyright (C) 2007 Novell,
/// and contributors to the Splat project.
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// </para>
/// </remarks>
internal static class KnownColors
{
    /// <summary>
    /// ARGB values (0xAARRGGBB), indexed by <see cref="KnownColor"/>.
    /// </summary>
    internal static readonly uint[] ArgbValues =
    [
        0x00000000, /* 000 - Empty */
        0xFFD4D0C8, /* 001 - ActiveBorder */
        0xFF0054E3, /* 002 - ActiveCaption */
        0xFFFFFFFF, /* 003 - ActiveCaptionText */
        0xFF808080, /* 004 - AppWorkspace */
        0xFFECE9D8, /* 005 - Control */
        0xFFACA899, /* 006 - ControlDark */
        0xFF716F64, /* 007 - ControlDarkDark */
        0xFFF1EFE2, /* 008 - ControlLight */
        0xFFFFFFFF, /* 009 - ControlLightLight */
        0xFF000000, /* 010 - ControlText */
        0xFF004E98, /* 011 - Desktop */
        0xFFACA899, /* 012 - GrayText */
        0xFF316AC5, /* 013 - Highlight */
        0xFFFFFFFF, /* 014 - HighlightText */
        0xFF000080, /* 015 - HotTrack */
        0xFFD4D0C8, /* 016 - InactiveBorder */
        0xFF7A96DF, /* 017 - InactiveCaption */
        0xFFD8E4F8, /* 018 - InactiveCaptionText */
        0xFFFFFFE1, /* 019 - Info */
        0xFF000000, /* 020 - InfoText */
        0xFFFFFFFF, /* 021 - Menu */
        0xFF000000, /* 022 - MenuText */
        0xFFD4D0C8, /* 023 - ScrollBar */
        0xFFFFFFFF, /* 024 - Window */
        0xFF000000, /* 025 - WindowFrame */
        0xFF000000, /* 026 - WindowText */
        0x00FFFFFF, /* 027 - Transparent */
        0xFFF0F8FF, /* 028 - AliceBlue */
        0xFFFAEBD7, /* 029 - AntiqueWhite */
        0xFF00FFFF, /* 030 - Aqua */
        0xFF7FFFD4, /* 031 - Aquamarine */
        0xFFF0FFFF, /* 032 - Azure */
        0xFFF5F5DC, /* 033 - Beige */
        0xFFFFE4C4, /* 034 - Bisque */
        0xFF000000, /* 035 - Black */
        0xFFFFEBCD, /* 036 - BlanchedAlmond */
        0xFF0000FF, /* 037 - Blue */
        0xFF8A2BE2, /* 038 - BlueViolet */
        0xFFA52A2A, /* 039 - Brown */
        0xFFDEB887, /* 040 - BurlyWood */
        0xFF5F9EA0, /* 041 - CadetBlue */
        0xFF7FFF00, /* 042 - Chartreuse */
        0xFFD2691E, /* 043 - Chocolate */
        0xFFFF7F50, /* 044 - Coral */
        0xFF6495ED, /* 045 - CornflowerBlue */
        0xFFFFF8DC, /* 046 - Cornsilk */
        0xFFDC143C, /* 047 - Crimson */
        0xFF00FFFF, /* 048 - Cyan */
        0xFF00008B, /* 049 - DarkBlue */
        0xFF008B8B, /* 050 - DarkCyan */
        0xFFB8860B, /* 051 - DarkGoldenrod */
        0xFFA9A9A9, /* 052 - DarkGray */
        0xFF006400, /* 053 - DarkGreen */
        0xFFBDB76B, /* 054 - DarkKhaki */
        0xFF8B008B, /* 055 - DarkMagenta */
        0xFF556B2F, /* 056 - DarkOliveGreen */
        0xFFFF8C00, /* 057 - DarkOrange */
        0xFF9932CC, /* 058 - DarkOrchid */
        0xFF8B0000, /* 059 - DarkRed */
        0xFFE9967A, /* 060 - DarkSalmon */
        0xFF8FBC8B, /* 061 - DarkSeaGreen */
        0xFF483D8B, /* 062 - DarkSlateBlue */
        0xFF2F4F4F, /* 063 - DarkSlateGray */
        0xFF00CED1, /* 064 - DarkTurquoise */
        0xFF9400D3, /* 065 - DarkViolet */
        0xFFFF1493, /* 066 - DeepPink */
        0xFF00BFFF, /* 067 - DeepSkyBlue */
        0xFF696969, /* 068 - DimGray */
        0xFF1E90FF, /* 069 - DodgerBlue */
        0xFFB22222, /* 070 - Firebrick */
        0xFFFFFAF0, /* 071 - FloralWhite */
        0xFF228B22, /* 072 - ForestGreen */
        0xFFFF00FF, /* 073 - Fuchsia */
        0xFFDCDCDC, /* 074 - Gainsboro */
        0xFFF8F8FF, /* 075 - GhostWhite */
        0xFFFFD700, /* 076 - Gold */
        0xFFDAA520, /* 077 - Goldenrod */
        0xFF808080, /* 078 - Gray */
        0xFF008000, /* 079 - Green */
        0xFFADFF2F, /* 080 - GreenYellow */
        0xFFF0FFF0, /* 081 - Honeydew */
        0xFFFF69B4, /* 082 - HotPink */
        0xFFCD5C5C, /* 083 - IndianRed */
        0xFF4B0082, /* 084 - Indigo */
        0xFFFFFFF0, /* 085 - Ivory */
        0xFFF0E68C, /* 086 - Khaki */
        0xFFE6E6FA, /* 087 - Lavender */
        0xFFFFF0F5, /* 088 - LavenderBlush */
        0xFF7CFC00, /* 089 - LawnGreen */
        0xFFFFFACD, /* 090 - LemonChiffon */
        0xFFADD8E6, /* 091 - LightBlue */
        0xFFF08080, /* 092 - LightCoral */
        0xFFE0FFFF, /* 093 - LightCyan */
        0xFFFAFAD2, /* 094 - LightGoldenrodYellow */
        0xFFD3D3D3, /* 095 - LightGray */
        0xFF90EE90, /* 096 - LightGreen */
        0xFFFFB6C1, /* 097 - LightPink */
        0xFFFFA07A, /* 098 - LightSalmon */
        0xFF20B2AA, /* 099 - LightSeaGreen */
        0xFF87CEFA, /* 100 - LightSkyBlue */
        0xFF778899, /* 101 - LightSlateGray */
        0xFFB0C4DE, /* 102 - LightSteelBlue */
        0xFFFFFFE0, /* 103 - LightYellow */
        0xFF00FF00, /* 104 - Lime */
        0xFF32CD32, /* 105 - LimeGreen */
        0xFFFAF0E6, /* 106 - Linen */
        0xFFFF00FF, /* 107 - Magenta */
        0xFF800000, /* 108 - Maroon */
        0xFF66CDAA, /* 109 - MediumAquamarine */
        0xFF0000CD, /* 110 - MediumBlue */
        0xFFBA55D3, /* 111 - MediumOrchid */
        0xFF9370DB, /* 112 - MediumPurple */
        0xFF3CB371, /* 113 - MediumSeaGreen */
        0xFF7B68EE, /* 114 - MediumSlateBlue */
        0xFF00FA9A, /* 115 - MediumSpringGreen */
        0xFF48D1CC, /* 116 - MediumTurquoise */
        0xFFC71585, /* 117 - MediumVioletRed */
        0xFF191970, /* 118 - MidnightBlue */
        0xFFF5FFFA, /* 119 - MintCream */
        0xFFFFE4E1, /* 120 - MistyRose */
        0xFFFFE4B5, /* 121 - Moccasin */
        0xFFFFDEAD, /* 122 - NavajoWhite */
        0xFF000080, /* 123 - Navy */
        0xFFFDF5E6, /* 124 - OldLace */
        0xFF808000, /* 125 - Olive */
        0xFF6B8E23, /* 126 - OliveDrab */
        0xFFFFA500, /* 127 - Orange */
        0xFFFF4500, /* 128 - OrangeRed */
        0xFFDA70D6, /* 129 - Orchid */
        0xFFEEE8AA, /* 130 - PaleGoldenrod */
        0xFF98FB98, /* 131 - PaleGreen */
        0xFFAFEEEE, /* 132 - PaleTurquoise */
        0xFFDB7093, /* 133 - PaleVioletRed */
        0xFFFFEFD5, /* 134 - PapayaWhip */
        0xFFFFDAB9, /* 135 - PeachPuff */
        0xFFCD853F, /* 136 - Peru */
        0xFFFFC0CB, /* 137 - Pink */
        0xFFDDA0DD, /* 138 - Plum */
        0xFFB0E0E6, /* 139 - PowderBlue */
        0xFF800080, /* 140 - Purple */
        0xFFFF0000, /* 141 - Red */
        0xFFBC8F8F, /* 142 - RosyBrown */
        0xFF4169E1, /* 143 - RoyalBlue */
        0xFF8B4513, /* 144 - SaddleBrown */
        0xFFFA8072, /* 145 - Salmon */
        0xFFF4A460, /* 146 - SandyBrown */
        0xFF2E8B57, /* 147 - SeaGreen */
        0xFFFFF5EE, /* 148 - SeaShell */
        0xFFA0522D, /* 149 - Sienna */
        0xFFC0C0C0, /* 150 - Silver */
        0xFF87CEEB, /* 151 - SkyBlue */
        0xFF6A5ACD, /* 152 - SlateBlue */
        0xFF708090, /* 153 - SlateGray */
        0xFFFFFAFA, /* 154 - Snow */
        0xFF00FF7F, /* 155 - SpringGreen */
        0xFF4682B4, /* 156 - SteelBlue */
        0xFFD2B48C, /* 157 - Tan */
        0xFF008080, /* 158 - Teal */
        0xFFD8BFD8, /* 159 - Thistle */
        0xFFFF6347, /* 160 - Tomato */
        0xFF40E0D0, /* 161 - Turquoise */
        0xFFEE82EE, /* 162 - Violet */
        0xFFF5DEB3, /* 163 - Wheat */
        0xFFFFFFFF, /* 164 - White */
        0xFFF5F5F5, /* 165 - WhiteSmoke */
        0xFFFFFF00, /* 166 - Yellow */
        0xFF9ACD32 /* 167 - YellowGreen */
    ];

    // Fast ARGB -> KnownColor reverse index. Keeps first occurrence for duplicates (e.g., Aqua/Cyan).
    private static readonly Dictionary<uint, KnownColor> ArgbToKnown = BuildReverseIndex();

        /// <summary>
    /// Creates a <see cref="SplatColor"/> for a given <see cref="KnownColor"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SplatColor FromKnownColor(KnownColor kc) => SplatColor.FromKnownColor(kc);

    /// <summary>
    /// Gets a user-friendly name for a <see cref="KnownColor"/> given as a numeric value.
    /// </summary>
    /// <remarks>Returns <see cref="string.Empty"/> if the value does not map to a defined enum member.</remarks>
    public static string GetName(short kc)
    {
        // Valid if value is within the defined enum range.
        // Update the upper bound if you add new enum members.
        const short MaxKnown = (short)KnownColor.MenuHighlight; // 174
        return (kc >= 0 && kc <= MaxKnown) ? ((KnownColor)kc).ToString() : string.Empty;
    }

    /// <summary>
    /// Gets a user-friendly name for a <see cref="KnownColor"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(KnownColor kc) => kc.ToString();

    /// <summary>
    /// Attempts to find the matching <see cref="KnownColor"/> for an exact ARGB color.
    /// </summary>
    /// <param name="c">The color to match.</param>
    /// <returns>
    /// A <see cref="SplatColor"/> constructed from the matched <see cref="KnownColor"/>,
    /// or <see cref="SplatColor.Empty"/> if no exact match is found.
    /// </returns>
    /// <remarks>
    /// Matching is exact (all 32 bits, including alpha). Duplicates (e.g., <c>Aqua</c> and <c>Cyan</c>)
    /// resolve to the first defined color in <see cref="ArgbValues"/>.
    /// </remarks>
    public static SplatColor FindColorMatch(SplatColor c)
    {
        var argb = c.ToArgb();
        return ArgbToKnown.TryGetValue(argb, out var kc) ? FromKnownColor(kc) : SplatColor.Empty;
    }

    /// <summary>
    /// Gets the ARGB value (<c>0xAARRGGBB</c>) for a known color.
    /// </summary>
    public static uint GetArgb(KnownColor kc) => ArgbValues[(int)kc];

    /// <summary>
    /// Overrides a known color with a new ARGB value and updates reverse lookup tables.
    /// This also allows teaching <see cref="SplatColor"/> new colors at runtime.
    /// </summary>
    /// <param name="knownColor">The numeric value of the <see cref="KnownColor"/> to update.</param>
    /// <param name="color">The new ARGB value (<c>0xAARRGGBB</c>).</param>
    /// <remarks>
    /// If multiple known colors share the same ARGB, the most recently updated color
    /// will be returned for that ARGB by <see cref="FindColorMatch(SplatColor)"/>.
    /// </remarks>
    public static void Update(int knownColor, int color)
    {
        var index = knownColor;
        var kc = (KnownColor)index;

        // remove old mapping if it pointed to this known color
        var oldArgb = ArgbValues[index];
        if (ArgbToKnown.TryGetValue(oldArgb, out var mapped) && mapped.Equals(kc))
        {
            ArgbToKnown.Remove(oldArgb);
        }

        // set new value and refresh reverse index
        var newArgb = (uint)color;
        ArgbValues[index] = newArgb;
        ArgbToKnown[newArgb] = kc;
    }

    private static Dictionary<uint, KnownColor> BuildReverseIndex()
    {
        var dict = new Dictionary<uint, KnownColor>(ArgbValues.Length);

        // prefer earliest defined color (e.g., Aqua over Cyan).
        for (var i = 0; i < ArgbValues.Length; i++)
        {
            var kc = (KnownColor)i;
            var argb = ArgbValues[i];
#if NET6_0_OR_GREATER || NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
            _ = dict.TryAdd(argb, kc);
#else
            if (!dict.ContainsKey(argb))
            {
                dict.Add(argb, kc);
            }
#endif
        }

        return dict;
    }
}
