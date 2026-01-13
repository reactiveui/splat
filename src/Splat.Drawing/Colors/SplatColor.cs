// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
    //// Private transparency (A) and R,G,B fields.

    private short _state;
    private short _knownColor;

    // #if ONLY_1_1
    // Mono bug #324144 is holding this change
    // MS 1.1 requires this member to be present for serialization (not so in 2.0)
    // however it's bad to keep a string (reference) in a struct
    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="SplatColor"/> struct with the specified color value, state, known color
    /// identifier, and name.
    /// </summary>
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

    /// <summary>
    /// Specifies the type or characteristics of a color value.
    /// </summary>
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
    internal enum ColorType : short
    {
        Empty = 0,
        Known = 1,
        ARGB = 2,
        Named = 4,
        System = 8,
    }

    /// <summary>
    /// Gets an instance of SplatColor that represents an uninitialized or empty color value (transparent).
    /// </summary>
    public static SplatColor Empty { get; }

    /// <summary>
    /// Gets a value indicating whether the current color is transparent black. Eg where R,G,B,A == 0.
    /// </summary>
    public readonly bool IsEmpty => _state == (short)ColorType.Empty;

    /// <summary>
    /// Gets the alpha component of the color.
    /// </summary>
    public byte A => (byte)(Value >> 24);

    /// <summary>
    /// Gets the red component of the color.
    /// </summary>
    public byte R => (byte)(Value >> 16);

    /// <summary>
    /// Gets the green component of the color.
    /// </summary>
    public byte G => (byte)(Value >> 8);

    /// <summary>
    /// Gets the blue component of the color.
    /// </summary>
    public byte B => (byte)Value;

    /// <summary>
    /// Gets the name of the color if one is known. Otherwise will be the hex value.
    /// </summary>
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

    /// <summary>
    /// Gets a value indicating whether the color is part of the <see cref="ColorType.Known"/> group.
    /// </summary>
    public readonly bool IsKnownColor => (_state & (short)ColorType.Known) != 0;

    /// <summary>
    /// Gets a value indicating whether the color is part of the <see cref="ColorType.System"/> group.
    /// </summary>
    public readonly bool IsSystemColor => (_state & (short)ColorType.System) != 0;

    /// <summary>
    /// Gets a value indicating whether the color is par tof the <see cref="ColorType.Known"/> or <see cref="ColorType.Named"/> groups.
    /// </summary>
    public readonly bool IsNamedColor => (_state & (short)(ColorType.Known | ColorType.Named)) != 0;

    /// <summary>
    /// Gets or sets the value of the color.
    /// </summary>
    [DataMember]
    internal uint Value
    {
        get
        {
            // Optimization for known colors that were deserialized
            // from an MS serialized stream.
            if (field == 0 && IsKnownColor)
            {
                field = KnownColors.FromKnownColor((KnownColor)_knownColor).ToArgb() & 0xFFFFFFFF;
            }

            return field;
        }

        set;
    }

    /// <summary>
    /// Compares two SplatColor references and determines if they are equivalent based on their A,R,G,B values.
    /// </summary>
    /// <param name="left">The first SplatColor to compare.</param>
    /// <param name="right">The second SplatColor to compare.</param>
    /// <returns>If they are equivalent to each other.</returns>
    public static bool operator ==(SplatColor left, SplatColor right) =>
        left.Equals(right);

    /// <summary>
    /// Compares two SplatColor references and determines if they are not equivalent based on their A,R,G,B values.
    /// </summary>
    /// <param name="left">The first SplatColor to compare.</param>
    /// <param name="right">The second SplatColor to compare.</param>
    /// <returns>If they are not equivalent to each other.</returns>
    public static bool operator !=(SplatColor left, SplatColor right) =>
        !left.Equals(right);

    /// <summary>
    /// Creates a SplatColor from the RGB values.
    /// The alpha will be set to 255 for full alpha.
    /// </summary>
    /// <param name="red">The red channel of the color.</param>
    /// <param name="green">The green channel of the color.</param>
    /// <param name="blue">The blue channel of the color.</param>
    /// <returns>A splat color from the specified channels.</returns>
    public static SplatColor FromArgb(int red, int green, int blue) => FromArgb(255, red, green, blue);

    /// <summary>
    /// Creates a SplatColor from the RGB values.
    /// </summary>
    /// <param name="alpha">The alpha channel of the color.</param>
    /// <param name="red">The red channel of the color.</param>
    /// <param name="green">The green channel of the color.</param>
    /// <param name="blue">The blue channel of the color.</param>
    /// <returns>A splat color from the specified channels.</returns>
    public static SplatColor FromArgb(int alpha, int red, int green, int blue)
    {
        CheckARGBValues(alpha, red, green, blue);
        var newColor = new SplatColor
        {
            _state = (short)ColorType.ARGB,
            Value = ((uint)alpha << 24) + ((uint)red << 16) + ((uint)green << 8) + (uint)blue,
        };

        return CheckIfIsKnownColor(newColor);
    }

    /// <summary>
    /// Creates a new <see cref="SplatColor"/> from another <see cref="SplatColor"/>, replacing its alpha with one specified.
    /// </summary>
    /// <param name="alpha">The new alpha component to set for the new <see cref="SplatColor"/>.</param>
    /// <param name="baseColor">The base color to use for the RGB values.</param>
    /// <returns>The new <see cref="SplatColor"/>.</returns>
    public static SplatColor FromArgb(int alpha, SplatColor baseColor) =>
        FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);

    /// <summary>
    /// Creates a new <see cref="SplatColor"/> from the specified int based ARGB value.
    /// </summary>
    /// <param name="argb">The int containing the ARGB values.</param>
    /// <returns>The new <see cref="SplatColor"/>.</returns>
    public static SplatColor FromArgb(uint argb) =>
        FromArgb((int)((argb >> 24) & 0x0FF), (int)((argb >> 16) & 0x0FF), (int)((argb >> 8) & 0x0FF), (int)(argb & 0x0FF));

    /// <summary>
    /// Gets a SplatColor from a <see cref="KnownColor"/> value.
    /// </summary>
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
            c._state |= (short)ColorType.Named;
        }
        else
        {
            c = Empty;
            c._state = (short)(ColorType.ARGB | ColorType.Known | ColorType.Named);
            if (n is < 27 or > 169)
            {
                c._state |= (short)ColorType.System;
            }

            c.Value = KnownColors.ArgbValues[n];
        }

        c._knownColor = n;
        return c;
    }

    /// <summary>
    /// Gets a SplatColor from a name.
    /// </summary>
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
            d._state |= (short)ColorType.Named;
            return d;
        }
    }

    /// <summary>
    /// Gets the brightness of the color.
    /// </summary>
    /// <returns>The brightness of the value between 0 and 1.</returns>
    public float GetBrightness()
    {
        var minval = Math.Min(R, Math.Min(G, B));
        var maxval = Math.Max(R, Math.Max(G, B));

        return (float)(maxval + minval) / 510;
    }

    /// <summary>
    /// Gets the saturation of the color.
    /// </summary>
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
        if (sum > 255)
        {
            sum = 510 - sum;
        }

        return (float)(maxval - minval) / sum;
    }

    /// <summary>
    /// Gets the integer value of the color.
    /// </summary>
    /// <returns>The integer value.</returns>
    public uint ToArgb() => Value;

    /// <summary>
    /// Gets the hue of the color.
    /// </summary>
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
            hue = 60.0f * (6.0f + bnorm - gnorm);
        }

        if (g == maxval)
        {
            hue = 60.0f * (2.0f + rnorm - bnorm);
        }

        if (b == maxval)
        {
            hue = 60.0f * (4.0f + gnorm - rnorm);
        }

        if (hue > 360.0f)
        {
            hue -= 360.0f;
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
        var hc = (int)(Value ^ (Value >> 32) ^ _state ^ (_knownColor >> 16));
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

    private static void CheckRGBValues(int red, int green, int blue)
    {
        if (red is > 255 or < 0)
        {
            throw CreateColorArgumentException(red, "red");
        }

        if (green is > 255 or < 0)
        {
            throw CreateColorArgumentException(green, "green");
        }

        if (blue is > 255 or < 0)
        {
            throw CreateColorArgumentException(blue, "blue");
        }
    }

    private static ArgumentException CreateColorArgumentException(int value, string color) =>
        new($"'{value}' is not a valid value for '{color}'. '{color}' should be greater or equal to 0 and less than or equal to 255.");

    private static void CheckARGBValues(int alpha, int red, int green, int blue)
    {
        if (alpha is > 255 or < 0)
        {
            throw CreateColorArgumentException(alpha, "alpha");
        }

        CheckRGBValues(red, green, blue);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We don't want to do anything if caught and just return the Splat color.")]
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
        }

        return splatColor;
    }
}
