// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Splat.Log4Net;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Splat.Tests.Logging;

/// <summary>Tests the <see cref="Log4NetLogger"/> construction guards, disposal, and handling of unmapped log levels.</summary>
[NotInParallel]
public sealed class Log4NetLoggerContractTests
{
    /// <summary>The memory appender attached to the hierarchy root for the current test.</summary>
    private MemoryAppender? _appender;

    /// <summary>Removes any residual appenders before each test so log events are captured cleanly.</summary>
    [SuppressMessage("Design", "SST2326:Concrete type narrowing", Justification = "Test needs the concrete log4net Hierarchy to reach Root, which ILoggerRepository does not expose.")]
    [Before(HookType.Test)]
    public void SetupTest()
    {
        var hierarchy = (Hierarchy)LogManager.GetRepository(GetType().Assembly);
        hierarchy.Root.RemoveAllAppenders();
        hierarchy.ResetConfiguration();
    }

    /// <summary>Detaches the test's appender afterwards to avoid leaking state into other tests.</summary>
    [SuppressMessage("Design", "SST2326:Concrete type narrowing", Justification = "Test needs the concrete log4net Hierarchy to reach Root, which ILoggerRepository does not expose.")]
    [After(HookType.Test)]
    public void CleanupTest()
    {
        var hierarchy = (Hierarchy)LogManager.GetRepository(GetType().Assembly);
        if (_appender is not null)
        {
            _ = hierarchy.Root.RemoveAppender(_appender);
            _appender.Close();
            _appender = null;
        }

        hierarchy.Root.RemoveAllAppenders();
        hierarchy.ResetConfiguration();
    }

