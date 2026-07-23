// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Splat.Tests.Logging;

/// <summary>
/// A Serilog sink that captures the level, rendered message, and attached exception of each emitted event so
/// tests can assert on what a <see cref="SerilogFullLogger"/> forwarded. Also exposes factory helpers that pair a
/// fresh logger with a fresh sink at a chosen enablement.
/// </summary>
internal sealed class RecordingSerilogSink : ILogEventSink
{
    /// <summary>The Serilog level one step above <see cref="LogEventLevel.Fatal"/>; used to disable every level.</summary>
    private const LogEventLevel AboveFatalLevel = LogEventLevel.Fatal + 1;

    /// <summary>Renders only the message portion of a captured event, without the surrounding quotes Serilog adds.</summary>
    private static readonly MessageTemplateTextFormatter _formatter = new("{Message}", CultureInfo.InvariantCulture);

    /// <summary>Gets the number of events captured so far.</summary>
    internal int Count { get; private set; }

    /// <summary>Gets the rendered message of the most recently captured event, trimmed of surrounding whitespace.</summary>
    internal string? LastMessage { get; private set; }

    /// <summary>Gets the level of the most recently captured event.</summary>
    internal LogEventLevel LastLevel { get; private set; }

    /// <summary>Gets the exception attached to the most recently captured event, if any.</summary>
    internal Exception? LastException { get; private set; }

    /// <inheritdoc/>
    public void Emit(LogEvent logEvent)
    {
        using var buffer = new StringWriter();
        _formatter.Format(logEvent, buffer);
        LastMessage = buffer.ToString().Trim();
        LastLevel = logEvent.Level;
        LastException = logEvent.Exception;
        Count++;
    }

    /// <summary>Creates a logger whose underlying Serilog instance captures every level.</summary>
    /// <returns>The full logger and the sink recording its output.</returns>
    internal static (SerilogFullLogger Logger, RecordingSerilogSink Sink) Create() => CreateWithMinimumLevel(LogEventLevel.Verbose);

    /// <summary>Creates a logger whose underlying Serilog instance is enabled from <paramref name="minimumLevel"/> upward.</summary>
    /// <param name="minimumLevel">The lowest Serilog level that remains enabled.</param>
    /// <returns>The full logger and the sink recording its output.</returns>
    internal static (SerilogFullLogger Logger, RecordingSerilogSink Sink) CreateWithMinimumLevel(LogEventLevel minimumLevel)
    {
        var sink = new RecordingSerilogSink();
        var serilog = new LoggerConfiguration()
            .MinimumLevel
            .Is(minimumLevel)
            .WriteTo
            .Sink(sink)
            .CreateLogger();

        return (new SerilogFullLogger(serilog), sink);
    }

    /// <summary>Creates a logger whose underlying Serilog instance reports every level, including Fatal, as disabled.</summary>
    /// <returns>The full logger and the sink recording its output.</returns>
    internal static (SerilogFullLogger Logger, RecordingSerilogSink Sink) CreateWithAllLevelsDisabled()
    {
        var sink = new RecordingSerilogSink();
        var levelSwitch = new LoggingLevelSwitch(AboveFatalLevel);
        var serilog = new LoggerConfiguration()
            .MinimumLevel
            .ControlledBy(levelSwitch)
            .WriteTo
            .Sink(sink)
            .CreateLogger();

        return (new SerilogFullLogger(serilog), sink);
    }
}
