// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Splat;

/// <summary>
/// Represents an ARGB color value with support for known, named, and system colors, as well as utility methods for
/// color manipulation and comparison.
/// </summary>
/// <remarks>The SplatColor struct provides methods to create colors from ARGB values, known color names, or
/// predefined color constants. It supports comparison, conversion to and from known colors, and exposes properties for
/// individual color components. SplatColor is immutable and suitable for use in graphics, UI, and serialization
/// scenarios. Thread safety is guaranteed for read operations, as the struct is immutable after creation.</remarks>
[DataContract]
public partial struct SplatColor : IEquatable<SplatColor>
{
    /// <summary>The maximum value of a single 8-bit color channel (0-255).</summary>
    private const int MaxChannelValue = 255;

    /// <summary>The bit mask that isolates a single 8-bit color channel.</summary>
    private const uint ChannelMask = 0xFF;

    /// <summary>The mask covering all 32 bits of a packed ARGB value.</summary>
    private const uint ArgbMask = 0xFFFFFFFF;

    /// <summary>The bit offset of the alpha channel within a packed ARGB value.</summary>
    private const int AlphaShift = 24;

    /// <summary>The bit offset of the red channel within a packed ARGB value.</summary>
    private const int RedShift = 16;

    /// <summary>The bit offset of the green channel within a packed ARGB value.</summary>
    private const int GreenShift = 8;

    /// <summary>The sum of the maximum values of two color channels (255 + 255).</summary>
    private const int TwoChannelMax = MaxChannelValue + MaxChannelValue;

    /// <summary>The lowest <see cref="KnownColor"/> index that is not a system color.</summary>
    private const int FirstNonSystemKnownColor = 27;

    /// <summary>The highest <see cref="KnownColor"/> index that is not a system color.</summary>
    private const int LastNonSystemKnownColor = 169;

    /// <summary>The number of degrees in one sextant of the HSL colour wheel.</summary>
    private const float DegreesPerSextant = 60.0f;

    /// <summary>The total number of degrees in the HSL colour wheel.</summary>
    private const float DegreesPerCircle = 360.0f;

    /// <summary>The hue sextant offset applied when red is the dominant channel.</summary>
    private const float RedHueOffset = 6.0f;

    /// <summary>The hue sextant offset applied when green is the dominant channel.</summary>
    private const float GreenHueOffset = 2.0f;

    /// <summary>The hue sextant offset applied when blue is the dominant channel.</summary>
    private const float BlueHueOffset = 4.0f;

    /// <summary>Packed state flags describing how this color was created (see <see cref="ColorTypes"/>).</summary>
    private short _state;

    /// <summary>Identifier of the known color this value was created from, when applicable.</summary>
    private short _knownColor;

    /// <summary>The name of the color, when it has one.</summary>
    /// <remarks>
    /// Retained for serialization compatibility: MS 1.1 required this member to be present for serialization
    /// (no longer needed in 2.0). Keeping a string reference inside a struct is not ideal, but Mono bug #324144
    /// held back changing it.
    /// </remarks>
    private string _name;

    /// <summary>Initializes a new instance of the <see cref="SplatColor"/> struct with the specified color value, state, known color identifier, and name.</summary>
    /// <param name="value">The packed ARGB color value represented as an unsigned integer.</param>
    /// <param name="state">A value indicating the state or flags associated with the color.</param>
    /// <param name="knownColor">An identifier for a known color, or a value indicating that the color is not a known color.</param>
    /// <param name="name">The name of the color. Can be null or empty if the color does not have a name.</param>
    internal SplatColor(uint value, short state, short knownColor, string name)
    {
        Value = value;
        _state = state;
        _knownColor = knownColor;
        _name = name;
    }

    /// <summary>Specifies the type or characteristics of a color value.</summary>
    /// <remarks><para>
    /// This enumeration supports bitwise combination of its values to represent multiple color
    /// characteristics. It is typically used to indicate how a color is defined or categorized, such as whether it is a
    /// known color, an ARGB value, a named color, or a system color.
    /// </para>
    /// <para>
    /// The specs also indicate that all three of these properties are true
    /// if created with FromKnownColor or FromNamedColor, false otherwise (FromARGB).
    /// Per Microsoft and ECMA specs these variables are set by which constructor is used, not by their values.
    /// </para>
    /// </remarks>
    [Flags]
    internal enum ColorTypes
    {
        /// <summary>The color is empty (uninitialized).</summary>
        Empty = 0,

