// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests that check the functionality of the <see cref="AllocationFreeLoggerBase"/> class.
/// </summary>
/// <typeparam name="T">The type of logger to test.</typeparam>
[TestFixture]
public abstract class AllocationFreeLoggerBaseTestBase<T> : AllocateFreeErrorLoggerTestBase<T>
    where T : IAllocationFreeLogger
{
    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}", 1, 2);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void DebugSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7"));
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void DebugSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Test]
    public void DebugEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8"));
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void DebugEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void DebugNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9"));
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void DebugNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void DebugTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10"));
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void DebugTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}", 1, 2);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void InfoSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7"));
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void InfoSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Test]
    public void InfoEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8"));
    }

    /// <summary>
    /// Tests the inner logger writes eight arguments.
    /// </summary>
    [Test]
    public void InfoEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void InfoNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9"));
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void InfoNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void InfoTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10"));
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void InfoTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}", 1, 2);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void WarnSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7"));
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void WarnSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void WarnEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8"));
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void WarnEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void WarnNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9"));
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void WarnNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void WarnTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10"));
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void WarnTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}", 1, 2);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void ErrorSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7"));
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void ErrorSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void ErrorEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8"));
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void ErrorEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void ErrorNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9"));
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void ErrorNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void ErrorTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10"));
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void ErrorTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}", 1, 2);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5"));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6"));
    }

    /// <summary>
    /// Tests the inner logger writes seven arguments.
    /// </summary>
    [Test]
    public void FatalSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7"));
    }

    /// <summary>
    /// Tests the inner logger writes eighth arguments.
    /// </summary>
    [Test]
    public void FatalEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8"));
    }

    /// <summary>
    /// Tests the inner logger writes nine arguments.
    /// </summary>
    [Test]
    public void FatalNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9"));
    }

    /// <summary>
    /// Tests the inner logger writes ten arguments.
    /// </summary>
    [Test]
    public void FatalTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1, 2, 3, 4, 5, 6, 7, 8, 9, 10"));
    }
}
