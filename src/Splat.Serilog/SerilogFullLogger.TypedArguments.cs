// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>The typed-argument overloads, kept apart from the rest of the type because the set is
/// dictated by the logging contract: one overload per argument count, each forwarding the arguments
/// on without boxing them.</summary>
public partial class SerilogFullLogger
{
    /// <inheritdoc />
    public void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) =>
        _logger.Debug(MessageTemplate, string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Debug<TArgument>([Localizable(false)] string messageFormat, TArgument argument) => _logger.Debug(messageFormat, argument);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Debug(MessageTemplate, string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) =>
        _logger.Debug(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Debug(MessageTemplate, string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument) =>
        _logger.Debug(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Debug(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
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
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

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
    public void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) =>
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Fatal<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) =>
        _logger.Fatal(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Fatal(MessageTemplate, string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument) =>
        _logger.Fatal(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
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
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) =>
        _logger.Information(MessageTemplate, string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Info<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Information(message, args);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Information(MessageTemplate, string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) =>
        _logger.Information(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Information(MessageTemplate, string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Information(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(Exception exception, string messageFormat, TArgument argument) =>
        _logger.Information(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Information(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Information(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
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
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) =>
        _logger.Warning(MessageTemplate, string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Warn<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Warning(MessageTemplate, string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) =>
        _logger.Warning(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Warning(MessageTemplate, string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        [Localizable(false)] string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8) =>
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(Exception exception, string messageFormat, TArgument argument) =>
        _logger.Warning(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2) =>
        _logger.Warning(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3) =>
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4) =>
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5) =>
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6) =>
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        Exception exception,
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7) =>
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
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
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
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
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
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
        _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
}
