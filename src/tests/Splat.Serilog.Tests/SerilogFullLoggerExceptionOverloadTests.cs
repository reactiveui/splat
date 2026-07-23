// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Serilog.Events;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests the exception-carrying overloads of <see cref="SerilogFullLogger"/> for every level: the
/// <c>Level(Exception, message)</c> and <c>LevelException(message, Exception)</c> pair (which fall back from an
/// explicit message to the exception's message and finally to an empty string) and their deferred
/// <see cref="Func{TResult}"/> counterparts (which honour the level's enablement before invoking the callback).
/// </summary>
public class SerilogFullLoggerExceptionOverloadTests
{
    /// <summary>The message of the exception forwarded to the logger.</summary>
    private const string ExceptionMessage = "recorded failure";

    /// <summary>The explicit message passed alongside the exception.</summary>
    private const string ExplicitMessage = "explicit log message";

    /// <summary>The message produced by the deferred callback overloads.</summary>
    private const string DeferredMessage = "deferred log message";

    /// <summary>The exception forwarded to the logger by every test.</summary>
    private static readonly Exception _exception = new InvalidOperationException(ExceptionMessage);

    /// <summary>Verifies the Debug <c>(Exception, message)</c> overload and its message fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Debug_ExceptionMessage_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.Debug(exception!, message), LogEventLevel.Debug);

    /// <summary>Verifies the Info <c>(Exception, message)</c> overload and its message fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Info_ExceptionMessage_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.Info(exception!, message), LogEventLevel.Information);

    /// <summary>Verifies the Warn <c>(Exception, message)</c> overload and its message fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Warn_ExceptionMessage_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.Warn(exception!, message), LogEventLevel.Warning);

    /// <summary>Verifies the Error <c>(Exception, message)</c> overload and its message fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Error_ExceptionMessage_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.Error(exception!, message), LogEventLevel.Error);

    /// <summary>Verifies the Fatal <c>(Exception, message)</c> overload and its message fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Fatal_ExceptionMessage_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.Fatal(exception!, message), LogEventLevel.Fatal);

    /// <summary>Verifies the <see cref="SerilogFullLogger.DebugException(string, Exception)"/> overload fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task DebugException_Message_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.DebugException(message, exception!), LogEventLevel.Debug);

    /// <summary>Verifies the <see cref="SerilogFullLogger.InfoException(string, Exception)"/> overload fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task InfoException_Message_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.InfoException(message, exception!), LogEventLevel.Information);

    /// <summary>Verifies the <see cref="SerilogFullLogger.WarnException(string, Exception)"/> overload fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task WarnException_Message_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.WarnException(message, exception!), LogEventLevel.Warning);

    /// <summary>Verifies the <see cref="SerilogFullLogger.ErrorException(string, Exception)"/> overload fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task ErrorException_Message_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.ErrorException(message, exception!), LogEventLevel.Error);

    /// <summary>Verifies the <see cref="SerilogFullLogger.FatalException(string, Exception)"/> overload fallbacks.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task FatalException_Message_FallsBackFromMessageToExceptionToEmpty() =>
        AssertMessageFallbackAsync(static (logger, exception, message) => logger.FatalException(message, exception!), LogEventLevel.Fatal);

    /// <summary>Verifies the Debug deferred <c>(Exception, Func)</c> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Debug_ExceptionFunction_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.Debug(exception, function), LogEventLevel.Debug);

    /// <summary>Verifies the Info deferred <c>(Exception, Func)</c> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Info_ExceptionFunction_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.Info(exception, function), LogEventLevel.Information);

    /// <summary>Verifies the Warn deferred <c>(Exception, Func)</c> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Warn_ExceptionFunction_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.Warn(exception, function), LogEventLevel.Warning);

    /// <summary>Verifies the Error deferred <c>(Exception, Func)</c> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Error_ExceptionFunction_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.Error(exception, function), LogEventLevel.Error);

    /// <summary>Verifies the Fatal deferred <c>(Exception, Func)</c> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task Fatal_ExceptionFunction_RespectsEnablement() =>
        AssertExceptionFunctionAsync(RecordingSerilogSink.CreateWithAllLevelsDisabled, static (logger, exception, function) => logger.Fatal(exception, function), LogEventLevel.Fatal);

    /// <summary>Verifies the <see cref="SerilogFullLogger.DebugException(Func{string}, Exception)"/> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task DebugException_Function_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.DebugException(function, exception), LogEventLevel.Debug);

    /// <summary>Verifies the <see cref="SerilogFullLogger.InfoException(Func{string}, Exception)"/> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task InfoException_Function_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.InfoException(function, exception), LogEventLevel.Information);

    /// <summary>Verifies the <see cref="SerilogFullLogger.WarnException(Func{string}, Exception)"/> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task WarnException_Function_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.WarnException(function, exception), LogEventLevel.Warning);

