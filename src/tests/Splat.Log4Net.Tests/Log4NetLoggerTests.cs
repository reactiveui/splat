// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

using Splat.Log4Net;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that verify the <see cref="Log4NetLogger"/> class.
/// </summary>
[InheritsTests]
[NotInParallel]
public class Log4NetLoggerTests : FullLoggerTestBase
{
    private static readonly Dictionary<Level, LogLevel> _log4Net2Splat = new()
    {
        { Level.Debug, LogLevel.Debug },
        { Level.Info, LogLevel.Info },
        { Level.Warn, LogLevel.Warn },
        { Level.Error, LogLevel.Error },
        { Level.Fatal, LogLevel.Fatal },
    };

    private static readonly Dictionary<LogLevel, Level> _splat2log4net = new()
    {
        { LogLevel.Debug, Level.Debug },
        { LogLevel.Info, Level.Info },
        { LogLevel.Warn, Level.Warn },
        { LogLevel.Error, Level.Error },
        { LogLevel.Fatal, Level.Fatal },
    };

    private Hierarchy? _hierarchy;
    private log4net.Appender.MemoryAppender? _currentAppender;

    /// <summary>
    /// Set up clean state before each test to prevent state leakage.
    /// </summary>
    [Before(HookType.Test)]
    public void SetupTest()
    {
        // Get the repository and clean up any existing appenders
        _hierarchy = (Hierarchy)LogManager.GetRepository(GetType().Assembly);

        // Remove all appenders from root to start with a clean state
        _hierarchy.Root.RemoveAllAppenders();

        // Reset the repository to unconfigured state
        _hierarchy.ResetConfiguration();
    }

    /// <summary>
    /// Clean up the current test's appender after each test.
    /// </summary>
    [After(HookType.Test)]
    public void CleanupTest()
    {
        if (_hierarchy != null && _currentAppender != null)
        {
            _hierarchy.Root.RemoveAppender(_currentAppender);
            _currentAppender.Close();
            _currentAppender = null;
        }

        if (_hierarchy != null)
        {
            // Reset to a clean state for the next test
            _hierarchy.Root.RemoveAllAppenders();
            _hierarchy.ResetConfiguration();
            _hierarchy = null;
        }
    }

    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        _hierarchy = (Hierarchy)LogManager.GetRepository(GetType().Assembly);

        // Configure the repository with appender
        var memoryAppender = new log4net.Appender.MemoryAppender
        {
            Threshold = _splat2log4net[minimumLogLevel],
            Layout = new PatternLayout
            {
                ConversionPattern = "%m %exception",
            },
        };

        memoryAppender.ActivateOptions();
        _currentAppender = memoryAppender;

        _hierarchy.Root.AddAppender(memoryAppender);
        _hierarchy.Root.Level = _splat2log4net[minimumLogLevel];
        _hierarchy.Configured = true;

        // Get a logger with a unique name
        var loggerName = Guid.NewGuid().ToString();
        var log4netLogger = LogManager.GetLogger(loggerName);

        // Set the individual logger's level
        var hierarchyLogger = (Logger)log4netLogger.Logger;
        hierarchyLogger.Level = _splat2log4net[minimumLogLevel];

        var memoryWrapper = new MemoryTargetWrapper(memoryAppender);

        return (new WrappingFullLogger(new Log4NetLogger(log4netLogger)), memoryWrapper);
    }

    private sealed class MemoryTargetWrapper(log4net.Appender.MemoryAppender memoryTarget) : IMockLogTarget
    {
        public log4net.Appender.MemoryAppender MemoryTarget { get; } = memoryTarget;

        public ICollection<(LogLevel logLevel, string message)> Logs
        {
            get
            {
                MemoryTarget.Flush(0);
                return MemoryTarget.GetEvents().Select(x =>
                {
#if NET8_0_OR_GREATER
                    var currentLevel = _log4Net2Splat.GetValueOrDefault(x.Level ?? Level.Debug, LogLevel.Debug);
#else
                    var levelKey = x.Level ?? Level.Debug;
                    var currentLevel = _log4Net2Splat.ContainsKey(levelKey) ? _log4Net2Splat[levelKey] : LogLevel.Debug;
#endif

                    return x.ExceptionObject switch
                    {
                        not null => (currentLevel, $"{x.MessageObject} {x.ExceptionObject}"),
                        _ => (currentLevel, x.MessageObject?.ToString() ?? string.Empty)
                    };
                }).ToList();
            }
        }
    }
}
