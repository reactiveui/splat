// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Detects various properties about a platform.
/// </summary>
public static class PlatformModeDetector
{
    private static bool? _cachedInDesignModeResult;

    /// <summary>
    /// Initializes static members of the <see cref="PlatformModeDetector"/> class.
    /// </summary>
    static PlatformModeDetector() => Current = new DefaultPlatformModeDetector();

    /// <summary>
    /// Gets or sets the current mode detector set.
    /// </summary>
    private static IPlatformModeDetector Current { get; set; }

    /// <summary>
    /// Overrides the mode detector with one of your own provided ones.
    /// </summary>
    /// <param name="modeDetector">The mode detector to use.</param>
    public static void OverrideModeDetector(IPlatformModeDetector modeDetector)
    {
        Current = modeDetector;
        _cachedInDesignModeResult = null;
    }

    /// <summary>
    /// Gets a value indicating whether we are currently running from within a GUI design editor.
    /// </summary>
    /// <returns>If we are currently running from design mode.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("Uses Activator.CreateInstance and reflection which require dynamic code generation")]
    [RequiresUnreferencedCode("Uses Type.GetType and Activator.CreateInstance which may be trimmed")]
#endif
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
}
