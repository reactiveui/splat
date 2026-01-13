// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Provides a default implementation for detecting whether the current process is running within a unit test
/// environment.
/// </summary>
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
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    public bool? InUnitTestRunner()
    {
        try
        {
            // Prefer explicit, deterministic signals first (opt-in).
            if (IsExplicitTestEnvironment())
            {
                return true;
            }

            // Then look for common runner environment variables.
            if (HasTestRunnerEnvironmentSignals())
            {
                return true;
            }

            // Next, fall back to process-name heuristics (no assembly scans).
            if (IsKnownTestHostProcess())
            {
                return true;
            }

            // Worst-case: fall back to legacy assembly scan.
            if (LegacyAssemblyScan(_testAssemblyMarkers))
            {
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            this.Log().Debug(e, "Unable to find unit test runner value");
            return null;
        }
    }

    /// <summary>
    /// Determines if the current environment explicitly indicates a test environment based on specific environment variables.
    /// Explicit env var that users can set in their test setup for deterministic behavior.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the environment explicitly signals a test environment; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    private static bool IsExplicitTestEnvironment()
    {
        try
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
            return value.Length switch
            {
                1 => value == "1",
                3 => value.Equals("yes", StringComparison.OrdinalIgnoreCase),
                4 => value.Equals("true", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Determines if current environment has test runner signals present.
    /// Common signals emitted by popular test runners or their hosts.
    /// </summary>
    /// <returns>
    /// A boolean value indicating whether test runner environment signals are detected.
    /// </returns>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    private static bool HasTestRunnerEnvironmentSignals()
    {
        try
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
            var vars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (DictionaryEntry kv in vars)
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
        }
        catch
        {
            // Ignore enumeration/access issues and treat as not found.
        }

        return false;
    }

    /// <summary>
    /// Determines whether the current process is a known test host process by examining the process name
    /// and other runtime-specific signals.
    /// Last-resort heuristic: detect known test hosts by process name.
    /// </summary>
    /// <returns>
    /// True if the process is identified as a known test host; otherwise, false.
    /// </returns>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    private static bool IsKnownTestHostProcess()
    {
        try
        {
            var procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (!string.IsNullOrEmpty(procName))
            {
                for (var i = 0; i < _knownTestHostProcesses.Length; i++)
                {
#if NETFRAMEWORK
                    if (procName.IndexOf(_knownTestHostProcesses[i], StringComparison.OrdinalIgnoreCase) >= 0)
#else
                    if (procName.Contains(_knownTestHostProcesses[i], StringComparison.OrdinalIgnoreCase))
#endif
                    {
                        return true;
                    }
                }
            }

            // In some environments, the process may be "dotnet" hosting testhost/vstest.
            try
            {
                using var p = System.Diagnostics.Process.GetCurrentProcess();
                var mainModule = p.MainModule?.FileName;
                if (mainModule is not null && !string.IsNullOrEmpty(mainModule))
                {
#if NETFRAMEWORK
                    if (mainModule.IndexOf("testhost", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        mainModule.IndexOf("vstest", StringComparison.OrdinalIgnoreCase) >= 0)
#else
                    if (mainModule.Contains("testhost", StringComparison.OrdinalIgnoreCase) ||
                        mainModule.Contains("vstest", StringComparison.OrdinalIgnoreCase))
#endif
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Access to MainModule can fail in restricted environments; ignore.
            }
        }
        catch
        {
            // Ignore all process inspection failures.
        }

        return false;
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
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Records to the log")]
    private static bool LegacyAssemblyScan(string[] assemblyMarkers)
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (var i = 0; i < assemblies.Length; i++)
            {
                var fullName = assemblies[i].FullName;
                if (string.IsNullOrEmpty(fullName))
                {
                    continue;
                }

                for (var j = 0; j < assemblyMarkers.Length; j++)
                {
#if NETFRAMEWORK
                    if (fullName.IndexOf(assemblyMarkers[j], StringComparison.InvariantCultureIgnoreCase) >= 0)
#else
                    if (fullName.Contains(assemblyMarkers[j], StringComparison.InvariantCultureIgnoreCase))
#endif
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
            // Ignore assembly loading/introspection failures and treat as not found.
        }

        return false;
    }
}
