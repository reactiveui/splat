// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests.Colors;

[NotInParallel] // Uses Locator (global static); keep this fixture serialized.
public class CoverageColorTests
{
    private const float Eps = 1e-6f;

    /// <summary>
    /// Initialize Splat before each test (matches xUnit ctor-per-test semantics).
    /// </summary>
    [Before(HookType.Test)]
    public void SetUp() => Locator.CurrentMutable.InitializeSplat();

    /// <summary>
    /// Colors the is empty.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsEmpty()
    {
        var fixture = SplatColor.Empty;

        using (Assert.Multiple())
        {
            await Assert.That(fixture.IsEmpty).IsTrue();
            await Assert.That(fixture.A).IsEqualTo((byte)0);
            await Assert.That(fixture.R).IsEqualTo((byte)0);
            await Assert.That(fixture.G).IsEqualTo((byte)0);
            await Assert.That(fixture.B).IsEqualTo((byte)0);
        }
    }

    /// <summary>
    /// Colors the is equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// ARGBs the color is equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ARGBColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// ARGBs the hexadecimal color is equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ARGBHexColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(0xFF00008B);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// ARGBs the and named color is equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ARGBAndNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// ARGBs the based on named color is equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ARGBBasedOnNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(255, fixture1);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// Incorrect named color is equal to empty.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IncorrectNamedColorIsEqualToEmpty()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        var fixture2 = SplatColor.Empty;

