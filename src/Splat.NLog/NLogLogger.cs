// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Globalization;

namespace Splat.NLog;

/// <summary>
/// Splat logger implementation that wraps NLog functionality.
/// </summary>
[DebuggerDisplay("Name={_inner.Name} Level={Level}")]
public sealed class NLogLogger : IFullLogger, IDisposable
{
#if NET5_0_OR_GREATER
    private static readonly LogLevel[] _allLogLevels = Enum.GetValues<LogLevel>();
#else
    private static readonly LogLevel[] _allLogLevels = [.. Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>()];
#endif
    private readonly global::NLog.Logger _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogLogger"/> class.
    /// </summary>
    /// <param name="inner">The NLog logger instance to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when the NLog logger is null.</exception>
    public NLogLogger(global::NLog.Logger inner)
    {
        ArgumentExceptionHelper.ThrowIfNull(inner);
        _inner = inner;
        SetLogLevel();
        _inner.LoggerReconfigured += OnInnerLoggerReconfigured;
    }

    /// <inheritdoc />
    public LogLevel Level
    {
        get; private set;
    }

    /// <inheritdoc />
    public bool IsDebugEnabled => _inner.IsDebugEnabled;

    /// <inheritdoc />
    public bool IsInfoEnabled => _inner.IsInfoEnabled;

    /// <inheritdoc />
    public bool IsWarnEnabled => _inner.IsWarnEnabled;

    /// <inheritdoc />
    public bool IsErrorEnabled => _inner.IsErrorEnabled;

    /// <inheritdoc />
    public bool IsFatalEnabled => _inner.IsFatalEnabled;

