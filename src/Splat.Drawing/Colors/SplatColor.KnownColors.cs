// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A platform independent color structure.
/// </summary>
public partial struct SplatColor
{
    /// <summary>
    /// Gets a color which is fully transparent.
    /// </summary>
    public static SplatColor Transparent => KnownColors.FromKnownColor(KnownColor.Transparent);

    /// <summary>
    /// Gets a color which is alice blue.
    /// </summary>
    public static SplatColor AliceBlue => KnownColors.FromKnownColor(KnownColor.AliceBlue);

    /// <summary>
    /// Gets a color which is antique white.
    /// </summary>
    public static SplatColor AntiqueWhite => KnownColors.FromKnownColor(KnownColor.AntiqueWhite);

    /// <summary>
    /// Gets a color which is aqua.
    /// </summary>
    public static SplatColor Aqua => KnownColors.FromKnownColor(KnownColor.Aqua);

    /// <summary>
    /// Gets a color which is aquamarine.
    /// </summary>
    public static SplatColor Aquamarine => KnownColors.FromKnownColor(KnownColor.Aquamarine);

    /// <summary>
    /// Gets a color which is azure.
    /// </summary>
    public static SplatColor Azure => KnownColors.FromKnownColor(KnownColor.Azure);

    /// <summary>
    /// Gets a color which is beige.
    /// </summary>
    public static SplatColor Beige => KnownColors.FromKnownColor(KnownColor.Beige);

    /// <summary>
    /// Gets a color which is bisque.
    /// </summary>
    public static SplatColor Bisque => KnownColors.FromKnownColor(KnownColor.Bisque);

    /// <summary>
    /// Gets a color which is black.
    /// </summary>
    public static SplatColor Black => KnownColors.FromKnownColor(KnownColor.Black);

    /// <summary>
    /// Gets a color which is blanched almond.
    /// </summary>
    public static SplatColor BlanchedAlmond => KnownColors.FromKnownColor(KnownColor.BlanchedAlmond);

    /// <summary>
    /// Gets a color which is blue.
    /// </summary>
    public static SplatColor Blue => KnownColors.FromKnownColor(KnownColor.Blue);

    /// <summary>
    /// Gets a color which is blue violet.
    /// </summary>
    public static SplatColor BlueViolet => KnownColors.FromKnownColor(KnownColor.BlueViolet);

    /// <summary>
    /// Gets a color which is brown.
    /// </summary>
    public static SplatColor Brown => KnownColors.FromKnownColor(KnownColor.Brown);

    /// <summary>
    /// Gets a color which is burly wood.
    /// </summary>
    public static SplatColor BurlyWood => KnownColors.FromKnownColor(KnownColor.BurlyWood);

    /// <summary>
    /// Gets a color which is cadet blue.
    /// </summary>
    public static SplatColor CadetBlue => KnownColors.FromKnownColor(KnownColor.CadetBlue);

    /// <summary>
    /// Gets a color which is churtreuse.
    /// </summary>
    public static SplatColor Chartreuse => KnownColors.FromKnownColor(KnownColor.Chartreuse);

    /// <summary>
    /// Gets a color which is chocolate.
    /// </summary>
    public static SplatColor Chocolate => KnownColors.FromKnownColor(KnownColor.Chocolate);

    /// <summary>
    /// Gets a color which is coral.
    /// </summary>
    public static SplatColor Coral => KnownColors.FromKnownColor(KnownColor.Coral);

    /// <summary>
    /// Gets a color which is cornflower blue.
    /// </summary>
    public static SplatColor CornflowerBlue => KnownColors.FromKnownColor(KnownColor.CornflowerBlue);

    /// <summary>
    /// Gets a color which is cornsilk.
    /// </summary>
    public static SplatColor Cornsilk => KnownColors.FromKnownColor(KnownColor.Cornsilk);

    /// <summary>
    /// Gets a color which is crimson.
    /// </summary>
    public static SplatColor Crimson => KnownColors.FromKnownColor(KnownColor.Crimson);

    /// <summary>
    /// Gets a color which is cyan.
    /// </summary>
    public static SplatColor Cyan => KnownColors.FromKnownColor(KnownColor.Cyan);

    /// <summary>
    /// Gets a color which is dark blue.
    /// </summary>
    public static SplatColor DarkBlue => KnownColors.FromKnownColor(KnownColor.DarkBlue);

