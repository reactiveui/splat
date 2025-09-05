namespace Splat.Drawing.Tests.Colors;

/// <summary>
/// Coverage Color Tests.
/// </summary>
[TestFixture]
public class CoverageColorTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CoverageColorTests"/> class.
    /// </summary>
    public CoverageColorTests() => Locator.CurrentMutable.InitializeSplat();

    /// <summary>
    /// Colors the is empty.
    /// </summary>
    [Test]
    public void ColorIsEmpty()
    {
        var fixture = SplatColor.Empty;
        Assert.That(fixture.IsEmpty, Is.True);
        Assert.That(fixture.A, Is.EqualTo((byte)0));
        Assert.That(fixture.R, Is.EqualTo((byte)0));
        Assert.That(fixture.G, Is.EqualTo((byte)0));
        Assert.That(fixture.B, Is.EqualTo((byte)0));
    }

    /// <summary>
    /// Colors the is equal.
    /// </summary>
    [Test]
    public void ColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.That(fixture1 == fixture2, Is.True);
    }

    /// <summary>
    /// ARGBs the color is equal.
    /// </summary>
    [Test]
    public void ARGBColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.That(fixture1 == fixture2, Is.True);
    }

    /// <summary>
    /// ARGBs the hexadecimal color is equal.
    /// </summary>
    [Test]
    public void ARGBHexColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(0xFF00008B);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.That(fixture1 == fixture2, Is.True);
    }

    /// <summary>
    /// ARGBs the and named color is equal.
    /// </summary>
    [Test]
    public void ARGBAndNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.That(fixture1 == fixture2, Is.True);
    }

    /// <summary>
    /// ARGBs the based on named color is equal.
    /// </summary>
    [Test]
    public void ARGBBasedOnNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(255, fixture1);
        Assert.That(fixture1 == fixture2, Is.True);
    }

    /// <summary>
    /// Incorrect named color is equal to empty.
    /// </summary>
    [Test]
    public void IncorrectNamedColorIsEqualToEmpty()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        var fixture2 = SplatColor.Empty;
        Assert.That(fixture1 == fixture2, Is.True);
        Assert.That(fixture1.Name, Is.EqualTo("TheBestColor"));
    }

    /// <summary>
    /// Colors the is not equal.
    /// </summary>
    [Test]
    public void ColorIsNotEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.That(fixture1 != fixture2, Is.True);
    }

    /// <summary>
    /// Colors the brightness.
    /// </summary>
    [Test]
    public void ColorBrightnessHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var brightness = fixture.GetBrightness();
        Assert.That(brightness, Is.EqualTo(0.272549033f));
    }

    /// <summary>
    /// Colors the saturation has correct value.
    /// </summary>
    [Test]
    public void ColorSaturationHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var saturation = fixture.GetSaturation();
        Assert.That(saturation, Is.EqualTo(1));
    }

    /// <summary>
    /// Colors the hue has correct value.
    /// </summary>
    [Test]
    public void ColorHueHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var hue = fixture.GetHue();
        Assert.That(hue, Is.EqualTo(240f));
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
        Assert.That(fixture1.Equals(fixture2, Is.True));
    }

    /// <summary>
    /// Incorrects the named color to string gives value.
    /// </summary>
    [Test]
    public void IncorrectNamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        Assert.That(fixture1.ToString(, Is.EqualTo("SplatColor [TheBestColor]")));
    }

    /// <summary>
    /// Nameds the color to string gives value.
    /// </summary>
    [Test]
    public void NamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.That(fixture1.ToString(, Is.EqualTo("SplatColor [DarkBlue]")));
    }

    /// <summary>
    /// ARGBs the color to string gives value.
    /// </summary>
    [Test]
    public void ARGBColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        Assert.That(R=0, G=0, B=138]", fixture1.ToString(, Is.EqualTo("SplatColor [A=255)));
    }

    /// <summary>
    /// Invalids the ARGB color A throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorAThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(256, 0, 0, 0));

    /// <summary>
    /// Invalids the ARGB color r throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorRThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 256, 0, 0));

    /// <summary>
    /// Invalids the ARGB color g throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorGThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 256, 0));

    /// <summary>
    /// Invalids the ARGB color b throws.
    /// </summary>
    [Test]
    public void InvalidARGBColorBThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 0, 256));
}
