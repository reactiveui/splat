// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// Provides an ILogger implementation that prefixes log messages with their log level before delegating to an inner
/// logger.
/// </summary>
/// <remarks>This logger wraps another ILogger and automatically prepends the log level to each message. It is
/// useful for scenarios where the inner logger does not include log level information in its output. All logging
/// operations are delegated to the specified inner logger.</remarks>
/// <param name="inner">The underlying ILogger instance to which log messages are forwarded. Cannot be null.</param>
public class WrappingLogLevelLogger(ILogger inner) : ILogger
{
    /// <inheritdoc />
    public LogLevel Level => inner.Level;

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => inner.Write($"{logLevel}: {message}", logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => inner.Write(exception, $"{logLevel}: {message}", logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write($"{logLevel}: {message}", type, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => inner.Write(exception, $"{logLevel}: {message}", type, logLevel);
}
