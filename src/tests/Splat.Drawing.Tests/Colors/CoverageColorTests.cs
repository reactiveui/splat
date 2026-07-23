// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests.Colors;

/// <summary>Unit tests covering <see cref="SplatColor"/> behaviour and known color values.</summary>
[NotInParallel] // Uses Locator (global static); keep this fixture serialized.
public class CoverageColorTests
{
    /// <summary>Tolerance used when comparing floating-point color components.</summary>
    private const float Eps = 1e-6F;

    /// <summary>The expected brightness of <see cref="KnownColor.DarkBlue"/>.</summary>
    private const float DarkBlueBrightness = 0.272549033F;

    /// <summary>The expected hue, in degrees, of <see cref="KnownColor.DarkBlue"/>.</summary>
    private const float DarkBlueHue = 240F;

    /// <summary>Fully opaque alpha component.</summary>
    private const int FullAlpha = 255;

    /// <summary>The blue component of <see cref="KnownColor.DarkBlue"/>.</summary>
    private const int DarkBlueBlue = 139;

    /// <summary>A blue component that differs from <see cref="KnownColor.DarkBlue"/>, used to build a non-matching color.</summary>
    private const int NonDarkBlueBlue = 138;

    /// <summary>One past the maximum valid ARGB component value, used to assert out-of-range rejection.</summary>
    private const int ComponentRange = 256;

    /// <summary>A color name that does not map to any known color.</summary>
    private const string UnknownColorName = "TheBestColor";

    /// <summary>Initialize Splat before each test (fresh state per test).</summary>
    [Before(Test)]
    public void SetUp() => Locator.CurrentMutable.InitializeSplat();

    /// <summary>Colors the is empty.</summary>
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

    /// <summary>Colors the is equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(FullAlpha, 0, 0, DarkBlueBlue);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Argbs the color is equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ArgbColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(FullAlpha, 0, 0, DarkBlueBlue);
        var fixture2 = SplatColor.FromArgb(0, 0, DarkBlueBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Argbs the hexadecimal color is equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ArgbHexColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(0xFF00008B);
        var fixture2 = SplatColor.FromArgb(0, 0, DarkBlueBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Argbs the and named color is equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ArgbAndNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(0, 0, DarkBlueBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Argbs the based on named color is equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ArgbBasedOnNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(FullAlpha, fixture1);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Incorrect named color is equal to empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IncorrectNamedColorIsEqualToEmpty()
    {
        var fixture1 = SplatColor.FromName(UnknownColorName);
        var fixture2 = SplatColor.Empty;

        using (Assert.Multiple())
        {
            await Assert.That(fixture1).IsEqualTo(fixture2);
            await Assert.That(fixture1.Name).IsEqualTo(UnknownColorName);
        }
    }

    /// <summary>Colors the is not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsNotEqual()
    {
        var fixture1 = SplatColor.FromArgb(FullAlpha, 0, 0, NonDarkBlueBlue);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(fixture1).IsNotEqualTo(fixture2);
    }

    /// <summary>Colors the brightness.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorBrightnessHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var brightness = fixture.GetBrightness();

        await Assert.That(brightness).IsEqualTo(DarkBlueBrightness).Within(Eps);
    }

    /// <summary>Colors the saturation has correct value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorSaturationHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var saturation = fixture.GetSaturation();

        await Assert.That(saturation).IsEqualTo(1F).Within(Eps);
    }

    /// <summary>Colors the hue has correct value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorHueHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var hue = fixture.GetHue();

        await Assert.That(hue).IsEqualTo(DarkBlueHue).Within(Eps);
    }

    /// <summary>Colors to known color has correct value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorToKnownColorHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var color = fixture.ToKnownColor();

