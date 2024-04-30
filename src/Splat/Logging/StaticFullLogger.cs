// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// A full logger which used by the default static logger to allow capture of .NET framework caller data. Wraps a <see cref="IFullLogger"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StaticFullLogger"/> class.
/// </remarks>
/// <param name="fullLogger">The <see cref="IFullLogger"/> to wrap in this class.</param>
public sealed class StaticFullLogger(IFullLogger fullLogger) : IStaticFullLogger
{
    private readonly IFullLogger _fullLogger = fullLogger ?? throw new ArgumentNullException(nameof(fullLogger));

    /// <inheritdoc/>
    public LogLevel Level => _fullLogger.Level;

    /// <inheritdoc/>
    public void Debug(Exception exception, string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Debug(exception, GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Debug(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Debug(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Debug<T>(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Debug<T>(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Info(Exception exception, string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Info(exception, GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Info(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Info(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Info<T>(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Info<T>(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Warn(Exception exception, string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Warn(exception, GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Warn(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Warn(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Warn<T>(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Warn<T>(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Error(Exception exception, string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Error(exception, GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Error(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Error(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Error<T>(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Error<T>(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Fatal(Exception exception, string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Fatal(exception, GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Fatal(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Fatal(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Fatal<T>(string? message, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Fatal<T>(GetSuffixedCallerData(message, callerMemberName));

    /// <inheritdoc/>
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Write(string? message, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Write(GetSuffixedCallerData(message, callerMemberName), logLevel);

    /// <inheritdoc/>
    public void Write(Exception exception, string? message, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Write(exception, GetSuffixedCallerData(message, callerMemberName), logLevel);

    /// <inheritdoc/>
    public void Write([Localizable(false)] string? message, [Localizable(false)] Type type, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Write(GetSuffixedCallerData(message, callerMemberName), type, logLevel);

    /// <inheritdoc/>
    public void Write(Exception exception, [Localizable(false)] string? message, [Localizable(false)] Type type, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null) => _fullLogger.Write(exception, GetSuffixedCallerData(message, callerMemberName), type, logLevel);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSuffixedCallerData(string? message, string? callerMemberName) =>
        $"{message} ({callerMemberName})";
}
