// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#define DEBUG

using System.ComponentModel;

namespace Splat;

/// <summary>
/// Provides an implementation of the ILogger interface that writes log messages to the debug output window. Intended
/// for use during development and debugging scenarios.
/// </summary>
/// <remarks>DebugLogger outputs log entries using System.Diagnostics.Debug.WriteLine. Log messages are only
/// written if their log level is greater than or equal to the configured Level property. This logger is typically used
/// in development environments, as messages are not persisted and are only visible when a debugger is
/// attached.</remarks>
public class DebugLogger : ILogger
{
    /// <inheritdoc />
    public LogLevel Level { get; set; }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine(message);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine($"{message} - {exception}");
    }

    /// <inheritdoc />
    public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine(message, type?.Name);
    }

    /// <inheritdoc />
    public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine($"{message} - {exception}", type?.Name);
    }
}
