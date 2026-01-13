// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Splat;

/// <summary>
/// Provides a mechanism for determining whether the current process is running within a graphical user interface (GUI)
/// design environment.
/// </summary>
/// <remarks>Implementations of this interface can be used to adapt application behavior when running in design
/// mode, such as within a visual designer or IDE. This is useful for components or controls that need to distinguish
/// between design-time and run-time execution.</remarks>
public interface IPlatformModeDetector
{
    /// <summary>
    /// Gets a value indicating whether the current library or application is running in a GUI design mode tool.
    /// </summary>
    /// <returns>If we are currently running in design mode.</returns>
    bool? InDesignMode();
}
