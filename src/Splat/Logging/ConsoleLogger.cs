// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;

namespace Splat;

/// <summary>
/// A logger which will send messages to the console.
/// </summary>
public class ConsoleLogger : ILogger
{
    /// <summary>
    /// Gets or sets the exception message format.
    /// First parameter will be the message, second will be the exception.
    /// </summary>
    public string ExceptionMessageFormat { get; set; } = "{0} - {1}";

    /// <inheritdoc />
    public LogLevel Level { get; set; }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        Console.WriteLine(message);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, ExceptionMessageFormat, message, exception));
    }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        Console.WriteLine(message);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, ExceptionMessageFormat, message, exception));
    }
}
