// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>The typed-argument overloads, kept apart from the rest of the type because the set is
/// dictated by the logging contract: one overload per argument count, each forwarding the arguments
/// on without boxing them.</summary>
public partial interface IStaticFullLogger
{
    /// <summary>Emits a message using formatting to the debug log.</summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the debug log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the debug log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Debug<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the info log.</summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the info log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the info log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Info<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the warning log.</summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the warning log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the warning log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Warn<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the error log.</summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the error log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the error log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Error<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the fatal log.</summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument, [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the fatal log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument1, TArgument2>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        [CallerMemberName] string? callerMemberName = null);

    /// <summary>Emits a message using formatting to the fatal log.</summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="message">A message to emit to the log which includes the standard formatting tags.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="callerMemberName">Allows you to pass the method or property name of the caller to the method, used to allow the capture
    /// in the static logger of some additional context for support and debugging.</param>
    void Fatal<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        [Localizable(false)] string message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null);
}
