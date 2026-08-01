// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>Contains the Error log level methods for the <see cref="SerilogFullLogger"/> class.</summary>
public partial class SerilogFullLogger
{
    /// <inheritdoc />
    public void Error<T>(T value) => _logger.Error(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>(IFormatProvider formatProvider, T value) =>
#if NET8_0_OR_GREATER
        _logger.Error(MessageTemplate, string.Format(formatProvider, _valueCompositeFormat, value));
#else
        _logger.Error(MessageTemplate, string.Format(formatProvider, "{0}", value));
#endif

    /// <inheritdoc />
    public void Error(Exception exception, string? message) => _logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) =>
        _logger.Error(MessageTemplate, string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Error([Localizable(false)] string? message) => _logger.Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error([Localizable(false)] string message, params object[] args) => _logger.Error(message, args);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Error(message, args);

    /// <inheritdoc />
    public void Error(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsErrorEnabled)
        {
            return;
        }

        _logger.Error(function.Invoke());
    }

    /// <inheritdoc />
    public void Error<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsErrorEnabled)
        {
            return;
        }

        _logger.ForContext<T>().Error(function.Invoke());
    }

    /// <inheritdoc />
    public void Error(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsErrorEnabled)
        {
            return;
        }

        _logger.Error(exception, function.Invoke());
    }

    /// <inheritdoc />
    public void ErrorException([Localizable(false)] string? message, Exception exception) => _logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void ErrorException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsErrorEnabled)
        {
            return;
        }

        _logger.Error(exception, function.Invoke());
    }
}
