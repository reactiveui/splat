// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// Defines a contract for logging messages with varying severity levels and contextual information.
/// </summary>
/// <remarks>Implementations of this interface allow applications to record diagnostic or operational messages,
/// optionally including exception details and type context. The interface supports logging at different levels of
/// severity, enabling filtering and categorization of log output. Thread safety and message formatting behavior depend
/// on the specific implementation.</remarks>
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
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel);

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="type">The type associated with the log message.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel);

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="type">The type associated with the log message.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel);
}