    /// <summary>
    /// Gets a color which is dark cyan.
    /// </summary>
    public static SplatColor DarkCyan => KnownColors.FromKnownColor(KnownColor.DarkCyan);

    /// <summary>
    /// Gets a color which is dark goldenrod.
    /// </summary>
    public static SplatColor DarkGoldenrod => KnownColors.FromKnownColor(KnownColor.DarkGoldenrod);

    /// <summary>
    /// Gets a color which is dark gray.
    /// </summary>
    public static SplatColor DarkGray => KnownColors.FromKnownColor(KnownColor.DarkGray);

    /// <summary>
    /// Gets a color which is dark green.
    /// </summary>
    public static SplatColor DarkGreen => KnownColors.FromKnownColor(KnownColor.DarkGreen);

    /// <summary>
    /// Gets a color which is dark khaki.
    /// </summary>
    public static SplatColor DarkKhaki => KnownColors.FromKnownColor(KnownColor.DarkKhaki);

    /// <summary>
    /// Gets a color which is dark magenta.
    /// </summary>
    public static SplatColor DarkMagenta => KnownColors.FromKnownColor(KnownColor.DarkMagenta);

    /// <summary>
    /// Gets a color which is dark olive green.
    /// </summary>
    public static SplatColor DarkOliveGreen => KnownColors.FromKnownColor(KnownColor.DarkOliveGreen);

    /// <summary>
    /// Gets a color which is dark orange.
    /// </summary>
    public static SplatColor DarkOrange => KnownColors.FromKnownColor(KnownColor.DarkOrange);

    /// <summary>
    /// Gets a color which is dark orchid.
    /// </summary>
    public static SplatColor DarkOrchid => KnownColors.FromKnownColor(KnownColor.DarkOrchid);

    /// <summary>
    /// Gets a color which is dark red.
    /// </summary>
    public static SplatColor DarkRed => KnownColors.FromKnownColor(KnownColor.DarkRed);

    /// <summary>
    /// Gets a color which is dark salmon.
    /// </summary>
    public static SplatColor DarkSalmon => KnownColors.FromKnownColor(KnownColor.DarkSalmon);

    /// <summary>
    /// Gets a color which is dark sea green.
    /// </summary>
    public static SplatColor DarkSeaGreen => KnownColors.FromKnownColor(KnownColor.DarkSeaGreen);

    /// <summary>
    /// Gets a color which is dark slate blue.
    /// </summary>
    public static SplatColor DarkSlateBlue => KnownColors.FromKnownColor(KnownColor.DarkSlateBlue);

    /// <summary>
    /// Gets a color which is dark slate gray.
    /// </summary>
    public static SplatColor DarkSlateGray => KnownColors.FromKnownColor(KnownColor.DarkSlateGray);

    /// <summary>
    /// Gets a color which is dark torquoise.
    /// </summary>
    public static SplatColor DarkTurquoise => KnownColors.FromKnownColor(KnownColor.DarkTurquoise);

    /// <summary>
    /// Gets a color which is dark violet.
    /// </summary>
    public static SplatColor DarkViolet => KnownColors.FromKnownColor(KnownColor.DarkViolet);

    /// <summary>
    /// Gets a color which is deep pink.
    /// </summary>
    public static SplatColor DeepPink => KnownColors.FromKnownColor(KnownColor.DeepPink);

    /// <summary>
    /// Gets a color which is deep sky blue.
    /// </summary>
    public static SplatColor DeepSkyBlue => KnownColors.FromKnownColor(KnownColor.DeepSkyBlue);

    /// <summary>
    /// Gets a color which is dim gray.
    /// </summary>
    public static SplatColor DimGray => KnownColors.FromKnownColor(KnownColor.DimGray);

    /// <summary>
    /// Gets a color which is dodger blue.
    /// </summary>
    public static SplatColor DodgerBlue => KnownColors.FromKnownColor(KnownColor.DodgerBlue);

    /// <summary>
    /// Gets a color which is fire brick.
    /// </summary>
    public static SplatColor Firebrick => KnownColors.FromKnownColor(KnownColor.Firebrick);

    /// <summary>
    /// Gets a color which is floral white.
    /// </summary>
    public static SplatColor FloralWhite => KnownColors.FromKnownColor(KnownColor.FloralWhite);

    /// <summary>
    /// Gets a color which is forest green.
    /// </summary>
    public static SplatColor ForestGreen => KnownColors.FromKnownColor(KnownColor.ForestGreen);

