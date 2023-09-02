// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that check the functionality of the <see cref="AllocationFreeLoggerBase"/> class.
/// </summary>
/// <typeparam name="T">The type of logger to test.</typeparam>
public abstract class AllocationFreeLoggerBaseTestBase<T> : AllocateFreeErrorLoggerTestBase<T>
    where T : IAllocationFreeLogger
{
    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}", 1);
        Assert.Equal("1", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}", 1, 2);
        Assert.Equal("1, 2", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void DebugSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void DebugSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Fact]
    public void DebugEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void DebugEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void DebugNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void DebugNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void DebugTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void DebugTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}", 1);
        Assert.Equal("1", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}", 1, 2);
        Assert.Equal("1, 2", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void InfoSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void InfoSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Fact]
    public void InfoEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Fact]
    public void InfoEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void InfoNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void InfoNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void InfoTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void InfoTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}", 1);
        Assert.Equal("1", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}", 1, 2);
        Assert.Equal("1, 2", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void WarnSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void WarnSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void WarnEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void WarnEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void WarnNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void WarnNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void WarnTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void WarnTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}", 1);
        Assert.Equal("1", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}", 1, 2);
        Assert.Equal("1, 2", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void ErrorSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void ErrorSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void ErrorEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void ErrorEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void ErrorNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void ErrorNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void ErrorTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void ErrorTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}", 1);
        Assert.Equal("1", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}", 1, 2);
        Assert.Equal("1, 2", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Fact]
    public void FatalSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Fact]
    public void FatalEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Fact]
    public void FatalNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Fact]
    public void FatalTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }
}
