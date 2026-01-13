// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <see cref="NullLogger"/> class.
/// </summary>
public class NullLoggerTests
{
    /// <summary>
    /// Test that NullLogger doesn't throw when writing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate usage of Exception")]
    public async Task Write_Should_Not_Throw()
    {
        var logger = new NullLogger();

        // NullLogger should accept all calls without throwing
        await Assert.That(() =>
        {
            logger.Write("Test message", LogLevel.Debug);
            logger.Write("Test message", typeof(NullLoggerTests), LogLevel.Info);
            logger.Write(new Exception("Test"), "Test exception", LogLevel.Error);
            logger.Write(new Exception("Test"), "Test exception", typeof(NullLoggerTests), LogLevel.Fatal);
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that Level property can be set and retrieved.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Should_Be_Settable()
    {
        var logger = new NullLogger { Level = LogLevel.Debug };

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Debug);

        logger.Level = LogLevel.Error;
        await Assert.That(logger.Level).IsEqualTo(LogLevel.Error);
    }
}
