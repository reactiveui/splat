// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Colors;

public class KnownColorTests
{
    /// <summary>
    /// Gets all KnownColor enum values for parameterized testing.
    /// </summary>
    public static IEnumerable<KnownColor> KnownColorValues => Enum.GetValues<KnownColor>();

    /// <summary>
    /// Tests to ensure a name is returned from a number akin to a KnownColor.
    /// </summary>
    /// <param name="knownColor">Known Color Enum to check.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [MethodDataSource(nameof(KnownColorValues))]
    public async Task GetNameReturnsName(KnownColor knownColor)
    {
#if !NET_2_0
        if ((short)knownColor > 167)
        {
            // Can't assess these legacy values in this target — match original behavior.
            return;
        }
#endif

        var name = KnownColors.GetName(knownColor);

        using (Assert.Multiple())
        {
            await Assert.That(name!.Trim()).IsNotEmpty(); // no whitespace-only names
        }
    }
}
