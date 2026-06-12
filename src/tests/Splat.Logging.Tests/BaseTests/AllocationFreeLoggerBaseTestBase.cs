// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using static Splat.Tests.Logging.LoggerTestConstants;

namespace Splat.Tests.Logging;

/// <summary>Base test fixture for verifying allocation-free logger implementations.</summary>
/// <typeparam name="T">The type of allocation-free logger under test.</typeparam>
[InheritsTests]
[NotInParallel]
public abstract class AllocationFreeLoggerBaseTestBase<T> : AllocateFreeErrorLoggerTestBase<T>
    where T : IAllocationFreeLogger
{
    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected6);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Debug(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected8);
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Debug(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Debug(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected6);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Info(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected8);
    }

    /// <summary>Tests the inner logger writes eight arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Info(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Info(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected6);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Warn(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected8);
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Warn(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Warn(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorOneArgumentMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format1, Arg1);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format2, Arg1, Arg2);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected6);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected7);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected8);
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected9);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Error(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected10);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Error(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalOneArgumentMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format1, Arg1);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected1);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTwoArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format2, Arg1, Arg2);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected2);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalThreeArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format3, Arg1, Arg2, Arg3);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected3);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalFourArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format4, Arg1, Arg2, Arg3, Arg4);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected4);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalFiveArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected5);
    }

    /// <summary>Tests the inner logger writes three arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalSixArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected6);
    }

    /// <summary>Tests the inner logger writes seven arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalSevenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected7);
    }

    /// <summary>Tests the inner logger writes eighth arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalEightArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected8);
    }

    /// <summary>Tests the inner logger writes nine arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalNineArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected9);
    }

    /// <summary>Tests the inner logger writes ten arguments.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTenArgumentsMethod_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Fatal(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo(Expected10);
    }
}