        /// <summary>The color was created from a known color.</summary>
        Known = 1,

        /// <summary>The color was created from an explicit ARGB value.</summary>
        ARGB = 2,

        /// <summary>The color has a name.</summary>
        Named = 4,

        /// <summary>The color is a system color.</summary>
        System = 8,
    }

    /// <summary>Gets an instance of SplatColor that represents an uninitialized or empty color value (transparent).</summary>
    public static SplatColor Empty { get; }

    /// <summary>Gets a value indicating whether the current color is transparent black. Eg where R,G,B,A == 0.</summary>
    public readonly bool IsEmpty => _state == (short)ColorTypes.Empty;

    /// <summary>Gets the alpha component of the color.</summary>
    public byte A => (byte)(Value >> AlphaShift);

    /// <summary>Gets the red component of the color.</summary>
    public byte R => (byte)(Value >> RedShift);

    /// <summary>Gets the green component of the color.</summary>
    public byte G => (byte)(Value >> GreenShift);

    /// <summary>Gets the blue component of the color.</summary>
    public byte B => (byte)Value;

    /// <summary>Gets the name of the color if one is known. Otherwise will be the hex value.</summary>
    public string Name
    {
        get
        {
            // name is required for serialization under 1.x, but not under 2.0
            // Can happen with stuff deserialized from MS
            _name ??= IsNamedColor ? KnownColors.GetName(_knownColor) : $"{ToArgb():x}";

            return _name;
        }
    }

    /// <summary>Gets a value indicating whether the color is part of the <see cref="ColorTypes.Known"/> group.</summary>
    public readonly bool IsKnownColor => (_state & (short)ColorTypes.Known) != 0;

    /// <summary>Gets a value indicating whether the color is part of the <see cref="ColorTypes.System"/> group.</summary>
    public readonly bool IsSystemColor => (_state & (short)ColorTypes.System) != 0;

    /// <summary>Gets a value indicating whether the color is par tof the <see cref="ColorTypes.Known"/> or <see cref="ColorTypes.Named"/> groups.</summary>
    public readonly bool IsNamedColor => (_state & (short)(ColorTypes.Known | ColorTypes.Named)) != 0;

    /// <summary>Gets or sets the value of the color.</summary>
    [DataMember]
    internal uint Value
    {
        get
        {
            // Optimization for known colors that were deserialized
            // from an MS serialized stream.
            if (field == 0 && IsKnownColor)
            {
                field = KnownColors.FromKnownColor((KnownColor)_knownColor).ToArgb() & ArgbMask;
            }

            return field;
        }

        set;
    }

    /// <summary>Compares two SplatColor references and determines if they are equivalent based on their A,R,G,B values.</summary>
    /// <param name="left">The first SplatColor to compare.</param>
    /// <param name="right">The second SplatColor to compare.</param>
    /// <returns>If they are equivalent to each other.</returns>
    public static bool operator ==(SplatColor left, SplatColor right) =>
        left.Equals(right);

    /// <summary>Compares two SplatColor references and determines if they are not equivalent based on their A,R,G,B values.</summary>
    /// <param name="left">The first SplatColor to compare.</param>
    /// <param name="right">The second SplatColor to compare.</param>
    /// <returns>If they are not equivalent to each other.</returns>
    public static bool operator !=(SplatColor left, SplatColor right) =>
        !left.Equals(right);

    /// <summary>Creates a SplatColor from the RGB values. The alpha will be set to 255 for full alpha.</summary>
    /// <param name="red">The red channel of the color.</param>
    /// <param name="green">The green channel of the color.</param>
    /// <param name="blue">The blue channel of the color.</param>
    /// <returns>A splat color from the specified channels.</returns>
    public static SplatColor FromArgb(int red, int green, int blue) => FromArgb(MaxChannelValue, red, green, blue);

    /// <summary>Creates a SplatColor from the RGB values.</summary>
    /// <param name="alpha">The alpha channel of the color.</param>
    /// <param name="red">The red channel of the color.</param>
    /// <param name="green">The green channel of the color.</param>
    /// <param name="blue">The blue channel of the color.</param>
    /// <returns>A splat color from the specified channels.</returns>
    public static SplatColor FromArgb(int alpha, int red, int green, int blue)
    {
        CheckArgbValues(alpha, red, green, blue);
        var newColor = new SplatColor
        {
            _state = (short)ColorTypes.ARGB,
            Value = ((uint)alpha << AlphaShift) + ((uint)red << RedShift) + ((uint)green << GreenShift) + (uint)blue,
        };

        return CheckIfIsKnownColor(newColor);
    }

