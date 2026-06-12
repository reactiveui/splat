// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using static Splat.Tests.Logging.LoggerTestConstants;

namespace Splat.Tests.Logging;

/// <summary>Tests the error based allocation free logging.</summary>
/// <typeparam name="T">The type of logger.</typeparam>
[InheritsTests]
[NotInParallel]
public abstract class AllocateFreeErrorLoggerTestBase<T> : LoggerBase<T>
    where T : IAllocationFreeErrorLogger
{
    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException6);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException8);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException4);
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException5);
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException6);
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException8);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Info(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException4);
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException5);
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException6);
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException8);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Warn(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes one argument.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException1);
    }

    /// <summary>Tests the inner logger writes one argument.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes two arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException2);
    }

    /// <summary>Tests the inner logger writes two arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException4);
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException5);
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException6);
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException8);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Error(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes one argument.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException1);
    }

    /// <summary>Tests the inner logger writes two arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException3);
    }

    /// <summary>Tests the inner logger writes four arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException4);
    }

    /// <summary>Tests the inner logger writes five arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException5);
    }

    /// <summary>Tests the inner logger writes six arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException6);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException7);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException8);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException9);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalExceptionTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Fatal(FormatHelper.Exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(ExpectedException10);
    }
}
