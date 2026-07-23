// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

using static Splat.Tests.Logging.LoggerTestConstants;

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

    /// <summary>Test that the Debug one/two/three-argument format-provider overloads forward the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Debug_With_FormatProvider_Arguments_Forwards_Formatted_Text() =>
        AssertFormatProviderArgumentsAsync(
            static logger => logger.Debug(CultureInfo.InvariantCulture, Format1, Arg1),
            static logger => logger.Debug(CultureInfo.InvariantCulture, Format2, Arg1, Arg2),
            static logger => logger.Debug(CultureInfo.InvariantCulture, Format3, Arg1, Arg2, Arg3));

    /// <summary>Test that the Info one/two/three-argument format-provider overloads forward the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Info_With_FormatProvider_Arguments_Forwards_Formatted_Text() =>
        AssertFormatProviderArgumentsAsync(
            static logger => logger.Info(CultureInfo.InvariantCulture, Format1, Arg1),
            static logger => logger.Info(CultureInfo.InvariantCulture, Format2, Arg1, Arg2),
            static logger => logger.Info(CultureInfo.InvariantCulture, Format3, Arg1, Arg2, Arg3));

    /// <summary>Test that the Warn one/two/three-argument format-provider overloads forward the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Warn_With_FormatProvider_Arguments_Forwards_Formatted_Text() =>
        AssertFormatProviderArgumentsAsync(
            static logger => logger.Warn(CultureInfo.InvariantCulture, Format1, Arg1),
            static logger => logger.Warn(CultureInfo.InvariantCulture, Format2, Arg1, Arg2),
            static logger => logger.Warn(CultureInfo.InvariantCulture, Format3, Arg1, Arg2, Arg3));

    /// <summary>Test that the Error one/two/three-argument format-provider overloads forward the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Error_With_FormatProvider_Arguments_Forwards_Formatted_Text() =>
        AssertFormatProviderArgumentsAsync(
            static logger => logger.Error(CultureInfo.InvariantCulture, Format1, Arg1),
            static logger => logger.Error(CultureInfo.InvariantCulture, Format2, Arg1, Arg2),
            static logger => logger.Error(CultureInfo.InvariantCulture, Format3, Arg1, Arg2, Arg3));

    /// <summary>Test that the Fatal one/two/three-argument format-provider overloads forward the formatted text.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Fatal_With_FormatProvider_Arguments_Forwards_Formatted_Text() =>
        AssertFormatProviderArgumentsAsync(
            static logger => logger.Fatal(CultureInfo.InvariantCulture, Format1, Arg1),
            static logger => logger.Fatal(CultureInfo.InvariantCulture, Format2, Arg1, Arg2),
            static logger => logger.Fatal(CultureInfo.InvariantCulture, Format3, Arg1, Arg2, Arg3));

    /// <summary>Asserts that the one, two, and three-argument format-provider overloads of a level each render
    /// the composite format string against <see cref="CultureInfo.InvariantCulture"/>.</summary>
    /// <param name="logOneArgument">Invokes the single-argument format-provider overload.</param>
    /// <param name="logTwoArguments">Invokes the two-argument format-provider overload.</param>
    /// <param name="logThreeArguments">Invokes the three-argument format-provider overload.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertFormatProviderArgumentsAsync(
        Action<SerilogFullLogger> logOneArgument,
        Action<SerilogFullLogger> logTwoArguments,
        Action<SerilogFullLogger> logThreeArguments)
    {
        var (logger, sink) = CreateLogger();

        logOneArgument(logger);
        var oneArgument = sink.LastMessage;

        logTwoArguments(logger);
        var twoArguments = sink.LastMessage;

        logThreeArguments(logger);
        var threeArguments = sink.LastMessage;

        using (Assert.Multiple())
        {
            await Assert.That(oneArgument).IsEqualTo(Expected1);
            await Assert.That(twoArguments).IsEqualTo(Expected2);
            await Assert.That(threeArguments).IsEqualTo(Expected3);
        }
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
