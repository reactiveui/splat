// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Splat.Microsoft.Extensions.Logging;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="MicrosoftExtensionsLoggingLogger"/> class.
    /// </summary>
    public class MicrosoftExtensionsLoggingLoggerTests : FullLoggerTestBase
    {
        private static readonly Dictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel> _splat2MSLog = new Dictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel>
        {
            { LogLevel.Debug, global::Microsoft.Extensions.Logging.LogLevel.Debug },
            { LogLevel.Info, global::Microsoft.Extensions.Logging.LogLevel.Information },
            { LogLevel.Warn, global::Microsoft.Extensions.Logging.LogLevel.Warning },
            { LogLevel.Error, global::Microsoft.Extensions.Logging.LogLevel.Error },
            { LogLevel.Fatal, global::Microsoft.Extensions.Logging.LogLevel.Critical }
        };

        private static readonly Dictionary<global::Microsoft.Extensions.Logging.LogLevel, LogLevel> _MSLog2Splat = new Dictionary<global::Microsoft.Extensions.Logging.LogLevel, LogLevel>
        {
            { global::Microsoft.Extensions.Logging.LogLevel.Debug,  LogLevel.Debug },
            { global::Microsoft.Extensions.Logging.LogLevel.Information,  LogLevel.Info },
            { global::Microsoft.Extensions.Logging.LogLevel.Warning,  LogLevel.Warn },
            { global::Microsoft.Extensions.Logging.LogLevel.Error,  LogLevel.Error },
            { global::Microsoft.Extensions.Logging.LogLevel.Critical,  LogLevel.Fatal }
        };

        /// <inheritdoc/>
        protected override (IFullLogger, IMockLogTarget) GetLogger(LogLevel minimumLogLevel)
        {
            var mockLogger = new MockActualMicrosoftExtensionsLoggingLogger(_splat2MSLog[minimumLogLevel]);

            return (new WrappingFullLogger(new MicrosoftExtensionsLoggingLogger(mockLogger)), mockLogger);
        }

        /// <summary>
        /// Mock Logger for Testing Microsoft.Extensions.Logging.
        /// </summary>
        private sealed class MockActualMicrosoftExtensionsLoggingLogger : global::Microsoft.Extensions.Logging.ILogger, IMockLogTarget
        {
            private readonly List<(LogLevel, string)> _logs = new List<(LogLevel, string)>();
            private readonly global::Microsoft.Extensions.Logging.LogLevel _logLevel;

            public MockActualMicrosoftExtensionsLoggingLogger(global::Microsoft.Extensions.Logging.LogLevel logLevel)
            {
                _logLevel = logLevel;
            }

            public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

            /// <inheritdoc/>
            public void Log<TState>(
                global::Microsoft.Extensions.Logging.LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (IsEnabled(logLevel))
                {
                    _logs.Add((_MSLog2Splat[logLevel], $"{state} {exception}"));
                }
            }

            /// <inheritdoc/>
            public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel)
            {
                return logLevel >= _logLevel;
            }

            /// <inheritdoc/>
            public IDisposable BeginScope<TState>(TState state)
            {
                return ActionDisposable.Empty;
            }
        }
    }
}
