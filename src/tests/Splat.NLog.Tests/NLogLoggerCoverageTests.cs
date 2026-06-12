// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NLog;
using NLog.Config;
using NLog.Targets;

using Splat.NLog;

namespace Splat.Tests.Logging;

/// <summary>Tests that cover the <see cref="NLogLogger"/> Write overloads and level mapping.</summary>
[NotInParallel]
public class NLogLoggerCoverageTests
{
    /// <summary>Mappings of NLog log levels to equivalent Splat log levels.</summary>
    private static readonly Dictionary<global::NLog.LogLevel, LogLevel> _nLog2Splat = new()
    {
        { global::NLog.LogLevel.Debug, LogLevel.Debug },
        { global::NLog.LogLevel.Error, LogLevel.Error },
        { global::NLog.LogLevel.Warn, LogLevel.Warn },
        { global::NLog.LogLevel.Fatal, LogLevel.Fatal },
        { global::NLog.LogLevel.Info, LogLevel.Info },
    };

    /// <summary>Mappings of Splat log levels to equivalent NLog log levels.</summary>
    private static readonly Dictionary<LogLevel, global::NLog.LogLevel> _splat2NLog = new()
    {
        { LogLevel.Debug, global::NLog.LogLevel.Debug },
        { LogLevel.Error, global::NLog.LogLevel.Error },
        { LogLevel.Warn, global::NLog.LogLevel.Warn },
        { LogLevel.Fatal, global::NLog.LogLevel.Fatal },
        { LogLevel.Info, global::NLog.LogLevel.Info },
    };

    /// <summary>Verifies the simple <see cref="NLogLogger.Write(string, LogLevel)"/> overload emits at each level.</summary>
    /// <param name="logLevel">The Splat log level under test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(LogLevel.Debug)]
    [Arguments(LogLevel.Info)]
    [Arguments(LogLevel.Warn)]
    [Arguments(LogLevel.Error)]
    [Arguments(LogLevel.Fatal)]
    public async Task Write_Message_LogLevel_Emits(LogLevel logLevel)
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("a message", logLevel);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(logLevel);
        await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo("a message");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(Exception, string, LogLevel)"/> overload emits message and exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write(new InvalidOperationException("boom"), "with exception", LogLevel.Error);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Error);
        await Assert.That(target.Logs[^1].message).Contains("with exception");
        await Assert.That(target.Logs[^1].message).Contains("boom");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(string, Type, LogLevel)"/> type-based overload emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_Type_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("typed message", typeof(NLogLoggerCoverageTests), LogLevel.Warn);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Warn);
        await Assert.That(target.Logs[^1].message.Trim()).IsEqualTo("typed message");
    }

    /// <summary>Verifies the <see cref="NLogLogger.Write(Exception, string, Type, LogLevel)"/> type-and-exception overload emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_Type_LogLevel_Emits()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write(new InvalidOperationException("kaboom"), "typed with exception", typeof(NLogLoggerCoverageTests), LogLevel.Fatal);

        await Assert.That(target.Logs[^1].logLevel).IsEqualTo(LogLevel.Fatal);
        await Assert.That(target.Logs[^1].message).Contains("typed with exception");
        await Assert.That(target.Logs[^1].message).Contains("kaboom");
    }

    /// <summary>Verifies that writing below the configured minimum level emits nothing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Below_Minimum_Level_Does_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Write("ignored", LogLevel.Debug);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Verifies that the cached <see cref="NLogLogger.Level"/> reflects the configured minimum level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Reflects_Configured_Minimum()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>Verifies the IsXxxEnabled flags follow the configured minimum level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task IsEnabled_Flags_Follow_Minimum_Level()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        using (Assert.Multiple())
        {
            await Assert.That(logger.IsDebugEnabled).IsFalse();
            await Assert.That(logger.IsInfoEnabled).IsFalse();
            await Assert.That(logger.IsWarnEnabled).IsTrue();
            await Assert.That(logger.IsErrorEnabled).IsTrue();
            await Assert.That(logger.IsFatalEnabled).IsTrue();
        }
    }

    /// <summary>Verifies that an unknown <see cref="LogLevel"/> value throws when mapped.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Unknown_LogLevel_Throws()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        await Assert.That(() => logger.Write("bad", (LogLevel)999)).Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>Verifies that disposing the logger does not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_Does_Not_Throw()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        var disposable = (IDisposable)logger;

        await Assert.That(() => disposable.Dispose()).ThrowsNothing();
    }

    /// <summary>Verifies that the constructor throws when given a null inner logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_Null_Inner_Throws() =>
        await Assert.That(() => new NLogLogger(null!)).Throws<ArgumentNullException>();

    /// <summary>Creates an <see cref="NLogLogger"/> backed by a capturing in-memory target.</summary>
    /// <param name="minimumLogLevel">The minimum Splat log level to configure.</param>
    /// <returns>The configured logger and its capture target.</returns>
    private static (NLogLogger Logger, MemoryTargetWrapper Target) GetLogger(LogLevel minimumLogLevel)
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
    private sealed class MemoryTargetWrapper : TargetWithLayout
    {
        /// <summary>The captured log entries.</summary>
        private readonly List<(LogLevel LogLevel, string Message)> _logs = [];

        /// <summary>Initializes a new instance of the <see cref="MemoryTargetWrapper"/> class.</summary>
        public MemoryTargetWrapper() => Name = "coverage wrapper";

        /// <summary>Gets the captured log entries.</summary>
        public IReadOnlyList<(LogLevel logLevel, string message)> Logs => _logs;

        /// <summary>Renders the logging event message and records it.</summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent) => _logs.Add((_nLog2Splat[logEvent.Level], RenderLogEvent(Layout, logEvent)));
    }
}
