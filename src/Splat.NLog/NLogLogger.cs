// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Splat.NLog
{
    /// <summary>
    /// NLog Logger taken from ReactiveUI 5.
    /// </summary>
    [DebuggerDisplay("Name={_inner.Name} Level={Level}")]
    public sealed class NLogLogger : ILogger
    {
        private static readonly KeyValuePair<LogLevel, global::NLog.LogLevel>[] _mappings = new[]
        {
            new KeyValuePair<LogLevel, global::NLog.LogLevel>(LogLevel.Debug, global::NLog.LogLevel.Debug),
            new KeyValuePair<LogLevel, global::NLog.LogLevel>(LogLevel.Info, global::NLog.LogLevel.Info),
            new KeyValuePair<LogLevel, global::NLog.LogLevel>(LogLevel.Warn, global::NLog.LogLevel.Warn),
            new KeyValuePair<LogLevel, global::NLog.LogLevel>(LogLevel.Error, global::NLog.LogLevel.Error),
            new KeyValuePair<LogLevel, global::NLog.LogLevel>(LogLevel.Fatal, global::NLog.LogLevel.Fatal)
        };

        private readonly global::NLog.ILogger _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual nlog logger.</param>
        /// <exception cref="ArgumentNullException">NLog logger not passed.</exception>
        public NLogLogger(global::NLog.ILogger inner)
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

            _inner.Log(SplatLogLevelToNLogLevel(logLevel), message);
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(SplatLogLevelToNLogLevel(logLevel), exception, message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(SplatLogLevelToNLogLevel(logLevel), $"{type.Name}: {message}");
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level)
            {
                return;
            }

            _inner.Log(SplatLogLevelToNLogLevel(logLevel), exception, $"{type.Name}: {message}");
        }

        private static global::NLog.LogLevel SplatLogLevelToNLogLevel(LogLevel logLevel)
        {
            return _mappings.First(x => x.Key == logLevel).Value;
        }
    }
}
