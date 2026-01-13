// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

using Serilog.Events;

namespace Splat;

/// <summary>
/// Provides a full-featured logger implementation that writes log events to Serilog using the IFullLogger interface.
/// </summary>
/// <remarks>This class adapts the IFullLogger interface to Serilog, enabling structured logging and support for
/// multiple log levels, message formatting, and exception logging. All log methods delegate to the underlying
/// Serilog.ILogger instance. Thread safety and configuration are determined by the provided Serilog logger.</remarks>
/// <param name="logger">The Serilog logger instance used to write log events. Cannot be null.</param>
public class SerilogFullLogger(global::Serilog.ILogger logger) : IFullLogger
{
    /// <inheritdoc />
    public bool IsDebugEnabled => logger.IsEnabled(LogEventLevel.Debug);

    /// <inheritdoc />
    public bool IsInfoEnabled => logger.IsEnabled(LogEventLevel.Information);

    /// <inheritdoc />
    public bool IsWarnEnabled => logger.IsEnabled(LogEventLevel.Warning);

    /// <inheritdoc />
    public bool IsErrorEnabled => logger.IsEnabled(LogEventLevel.Error);

    /// <inheritdoc />
    public bool IsFatalEnabled => logger.IsEnabled(LogEventLevel.Fatal);

    /// <inheritdoc />
    public LogLevel Level
    {
        get
        {
            foreach (var mapping in SerilogHelper.Mappings)
            {
                if (logger.IsEnabled(mapping.Value))
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
    public void Debug<T>(T value) => logger.Debug(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>(IFormatProvider formatProvider, T value) => logger.Debug(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => logger.Debug(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Debug([Localizable(false)] string? message) => logger.Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string? message) => logger.ForContext<T>().Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug([Localizable(false)] string message, params object[] args) => logger.Debug(message, args);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string message, params object[] args) => logger.ForContext<T>().Debug(message, args);

    /// <inheritdoc />
    public void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => logger.Debug(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Debug<TArgument>([Localizable(false)] string message, TArgument args) => logger.Debug(message, args);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => logger.Debug(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Debug(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Debug(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Debug(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Debug(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void DebugException([Localizable(false)] string? message, Exception exception) => logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Debug(Exception exception, [Localizable(false)] string? message) => logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Debug(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            logger.Debug(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Debug<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            logger.ForContext<T>().Debug(function.Invoke());
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void DebugException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            logger.Debug(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Debug(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            logger.Debug(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Error<T>(T value) => logger.Error(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>(IFormatProvider formatProvider, T value) => logger.Error(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Error(Exception exception, string? message) => logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => logger.Error(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Error([Localizable(false)] string? message) => logger.Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string? message) => logger.ForContext<T>().Error(message ?? string.Empty);

    /// <inheritdoc />
    public void Error([Localizable(false)] string message, params object[] args) => logger.Error(message, args);

    /// <inheritdoc />
    public void Error<T>([Localizable(false)] string message, params object[] args) => logger.ForContext<T>().Error(message, args);

    /// <inheritdoc />
    public void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => logger.Error(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Error<TArgument>([Localizable(false)] string message, TArgument args) => logger.Error(message, args);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => logger.Error(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Error(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Error(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Error(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Error(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Error(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void ErrorException([Localizable(false)] string? message, Exception exception) => logger.Error(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Error(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            logger.Error(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Error<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            logger.ForContext<T>().Error(function.Invoke());
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void ErrorException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            logger.Error(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Error(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            logger.Error(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Fatal<T>(T value) => logger.Fatal(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>(IFormatProvider formatProvider, T value) => logger.Fatal(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Fatal(Exception exception, string? message) => logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => logger.Fatal(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string? message) => logger.Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string? message) => logger.ForContext<T>().Fatal(message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal([Localizable(false)] string message, params object[] args) => logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<T>([Localizable(false)] string message, params object[] args) => logger.ForContext<T>().Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => logger.Fatal(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Fatal<TArgument>([Localizable(false)] string message, TArgument args) => logger.Fatal(message, args);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => logger.Fatal(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Fatal(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Fatal(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Fatal(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Fatal(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void FatalException([Localizable(false)] string? message, Exception exception) => logger.Fatal(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            logger.Fatal(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Fatal<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            logger.ForContext<T>().Fatal(function.Invoke());
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void FatalException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            logger.Fatal(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Fatal(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            logger.Fatal(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Info<T>(T value) => logger.Information(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>(IFormatProvider formatProvider, T value) => logger.Information(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Info(Exception exception, string? message) => logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => logger.Information(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Info([Localizable(false)] string? message) => logger.Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string? message) => logger.ForContext<T>().Information(message ?? string.Empty);

    /// <inheritdoc />
    public void Info([Localizable(false)] string message, params object[] args) => logger.Information(message, args);

    /// <inheritdoc />
    public void Info<T>([Localizable(false)] string message, params object[] args) => logger.ForContext<T>().Information(message, args);

    /// <inheritdoc />
    public void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => logger.Information(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Info<TArgument>([Localizable(false)] string message, TArgument args) => logger.Information(message, args);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => logger.Information(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Information(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Information(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Information(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Information(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Information(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void InfoException([Localizable(false)] string? message, Exception exception) => logger.Information(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Info(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            logger.Information(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Info<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            logger.ForContext<T>().Information(function.Invoke());
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void InfoException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            logger.Information(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Info(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            logger.Information(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Warn<T>(T value) => logger.Warning(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>(IFormatProvider formatProvider, T value) => logger.Warning(string.Format(formatProvider, "{0}", value));

    /// <inheritdoc />
    public void Warn(Exception exception, string? message) => logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) => logger.Warning(string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Warn([Localizable(false)] string? message) => logger.Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string? message) => logger.ForContext<T>().Warning(message ?? string.Empty);

    /// <inheritdoc />
    public void Warn([Localizable(false)] string message, params object[] args) => logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<T>([Localizable(false)] string message, params object[] args) => logger.ForContext<T>().Warning(message, args);

    /// <inheritdoc />
    public void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument) => logger.Warning(string.Format(formatProvider, message, argument));

    /// <inheritdoc />
    public void Warn<TArgument>([Localizable(false)] string message, TArgument args) => logger.Warning(message, args);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2) => logger.Warning(string.Format(formatProvider, message, argument1, argument2));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Warning(messageFormat, argument1, argument2);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Warning(string.Format(formatProvider, message, argument1, argument2, argument3));

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Warning(messageFormat, argument1, argument2, argument3);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Warning(messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc />
    public void WarnException([Localizable(false)] string? message, Exception exception) => logger.Warning(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            logger.Warning(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Warn<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            logger.ForContext<T>().Warning(function.Invoke());
        }
    }

    /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
    public void WarnException(Func<string> function, Exception exception)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            logger.Warning(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Warn(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            logger.Warning(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => logger.Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => logger.Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) => logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);

    /// <inheritdoc/>
    public void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument) => logger.Debug(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Debug(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Debug(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(Exception exception, string messageFormat, TArgument argument) => logger.Information(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Information(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Information(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Information(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(Exception exception, string messageFormat, TArgument argument) => logger.Warning(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Warning(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Warning(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Warning(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument) => logger.Error(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Error(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Error(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument) => logger.Fatal(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => logger.Fatal(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => logger.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
}
