// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;
using Exceptionless.Models;
using Exceptionless.Plugins;

using Splat.Exceptionless;
using Splat.Tests.Mocks;

namespace Splat.Tests.Logging.WrappingFullLoggers;

/// <summary>Unit tests for the Exceptionless Logger.</summary>
[InheritsTests]
public class ExceptionlessLoggerTests : FullLoggerTestBase
{
    /// <summary>Maps each Exceptionless log level to the equivalent Splat <see cref="LogLevel"/>.</summary>
    private static readonly Dictionary<global::Exceptionless.Logging.LogLevel, LogLevel> _exceptionless2Splat = new()
    {
        { global::Exceptionless.Logging.LogLevel.Debug, LogLevel.Debug },
        { global::Exceptionless.Logging.LogLevel.Info, LogLevel.Info },
        { global::Exceptionless.Logging.LogLevel.Warn, LogLevel.Warn },
        { global::Exceptionless.Logging.LogLevel.Error, LogLevel.Error },
        { global::Exceptionless.Logging.LogLevel.Fatal, LogLevel.Fatal },
    };

    /// <summary>Maps each Splat <see cref="LogLevel"/> to the equivalent Exceptionless log level.</summary>
    private static readonly Dictionary<LogLevel, global::Exceptionless.Logging.LogLevel> _splat2Exceptionless = new()
    {
        { LogLevel.Debug, global::Exceptionless.Logging.LogLevel.Debug },
        { LogLevel.Info, global::Exceptionless.Logging.LogLevel.Info },
        { LogLevel.Warn, global::Exceptionless.Logging.LogLevel.Warn },
        { LogLevel.Error, global::Exceptionless.Logging.LogLevel.Error },
        { LogLevel.Fatal, global::Exceptionless.Logging.LogLevel.Fatal },
    };

    /// <inheritdoc/>
    protected override (IFullLogger logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel)
    {
        var logTarget = new InMemoryExceptionlessLogTarget();

        var exceptionlessClient = new ExceptionlessClient();
        exceptionlessClient.Configuration.ApiKey = "someapikey";

        var exceptionlessMinLogLevel = _splat2Exceptionless[minimumLogLevel];
        exceptionlessClient.Configuration.UseInMemoryLogger(exceptionlessMinLogLevel);
        exceptionlessClient.Configuration.SetDefaultMinLogLevel(exceptionlessMinLogLevel);
        exceptionlessClient.Configuration.AddPlugin("Use Mock Log Target Interceptor", context => PluginAction(context, logTarget));
        var inner = new ExceptionlessSplatLogger(typeof(ExceptionlessLoggerTests), exceptionlessClient);
        return (new WrappingFullLogger(inner), logTarget);
    }

    /// <summary>Maps an Exceptionless log level to a Splat log level.</summary>
    /// <param name="logLevel">The Exceptionless log level.</param>
    /// <returns>The equivalent Splat log level.</returns>
    private static LogLevel GetSplatLogLevel(global::Exceptionless.Logging.LogLevel logLevel) => _exceptionless2Splat[logLevel];

    /// <summary>Intercepts an Exceptionless event and records it in the mock log target.</summary>
    /// <param name="obj">The event plugin context.</param>
    /// <param name="logTarget">The in-memory log target to record the event in.</param>
    private static void PluginAction(EventPluginContext obj, InMemoryExceptionlessLogTarget logTarget)
    {
        obj.Cancel = true;
        if (!obj.Event.Type.Equals(Event.KnownTypes.Log, StringComparison.Ordinal))
        {
            return;
        }

        var level = obj.Event.Data.GetValueOrDefault(Event.KnownDataKeys.Level) as string;

        var logLevel = global::Exceptionless.Logging.LogLevel.FromString(level!);

        var splatLogLevel = GetSplatLogLevel(logLevel);
        var exception = obj.ContextData.HasException() ? obj.ContextData.GetException() : null;

        var exceptionMessageSuffix = exception is not null ? $" {exception}" : string.Empty;
        (LogLevel logLevel, string message) tuple = (splatLogLevel, obj.Event.Message + exceptionMessageSuffix);
        logTarget.Logs.Add(tuple);
    }

    /// <summary>An in-memory <see cref="IMockLogTarget"/> that captures Exceptionless log events.</summary>
    private sealed class InMemoryExceptionlessLogTarget : IMockLogTarget
    {
        /// <summary>Initializes a new instance of the <see cref="InMemoryExceptionlessLogTarget"/> class.</summary>
        public InMemoryExceptionlessLogTarget() => Logs = [];

        /// <inheritdoc />
        public ICollection<(LogLevel logLevel, string message)> Logs { get; }
    }
}
#endif
