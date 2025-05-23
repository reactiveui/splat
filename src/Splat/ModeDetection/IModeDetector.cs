﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Detects if unit tests or design mode are currently running for the current application or library.
/// </summary>
public interface IModeDetector
{
    /// <summary>
    /// Gets a value indicating whether the current library or application is running through a unit test.
    /// </summary>
    /// <returns>If we are currently running in a unit test.</returns>
    bool? InUnitTestRunner();
}
