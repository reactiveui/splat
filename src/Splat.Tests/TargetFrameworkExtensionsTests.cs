// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>
/// Unit Tests for Target Framework Extensions.
/// </summary>
public class TargetFrameworkExtensionsTests
{
    /// <summary>
    /// Gets the test source data for Framework names.
    /// </summary>
    public static IEnumerable<object[]> FrameworkNamesTestSource { get; } = new[]
    {
        new object[]
        {
            ".NETCoreApp,Version=v2.2",
            "netcoreapp2.2",
        },
        [
            ".NETCoreApp,Version=v2.1",
            "netcoreapp2.1",
        ],
        [
            ".NETCoreApp,Version=v2.0",
            "netcoreapp2.0",
        ],
        [
            ".NETCoreApp,Version=v1.1",
            "netcoreapp1.1",
        ],
        [
            ".NETCoreApp,Version=v1.0",
            "netcoreapp1.0",
        ],
        [
            ".NETStandard,Version=v2.0",
            "netstandard2.0",
        ],
        [
            ".NETStandard,Version=v1.6",
            "netstandard1.6",
        ],
        [
            ".NETStandard,Version=v1.5",
            "netstandard1.5",
        ],
        [
            ".NETStandard,Version=v1.4",
            "netstandard1.4",
        ],
        [
            ".NETStandard,Version=v1.3",
            "netstandard1.3",
        ],
        [
            ".NETStandard,Version=v1.2",
            "netstandard1.2",
        ],
        [
            ".NETStandard,Version=v1.1",
            "netstandard1.1",
        ],
        [
            ".NETStandard,Version=v1.0",
            "netstandard1.0",
        ],
        [
            ".NETFramework,Version=v4.7.2",
            "net472",
        ],
        [
            ".NETFramework,Version=v4.7.1",
            "net471",
        ],
        [
            ".NETFramework,Version=v4.7",
            "net47",
        ],
        [
            ".NETFramework,Version=v4.6.2",
            "net462",
        ],
        [
            ".NETFramework,Version=v4.6.1",
            "net461",
        ],
        [
            ".NETFramework,Version=v4.6",
            "net46",
        ],
        [
            ".NETFramework,Version=v4.5.2",
            "net452",
        ],
        [
            ".NETFramework,Version=v4.5.1",
            "net451",
        ],
        [
            ".NETFramework,Version=v4.5",
            "net45",
        ],
        [
            ".NETFramework,Version=v4.0.3",
            "net403",
        ],
        [
            ".NETFramework,Version=v4.0",
            "net40",
        ],
        [
            ".NETFramework,Version=v3.5",
            "net35",
        ],
        [
            ".NETFramework,Version=v2.0",
            "net20",
        ],
        [
            ".NETFramework,Version=v1.1",
            "net11",
        ],
    };

    /// <summary>
    /// Checks to ensure the framework name is returned.
    /// </summary>
    /// <param name="frameworkName">The framework name.</param>
    /// <param name="expected">The expected result.</param>
    [Theory]
    [MemberData(nameof(FrameworkNamesTestSource))]
    public void ReturnsName(string frameworkName, string expected) => Assert.Equal(expected, TargetFrameworkExtensions.GetTargetFrameworkName(frameworkName));
}
