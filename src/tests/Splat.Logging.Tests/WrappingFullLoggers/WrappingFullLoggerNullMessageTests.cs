// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.WrappingFullLoggerTestFactory;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that the simple message overloads of <see cref="WrappingFullLogger"/>
/// (<c>Debug/Info/Warn/Error/Fatal(string?)</c> and their generic variants) substitute the null placeholder for a
/// <see langword="null"/> message and forward it at the matching level.
/// </summary>
public class WrappingFullLoggerNullMessageTests
{
    /// <summary>The text substituted for a <see langword="null"/> message.</summary>
    private const string NullPlaceholder = "(null)";

    /// <summary>Test that Debug substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug((string?)null);

        await AssertPlaceholder(target, LogLevel.Debug);
    }

    /// <summary>Test that the generic Debug substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Generic_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>((string?)null);

        await AssertPlaceholder(target, LogLevel.Debug);
    }

    /// <summary>Test that Info substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Info((string?)null);

        await AssertPlaceholder(target, LogLevel.Info);
    }

    /// <summary>Test that the generic Info substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Generic_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass1>((string?)null);

        await AssertPlaceholder(target, LogLevel.Info);
    }

    /// <summary>Test that Warn substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Warn((string?)null);

        await AssertPlaceholder(target, LogLevel.Warn);
    }

    /// <summary>Test that the generic Warn substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Generic_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass1>((string?)null);

        await AssertPlaceholder(target, LogLevel.Warn);
    }

    /// <summary>Test that Error substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Error((string?)null);

        await AssertPlaceholder(target, LogLevel.Error);
    }

    /// <summary>Test that the generic Error substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Generic_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass1>((string?)null);

        await AssertPlaceholder(target, LogLevel.Error);
    }

    /// <summary>Test that Fatal substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Fatal((string?)null);

        await AssertPlaceholder(target, LogLevel.Fatal);
    }

    /// <summary>Test that the generic Fatal substitutes the placeholder for a null message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Generic_Null_Message_Forwards_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass1>((string?)null);

        await AssertPlaceholder(target, LogLevel.Fatal);
    }

    /// <summary>Asserts the most recent captured entry is the null placeholder at the expected level.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="expectedLevel">The level the entry should have been logged at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertPlaceholder(TextLogger target, LogLevel expectedLevel)
    {
        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(NullPlaceholder);
            await Assert.That(entry.logLevel).IsEqualTo(expectedLevel);
        }
    }
}