    /// <summary>Creates a new <see cref="SplatColor"/> from another <see cref="SplatColor"/>, replacing its alpha with one specified.</summary>
    /// <param name="alpha">The new alpha component to set for the new <see cref="SplatColor"/>.</param>
    /// <param name="baseColor">The base color to use for the RGB values.</param>
    /// <returns>The new <see cref="SplatColor"/>.</returns>
    public static SplatColor FromArgb(int alpha, SplatColor baseColor) =>
        FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);

    /// <summary>Creates a new <see cref="SplatColor"/> from the specified int based ARGB value.</summary>
    /// <param name="argb">The int containing the ARGB values.</param>
    /// <returns>The new <see cref="SplatColor"/>.</returns>
    public static SplatColor FromArgb(uint argb) =>
        FromArgb(
            (int)((argb >> AlphaShift) & ChannelMask),
            (int)((argb >> RedShift) & ChannelMask),
            (int)((argb >> GreenShift) & ChannelMask),
            (int)(argb & ChannelMask));

    /// <summary>Gets a SplatColor from a <see cref="KnownColor"/> value.</summary>
    /// <param name="color">The color to generate.</param>
    /// <returns>The generated SplatValue.</returns>
    public static SplatColor FromKnownColor(KnownColor color)
    {
        var n = (short)color;
        SplatColor c;
        if (n <= 0 || n >= KnownColors.ArgbValues.Length)
        {
            // This is what it returns!
            c = Empty;
            c._state |= (short)ColorTypes.Named;
        }
        else
        {
            c = Empty;
            c._state = (short)(ColorTypes.ARGB | ColorTypes.Known | ColorTypes.Named);
            if (n is < FirstNonSystemKnownColor or > LastNonSystemKnownColor)
            {
                c._state |= (short)ColorTypes.System;
            }

            c.Value = KnownColors.ArgbValues[n];
        }

        c._knownColor = n;
        return c;
    }

    /// <summary>Gets a SplatColor from a name.</summary>
    /// <param name="name">The name of the color to generate.</param>
    /// <returns>The generated SplatValue.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Attempt to find best match.")]
    public static SplatColor FromName(string name)
    {
        try
        {
#if NET6_0_OR_GREATER
            var kc = Enum.Parse<KnownColor>(name, true);
#else
            KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), name, true);