    /// <inheritdoc />
    public void Dispose() => _inner.LoggerReconfigured -= OnInnerLoggerReconfigured;

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => _inner.Log(ResolveLogLevel(logLevel), message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => _inner.Log(ResolveLogLevel(logLevel), exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(ResolveLogLevel(logLevel), message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(ResolveLogLevel(logLevel), exception, message);

    /// <inheritdoc/>
    public void Debug<TArgument>(string message, TArgument args) => _inner.Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(string message, TArgument args) => _inner.Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(string message, TArgument args) => _inner.Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Error<TArgument>(string message, TArgument args) => _inner.Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(string message, TArgument args) => _inner.Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Debug<T>(T value) => _inner.Debug(value);

    /// <inheritdoc/>
    public void Debug<T>(IFormatProvider formatProvider, T value) => _inner.Debug(formatProvider, value);

    /// <inheritdoc/>
    public void DebugException(string? message, Exception exception) => _inner.Debug(exception, message ?? string.Empty);

    /// <inheritdoc/>
    public void Debug(Exception exception, string? message) => _inner.Debug(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Debug(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            _inner.Debug(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Debug<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            LogResolver.Resolve(typeof(T)).Debug(function.Invoke());
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
            _inner.Debug(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Debug(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsDebugEnabled)
        {
            _inner.Debug(exception, function.Invoke());
        }
    }

    /// <inheritdoc/>
    public void Debug(IFormatProvider formatProvider, string message, params object[] args) => _inner.Debug(formatProvider, message, args);

    /// <inheritdoc/>
    public void Debug(string? message) => _inner.Debug(message ?? string.Empty);

    /// <inheritdoc/>
    public void Debug<T>(string? message) => LogResolver.Resolve(typeof(T)).Debug(message ?? string.Empty);

    /// <inheritdoc/>
    public void Debug(string message, params object[] args) => _inner.Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Debug(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Debug(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Debug(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<T>(T value) => LogResolver.Resolve(typeof(T)).Info(value);

    /// <inheritdoc/>
    public void Info<T>(IFormatProvider formatProvider, T value) => _inner.Info(formatProvider, value);

    /// <inheritdoc/>
    public void InfoException(string? message, Exception exception) => _inner.Info(exception, message ?? string.Empty);

    /// <inheritdoc/>
    public void Info(Exception exception, string? message) => _inner.Info(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Info(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            _inner.Info(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Info<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            LogResolver.Resolve(typeof(T)).Info(function.Invoke());
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
            _inner.Info(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Info(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsInfoEnabled)
        {
            _inner.Info(exception, function.Invoke());
        }
    }

    /// <inheritdoc/>
    public void Info(IFormatProvider formatProvider, string message, params object[] args) => _inner.Info(formatProvider, message, args);

    /// <inheritdoc/>
    public void Info(string? message) => _inner.Info(message ?? string.Empty);

    /// <inheritdoc/>
    public void Info<T>(string? message) => LogResolver.Resolve(typeof(T)).Info(message ?? string.Empty);

    /// <inheritdoc/>
    public void Info(string message, params object[] args) => _inner.Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Info(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Info(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Info(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<T>(T value) => LogResolver.Resolve(typeof(T)).Warn(value);

    /// <inheritdoc/>
    public void Warn<T>(IFormatProvider formatProvider, T value) => _inner.Warn(formatProvider, value);

    /// <inheritdoc/>
    public void WarnException(string? message, Exception exception) => _inner.Warn(exception, message ?? string.Empty);

    /// <inheritdoc/>
    public void Warn(Exception exception, string? message) => _inner.Warn(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Warn(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            _inner.Warn(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Warn<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            LogResolver.Resolve(typeof(T)).Warn(function.Invoke());
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
            _inner.Warn(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Warn(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsWarnEnabled)
        {
            _inner.Warn(exception, function.Invoke());
        }
    }

    /// <inheritdoc/>
    public void Warn(IFormatProvider formatProvider, string message, params object[] args) => _inner.Warn(formatProvider, message, args);

    /// <inheritdoc/>
    public void Warn(string? message) => _inner.Warn(message ?? string.Empty);

    /// <inheritdoc/>
    public void Warn<T>(string? message) => LogResolver.Resolve(typeof(T)).Warn(message ?? string.Empty);

    /// <inheritdoc/>
    public void Warn(string message, params object[] args) => _inner.Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Warn(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Warn(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Warn(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<T>(T value) => LogResolver.Resolve(typeof(T)).Error(value);

    /// <inheritdoc/>
    public void Error<T>(IFormatProvider formatProvider, T value) => _inner.Error(formatProvider, value);

    /// <inheritdoc/>
    public void ErrorException(string? message, Exception exception) => _inner.Error(exception, message ?? string.Empty);

    /// <inheritdoc/>
    public void Error(Exception exception, string? message) => _inner.Error(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Error(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            _inner.Error(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Error<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            LogResolver.Resolve(typeof(T)).Error(function.Invoke());
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
            _inner.Error(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Error(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsErrorEnabled)
        {
            _inner.Error(exception, function.Invoke());
        }
    }

    /// <inheritdoc/>
    public void Error(IFormatProvider formatProvider, string message, params object[] args) => _inner.Error(formatProvider, message, args);

    /// <inheritdoc/>
    public void Error(string? message) => _inner.Error(message ?? string.Empty);

    /// <inheritdoc/>
    public void Error<T>(string? message) => LogResolver.Resolve(typeof(T)).Error(message ?? string.Empty);

    /// <inheritdoc/>
    public void Error(string message, params object[] args) => _inner.Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Error(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Error(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Error(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<T>(T value) => LogResolver.Resolve(typeof(T)).Fatal(value);

    /// <inheritdoc/>
    public void Fatal<T>(IFormatProvider formatProvider, T value) => _inner.Fatal(formatProvider, value);

    /// <inheritdoc/>
    public void FatalException(string? message, Exception exception) => _inner.Fatal(exception, message ?? string.Empty);

    /// <inheritdoc/>
    public void Fatal(Exception exception, string? message) => _inner.Fatal(exception, message ?? string.Empty);

    /// <inheritdoc />
    public void Fatal(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            _inner.Fatal(function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Fatal<T>(Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            LogResolver.Resolve(typeof(T)).Fatal(function.Invoke());
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
            _inner.Fatal(exception, function.Invoke());
        }
    }

    /// <inheritdoc />
    public void Fatal(Exception exception, Func<string> function)
    {
        ArgumentExceptionHelper.ThrowIfNull(function);

        if (IsFatalEnabled)
        {
            _inner.Fatal(exception, function.Invoke());
        }
    }

    /// <inheritdoc/>
    public void Fatal(IFormatProvider formatProvider, string message, params object[] args) => _inner.Fatal(formatProvider, message, args);

    /// <inheritdoc/>
    public void Fatal(string? message) => _inner.Fatal(message ?? string.Empty);

    /// <inheritdoc/>
    public void Fatal<T>(string? message) => LogResolver.Resolve(typeof(T)).Fatal(message ?? string.Empty);

    /// <inheritdoc/>
    public void Fatal(string message, params object[] args) => _inner.Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<T>(string message, params object[] args) => LogResolver.Resolve(typeof(T)).Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Fatal(formatProvider, message, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) => _inner.Fatal(formatProvider, message, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Fatal(formatProvider, message, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument>(Exception exception, string messageFormat, TArgument argument) => _inner.Debug(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Debug(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Debug(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(Exception exception, string messageFormat, TArgument argument) => _inner.Info(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Info(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Info(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Info(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(Exception exception, string messageFormat, TArgument argument) => _inner.Warn(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Warn(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Warn(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Error<TArgument>(Exception exception, string messageFormat, TArgument argument) => _inner.Error(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Error(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Error(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(Exception exception, string messageFormat, TArgument argument) => _inner.Fatal(exception, messageFormat, argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2) => _inner.Fatal(exception, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    private static global::NLog.LogLevel ResolveLogLevel(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Debug => global::NLog.LogLevel.Debug,
        LogLevel.Info => global::NLog.LogLevel.Info,
        LogLevel.Warn => global::NLog.LogLevel.Warn,
        LogLevel.Error => global::NLog.LogLevel.Error,
        LogLevel.Fatal => global::NLog.LogLevel.Fatal,
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Unknown LogLevel {logLevel}"),
    };

    private void OnInnerLoggerReconfigured(object? sender, EventArgs e) => SetLogLevel();

    /// <summary>
    /// Determines the current effective log level based on NLog configuration.
    /// </summary>
    /// <remarks>
    /// This optimization avoids re-evaluating the log level on each Write method call.
    /// </remarks>
    private void SetLogLevel()
    {
        foreach (LogLevel logLevel in _allLogLevels)
        {
            if (_inner.IsEnabled(ResolveLogLevel(logLevel)))
            {
                Level = logLevel;
                return;
            }
        }

        // Default to Fatal, it should always be enabled anyway.
        Level = LogLevel.Fatal;
    }
}
