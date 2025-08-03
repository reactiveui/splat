// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Splat;

/// <summary>
/// Detects properties about the current platform.
/// </summary>
public interface IPlatformModeDetector
{
    /// <summary>
    /// Gets a value indicating whether the current library or application is running in a GUI design mode tool.
    /// </summary>
    /// <returns>If we are currently running in design mode.</returns>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("Uses Type.GetType and Activator.CreateInstance which may be trimmed")]
    [RequiresDynamicCode("Uses Activator.CreateInstance and reflection which require dynamic code generation")]
#endif
    bool? InDesignMode();
}