        await Assert.That(color).IsEqualTo(KnownColor.DarkBlue);
    }

    /// <summary>Colors the is equals.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ColorIsEquals()
    {
        object fixture1 = SplatColor.FromArgb(0xFF00008B);
        object fixture2 = SplatColor.FromArgb(0, 0, DarkBlueBlue);

        await Assert.That(fixture1).IsEqualTo(fixture2);
    }

    /// <summary>Incorrects the named color to string gives value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IncorrectNamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromName(UnknownColorName);
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [TheBestColor]");
    }

    /// <summary>Nameds the color to string gives value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [DarkBlue]");
    }

    /// <summary>Argbs the color to string gives value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ArgbColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromArgb(FullAlpha, 0, 0, NonDarkBlueBlue);
        await Assert.That(fixture1.ToString()).IsEqualTo("SplatColor [A=255, R=0, G=0, B=138]");
    }

    /// <summary>Invalids the Argb color A throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidArgbColorAThrows()
        => await Assert.That(static () => SplatColor.FromArgb(ComponentRange, 0, 0, 0)).Throws<ArgumentException>();

    /// <summary>Invalids the Argb color r throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidArgbColorRThrows()
        => await Assert.That(static () => SplatColor.FromArgb(0, ComponentRange, 0, 0)).Throws<ArgumentException>();

    /// <summary>Invalids the Argb color g throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidArgbColorGThrows()
        => await Assert.That(static () => SplatColor.FromArgb(0, 0, ComponentRange, 0)).Throws<ArgumentException>();

    /// <summary>Invalids the Argb color b throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InvalidArgbColorBThrows()
        => await Assert.That(static () => SplatColor.FromArgb(0, 0, 0, ComponentRange)).Throws<ArgumentException>();

    // Known color static properties tests - each property is tested individually for coverage
    /// <summary>Verifies that the Transparent known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Transparent_IsValid() => await Assert.That(SplatColor.Transparent).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Transparent));

    /// <summary>Verifies that the Alice Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_AliceBlue_IsValid() => await Assert.That(SplatColor.AliceBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.AliceBlue));

    /// <summary>Verifies that the Antique White known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_AntiqueWhite_IsValid() => await Assert.That(SplatColor.AntiqueWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.AntiqueWhite));

    /// <summary>Verifies that the Aqua known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Aqua_IsValid() => await Assert.That(SplatColor.Aqua).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Aqua));

    /// <summary>Verifies that the Aquamarine known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Aquamarine_IsValid() => await Assert.That(SplatColor.Aquamarine).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Aquamarine));

    /// <summary>Verifies that the Azure known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Azure_IsValid() => await Assert.That(SplatColor.Azure).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Azure));

    /// <summary>Verifies that the Beige known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Beige_IsValid() => await Assert.That(SplatColor.Beige).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Beige));

    /// <summary>Verifies that the Bisque known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Bisque_IsValid() => await Assert.That(SplatColor.Bisque).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Bisque));

    /// <summary>Verifies that the Black known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Black_IsValid() => await Assert.That(SplatColor.Black).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Black));

    /// <summary>Verifies that the Blanched Almond known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_BlanchedAlmond_IsValid() => await Assert.That(SplatColor.BlanchedAlmond).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BlanchedAlmond));

    /// <summary>Verifies that the Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Blue_IsValid() => await Assert.That(SplatColor.Blue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Blue));

    /// <summary>Verifies that the Blue Violet known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_BlueViolet_IsValid() => await Assert.That(SplatColor.BlueViolet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BlueViolet));

    /// <summary>Verifies that the Brown known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Brown_IsValid() => await Assert.That(SplatColor.Brown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Brown));

    /// <summary>Verifies that the Burly Wood known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_BurlyWood_IsValid() => await Assert.That(SplatColor.BurlyWood).IsEqualTo(SplatColor.FromKnownColor(KnownColor.BurlyWood));

    /// <summary>Verifies that the Cadet Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_CadetBlue_IsValid() => await Assert.That(SplatColor.CadetBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.CadetBlue));

    /// <summary>Verifies that the Chartreuse known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Chartreuse_IsValid() => await Assert.That(SplatColor.Chartreuse).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Chartreuse));

    /// <summary>Verifies that the Chocolate known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Chocolate_IsValid() => await Assert.That(SplatColor.Chocolate).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Chocolate));

    /// <summary>Verifies that the Coral known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Coral_IsValid() => await Assert.That(SplatColor.Coral).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Coral));

    /// <summary>Verifies that the Cornflower Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_CornflowerBlue_IsValid() => await Assert.That(SplatColor.CornflowerBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.CornflowerBlue));

    /// <summary>Verifies that the Cornsilk known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Cornsilk_IsValid() => await Assert.That(SplatColor.Cornsilk).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Cornsilk));

    /// <summary>Verifies that the Crimson known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Crimson_IsValid() => await Assert.That(SplatColor.Crimson).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Crimson));

    /// <summary>Verifies that the Cyan known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Cyan_IsValid() => await Assert.That(SplatColor.Cyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Cyan));

    /// <summary>Verifies that the Dark Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkBlue_IsValid() => await Assert.That(SplatColor.DarkBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkBlue));

    /// <summary>Verifies that the Dark Cyan known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkCyan_IsValid() => await Assert.That(SplatColor.DarkCyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkCyan));

    /// <summary>Verifies that the Dark Goldenrod known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkGoldenrod_IsValid() => await Assert.That(SplatColor.DarkGoldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGoldenrod));

    /// <summary>Verifies that the Dark Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkGray_IsValid() => await Assert.That(SplatColor.DarkGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGray));

    /// <summary>Verifies that the Dark Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkGreen_IsValid() => await Assert.That(SplatColor.DarkGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkGreen));

    /// <summary>Verifies that the Dark Khaki known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkKhaki_IsValid() => await Assert.That(SplatColor.DarkKhaki).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkKhaki));

    /// <summary>Verifies that the Dark Magenta known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkMagenta_IsValid() => await Assert.That(SplatColor.DarkMagenta).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkMagenta));

    /// <summary>Verifies that the Dark Olive Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkOliveGreen_IsValid() => await Assert.That(SplatColor.DarkOliveGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOliveGreen));

    /// <summary>Verifies that the Dark Orange known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkOrange_IsValid() => await Assert.That(SplatColor.DarkOrange).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOrange));

    /// <summary>Verifies that the Dark Orchid known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkOrchid_IsValid() => await Assert.That(SplatColor.DarkOrchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkOrchid));

    /// <summary>Verifies that the Dark Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkRed_IsValid() => await Assert.That(SplatColor.DarkRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkRed));

    /// <summary>Verifies that the Dark Salmon known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkSalmon_IsValid() => await Assert.That(SplatColor.DarkSalmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSalmon));

    /// <summary>Verifies that the Dark Sea Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkSeaGreen_IsValid() => await Assert.That(SplatColor.DarkSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSeaGreen));

    /// <summary>Verifies that the Dark Slate Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkSlateBlue_IsValid() => await Assert.That(SplatColor.DarkSlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSlateBlue));

    /// <summary>Verifies that the Dark Slate Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkSlateGray_IsValid() => await Assert.That(SplatColor.DarkSlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkSlateGray));

    /// <summary>Verifies that the Dark Turquoise known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkTurquoise_IsValid() => await Assert.That(SplatColor.DarkTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkTurquoise));

    /// <summary>Verifies that the Dark Violet known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DarkViolet_IsValid() => await Assert.That(SplatColor.DarkViolet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DarkViolet));

    /// <summary>Verifies that the Deep Pink known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DeepPink_IsValid() => await Assert.That(SplatColor.DeepPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DeepPink));

    /// <summary>Verifies that the Deep Sky Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DeepSkyBlue_IsValid() => await Assert.That(SplatColor.DeepSkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DeepSkyBlue));

    /// <summary>Verifies that the Dim Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DimGray_IsValid() => await Assert.That(SplatColor.DimGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DimGray));

    /// <summary>Verifies that the Dodger Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_DodgerBlue_IsValid() => await Assert.That(SplatColor.DodgerBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.DodgerBlue));

    /// <summary>Verifies that the Firebrick known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Firebrick_IsValid() => await Assert.That(SplatColor.Firebrick).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Firebrick));

    /// <summary>Verifies that the Floral White known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_FloralWhite_IsValid() => await Assert.That(SplatColor.FloralWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.FloralWhite));

    /// <summary>Verifies that the Forest Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_ForestGreen_IsValid() => await Assert.That(SplatColor.ForestGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.ForestGreen));

    /// <summary>Verifies that the Fuchsia known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Fuchsia_IsValid() => await Assert.That(SplatColor.Fuchsia).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Fuchsia));

    /// <summary>Verifies that the Gainsboro known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Gainsboro_IsValid() => await Assert.That(SplatColor.Gainsboro).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gainsboro));

    /// <summary>Verifies that the Ghost White known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_GhostWhite_IsValid() => await Assert.That(SplatColor.GhostWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.GhostWhite));

    /// <summary>Verifies that the Gold known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Gold_IsValid() => await Assert.That(SplatColor.Gold).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gold));

    /// <summary>Verifies that the Goldenrod known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Goldenrod_IsValid() => await Assert.That(SplatColor.Goldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Goldenrod));

    /// <summary>Verifies that the Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Gray_IsValid() => await Assert.That(SplatColor.Gray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Gray));

    /// <summary>Verifies that the Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Green_IsValid() => await Assert.That(SplatColor.Green).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Green));

    /// <summary>Verifies that the Green Yellow known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_GreenYellow_IsValid() => await Assert.That(SplatColor.GreenYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.GreenYellow));

    /// <summary>Verifies that the Honeydew known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Honeydew_IsValid() => await Assert.That(SplatColor.Honeydew).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Honeydew));

    /// <summary>Verifies that the Hot Pink known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_HotPink_IsValid() => await Assert.That(SplatColor.HotPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.HotPink));

    /// <summary>Verifies that the Indian Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_IndianRed_IsValid() => await Assert.That(SplatColor.IndianRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.IndianRed));

    /// <summary>Verifies that the Indigo known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Indigo_IsValid() => await Assert.That(SplatColor.Indigo).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Indigo));

    /// <summary>Verifies that the Ivory known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Ivory_IsValid() => await Assert.That(SplatColor.Ivory).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Ivory));

    /// <summary>Verifies that the Khaki known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Khaki_IsValid() => await Assert.That(SplatColor.Khaki).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Khaki));

    /// <summary>Verifies that the Lavender known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Lavender_IsValid() => await Assert.That(SplatColor.Lavender).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Lavender));

    /// <summary>Verifies that the Lavender Blush known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LavenderBlush_IsValid() => await Assert.That(SplatColor.LavenderBlush).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LavenderBlush));

    /// <summary>Verifies that the Lawn Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LawnGreen_IsValid() => await Assert.That(SplatColor.LawnGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LawnGreen));

    /// <summary>Verifies that the Lemon Chiffon known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LemonChiffon_IsValid() => await Assert.That(SplatColor.LemonChiffon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LemonChiffon));

    /// <summary>Verifies that the Light Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightBlue_IsValid() => await Assert.That(SplatColor.LightBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightBlue));

    /// <summary>Verifies that the Light Coral known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightCoral_IsValid() => await Assert.That(SplatColor.LightCoral).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightCoral));

    /// <summary>Verifies that the Light Cyan known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightCyan_IsValid() => await Assert.That(SplatColor.LightCyan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightCyan));

    /// <summary>Verifies that the Light Goldenrod Yellow known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightGoldenrodYellow_IsValid() => await Assert.That(SplatColor.LightGoldenrodYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGoldenrodYellow));

    /// <summary>Verifies that the Light Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightGray_IsValid() => await Assert.That(SplatColor.LightGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGray));

    /// <summary>Verifies that the Light Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightGreen_IsValid() => await Assert.That(SplatColor.LightGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightGreen));

    /// <summary>Verifies that the Light Pink known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightPink_IsValid() => await Assert.That(SplatColor.LightPink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightPink));

    /// <summary>Verifies that the Light Salmon known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightSalmon_IsValid() => await Assert.That(SplatColor.LightSalmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSalmon));

    /// <summary>Verifies that the Light Sea Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightSeaGreen_IsValid() => await Assert.That(SplatColor.LightSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSeaGreen));

    /// <summary>Verifies that the Light Sky Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightSkyBlue_IsValid() => await Assert.That(SplatColor.LightSkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSkyBlue));

    /// <summary>Verifies that the Light Slate Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightSlateGray_IsValid() => await Assert.That(SplatColor.LightSlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSlateGray));

    /// <summary>Verifies that the Light Steel Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightSteelBlue_IsValid() => await Assert.That(SplatColor.LightSteelBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightSteelBlue));

    /// <summary>Verifies that the Light Yellow known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LightYellow_IsValid() => await Assert.That(SplatColor.LightYellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LightYellow));

    /// <summary>Verifies that the Lime known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Lime_IsValid() => await Assert.That(SplatColor.Lime).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Lime));

    /// <summary>Verifies that the Lime Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_LimeGreen_IsValid() => await Assert.That(SplatColor.LimeGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.LimeGreen));

    /// <summary>Verifies that the Linen known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Linen_IsValid() => await Assert.That(SplatColor.Linen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Linen));

    /// <summary>Verifies that the Magenta known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Magenta_IsValid() => await Assert.That(SplatColor.Magenta).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Magenta));

    /// <summary>Verifies that the Maroon known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Maroon_IsValid() => await Assert.That(SplatColor.Maroon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Maroon));

    /// <summary>Verifies that the Medium Aquamarine known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumAquamarine_IsValid() => await Assert.That(SplatColor.MediumAquamarine).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumAquamarine));

    /// <summary>Verifies that the Medium Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumBlue_IsValid() => await Assert.That(SplatColor.MediumBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumBlue));

    /// <summary>Verifies that the Medium Orchid known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumOrchid_IsValid() => await Assert.That(SplatColor.MediumOrchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumOrchid));

    /// <summary>Verifies that the Medium Purple known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumPurple_IsValid() => await Assert.That(SplatColor.MediumPurple).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumPurple));

    /// <summary>Verifies that the Medium Sea Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumSeaGreen_IsValid() => await Assert.That(SplatColor.MediumSeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSeaGreen));

    /// <summary>Verifies that the Medium Slate Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumSlateBlue_IsValid() => await Assert.That(SplatColor.MediumSlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSlateBlue));

    /// <summary>Verifies that the Medium Spring Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumSpringGreen_IsValid() => await Assert.That(SplatColor.MediumSpringGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumSpringGreen));

    /// <summary>Verifies that the Medium Turquoise known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumTurquoise_IsValid() => await Assert.That(SplatColor.MediumTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumTurquoise));

    /// <summary>Verifies that the Medium Violet Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MediumVioletRed_IsValid() => await Assert.That(SplatColor.MediumVioletRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MediumVioletRed));

    /// <summary>Verifies that the Midnight Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MidnightBlue_IsValid() => await Assert.That(SplatColor.MidnightBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MidnightBlue));

    /// <summary>Verifies that the Mint Cream known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MintCream_IsValid() => await Assert.That(SplatColor.MintCream).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MintCream));

    /// <summary>Verifies that the Misty Rose known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_MistyRose_IsValid() => await Assert.That(SplatColor.MistyRose).IsEqualTo(SplatColor.FromKnownColor(KnownColor.MistyRose));

    /// <summary>Verifies that the Moccasin known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Moccasin_IsValid() => await Assert.That(SplatColor.Moccasin).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Moccasin));

    /// <summary>Verifies that the Navajo White known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_NavajoWhite_IsValid() => await Assert.That(SplatColor.NavajoWhite).IsEqualTo(SplatColor.FromKnownColor(KnownColor.NavajoWhite));

    /// <summary>Verifies that the Navy known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Navy_IsValid() => await Assert.That(SplatColor.Navy).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Navy));

    /// <summary>Verifies that the Old Lace known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_OldLace_IsValid() => await Assert.That(SplatColor.OldLace).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OldLace));

    /// <summary>Verifies that the Olive known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Olive_IsValid() => await Assert.That(SplatColor.Olive).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Olive));

    /// <summary>Verifies that the Olive Drab known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_OliveDrab_IsValid() => await Assert.That(SplatColor.OliveDrab).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OliveDrab));

    /// <summary>Verifies that the Orange known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Orange_IsValid() => await Assert.That(SplatColor.Orange).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Orange));

    /// <summary>Verifies that the Orange Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_OrangeRed_IsValid() => await Assert.That(SplatColor.OrangeRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.OrangeRed));

    /// <summary>Verifies that the Orchid known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Orchid_IsValid() => await Assert.That(SplatColor.Orchid).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Orchid));

    /// <summary>Verifies that the Pale Goldenrod known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PaleGoldenrod_IsValid() => await Assert.That(SplatColor.PaleGoldenrod).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleGoldenrod));

    /// <summary>Verifies that the Pale Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PaleGreen_IsValid() => await Assert.That(SplatColor.PaleGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleGreen));

    /// <summary>Verifies that the Pale Turquoise known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PaleTurquoise_IsValid() => await Assert.That(SplatColor.PaleTurquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleTurquoise));

    /// <summary>Verifies that the Pale Violet Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PaleVioletRed_IsValid() => await Assert.That(SplatColor.PaleVioletRed).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PaleVioletRed));

    /// <summary>Verifies that the Papaya Whip known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PapayaWhip_IsValid() => await Assert.That(SplatColor.PapayaWhip).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PapayaWhip));

    /// <summary>Verifies that the Peach Puff known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PeachPuff_IsValid() => await Assert.That(SplatColor.PeachPuff).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PeachPuff));

    /// <summary>Verifies that the Peru known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Peru_IsValid() => await Assert.That(SplatColor.Peru).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Peru));

    /// <summary>Verifies that the Pink known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Pink_IsValid() => await Assert.That(SplatColor.Pink).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Pink));

    /// <summary>Verifies that the Plum known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Plum_IsValid() => await Assert.That(SplatColor.Plum).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Plum));

    /// <summary>Verifies that the Powder Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_PowderBlue_IsValid() => await Assert.That(SplatColor.PowderBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.PowderBlue));

    /// <summary>Verifies that the Purple known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Purple_IsValid() => await Assert.That(SplatColor.Purple).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Purple));

    /// <summary>Verifies that the Red known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Red_IsValid() => await Assert.That(SplatColor.Red).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Red));

    /// <summary>Verifies that the Rosy Brown known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_RosyBrown_IsValid() => await Assert.That(SplatColor.RosyBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.RosyBrown));

    /// <summary>Verifies that the Royal Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_RoyalBlue_IsValid() => await Assert.That(SplatColor.RoyalBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.RoyalBlue));

    /// <summary>Verifies that the Saddle Brown known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SaddleBrown_IsValid() => await Assert.That(SplatColor.SaddleBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SaddleBrown));

    /// <summary>Verifies that the Salmon known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Salmon_IsValid() => await Assert.That(SplatColor.Salmon).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Salmon));

    /// <summary>Verifies that the Sandy Brown known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SandyBrown_IsValid() => await Assert.That(SplatColor.SandyBrown).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SandyBrown));

    /// <summary>Verifies that the Sea Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SeaGreen_IsValid() => await Assert.That(SplatColor.SeaGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SeaGreen));

    /// <summary>Verifies that the Sea Shell known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SeaShell_IsValid() => await Assert.That(SplatColor.SeaShell).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SeaShell));

    /// <summary>Verifies that the Sienna known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Sienna_IsValid() => await Assert.That(SplatColor.Sienna).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Sienna));

    /// <summary>Verifies that the Silver known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Silver_IsValid() => await Assert.That(SplatColor.Silver).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Silver));

    /// <summary>Verifies that the Sky Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SkyBlue_IsValid() => await Assert.That(SplatColor.SkyBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SkyBlue));

    /// <summary>Verifies that the Slate Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SlateBlue_IsValid() => await Assert.That(SplatColor.SlateBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SlateBlue));

    /// <summary>Verifies that the Slate Gray known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SlateGray_IsValid() => await Assert.That(SplatColor.SlateGray).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SlateGray));

    /// <summary>Verifies that the Snow known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Snow_IsValid() => await Assert.That(SplatColor.Snow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Snow));

    /// <summary>Verifies that the Spring Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SpringGreen_IsValid() => await Assert.That(SplatColor.SpringGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SpringGreen));

    /// <summary>Verifies that the Steel Blue known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_SteelBlue_IsValid() => await Assert.That(SplatColor.SteelBlue).IsEqualTo(SplatColor.FromKnownColor(KnownColor.SteelBlue));

    /// <summary>Verifies that the Tan known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Tan_IsValid() => await Assert.That(SplatColor.Tan).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Tan));

    /// <summary>Verifies that the Teal known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Teal_IsValid() => await Assert.That(SplatColor.Teal).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Teal));

    /// <summary>Verifies that the Thistle known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Thistle_IsValid() => await Assert.That(SplatColor.Thistle).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Thistle));

    /// <summary>Verifies that the Tomato known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Tomato_IsValid() => await Assert.That(SplatColor.Tomato).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Tomato));

    /// <summary>Verifies that the Turquoise known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Turquoise_IsValid() => await Assert.That(SplatColor.Turquoise).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Turquoise));

    /// <summary>Verifies that the Violet known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Violet_IsValid() => await Assert.That(SplatColor.Violet).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Violet));

    /// <summary>Verifies that the Wheat known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Wheat_IsValid() => await Assert.That(SplatColor.Wheat).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Wheat));

    /// <summary>Verifies that the White known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_White_IsValid() => await Assert.That(SplatColor.White).IsEqualTo(SplatColor.FromKnownColor(KnownColor.White));

    /// <summary>Verifies that the White Smoke known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_WhiteSmoke_IsValid() => await Assert.That(SplatColor.WhiteSmoke).IsEqualTo(SplatColor.FromKnownColor(KnownColor.WhiteSmoke));

    /// <summary>Verifies that the Yellow known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_Yellow_IsValid() => await Assert.That(SplatColor.Yellow).IsEqualTo(SplatColor.FromKnownColor(KnownColor.Yellow));

    /// <summary>Verifies that the Yellow Green known color is valid.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task KnownColor_YellowGreen_IsValid() => await Assert.That(SplatColor.YellowGreen).IsEqualTo(SplatColor.FromKnownColor(KnownColor.YellowGreen));
}
