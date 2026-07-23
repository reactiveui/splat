// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Serilog.Events;

namespace Splat;

/// <summary>Provides a full-featured logger implementation that writes log events to Serilog using the IFullLogger interface.</summary>
/// <remarks>This class adapts the IFullLogger interface to Serilog, enabling structured logging and support for
/// multiple log levels, message formatting, and exception logging. All log methods delegate to the underlying
/// Serilog.ILogger instance. Thread safety and configuration are determined by the provided Serilog logger.</remarks>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "The generic type parameter is the caller-supplied calling type used only to scope the log entry; it intentionally has no corresponding method parameter and cannot be inferred.")]
public partial class SerilogFullLogger : IFullLogger
{
    /// <summary>
    /// Serilog message template used to forward an already-rendered message as a single property. The <c>:l</c>
    /// (literal) specifier renders the string value without the surrounding quotes Serilog adds to string
    /// properties by default, so the emitted text matches the message that was passed in.
    /// </summary>
    private const string MessageTemplate = "{Message:l}";

#if NET8_0_OR_GREATER
    /// <summary>The parsed composite format used to render a single value with a caller-supplied format provider.</summary>
    private static readonly System.Text.CompositeFormat _valueCompositeFormat = System.Text.CompositeFormat.Parse("{0}");
#endif

    /// <summary>The underlying Serilog logger that messages are forwarded to.</summary>
    private readonly global::Serilog.ILogger _logger;

    /// <summary>Initializes a new instance of the <see cref="SerilogFullLogger"/> class.</summary>
    /// <param name="logger">The Serilog logger instance used to write log events. Cannot be null.</param>
    public SerilogFullLogger(global::Serilog.ILogger logger) => _logger = logger;

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

        // Do nothing. set is going soon anyway.
        set => _ = value;
    }

    /// <inheritdoc />
    public void Debug<T>(T value) => _logger.Debug(value?.ToString() ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>(IFormatProvider formatProvider, T value) =>
#if NET8_0_OR_GREATER
        _logger.Debug(MessageTemplate, string.Format(formatProvider, _valueCompositeFormat, value));
#else
        _logger.Debug(MessageTemplate, string.Format(formatProvider, "{0}", value));
#endif

    /// <inheritdoc />
    public void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args) =>
        _logger.Debug(MessageTemplate, string.Format(formatProvider, message, args));

    /// <inheritdoc />
    public void Debug([Localizable(false)] string? message) => _logger.Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string? message) => _logger.ForContext<T>().Debug(message ?? string.Empty);

    /// <inheritdoc />
    public void Debug([Localizable(false)] string message, params object[] args) => _logger.Debug(message, args);

    /// <inheritdoc />
    public void Debug<T>([Localizable(false)] string message, params object[] args) => _logger.ForContext<T>().Debug(message, args);

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

    /// <inheritdoc />
    public void Debug(Exception exception, [Localizable(false)] string? message) => _logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void Debug(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsDebugEnabled)
        {
            return;
        }

        _logger.Debug(function.Invoke());
    }

    /// <inheritdoc />
    public void Debug<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsDebugEnabled)
        {
            return;
        }

        _logger.ForContext<T>().Debug(function.Invoke());
    }

    /// <inheritdoc />
    public void Debug(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsDebugEnabled)
        {
            return;
        }

        _logger.Debug(exception, function.Invoke());
    }

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
    public void DebugException([Localizable(false)] string? message, Exception exception) => _logger.Debug(exception, message ?? exception?.Message ?? string.Empty);

    /// <inheritdoc />
    public void DebugException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsDebugEnabled)
        {
            return;
        }

        _logger.Debug(exception, function.Invoke());
    }

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) =>
        _logger.Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) =>
        _logger.Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) =>
        _logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) =>
        _logger.ForContext(type).Write(SerilogHelper.MappingsDictionary[logLevel], exception, message);
}
