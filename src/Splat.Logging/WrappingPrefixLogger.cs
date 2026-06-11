// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// Provides an ILogger implementation that prefixes all log messages with the name of a specified type, enabling easier
/// identification of log sources.
/// </summary>
/// <remarks>This logger is useful for scenarios where log output should be clearly associated with a particular
/// class or component. All log messages written through this logger are automatically prefixed with the name of the
/// provided type, followed by a colon and a space. This can help distinguish log entries in applications with multiple
/// components sharing a common logging infrastructure.</remarks>
/// <param name="inner">The underlying ILogger instance to which log messages are forwarded. Cannot be null.</param>
/// <param name="callingType">The type whose name is used as a prefix for all log messages. Cannot be null.</param>
public class WrappingPrefixLogger(ILogger inner, Type callingType) : ILogger
{
    /// <summary>The calling type's name prepended to every log message.</summary>
    private readonly string _prefix = $"{callingType?.Name}: ";

    /// <inheritdoc />
    public LogLevel Level => inner.Level;

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => inner.Write(_prefix + message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => inner.Write(exception, _prefix + message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        inner.Write($"{type.Name}: {message}", type, logLevel);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        inner.Write(exception, $"{type.Name}: {message}", type, logLevel);
    }
}
