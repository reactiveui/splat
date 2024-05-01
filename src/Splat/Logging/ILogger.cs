// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// Represents a log target where messages can be written to.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Gets the level at which the target will emit messages.
    /// </summary>
    LogLevel Level { get; }

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Write([Localizable(false)] string message, LogLevel logLevel);

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel);

    /// <summary>
    /// Writes a messge to the target.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="type">The type.</param>
    /// <param name="logLevel">The log level.</param>
    void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel);

    /// <summary>
    /// Writes a messge to the target.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    /// <param name="message">The message.</param>
    /// <param name="type">The type.</param>
    /// <param name="logLevel">The log level.</param>
    void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel);
}