    /// <summary>
    /// Gets a color which is fuchsia.
    /// </summary>
    public static SplatColor Fuchsia => KnownColors.FromKnownColor(KnownColor.Fuchsia);

    /// <summary>
    /// Gets a color which is gainsboro.
    /// </summary>
    public static SplatColor Gainsboro => KnownColors.FromKnownColor(KnownColor.Gainsboro);

    /// <summary>
    /// Gets a color which is ghost white.
    /// </summary>
    public static SplatColor GhostWhite => KnownColors.FromKnownColor(KnownColor.GhostWhite);

    /// <summary>
    /// Gets a color which is gold.
    /// </summary>
    public static SplatColor Gold => KnownColors.FromKnownColor(KnownColor.Gold);

    /// <summary>
    /// Gets a color which is golden rod.
    /// </summary>
    public static SplatColor Goldenrod => KnownColors.FromKnownColor(KnownColor.Goldenrod);

    /// <summary>
    /// Gets a color which is gray.
    /// </summary>
    public static SplatColor Gray => KnownColors.FromKnownColor(KnownColor.Gray);

    /// <summary>
    /// Gets a color which is green.
    /// </summary>
    public static SplatColor Green => KnownColors.FromKnownColor(KnownColor.Green);

    /// <summary>
    /// Gets a color which is green yellow.
    /// </summary>
    public static SplatColor GreenYellow => KnownColors.FromKnownColor(KnownColor.GreenYellow);

    /// <summary>
    /// Gets a color which is honeydew.
    /// </summary>
    public static SplatColor Honeydew => KnownColors.FromKnownColor(KnownColor.Honeydew);

    /// <summary>
    /// Gets a color which is hot pink.
    /// </summary>
    public static SplatColor HotPink => KnownColors.FromKnownColor(KnownColor.HotPink);

    /// <summary>
    /// Gets a color which is indian red.
    /// </summary>
    public static SplatColor IndianRed => KnownColors.FromKnownColor(KnownColor.IndianRed);

    /// <summary>
    /// Gets a color which is indigo.
    /// </summary>
    public static SplatColor Indigo => KnownColors.FromKnownColor(KnownColor.Indigo);

    /// <summary>
    /// Gets a color which is ivory.
    /// </summary>
    public static SplatColor Ivory => KnownColors.FromKnownColor(KnownColor.Ivory);

    /// <summary>
    /// Gets a color which is khaki.
    /// </summary>
    public static SplatColor Khaki => KnownColors.FromKnownColor(KnownColor.Khaki);

    /// <summary>
    /// Gets a color which is lavender.
    /// </summary>
    public static SplatColor Lavender => KnownColors.FromKnownColor(KnownColor.Lavender);

    /// <summary>
    /// Gets a color which is lavender blush.
    /// </summary>
    public static SplatColor LavenderBlush => KnownColors.FromKnownColor(KnownColor.LavenderBlush);

    /// <summary>
    /// Gets a color which is lawn green.
    /// </summary>
    public static SplatColor LawnGreen => KnownColors.FromKnownColor(KnownColor.LawnGreen);

    /// <summary>
    /// Gets a color which is lemon chiffon.
    /// </summary>
    public static SplatColor LemonChiffon => KnownColors.FromKnownColor(KnownColor.LemonChiffon);

    /// <summary>
    /// Gets a color which is light blue.
    /// </summary>
    public static SplatColor LightBlue => KnownColors.FromKnownColor(KnownColor.LightBlue);

    /// <summary>
    /// Gets a color which is light coral.
    /// </summary>
    public static SplatColor LightCoral => KnownColors.FromKnownColor(KnownColor.LightCoral);

    /// <summary>
    /// Gets a color which is light cyan.
    /// </summary>
    public static SplatColor LightCyan => KnownColors.FromKnownColor(KnownColor.LightCyan);

    /// <summary>
    /// Gets a color which is light golden rod yellow.
    /// </summary>
    public static SplatColor LightGoldenrodYellow => KnownColors.FromKnownColor(KnownColor.LightGoldenrodYellow);

    /// <summary>
    /// Gets a value which is light green.
    /// </summary>
    public static SplatColor LightGreen => KnownColors.FromKnownColor(KnownColor.LightGreen);

    /// <summary>
    /// Gets a color which is light gray.
    /// </summary>
    public static SplatColor LightGray => KnownColors.FromKnownColor(KnownColor.LightGray);

