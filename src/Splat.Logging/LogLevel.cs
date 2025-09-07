// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents the minimum log level a <see cref="ILogger"/> will start emitting from.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "Existing API")]
public enum LogLevel
{
    /// <summary>
    /// The log message is for debugging purposes.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// The log message is for information purposes.
    /// </summary>
    Info,

    /// <summary>
    /// The log message is for warning purposes.
    /// </summary>
    Warn,

    /// <summary>
    /// The log message is for error purposes.
    /// </summary>
    Error,

    /// <summary>
    /// The log message is for fatal purposes.
    /// </summary>
    Fatal,
}
