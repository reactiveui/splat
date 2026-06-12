// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="DebugLogger"/> class.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate usage of Exception for testing")]
public class DebugLoggerCoverageTests
{
    /// <summary>A sample debug-level message used across the Write overload tests.</summary>
    private const string DebugMessage = "debug";
    /// <summary>A sample message used across the Write overload tests.</summary>
    private const string SampleMessage = "message";

    /// <summary>Test that the Level property is settable and reads back the value that was set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Is_Settable()
    {
        var logger = new DebugLogger { Level = LogLevel.Warn };

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>Test that messages at or above the configured Level do not throw.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_At_Or_Above_Level_Should_Not_Throw()
    {
        var logger = new DebugLogger { Level = LogLevel.Info };

        await Assert.That(() =>
        {
            logger.Write("info", LogLevel.Info);
            logger.Write("warn", LogLevel.Warn);
            logger.Write(new Exception("ex"), "error", LogLevel.Error);
            logger.Write("fatal", typeof(DebugLoggerCoverageTests), LogLevel.Fatal);
            logger.Write(new Exception("ex"), "fatal", typeof(DebugLoggerCoverageTests), LogLevel.Fatal);
        }).ThrowsNothing();
    }

    /// <summary>Test that messages below the configured Level are gated out without throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Below_Level_Should_Be_Gated_Without_Throwing()
    {
        var logger = new DebugLogger { Level = LogLevel.Fatal };

        await Assert.That(() =>
        {
            logger.Write(DebugMessage, LogLevel.Debug);
            logger.Write(new Exception("ex"), DebugMessage, LogLevel.Debug);
            logger.Write(DebugMessage, typeof(DebugLoggerCoverageTests), LogLevel.Info);
            logger.Write(new Exception("ex"), DebugMessage, typeof(DebugLoggerCoverageTests), LogLevel.Warn);
        }).ThrowsNothing();
    }

    /// <summary>Test that each Write overload can be invoked across every log level without throwing.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_All_Overloads_Across_All_Levels_Should_Not_Throw()
    {
        var logger = new DebugLogger { Level = LogLevel.Debug };

        await Assert.That(() =>
        {
            foreach (var level in new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal })
            {
                logger.Write(SampleMessage, level);
                logger.Write(new Exception("ex"), SampleMessage, level);
                logger.Write(SampleMessage, typeof(DebugLoggerCoverageTests), level);
                logger.Write(new Exception("ex"), SampleMessage, typeof(DebugLoggerCoverageTests), level);
            }
        }).ThrowsNothing();
    }
}
