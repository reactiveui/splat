// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;

namespace Splat.NLog;

/// <summary>
/// NLog Logger taken from ReactiveUI 5.
/// </summary>
[DebuggerDisplay("Name={_inner.Name} Level={Level}")]
public sealed class NLogLogger : IFullLogger, IDisposable
{
    private static readonly KeyValuePair<LogLevel, global::NLog.LogLevel>[] _mappings =
    [
        new(LogLevel.Debug, global::NLog.LogLevel.Debug),
        new(LogLevel.Info, global::NLog.LogLevel.Info),
        new(LogLevel.Warn, global::NLog.LogLevel.Warn),
        new(LogLevel.Error, global::NLog.LogLevel.Error),
        new(LogLevel.Fatal, global::NLog.LogLevel.Fatal),
    ];

    private static readonly ImmutableDictionary<LogLevel, global::NLog.LogLevel> _mappingsDictionary = _mappings.ToImmutableDictionary();

    private readonly global::NLog.Logger _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogLogger"/> class.
    /// </summary>
    /// <param name="inner">The actual nlog logger.</param>
    /// <exception cref="ArgumentNullException">NLog logger not passed.</exception>
    public NLogLogger(global::NLog.Logger inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        SetLogLevel();
        _inner.LoggerReconfigured += OnInnerLoggerReconfigured;
    }

    /// <inheritdoc />
    public LogLevel Level
    {
        get; private set;
    }

    /// <inheritdoc />
    public bool IsDebugEnabled => _inner.IsEnabled(global::NLog.LogLevel.Debug);

    /// <inheritdoc />
    public bool IsInfoEnabled => _inner.IsEnabled(global::NLog.LogLevel.Info);

    /// <inheritdoc />
    public bool IsWarnEnabled => _inner.IsEnabled(global::NLog.LogLevel.Warn);

    /// <inheritdoc />
    public bool IsErrorEnabled => _inner.IsEnabled(global::NLog.LogLevel.Error);

    /// <inheritdoc />
    public bool IsFatalEnabled => _inner.IsEnabled(global::NLog.LogLevel.Fatal);

    /// <inheritdoc />
    public void Dispose() => _inner.LoggerReconfigured -= OnInnerLoggerReconfigured;

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => _inner.Log(_mappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => _inner.Log(_mappingsDictionary[logLevel], exception, message);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(_mappingsDictionary[logLevel], message);

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => LogResolver.Resolve(type).Log(_mappingsDictionary[logLevel], exception, message);

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
    public void DebugException(string? message, Exception exception) => _inner.Debug(exception, message);

    /// <inheritdoc/>
    public void Debug(Exception exception, string? message) => _inner.Debug(exception, message);

    /// <inheritdoc/>
    public void Debug(IFormatProvider formatProvider, string message, params object[] args) => _inner.Debug(formatProvider, message, args);

    /// <inheritdoc/>
    public void Debug(string? message) => _inner.Debug(message);

    /// <inheritdoc/>
    public void Debug<T>(string? message) => LogResolver.Resolve(typeof(T)).Debug(message);

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
    public void InfoException(string? message, Exception exception) => _inner.Info(exception, message);

    /// <inheritdoc/>
    public void Info(Exception exception, string? message) => _inner.Info(exception, message);

    /// <inheritdoc/>
    public void Info(IFormatProvider formatProvider, string message, params object[] args) => _inner.Info(formatProvider, message, args);

    /// <inheritdoc/>
    public void Info(string? message) => _inner.Info(message);

    /// <inheritdoc/>
    public void Info<T>(string? message) => LogResolver.Resolve(typeof(T)).Info(message);

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
    public void WarnException(string? message, Exception exception) => _inner.Warn(exception, message);

    /// <inheritdoc/>
    public void Warn(Exception exception, string? message) => _inner.Warn(exception, message);

    /// <inheritdoc/>
    public void Warn(IFormatProvider formatProvider, string message, params object[] args) => _inner.Warn(formatProvider, message, args);

    /// <inheritdoc/>
    public void Warn(string? message) => _inner.Warn(message);

    /// <inheritdoc/>
    public void Warn<T>(string? message) => LogResolver.Resolve(typeof(T)).Warn(message);

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
    public void ErrorException(string? message, Exception exception) => _inner.Error(exception, message);

    /// <inheritdoc/>
    public void Error(Exception exception, string? message) => _inner.Error(exception, message);

    /// <inheritdoc/>
    public void Error(IFormatProvider formatProvider, string message, params object[] args) => _inner.Error(formatProvider, message, args);

    /// <inheritdoc/>
    public void Error(string? message) => _inner.Error(message);

    /// <inheritdoc/>
    public void Error<T>(string? message) => LogResolver.Resolve(typeof(T)).Error(message);

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
    public void FatalException(string? message, Exception exception) => _inner.Fatal(exception, message);

    /// <inheritdoc/>
    public void Fatal(Exception exception, string? message) => _inner.Fatal(exception, message);

    /// <inheritdoc/>
    public void Fatal(IFormatProvider formatProvider, string message, params object[] args) => _inner.Fatal(formatProvider, message, args);

    /// <inheritdoc/>
    public void Fatal(string? message) => _inner.Fatal(message);

    /// <inheritdoc/>
    public void Fatal<T>(string? message) => LogResolver.Resolve(typeof(T)).Fatal(message);

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
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8) => _inner.Error(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(Exception exception, string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10) => _inner.Fatal(exception, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    private void OnInnerLoggerReconfigured(object? sender, EventArgs e) => SetLogLevel();

    /// <summary>
    /// Works out the log level.
    /// </summary>
    /// <remarks>
    /// This was done so the Level property doesn't keep getting re-evaluated each time a Write method is called.
    /// </remarks>
    private void SetLogLevel()
    {
        foreach (var mapping in _mappings)
        {
            if (_inner.IsEnabled(mapping.Value))
            {
                Level = mapping.Key;
                return;
            }
        }

        // Default to Fatal, it should always be enabled anyway.
        Level = LogLevel.Fatal;
    }
}