#endif
            return FromKnownColor(kc);
        }
        catch (Exception ex)
        {
            LogHost.Default.Debug(ex, "Unable to parse the known colour name.");

            // This is what it returns!
            var d = Empty;
            d._name = name;
            d._state |= (short)ColorTypes.Named;
            return d;
        }
    }

    /// <summary>Gets the brightness of the color.</summary>
    /// <returns>The brightness of the value between 0 and 1.</returns>
    public float GetBrightness()
    {
        var minval = Math.Min(R, Math.Min(G, B));
        var maxval = Math.Max(R, Math.Max(G, B));

        return (float)(maxval + minval) / TwoChannelMax;
    }

    /// <summary>Gets the saturation of the color.</summary>
    /// <returns>The saturation of the value between 0 and 1.</returns>
    public float GetSaturation()
    {
        var minval = Math.Min(R, Math.Min(G, B));
        var maxval = Math.Max(R, Math.Max(G, B));

        if (maxval == minval)
        {
            return 0.0f;
        }

        var sum = maxval + minval;
        if (sum > MaxChannelValue)
        {
            sum = TwoChannelMax - sum;
        }

        return (float)(maxval - minval) / sum;
    }

    /// <summary>Gets the integer value of the color.</summary>
    /// <returns>The integer value.</returns>
    public uint ToArgb() => Value;

    /// <summary>Gets the hue of the color.</summary>
    /// <returns>The hue component of the color.</returns>
    public float GetHue()
    {
        int r = R;
        int g = G;
        int b = B;
        var minval = (byte)Math.Min(r, Math.Min(g, b));
        var maxval = (byte)Math.Max(r, Math.Max(g, b));

        if (maxval == minval)
        {
            return 0.0f;
        }

        float diff = maxval - minval;
        var rnorm = (maxval - r) / diff;
        var gnorm = (maxval - g) / diff;
        var bnorm = (maxval - b) / diff;

        var hue = 0.0f;
        if (r == maxval)
        {
            hue = DegreesPerSextant * (RedHueOffset + bnorm - gnorm);
        }

        if (g == maxval)
        {
            hue = DegreesPerSextant * (GreenHueOffset + rnorm - bnorm);
        }

        if (b == maxval)
        {
            hue = DegreesPerSextant * (BlueHueOffset + gnorm - rnorm);
        }

        if (hue > DegreesPerCircle)
        {
            hue -= DegreesPerCircle;
        }

        return hue;
    }

    /// <summary>Gets the <see cref="KnownColor"/> of the current value (if one is available).</summary>
    /// <returns>Returns the KnownColor enum value for this color, 0 if is not known.</returns>
    public readonly KnownColor ToKnownColor() => (KnownColor)_knownColor;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj switch
    {
        SplatColor splatColor => Equals(splatColor),
        _ => false
    };

    /// <inheritdoc />
    public bool Equals(SplatColor other) =>
        A == other.A && R == other.R && G == other.G && B == other.B;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // Value is a 32-bit uint, so the whole colour already fits in the hash; the
        // historic "Value >> 32" fold came from a 64-bit source value and would be a
        // no-op shift (undefined for a 32-bit operand), so it is omitted here.
        var hc = (int)(Value ^ (uint)_state ^ ((uint)_knownColor >> RedShift));
        if (IsNamedColor)
        {
            hc ^= StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        return hc;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsEmpty)
        {
            return "SplatColor [Empty]";
        }

        // Use the property here, not the field.
        return IsNamedColor ? "SplatColor [" + Name + "]" : $"SplatColor [A={A}, R={R}, G={G}, B={B}]";
    }

    /// <summary>Validates that the red, green, and blue components are each within the 0-255 range.</summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    /// <exception cref="ArgumentException">Thrown when any component is outside the 0-255 range.</exception>
    private static void CheckRgbValues(int red, int green, int blue)
    {
        if (red is > MaxChannelValue or < 0)
        {
            throw CreateColorArgumentException(red, nameof(red));
        }

        if (green is > MaxChannelValue or < 0)
        {
            throw CreateColorArgumentException(green, nameof(green));
        }

        if (!(blue is > MaxChannelValue or < 0))
        {
            return;
        }

        throw CreateColorArgumentException(blue, nameof(blue));
    }

    /// <summary>Creates an <see cref="ArgumentException"/> describing an out-of-range color component.</summary>
    /// <param name="value">The invalid component value.</param>
    /// <param name="color">The name of the component (for example, "red").</param>
    /// <returns>An <see cref="ArgumentException"/> describing the invalid value.</returns>
    private static ArgumentException CreateColorArgumentException(int value, string color) =>
        new(
            $"'{value}' is not a valid value for '{color}'. '{color}' should be greater or equal to 0 and less than or equal to {MaxChannelValue}.");

    /// <summary>Validates that the alpha, red, green, and blue components are each within the 0-255 range.</summary>
    /// <param name="alpha">The alpha component.</param>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    /// <exception cref="ArgumentException">Thrown when any component is outside the 0-255 range.</exception>
    private static void CheckArgbValues(int alpha, int red, int green, int blue)
    {
        if (alpha is > MaxChannelValue or < 0)
        {
            throw CreateColorArgumentException(alpha, nameof(alpha));
        }

        CheckRgbValues(red, green, blue);
    }

    /// <summary>Returns the known-color equivalent of the supplied color when its value matches one; otherwise returns it unchanged.</summary>
    /// <param name="splatColor">The color to test against the known-color table.</param>
    /// <returns>The matching known <see cref="SplatColor"/>, or <paramref name="splatColor"/> when no match exists.</returns>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "We don't want to do anything if caught and just return the Splat color.")]
    private static SplatColor CheckIfIsKnownColor(SplatColor splatColor)
    {
        try
        {
            var index = Array.IndexOf(KnownColors.ArgbValues, splatColor.Value);
            if (index >= 0)
            {
                var knownColorLookup = (KnownColor)index;
                return FromKnownColor(knownColorLookup);
            }
        }
        catch
        {
            // Intentionally ignored: if the known-color lookup fails for any reason we
            // simply fall back to returning the original color unchanged.
        }

        return splatColor;
    }
}
