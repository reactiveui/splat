// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Splat;
using Splat.Exceptionless;
using Splat.Log4Net;
using Splat.Microsoft.Extensions.Logging;
using Splat.NLog;
using Splat.Serilog;

namespace DispatchAdapters;

/// <summary>Reports a delayed collection through each logging adapter.</summary>
internal static partial class DispatchLogs
{
    /// <summary>The dispatch event written to each local log.</summary>
    private const string DelayMessage = "Carrier collection delayed";

    /// <summary>Checks the message received by each provider.</summary>
    internal static void CheckProviders()
    {
        CheckMicrosoftAndSerilog();
        CheckNLog();
        CheckLog4Net();
        CheckExceptionless();
    }

    /// <summary>Sends a Microsoft log through Splat to an in-memory Serilog sink.</summary>
    private static void CheckMicrosoftAndSerilog()
    {
        var sink = new DelaySink();
        using var backend = new LoggerConfiguration().WriteTo.Sink(sink).CreateLogger();
        using var resolver = new ModernDependencyResolver();
        using var scope = resolver.WithResolver();
        resolver.UseSerilogFullLogger(backend);
        using var factory = LoggerFactory.Create(static builder => builder.AddSplat());
        LogCollectionDelay(factory.CreateLogger(nameof(DispatchLogs)));
        CheckMessage(sink.Message);
    }

    /// <summary>Writes the dispatch warning through NLog to a memory target.</summary>
    private static void CheckNLog()
    {
        using var factory = new NLog.LogFactory();
        using var target = new MemoryTarget("dispatch") { Layout = "${message}" };
        var config = new LoggingConfiguration();
        config.AddRuleForAllLevels(target);
        factory.Configuration = config;
        using var logger = new NLogLogger(factory.GetLogger(nameof(DispatchLogs)));
        logger.Warn(DelayMessage);
        CheckMessage(target.Logs[0]);
    }

    /// <summary>Writes through log4net using a repository local to this dispatch desk.</summary>
    private static void CheckLog4Net()
    {
        var repository = new log4net.Repository.Hierarchy.Hierarchy();
        var events = new log4net.Appender.MemoryAppender();
        try
        {
            _ = log4net.Config.BasicConfigurator.Configure(repository, events);
            using var logger = new Log4NetLogger(new log4net.Core.LogImpl(repository.GetLogger(nameof(DispatchLogs))));
            logger.Write(DelayMessage, Splat.LogLevel.Warn);
            CheckMessage(events.GetEvents()[0].RenderedMessage);
        }
        finally
        {
            repository.Shutdown();
        }
    }

    /// <summary>Captures the Exceptionless event before its transport sends it.</summary>
    private static void CheckExceptionless()
    {
        const int LocalKeyBytes = 16;
        using var client = new ExceptionlessClient();
        client.Configuration.ApiKey = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(LocalKeyBytes));
        string? message = null;
        client.Configuration.AddPlugin("Local dispatch log", context =>
        {
            message = context.Event.Message;
            context.Cancel = true;
        });
        using var resolver = new ModernDependencyResolver();
        resolver.UseExceptionlessWithWrappingFullLogger(client);
        var manager = resolver.GetService<ILogManager>() ?? throw new InvalidOperationException("Register the Exceptionless log manager.");
        manager.GetLogger(typeof(DispatchLogs)).Warn(DelayMessage);
        CheckMessage(message);
        const string View = "Delivery exceptions";
        new ExceptionlessViewTracking(client).OnViewNavigation(View);
        if (message == View)
        {
            return;
        }

        throw new InvalidOperationException("The delivery exceptions view should appear in the local event log.");
    }

    /// <summary>Checks the warning received by a dispatch log.</summary>
    /// <param name="message">The message captured by the provider.</param>
    /// <exception cref="InvalidOperationException">The provider did not receive the dispatch warning.</exception>
    private static void CheckMessage(string? message)
    {
        if (message?.Contains(DelayMessage, StringComparison.Ordinal) == true)
        {
            return;
        }

        throw new InvalidOperationException("The log did not include the carrier delay.");
    }

    /// <summary>Writes a collection delay using the Microsoft logging API.</summary>
    /// <param name="logger">The logger that forwards into Splat.</param>
    [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Warning, Message = DelayMessage)]
    private static partial void LogCollectionDelay(Microsoft.Extensions.Logging.ILogger logger);

    /// <summary>Records the most recent warning written by Serilog.</summary>
    private sealed class DelaySink : ILogEventSink
    {
        /// <summary>Gets the text of the received warning.</summary>
        internal string? Message { get; private set; }

        /// <inheritdoc/>
        public void Emit(LogEvent logEvent) => Message = logEvent.RenderMessage(System.Globalization.CultureInfo.InvariantCulture);
    }
}
