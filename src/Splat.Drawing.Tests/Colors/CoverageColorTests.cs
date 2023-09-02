namespace Splat.Drawing.Tests.Colors;

/// <summary>
/// Coverage Color Tests.
/// </summary>
public class CoverageColorTests
{
    /// <summary>
    /// Colors the is empty.
    /// </summary>
    [Fact]
    public void ColorIsEmpty()
    {
        var fixture = SplatColor.Empty;
        Assert.True(fixture.IsEmpty);
        Assert.Equal((byte)0, fixture.A);
        Assert.Equal((byte)0, fixture.R);
        Assert.Equal((byte)0, fixture.G);
        Assert.Equal((byte)0, fixture.B);
    }

    /// <summary>
    /// Colors the is equal.
    /// </summary>
    [Fact]
    public void ColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.True(fixture1 == fixture2);
    }

    /// <summary>
    /// ARGBs the color is equal.
    /// </summary>
    [Fact]
    public void ARGBColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 139);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.True(fixture1 == fixture2);
    }

    /// <summary>
    /// ARGBs the hexadecimal color is equal.
    /// </summary>
    [Fact]
    public void ARGBHexColorIsEqual()
    {
        var fixture1 = SplatColor.FromArgb(0xFF00008B);
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.True(fixture1 == fixture2);
    }

    /// <summary>
    /// ARGBs the and named color is equal.
    /// </summary>
    [Fact]
    public void ARGBAndNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.True(fixture1 == fixture2);
    }

    /// <summary>
    /// ARGBs the based on named color is equal.
    /// </summary>
    [Fact]
    public void ARGBBasedOnNamedColorIsEqual()
    {
        var fixture1 = SplatColor.FromName("DarkBlue");
        var fixture2 = SplatColor.FromArgb(255, fixture1);
        Assert.True(fixture1 == fixture2);
    }

    /// <summary>
    /// Incorrect named color is equal to empty.
    /// </summary>
    [Fact]
    public void IncorrectNamedColorIsEqualToEmpty()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        var fixture2 = SplatColor.Empty;
        Assert.True(fixture1 == fixture2);
        Assert.Equal("TheBestColor", fixture1.Name);
    }

    /// <summary>
    /// Colors the is not equal.
    /// </summary>
    [Fact]
    public void ColorIsNotEqual()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        var fixture2 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.True(fixture1 != fixture2);
    }

    /// <summary>
    /// Colors the brightness.
    /// </summary>
    [Fact]
    public void ColorBrightnessHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var brightness = fixture.GetBrightness();
        Assert.Equal(0.272549033f, brightness);
    }

    /// <summary>
    /// Colors the saturation has correct value.
    /// </summary>
    [Fact]
    public void ColorSaturationHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var saturation = fixture.GetSaturation();
        Assert.Equal(1, saturation);
    }

    /// <summary>
    /// Colors the hue has correct value.
    /// </summary>
    [Fact]
    public void ColorHueHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var hue = fixture.GetHue();
        Assert.Equal(240f, hue);
    }

    /// <summary>
    /// Colors to known color has correct value.
    /// </summary>
    [Fact]
    public void ColorToKnownColorHasCorrectValue()
    {
        var fixture = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        var color = fixture.ToKnownColor();
        Assert.Equal(KnownColor.DarkBlue, color);
    }

    /// <summary>
    /// Colors the is equals.
    /// </summary>
    [Fact]
    public void ColorIsEquals()
    {
        object fixture1 = SplatColor.FromArgb(0xFF00008B);
        object fixture2 = SplatColor.FromArgb(0, 0, 139);
        Assert.True(fixture1.Equals(fixture2));
    }

    /// <summary>
    /// Incorrects the named color to string gives value.
    /// </summary>
    [Fact]
    public void IncorrectNamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromName("TheBestColor");
        Assert.Equal("SplatColor [TheBestColor]", fixture1.ToString());
    }

    /// <summary>
    /// Nameds the color to string gives value.
    /// </summary>
    [Fact]
    public void NamedColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromKnownColor(KnownColor.DarkBlue);
        Assert.Equal("SplatColor [DarkBlue]", fixture1.ToString());
    }

    /// <summary>
    /// ARGBs the color to string gives value.
    /// </summary>
    [Fact]
    public void ARGBColorToStringGivesValue()
    {
        var fixture1 = SplatColor.FromArgb(255, 0, 0, 138);
        Assert.Equal("SplatColor [A=255, R=0, G=0, B=138]", fixture1.ToString());
    }

    /// <summary>
    /// Invalids the ARGB color A throws.
    /// </summary>
    [Fact]
    public void InvalidARGBColorAThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(256, 0, 0, 0));

    /// <summary>
    /// Invalids the ARGB color r throws.
    /// </summary>
    [Fact]
    public void InvalidARGBColorRThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 256, 0, 0));

    /// <summary>
    /// Invalids the ARGB color g throws.
    /// </summary>
    [Fact]
    public void InvalidARGBColorGThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 256, 0));

    /// <summary>
    /// Invalids the ARGB color b throws.
    /// </summary>
    [Fact]
    public void InvalidARGBColorBThrows() => Assert.Throws<ArgumentException>(() => SplatColor.FromArgb(0, 0, 0, 256));
}
