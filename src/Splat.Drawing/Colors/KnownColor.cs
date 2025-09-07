// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat
{
    /// <summary>
    /// Well-known color names supported by <c>Splat</c>.
    /// The underlying numeric value of each member indexes into the ARGB table
    /// used by <c>KnownColors</c> (ARGB format is <c>0xAARRGGBB</c>).
    /// </summary>
    /// <remarks>
    /// Historical attribution (preserved from earlier sources):
    /// <para>Author: Dennis Hayes (dennish@raytek.com), Ben Houston (ben@exocortex.org).</para>
    /// <para>(C) 2002 Dennis Hayes.</para>
    /// <para>Copyright (C) 2004,2006 Novell, Inc (http://www.novell.com).</para>
    /// <para>Licensed under the MIT license as part of the ReactiveUI repository.</para>
    /// </remarks>
    public enum KnownColor
    {
        /// <summary>Empty (0x00000000). Represents no color.</summary>
        Empty = 0,

        /// <summary>ActiveBorder (0xFFD4D0C8).</summary>
        ActiveBorder = 1,

        /// <summary>ActiveCaption (0xFF0054E3).</summary>
        ActiveCaption = 2,

        /// <summary>ActiveCaptionText (0xFFFFFFFF).</summary>
        ActiveCaptionText = 3,

        /// <summary>AppWorkspace (0xFF808080).</summary>
        AppWorkspace = 4,

        /// <summary>Control (0xFFECE9D8).</summary>
        Control = 5,

        /// <summary>ControlDark (0xFFACA899).</summary>
        ControlDark = 6,

        /// <summary>ControlDarkDark (0xFF716F64).</summary>
        ControlDarkDark = 7,

        /// <summary>ControlLight (0xFFF1EFE2).</summary>
        ControlLight = 8,

        /// <summary>ControlLightLight (0xFFFFFFFF).</summary>
        ControlLightLight = 9,

        /// <summary>ControlText (0xFF000000).</summary>
        ControlText = 10,

        /// <summary>Desktop (0xFF004E98).</summary>
        Desktop = 11,

        /// <summary>GrayText (0xFFACA899).</summary>
        GrayText = 12,

        /// <summary>Highlight (0xFF316AC5).</summary>
        Highlight = 13,

        /// <summary>HighlightText (0xFFFFFFFF).</summary>
        HighlightText = 14,

        /// <summary>HotTrack (0xFF000080).</summary>
        HotTrack = 15,

        /// <summary>InactiveBorder (0xFFD4D0C8).</summary>
        InactiveBorder = 16,

        /// <summary>InactiveCaption (0xFF7A96DF).</summary>
        InactiveCaption = 17,

        /// <summary>InactiveCaptionText (0xFFD8E4F8).</summary>
        InactiveCaptionText = 18,

        /// <summary>Info (0xFFFFFFE1).</summary>
        Info = 19,

        /// <summary>InfoText (0xFF000000).</summary>
        InfoText = 20,

        /// <summary>Menu (0xFFFFFFFF).</summary>
        Menu = 21,

        /// <summary>MenuText (0xFF000000).</summary>
        MenuText = 22,

        /// <summary>ScrollBar (0xFFD4D0C8).</summary>
        ScrollBar = 23,

        /// <summary>Window (0xFFFFFFFF).</summary>
        Window = 24,

        /// <summary>WindowFrame (0xFF000000).</summary>
        WindowFrame = 25,

        /// <summary>WindowText (0xFF000000).</summary>
        WindowText = 26,

        /// <summary>Transparent (0x00FFFFFF). Fully transparent (alpha = 0).</summary>
        Transparent = 27,

        /// <summary>AliceBlue (0xFFF0F8FF).</summary>
        AliceBlue = 28,

        /// <summary>AntiqueWhite (0xFFFAEBD7).</summary>
        AntiqueWhite = 29,

        /// <summary>Aqua (0xFF00FFFF). Same RGB as <see cref="Cyan"/>.</summary>
        Aqua = 30,

        /// <summary>Aquamarine (0xFF7FFFD4).</summary>
        Aquamarine = 31,

        /// <summary>Azure (0xFFF0FFFF).</summary>
        Azure = 32,

        /// <summary>Beige (0xFFF5F5DC).</summary>
        Beige = 33,

        /// <summary>Bisque (0xFFFFE4C4).</summary>
        Bisque = 34,

        /// <summary>Black (0xFF000000).</summary>
        Black = 35,

        /// <summary>BlanchedAlmond (0xFFFFEBCD).</summary>
        BlanchedAlmond = 36,

        /// <summary>Blue (0xFF0000FF).</summary>
        Blue = 37,

        /// <summary>BlueViolet (0xFF8A2BE2).</summary>
        BlueViolet = 38,

        /// <summary>Brown (0xFFA52A2A).</summary>
        Brown = 39,

        /// <summary>BurlyWood (0xFFDEB887).</summary>
        BurlyWood = 40,

        /// <summary>CadetBlue (0xFF5F9EA0).</summary>
        CadetBlue = 41,

        /// <summary>Chartreuse (0xFF7FFF00).</summary>
        Chartreuse = 42,

        /// <summary>Chocolate (0xFFD2691E).</summary>
        Chocolate = 43,

        /// <summary>Coral (0xFFFF7F50).</summary>
        Coral = 44,

        /// <summary>CornflowerBlue (0xFF6495ED).</summary>
        CornflowerBlue = 45,

        /// <summary>Cornsilk (0xFFFFF8DC).</summary>
        Cornsilk = 46,

        /// <summary>Crimson (0xFFDC143C).</summary>
        Crimson = 47,

        /// <summary>Cyan (0xFF00FFFF). Same RGB as <see cref="Aqua"/>.</summary>
        Cyan = 48,

        /// <summary>DarkBlue (0xFF00008B).</summary>
        DarkBlue = 49,

        /// <summary>DarkCyan (0xFF008B8B).</summary>
        DarkCyan = 50,

        /// <summary>DarkGoldenrod (0xFFB8860B).</summary>
        DarkGoldenrod = 51,

        /// <summary>DarkGray (0xFFA9A9A9).</summary>
        DarkGray = 52,

        /// <summary>DarkGreen (0xFF006400).</summary>
        DarkGreen = 53,

        /// <summary>DarkKhaki (0xFFBDB76B).</summary>
        DarkKhaki = 54,

        /// <summary>DarkMagenta (0xFF8B008B).</summary>
        DarkMagenta = 55,

        /// <summary>DarkOliveGreen (0xFF556B2F).</summary>
        DarkOliveGreen = 56,

        /// <summary>DarkOrange (0xFFFF8C00).</summary>
        DarkOrange = 57,

        /// <summary>DarkOrchid (0xFF9932CC).</summary>
        DarkOrchid = 58,

        /// <summary>DarkRed (0xFF8B0000).</summary>
        DarkRed = 59,

        /// <summary>DarkSalmon (0xFFE9967A).</summary>
        DarkSalmon = 60,

        /// <summary>DarkSeaGreen (0xFF8FBC8B).</summary>
        DarkSeaGreen = 61,

        /// <summary>DarkSlateBlue (0xFF483D8B).</summary>
        DarkSlateBlue = 62,

        /// <summary>DarkSlateGray (0xFF2F4F4F).</summary>
        DarkSlateGray = 63,

        /// <summary>DarkTurquoise (0xFF00CED1).</summary>
        DarkTurquoise = 64,

        /// <summary>DarkViolet (0xFF9400D3).</summary>
        DarkViolet = 65,

        /// <summary>DeepPink (0xFFFF1493).</summary>
        DeepPink = 66,

        /// <summary>DeepSkyBlue (0xFF00BFFF).</summary>
        DeepSkyBlue = 67,

        /// <summary>DimGray (0xFF696969).</summary>
        DimGray = 68,

        /// <summary>DodgerBlue (0xFF1E90FF).</summary>
        DodgerBlue = 69,

        /// <summary>Firebrick (0xFFB22222).</summary>
        Firebrick = 70,

        /// <summary>FloralWhite (0xFFFFFAF0).</summary>
        FloralWhite = 71,

        /// <summary>ForestGreen (0xFF228B22).</summary>
        ForestGreen = 72,

        /// <summary>Fuchsia (0xFFFF00FF). Same RGB as <see cref="Magenta"/>.</summary>
        Fuchsia = 73,

        /// <summary>Gainsboro (0xFFDCDCDC).</summary>
        Gainsboro = 74,

        /// <summary>GhostWhite (0xFFF8F8FF).</summary>
        GhostWhite = 75,

        /// <summary>Gold (0xFFFFD700).</summary>
        Gold = 76,

        /// <summary>Goldenrod (0xFFDAA520).</summary>
        Goldenrod = 77,

        /// <summary>Gray (0xFF808080).</summary>
        Gray = 78,

        /// <summary>Green (0xFF008000).</summary>
        Green = 79,

        /// <summary>GreenYellow (0xFFADFF2F).</summary>
        GreenYellow = 80,

        /// <summary>Honeydew (0xFFF0FFF0).</summary>
        Honeydew = 81,

        /// <summary>HotPink (0xFFFF69B4).</summary>
        HotPink = 82,

        /// <summary>IndianRed (0xFFCD5C5C).</summary>
        IndianRed = 83,

        /// <summary>Indigo (0xFF4B0082).</summary>
        Indigo = 84,

        /// <summary>Ivory (0xFFFFFFF0).</summary>
        Ivory = 85,

        /// <summary>Khaki (0xFFF0E68C).</summary>
        Khaki = 86,

        /// <summary>Lavender (0xFFE6E6FA).</summary>
        Lavender = 87,

        /// <summary>LavenderBlush (0xFFFFF0F5).</summary>
        LavenderBlush = 88,

        /// <summary>LawnGreen (0xFF7CFC00).</summary>
        LawnGreen = 89,

        /// <summary>LemonChiffon (0xFFFFFACD).</summary>
        LemonChiffon = 90,

        /// <summary>LightBlue (0xFFADD8E6).</summary>
        LightBlue = 91,

        /// <summary>LightCoral (0xFFF08080).</summary>
        LightCoral = 92,

        /// <summary>LightCyan (0xFFE0FFFF).</summary>
        LightCyan = 93,

        /// <summary>LightGoldenrodYellow (0xFFFAFAD2).</summary>
        LightGoldenrodYellow = 94,

        /// <summary>LightGray (0xFFD3D3D3).</summary>
        LightGray = 95,

        /// <summary>LightGreen (0xFF90EE90).</summary>
        LightGreen = 96,

        /// <summary>LightPink (0xFFFFB6C1).</summary>
        LightPink = 97,

        /// <summary>LightSalmon (0xFFFFA07A).</summary>
        LightSalmon = 98,

        /// <summary>LightSeaGreen (0xFF20B2AA).</summary>
        LightSeaGreen = 99,

        /// <summary>LightSkyBlue (0xFF87CEFA).</summary>
        LightSkyBlue = 100,

        /// <summary>LightSlateGray (0xFF778899).</summary>
        LightSlateGray = 101,

        /// <summary>LightSteelBlue (0xFFB0C4DE).</summary>
        LightSteelBlue = 102,

        /// <summary>LightYellow (0xFFFFFFE0).</summary>
        LightYellow = 103,

        /// <summary>Lime (0xFF00FF00).</summary>
        Lime = 104,

        /// <summary>LimeGreen (0xFF32CD32).</summary>
        LimeGreen = 105,

        /// <summary>Linen (0xFFFAF0E6).</summary>
        Linen = 106,

        /// <summary>Magenta (0xFFFF00FF). Same RGB as <see cref="Fuchsia"/>.</summary>
        Magenta = 107,

        /// <summary>Maroon (0xFF800000).</summary>
        Maroon = 108,

        /// <summary>MediumAquamarine (0xFF66CDAA).</summary>
        MediumAquamarine = 109,

        /// <summary>MediumBlue (0xFF0000CD).</summary>
        MediumBlue = 110,

        /// <summary>MediumOrchid (0xFFBA55D3).</summary>
        MediumOrchid = 111,

        /// <summary>MediumPurple (0xFF9370DB).</summary>
        MediumPurple = 112,

        /// <summary>MediumSeaGreen (0xFF3CB371).</summary>
        MediumSeaGreen = 113,

        /// <summary>MediumSlateBlue (0xFF7B68EE).</summary>
        MediumSlateBlue = 114,

        /// <summary>MediumSpringGreen (0xFF00FA9A).</summary>
        MediumSpringGreen = 115,

        /// <summary>MediumTurquoise (0xFF48D1CC).</summary>
        MediumTurquoise = 116,

        /// <summary>MediumVioletRed (0xFFC71585).</summary>
        MediumVioletRed = 117,

        /// <summary>MidnightBlue (0xFF191970).</summary>
        MidnightBlue = 118,

        /// <summary>MintCream (0xFFF5FFFA).</summary>
        MintCream = 119,

        /// <summary>MistyRose (0xFFFFE4E1).</summary>
        MistyRose = 120,

        /// <summary>Moccasin (0xFFFFE4B5).</summary>
        Moccasin = 121,

        /// <summary>NavajoWhite (0xFFFFDEAD).</summary>
        NavajoWhite = 122,

        /// <summary>Navy (0xFF000080).</summary>
        Navy = 123,

        /// <summary>OldLace (0xFFFDF5E6).</summary>
        OldLace = 124,

        /// <summary>Olive (0xFF808000).</summary>
        Olive = 125,

        /// <summary>OliveDrab (0xFF6B8E23).</summary>
        OliveDrab = 126,

        /// <summary>Orange (0xFFFFA500).</summary>
        Orange = 127,

        /// <summary>OrangeRed (0xFFFF4500).</summary>
        OrangeRed = 128,

        /// <summary>Orchid (0xFFDA70D6).</summary>
        Orchid = 129,

        /// <summary>PaleGoldenrod (0xFFEEE8AA).</summary>
        PaleGoldenrod = 130,

        /// <summary>PaleGreen (0xFF98FB98).</summary>
        PaleGreen = 131,

        /// <summary>PaleTurquoise (0xFFAFEEEE).</summary>
        PaleTurquoise = 132,

        /// <summary>PaleVioletRed (0xFFDB7093).</summary>
        PaleVioletRed = 133,

        /// <summary>PapayaWhip (0xFFFFEFD5).</summary>
        PapayaWhip = 134,

        /// <summary>PeachPuff (0xFFFFDAB9).</summary>
        PeachPuff = 135,

        /// <summary>Peru (0xFFCD853F).</summary>
        Peru = 136,

        /// <summary>Pink (0xFFFFC0CB).</summary>
        Pink = 137,

        /// <summary>Plum (0xFFDDA0DD).</summary>
        Plum = 138,

        /// <summary>PowderBlue (0xFFB0E0E6).</summary>
        PowderBlue = 139,

        /// <summary>Purple (0xFF800080).</summary>
        Purple = 140,

        /// <summary>Red (0xFFFF0000).</summary>
        Red = 141,

        /// <summary>RosyBrown (0xFFBC8F8F).</summary>
        RosyBrown = 142,

        /// <summary>RoyalBlue (0xFF4169E1).</summary>
        RoyalBlue = 143,

        /// <summary>SaddleBrown (0xFF8B4513).</summary>
        SaddleBrown = 144,

        /// <summary>Salmon (0xFFFA8072).</summary>
        Salmon = 145,

        /// <summary>SandyBrown (0xFFF4A460).</summary>
        SandyBrown = 146,

        /// <summary>SeaGreen (0xFF2E8B57).</summary>
        SeaGreen = 147,

        /// <summary>SeaShell (0xFFFFF5EE).</summary>
        SeaShell = 148,

        /// <summary>Sienna (0xFFA0522D).</summary>
        Sienna = 149,

        /// <summary>Silver (0xFFC0C0C0).</summary>
        Silver = 150,

        /// <summary>SkyBlue (0xFF87CEEB).</summary>
        SkyBlue = 151,

        /// <summary>SlateBlue (0xFF6A5ACD).</summary>
        SlateBlue = 152,

        /// <summary>SlateGray (0xFF708090).</summary>
        SlateGray = 153,

        /// <summary>Snow (0xFFFFFAFA).</summary>
        Snow = 154,

        /// <summary>SpringGreen (0xFF00FF7F).</summary>
        SpringGreen = 155,

        /// <summary>SteelBlue (0xFF4682B4).</summary>
        SteelBlue = 156,

        /// <summary>Tan (0xFFD2B48C).</summary>
        Tan = 157,

        /// <summary>Teal (0xFF008080).</summary>
        Teal = 158,

        /// <summary>Thistle (0xFFD8BFD8).</summary>
        Thistle = 159,

        /// <summary>Tomato (0xFFFF6347).</summary>
        Tomato = 160,

        /// <summary>Turquoise (0xFF40E0D0).</summary>
        Turquoise = 161,

        /// <summary>Violet (0xFFEE82EE).</summary>
        Violet = 162,

        /// <summary>Wheat (0xFFF5DEB3).</summary>
        Wheat = 163,

        /// <summary>White (0xFFFFFFFF).</summary>
        White = 164,

        /// <summary>WhiteSmoke (0xFFF5F5F5).</summary>
        WhiteSmoke = 165,

        /// <summary>Yellow (0xFFFFFF00).</summary>
        Yellow = 166,

        /// <summary>YellowGreen (0xFF9ACD32).</summary>
        YellowGreen = 167,

        /// <summary>ButtonFace (0xFFECE9D8).</summary>
        ButtonFace = 168,

        /// <summary>ButtonHighlight (0xFFFFFFFF).</summary>
        ButtonHighlight = 169,

        /// <summary>ButtonShadow (0xFFACA899).</summary>
        ButtonShadow = 170,

        /// <summary>GradientActiveCaption (0xFF3D95FF).</summary>
        GradientActiveCaption = 171,

        /// <summary>GradientInactiveCaption (0xFF9DB9EB).</summary>
        GradientInactiveCaption = 172,

        /// <summary>MenuBar (0xFFECE9D8).</summary>
        MenuBar = 173,

        /// <summary>MenuHighlight (0xFF316AC5).</summary>
        MenuHighlight = 174
    }
}
