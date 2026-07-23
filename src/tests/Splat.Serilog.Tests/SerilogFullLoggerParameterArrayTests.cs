// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Serilog.Events;

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.LoggerTestConstants;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests the <c>params object[]</c> message overloads of <see cref="SerilogFullLogger"/> for every level: the
/// plain <c>Level(message, params object[])</c> form and the generic <c>Level&lt;T&gt;(message, params object[])</c>
/// form that scopes the entry to a calling type. Both forward the composite message and arguments to Serilog for
/// rendering.
/// </summary>
public class SerilogFullLoggerParameterArrayTests
{
    /// <summary>Verifies the Debug <c>params object[]</c> overloads forward their rendered message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Debug_ParameterArray_Overloads_Forward_Message() =>
        AssertParameterArrayAsync(
            static logger => logger.Debug,
            static logger => logger.Debug<DummyObjectClass1>,
            LogEventLevel.Debug);

    /// <summary>Verifies the Info <c>params object[]</c> overloads forward their rendered message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Info_ParameterArray_Overloads_Forward_Message() =>
        AssertParameterArrayAsync(
            static logger => logger.Info,
            static logger => logger.Info<DummyObjectClass1>,
            LogEventLevel.Information);

    /// <summary>Verifies the Warn <c>params object[]</c> overloads forward their rendered message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Warn_ParameterArray_Overloads_Forward_Message() =>
        AssertParameterArrayAsync(
            static logger => logger.Warn,
            static logger => logger.Warn<DummyObjectClass1>,
            LogEventLevel.Warning);

    /// <summary>Verifies the Error <c>params object[]</c> overloads forward their rendered message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Error_ParameterArray_Overloads_Forward_Message() =>
        AssertParameterArrayAsync(
            static logger => logger.Error,
            static logger => logger.Error<DummyObjectClass1>,
            LogEventLevel.Error);

    /// <summary>Verifies the Fatal <c>params object[]</c> overloads forward their rendered message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Fatal_ParameterArray_Overloads_Forward_Message() =>
        AssertParameterArrayAsync(
            static logger => logger.Fatal,
            static logger => logger.Fatal<DummyObjectClass1>,
            LogEventLevel.Fatal);

    /// <summary>Asserts that both <c>params object[]</c> overloads of a level render the composite message. The
    /// overloads are supplied as method-group bound delegates so the culture-agnostic (no format provider) form is
    /// selected without steering overload resolution toward the format-provider overload.</summary>
    /// <param name="messageOverload">Binds the plain <c>Level(message, params object[])</c> overload.</param>
    /// <param name="contextOverload">Binds the generic <c>Level&lt;T&gt;(message, params object[])</c> overload.</param>
    /// <param name="expectedLevel">The Serilog level the overloads are expected to emit at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertParameterArrayAsync(
        Func<SerilogFullLogger, Action<string, object[]>> messageOverload,
        Func<SerilogFullLogger, Action<string, object[]>> contextOverload,
        LogEventLevel expectedLevel)
    {
        var (logger, sink) = RecordingSerilogSink.Create();

        var logMessage = messageOverload(logger);
        logMessage(Format2, [Arg1, Arg2]);
        var messageOutput = sink.LastMessage;
        var messageLevel = sink.LastLevel;

        var logWithContext = contextOverload(logger);
        logWithContext(Format1, [Arg1]);
        var contextOutput = sink.LastMessage;
        var contextLevel = sink.LastLevel;

        using (Assert.Multiple())
        {
            await Assert.That(messageOutput).IsEqualTo(Expected2);
            await Assert.That(messageLevel).IsEqualTo(expectedLevel);
            await Assert.That(contextOutput).IsEqualTo(Expected1);
            await Assert.That(contextLevel).IsEqualTo(expectedLevel);
        }
    }
}
