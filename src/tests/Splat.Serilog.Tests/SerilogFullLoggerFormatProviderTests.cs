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
/// Tests for the single-value <see cref="IFormatProvider"/> overloads of <see cref="SerilogFullLogger"/>
/// (Debug/Info/Warn/Error/Fatal) and its <c>Level</c> setter. Each overload renders the value through
/// <see cref="string.Format(IFormatProvider, string, object?)"/> with the caller-supplied provider and
/// forwards the resulting literal text to Serilog.
/// </summary>
public class SerilogFullLoggerFormatProviderTests
{
    /// <summary>The value passed to each single-value overload.</summary>
    private const int SampleValue = 42;

    /// <summary>The text the value renders to under <see cref="CultureInfo.InvariantCulture"/>.</summary>
    private const string ExpectedText = "42";

    /// <summary>Test that the Debug single-value format-provider overload forwards the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, sink) = CreateLogger();

        logger.Debug(CultureInfo.InvariantCulture, SampleValue);

        await Assert.That(sink.LastMessage).IsEqualTo(ExpectedText);
    }

    /// <summary>Test that the Info single-value format-provider overload forwards the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, sink) = CreateLogger();

        logger.Info(CultureInfo.InvariantCulture, SampleValue);

        await Assert.That(sink.LastMessage).IsEqualTo(ExpectedText);
    }

    /// <summary>Test that the Warn single-value format-provider overload forwards the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, sink) = CreateLogger();

        logger.Warn(CultureInfo.InvariantCulture, SampleValue);

        await Assert.That(sink.LastMessage).IsEqualTo(ExpectedText);
    }

    /// <summary>Test that the Error single-value format-provider overload forwards the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, sink) = CreateLogger();

        logger.Error(CultureInfo.InvariantCulture, SampleValue);

        await Assert.That(sink.LastMessage).IsEqualTo(ExpectedText);
    }

    /// <summary>Test that the Fatal single-value format-provider overload forwards the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, sink) = CreateLogger();

        logger.Fatal(CultureInfo.InvariantCulture, SampleValue);

        await Assert.That(sink.LastMessage).IsEqualTo(ExpectedText);
    }

    /// <summary>Test that the Level setter is a no-op and does not alter the level reported by the getter.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Setter_Is_A_No_Op()
    {
        var (logger, _) = CreateLogger();
        var before = logger.Level;

        logger.Level = LogLevel.Fatal;

        await Assert.That(logger.Level).IsEqualTo(before);
    }

    /// <summary>Creates a <see cref="SerilogFullLogger"/> writing to a verbose-level capturing sink.</summary>
    /// <returns>The Serilog full logger and the sink capturing its rendered messages.</returns>
    private static (SerilogFullLogger Logger, CapturingSink Sink) CreateLogger()
    {
        var sink = new CapturingSink();
        var serilog = new LoggerConfiguration()
            .MinimumLevel
            .Verbose()
            .WriteTo
            .Sink(sink)
            .CreateLogger();

        return (new SerilogFullLogger(serilog), sink);
    }

    /// <summary>A Serilog sink that records the rendered message of the most recent log event.</summary>
    private sealed class CapturingSink : ILogEventSink
    {
        /// <summary>Renders only the message portion of a captured log event.</summary>
        private static readonly MessageTemplateTextFormatter _formatter = new("{Message}", CultureInfo.InvariantCulture);

        /// <summary>Gets the rendered message of the most recently emitted log event.</summary>
        public string? LastMessage { get; private set; }

        /// <inheritdoc/>
        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();
            _formatter.Format(logEvent, buffer);
            LastMessage = buffer.ToString().Trim();
        }
    }
}
