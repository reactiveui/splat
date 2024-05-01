// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
    bool? InDesignMode();
}
