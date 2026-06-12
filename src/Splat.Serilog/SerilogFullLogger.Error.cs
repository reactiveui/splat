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
        _logger.Error(MessageTemplate, string.Format(formatProvider, "{0}", value));

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
    public void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) =>
        _logger.Error(MessageTemplate, string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Error<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Error(message, args);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Error(MessageTemplate, string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) =>
        _logger.Error(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Error(MessageTemplate, string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Error(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10) =>
        _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

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

    /// <inheritdoc/>
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument) =>
        _logger.Error(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Error(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10) =>
        _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

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
