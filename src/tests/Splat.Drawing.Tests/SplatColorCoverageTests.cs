// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Additional coverage tests for <see cref="SplatColor"/> members and classification flags.</summary>
public sealed class SplatColorCoverageTests
{
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
        var color = SplatColor.FromArgb(10, 20, 30, 40);

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
        var color = SplatColor.FromArgb(10, 20, 30, 40);

        await Assert.That(color.Name).IsEqualTo($"{color.ToArgb():x}");
    }

    /// <summary>Verifies that the A, R, G, and B components are extracted correctly from a packed value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Components_AreExtractedCorrectly()
    {
        var color = SplatColor.FromArgb(0x12, 0x34, 0x56, 0x78);

        using (Assert.Multiple())
        {
            await Assert.That(color.A).IsEqualTo((byte)0x12);
            await Assert.That(color.R).IsEqualTo((byte)0x34);
            await Assert.That(color.G).IsEqualTo((byte)0x56);
            await Assert.That(color.B).IsEqualTo((byte)0x78);
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
        var first = SplatColor.FromArgb(255, 0, 0, 139);
        var second = SplatColor.FromKnownColor(KnownColor.DarkBlue);

        await Assert.That(first.GetHashCode()).IsEqualTo(second.GetHashCode());
    }

    /// <summary>Verifies that comparing a color to a non-color object returns false.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Equals_WithNonColorObject_IsFalse()
    {
        var color = SplatColor.FromArgb(1, 2, 3, 4);

        await Assert.That(color.Equals("not a color")).IsFalse();
    }

    /// <summary>Verifies that the equality operator reports equal colors as equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task EqualityOperator_ForEqualColors_IsTrue()
    {
        var first = SplatColor.FromArgb(0, 0, 139);
        var second = SplatColor.FromArgb(255, 0, 0, 139);

        await Assert.That(first == second).IsTrue();
    }

    /// <summary>Verifies that the inequality operator reports differing colors as not equal.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InequalityOperator_ForDifferingColors_IsTrue()
    {
        var first = SplatColor.FromArgb(0, 0, 1);
        var second = SplatColor.FromArgb(0, 0, 2);

        await Assert.That(first != second).IsTrue();
    }

    /// <summary>Verifies that <see cref="SplatColor.Empty"/> stringifies to its empty form.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ToString_ForEmpty_GivesEmptyForm()
    {
        await Assert.That(SplatColor.Empty.ToString()).IsEqualTo("SplatColor [Empty]");
    }

    /// <summary>Verifies that a system known color reports as a system color.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FromKnownColor_SystemColor_IsSystemColor()
    {
        var color = SplatColor.FromKnownColor(KnownColor.Control);

        await Assert.That(color.IsSystemColor).IsTrue();
    }
}
