// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Colors;

/// <summary>Unit Tests for the Splat Color logic.</summary>
public class SplatColorTests
{
    /// <summary>Gets every <see cref="KnownColor"/> value as a strongly-typed TUnit data source.</summary>
    public static IEnumerable<KnownColor> KnownColorValues => Enum.GetValues<KnownColor>();

    /// <summary>Tests to check you can get a SplatColor from a KnownColor.</summary>
    /// <param name="knownColor">The Known Colour to convert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorValues))]
    public async Task FromKnownColorTests(KnownColor knownColor)
    {
        var splatColor = SplatColor.FromKnownColor(knownColor);

        await Assert.That(splatColor.Name).IsNotNull();
    }

    /// <summary>Tests to check you can get a SplatColor from a name.</summary>
    /// <param name="knownColor">The Known Colour to convert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorValues))]
    public async Task FromNameTests(KnownColor knownColor)
    {
        var splatColor = SplatColor.FromName(knownColor.ToString());

        await Assert.That(splatColor.Name).IsNotNull();
    }
}
