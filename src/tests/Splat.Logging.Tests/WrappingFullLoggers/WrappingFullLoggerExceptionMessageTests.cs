// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.WrappingFullLoggerTestFactory;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the exception-with-message overloads of <see cref="WrappingFullLogger"/>
/// (<c>Debug/Info/Warn/Error/Fatal(Exception, string?)</c> and <c>DebugException/... (string?, Exception)</c>). These
/// forward the exception together with the caller message at the matching level, substituting the null placeholder for
/// a <see langword="null"/> message.
/// </summary>
public class WrappingFullLoggerExceptionMessageTests
{
    /// <summary>The caller message forwarded alongside the exception.</summary>
    private const string ExceptionMessage = "explosion-detail";

    /// <summary>The text substituted for a <see langword="null"/> message.</summary>
    private const string NullPlaceholder = "(null)";

    /// <summary>The exception instance forwarded through the exception overloads.</summary>
    private static readonly Exception _exception = new InvalidOperationException("wrapped-failure");

    /// <summary>Test that Debug forwards the exception and message at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Message_Forwards_At_Debug()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug(_exception, ExceptionMessage);

        await AssertContains(target, ExceptionMessage, LogLevel.Debug);
    }

    /// <summary>Test that Debug substitutes the placeholder for a null exception message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug(_exception, (string?)null);

        await AssertContains(target, NullPlaceholder, LogLevel.Debug);
    }

    /// <summary>Test that DebugException forwards the message and exception at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Forwards_At_Debug()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.DebugException(ExceptionMessage, _exception);

        await AssertContains(target, ExceptionMessage, LogLevel.Debug);
    }

    /// <summary>Test that Info forwards the exception and message at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Message_Forwards_At_Info()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Info(_exception, ExceptionMessage);

        await AssertContains(target, ExceptionMessage, LogLevel.Info);
    }

    /// <summary>Test that Info substitutes the placeholder for a null exception message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Info(_exception, (string?)null);

        await AssertContains(target, NullPlaceholder, LogLevel.Info);
    }

    /// <summary>Test that InfoException forwards the message and exception at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Forwards_At_Info()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.InfoException(ExceptionMessage, _exception);

        await AssertContains(target, ExceptionMessage, LogLevel.Info);
    }

    /// <summary>Test that Warn forwards the exception and message at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Message_Forwards_At_Warn()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Warn(_exception, ExceptionMessage);

        await AssertContains(target, ExceptionMessage, LogLevel.Warn);
    }

    /// <summary>Test that Warn substitutes the placeholder for a null exception message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Warn(_exception, (string?)null);

        await AssertContains(target, NullPlaceholder, LogLevel.Warn);
    }

    /// <summary>Test that WarnException forwards the message and exception at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Forwards_At_Warn()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.WarnException(ExceptionMessage, _exception);

        await AssertContains(target, ExceptionMessage, LogLevel.Warn);
    }

    /// <summary>Test that Error forwards the exception and message at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Message_Forwards_At_Error()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Error(_exception, ExceptionMessage);

        await AssertContains(target, ExceptionMessage, LogLevel.Error);
    }

    /// <summary>Test that Error substitutes the placeholder for a null exception message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Error(_exception, (string?)null);

        await AssertContains(target, NullPlaceholder, LogLevel.Error);
    }

    /// <summary>Test that ErrorException forwards the message and exception at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Forwards_At_Error()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.ErrorException(ExceptionMessage, _exception);

        await AssertContains(target, ExceptionMessage, LogLevel.Error);
    }

    /// <summary>Test that Fatal forwards the exception and message at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Message_Forwards_At_Fatal()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Fatal(_exception, ExceptionMessage);

        await AssertContains(target, ExceptionMessage, LogLevel.Fatal);
    }

    /// <summary>Test that Fatal substitutes the placeholder for a null exception message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Fatal(_exception, (string?)null);

        await AssertContains(target, NullPlaceholder, LogLevel.Fatal);
    }

    /// <summary>Test that FatalException forwards the message and exception at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Forwards_At_Fatal()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.FatalException(ExceptionMessage, _exception);

        await AssertContains(target, ExceptionMessage, LogLevel.Fatal);
    }

    /// <summary>Asserts the most recent captured entry contains the expected text at the expected level.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="expectedText">Text the captured message must contain.</param>
    /// <param name="expectedLevel">The level the entry should have been logged at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertContains(TextLogger target, string expectedText, LogLevel expectedLevel)
    {
        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).Contains(expectedText);
            await Assert.That(entry.logLevel).IsEqualTo(expectedLevel);
        }
    }
}
