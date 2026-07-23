// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.LoggerTestConstants;
using static Splat.Tests.Logging.WrappingFullLoggerTestFactory;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <c>params object[]</c> composite-format overloads of <see cref="WrappingFullLogger"/>
/// (<c>Debug/Info/Warn/Error/Fatal(string, params object[])</c> and their generic variants). Each formats the message
/// with <see cref="System.Globalization.CultureInfo.InvariantCulture"/> and forwards the rendered text to the wrapped
/// <see cref="ILogger"/> at the matching level.
/// </summary>
public class WrappingFullLoggerParameterizedMessageTests
{
    /// <summary>The single-argument set forwarded to the composite-format overloads.</summary>
    private static readonly object[] _singleArgument = [Arg1];

    /// <summary>Test that Debug renders its arguments and forwards the text at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        ((Action<string, object[]>)logger.Debug)(Format1, _singleArgument);

        await AssertLast(target, LogLevel.Debug);
    }

    /// <summary>Test that the generic Debug renders its arguments and forwards the text at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Generic_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>(Format1, Arg1);

        await AssertLast(target, LogLevel.Debug);
    }

    /// <summary>Test that Info renders its arguments and forwards the text at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);

        ((Action<string, object[]>)logger.Info)(Format1, _singleArgument);

        await AssertLast(target, LogLevel.Info);
    }

    /// <summary>Test that the generic Info renders its arguments and forwards the text at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Generic_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);

        logger.Info<DummyObjectClass1>(Format1, Arg1);

        await AssertLast(target, LogLevel.Info);
    }

    /// <summary>Test that Warn renders its arguments and forwards the text at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);

        ((Action<string, object[]>)logger.Warn)(Format1, _singleArgument);

        await AssertLast(target, LogLevel.Warn);
    }

    /// <summary>Test that the generic Warn renders its arguments and forwards the text at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Generic_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);

        logger.Warn<DummyObjectClass1>(Format1, Arg1);

        await AssertLast(target, LogLevel.Warn);
    }

    /// <summary>Test that Error renders its arguments and forwards the text at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);

        ((Action<string, object[]>)logger.Error)(Format1, _singleArgument);

        await AssertLast(target, LogLevel.Error);
    }

    /// <summary>Test that the generic Error renders its arguments and forwards the text at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Generic_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);

        logger.Error<DummyObjectClass1>(Format1, Arg1);

        await AssertLast(target, LogLevel.Error);
    }

    /// <summary>Test that Fatal renders its arguments and forwards the text at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        ((Action<string, object[]>)logger.Fatal)(Format1, _singleArgument);

        await AssertLast(target, LogLevel.Fatal);
    }

    /// <summary>Test that the generic Fatal renders its arguments and forwards the text at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Generic_Params_Forwards_Rendered_Text()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Fatal<DummyObjectClass1>(Format1, Arg1);

        await AssertLast(target, LogLevel.Fatal);
    }

    /// <summary>Asserts the most recent captured entry rendered the single argument at the expected level.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="expectedLevel">The level the entry should have been logged at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertLast(TextLogger target, LogLevel expectedLevel)
    {
        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(Expected1);
            await Assert.That(entry.logLevel).IsEqualTo(expectedLevel);
        }
    }
}
