// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ModeDetection;

/// <summary>
/// Represents an application mode that indicates whether the code is running under a unit test runner or in normal
/// execution.
/// </summary>
/// <remarks>Use the predefined <see cref="Run"/> and <see cref="Test"/> instances to distinguish between standard
/// execution and test environments. This type is typically used to alter behavior based on the current execution
/// context, such as enabling test-specific logic or bypassing certain runtime checks during automated
/// testing.</remarks>
public sealed class Mode : IModeDetector
{
    /// <summary>
    /// The default implementation of the run mode.
    /// </summary>
    public static readonly Mode Run = new(false);

    /// <summary>
    /// The default implementation of the test mode.
    /// </summary>
    public static readonly Mode Test = new(true);

    private readonly bool _inUnitTestRunner;

    private Mode(bool inUnitTestRunner) => _inUnitTestRunner = inUnitTestRunner;

    /// <inheritdoc/>
    public bool? InUnitTestRunner() => _inUnitTestRunner;
}
