// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NLog;
using NLog.Config;
using NLog.Targets;

using Splat.NLog;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="NLogLogger"/> class.</summary>
[InheritsTests]
public class NLogLoggerTests : FullLoggerTestBase
{
    /// <summary>Mappings of Splat log levels to equivalent NLog log levels.</summary>
    private static readonly Dictionary<LogLevel, global::NLog.LogLevel> _splat2NLog = new()
    {
            { LogLevel.Debug, global::NLog.LogLevel.Debug },
            { LogLevel.Error, global::NLog.LogLevel.Error },
            { LogLevel.Warn, global::NLog.LogLevel.Warn },
            { LogLevel.Fatal, global::NLog.LogLevel.Fatal },
            { LogLevel.Info, global::NLog.LogLevel.Info },
    };

    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var configuration = new LoggingConfiguration();

        var errorTarget = new MemoryTargetWrapper
        {
            Layout = "${message} ${exception:format=tostring}",
        };

        configuration.AddTarget(errorTarget);
        var errorLoggingRule = new LoggingRule("*", _splat2NLog[minimumLogLevel], errorTarget);
        configuration.LoggingRules.Add(errorLoggingRule);

        LogManager.Configuration = configuration;

        return (new NLogLogger(LogManager.GetCurrentClassLogger()), errorTarget);
    }

    /// <summary>An NLog target that captures rendered log events for assertions.</summary>
    private sealed class MemoryTargetWrapper : TargetWithLayout, IMockLogTarget
    {
        /// <summary>Mappings of NLog log levels to equivalent Splat log levels.</summary>
        private static readonly Dictionary<global::NLog.LogLevel, LogLevel> _nlog2Splat = new()
        {
            { global::NLog.LogLevel.Debug, LogLevel.Debug },
            { global::NLog.LogLevel.Error, LogLevel.Error },
            { global::NLog.LogLevel.Warn, LogLevel.Warn },
            { global::NLog.LogLevel.Fatal, LogLevel.Fatal },
            { global::NLog.LogLevel.Info, LogLevel.Info },
        };

        /// <summary>The captured log entries.</summary>
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        /// <summary>Initializes a new instance of the <see cref="MemoryTargetWrapper"/> class.</summary>
        public MemoryTargetWrapper() => Name = "test wrapper";

        /// <inheritdoc/>
        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        /// <summary>Renders the logging event message and adds it to the internal ArrayList of log messages.</summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent) => _logs.Add((_nlog2Splat[logEvent.Level], RenderLogEvent(Layout, logEvent)));
    }
}