    /// <summary>
    /// Gets a color which is light pink.
    /// </summary>
    public static SplatColor LightPink => KnownColors.FromKnownColor(KnownColor.LightPink);

    /// <summary>
    /// Gets a color which is light salmon.
    /// </summary>
    public static SplatColor LightSalmon => KnownColors.FromKnownColor(KnownColor.LightSalmon);

    /// <summary>
    /// Gets a color which is light sea green.
    /// </summary>
    public static SplatColor LightSeaGreen => KnownColors.FromKnownColor(KnownColor.LightSeaGreen);

    /// <summary>
    /// Gets a color which is light sky blue.
    /// </summary>
    public static SplatColor LightSkyBlue => KnownColors.FromKnownColor(KnownColor.LightSkyBlue);

    /// <summary>
    /// Gets a color which is light slate gray.
    /// </summary>
    public static SplatColor LightSlateGray => KnownColors.FromKnownColor(KnownColor.LightSlateGray);

    /// <summary>
    /// Gets a color which is light steel blue.
    /// </summary>
    public static SplatColor LightSteelBlue => KnownColors.FromKnownColor(KnownColor.LightSteelBlue);

    /// <summary>
    /// Gets a color which is light yellow.
    /// </summary>
    public static SplatColor LightYellow => KnownColors.FromKnownColor(KnownColor.LightYellow);

    /// <summary>
    /// Gets a color which is lime.
    /// </summary>
    public static SplatColor Lime => KnownColors.FromKnownColor(KnownColor.Lime);

    /// <summary>
    /// Gets a color which is lime green.
    /// </summary>
    public static SplatColor LimeGreen => KnownColors.FromKnownColor(KnownColor.LimeGreen);

    /// <summary>
    /// Gets a color which is linen.
    /// </summary>
    public static SplatColor Linen => KnownColors.FromKnownColor(KnownColor.Linen);

    /// <summary>
    /// Gets a color which is magenta.
    /// </summary>
    public static SplatColor Magenta => KnownColors.FromKnownColor(KnownColor.Magenta);

    /// <summary>
    /// Gets a color which is maroon.
    /// </summary>
    public static SplatColor Maroon => KnownColors.FromKnownColor(KnownColor.Maroon);

    /// <summary>
    /// Gets a color which is medium aquamarine.
    /// </summary>
    public static SplatColor MediumAquamarine => KnownColors.FromKnownColor(KnownColor.MediumAquamarine);

    /// <summary>
    /// Gets a color which is medium blue.
    /// </summary>
    public static SplatColor MediumBlue => KnownColors.FromKnownColor(KnownColor.MediumBlue);

    /// <summary>
    /// Gets a color which is medium orchid.
    /// </summary>
    public static SplatColor MediumOrchid => KnownColors.FromKnownColor(KnownColor.MediumOrchid);

    /// <summary>
    /// Gets a color which is medium purple.
    /// </summary>
    public static SplatColor MediumPurple => KnownColors.FromKnownColor(KnownColor.MediumPurple);

    /// <summary>
    /// Gets a color which is medium sea green.
    /// </summary>
    public static SplatColor MediumSeaGreen => KnownColors.FromKnownColor(KnownColor.MediumSeaGreen);

    /// <summary>
    /// Gets a color which is medium slate blue.
    /// </summary>
    public static SplatColor MediumSlateBlue => KnownColors.FromKnownColor(KnownColor.MediumSlateBlue);

    /// <summary>
    /// Gets a color which is medium spring green.
    /// </summary>
    public static SplatColor MediumSpringGreen => KnownColors.FromKnownColor(KnownColor.MediumSpringGreen);

    /// <summary>
    /// Gets a color which is medium turquoise.
    /// </summary>
    public static SplatColor MediumTurquoise => KnownColors.FromKnownColor(KnownColor.MediumTurquoise);

    /// <summary>
    /// Gets a color which is medium violet red.
    /// </summary>
    public static SplatColor MediumVioletRed => KnownColors.FromKnownColor(KnownColor.MediumVioletRed);

    /// <summary>
    /// Gets a color which is midnight blue.
    /// </summary>
    public static SplatColor MidnightBlue => KnownColors.FromKnownColor(KnownColor.MidnightBlue);

    /// <summary>
    /// Gets a color which is mint cream.
    /// </summary>
    public static SplatColor MintCream => KnownColors.FromKnownColor(KnownColor.MintCream);

