// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests.Colors;

/// <summary>
/// Coverage Color Tests.
/// </summary>
[TestFixture]
[NonParallelizable] // Uses Locator (global static); keep this fixture serialized.
public class CoverageColorTests
{
    private const float Eps = 1e-6f;

    /// <summary>
    /// Initialize Splat before each test (matches xUnit ctor-per-test semantics).
    /// </summary>
    [SetUp]
    public void SetUp() => Locator.CurrentMutable.InitializeSplat();

    /// <summary>
    /// Colors the is empty.
    /// </summary>
    [Test]
    public void ColorIsEmpty()
    {
        var fixture = SplatColor.Empty;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fixture.IsEmpty, Is.True);
            Assert.That(fixture.A, Is.Zero);
            Assert.That(fixture.R, Is.Zero);
            Assert.That(fixture.G, Is.Zero);
            Assert.That(fixture.B, Is.Zero);
        }
    }

    /// <summary>
    /// Colors the is equal.
    /// </summary>
    [Test]
    public void ColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// ARGBs the color is equal.
    /// </summary>
    [Test]
    public void ARGBColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// ARGBs the hexadecimal color is equal.
    /// </summary>
    [Test]
    public void ARGBHexColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(0xFF00008B);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// ARGBs the and named color is equal.
    /// </summary>
    [Test]
    public void ARGBAndNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(0, 0, 139);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// ARGBs the based on named color is equal.
    /// </summary>
    [Test]
    public void ARGBBasedOnNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(255, fixture1);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// Incorrect named color is equal to empty.
    /// </summary>
    [Test]
    public void IncorrectNamedColorIsEqualToEmpty()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        var fixture2 = SplatColor.Empty;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fixture1, Is.EqualTo(fixture2));
            Assert.That(fixture1.Name, Is.EqualTo("TheBestColor"));
        }
    }

    /// <summary>
    /// Colors the is not equal.
    /// </summary>
    [Test]
    public void ColorIsNotEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        Assert.That(fixture1, Is.Not.EqualTo(fixture2));
    }

    /// <summary>
    /// Colors the brightness.
    /// </summary>
    [Test]
    public void ColorBrightnessHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var brightness = fixture.GetBrightness();

        Assert.That(brightness, Is.EqualTo(0.272549033f).Within(Eps));
    }

    /// <summary>
    /// Colors the saturation has correct value.
    /// </summary>
    [Test]
    public void ColorSaturationHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var saturation = fixture.GetSaturation();

        Assert.That(saturation, Is.EqualTo(1f).Within(Eps));
    }

    /// <summary>
    /// Colors the hue has correct value.
    /// </summary>
    [Test]
    public void ColorHueHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var hue = fixture.GetHue();

        Assert.That(hue, Is.EqualTo(240f).Within(Eps));
    }

    /// <summary>
    /// Colors to known color has correct value.
    /// </summary>
    [Test]
    public void ColorToKnownColorHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var color = fixture.ToKnownColor();

        Assert.That(color, Is.EqualTo(KnownColor.DarkBlue));
    }

    /// <summary>
    /// Colors the is equals.
    /// </summary>
    [Test]
    public void ColorIsEquals()
    {
        object fixture1 = SplatColor.FromArgb(0xFF00008B);
        object fixture2 = SplatColor.FromArgb(0, 0, 139);

        Assert.That(fixture1, Is.EqualTo(fixture2));
    }

    /// <summary>
    /// Incorrects the named color to string gives value.
    /// </summary>
    [Test]
    public void IncorrectNamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        Assert.That(fixture1.ToString(), Is.EqualTo("SplatColor [TheBestColor]"));
    }

    /// <summary>
    /// Nameds the color to string gives value.
    /// </summary>
    [Test]
    public void NamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.That(fixture1.ToString(), Is.EqualTo("SplatColor [DarkBlue]"));
    }

    /// <summary>
    /// ARGBs the color to string gives value.
    /// </summary>
    [Test]
    public void ARGBColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        Assert.That(fixture1.ToString(), Is.EqualTo("SplatColor [A=255, R=0, G=0, B=138]"));
    }

    /// <summary>
    /// Invalids the ARGB color A throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorAThrows()
        => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(256, 0, 0, 0));

    /// <summary>
    /// Invalids the ARGB color r throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorRThrows()
        => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 256, 0, 0));

    /// <summary>
    /// Invalids the ARGB color g throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorGThrows()
        => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 256, 0));

    /// <summary>
    /// Invalids the ARGB color b throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorBThrows()
        => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 0, 256));
}
