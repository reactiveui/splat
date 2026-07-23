// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>Provides a default implementation for detecting whether the current process is running within a unit test environment.</summary>
/// <remarks>This class uses a combination of environment variable checks, process name heuristics, and assembly
/// scanning to determine if the application is executing under a known test runner. It is intended for use in scenarios
/// where behavior should change when running under test conditions, such as enabling test-specific features or
/// diagnostics. The detection logic prioritizes explicit signals and is designed to minimize performance impact by
/// avoiding unnecessary allocations and expensive operations where possible.</remarks>
[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach", Justification = "Performance")]
[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery", Justification = "Performance")]
public class DefaultModeDetector : IModeDetector, IEnableLogger
{
    /// <summary>
    /// A static array containing known markers of test assemblies.
    /// These markers are utilized as part of the detection logic to determine
    /// if the current process is running in a unit test environment. Each marker
    /// represents a known identifier or namespace commonly used in unit testing frameworks.
    /// </summary>
    private static readonly string[] _testAssemblyMarkers =
    [
        "CSUNIT",
        "NUNIT",
        "XUNIT",
        "MBUNIT",
        "NBEHAVE",
        "VISUALSTUDIO.QUALITYTOOLS",
        "VISUALSTUDIO.TESTPLATFORM",
        "MICROSOFT.TESTING.PLATFORM",
        "FIXIE",
        "NCRUNCH"
    ];

    /// <summary>
    /// A static array containing the names of known test host processes.
    /// These process names are used to detect whether the current application
    /// is being executed in a unit testing environment. Each entry indicates
    /// a common test host or console runner associated with various testing frameworks.
    /// </summary>
    private static readonly string[] _knownTestHostProcesses =
    [
        "testhost",
        "testhost.x86",
        "vstest.console",
        "vstest.executionengine",
        "xunit.console",
        "nunit3-console",
        "nunit-console",
        "nunit-agent",
        "resharpertestrunner",
        "jetbrains.resharper.taskrunner.clr45"
    ];

    /// <summary>
    /// A static array containing markers found within the main module path of a process that hosts a test runner
    /// (for example, a "dotnet" process hosting testhost or vstest).
    /// </summary>
    private static readonly string[] _testHostModuleMarkers =
    [
        "testhost",
        "vstest"
    ];

    /// <summary>
    /// A static array containing exact environment variable names commonly used to identify
    /// test runners. Each variable in this list is checked to directly determine
    /// whether the application is running in a unit test environment.
    /// </summary>
    private static readonly string[] _exactEnvVars =
    [
        "XUNIT_TEST",
        "NUNIT_TEST"
    ];

    /// <summary>
    /// A static array containing prefixes for environment variable names that are indicative of test runners.
    /// These prefixes are used to detect if the current process is executing in a unit test environment by checking
    /// whether any environment variables start with these prefixes.
    /// </summary>
    private static readonly string[] _envPrefixes =
    [
        "VSTEST_",
        "NUNIT_",
        "XUNIT_"
    ];

    /// <inheritdoc />
    public bool? InUnitTestRunner() => DetectUnitTestRunnerSafely();

    /// <summary>Determines whether any of the supplied markers appears within a candidate name.</summary>
    /// <param name="name">The candidate name (process name, module path, or assembly full name); may be <see langword="null"/>.</param>
    /// <param name="markers">The markers to search for within <paramref name="name"/>.</param>
    /// <param name="comparison">The comparison used when searching for each marker.</param>
    /// <returns><see langword="true"/> when <paramref name="name"/> contains any marker; otherwise <see langword="false"/>.</returns>
    internal static bool NameContainsAnyMarker(string? name, string[] markers, StringComparison comparison)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        for (var i = 0; i < markers.Length; i++)
        {
#if NETFRAMEWORK
            if (name!.IndexOf(markers[i], comparison) >= 0)
#else
            if (name.Contains(markers[i], comparison))
#endif
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Reads and interprets the explicit DOTNET_RUNNING_IN_TEST signal.
    /// Explicit signal that users can set in their test setup for deterministic behavior.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the environment explicitly signals a test environment; otherwise, <c>false</c>.
    /// </returns>
    private static bool HasExplicitTestSignal()
    {
        var value = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_TEST");
        if (string.IsNullOrWhiteSpace(value))
        {
#if NETFRAMEWORK
            var appCtx = AppDomain.CurrentDomain.GetData("DOTNET_RUNNING_IN_TEST") as string;
#else
            var appCtx = AppContext.GetData("DOTNET_RUNNING_IN_TEST") as string;
#endif
            if (!string.IsNullOrWhiteSpace(appCtx))
            {
                value = appCtx;
            }
        }

        if (value is null)
        {
            return false;
        }

        // Normalize checks without allocating additional strings.
        const int yesLength = 3;
        const int trueLength = 4;
        return value.Length switch
        {
            1 => value == "1",
            yesLength => value.Equals("yes", StringComparison.OrdinalIgnoreCase),
            trueLength => value.Equals("true", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }

    /// <summary>Scans exact-named and prefixed environment variables for signals emitted by popular test runners or their hosts.</summary>
    /// <returns>
    /// A boolean value indicating whether test runner environment signals are detected.
    /// </returns>
    private static bool ScanEnvironmentForTestRunnerSignals()
    {
        // Fast-path: exact-name checks (no enumeration).
        for (var i = 0; i < _exactEnvVars.Length; i++)
        {
            var v = Environment.GetEnvironmentVariable(_exactEnvVars[i]);
            if (!string.IsNullOrEmpty(v))
            {
                return true;
            }
        }

        // Slower path: enumerate env vars once and check prefixes.
        foreach (DictionaryEntry kv in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
        {
            if (kv.Key is not string key || string.IsNullOrEmpty(key))
            {
                continue;
            }

            for (var i = 0; i < _envPrefixes.Length; i++)
            {
                if (key.StartsWith(_envPrefixes[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines if the current environment explicitly indicates a test environment based on specific environment variables.</summary>
    /// <returns>
    /// <c>true</c> if the environment explicitly signals a test environment; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>The try/catch guards environment and AppContext access, which can throw under restricted hosts, and is excluded from coverage.</remarks>
    [ExcludeFromCodeCoverage]
    private static bool IsExplicitTestEnvironment()
    {
        try
        {
            return HasExplicitTestSignal();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Determines if current environment has test runner signals present.</summary>
    /// <returns>
    /// A boolean value indicating whether test runner environment signals are detected.
    /// </returns>
    /// <remarks>The try/catch guards environment enumeration, which can throw under restricted hosts, and is excluded from coverage.</remarks>
    [ExcludeFromCodeCoverage]
    private static bool HasTestRunnerEnvironmentSignals()
    {
        try
        {
            return ScanEnvironmentForTestRunnerSignals();
        }
        catch (Exception ex)
        {
            // Ignore enumeration/access issues and treat as not found.
            System.Diagnostics.Debug.WriteLine(ex);
            return false;
        }
    }

    /// <summary>
    /// Determines whether the current process is a known test host process by examining the process name.
    /// Last-resort heuristic: detect known test hosts by process name.
    /// </summary>
    /// <returns>
    /// True if the process is identified as a known test host; otherwise, false.
    /// </returns>
    /// <remarks>Process inspection is environment-dependent and its try/catch guards restricted hosts, so it is excluded from
    /// coverage; the pure name matching is covered via <see cref="NameContainsAnyMarker"/>.</remarks>
    [ExcludeFromCodeCoverage]
    private static bool IsKnownTestHostProcess()
    {
        try
        {
            // The process name matches a known host directly, or (for a "dotnet" process hosting testhost/vstest) its main module path does.
            var procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return NameContainsAnyMarker(procName, _knownTestHostProcesses, StringComparison.OrdinalIgnoreCase)
                || MainModuleIndicatesTestHost();
        }
        catch (Exception ex)
        {
            // Ignore all process inspection failures.
            System.Diagnostics.Debug.WriteLine(ex);
            return false;
        }
    }

    /// <summary>Determines whether the current process's main module path indicates a test host (for example, a "dotnet" process hosting testhost/vstest).</summary>
    /// <returns>True if the main module path indicates a test host; otherwise, false.</returns>
    /// <remarks>Access to MainModule is environment-dependent and can fail in restricted environments, so the method is excluded from coverage.</remarks>
    [ExcludeFromCodeCoverage]
    private static bool MainModuleIndicatesTestHost()
    {
        try
        {
            using var p = System.Diagnostics.Process.GetCurrentProcess();
            return NameContainsAnyMarker(p.MainModule?.FileName, _testHostModuleMarkers, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            // Access to MainModule can fail in restricted environments; ignore.
            System.Diagnostics.Debug.WriteLine(ex);
            return false;
        }
    }

    /// <summary>
    /// Legacy assembly-name scan fallback (avoids LINQ to reduce allocations).
    /// Checks if any of the loaded assemblies in the current application domain
    /// match the specified assembly markers, using case-insensitive comparison.
    /// Typically used as a fallback mechanism for detecting test environments.
    /// </summary>
    /// <param name="assemblyMarkers">
    /// An array of string markers representing partial or full names of assemblies
    /// to be matched against the loaded assemblies in the application domain.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether at least one of the loaded assemblies
    /// matches an assembly marker. Returns <c>true</c> if a match is found; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>Assembly enumeration is environment-dependent (it reflects which assemblies happen to be loaded) and its try/catch
    /// guards restricted hosts, so it is excluded from coverage; the pure name matching is covered via <see cref="NameContainsAnyMarker"/>.</remarks>
    [ExcludeFromCodeCoverage]
    private static bool LegacyAssemblyScan(string[] assemblyMarkers)
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (var i = 0; i < assemblies.Length; i++)
            {
                if (NameContainsAnyMarker(assemblies[i].FullName, assemblyMarkers, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            // Ignore assembly loading/introspection failures and treat as not found.
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return false;
    }

    /// <summary>Runs the detection probes, returning <see langword="null"/> when an unexpected failure prevents a determination.</summary>
    /// <returns>The detection result, or <see langword="null"/> when detection failed unexpectedly.</returns>
    /// <remarks>Each probe suppresses its own failures, so this top-level guard only triggers on an unexpected rethrow and is excluded from coverage.</remarks>
    [ExcludeFromCodeCoverage]
    private bool? DetectUnitTestRunnerSafely()
    {
        try
        {
            // Prefer explicit, deterministic signals first (opt-in), then common runner environment
            // variables, then process-name heuristics, and finally the legacy assembly scan. The OR
            // chain short-circuits, so the first positive signal wins.
            return IsExplicitTestEnvironment()
                || HasTestRunnerEnvironmentSignals()
                || IsKnownTestHostProcess()
                || LegacyAssemblyScan(_testAssemblyMarkers);
        }
        catch (Exception e)
        {
            this.Log().Debug(e, "Unable to find unit test runner value");
            return null;
        }
    }
}
