// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides methods to detect whether the application is running in design mode, such as within a GUI design editor.
/// Allows overriding the platform mode detection logic for customization or testing purposes.
/// </summary>
/// <remarks>This class is intended for use in scenarios where it is necessary to distinguish between design-time
/// and run-time environments, such as when developing custom controls or components. The detection logic can be
/// overridden to support different platforms or testing requirements. All members are thread-safe for typical usage
/// patterns.</remarks>
public static class PlatformModeDetector
{
    /// <summary>Memoizes the design-mode detection result; <see langword="null"/> until first computed.</summary>
    private static bool? _cachedInDesignModeResult;

    /// <summary>Initializes static members of the <see cref="PlatformModeDetector"/> class.</summary>
    /// <remarks>This static constructor assigns a default implementation to the Current property. It is
    /// invoked automatically before any static members are accessed or any instances are created.</remarks>
    static PlatformModeDetector() => Current = new DefaultPlatformModeDetector();

    /// <summary>Gets or sets the current mode detector set.</summary>
    private static IPlatformModeDetector Current { get; set; }

    /// <summary>Overrides the mode detector with one of your own provided ones.</summary>
    /// <param name="modeDetector">The mode detector to use.</param>
    public static void OverrideModeDetector(IPlatformModeDetector modeDetector)
    {
        Current = modeDetector;
        _cachedInDesignModeResult = null;
    }

    /// <summary>Gets a value indicating whether we are currently running from within a GUI design editor.</summary>
    /// <returns>If we are currently running from design mode.</returns>
    public static bool InDesignMode()
    {
        if (_cachedInDesignModeResult.HasValue)
        {
            return _cachedInDesignModeResult.Value;
        }

        if (Current is not null)
        {
            _cachedInDesignModeResult = Current.InDesignMode();
            if (_cachedInDesignModeResult.HasValue)
            {
                return _cachedInDesignModeResult.Value;
            }
        }

        return _cachedInDesignModeResult.GetValueOrDefault();
    }

    /// <summary>Gets the current state for test isolation. Used by test scopes.</summary>
    /// <returns>A tuple containing the current detector, the cached result, and the default detector's own cached result.</returns>
    /// <remarks><see cref="DefaultPlatformModeDetector"/> memoizes separately, so its cache has to travel with this
    /// state; otherwise a test that leaves a design-mode value behind there leaks into every later test.</remarks>
    internal static (IPlatformModeDetector detector, bool? cachedResult, bool? defaultDetectorCachedResult) GetState() =>
        (Current, _cachedInDesignModeResult, DefaultPlatformModeDetector.GetState());

    /// <summary>Restores the state for test isolation. Used by test scopes.</summary>
    /// <param name="state">The state to restore.</param>
    internal static void RestoreState((IPlatformModeDetector detector, bool? cachedResult, bool? defaultDetectorCachedResult) state)
    {
        Current = state.detector;
        _cachedInDesignModeResult = state.cachedResult;
        DefaultPlatformModeDetector.RestoreState(state.defaultDetectorCachedResult);
    }

    /// <summary>Resets the state to default for test isolation. Used by test scopes.</summary>
    internal static void ResetState()
    {
        Current = new DefaultPlatformModeDetector();
        _cachedInDesignModeResult = null;
        DefaultPlatformModeDetector.ResetState();
    }
}
