// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>
/// A logger where you pass in Action delegates that will be invoked when the Write methods are invoked.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ActionLogger"/> class.
/// </remarks>
/// <param name="writeNoType">A action which is called when the <see cref="Write(string, LogLevel)"/> is called.</param>
/// <param name="writeWithType">A action which is called when the <see cref="Write(string, Type, LogLevel)"/> is called.</param>
/// <param name="writeNoTypeWithException">A action which is called when the <see cref="Write(Exception, string, LogLevel)"/> is called.</param>
/// <param name="writeWithTypeAndException">A action which is called when the <see cref="Write(Exception, string, Type, LogLevel)"/> is called.</param>
public class ActionLogger(
    Action<string, LogLevel> writeNoType,
    Action<string, Type, LogLevel> writeWithType,
    Action<Exception, string, LogLevel> writeNoTypeWithException,
    Action<Exception, string, Type, LogLevel> writeWithTypeAndException) : ILogger
{
    private readonly Action<string, LogLevel> _writeNoType = writeNoType;
    private readonly Action<Exception, string, LogLevel> _writeNoTypeWithException = writeNoTypeWithException;
    private readonly Action<string, Type, LogLevel> _writeWithType = writeWithType;
    private readonly Action<Exception, string, Type, LogLevel> _writeWithTypeAndException = writeWithTypeAndException;

    /// <inheritdoc />
    public LogLevel Level { get; set; }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => _writeNoType?.Invoke(message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => _writeNoTypeWithException?.Invoke(exception, message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => _writeWithType?.Invoke(message, type, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => _writeWithTypeAndException?.Invoke(exception, message, type, logLevel);
}
