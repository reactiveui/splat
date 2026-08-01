// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>Contains the Info log level methods for the <see cref="SerilogFullLogger"/> class.</summary>
public partial class SerilogFullLogger
{
    /// <inheritdoc />
    public void Info<T>(T value) => _logger.Information(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>(IFormatProvider formatProvider, T value) =>
#if NET8_0_OR_GREATER
        _logger.Information(MessageTemplate, string.Format(formatProvider, _valueCompositeFormat, value));
#else
        _logger.Information(MessageTemplate, string.Format(formatProvider, "{0}", value));
#endif

    /// <inheritdoc />
    public void Info(Exception exception, string? message) => _logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) =>
        _logger.Information(MessageTemplate, string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Info([Localizable(false)] string? message) => _logger.Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info([Localizable(false)] string message, params object[] args) => _logger.Information(message, args);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Information(message, args);

    /// <inheritdoc />
    public void Info(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Information(function.Invoke());
    }

    /// <inheritdoc />
    public void Info<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.ForContext<T>().Information(function.Invoke());
    }

    /// <inheritdoc />
    public void Info(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Information(exception, function.Invoke());
    }

    /// <inheritdoc />
    public void InfoException([Localizable(false)] string? message, Exception exception) => _logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void InfoException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Information(exception, function.Invoke());
    }
}
