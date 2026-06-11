// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Display;

using Splat.Common.Test;
using Splat.Serilog;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="SerilogFullLogger"/> class.</summary>
[NotInParallel] // touches global static state (AppLocator, Serilog.Log.Logger)
[InheritsTests]
public class SerilogLoggerTests : FullLoggerTestBase
{
    /// <summary>The newline characters used to trim log messages.</summary>
    private static readonly char[] _newLine = Environment.NewLine.ToCharArray();

    /// <summary>Gets a list of mappings of Serilog levels and equivalent Splat log levels.</summary>
    private static readonly Dictionary<LogLevel, LogEventLevel> _mappingsToSerilog = new()
    {
        { LogLevel.Debug, LogEventLevel.Debug },
        { LogLevel.Info, LogEventLevel.Information },
        { LogLevel.Warn, LogEventLevel.Warning },
        { LogLevel.Error, LogEventLevel.Error },
        { LogLevel.Fatal, LogEventLevel.Fatal },
    };

    /// <summary>Gets a list of mappings of Serilog levels and equivalent Splat log levels.</summary>
    private static readonly Dictionary<LogEventLevel, LogLevel> _mappingsToSplat = new()
    {
        { LogEventLevel.Debug, LogLevel.Debug },
        { LogEventLevel.Information, LogLevel.Info },
        { LogEventLevel.Warning, LogLevel.Warn },
        { LogEventLevel.Error, LogLevel.Error },
        { LogEventLevel.Fatal, LogLevel.Fatal },
    };

    /// <summary>The app locator scope used to isolate registrations per test.</summary>
    private AppLocatorScope? _appLocatorScope;

    /// <summary>Setup method to initialize AppLocatorScope before each test.</summary>
    [Before(Test)]
    public void SetUpAppLocatorScope() => _appLocatorScope = new();

    /// <summary>Teardown method to dispose AppLocatorScope after each test.</summary>
    [After(Test)]
    public void TearDownAppLocatorScope()
    {
        _appLocatorScope?.Dispose();
        _appLocatorScope = null;
    }

    /// <summary>Test to make sure the calling `UseSerilogWithWrappingFullLogger` logs.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configuring_With_Static_Log_Should_Write_Message()
    {
        var (seriLogger, target) = CreateSerilogger(LogLevel.Debug);
        Log.Logger = seriLogger;

        Locator.CurrentMutable.UseSerilogFullLogger();

        await Assert.That(target.Logs).IsEmpty();

        IEnableLogger logger = null!;
        logger.Log().Debug<DummyObjectClass2>("This is a test.");

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).Count().IsEqualTo(1);
            await Assert.That(target.Logs.Last().message.Trim(_newLine).Trim()).IsEqualTo("This is a test.");
        }
    }

    /// <summary>Test to make calling `UseSerilogWithWrappingFullLogger(Serilog.ILogger)` logs.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configuring_With_PreConfigured_Log_Should_Write_Message()
    {
        var (seriLogger, target) = CreateSerilogger(LogLevel.Debug);
        AppLocator.CurrentMutable.UseSerilogFullLogger(seriLogger);

        await Assert.That(target.Logs).IsEmpty();

        IEnableLogger logger = null!;
        logger.Log().Debug<DummyObjectClass2>("This is a test.");

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).Count().IsEqualTo(1);
            await Assert.That(target.Logs.Last().message.Trim(_newLine).Trim()).IsEqualTo("This is a test.");
        }
    }

    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var (log, messages) = CreateSerilogger(minimumLogLevel);
        return (new SerilogFullLogger(log), messages);
    }

    /// <summary>Creates a Serilog logger and its associated mock log target.</summary>
    /// <param name="minimumLogLevel">The minimum log level to capture.</param>
    /// <returns>A tuple containing the Serilog logger and the mock log target.</returns>
    private static (Logger logger, IMockLogTarget mockTarget) CreateSerilogger(LogLevel minimumLogLevel)
    {
        var messages = new LogTarget();

        var log = new LoggerConfiguration()
            .Enrich
            .WithExceptionDetails()
            .MinimumLevel
            .Is(_mappingsToSerilog[minimumLogLevel])
            .WriteTo
            .Sink(messages)
            .CreateLogger();

        return (log, messages);
    }

    /// <summary>A Serilog sink that captures emitted log events for assertions.</summary>
    private sealed class LogTarget : ILogEventSink, IMockLogTarget
    {
        /// <summary>The formatter used to render captured log events.</summary>
        private static readonly MessageTemplateTextFormatter _formatter = new("{Message} {Exception}", CultureInfo.InvariantCulture);

        /// <summary>The captured log entries.</summary>
        private readonly List<(LogLevel logLevel, string message)> _logs = [];

        /// <inheritdoc/>
        public ICollection<(LogLevel logLevel, string message)> Logs => _logs;

        /// <inheritdoc/>
        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();
            var logLevel = _mappingsToSplat[logEvent.Level];
            _formatter.Format(logEvent, buffer);
            var message = buffer.ToString();

            _logs.Add((logLevel, message));
        }
    }
}