    /// <summary>
    /// Gets a color which is misty rose.
    /// </summary>
    public static SplatColor MistyRose => KnownColors.FromKnownColor(KnownColor.MistyRose);

    /// <summary>
    /// Gets a color which is moccasin.
    /// </summary>
    public static SplatColor Moccasin => KnownColors.FromKnownColor(KnownColor.Moccasin);

    /// <summary>
    /// Gets a color which is navajo white.
    /// </summary>
    public static SplatColor NavajoWhite => KnownColors.FromKnownColor(KnownColor.NavajoWhite);

    /// <summary>
    /// Gets a color which is navy.
    /// </summary>
    public static SplatColor Navy => KnownColors.FromKnownColor(KnownColor.Navy);

    /// <summary>
    /// Gets a color hwich is old lace.
    /// </summary>
    public static SplatColor OldLace => KnownColors.FromKnownColor(KnownColor.OldLace);

    /// <summary>
    /// Gets a color which is olive.
    /// </summary>
    public static SplatColor Olive => KnownColors.FromKnownColor(KnownColor.Olive);

    /// <summary>
    /// Gets a color which is olive drab.
    /// </summary>
    public static SplatColor OliveDrab => KnownColors.FromKnownColor(KnownColor.OliveDrab);

    /// <summary>
    /// Gets a color which is orange.
    /// </summary>
    public static SplatColor Orange => KnownColors.FromKnownColor(KnownColor.Orange);

    /// <summary>
    /// Gets a color which is orange red.
    /// </summary>
    public static SplatColor OrangeRed => KnownColors.FromKnownColor(KnownColor.OrangeRed);

    /// <summary>
    /// Gets a color which is orchid.
    /// </summary>
    public static SplatColor Orchid => KnownColors.FromKnownColor(KnownColor.Orchid);

    /// <summary>
    /// Gets a color which is pale golden rod.
    /// </summary>
    public static SplatColor PaleGoldenrod => KnownColors.FromKnownColor(KnownColor.PaleGoldenrod);

    /// <summary>
    /// Gets a color which is pale green.
    /// </summary>
    public static SplatColor PaleGreen => KnownColors.FromKnownColor(KnownColor.PaleGreen);

    /// <summary>
    /// Gets a color which is pale turquoise.
    /// </summary>
    public static SplatColor PaleTurquoise => KnownColors.FromKnownColor(KnownColor.PaleTurquoise);

    /// <summary>
    /// Gets a color which is pale violet red.
    /// </summary>
    public static SplatColor PaleVioletRed => KnownColors.FromKnownColor(KnownColor.PaleVioletRed);

    /// <summary>
    /// Gets a color which is papaya whip.
    /// </summary>
    public static SplatColor PapayaWhip => KnownColors.FromKnownColor(KnownColor.PapayaWhip);

    /// <summary>
    /// Gets a color which is peach puff.
    /// </summary>
    public static SplatColor PeachPuff => KnownColors.FromKnownColor(KnownColor.PeachPuff);

    /// <summary>
    /// Gets a color which is peru.
    /// </summary>
    public static SplatColor Peru => KnownColors.FromKnownColor(KnownColor.Peru);

    /// <summary>
    /// Gets a color which is pink.
    /// </summary>
    public static SplatColor Pink => KnownColors.FromKnownColor(KnownColor.Pink);

    /// <summary>
    /// Gets a color which is plum.
    /// </summary>
    public static SplatColor Plum => KnownColors.FromKnownColor(KnownColor.Plum);

    /// <summary>
    /// Gets a color which is powder blue.
    /// </summary>
    public static SplatColor PowderBlue => KnownColors.FromKnownColor(KnownColor.PowderBlue);

    /// <summary>
    /// Gets a color which is purple.
    /// </summary>
    public static SplatColor Purple => KnownColors.FromKnownColor(KnownColor.Purple);

    /// <summary>
    /// Gets a color which is red.
    /// </summary>
    public static SplatColor Red => KnownColors.FromKnownColor(KnownColor.Red);

    /// <summary>
    /// Gets a color which is rosy brown.
    /// </summary>
    public static SplatColor RosyBrown => KnownColors.FromKnownColor(KnownColor.RosyBrown);

    /// <summary>
    /// Gets a color which is royal blue.
    /// </summary>
    public static SplatColor RoyalBlue => KnownColors.FromKnownColor(KnownColor.RoyalBlue);

