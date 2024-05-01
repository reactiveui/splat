// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat.Serilog;

/// <summary>
/// Provides a legacy interface for registrations that was compatible with the old
/// Serilog interfaces provided by Joel Weiss.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class Registration
{
    /// <summary>
    /// Registers a serilog logger with Splat.
    /// </summary>
    /// <param name="logger">The logger to register.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This method will be removed in the future, Use Splat.Locator.CurrentMutable.UseSerilogWithWrappingFullLogger() instead.")]
    public static void Register(global::Serilog.ILogger logger) => Locator.CurrentMutable.UseSerilogFullLogger(logger);
}