    /// <summary>Verifies the constructor rejects a logger whose repository has not been initialized.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithUninitializedRepository_Throws()
    {
        var inner = new NullRepositoryLog();

        await Assert.That(() => new Log4NetLogger(inner)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies disposing the logger detaches its reconfiguration handler without throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_DetachesReconfigurationHandler()
    {
        var log4netLogger = LogManager.GetLogger(GetType().Assembly, Guid.NewGuid().ToString());
        using var logger = new Log4NetLogger(log4netLogger);

        await Assert.That(logger.Dispose).ThrowsNothing();
    }

    /// <summary>Verifies an unmapped log level on the message overload falls back to a debug write.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_WithUnmappedLevel_FallsBackToDebug()
    {
        var logger = CreateLoggerCapturingToRoot();

        logger.Write("fallback-message", (LogLevel)int.MaxValue);

        var logged = _appender!.GetEvents().Single();
        using (Assert.Multiple())
        {
            await Assert.That(logged.Level).IsEqualTo(Level.Debug);
            await Assert.That(logged.RenderedMessage).IsEqualTo("fallback-message");
        }
    }

    /// <summary>Verifies an unmapped log level on the exception overload falls back to a debug write.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_WithUnmappedLevel_FallsBackToDebug()
    {
        var logger = CreateLoggerCapturingToRoot();

        logger.Write(new InvalidOperationException("boom"), "fallback-exception", (LogLevel)int.MaxValue);

        var logged = _appender!.GetEvents().Single();
        using (Assert.Multiple())
        {
            await Assert.That(logged.Level).IsEqualTo(Level.Debug);
            await Assert.That(logged.RenderedMessage).IsEqualTo("fallback-exception");
        }
    }

    /// <summary>Verifies an unmapped log level on the typed message overload falls back to a debug write.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_MessageWithType_WithUnmappedLevel_FallsBackToDebug()
    {
        var logger = CreateLoggerCapturingToRoot();

        logger.Write("typed-fallback-message", typeof(Log4NetLoggerContractTests), (LogLevel)int.MaxValue);

        var logged = _appender!.GetEvents().Single();
        using (Assert.Multiple())
        {
            await Assert.That(logged.Level).IsEqualTo(Level.Debug);
            await Assert.That(logged.RenderedMessage).IsEqualTo("typed-fallback-message");
        }
    }

    /// <summary>Verifies an unmapped log level on the typed exception overload falls back to a debug write.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_ExceptionWithType_WithUnmappedLevel_FallsBackToDebug()
    {
        var logger = CreateLoggerCapturingToRoot();

        logger.Write(new InvalidOperationException("boom"), "typed-fallback-exception", typeof(Log4NetLoggerContractTests), (LogLevel)int.MaxValue);

        var logged = _appender!.GetEvents().Single();
        using (Assert.Multiple())
        {
            await Assert.That(logged.Level).IsEqualTo(Level.Debug);
            await Assert.That(logged.RenderedMessage).IsEqualTo("typed-fallback-exception");
        }
    }

    /// <summary>Builds a <see cref="Log4NetLogger"/> whose writes are captured by a root memory appender.</summary>
    /// <returns>A logger backed by a freshly configured hierarchy root capturing every level.</returns>
    [SuppressMessage("Design", "SST2326:Concrete type narrowing", Justification = "Test needs the concrete log4net Hierarchy to reach Root, which ILoggerRepository does not expose.")]
    private Log4NetLogger CreateLoggerCapturingToRoot()
    {
        var hierarchy = (Hierarchy)LogManager.GetRepository(GetType().Assembly);

        var appender = new MemoryAppender { Threshold = Level.All };
        appender.ActivateOptions();
        _appender = appender;

        hierarchy.Root.AddAppender(appender);
        hierarchy.Root.Level = Level.All;
        hierarchy.Configured = true;

        var log4netLogger = LogManager.GetLogger(GetType().Assembly, Guid.NewGuid().ToString());
        ((Logger)log4netLogger.Logger).Level = Level.All;

        return new(log4netLogger);
    }

    /// <summary>A log4net logger whose repository is null, used to exercise the constructor guard.</summary>
    private sealed class NullRepositoryLogger : log4net.Core.ILogger
    {
        /// <inheritdoc />
        public string Name => "null-repository";

        /// <inheritdoc />
        public ILoggerRepository Repository => null!;

        /// <inheritdoc />
        public bool IsEnabledFor(Level? level) => false;

        /// <inheritdoc />
        public void Log(Type callerStackBoundaryDeclaringType, Level? level, object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void Log(LoggingEvent logEvent)
        {
        }
    }

    /// <summary>A minimal <see cref="ILog"/> whose wrapped logger reports a null repository.</summary>
    private sealed class NullRepositoryLog : ILog
    {
        /// <inheritdoc />
        public log4net.Core.ILogger Logger { get; } = new NullRepositoryLogger();

        /// <inheritdoc />
        public bool IsDebugEnabled => false;

        /// <inheritdoc />
        public bool IsInfoEnabled => false;

        /// <inheritdoc />
        public bool IsWarnEnabled => false;

        /// <inheritdoc />
        public bool IsErrorEnabled => false;

        /// <inheritdoc />
        public bool IsFatalEnabled => false;

        /// <inheritdoc />
        public void Debug(object? message)
        {
        }

        /// <inheritdoc />
        public void Debug(object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(string format, object? arg0)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(string format, object? arg0, object? arg1)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(string format, object? arg0, object? arg1, object? arg2)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(IFormatProvider? provider, string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void Info(object? message)
        {
        }

        /// <inheritdoc />
        public void Info(object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(string format, object? arg0)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(string format, object? arg0, object? arg1)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(string format, object? arg0, object? arg1, object? arg2)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(IFormatProvider? provider, string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void Warn(object? message)
        {
        }

        /// <inheritdoc />
        public void Warn(object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(string format, object? arg0)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(string format, object? arg0, object? arg1)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(string format, object? arg0, object? arg1, object? arg2)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(IFormatProvider? provider, string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void Error(object? message)
        {
        }

        /// <inheritdoc />
        public void Error(object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, object? arg0)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, object? arg0, object? arg1)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, object? arg0, object? arg1, object? arg2)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(IFormatProvider? provider, string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void Fatal(object? message)
        {
        }

        /// <inheritdoc />
        public void Fatal(object? message, Exception? exception)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(string format, params object?[]? args)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(string format, object? arg0)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(string format, object? arg0, object? arg1)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(string format, object? arg0, object? arg1, object? arg2)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(IFormatProvider? provider, string format, params object?[]? args)
        {
        }
    }
}
