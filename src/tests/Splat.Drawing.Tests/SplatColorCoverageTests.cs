// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Additional coverage tests for <see cref="SplatColor"/> members and classification flags.</summary>
public sealed class SplatColorCoverageTests
{
    /// <summary>The alpha component used by the component-extraction test.</summary>
    private const byte AlphaComponent = 0x12;

    /// <summary>The red component used by the component-extraction test.</summary>
    private const byte RedComponent = 0x34;

    /// <summary>The green component used by the component-extraction test.</summary>
    private const byte GreenComponent = 0x56;

    /// <summary>The blue component used by the component-extraction test.</summary>
    private const byte BlueComponent = 0x78;

    /// <summary>Fully opaque alpha component.</summary>
    private const int FullAlpha = 255;

    /// <summary>The blue component of <see cref="KnownColor.DarkBlue"/>.</summary>
    private const int DarkBlueBlue = 139;

    /// <summary>The alpha component of an arbitrary color that has no known match.</summary>
    private const int SampleAlpha = 10;

    /// <summary>The red component of an arbitrary color that has no known match.</summary>
    private const int SampleRed = 20;

    /// <summary>The green component of an arbitrary color that has no known match.</summary>
    private const int SampleGreen = 30;

    /// <summary>The blue component of an arbitrary color that has no known match.</summary>
    private const int SampleBlue = 40;

    /// <summary>The blue component of the first color in the inequality comparison.</summary>
    private const int BaseBlue = 1;

    /// <summary>The blue component of the second, differing color in the inequality comparison.</summary>
    private const int DifferentBlue = 2;

    /// <summary>The red component of a light color whose channel sum exceeds a single channel maximum.</summary>
    private const int LightRed = 200;

    /// <summary>The green component of a light color whose channel sum exceeds a single channel maximum.</summary>
    private const int LightGreen = 220;

    /// <summary>The blue component of a light color whose channel sum exceeds a single channel maximum.</summary>
    private const int LightBlue = 240;

    /// <summary>The saturation expected from the light color, taken through the high-sum branch.</summary>
    private const float LightSaturation = 40F / 70F;

    /// <summary>The red component of a red-dominant color whose hue wraps past a full circle.</summary>
    private const int RedDominantRed = 200;

    /// <summary>The green component of a red-dominant color whose hue wraps past a full circle.</summary>
    private const int RedDominantGreen = 199;

    /// <summary>The blue component of a red-dominant color whose hue wraps past a full circle.</summary>
    private const int RedDominantBlue = 0;

    /// <summary>The hue, in degrees, expected from the red-dominant color after wrapping past a full circle.</summary>
    private const float RedDominantHue = 59.7F;

    /// <summary>The red component of a green-dominant color.</summary>
    private const int GreenDominantRed = 100;

    /// <summary>The green component of a green-dominant color.</summary>
    private const int GreenDominantGreen = 200;

    /// <summary>The blue component of a green-dominant color.</summary>
    private const int GreenDominantBlue = 50;

    /// <summary>The hue, in degrees, expected from the green-dominant color.</summary>
    private const float GreenDominantHue = 100F;

    /// <summary>Tolerance used when comparing floating-point color components.</summary>
    private const float ColorEpsilon = 1e-6F;

    /// <summary>Tolerance used when comparing floating-point hue values.</summary>
    private const float HueTolerance = 0.1F;

    /// <summary>An alpha component differing from <see cref="SampleAlpha"/>.</summary>
    private const int DifferentAlpha = SampleAlpha + 1;

    /// <summary>A red component differing from <see cref="SampleRed"/>.</summary>
    private const int DifferentRed = SampleRed + 1;

    /// <summary>A green component differing from <see cref="SampleGreen"/>.</summary>
    private const int DifferentGreen = SampleGreen + 1;

    /// <summary>Verifies that a color created from a known color reports as known, named, and not empty.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromKnownColor_ReportsKnownAndNamed()
    {
        var color = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        using (Assert.Multiple())
        {
            await Assert.That(color.IsKnownColor).IsTrue();
            await Assert.That(color.IsNamedColor).IsTrue();
            await Assert.That(color.IsEmpty).IsFalse();
        }
    }

    /// <summary>Verifies that an out-of-range known color falls back to an empty named color.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromKnownColor_OutOfRange_IsNamedEmpty()
    {
        var color = SplatColor.FromKnownColor((KnownColor)(-1));

        using (Assert.Multiple())
        {
            await Assert.That(color.IsNamedColor).IsTrue();
            await Assert.That(color.IsKnownColor).IsFalse();
        }
    }

    /// <summary>Verifies that a plain ARGB color that has no known match is neither known nor named.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromArgb_NonKnownValue_IsNotKnownOrNamed()
    {
        var color = SplatColor.FromArgb(SampleAlpha, SampleRed, SampleGreen, SampleBlue);

        using (Assert.Multiple())
        {
            await Assert.That(color.IsKnownColor).IsFalse();
            await Assert.That(color.IsNamedColor).IsFalse();
            await Assert.That(color.IsSystemColor).IsFalse();
        }
    }

    /// <summary>Verifies that the name of an unnamed ARGB color is its hexadecimal ARGB value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Name_ForUnnamedColor_IsHexValue()
    {
        var color = SplatColor.FromArgb(SampleAlpha, SampleRed, SampleGreen, SampleBlue);

        await Assert.That(color.Name).IsEqualTo($"{color.ToArgb():x}");
    }

