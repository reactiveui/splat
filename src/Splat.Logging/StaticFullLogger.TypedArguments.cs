// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>The typed-argument overloads, kept apart from the rest of the type because the set is
/// dictated by the logging contract: one overload per argument count, each forwarding the arguments
/// on without boxing them.</summary>
public sealed partial class StaticFullLogger
{
    /// <inheritdoc/>
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string? message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Debug(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string? message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Info(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string? message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Warn(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string? message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Error(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string? message, TArgument1 argument1, TArgument2 argument2, [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        IFormatProvider formatProvider,
        string? message,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        [CallerMemberName] string? callerMemberName = null) =>
        _fullLogger.Fatal(
            formatProvider,
            GetSuffixedCallerData(message, callerMemberName),
            argument1,
            argument2,
            argument3);
}
