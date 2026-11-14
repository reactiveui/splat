// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Splat.Log4Net;

/// <summary>Provides an implementation of the ILogger interface that writes log messages using a wrapped Log4Net logger instance.</summary>
/// <remarks>This class enables integration of Log4Net logging with components that consume the ILogger
/// abstraction. The effective log level is determined from the underlying Log4Net configuration and is updated
/// automatically if the configuration changes. Instances of this class are not thread-safe for disposal; ensure that
/// Dispose is not called concurrently with logging operations.</remarks>
[DebuggerDisplay("Name={_logger.Logger.Name} Level={Level}")]
public sealed class Log4NetLogger : ILogger, IDisposable
{
    /// <summary>The underlying log4net logger that messages are forwarded to.</summary>
    private readonly log4net.ILog _logger;

    /// <summary>Initializes a new instance of the <see cref="Log4NetLogger"/> class.</summary>
    /// <param name="inner">The Log4Net logger instance to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when the Log4Net logger is null.</exception>
    public Log4NetLogger(log4net.ILog inner)
    {
        ArgumentExceptionHelper.ThrowIfNull(inner);
        _logger = inner;
        if (_logger.Logger.Repository is null)
        {
            throw new ArgumentNullException(nameof(inner), "Log4Net repository is not initialized. Configure Log4Net before using with Splat.");
        }

        SetLogLevel();
        _logger.Logger.Repository.ConfigurationChanged += OnInnerLoggerReconfigured;
    }

    /// <inheritdoc />
    public LogLevel Level { get; private set; }

    /// <inheritdoc />
    public void Dispose() => _logger.Logger.Repository!.ConfigurationChanged -= OnInnerLoggerReconfigured;

    /// <inheritdoc />
    public void Write(string message, LogLevel logLevel)
    {
        if (logLevel == LogLevel.Info)
        {
            _logger.Info(message);
        }
        else if (logLevel == LogLevel.Warn)
        {
            _logger.Warn(message);
        }
        else if (logLevel == LogLevel.Error)
        {
            _logger.Error(message);
        }
        else if (logLevel == LogLevel.Fatal)
        {
            _logger.Fatal(message);
        }
        else
        {
            _logger.Debug(message);
        }
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, LogLevel logLevel)
    {
        if (logLevel == LogLevel.Info)
        {
            _logger.Info(message, exception);
        }
        else if (logLevel == LogLevel.Warn)
        {
            _logger.Warn(message, exception);
        }
        else if (logLevel == LogLevel.Error)
        {
            _logger.Error(message, exception);
        }
        else if (logLevel == LogLevel.Fatal)
        {
            _logger.Fatal(message, exception);
        }
        else
        {
            _logger.Debug(message, exception);
        }
    }

    /// <inheritdoc />
    public void Write(string message, Type type, LogLevel logLevel)
    {
        var logger = LogResolver.Resolve(type);
        if (logLevel == LogLevel.Info)
        {
            logger.Info(message);
        }
        else if (logLevel == LogLevel.Warn)
        {
            logger.Warn(message);
        }
        else if (logLevel == LogLevel.Error)
        {
            logger.Error(message);
        }
        else if (logLevel == LogLevel.Fatal)
        {
            logger.Fatal(message);
        }
        else
        {
            logger.Debug(message);
        }
    }

    /// <inheritdoc />
    public void Write(Exception exception, string message, Type type, LogLevel logLevel)
    {
        var logger = LogResolver.Resolve(type);
        if (logLevel == LogLevel.Info)
        {
            logger.Info(message, exception);
        }
        else if (logLevel == LogLevel.Warn)
        {
            logger.Warn(message, exception);
        }
        else if (logLevel == LogLevel.Error)
        {
            logger.Error(message, exception);
        }
        else if (logLevel == LogLevel.Fatal)
        {
            logger.Fatal(message, exception);
        }
        else
        {
            logger.Debug(message, exception);
        }
    }

    /// <summary>Determines the current effective log level based on Log4Net configuration.</summary>
    /// <remarks>
    /// This optimization avoids re-evaluating the log level on each Write method call.
    /// </remarks>
    private void SetLogLevel()
    {
        if (_logger.IsDebugEnabled)
        {
            Level = LogLevel.Debug;
            return;
        }

        if (_logger.IsInfoEnabled)
        {
            Level = LogLevel.Info;
            return;
        }

        if (_logger.IsWarnEnabled)
        {
            Level = LogLevel.Warn;
            return;
        }

        if (_logger.IsErrorEnabled)
        {
            Level = LogLevel.Error;
            return;
        }

        Level = LogLevel.Fatal;
    }

    /// <summary>Handles reconfiguration of the underlying Log4Net repository by recalculating the effective log level.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data associated with the configuration change.</param>
    private void OnInnerLoggerReconfigured(object sender, EventArgs e) => SetLogLevel();
}
