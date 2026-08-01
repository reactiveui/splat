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
