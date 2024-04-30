// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// A helper class which detect if we are currently running via a unit test or design mode.
/// </summary>
public static class ModeDetector
{
    private static bool? _cachedInUnitTestRunnerResult;

    /// <summary>
    /// Initializes static members of the <see cref="ModeDetector"/> class.
    /// </summary>
    static ModeDetector() => Current = new DefaultModeDetector();

    /// <summary>
    /// Gets or sets the current mode detector set.
    /// </summary>
    private static IModeDetector Current { get; set; }

    /// <summary>
    /// Overrides the mode detector with one of your own provided ones.
    /// </summary>
    /// <param name="modeDetector">The mode detector to use.</param>
    public static void OverrideModeDetector(IModeDetector modeDetector)
    {
        Current = modeDetector;
        _cachedInUnitTestRunnerResult = null;
    }

    /// <summary>
    /// Gets a value indicating whether we are currently running from a unit test.
    /// </summary>
    /// <returns>If we are currently running from a unit test.</returns>
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
}
