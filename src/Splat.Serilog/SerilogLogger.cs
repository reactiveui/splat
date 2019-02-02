// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Serilog.Events;

namespace Splat.Serilog
{
    /// <summary>
    /// Serilog adapter for Splat.
    /// </summary>
    /// <remarks><seealso cref="ILogger" /></remarks>
    public sealed class SerilogLogger : ILogger
    {
        private static readonly Dictionary<int, LogEventLevel> _levelMapping = new Dictionary<int, LogEventLevel>()
        {
            { (int)LogLevel.Debug, LogEventLevel.Debug },
            { (int)LogLevel.Info, LogEventLevel.Information },
            { (int)LogLevel.Warn, LogEventLevel.Warning },
            { (int)LogLevel.Error, LogEventLevel.Error },
            { (int)LogLevel.Fatal, LogEventLevel.Fatal },
        };

        private readonly global::Serilog.ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual serilog logger.</param>
        /// <exception cref="ArgumentNullException">Serilog logger not passed.</exception>
        public SerilogLogger(global::Serilog.ILogger inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <inheritdoc />
        public LogLevel Level { get; set; }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Write(SplatLogLevelToSerilogLevel(logLevel), message);
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Write(SplatLogLevelToSerilogLevel(logLevel), exception, message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Write(SplatLogLevelToSerilogLevel(logLevel), $"{type.Name}: {message}");
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Write(SplatLogLevelToSerilogLevel(logLevel), exception, $"{type.Name}: {message}");
        }

        private static LogEventLevel SplatLogLevelToSerilogLevel(LogLevel logLevel)
        {
            return _levelMapping[(int)logLevel];
        }
    }
}
