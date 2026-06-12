// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Mocks;

/// <summary>A <see cref="TextWriter"/> implementation of <see cref="ILogger"/> for testing.</summary>
/// <seealso cref="ILogger" />
public class TextLogger : ILogger, IMockLogTarget
{
    /// <summary>Types registered with this logger via <see cref="IMockLogTarget"/>.</summary>
    private readonly List<Type> _types = [];

    /// <summary>The log entries captured by this logger.</summary>
    private readonly List<(LogLevel logLevel, string message)> _logs = [];

    /// <inheritdoc />
    public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

    /// <inheritdoc />
    public LogLevel Level { get; set; }

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel) => _logs.Add((logLevel, message));

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel) => Write($"{message} {exception}", logLevel);

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel)
    {
        _logs.Add((logLevel, message));
        _types.Add(type);
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel) => Write($"{message} {exception}", type, logLevel);
}
