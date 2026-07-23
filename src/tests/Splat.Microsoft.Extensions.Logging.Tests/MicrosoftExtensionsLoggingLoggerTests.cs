// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

using ReactiveUI.Primitives.Disposables;

using Splat.Microsoft.Extensions.Logging;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="MicrosoftExtensionsLoggingLogger"/> class.</summary>
[InheritsTests]
public class MicrosoftExtensionsLoggingLoggerTests : FullLoggerTestBase
{
    /// <summary>Mappings of Splat log levels to equivalent Microsoft.Extensions.Logging log levels.</summary>
    private static readonly Dictionary<LogLevel, global::Microsoft.Extensions.Logging.LogLevel> _splat2MSLog = new()
    {
        { LogLevel.Debug, global::Microsoft.Extensions.Logging.LogLevel.Debug },
        { LogLevel.Info, global::Microsoft.Extensions.Logging.LogLevel.Information },
        { LogLevel.Warn, global::Microsoft.Extensions.Logging.LogLevel.Warning },
        { LogLevel.Error, global::Microsoft.Extensions.Logging.LogLevel.Error },
        { LogLevel.Fatal, global::Microsoft.Extensions.Logging.LogLevel.Critical },
    };

    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var mockLogger = new MockActualMicrosoftExtensionsLoggingLogger(_splat2MSLog[minimumLogLevel]);

        return (new WrappingFullLogger(new MicrosoftExtensionsLoggingLogger(mockLogger)), mockLogger);
    }

    /// <summary>Mock Logger for Testing Microsoft.Extensions.Logging.</summary>
    /// <param name="level">The minimum log level the logger is enabled for.</param>
    private sealed class MockActualMicrosoftExtensionsLoggingLogger(global::Microsoft.Extensions.Logging.LogLevel level) : global::Microsoft.Extensions.Logging.ILogger, IMockLogTarget
    {
        /// <summary>Mappings of Microsoft.Extensions.Logging log levels to equivalent Splat log levels.</summary>
        private static readonly Dictionary<global::Microsoft.Extensions.Logging.LogLevel, LogLevel> _MSLog2Splat = new()
        {
            { global::Microsoft.Extensions.Logging.LogLevel.Debug, LogLevel.Debug },
            { global::Microsoft.Extensions.Logging.LogLevel.Information, LogLevel.Info },
            { global::Microsoft.Extensions.Logging.LogLevel.Warning, LogLevel.Warn },
            { global::Microsoft.Extensions.Logging.LogLevel.Error, LogLevel.Error },
            { global::Microsoft.Extensions.Logging.LogLevel.Critical, LogLevel.Fatal },
        };

        /// <summary>The captured log entries.</summary>
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        /// <inheritdoc/>
        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        /// <inheritdoc/>
        public void Log<TState>(
            global::Microsoft.Extensions.Logging.LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            _logs.Add((_MSLog2Splat[logLevel], $"{state} {exception}"));
        }

        /// <inheritdoc/>
        public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) => logLevel >= level;

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
             where TState : notnull =>
            EmptyDisposable.Instance;
    }
}
