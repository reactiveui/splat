// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#define DEBUG

using System.ComponentModel;

namespace Splat;

/// <summary>
/// A logger which will send messages to the debug logger.
/// </summary>
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
