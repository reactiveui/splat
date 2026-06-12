// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

namespace Splat.NLog;

/// <summary>Contains the allocation-free, strongly-typed logging overloads that accept a message format and typed arguments.</summary>
public sealed partial class NLogLogger
{
    /// <inheritdoc/>
    public void Debug<TArgument>(string message, TArgument args) => _logger.Debug(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10)
        => _logger.Debug(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Info<TArgument>(string message, TArgument args) => _logger.Info(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10)
        => _logger.Info(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Warn<TArgument>(string message, TArgument args) => _logger.Warn(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10)
        => _logger.Warn(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Error<TArgument>(string message, TArgument args) => _logger.Error(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10)
        => _logger.Error(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);

    /// <inheritdoc/>
    public void Fatal<TArgument>(string message, TArgument args) => _logger.Fatal(CultureInfo.InvariantCulture, message, args);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2>(string messageFormat, TArgument1 argument1, TArgument2 argument2) => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);

    /// <inheritdoc/>
    public void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>(
        string messageFormat,
        TArgument1 argument1,
        TArgument2 argument2,
        TArgument3 argument3,
        TArgument4 argument4,
        TArgument5 argument5,
        TArgument6 argument6,
        TArgument7 argument7,
        TArgument8 argument8,
        TArgument9 argument9,
        TArgument10 argument10)
        => _logger.Fatal(CultureInfo.InvariantCulture, messageFormat, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
}
