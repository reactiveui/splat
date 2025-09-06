// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Splat.Log4Net;

/// <summary>
/// Splat logger implementation that wraps Log4Net functionality.
/// </summary>
[DebuggerDisplay("Name={_inner.Logger.Name} Level={Level}")]
public sealed class Log4NetLogger : ILogger, IDisposable
{
    private readonly log4net.ILog _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
    /// </summary>
    /// <param name="inner">The Log4Net logger instance to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when the Log4Net logger is null.</exception>
    public Log4NetLogger(log4net.ILog inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        if (_inner.Logger.Repository == null)
        {
            throw new ArgumentException("Log4Net repository is not initialized. Configure Log4Net before using with Splat.", nameof(inner));
        }

        SetLogLevel();
        _inner.Logger.Repository.ConfigurationChanged += OnInnerLoggerReconfigured;
    }

    /// <inheritdoc />
    public LogLevel Level { get; private set; }

    /// <inheritdoc />
    public void Dispose() => _inner.Logger.Repository!.ConfigurationChanged -= OnInnerLoggerReconfigured;

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Debug:
                _inner.Debug(message);

                break;
            case LogLevel.Info:
                _inner.Info(message);

                break;
            case LogLevel.Warn:
                _inner.Warn(message);

                break;
            case LogLevel.Error:
                _inner.Error(message);

                break;
            case LogLevel.Fatal:
                _inner.Fatal(message);

                break;
            default:
                _inner.Debug(message);

                break;
        }
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Debug:
                _inner.Debug(message, exception);
                break;
            case LogLevel.Info:
                _inner.Info(message, exception);

                break;
            case LogLevel.Warn:
                _inner.Warn(message, exception);

                break;
            case LogLevel.Error:
                _inner.Error(message, exception);

                break;
            case LogLevel.Fatal:
                _inner.Fatal(message, exception);

                break;
            default:
                _inner.Debug(message, exception);

                break;
        }
    }

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel)
    {
        var logger = LogResolver.Resolve(type);
        switch (logLevel)
        {
            case LogLevel.Debug:
                logger.Debug(message);
                break;
            case LogLevel.Info:
                logger.Info(message);

                break;
            case LogLevel.Warn:
                logger.Warn(message);

                break;
            case LogLevel.Error:
                logger.Error(message);

                break;
            case LogLevel.Fatal:
                logger.Fatal(message);

                break;
            default:
                logger.Debug(message);

                break;
        }
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel)
    {
        var logger = LogResolver.Resolve(type);
        switch (logLevel)
        {
            case LogLevel.Debug:
                logger.Debug(message, exception);
                break;
            case LogLevel.Info:
                logger.Info(message, exception);

                break;
            case LogLevel.Warn:
                logger.Warn(message, exception);

                break;
            case LogLevel.Error:
                logger.Error(message, exception);

                break;
            case LogLevel.Fatal:
                logger.Fatal(message, exception);

                break;
            default:
                logger.Debug(message, exception);

                break;
        }
    }

    /// <summary>
    /// Determines the current effective log level based on Log4Net configuration.
    /// </summary>
    /// <remarks>
    /// This optimization avoids re-evaluating the log level on each Write method call.
    /// </remarks>
    private void SetLogLevel()
    {
        if (_inner.IsDebugEnabled)
        {
            Level = LogLevel.Debug;
            return;
        }

        if (_inner.IsInfoEnabled)
        {
            Level = LogLevel.Info;
            return;
        }

        if (_inner.IsWarnEnabled)
        {
            Level = LogLevel.Warn;
            return;
        }

        if (_inner.IsErrorEnabled)
        {
            Level = LogLevel.Error;
            return;
        }

        Level = LogLevel.Fatal;
    }

    private void OnInnerLoggerReconfigured(object sender, EventArgs e) => SetLogLevel();
}
