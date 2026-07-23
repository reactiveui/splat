// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Colors;

/// <summary>Unit tests for the <see cref="KnownColor"/> enumeration and its helpers.</summary>
public class KnownColorTests
{
    /// <summary>The highest <see cref="KnownColor"/> numeric value whose name can be asserted on every target framework.</summary>
    private const short MaxAssessableKnownColorValue = 167;

    /// <summary>A replacement ARGB value not present in the known-color table, used to re-point a known color at runtime.</summary>
    private const int ReplacementArgb = 0x1A2B3C4D;

    /// <summary>Gets all KnownColor enum values for parameterized testing.</summary>
    public static IEnumerable<KnownColor> KnownColorValues => Enum.GetValues<KnownColor>();

    /// <summary>Tests to ensure a name is returned from a number akin to a KnownColor.</summary>
    /// <param name="knownColor">Known Color Enum to check.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorValues))]
    public async Task GetNameReturnsName(KnownColor knownColor)
    {
#if !NET_2_0
        if ((short)knownColor > MaxAssessableKnownColorValue)
        {
            // Can't assess these legacy values in this target — match original behavior.
            return;
        }
#endif

        var name = KnownColors.GetName(knownColor);

        using (Assert.Multiple())
        {
            await Assert.That(name.Trim()).IsNotEmpty(); // no whitespace-only names
        }
    }

    /// <summary>
    /// Verifies that <see cref="KnownColors.Update(int,int)"/> drops the stale reverse-lookup entry when the
    /// updated color is the canonical owner of its previous ARGB, and re-points the reverse index at the new value.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [NotInParallel] // Mutates the global KnownColors ARGB table and reverse index.
    public async Task UpdateRepointsReverseIndexForCanonicalColor()
    {
        // Transparent is the sole owner of 0x00FFFFFF, so the reverse index maps that ARGB back to it.
        const KnownColor knownColor = KnownColor.Transparent;
        const int index = (int)knownColor;
        var originalArgb = KnownColors.GetArgb(knownColor);

        try
        {
            KnownColors.Update(index, ReplacementArgb);

            var newMatch = KnownColors.FindColorMatch(SplatColor.FromArgb((uint)ReplacementArgb));
            var oldMatch = KnownColors.FindColorMatch(SplatColor.FromArgb(originalArgb));

            using (Assert.Multiple())
            {
                await Assert.That(KnownColors.GetArgb(knownColor)).IsEqualTo((uint)ReplacementArgb);

                // The reverse index now resolves the new ARGB to the updated known color.
                await Assert.That(newMatch.ToKnownColor()).IsEqualTo(knownColor);

                // The stale mapping for the previous ARGB was removed, so it no longer matches.
                await Assert.That(oldMatch.IsEmpty).IsTrue();
            }
        }
        finally
        {
            KnownColors.Update(index, (int)originalArgb);
        }
    }
}
