// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using Serilog.Events;

namespace Splat;

/// <summary>
/// A full wrapping logger over Serilog.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SerilogFullLogger"/> class.
/// </remarks>
/// <param name="logger">The logger we are wrapping.</param>
public class SerilogFullLogger(global::Serilog.ILogger logger) : IFullLogger
{
    private readonly global::Serilog.ILogger _logger = logger;

    /// <inheritdoc />
    public bool IsDebugEnabled => _logger.IsEnabled(LogEventLevel.Debug);

    /// <inheritdoc />
    public bool IsInfoEnabled => _logger.IsEnabled(LogEventLevel.Information);

    /// <inheritdoc />
    public bool IsWarnEnabled => _logger.IsEnabled(LogEventLevel.Warning);

    /// <inheritdoc />
    public bool IsErrorEnabled => _logger.IsEnabled(LogEventLevel.Error);

    /// <inheritdoc />
    public bool IsFatalEnabled => _logger.IsEnabled(LogEventLevel.Fatal);

    /// <inheritdoc />
    public LogLevel Level
    {
        get
        {
            foreach (var mapping in SerilogHelper.Mappings)
            {
                if (_logger.IsEnabled(mapping.Value))
                {
                    return mapping.Key;
                }
            }

            // Default to Fatal, it should always be enabled anyway.
            return LogLevel.Fatal;
        }

        set
        {
            // Do nothing. set is going soon anyway.
        }
    }

    /// <inheritdoc />
    public void Debug<T>(T value) => _logger.Debug(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>(IFormatProvider formatProvider, T value) => _logger.Debug(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => _logger.Debug(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Debug([Localizable(false)] string? message) => _logger.Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug([Localizable(false)] string message, params object[] args) => _logger.Debug(message, args);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Debug(message, args);

    /// <inheritdoc />
    public void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => _logger.Debug(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Debug<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Debug(message, args);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => _logger.Debug(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Debug(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Debug(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Debug(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void DebugException([Localizable(false)] string? message, Exception exception) => _logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Debug(Exception exception, [Localizable(false)] string? message) => _logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>(T value) => _logger.Error(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>(IFormatProvider formatProvider, T value) => _logger.Error(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Error(Exception exception, string? message) => _logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => _logger.Error(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Error([Localizable(false)] string? message) => _logger.Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error([Localizable(false)] string message, params object[] args) => _logger.Error(message, args);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Error(message, args);

    /// <inheritdoc />
    public void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => _logger.Error(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Error<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Error(message, args);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => _logger.Error(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Error(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Error(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Error(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void ErrorException([Localizable(false)] string? message, Exception exception) => _logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>(T value) => _logger.Fatal(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>(IFormatProvider formatProvider, T value) => _logger.Fatal(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Fatal(Exception exception, string? message) => _logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => _logger.Fatal(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string? message) => _logger.Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string message, params object[] args) => _logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => _logger.Fatal(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Fatal<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => _logger.Fatal(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Fatal(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Fatal(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Fatal(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void FatalException([Localizable(false)] string? message, Exception exception) => _logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>(T value) => _logger.Information(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>(IFormatProvider formatProvider, T value) => _logger.Information(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Info(Exception exception, string? message) => _logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => _logger.Information(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Info([Localizable(false)] string? message) => _logger.Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info([Localizable(false)] string message, params object[] args) => _logger.Information(message, args);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Information(message, args);

    /// <inheritdoc />
    public void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => _logger.Information(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Info<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Information(message, args);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => _logger.Information(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Information(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Information(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Information(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void InfoException([Localizable(false)] string? message, Exception exception) => _logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>(T value) => _logger.Warning(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>(IFormatProvider formatProvider, T value) => _logger.Warning(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Warn(Exception exception, string? message) => _logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => _logger.Warning(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Warn([Localizable(false)] string? message) => _logger.Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn([Localizable(false)] string message, params object[] args) => _logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Warning(message, args);

    /// <inheritdoc />
    public void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => _logger.Warning(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Warn<TArgument>([Localizable(false)] string message, TArgument args) => _logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => _logger.Warning(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Warning(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Warning(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Warning(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void WarnException([Localizable(false)] string? message, Exception exception) => _logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => _logger.Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => _logger.Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) => _logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => _logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);

    /// <inheritdoc/>
    public void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument) => _logger.Debug(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Debug(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(Exception exception, string messageFormat, TArgument argument) => _logger.Information(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Information(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Information(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(Exception exception, string messageFormat, TArgument argument) => _logger.Warning(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Warning(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument) => _logger.Error(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Error(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Error(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument) => _logger.Fatal(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Fatal(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
}
