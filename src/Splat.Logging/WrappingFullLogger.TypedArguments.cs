// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>The typed-argument overloads, kept apart from the rest of the type because the set is
/// dictated by the logging contract: one overload per argument count, each forwarding the arguments
/// on without boxing them.</summary>
public partial class WrappingFullLogger
{
    /// <inheritdoc />
    public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Debug);

    /// <inheritdoc />
    public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Debug);

    /// <inheritdoc />
    public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Info);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Info);

    /// <inheritdoc />
    public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Info);

    /// <inheritdoc />
    public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Warn);

    /// <inheritdoc />
    public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Warn);

    /// <inheritdoc />
    public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Error);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Error);

    /// <inheritdoc />
    public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Error);

    /// <inheritdoc />
    public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument) => _inner.Write(string.Format(formatProvider, message, argument), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2), LogLevel.Fatal);

    /// <inheritdoc />
    public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3) =>
        _inner.Write(string.Format(formatProvider, message, argument1, argument2, argument3), LogLevel.Fatal);
}
