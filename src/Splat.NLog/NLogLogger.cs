// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Splat.NLog;

/// <summary>Provides an implementation of the <see cref="IFullLogger"/> interface that writes log events using the NLog logging framework.</summary>
/// <remarks>This class acts as an adapter, allowing code that depends on <see cref="IFullLogger"/> to log
/// messages through NLog. It supports all standard log levels and message formats, and automatically tracks changes to
/// the underlying NLog logger's configuration. Instances of <see cref="NLogLogger"/> are not thread-affine and can be
/// used concurrently from multiple threads. Disposing the logger detaches internal event handlers but does not dispose
/// the underlying NLog logger.</remarks>
[DebuggerDisplay("Name={_logger.Name} Level={Level}")]
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "T is the caller-supplied source type used via LogResolver.Resolve(typeof(T)); it cannot appear in the parameter list without breaking the IFullLogger API contract.")]
public sealed partial class NLogLogger : IFullLogger, IDisposable
{
    /// <summary>All defined <see cref="LogLevel"/> values, cached to avoid repeated enumeration.</summary>
#if NET5_0_OR_GREATER
    private static readonly LogLevel[] _allLogLevels = Enum.GetValues<LogLevel>();
#else
    private static readonly LogLevel[] _allLogLevels = [.. Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>()];
#endif

    /// <summary>The underlying NLog logger that messages are forwarded to.</summary>
    private readonly global::NLog.Logger _logger;

    /// <summary>Initializes a new instance of the <see cref="NLogLogger"/> class.</summary>
    /// <param name="inner">The NLog logger instance to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when the NLog logger is null.</exception>
    public NLogLogger(global::NLog.Logger inner)
    {
        ArgumentExceptionHelper.ThrowIfNull(inner);
        _logger = inner;
        SetLogLevel();
        _logger.LoggerReconfigured += OnInnerLoggerReconfigured;
    }

    /// <inheritdoc />
    public LogLevel Level
    {
        get; private set;
    }

    /// <inheritdoc />
    public bool IsDebugEnabled => _logger.IsDebugEnabled;

    /// <inheritdoc />
    public bool IsInfoEnabled => _logger.IsInfoEnabled;

    /// <inheritdoc />
    public bool IsWarnEnabled => _logger.IsWarnEnabled;

    /// <inheritdoc />
    public bool IsErrorEnabled => _logger.IsErrorEnabled;

    /// <inheritdoc />
    public bool IsFatalEnabled => _logger.IsFatalEnabled;

