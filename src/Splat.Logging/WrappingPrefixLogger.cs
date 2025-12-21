// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// A prefix logger which wraps a <see cref="ILogger"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WrappingPrefixLogger"/> class.
/// Placeholder.
/// </remarks>
/// <param name="inner">The <see cref="ILogger"/> to wrap in this class.</param>
/// <param name="callingType">The type which will be calling this logger.</param>
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
