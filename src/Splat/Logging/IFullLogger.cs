// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// A full logger which wraps all the possible logging methods available.
/// Often not needed for your own loggers.
/// A <see cref="WrappingFullLogger"/> will wrap simple loggers into a full logger.
/// </summary>
[SuppressMessage("Naming", "CA1716: Do not use built in identifiers", Justification = "Deliberate usage")]
public interface IFullLogger : IAllocationFreeLogger
{
    /// <summary>
    /// Emits a debug log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="value">The log to emit.</param>
    void Debug<T>(T value);

    /// <summary>
    /// Emits a debug log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="value">The value to emit.</param>
    void Debug<T>(IFormatProvider formatProvider, T value);

    /// <summary>
    /// Emits a debug log message.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="message">A message to emit.</param>
    /// <param name="exception">The exception which to emit in the log.</param>
    [Obsolete("Use void Debug(Exception exception, [Localizable(false)] string? message)")]
    void DebugException([Localizable(false)] string? message, Exception exception);

    /// <summary>
    /// Emits a debug log message with an exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Debug(Exception exception, [Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message to the debug log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Debug([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message to the debug log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Debug<T>([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Debug([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Debug<T>([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a info log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="value">The log to emit.</param>
    void Info<T>(T value);

    /// <summary>
    /// Emits a info log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="value">The value to emit.</param>
    void Info<T>(IFormatProvider formatProvider, T value);

    /// <summary>
    /// Emits a info log message.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="message">A message to emit.</param>
    /// <param name="exception">The exception which to emit in the log.</param>
    [Obsolete("Use void Info(Exception exception, [Localizable(false)] string? message)")]
    void InfoException([Localizable(false)] string? message, Exception exception);

    /// <summary>
    /// Emits a info log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    void Info(Exception exception, [Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message to the info log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Info([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message to the info log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Info<T>([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Info([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Info<T>([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a warning log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="value">The log to emit.</param>
    void Warn<T>(T value);

    /// <summary>
    /// Emits a warning log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="value">The value to emit.</param>
    void Warn<T>(IFormatProvider formatProvider, T value);

    /// <summary>
    /// Emits a warning log message.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="message">A message to emit.</param>
    /// <param name="exception">The exception which to emit in the log.</param>
    [Obsolete("Use void Warn(Exception exception, [Localizable(false)] string? message)")]
    void WarnException([Localizable(false)] string? message, Exception exception);

    /// <summary>
    /// Emits a warning log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    void Warn(Exception exception, [Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message to the warning log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Warn([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message to the warning log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Warn<T>([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Warn([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Warn<T>([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a error log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="value">The log to emit.</param>
    void Error<T>(T value);

    /// <summary>
    /// Emits a error log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="value">The value to emit.</param>
    void Error<T>(IFormatProvider formatProvider, T value);

    /// <summary>
    /// Emits a error log message.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="message">A message to emit.</param>
    /// <param name="exception">The exception which to emit in the log.</param>
    [Obsolete("Use void Error(Exception exception, [Localizable(false)] string? message)")]
    void ErrorException([Localizable(false)] string? message, Exception exception);

    /// <summary>
    /// Emits a error log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    void Error(Exception exception, [Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message to the error log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Error([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message to the error log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Error<T>([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Error([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Error<T>([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a fatal log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="value">The log to emit.</param>
    void Fatal<T>(T value);

    /// <summary>
    /// Emits a fatal log message.
    /// This will emit the public contents of the object provided to the log.
    /// </summary>
    /// <typeparam name="T">The type of object used as the message.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="value">The value to emit.</param>
    void Fatal<T>(IFormatProvider formatProvider, T value);

    /// <summary>
    /// Emits a fatal log message.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="message">A message to emit.</param>
    /// <param name="exception">The exception which to emit in the log.</param>
    [Obsolete("Use void Fatal(Exception exception, [Localizable(false)] string? message)")]
    void FatalException([Localizable(false)] string? message, Exception exception);

    /// <summary>
    /// Emits a fatal log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    void Fatal(Exception exception, [Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message to the fatal log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Fatal([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message to the fatal log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    void Fatal<T>([Localizable(false)] string? message);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Fatal([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="args">The arguments for formatting purposes.</param>
    void Fatal<T>([Localizable(false)] string message, params object[] args);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
}
