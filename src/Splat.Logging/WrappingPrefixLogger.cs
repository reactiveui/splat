// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    private readonly ILogger _inner = inner;
    private readonly string _prefix = $"{callingType?.Name}: ";

    /// <inheritdoc />
    public LogLevel Level => _inner.Level;

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => _inner.Write(_prefix + message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => _inner.Write(exception, _prefix + message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        _inner.Write($"{type.Name}: {message}", type, logLevel);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        _inner.Write(exception, $"{type.Name}: {message}", type, logLevel);
    }
}
