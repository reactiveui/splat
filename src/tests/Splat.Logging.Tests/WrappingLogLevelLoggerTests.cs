// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <see cref="WrappingLogLevelLogger"/> class.
/// </summary>
public class WrappingLogLevelLoggerTests
{
    /// <summary>
    /// Test that WrappingLogLevelLogger adds level prefix to messages.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Add_LogLevel_Prefix()
    {
        var inner = new TextLogger { Level = LogLevel.Debug };
        var logger = new WrappingLogLevelLogger(inner);

        logger.Write("Test message", LogLevel.Debug);
        logger.Write("Info message", LogLevel.Info);
        logger.Write("Warn message", LogLevel.Warn);

        var logsList = inner.Logs.ToList();

        using (Assert.Multiple())
        {
            await Assert.That(logsList).Count().IsEqualTo(3);
            await Assert.That(logsList[0].message).IsEqualTo("Debug: Test message");
            await Assert.That(logsList[1].message).IsEqualTo("Info: Info message");
            await Assert.That(logsList[2].message).IsEqualTo("Warn: Warn message");
        }
    }

    /// <summary>
    /// Test that Level property is forwarded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Should_Be_Forwarded_From_Inner()
    {
        var inner = new TextLogger { Level = LogLevel.Error };
        var logger = new WrappingLogLevelLogger(inner);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Error);
    }
}
