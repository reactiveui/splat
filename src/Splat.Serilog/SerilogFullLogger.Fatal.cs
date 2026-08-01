// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>Contains the Fatal log level methods for the <see cref="SerilogFullLogger"/> class.</summary>
public partial class SerilogFullLogger
{
    /// <inheritdoc />
    public void Fatal<T>(T value) => _logger.Fatal(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>(IFormatProvider formatProvider, T value) =>
#if NET8_0_OR_GREATER
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, _valueCompositeFormat, value));
#else
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, "{0}", value));
#endif

    /// <inheritdoc />
    public void Fatal(Exception exception, string? message) => _logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) =>
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string? message) => _logger.Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string message, params object[] args) => _logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Fatal(message, args);

    /// <inheritdoc />
    public void Fatal(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsFatalEnabled)
        {
            return;
        }

        _logger.Fatal(function.Invoke());
    }

    /// <inheritdoc />
    public void Fatal<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsFatalEnabled)
        {
            return;
        }

        _logger.ForContext<T>().Fatal(function.Invoke());
    }

    /// <inheritdoc />
    public void Fatal(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsFatalEnabled)
        {
            return;
        }

        _logger.Fatal(exception, function.Invoke());
    }

    /// <inheritdoc />
    public void FatalException([Localizable(false)] string? message, Exception exception) => _logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void FatalException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsFatalEnabled)
        {
            return;
        }

        _logger.Fatal(exception, function.Invoke());
    }
}
