// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests the error based allocation free logging.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
[TestFixture]
public abstract class AllocateFreeErrorLoggerTestBase<T> : LoggerBase<T>
    where T : IAllocationFreeErrorLogger
{
    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("1 System.Exception: Exception of type 'System.Exception' was thrown."));
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void DebugExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs.Last(, Is.EqualTo("1 System.Exception: Exception of type 'System.Exception' was thrown.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void InfoExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs.Last(, Is.EqualTo("1 System.Exception: Exception of type 'System.Exception' was thrown.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void WarnExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs.Last(, Is.EqualTo("1 System.Exception: Exception of type 'System.Exception' was thrown.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void ErrorExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}", 1);
        Assert.That(target.Logs.Last(, Is.EqualTo("1 System.Exception: Exception of type 'System.Exception' was thrown.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}", 1, 2);
        Assert.That(2 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}", 1, 2, 3);
        Assert.That(2, 3 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
        Assert.That(2, 3, 4 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
        Assert.That(2, 3, 4, 5 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
        Assert.That(2, 3, 4, 5, 6 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
        Assert.That(2, 3, 4, 5, 6, 7 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
        Assert.That(2, 3, 4, 5, 6, 7, 8 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests the inner logger writes three arguments.
    /// </summary>
    [Test]
    public void FatalExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        Assert.That(2, 3, 4, 5, 6, 7, 8, 9, 10 System.Exception: Exception of type 'System.Exception' was thrown.", target.Logs.Last(, Is.EqualTo("1)).message.Trim(FormatHelper.NewLine).Trim());
    }
}
