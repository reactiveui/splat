// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ModeDetection;

/// <summary>
/// The default implementation of the <see cref="Run"/> and <see cref="Test"/> mode.
/// </summary>
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
