// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    /// The text writer to write log messages to.
    /// </summary>
    private readonly TextWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
    /// </summary>
    /// <param name="writer">The text writer to write log messages to. Defaults to Console.Out if null.</param>
    public ConsoleLogger(TextWriter? writer = null) => _writer = writer ?? Console.Out;

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

        _writer.WriteLine(message);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        _writer.WriteLine(string.Format(CultureInfo.InvariantCulture, ExceptionMessageFormat, message, exception));
    }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        _writer.WriteLine(message);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        _writer.WriteLine(string.Format(CultureInfo.InvariantCulture, ExceptionMessageFormat, message, exception));
    }
}
