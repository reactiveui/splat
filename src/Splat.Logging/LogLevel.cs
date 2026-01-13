// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Specifies the minimum severity level of a log message.
/// </summary>
/// <remarks>Use this enumeration to indicate the importance or urgency of log entries. Higher values represent
/// more severe conditions, such as errors or fatal failures, while lower values are used for informational or debugging
/// messages. The specific meaning and handling of each level may vary depending on the logging framework or application
/// configuration.</remarks>
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
