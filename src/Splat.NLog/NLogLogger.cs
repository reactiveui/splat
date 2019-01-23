// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
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
        private readonly global::NLog.ILogger _inner;
        private LogLevel _level;

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
        public LogLevel Level
        {
            get => _level;

            set
            {
                // would it be better for ILogger to have a readonly property?
                // rather than a rather blunt way to adjust the level?
                // another considerationwas i didn't want to add a dependency on System.Reactive
                // up to you
                _level = value;
                foreach (var configurationLoggingRule in _inner.Factory.Configuration.LoggingRules)
                {
                    configurationLoggingRule.SetLoggingLevels(SplatLogLevelToNLogLevel(value), global::NLog.LogLevel.Fatal);
                }
            }
        }

        /// <inheritdoc />
        public void Write(string message, LogLevel logLevel)
        {
            _inner.Log(SplatLogLevelToNLogLevel(logLevel), message);
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            _inner.Log(SplatLogLevelToNLogLevel(logLevel), exception, message);
        }

        /// <inheritdoc />
        public void Write(string message, Type type, LogLevel logLevel)
        {
            _inner.Log(SplatLogLevelToNLogLevel(logLevel), $"{type.Name}: {message}");
        }

        /// <inheritdoc />
        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            _inner.Log(SplatLogLevelToNLogLevel(logLevel), exception, $"{type.Name}: {message}");
        }

        private static global::NLog.LogLevel SplatLogLevelToNLogLevel(LogLevel logLevel)
        {
            var mappings = new[]
                               {
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Debug, global::NLog.LogLevel.Debug),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Info, global::NLog.LogLevel.Info),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Warn, global::NLog.LogLevel.Warn),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Error, global::NLog.LogLevel.Error),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Fatal, global::NLog.LogLevel.Fatal)
                               };

            return mappings.First(x => x.Item1 == logLevel).Item2;
        }
    }
}
