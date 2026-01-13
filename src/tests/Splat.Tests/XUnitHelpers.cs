// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>
/// Helpers for XUnit.
/// </summary>
public static class XUnitHelpers
{
    /// <summary>
    /// Gets Theory Permutations for all the values in an enum.
    /// </summary>
    /// <typeparam name="TEnum">The type of enum.</typeparam>
    /// <returns>An XUnit theory data source.</returns>
    public static IEnumerable<object[]> GetEnumAsTestTheory<TEnum>()
    {
        var values = Enum.GetValues(typeof(TEnum));
        var results = new List<object[]>(values.Length);
        results.AddRange(values.Cast<object?>().Select(value => new[] { value! }));

        return results;
    }
}
