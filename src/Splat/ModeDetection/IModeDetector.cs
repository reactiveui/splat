// Copyright (c) 2026 ReactiveUI. All rights reserved.
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
    /// Determines whether the current process is running within a recognized unit test runner environment.
    /// </summary>
    /// <remarks>This method can be used to alter behavior when running under test conditions, such as
    /// skipping certain operations or enabling test-specific logic. The result may be null if the environment cannot be
    /// reliably detected, so callers should account for this possibility.</remarks>
    /// <returns>true if the process is running under a supported unit test runner; false if it is not; or null if the detection
    /// could not be performed.</returns>
    bool? InUnitTestRunner();
}
