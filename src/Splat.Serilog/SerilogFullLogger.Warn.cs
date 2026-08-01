// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>Contains the Warn log level methods for the <see cref="SerilogFullLogger"/> class.</summary>
public partial class SerilogFullLogger
{
    /// <inheritdoc />
    public void Warn<T>(T value) => _logger.Warning(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>(IFormatProvider formatProvider, T value) =>
#if NET8_0_OR_GREATER
        _logger.Warning(MessageTemplate, string.Format(formatProvider, _valueCompositeFormat, value));
#else
        _logger.Warning(MessageTemplate, string.Format(formatProvider, "{0}", value));
#endif

    /// <inheritdoc />
    public void Warn(Exception exception, string? message) => _logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) =>
        _logger.Warning(MessageTemplate, string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Warn([Localizable(false)] string? message) => _logger.Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn([Localizable(false)] string message, params object[] args) => _logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Warning(message, args);

    /// <inheritdoc />
    public void Warn(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warning(function.Invoke());
    }

    /// <inheritdoc />
    public void Warn<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.ForContext<T>().Warning(function.Invoke());
    }

    /// <inheritdoc />
    public void Warn(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warning(exception, function.Invoke());
    }

    /// <inheritdoc />
    public void WarnException([Localizable(false)] string? message, Exception exception) => _logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void WarnException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warning(exception, function.Invoke());
    }
}
