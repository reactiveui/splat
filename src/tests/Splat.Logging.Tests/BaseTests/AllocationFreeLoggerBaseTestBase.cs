// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

[InheritsTests]
[NotInParallel]
public abstract class AllocationFreeLoggerBaseTestBase<T> : AllocateFreeErrorLoggerTestBase<T>
    where T : IAllocationFreeLogger
{
    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}", 1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}", 1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}", 1, 2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}", 1, 2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7");
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8");
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9");
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}", 1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}", 1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}", 1, 2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}", 1, 2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7");
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8");
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9");
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}", 1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}", 1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}", 1, 2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}", 1, 2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7");
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8");
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9");
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}", 1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}", 1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}", 1, 2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}", 1, 2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7");
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8");
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9");
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}", 1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}", 1, 2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}", 1, 2, 3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5");
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6");
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7");
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8");
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9");
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
    }
}