    /// <inheritdoc />
    public void Dispose() => _logger.LoggerReconfigured -= OnInnerLoggerReconfigured;

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => _logger.Log(ResolveLogLevel(logLevel), message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => _logger.Log(ResolveLogLevel(logLevel), exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(ResolveLogLevel(logLevel), message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(ResolveLogLevel(logLevel), exception, message);

    /// <inheritdoc/>
    public void Debug<T>(T value) => _logger.Debug(value);

    /// <inheritdoc/>
    public void Debug<T>(IFormatProvider formatProvider, T value) => _logger.Debug(formatProvider, value);

    /// <inheritdoc/>
    public void Debug(Exception exception, string? message) => _logger.Debug(exception, message ?? string.Empty);

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

        LogResolver.Resolve(typeof(T)).Debug(function.Invoke());
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
    public void Debug(IFormatProvider formatProvider, string message, params object[] args) => _logger.Debug(formatProvider, message, args);

    /// <inheritdoc/>
    public void Debug(string? message) => _logger.Debug(message ?? string.Empty);

    /// <inheritdoc/>
    public void Debug<T>(string? message) => LogResolver.Resolve(typeof(T)).Debug(message ?? string.Empty);

    /// <inheritdoc/>
    public void Debug(string message, params object[] args) => _logger.Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _logger.Debug(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2)
        => _logger.Debug(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Debug(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void DebugException(string? message, Exception exception) => _logger.Debug(exception, message ?? string.Empty);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CS0618:Type or member is obsolete", Justification = "Implements an obsolete IFullLogger member retained for backward compatibility.")]
    public void DebugException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsDebugEnabled)
        {
            return;
        }

        _logger.Debug(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Info<T>(T value) => LogResolver.Resolve(typeof(T)).Info(value);

    /// <inheritdoc/>
    public void Info<T>(IFormatProvider formatProvider, T value) => _logger.Info(formatProvider, value);

    /// <inheritdoc/>
    public void Info(Exception exception, string? message) => _logger.Info(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Info(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Info(function.Invoke());
    }

    /// <inheritdoc />
    public void Info<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        LogResolver.Resolve(typeof(T)).Info(function.Invoke());
    }

    /// <inheritdoc />
    public void Info(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Info(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Info(IFormatProvider formatProvider, string message, params object[] args) => _logger.Info(formatProvider, message, args);

    /// <inheritdoc/>
    public void Info(string? message) => _logger.Info(message ?? string.Empty);

    /// <inheritdoc/>
    public void Info<T>(string? message) => LogResolver.Resolve(typeof(T)).Info(message ?? string.Empty);

    /// <inheritdoc/>
    public void Info(string message, params object[] args) => _logger.Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _logger.Info(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _logger.Info(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Info(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void InfoException(string? message, Exception exception) => _logger.Info(exception, message ?? string.Empty);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CS0618:Type or member is obsolete", Justification = "Implements an obsolete IFullLogger member retained for backward compatibility.")]
    public void InfoException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsInfoEnabled)
        {
            return;
        }

        _logger.Info(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Warn<T>(T value) => LogResolver.Resolve(typeof(T)).Warn(value);

    /// <inheritdoc/>
    public void Warn<T>(IFormatProvider formatProvider, T value) => _logger.Warn(formatProvider, value);

    /// <inheritdoc/>
    public void Warn(Exception exception, string? message) => _logger.Warn(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warn(function.Invoke());
    }

    /// <inheritdoc />
    public void Warn<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        LogResolver.Resolve(typeof(T)).Warn(function.Invoke());
    }

    /// <inheritdoc />
    public void Warn(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warn(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Warn(IFormatProvider formatProvider, string message, params object[] args) => _logger.Warn(formatProvider, message, args);

    /// <inheritdoc/>
    public void Warn(string? message) => _logger.Warn(message ?? string.Empty);

    /// <inheritdoc/>
    public void Warn<T>(string? message) => LogResolver.Resolve(typeof(T)).Warn(message ?? string.Empty);

    /// <inheritdoc/>
    public void Warn(string message, params object[] args) => _logger.Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _logger.Warn(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _logger.Warn(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Warn(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void WarnException(string? message, Exception exception) => _logger.Warn(exception, message ?? string.Empty);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CS0618:Type or member is obsolete", Justification = "Implements an obsolete IFullLogger member retained for backward compatibility.")]
    public void WarnException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsWarnEnabled)
        {
            return;
        }

        _logger.Warn(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Error<T>(T value) => LogResolver.Resolve(typeof(T)).Error(value);

    /// <inheritdoc/>
    public void Error<T>(IFormatProvider formatProvider, T value) => _logger.Error(formatProvider, value);

    /// <inheritdoc/>
    public void Error(Exception exception, string? message) => _logger.Error(exception, message ?? string.Empty);

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

        LogResolver.Resolve(typeof(T)).Error(function.Invoke());
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
    public void Error(IFormatProvider formatProvider, string message, params object[] args) => _logger.Error(formatProvider, message, args);

    /// <inheritdoc/>
    public void Error(string? message) => _logger.Error(message ?? string.Empty);

    /// <inheritdoc/>
    public void Error<T>(string? message) => LogResolver.Resolve(typeof(T)).Error(message ?? string.Empty);

    /// <inheritdoc/>
    public void Error(string message, params object[] args) => _logger.Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _logger.Error(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2)
        => _logger.Error(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Error(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void ErrorException(string? message, Exception exception) => _logger.Error(exception, message ?? string.Empty);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CS0618:Type or member is obsolete", Justification = "Implements an obsolete IFullLogger member retained for backward compatibility.")]
    public void ErrorException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsErrorEnabled)
        {
            return;
        }

        _logger.Error(exception, function.Invoke());
    }

    /// <inheritdoc/>
    public void Fatal<T>(T value) => LogResolver.Resolve(typeof(T)).Fatal(value);

    /// <inheritdoc/>
    public void Fatal<T>(IFormatProvider formatProvider, T value) => _logger.Fatal(formatProvider, value);

    /// <inheritdoc/>
    public void Fatal(Exception exception, string? message) => _logger.Fatal(exception, message ?? string.Empty);

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

        LogResolver.Resolve(typeof(T)).Fatal(function.Invoke());
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

    /// <inheritdoc/>
    public void Fatal(IFormatProvider formatProvider, string message, params object[] args) => _logger.Fatal(formatProvider, message, args);

    /// <inheritdoc/>
    public void Fatal(string? message) => _logger.Fatal(message ?? string.Empty);

    /// <inheritdoc/>
    public void Fatal<T>(string? message) => LogResolver.Resolve(typeof(T)).Fatal(message ?? string.Empty);

    /// <inheritdoc/>
    public void Fatal(string message, params object[] args) => _logger.Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _logger.Fatal(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2)
        => _logger.Fatal(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Fatal(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void FatalException(string? message, Exception exception) => _logger.Fatal(exception, message ?? string.Empty);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CS0618:Type or member is obsolete", Justification = "Implements an obsolete IFullLogger member retained for backward compatibility.")]
    public void FatalException(Func<string> function, Exception exception)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (!IsFatalEnabled)
        {
            return;
        }

        _logger.Fatal(exception, function.Invoke());
    }

    /// <summary>Maps a Splat <see cref="LogLevel"/> to the equivalent NLog log level.</summary>
    /// <param name="logLevel">The Splat log level to translate.</param>
    /// <returns>The corresponding <see cref="global::NLog.LogLevel"/>.</returns>
    private static global::NLog.LogLevel ResolveLogLevel(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Debug => global::NLog.LogLevel.Debug,
        LogLevel.Info => global::NLog.LogLevel.Info,
        LogLevel.Warn => global::NLog.LogLevel.Warn,
        LogLevel.Error => global::NLog.LogLevel.Error,
        LogLevel.Fatal => global::NLog.LogLevel.Fatal,
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Unknown LogLevel {logLevel}"),
    };

    /// <summary>Handles the inner NLog logger being reconfigured by recomputing the cached log level.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnInnerLoggerReconfigured(object? sender, EventArgs e) => SetLogLevel();

    /// <summary>Determines the current effective log level based on NLog configuration.</summary>
    /// <remarks>
    /// This optimization avoids re-evaluating the log level on each Write method call.
    /// </remarks>
    private void SetLogLevel()
    {
        foreach (LogLevel logLevel in _allLogLevels)
        {
            if (_logger.IsEnabled(ResolveLogLevel(logLevel)))
            {
                Level = logLevel;
                return;
            }
        }

        // Default to Fatal, it should always be enabled anyway.
        Level = LogLevel.Fatal;
    }
}