    /// <summary>Verifies the <see cref="SerilogFullLogger.ErrorException(Func{string}, Exception)"/> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task ErrorException_Function_RespectsEnablement() =>
        AssertExceptionFunctionAsync(DisabledBelowFatal, static (logger, exception, function) => logger.ErrorException(function, exception), LogEventLevel.Error);

    /// <summary>Verifies the <see cref="SerilogFullLogger.FatalException(Func{string}, Exception)"/> overload honours enablement.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public Task FatalException_Function_RespectsEnablement() =>
        AssertExceptionFunctionAsync(RecordingSerilogSink.CreateWithAllLevelsDisabled, static (logger, exception, function) => logger.FatalException(function, exception), LogEventLevel.Fatal);

    /// <summary>Verifies the deferred <c>Debug(Exception, Func)</c> overload rejects a null callback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_ExceptionFunction_NullFunction_Throws()
    {
        var (logger, _) = RecordingSerilogSink.Create();

        await Assert.That(() => logger.Debug(_exception, (Func<string>)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies the deferred <c>DebugException(Func, Exception)</c> overload rejects a null callback.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Function_NullFunction_Throws()
    {
        var (logger, _) = RecordingSerilogSink.Create();

        await Assert.That(() => logger.DebugException((Func<string>)null!, _exception)).Throws<ArgumentNullException>();
    }

    /// <summary>Creates a logger that has every level below Fatal disabled.</summary>
    /// <returns>The full logger and the sink recording its output.</returns>
    private static (SerilogFullLogger Logger, RecordingSerilogSink Sink) DisabledBelowFatal() =>
        RecordingSerilogSink.CreateWithMinimumLevel(LogEventLevel.Fatal);

    /// <summary>Asserts that an <c>(Exception, message)</c>-shaped overload renders the message, then the
    /// exception's message when the message is null, then an empty string when both are null.</summary>
    /// <param name="log">Invokes the overload under test with a logger, exception, and message.</param>
    /// <param name="expectedLevel">The Serilog level the overload is expected to emit at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertMessageFallbackAsync(Action<SerilogFullLogger, Exception?, string?> log, LogEventLevel expectedLevel)
    {
        var (logger, sink) = RecordingSerilogSink.Create();

        log(logger, _exception, ExplicitMessage);
        var explicitMessage = sink.LastMessage;
        var explicitException = sink.LastException;
        var explicitLevel = sink.LastLevel;

        log(logger, _exception, null);
        var exceptionFallback = sink.LastMessage;

        log(logger, null, null);
        var emptyFallback = sink.LastMessage;
        var emptyException = sink.LastException;

        using (Assert.Multiple())
        {
            await Assert.That(explicitMessage).IsEqualTo(ExplicitMessage);
            await Assert.That(explicitException).IsEqualTo(_exception);
            await Assert.That(explicitLevel).IsEqualTo(expectedLevel);
            await Assert.That(exceptionFallback).IsEqualTo(ExceptionMessage);
            await Assert.That(emptyFallback).IsEqualTo(string.Empty);
            await Assert.That(emptyException).IsNull();
        }
    }

    /// <summary>Asserts that a deferred <see cref="Func{TResult}"/> exception overload invokes the callback and
    /// forwards its message and exception when enabled, and skips both when the level is disabled.</summary>
    /// <param name="createDisabled">Creates a logger on which the overload's level is disabled.</param>
    /// <param name="log">Invokes the overload under test with a logger, exception, and callback.</param>
    /// <param name="expectedLevel">The Serilog level the overload is expected to emit at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertExceptionFunctionAsync(
        Func<(SerilogFullLogger Logger, RecordingSerilogSink Sink)> createDisabled,
        Action<SerilogFullLogger, Exception, Func<string>> log,
        LogEventLevel expectedLevel)
    {
        var (enabledLogger, enabledSink) = RecordingSerilogSink.Create();
        var enabledInvoked = false;
        log(enabledLogger, _exception, () =>
        {
            enabledInvoked = true;
            return DeferredMessage;
        });

        var (disabledLogger, disabledSink) = createDisabled();
        var disabledInvoked = false;
        log(disabledLogger, _exception, () =>
        {
            disabledInvoked = true;
            return DeferredMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(enabledInvoked).IsTrue();
            await Assert.That(enabledSink.LastMessage).IsEqualTo(DeferredMessage);
            await Assert.That(enabledSink.LastException).IsEqualTo(_exception);
            await Assert.That(enabledSink.LastLevel).IsEqualTo(expectedLevel);
            await Assert.That(disabledInvoked).IsFalse();
            await Assert.That(disabledSink.Count).IsEqualTo(0);
        }
    }
}
