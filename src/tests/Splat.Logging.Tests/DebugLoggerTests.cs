// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <see cref="DebugLogger"/> class.
/// </summary>
public class DebugLoggerTests
{
    /// <summary>
    /// Test that DebugLogger writes to System.Diagnostics.Debug.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate usage of Exception for testing")]
    public async Task Write_Should_Write_To_Debug_Output()
    {
        var logger = new DebugLogger { Level = LogLevel.Debug };

        // DebugLogger writes to System.Diagnostics.Debug which we can't easily capture
        // So we just verify it doesn't throw
        await Assert.That(() =>
        {
            logger.Write("Test message", LogLevel.Debug);
            logger.Write("Test message", typeof(DebugLoggerTests), LogLevel.Info);
            logger.Write(new Exception("Test"), "Test exception", LogLevel.Error);
            logger.Write(new Exception("Test"), "Test exception", typeof(DebugLoggerTests), LogLevel.Fatal);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that Level property works.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Should_Be_Settable()
    {
        var logger = new DebugLogger { Level = LogLevel.Warn };

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);

        logger.Level = LogLevel.Error;
        await Assert.That(logger.Level).IsEqualTo(LogLevel.Error);
    }
}
