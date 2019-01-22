// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Splat.Log4Net
{
    /// <summary>
    /// Log4Net Logger integration into Splat.
    /// </summary>
    [DebuggerDisplay("Name={_inner.Logger.Name} Level={Level}")]
    public sealed class Log4NetLogger : ILogger
    {
        private readonly global::log4net.ILog _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual log4net logger.</param>
        /// <exception cref="ArgumentNullException">Log4Net logger not passed.</exception>
        public Log4NetLogger(global::log4net.ILog inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    if (Level <= LogLevel.Debug)
                    {
                        _inner.Debug(message);
                    }

                    break;
                case LogLevel.Info:
                    if (Level <= LogLevel.Info)
                    {
                        _inner.Info(message);
                    }

                    break;
                case LogLevel.Warn:
                    if (Level <= LogLevel.Warn)
                    {
                        _inner.Info(message);
                    }

                    break;
                case LogLevel.Error:
                    if (Level <= LogLevel.Error)
                    {
                        _inner.Error(message);
                    }

                    break;
                case LogLevel.Fatal:
                    if (Level <= LogLevel.Fatal)
                    {
                        _inner.Fatal(message);
                    }

                    break;
                default:
                    if (Level <= LogLevel.Debug)
                    {
                        _inner.Debug(message);
                    }

                    break;
            }
        }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    if (Level <= LogLevel.Debug)
                    {
                        _inner.Debug(message, exception);
                    }

                    break;
                case LogLevel.Info:
                    if (Level <= LogLevel.Info)
                    {
                        _inner.Info(message, exception);
                    }

                    break;
                case LogLevel.Warn:
                    if (Level <= LogLevel.Warn)
                    {
                        _inner.Info(message, exception);
                    }

                    break;
                case LogLevel.Error:
                    if (Level <= LogLevel.Error)
                    {
                        _inner.Error(message, exception);
                    }

                    break;
                case LogLevel.Fatal:
                    if (Level <= LogLevel.Fatal)
                    {
                        _inner.Fatal(message, exception);
                    }

                    break;
                default:
                    if (Level <= LogLevel.Debug)
                    {
                        _inner.Debug(message, exception);
                    }

                    break;
            }
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            Write($"{type.Name}: {message}", logLevel);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel, Exception exception)
        {
            Write($"{type.Name}: {message}", logLevel, exception);
        }
    }
}
