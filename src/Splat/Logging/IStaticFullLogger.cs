// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

#pragma warning disable CA1716 // Identifiers should not match keywords

namespace Splat;

/// <summary>
/// Represents the logging interface for the Static Default Logger.
/// </summary>
public interface IStaticFullLogger
{
    /// <summary>
    /// Gets the level at which the target will emit messages.
    /// </summary>
    LogLevel Level { get; }

    /// <summary>
    /// Emits a debug log message with an exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug(Exception exception, [Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the debug log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the debug log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug<T>([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null);

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
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a info log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info(Exception exception, [Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the info log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the info log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info<T>([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null);

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
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a warning log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn(Exception exception, [Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the warning log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the warning log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn<T>([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null);

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
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a error log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error(Exception exception, [Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the error log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the error log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error<T>([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null);

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
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a fatal log message with exception.
    /// This will emit details about a exception.
    /// This type of logging is not able to be localized.
    /// </summary>
    /// <param name="exception">The exception which to emit in the log.</param>
    /// <param name="message">A message to emit.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal(Exception exception, [Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the fatal log.
    /// </summary>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message to the fatal log.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="message">A non-localizable message to send to the log.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal<T>([Localizable(false)] string message, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName]string? callerMemberName = null);

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
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Write([Localizable(false)] string message, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Writes a message to the target.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="logLevel">The severity level of the log message.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Writes a messge to the target.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="type">The type.</param>
    /// <param name="logLevel">The log level.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null);

    /// <summary>
    /// Writes a messge to the target.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    /// <param name="message">The message.</param>
    /// <param name="type">The type.</param>
    /// <param name="logLevel">The log level.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture in the static logger of some additional context for support and debugging.</param>
    void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel, [CallerMemberName]string? callerMemberName = null);
}
#pragma warning restore CA1716 // Identifiers should not match keywords
