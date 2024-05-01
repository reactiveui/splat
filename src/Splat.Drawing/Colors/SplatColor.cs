// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Splat;

/// <summary>
/// A platform independent color structure.
/// </summary>
[DataContract]
public partial struct SplatColor : IEquatable<SplatColor>
{
    // Private transparency (A) and R,G,B fields.
    private uint _value;

    private short _state;
    private short _knownColor;

    // #if ONLY_1_1
    // Mono bug #324144 is holding this change
    // MS 1.1 requires this member to be present for serialization (not so in 2.0)
    // however it's bad to keep a string (reference) in a struct
    private string _name;

    internal SplatColor(uint value, short state, short knownColor, string name)
    {
        _value = value;
        _state = state;
        _knownColor = knownColor;
        _name = name;
    }

    // The specs also indicate that all three of these properties are true
    // if created with FromKnownColor or FromNamedColor, false otherwise (FromARGB).
    // Per Microsoft and ECMA specs these varibles are set by which constructor is used, not by their values.
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
    /// Gets a full empty which is fully transparent.
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
#if NET_2_0_ONCE_MONO_BUG_324144_IS_FIXED
    if (IsNamedColor)
      return KnownColors.GetName (knownColor);
    else
      return String.Format ("{0:x}", ToArgb ());
#else
            // name is required for serialization under 1.x, but not under 2.0
            // Can happen with stuff deserialized from MS
            _name ??= IsNamedColor ? KnownColors.GetName(_knownColor) : $"{ToArgb():x}";

            return _name;
#endif
        }
    }

    /// <summary>
    /// Gets a value indicating whether the color is part of the <see cref="ColorType.Known"/> group.
    /// </summary>
    public readonly bool IsKnownColor => (_state & ((short)ColorType.Known)) != 0;

    /// <summary>
    /// Gets a value indicating whether the color is part of the <see cref="ColorType.System"/> group.
    /// </summary>
    public readonly bool IsSystemColor => (_state & ((short)ColorType.System)) != 0;

    /// <summary>
    /// Gets a value indicating whether the color is par tof the <see cref="ColorType.Known"/> or <see cref="ColorType.Named"/> groups.
    /// </summary>
    public readonly bool IsNamedColor => (_state & (short)(ColorType.Known | ColorType.Named)) != 0;

#if TARGET_JVM
    /// <summary>
    /// Gets the java native object of the color.
    /// </summary>
    internal java.awt.SplatColor NativeObject => return new java.awt.SplatColor (R, G, B, A);
#endif

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
            if (_value == 0 && IsKnownColor)
            {
                _value = KnownColors.FromKnownColor((KnownColor)_knownColor).ToArgb() & 0xFFFFFFFF;
            }

            return _value;
        }

        set => _value = value;
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
        if ((n <= 0) || (n >= KnownColors.ArgbValues.Length))
        {
            // This is what it returns!
            c = Empty;
            c._state |= (short)ColorType.Named;
        }
        else
        {
            c = Empty;
            c._state = (short)(ColorType.ARGB | ColorType.Known | ColorType.Named);
            if ((n < 27) || (n > 169))
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
    public static SplatColor FromName(string name)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), name, true);
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
#pragma warning restore CA1031 // Do not catch general exception types
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

#if TARGET_JVM
internal static SplatColor FromArgbNamed (int alpha, int red, int green, int blue, string name, KnownColor knownColor)
{
  SplatColor color = FromArgb (alpha, red, green, blue);
  color.state = (short) (ColorType.Known|ColorType.Named);
  color.name = KnownColors.GetName (knownColor);
  color.knownColor = (short) knownColor;
  return color;
}

internal static SplatColor FromArgbSystem (int alpha, int red, int green, int blue, string name, KnownColor knownColor)
{
  SplatColor color = FromArgbNamed (alpha, red, green, blue, name, knownColor);
  color.state |= (short) ColorType.System;
  return color;
}
#endif

    private static void CheckRGBValues(int red, int green, int blue)
    {
        if ((red > 255) || (red < 0))
        {
            throw CreateColorArgumentException(red, "red");
        }

        if ((green > 255) || (green < 0))
        {
            throw CreateColorArgumentException(green, "green");
        }

        if ((blue > 255) || (blue < 0))
        {
            throw CreateColorArgumentException(blue, "blue");
        }
    }

    private static ArgumentException CreateColorArgumentException(int value, string color) =>
        new($"'{value}' is not a valid value for '{color}'. '{color}' should be greater or equal to 0 and less than or equal to 255.");

    private static void CheckARGBValues(int alpha, int red, int green, int blue)
    {
        if ((alpha > 255) || (alpha < 0))
        {
            throw CreateColorArgumentException(alpha, "alpha");
        }

        CheckRGBValues(red, green, blue);
    }

    private static SplatColor CheckIfIsKnownColor(SplatColor splatColor)
    {
#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types

        return splatColor;
    }
}
