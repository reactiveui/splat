// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides static methods for detecting the current execution mode, such as whether the code is running under a unit
/// test runner. Allows overriding the mode detection logic for testing or customization purposes.
/// </summary>
/// <remarks>This class is intended for scenarios where code behavior needs to adapt based on the execution
/// environment, such as distinguishing between normal application runs and unit test execution. The mode detection
/// logic can be customized by supplying an alternative implementation of the mode detector interface using the
/// OverrideModeDetector method. Thread safety is not guaranteed; callers should ensure appropriate synchronization if
/// accessing from multiple threads.</remarks>
public static class ModeDetector
{
    private static bool? _cachedInUnitTestRunnerResult;

    /// <summary>
    /// Initializes static members of the <see cref="ModeDetector"/> class and sets the default mode detector implementation.
    /// </summary>
    /// <remarks>This static constructor ensures that the Current property is assigned a default
    /// implementation before any static members are accessed. This guarantees that mode detection functionality is
    /// available throughout the application's lifetime.</remarks>
    static ModeDetector() => Current = new DefaultModeDetector();

    /// <summary>
    /// Gets or sets the current mode detector instance used by the application.
    /// </summary>
    private static IModeDetector Current { get; set; }

    /// <summary>
    /// Overrides the current mode detector with the specified implementation.
    /// </summary>
    /// <remarks>This method is typically used for testing or to customize mode detection behavior at runtime.
    /// Calling this method replaces the global mode detector instance and clears any cached detection
    /// results.</remarks>
    /// <param name="modeDetector">The mode detector to use as the current implementation. Cannot be null.</param>
    public static void OverrideModeDetector(IModeDetector modeDetector)
    {
        Current = modeDetector;
        _cachedInUnitTestRunnerResult = null;
    }

    /// <summary>
    /// Determines whether the current process is running within a unit test runner environment.
    /// </summary>
    /// <remarks>This method attempts to detect common unit test runner environments, but may return false if
    /// detection is not possible on the current platform or with certain test frameworks. The result is cached for
    /// subsequent calls.</remarks>
    /// <returns>true if the current process is detected to be running under a unit test runner; otherwise, false.</returns>
    public static bool InUnitTestRunner()
    {
        if (_cachedInUnitTestRunnerResult.HasValue)
        {
            return _cachedInUnitTestRunnerResult.Value;
        }

        if (Current is not null)
        {
            _cachedInUnitTestRunnerResult = Current.InUnitTestRunner();
            if (_cachedInUnitTestRunnerResult.HasValue)
            {
                return _cachedInUnitTestRunnerResult.Value;
            }
        }

        // We have no sane platform-independent way to detect a unit test
        // runner :-/
        return false;
    }

    /// <summary>
    /// Retrieves the current mode detector instance and the cached result indicating whether the code is running within
    /// a unit test runner.
    /// </summary>
    /// <returns>A tuple containing the current <see cref="IModeDetector"/> instance and a nullable Boolean value representing
    /// the cached result. The Boolean is <see langword="true"/> if running in a unit test runner, <see
    /// langword="false"/> if not, or <see langword="null"/> if the result is not cached.</returns>
    internal static (IModeDetector detector, bool? cachedResult) GetState() =>
        (Current, _cachedInUnitTestRunnerResult);

    /// <summary>
    /// Restores the mode detector state and its cached result from the specified tuple.
    /// </summary>
    /// <param name="state">A tuple containing the mode detector to restore and an optional cached result value. The first item is the
    /// detector instance; the second item is the cached result, or null if no cached value is available.</param>
    internal static void RestoreState((IModeDetector detector, bool? cachedResult) state)
    {
        Current = state.detector;
        _cachedInUnitTestRunnerResult = state.cachedResult;
    }

    /// <summary>
    /// Resets the internal state of the mode detector to its default configuration.
    /// </summary>
    /// <remarks>This method is intended for internal use only. It reinitializes static fields to their
    /// default values, which may affect subsequent mode detection operations. Calling this method may impact the
    /// behavior of components that rely on the current mode detector state.</remarks>
    internal static void ResetState()
    {
        Current = new DefaultModeDetector();
        _cachedInUnitTestRunnerResult = null;
    }
}