    /// <summary>Verifies that the A, R, G, and B components are extracted correctly from a packed value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Components_AreExtractedCorrectly()
    {
        var color = SplatColor.FromArgb(AlphaComponent, RedComponent, GreenComponent, BlueComponent);

        using (Assert.Multiple())
        {
            await Assert.That(color.A).IsEqualTo(AlphaComponent);
            await Assert.That(color.R).IsEqualTo(RedComponent);
            await Assert.That(color.G).IsEqualTo(GreenComponent);
            await Assert.That(color.B).IsEqualTo(BlueComponent);
        }
    }

    /// <summary>Verifies that <see cref="SplatColor.ToArgb"/> round-trips through <see cref="SplatColor.FromArgb(uint)"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromArgbUint_RoundTrips()
    {
        const uint packed = 0x12345678;
        var color = SplatColor.FromArgb(packed);

        await Assert.That(color.ToArgb()).IsEqualTo(packed);
    }

    /// <summary>Verifies that equal colors produce equal hash codes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetHashCode_IsEqualForEqualColors()
    {
        var first = SplatColor.FromArgb(FullAlpha, 0, 0, DarkBlueBlue);
        var second = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(first.GetHashCode()).IsEqualTo(second.GetHashCode());
    }

    /// <summary>Verifies that comparing a color to a non-color object returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Equals_WithNonColorObject_IsFalse()
    {
        var color = SplatColor.FromArgb(AlphaComponent, RedComponent, GreenComponent, BlueComponent);

        await Assert.That(color.Equals("not a color")).IsFalse();
    }

    /// <summary>Verifies that the equality operator reports equal colors as equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task EqualityOperator_ForEqualColors_IsTrue()
    {
        var first = SplatColor.FromArgb(0, 0, DarkBlueBlue);
        var second = SplatColor.FromArgb(FullAlpha, 0, 0, DarkBlueBlue);

        await Assert.That(first == second).IsTrue();
    }

    /// <summary>Verifies that the inequality operator reports differing colors as not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InequalityOperator_ForDifferingColors_IsTrue()
    {
        var first = SplatColor.FromArgb(0, 0, BaseBlue);
        var second = SplatColor.FromArgb(0, 0, DifferentBlue);

        await Assert.That(first != second).IsTrue();
    }

    /// <summary>Verifies that <see cref="SplatColor.Empty"/> stringifies to its empty form.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToString_ForEmpty_GivesEmptyForm() => await Assert.That(SplatColor.Empty.ToString()).IsEqualTo("SplatColor [Empty]");

    /// <summary>Verifies that a system known color reports as a system color.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromKnownColor_SystemColor_IsSystemColor()
    {
        var color = SplatColor.FromKnownColor(KnownColor.Control);

        await Assert.That(color.IsSystemColor).IsTrue();
    }

    /// <summary>Verifies that a light color whose channel sum exceeds a single channel maximum uses the high-sum saturation branch.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetSaturation_ForLightColor_UsesHighSumBranch()
    {
        var color = SplatColor.FromArgb(FullAlpha, LightRed, LightGreen, LightBlue);

        await Assert.That(color.GetSaturation()).IsEqualTo(LightSaturation).Within(ColorEpsilon);
    }

    /// <summary>Verifies that a red-dominant color produces a hue that wraps back past a full circle.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetHue_ForRedDominantColor_WrapsPastFullCircle()
    {
        var color = SplatColor.FromArgb(FullAlpha, RedDominantRed, RedDominantGreen, RedDominantBlue);

        await Assert.That(color.GetHue()).IsEqualTo(RedDominantHue).Within(HueTolerance);
    }

    /// <summary>Verifies that a green-dominant color produces a hue in the green sextant.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetHue_ForGreenDominantColor_UsesGreenSextant()
    {
        var color = SplatColor.FromArgb(FullAlpha, GreenDominantRed, GreenDominantGreen, GreenDominantBlue);

        await Assert.That(color.GetHue()).IsEqualTo(GreenDominantHue).Within(HueTolerance);
    }

    /// <summary>Verifies that colors differing only in their alpha component are not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Equals_WhenAlphaDiffers_IsFalse()
    {
        var first = SplatColor.FromArgb(SampleAlpha, SampleRed, SampleGreen, SampleBlue);
        var second = SplatColor.FromArgb(DifferentAlpha, SampleRed, SampleGreen, SampleBlue);

        await Assert.That(first.Equals(second)).IsFalse();
    }

    /// <summary>Verifies that colors differing only in their red component are not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Equals_WhenRedDiffers_IsFalse()
    {
        var first = SplatColor.FromArgb(SampleAlpha, SampleRed, SampleGreen, SampleBlue);
        var second = SplatColor.FromArgb(SampleAlpha, DifferentRed, SampleGreen, SampleBlue);

        await Assert.That(first.Equals(second)).IsFalse();
    }

    /// <summary>Verifies that colors differing only in their green component are not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Equals_WhenGreenDiffers_IsFalse()
    {
        var first = SplatColor.FromArgb(SampleAlpha, SampleRed, SampleGreen, SampleBlue);
        var second = SplatColor.FromArgb(SampleAlpha, SampleRed, DifferentGreen, SampleBlue);

        await Assert.That(first.Equals(second)).IsFalse();
    }
}
