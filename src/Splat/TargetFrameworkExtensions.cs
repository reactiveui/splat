// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;
using System.Runtime.Versioning;

namespace Splat;

/// <summary>Provides extension methods for retrieving target framework information from assemblies.</summary>
/// <remarks>This class contains static methods that extend the functionality of the <see
/// cref="System.Reflection.Assembly"/> type, enabling callers to determine the target framework an assembly was built
/// against. These methods are useful for scenarios where runtime inspection of assembly metadata is required, such as
/// diagnostics, tooling, or compatibility checks.</remarks>
public static class TargetFrameworkExtensions
{
    /// <summary>Extension members for retrieving target framework information from an <see cref="Assembly"/>.</summary>
    /// <param name="assembly">The assembly the extension members operate on.</param>
    extension(Assembly assembly)
    {
        /// <summary>Retrieves the target framework name specified for the given assembly, if available.</summary>
        /// <remarks>The target framework name is typically defined by the TargetFrameworkAttribute applied to the
        /// assembly. If the attribute is not present, this method returns null.</remarks>
        /// <returns>A string containing the target framework name (for example, ".NETCoreApp,Version=v8.0"), or null if the assembly
        /// does not specify a target framework.</returns>
        public string? GetTargetFrameworkName()
        {
            var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

            return TargetFrameworkExtensions.GetTargetFrameworkName(targetFrameworkAttribute?.FrameworkName);
        }
    }

    /// <summary>Maps full framework monikers (for example, ".NETCoreApp,Version=v8.0") to their short TFMs (for example, "net8.0").</summary>
    private static readonly Dictionary<string, string> _frameworkNameToShortName = new(StringComparer.Ordinal)
    {
        [".NETCoreApp,Version=v10.0"] = "net10.0",
        [".NETCoreApp,Version=v9.0"] = "net9.0",
        [".NETCoreApp,Version=v8.0"] = "net8.0",
        [".NETCoreApp,Version=v7.0"] = "net7.0",
        [".NETCoreApp,Version=v6.0"] = "net6.0",
        [".NETCoreApp,Version=v5.0"] = "net5.0",
        [".NETCoreApp,Version=v3.1"] = "netcoreapp3.1",
        [".NETCoreApp,Version=v3.0"] = "netcoreapp3.0",
        [".NETCoreApp,Version=v2.2"] = "netcoreapp2.2",
        [".NETCoreApp,Version=v2.1"] = "netcoreapp2.1",
        [".NETCoreApp,Version=v2.0"] = "netcoreapp2.0",
        [".NETCoreApp,Version=v1.1"] = "netcoreapp1.1",
        [".NETCoreApp,Version=v1.0"] = "netcoreapp1.0",
        [".NETStandard,Version=v2.1"] = "netstandard2.1",
        [".NETStandard,Version=v2.0"] = "netstandard2.0",
        [".NETStandard,Version=v1.6"] = "netstandard1.6",
        [".NETStandard,Version=v1.5"] = "netstandard1.5",
        [".NETStandard,Version=v1.4"] = "netstandard1.4",
        [".NETStandard,Version=v1.3"] = "netstandard1.3",
        [".NETStandard,Version=v1.2"] = "netstandard1.2",
        [".NETStandard,Version=v1.1"] = "netstandard1.1",
        [".NETStandard,Version=v1.0"] = "netstandard1.0",
        [".NETFramework,Version=v4.8.1"] = "net481",
        [".NETFramework,Version=v4.8"] = "net48",
        [".NETFramework,Version=v4.7.2"] = "net472",
        [".NETFramework,Version=v4.7.1"] = "net471",
        [".NETFramework,Version=v4.7"] = "net47",
        [".NETFramework,Version=v4.6.2"] = "net462",
        [".NETFramework,Version=v4.6.1"] = "net461",
        [".NETFramework,Version=v4.6"] = "net46",
        [".NETFramework,Version=v4.5.2"] = "net452",
        [".NETFramework,Version=v4.5.1"] = "net451",
        [".NETFramework,Version=v4.5"] = "net45",
        [".NETFramework,Version=v4.0.3"] = "net403",
        [".NETFramework,Version=v4.0"] = "net40",
        [".NETFramework,Version=v3.5"] = "net35",
        [".NETFramework,Version=v2.0"] = "net20",
        [".NETFramework,Version=v1.1"] = "net11",
    };

    /// <summary>Maps a full framework moniker (for example, ".NETCoreApp,Version=v8.0") to its short TFM (for example, "net8.0").</summary>
    /// <param name="frameworkName">The full framework name, or <see langword="null"/>.</param>
    /// <returns>The short target framework moniker, or <see langword="null"/> when the framework is unknown.</returns>
    internal static string? GetTargetFrameworkName(string? frameworkName) =>
        frameworkName is not null && _frameworkNameToShortName.TryGetValue(frameworkName, out var shortName)
            ? shortName
            : null;
}
