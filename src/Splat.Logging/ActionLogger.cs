// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Splat;

/// <summary>A logger where you pass in Action delegates that will be invoked when the Write methods are invoked.</summary>
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
    /// <inheritdoc />
    public LogLevel Level { get; set; }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel) => writeNoType?.Invoke(message, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel) => writeNoTypeWithException?.Invoke(exception, message, logLevel);

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => writeWithType?.Invoke(message, type, logLevel);

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel) => writeWithTypeAndException?.Invoke(exception, message, type, logLevel);
}
