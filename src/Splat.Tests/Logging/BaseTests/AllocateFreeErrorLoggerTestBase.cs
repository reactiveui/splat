// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests the error based allocation free logging.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
public abstract class AllocateFreeErrorLoggerTestBase<T> : LoggerBase<T>
    where T : IAllocationFreeErrorLogger
{
    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}", 1);
        Assert.Equal("1 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Equal("1, 2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void DebugExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}", 1);
        Assert.Equal("1 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Equal("1, 2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void InfoExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}", 1);
        Assert.Equal("1 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Equal("1, 2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void WarnExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}", 1);
        Assert.Equal("1 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}", 1);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Equal("1, 2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void ErrorExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Empty(target.Logs);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}", 1);
        Assert.Equal("1 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.Equal("1, 2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.Equal("1, 2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.Equal("1, 2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.Equal("1, 2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.Equal("1, 2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Fact]
    public void FatalExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }
}
