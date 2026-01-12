// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldShouldBePrivate", Justification = "XUnit Theories.")]

namespace Splat.Tests.Colors;

/// <summary>
/// Unit Tests for the Splat Color logic.
/// </summary>
public class SplatColorTests
{
    /// <summary>
    /// Gets the test data for FromKnownColor.
    /// </summary>
    public static IEnumerable<object[]> KnownColorEnums { get; } = XUnitHelpers.GetEnumAsTestTheory<KnownColor>();

    /// <summary>
    /// Tests to check you can get a SplatColor from a KnownColor.
    /// </summary>
    /// <param name="knownColor">The Known Colour to convert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorEnums))]
    public async Task FromKnownColorTests(KnownColor knownColor)
    {
        var splatColor = SplatColor.FromKnownColor(knownColor);

        await Assert.That(splatColor.Name).IsNotNull();
    }

    /// <summary>
    /// Tests to check you can get a SplatColor from a name.
    /// </summary>
    /// <param name="knownColor">The Known Colour to convert.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorEnums))]
    public async Task FromNameTests(KnownColor knownColor)
    {
        var splatColor = SplatColor.FromName(knownColor.ToString());

        await Assert.That(splatColor.Name).IsNotNull();
    }

    ////private static IEnumerable<object[]> GetEnumAsTestTheory()
    ////{
    ////    var values = Enum.GetValues(typeof(KnownColor));
    ////    var results = new List<object[]>(values.Length);
    ////    results.AddRange(values.Cast<object?>().Select(value => new[] { value! }));

    ////    return results;
    ////}
}
