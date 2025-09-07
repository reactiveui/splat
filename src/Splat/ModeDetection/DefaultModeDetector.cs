// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Contains the default mode detector to detect if we are currently in a unit test.
/// </summary>
public class DefaultModeDetector : IModeDetector, IEnableLogger
{
    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    public bool? InUnitTestRunner()
    {
        var testAssemblies = new[]
        {
            "CSUNIT",
            "NUNIT",
            "XUNIT",
            "MBUNIT",
            "NBEHAVE",
            "VISUALSTUDIO.QUALITYTOOLS",
            "VISUALSTUDIO.TESTPLATFORM",
            "FIXIE",
            "NCRUNCH",
        };
        try
        {
            return SearchForAssembly(testAssemblies);
        }
        catch (Exception e)
        {
            this.Log().Debug(e, "Unable to find unit test runner value");
            return null;
        }
    }

    private static bool SearchForAssembly(IEnumerable<string> assemblyList) =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Select(x => x.FullName?.ToUpperInvariant())
            .Where(x => x is not null)
            .Select(x => x!)
#if NETSTANDARD || NET_45
            .Any(x => assemblyList.Any(name => x.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) != -1));
#else
            .Any(x => assemblyList.Any(name => x.Contains(name, StringComparison.InvariantCultureIgnoreCase)));
#endif
}
