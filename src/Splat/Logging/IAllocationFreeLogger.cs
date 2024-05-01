// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// An allocation free logger which wraps all the possible logging methods available.
/// Often not needed for your own loggers.
/// A <see cref="WrappingFullLogger"/> will wrap simple loggers into a full logger.
/// </summary>
[SuppressMessage("Naming", "CA1716: Do not use built in identifiers", Justification = "Deliberate usage")]
public interface IAllocationFreeLogger : IAllocationFreeErrorLogger
{
    /// <summary>
    /// Gets a value indicating whether the logger currently emits debug logs.
    /// </summary>
    bool IsDebugEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the logger currently emits information logs.
    /// </summary>
    bool IsInfoEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the logger currently emits warning logs.
    /// </summary>
    bool IsWarnEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the logger currently emits error logs.
    /// </summary>
    bool IsErrorEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the logger currently emits fatal logs.
    /// </summary>
    bool IsFatalEnabled { get; }

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Debug<TArgument>([Localizable(false)] string messageFormat, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9);

    /// <summary>
    /// Emits a message using formatting to the debug log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument10">The type of the tenth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    /// <param name="argument10">The tenth argument for formatting purposes.</param>
    void Debug<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Info<TArgument>([Localizable(false)] string messageFormat, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4);

    /// <summary>
    /// Logs a info message with the provided message format and values.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5);

    /// <summary>
    /// Logs a info message with the provided message format and values.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9);

    /// <summary>
    /// Emits a message using formatting to the info log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument10">The type of the tenth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    /// <param name="argument10">The tenth argument for formatting purposes.</param>
    void Info<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Warn<TArgument>([Localizable(false)] string messageFormat, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5);

    /// <summary>
    /// Emits a message using formatting to the warning log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6);

    /// <summary>
    /// Emits a message using formatting to the warn log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7);

    /// <summary>
    /// Emits a message using formatting to the warn log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8);

    /// <summary>
    /// Emits a message using formatting to the warn log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9);

    /// <summary>
    /// Emits a message using formatting to the warn log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument10">The type of the tenth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    /// <param name="argument10">The tenth argument for formatting purposes.</param>
    void Warn<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Error<TArgument>([Localizable(false)] string messageFormat, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9);

    /// <summary>
    /// Emits a message using formatting to the error log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument10">The type of the tenth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    /// <param name="argument10">The tenth argument for formatting purposes.</param>
    void Error<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument">The argument for formatting purposes.</param>
    void Fatal<TArgument>([Localizable(false)] string messageFormat, TArgument argument);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9);

    /// <summary>
    /// Emits a message using formatting to the fatal log.
    /// </summary>
    /// <typeparam name="TArgument1">The type of the first argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument2">The type of the second argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument3">The type of the third argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument4">The type of the fourth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument5">The type of the fifth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument6">The type of the sixth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument7">The type of the seventh argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument8">The type of the eighth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument9">The type of the ninth argument which is used in the formatting.</typeparam>
    /// <typeparam name="TArgument10">The type of the tenth argument which is used in the formatting.</typeparam>
    /// <param name="messageFormat">The message format used to emit a message with the type arguments.</param>
    /// <param name="argument1">The first argument for formatting purposes.</param>
    /// <param name="argument2">The second argument for formatting purposes.</param>
    /// <param name="argument3">The third argument for formatting purposes.</param>
    /// <param name="argument4">The fourth argument for formatting purposes.</param>
    /// <param name="argument5">The fifth argument for formatting purposes.</param>
    /// <param name="argument6">The sixth argument for formatting purposes.</param>
    /// <param name="argument7">The seventh argument for formatting purposes.</param>
    /// <param name="argument8">The eighth argument for formatting purposes.</param>
    /// <param name="argument9">The ninth argument for formatting purposes.</param>
    /// <param name="argument10">The tenth argument for formatting purposes.</param>
    void Fatal<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7, TArgument8, TArgument9, TArgument10>([Localizable(false)] string messageFormat, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3, TArgument4 argument4, TArgument5 argument5, TArgument6 argument6, TArgument7 argument7, TArgument8 argument8, TArgument9 argument9, TArgument10 argument10);
}
