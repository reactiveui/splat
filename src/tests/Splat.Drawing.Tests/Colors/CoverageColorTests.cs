// Copyright (c) 2025 ReactiveUI. All rights reserved.
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
}