    /// <summary>
    /// Gets a color which is saddle brown.
    /// </summary>
    public static SplatColor SaddleBrown => KnownColors.FromKnownColor(KnownColor.SaddleBrown);

    /// <summary>
    /// Gets a color which is salmon.
    /// </summary>
    public static SplatColor Salmon => KnownColors.FromKnownColor(KnownColor.Salmon);

    /// <summary>
    /// Gets a color which is sandy brown.
    /// </summary>
    public static SplatColor SandyBrown => KnownColors.FromKnownColor(KnownColor.SandyBrown);

    /// <summary>
    /// Gets a color which is sea green.
    /// </summary>
    public static SplatColor SeaGreen => KnownColors.FromKnownColor(KnownColor.SeaGreen);

    /// <summary>
    /// Gets a color which is sea shell.
    /// </summary>
    public static SplatColor SeaShell => KnownColors.FromKnownColor(KnownColor.SeaShell);

    /// <summary>
    /// Gets a color which is sienna.
    /// </summary>
    public static SplatColor Sienna => KnownColors.FromKnownColor(KnownColor.Sienna);

    /// <summary>
    /// Gets a color which is silver.
    /// </summary>
    public static SplatColor Silver => KnownColors.FromKnownColor(KnownColor.Silver);

    /// <summary>
    /// Gets a color which is sky blue.
    /// </summary>
    public static SplatColor SkyBlue => KnownColors.FromKnownColor(KnownColor.SkyBlue);

    /// <summary>
    /// Gets a color which is slate blue.
    /// </summary>
    public static SplatColor SlateBlue => KnownColors.FromKnownColor(KnownColor.SlateBlue);

    /// <summary>
    /// Gets a color which is slate gray.
    /// </summary>
    public static SplatColor SlateGray => KnownColors.FromKnownColor(KnownColor.SlateGray);

    /// <summary>
    /// Gets a color which is snow.
    /// </summary>
    public static SplatColor Snow => KnownColors.FromKnownColor(KnownColor.Snow);

    /// <summary>
    /// Gets a color which is spring green.
    /// </summary>
    public static SplatColor SpringGreen => KnownColors.FromKnownColor(KnownColor.SpringGreen);

    /// <summary>
    /// Gets a color which is steel blue.
    /// </summary>
    public static SplatColor SteelBlue => KnownColors.FromKnownColor(KnownColor.SteelBlue);

    /// <summary>
    /// Gets a color which is tan.
    /// </summary>
    public static SplatColor Tan => KnownColors.FromKnownColor(KnownColor.Tan);

    /// <summary>
    /// Gets a color which is teal.
    /// </summary>
    public static SplatColor Teal => KnownColors.FromKnownColor(KnownColor.Teal);

    /// <summary>
    /// Gets a color which is thistle.
    /// </summary>
    public static SplatColor Thistle => KnownColors.FromKnownColor(KnownColor.Thistle);

    /// <summary>
    /// Gets a color which is tomato.
    /// </summary>
    public static SplatColor Tomato => KnownColors.FromKnownColor(KnownColor.Tomato);

    /// <summary>
    /// Gets a color which is turquoise.
    /// </summary>
    public static SplatColor Turquoise => KnownColors.FromKnownColor(KnownColor.Turquoise);

    /// <summary>
    /// Gets a color which is violet.
    /// </summary>
    public static SplatColor Violet => KnownColors.FromKnownColor(KnownColor.Violet);

    /// <summary>
    /// Gets a color which is wheat.
    /// </summary>
    public static SplatColor Wheat => KnownColors.FromKnownColor(KnownColor.Wheat);

    /// <summary>
    /// Gets a color which is white.
    /// </summary>
    public static SplatColor White => KnownColors.FromKnownColor(KnownColor.White);

    /// <summary>
    /// Gets a color which is white smoke.
    /// </summary>
    public static SplatColor WhiteSmoke => KnownColors.FromKnownColor(KnownColor.WhiteSmoke);

    /// <summary>
    /// Gets a color which is yellow.
    /// </summary>
    public static SplatColor Yellow => KnownColors.FromKnownColor(KnownColor.Yellow);

    /// <summary>
    /// Gets a color which is yellow green.
    /// </summary>
    public static SplatColor YellowGreen => KnownColors.FromKnownColor(KnownColor.YellowGreen);
}
