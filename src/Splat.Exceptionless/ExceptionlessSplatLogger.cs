// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Diagnostics;

using Exceptionless;

namespace Splat.Exceptionless;

/// <summary>
/// Splat logger implementation that wraps Exceptionless functionality.
/// </summary>
[DebuggerDisplay("Name={_sourceType} Level={Level}")]
public sealed class ExceptionlessSplatLogger : ILogger
{
    private static readonly KeyValuePair<LogLevel, global::Exceptionless.Logging.LogLevel>[] _mappings =
    [
        new(LogLevel.Debug, global::Exceptionless.Logging.LogLevel.Debug),
        new(LogLevel.Info, global::Exceptionless.Logging.LogLevel.Info),
        new(LogLevel.Warn, global::Exceptionless.Logging.LogLevel.Warn),
        new(LogLevel.Error, global::Exceptionless.Logging.LogLevel.Error),
        new(LogLevel.Fatal, global::Exceptionless.Logging.LogLevel.Fatal)];

    private static readonly ImmutableDictionary<LogLevel, global::Exceptionless.Logging.LogLevel> _mappingsDictionary = _mappings.ToImmutableDictionary();

    private readonly string _sourceType;
    private readonly ExceptionlessClient _exceptionlessClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionlessSplatLogger"/> class.
    /// </summary>
    /// <param name="sourceType">The type that this logger will represent.</param>
    /// <param name="exceptionlessClient">The Exceptionless client instance to use.</param>
    public ExceptionlessSplatLogger(
        Type sourceType,
        ExceptionlessClient exceptionlessClient)
    {
        ArgumentExceptionHelper.ThrowIfNull(sourceType);

        ArgumentExceptionHelper.ThrowIfNullWithMessage(sourceType.FullName, "Cannot find the source type name");
        _sourceType = sourceType.FullName!;
        ArgumentExceptionHelper.ThrowIfNull(exceptionlessClient);
        _exceptionlessClient = exceptionlessClient;
        _exceptionlessClient.Configuration.Changed += OnInnerLoggerReconfigured;
        SetLogLevel();
    }

    /// <inheritdoc />
    public LogLevel Level { get; private set; }

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        CreateLog(message, _mappingsDictionary[logLevel]);
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel)
    {
        if ((int)logLevel < (int)Level)
        {
            return;
        }

        CreateLog(exception, message, _mappingsDictionary[logLevel]);
    }

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        if ((int)logLevel < (int)Level)
        {
            return;
        }

        CreateLog(type.FullName ?? "(unknown)", message, _mappingsDictionary[logLevel]);
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel)
    {
        ArgumentExceptionHelper.ThrowIfNull(type);

        if ((int)logLevel < (int)Level)
        {
            return;
        }

        CreateLog(exception, type.FullName ?? "(unknown)", message, _mappingsDictionary[logLevel]);
    }

    private void CreateLog(string message, global::Exceptionless.Logging.LogLevel level) => CreateLog(_sourceType, message, level);

    private void CreateLog(string type, string message, global::Exceptionless.Logging.LogLevel level) =>
        _exceptionlessClient.SubmitLog(type, message, level);

    private void CreateLog(Exception exception, string message, global::Exceptionless.Logging.LogLevel level) => CreateLog(exception, _sourceType, message, level);

    private void CreateLog(Exception exception, string type, string message, global::Exceptionless.Logging.LogLevel level) =>
        _exceptionlessClient.CreateLog(
            type,
            message,
            level)
            .SetException(exception)
            .Submit();

    /// <summary>
    /// Determines the current effective log level based on Exceptionless configuration.
    /// </summary>
    /// <remarks>
    /// This optimization avoids re-evaluating the log level on each Write method call.
    /// </remarks>
    private void SetLogLevel()
    {
        if (_exceptionlessClient.Configuration.Settings.TryGetValue("@@log:*", out var logLevel))
        {
            var l = global::Exceptionless.Logging.LogLevel.FromString(logLevel);
            Level = _mappingsDictionary.First(x => x.Value == l).Key;
        }
        else
        {
            // Default to Debug if no configuration is found
            Level = LogLevel.Debug;
        }
    }

    private void OnInnerLoggerReconfigured(object? sender, EventArgs e) => SetLogLevel();
}