        using (Assert.Multiple())
        {
            await Assert.That(fixture1).IsEqualTo(fixture2);
            await Assert.That(fixture1.Name).IsEqualTo("TheBestColor");
        }
    }

    /// <summary>
    /// Colors the is not equal.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsNotEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(fixture1).IsNotEqualTo(fixture2);
    }

    /// <summary>
    /// Colors the brightness.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorBrightnessHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var brightness = fixture.GetBrightness();

        await Assert.That(brightness).IsEqualTo(0.272549033f).Within(Eps);
    }

    /// <summary>
    /// Colors the saturation has correct value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorSaturationHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var saturation = fixture.GetSaturation();

        await Assert.That(saturation).IsEqualTo(1f).Within(Eps);
    }

    /// <summary>
    /// Colors the hue has correct value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorHueHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var hue = fixture.GetHue();

        await Assert.That(hue).IsEqualTo(240f).Within(Eps);
    }

    /// <summary>
    /// Colors to known color has correct value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorToKnownColorHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var color = fixture.ToKnownColor();

        await Assert.That(color).IsEqualTo(KnownColor.DarkBlue);
    }

    /// <summary>
    /// Colors the is equals.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsEquals()
    {
        object fixture1 = SplatColor.FromArgb(0xFF00008B);
        object fixture2 = SplatColor.FromArgb(0, 0, 139);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>
    /// Incorrects the named color to string gives value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IncorrectNamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [TheBestColor]");
    }

    /// <summary>
    /// Nameds the color to string gives value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [DarkBlue]");
    }

    /// <summary>
    /// ARGBs the color to string gives value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ARGBColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [A=255, R=0, G=0, B=138]");
    }

    /// <summary>
    /// Invalids the ARGB color A throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidARGBColorAThrows()
        => await Assert.That(() => SplatColor.FromArgb(256, 0, 0, 0)).Throws<ArgumentException>();

    /// <summary>
    /// Invalids the ARGB color r throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidARGBColorRThrows()
        => await Assert.That(() => SplatColor.FromArgb(0, 256, 0, 0)).Throws<ArgumentException>();

    /// <summary>
    /// Invalids the ARGB color g throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidARGBColorGThrows()
        => await Assert.That(() => SplatColor.FromArgb(0, 0, 256, 0)).Throws<ArgumentException>();

    /// <summary>
    /// Invalids the ARGB color b throws.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidARGBColorBThrows()
        => await Assert.That(() => SplatColor.FromArgb(0, 0, 0, 256)).Throws<ArgumentException>();

    // Known color static properties tests - each property is tested individually for coverage
    [Test]
    public async Task KnownColor_Transparent_IsValid() => await Assert.That(SplatColor.Transparent).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Transparent));

    [Test]
    public async Task KnownColor_AliceBlue_IsValid() => await Assert.That(SplatColor.AliceBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.AliceBlue));

    [Test]
    public async Task KnownColor_AntiqueWhite_IsValid() => await Assert.That(SplatColor.AntiqueWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.AntiqueWhite));

    [Test]
    public async Task KnownColor_Aqua_IsValid() => await Assert.That(SplatColor.Aqua).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Aqua));

    [Test]
    public async Task KnownColor_Aquamarine_IsValid() => await Assert.That(SplatColor.Aquamarine).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Aquamarine));

    [Test]
    public async Task KnownColor_Azure_IsValid() => await Assert.That(SplatColor.Azure).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Azure));

    [Test]
    public async Task KnownColor_Beige_IsValid() => await Assert.That(SplatColor.Beige).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Beige));

    [Test]
    public async Task KnownColor_Bisque_IsValid() => await Assert.That(SplatColor.Bisque).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Bisque));

    [Test]
    public async Task KnownColor_Black_IsValid() => await Assert.That(SplatColor.Black).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Black));

    [Test]
    public async Task KnownColor_BlanchedAlmond_IsValid() => await Assert.That(SplatColor.BlanchedAlmond).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BlanchedAlmond));

    [Test]
    public async Task KnownColor_Blue_IsValid() => await Assert.That(SplatColor.Blue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Blue));

    [Test]
    public async Task KnownColor_BlueViolet_IsValid() => await Assert.That(SplatColor.BlueViolet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BlueViolet));

    [Test]
    public async Task KnownColor_Brown_IsValid() => await Assert.That(SplatColor.Brown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Brown));

    [Test]
    public async Task KnownColor_BurlyWood_IsValid() => await Assert.That(SplatColor.BurlyWood).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BurlyWood));

    [Test]
    public async Task KnownColor_CadetBlue_IsValid() => await Assert.That(SplatColor.CadetBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.CadetBlue));

    [Test]
    public async Task KnownColor_Chartreuse_IsValid() => await Assert.That(SplatColor.Chartreuse).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Chartreuse));

    [Test]
    public async Task KnownColor_Chocolate_IsValid() => await Assert.That(SplatColor.Chocolate).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Chocolate));

    [Test]
    public async Task KnownColor_Coral_IsValid() => await Assert.That(SplatColor.Coral).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Coral));

    [Test]
    public async Task KnownColor_CornflowerBlue_IsValid() => await Assert.That(SplatColor.CornflowerBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.CornflowerBlue));

    [Test]
    public async Task KnownColor_Cornsilk_IsValid() => await Assert.That(SplatColor.Cornsilk).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Cornsilk));

    [Test]
    public async Task KnownColor_Crimson_IsValid() => await Assert.That(SplatColor.Crimson).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Crimson));

    [Test]
    public async Task KnownColor_Cyan_IsValid() => await Assert.That(SplatColor.Cyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Cyan));

    [Test]
    public async Task KnownColor_DarkBlue_IsValid() => await Assert.That(SplatColor.DarkBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkBlue));

    [Test]
    public async Task KnownColor_DarkCyan_IsValid() => await Assert.That(SplatColor.DarkCyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkCyan));

    [Test]
    public async Task KnownColor_DarkGoldenrod_IsValid() => await Assert.That(SplatColor.DarkGoldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGoldenrod));

    [Test]
    public async Task KnownColor_DarkGray_IsValid() => await Assert.That(SplatColor.DarkGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGray));

    [Test]
    public async Task KnownColor_DarkGreen_IsValid() => await Assert.That(SplatColor.DarkGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGreen));

    [Test]
    public async Task KnownColor_DarkKhaki_IsValid() => await Assert.That(SplatColor.DarkKhaki).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkKhaki));

    [Test]
    public async Task KnownColor_DarkMagenta_IsValid() => await Assert.That(SplatColor.DarkMagenta).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkMagenta));

    [Test]
    public async Task KnownColor_DarkOliveGreen_IsValid() => await Assert.That(SplatColor.DarkOliveGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOliveGreen));

    [Test]
    public async Task KnownColor_DarkOrange_IsValid() => await Assert.That(SplatColor.DarkOrange).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOrange));

    [Test]
    public async Task KnownColor_DarkOrchid_IsValid() => await Assert.That(SplatColor.DarkOrchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOrchid));

    [Test]
    public async Task KnownColor_DarkRed_IsValid() => await Assert.That(SplatColor.DarkRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkRed));

    [Test]
    public async Task KnownColor_DarkSalmon_IsValid() => await Assert.That(SplatColor.DarkSalmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSalmon));

    [Test]
    public async Task KnownColor_DarkSeaGreen_IsValid() => await Assert.That(SplatColor.DarkSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSeaGreen));

    [Test]
    public async Task KnownColor_DarkSlateBlue_IsValid() => await Assert.That(SplatColor.DarkSlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSlateBlue));

    [Test]
    public async Task KnownColor_DarkSlateGray_IsValid() => await Assert.That(SplatColor.DarkSlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSlateGray));

    [Test]
    public async Task KnownColor_DarkTurquoise_IsValid() => await Assert.That(SplatColor.DarkTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkTurquoise));

    [Test]
    public async Task KnownColor_DarkViolet_IsValid() => await Assert.That(SplatColor.DarkViolet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkViolet));

    [Test]
    public async Task KnownColor_DeepPink_IsValid() => await Assert.That(SplatColor.DeepPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DeepPink));

    [Test]
    public async Task KnownColor_DeepSkyBlue_IsValid() => await Assert.That(SplatColor.DeepSkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DeepSkyBlue));

    [Test]
    public async Task KnownColor_DimGray_IsValid() => await Assert.That(SplatColor.DimGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DimGray));

    [Test]
    public async Task KnownColor_DodgerBlue_IsValid() => await Assert.That(SplatColor.DodgerBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DodgerBlue));

    [Test]
    public async Task KnownColor_Firebrick_IsValid() => await Assert.That(SplatColor.Firebrick).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Firebrick));

    [Test]
    public async Task KnownColor_FloralWhite_IsValid() => await Assert.That(SplatColor.FloralWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.FloralWhite));

    [Test]
    public async Task KnownColor_ForestGreen_IsValid() => await Assert.That(SplatColor.ForestGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.ForestGreen));

    [Test]
    public async Task KnownColor_Fuchsia_IsValid() => await Assert.That(SplatColor.Fuchsia).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Fuchsia));

    [Test]
    public async Task KnownColor_Gainsboro_IsValid() => await Assert.That(SplatColor.Gainsboro).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gainsboro));

    [Test]
    public async Task KnownColor_GhostWhite_IsValid() => await Assert.That(SplatColor.GhostWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.GhostWhite));

    [Test]
    public async Task KnownColor_Gold_IsValid() => await Assert.That(SplatColor.Gold).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gold));

    [Test]
    public async Task KnownColor_Goldenrod_IsValid() => await Assert.That(SplatColor.Goldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Goldenrod));

    [Test]
    public async Task KnownColor_Gray_IsValid() => await Assert.That(SplatColor.Gray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gray));

    [Test]
    public async Task KnownColor_Green_IsValid() => await Assert.That(SplatColor.Green).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Green));

    [Test]
    public async Task KnownColor_GreenYellow_IsValid() => await Assert.That(SplatColor.GreenYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.GreenYellow));

    [Test]
    public async Task KnownColor_Honeydew_IsValid() => await Assert.That(SplatColor.Honeydew).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Honeydew));

    [Test]
    public async Task KnownColor_HotPink_IsValid() => await Assert.That(SplatColor.HotPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.HotPink));

    [Test]
    public async Task KnownColor_IndianRed_IsValid() => await Assert.That(SplatColor.IndianRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.IndianRed));

    [Test]
    public async Task KnownColor_Indigo_IsValid() => await Assert.That(SplatColor.Indigo).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Indigo));

    [Test]
    public async Task KnownColor_Ivory_IsValid() => await Assert.That(SplatColor.Ivory).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Ivory));

    [Test]
    public async Task KnownColor_Khaki_IsValid() => await Assert.That(SplatColor.Khaki).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Khaki));

    [Test]
    public async Task KnownColor_Lavender_IsValid() => await Assert.That(SplatColor.Lavender).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Lavender));

    [Test]
    public async Task KnownColor_LavenderBlush_IsValid() => await Assert.That(SplatColor.LavenderBlush).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LavenderBlush));

    [Test]
    public async Task KnownColor_LawnGreen_IsValid() => await Assert.That(SplatColor.LawnGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LawnGreen));

    [Test]
    public async Task KnownColor_LemonChiffon_IsValid() => await Assert.That(SplatColor.LemonChiffon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LemonChiffon));

    [Test]
    public async Task KnownColor_LightBlue_IsValid() => await Assert.That(SplatColor.LightBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightBlue));

    [Test]
    public async Task KnownColor_LightCoral_IsValid() => await Assert.That(SplatColor.LightCoral).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightCoral));

    [Test]
    public async Task KnownColor_LightCyan_IsValid() => await Assert.That(SplatColor.LightCyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightCyan));

    [Test]
    public async Task KnownColor_LightGoldenrodYellow_IsValid() => await Assert.That(SplatColor.LightGoldenrodYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGoldenrodYellow));

    [Test]
    public async Task KnownColor_LightGray_IsValid() => await Assert.That(SplatColor.LightGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGray));

    [Test]
    public async Task KnownColor_LightGreen_IsValid() => await Assert.That(SplatColor.LightGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGreen));

    [Test]
    public async Task KnownColor_LightPink_IsValid() => await Assert.That(SplatColor.LightPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightPink));

    [Test]
    public async Task KnownColor_LightSalmon_IsValid() => await Assert.That(SplatColor.LightSalmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSalmon));

    [Test]
    public async Task KnownColor_LightSeaGreen_IsValid() => await Assert.That(SplatColor.LightSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSeaGreen));

    [Test]
    public async Task KnownColor_LightSkyBlue_IsValid() => await Assert.That(SplatColor.LightSkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSkyBlue));

    [Test]
    public async Task KnownColor_LightSlateGray_IsValid() => await Assert.That(SplatColor.LightSlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSlateGray));

    [Test]
    public async Task KnownColor_LightSteelBlue_IsValid() => await Assert.That(SplatColor.LightSteelBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSteelBlue));

    [Test]
    public async Task KnownColor_LightYellow_IsValid() => await Assert.That(SplatColor.LightYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightYellow));

    [Test]
    public async Task KnownColor_Lime_IsValid() => await Assert.That(SplatColor.Lime).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Lime));

    [Test]
    public async Task KnownColor_LimeGreen_IsValid() => await Assert.That(SplatColor.LimeGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LimeGreen));

    [Test]
    public async Task KnownColor_Linen_IsValid() => await Assert.That(SplatColor.Linen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Linen));

    [Test]
    public async Task KnownColor_Magenta_IsValid() => await Assert.That(SplatColor.Magenta).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Magenta));

    [Test]
    public async Task KnownColor_Maroon_IsValid() => await Assert.That(SplatColor.Maroon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Maroon));

    [Test]
    public async Task KnownColor_MediumAquamarine_IsValid() => await Assert.That(SplatColor.MediumAquamarine).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumAquamarine));

    [Test]
    public async Task KnownColor_MediumBlue_IsValid() => await Assert.That(SplatColor.MediumBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumBlue));

    [Test]
    public async Task KnownColor_MediumOrchid_IsValid() => await Assert.That(SplatColor.MediumOrchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumOrchid));

    [Test]
    public async Task KnownColor_MediumPurple_IsValid() => await Assert.That(SplatColor.MediumPurple).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumPurple));

    [Test]
    public async Task KnownColor_MediumSeaGreen_IsValid() => await Assert.That(SplatColor.MediumSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSeaGreen));

    [Test]
    public async Task KnownColor_MediumSlateBlue_IsValid() => await Assert.That(SplatColor.MediumSlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSlateBlue));

    [Test]
    public async Task KnownColor_MediumSpringGreen_IsValid() => await Assert.That(SplatColor.MediumSpringGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSpringGreen));

    [Test]
    public async Task KnownColor_MediumTurquoise_IsValid() => await Assert.That(SplatColor.MediumTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumTurquoise));

    [Test]
    public async Task KnownColor_MediumVioletRed_IsValid() => await Assert.That(SplatColor.MediumVioletRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumVioletRed));

    [Test]
    public async Task KnownColor_MidnightBlue_IsValid() => await Assert.That(SplatColor.MidnightBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MidnightBlue));

    [Test]
    public async Task KnownColor_MintCream_IsValid() => await Assert.That(SplatColor.MintCream).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MintCream));

    [Test]
    public async Task KnownColor_MistyRose_IsValid() => await Assert.That(SplatColor.MistyRose).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MistyRose));

    [Test]
    public async Task KnownColor_Moccasin_IsValid() => await Assert.That(SplatColor.Moccasin).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Moccasin));

    [Test]
    public async Task KnownColor_NavajoWhite_IsValid() => await Assert.That(SplatColor.NavajoWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.NavajoWhite));

    [Test]
    public async Task KnownColor_Navy_IsValid() => await Assert.That(SplatColor.Navy).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Navy));

    [Test]
    public async Task KnownColor_OldLace_IsValid() => await Assert.That(SplatColor.OldLace).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OldLace));

    [Test]
    public async Task KnownColor_Olive_IsValid() => await Assert.That(SplatColor.Olive).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Olive));

    [Test]
    public async Task KnownColor_OliveDrab_IsValid() => await Assert.That(SplatColor.OliveDrab).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OliveDrab));

    [Test]
    public async Task KnownColor_Orange_IsValid() => await Assert.That(SplatColor.Orange).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Orange));

    [Test]
    public async Task KnownColor_OrangeRed_IsValid() => await Assert.That(SplatColor.OrangeRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OrangeRed));

    [Test]
    public async Task KnownColor_Orchid_IsValid() => await Assert.That(SplatColor.Orchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Orchid));

    [Test]
    public async Task KnownColor_PaleGoldenrod_IsValid() => await Assert.That(SplatColor.PaleGoldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleGoldenrod));

    [Test]
    public async Task KnownColor_PaleGreen_IsValid() => await Assert.That(SplatColor.PaleGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleGreen));

    [Test]
    public async Task KnownColor_PaleTurquoise_IsValid() => await Assert.That(SplatColor.PaleTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleTurquoise));

    [Test]
    public async Task KnownColor_PaleVioletRed_IsValid() => await Assert.That(SplatColor.PaleVioletRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleVioletRed));

    [Test]
    public async Task KnownColor_PapayaWhip_IsValid() => await Assert.That(SplatColor.PapayaWhip).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PapayaWhip));

    [Test]
    public async Task KnownColor_PeachPuff_IsValid() => await Assert.That(SplatColor.PeachPuff).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PeachPuff));

    [Test]
    public async Task KnownColor_Peru_IsValid() => await Assert.That(SplatColor.Peru).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Peru));

    [Test]
    public async Task KnownColor_Pink_IsValid() => await Assert.That(SplatColor.Pink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Pink));

    [Test]
    public async Task KnownColor_Plum_IsValid() => await Assert.That(SplatColor.Plum).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Plum));

    [Test]
    public async Task KnownColor_PowderBlue_IsValid() => await Assert.That(SplatColor.PowderBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PowderBlue));

    [Test]
    public async Task KnownColor_Purple_IsValid() => await Assert.That(SplatColor.Purple).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Purple));

    [Test]
    public async Task KnownColor_Red_IsValid() => await Assert.That(SplatColor.Red).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Red));

    [Test]
    public async Task KnownColor_RosyBrown_IsValid() => await Assert.That(SplatColor.RosyBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.RosyBrown));

    [Test]
    public async Task KnownColor_RoyalBlue_IsValid() => await Assert.That(SplatColor.RoyalBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.RoyalBlue));

    [Test]
    public async Task KnownColor_SaddleBrown_IsValid() => await Assert.That(SplatColor.SaddleBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SaddleBrown));

    [Test]
    public async Task KnownColor_Salmon_IsValid() => await Assert.That(SplatColor.Salmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Salmon));

    [Test]
    public async Task KnownColor_SandyBrown_IsValid() => await Assert.That(SplatColor.SandyBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SandyBrown));

    [Test]
    public async Task KnownColor_SeaGreen_IsValid() => await Assert.That(SplatColor.SeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SeaGreen));

    [Test]
    public async Task KnownColor_SeaShell_IsValid() => await Assert.That(SplatColor.SeaShell).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SeaShell));

    [Test]
    public async Task KnownColor_Sienna_IsValid() => await Assert.That(SplatColor.Sienna).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Sienna));

    [Test]
    public async Task KnownColor_Silver_IsValid() => await Assert.That(SplatColor.Silver).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Silver));

    [Test]
    public async Task KnownColor_SkyBlue_IsValid() => await Assert.That(SplatColor.SkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SkyBlue));

    [Test]
    public async Task KnownColor_SlateBlue_IsValid() => await Assert.That(SplatColor.SlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SlateBlue));

    [Test]
    public async Task KnownColor_SlateGray_IsValid() => await Assert.That(SplatColor.SlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SlateGray));

    [Test]
    public async Task KnownColor_Snow_IsValid() => await Assert.That(SplatColor.Snow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Snow));

    [Test]
    public async Task KnownColor_SpringGreen_IsValid() => await Assert.That(SplatColor.SpringGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SpringGreen));

    [Test]
    public async Task KnownColor_SteelBlue_IsValid() => await Assert.That(SplatColor.SteelBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SteelBlue));

    [Test]
    public async Task KnownColor_Tan_IsValid() => await Assert.That(SplatColor.Tan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Tan));

    [Test]
    public async Task KnownColor_Teal_IsValid() => await Assert.That(SplatColor.Teal).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Teal));

    [Test]
    public async Task KnownColor_Thistle_IsValid() => await Assert.That(SplatColor.Thistle).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Thistle));

    [Test]
    public async Task KnownColor_Tomato_IsValid() => await Assert.That(SplatColor.Tomato).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Tomato));

    [Test]
    public async Task KnownColor_Turquoise_IsValid() => await Assert.That(SplatColor.Turquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Turquoise));

    [Test]
    public async Task KnownColor_Violet_IsValid() => await Assert.That(SplatColor.Violet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Violet));

    [Test]
    public async Task KnownColor_Wheat_IsValid() => await Assert.That(SplatColor.Wheat).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Wheat));

    [Test]
    public async Task KnownColor_White_IsValid() => await Assert.That(SplatColor.White).IsEqualTo(SplatColor.FromKnownColor(KnownColor.White));

    [Test]
    public async Task KnownColor_WhiteSmoke_IsValid() => await Assert.That(SplatColor.WhiteSmoke).IsEqualTo(SplatColor.FromKnownColor(KnownColor.WhiteSmoke));

    [Test]
    public async Task KnownColor_Yellow_IsValid() => await Assert.That(SplatColor.Yellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Yellow));

    [Test]
    public async Task KnownColor_YellowGreen_IsValid() => await Assert.That(SplatColor.YellowGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.YellowGreen));
}
