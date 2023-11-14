// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NLog;
using NLog.Config;
using NLog.Targets;
using Splat.NLog;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that verify the <see cref="NLogLogger"/> class.
/// </summary>
public class NLogLoggerTests : FullLoggerTestBase
{
    private static readonly Dictionary<global::NLog.LogLevel, LogLevel> _nLog2Splat = new()
    {
            { global::NLog.LogLevel.Debug, LogLevel.Debug },
            { global::NLog.LogLevel.Error, LogLevel.Error },
            { global::NLog.LogLevel.Warn, LogLevel.Warn },
            { global::NLog.LogLevel.Fatal, LogLevel.Fatal },
            { global::NLog.LogLevel.Info, LogLevel.Info },
    };

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

    private sealed class MemoryTargetWrapper : TargetWithLayout, IMockLogTarget
    {
        private readonly List<(LogLevel, string)> _logs = [];

        public MemoryTargetWrapper() => Name = "test wrapper";

        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        /// <summary>
        /// Renders the logging event message and adds it to the internal ArrayList of log messages.
        /// </summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent) => _logs.Add((_nLog2Splat[logEvent.Level], RenderLogEvent(Layout, logEvent)));
    }
}
