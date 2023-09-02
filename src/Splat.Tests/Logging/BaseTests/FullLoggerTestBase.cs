// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// A base class for testing full loggers that are available.
/// </summary>
public abstract class FullLoggerTestBase : AllocationFreeLoggerBaseTestBase<IFullLogger>
{
    /// <summary>
    /// Test to make sure the debug emits nothing when not enabled.
    /// </summary>
    [Fact]
    public void Debug_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Debug<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Empty(target.Logs);
        Assert.False(invoked);
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Debug_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Debug<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
        Assert.True(invoked);
    }

    /// <summary>
    /// Test to make sure the Info emits nothing when not enabled.
    /// </summary>
    [Fact]
    public void Info_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Info<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Empty(target.Logs);
        Assert.False(invoked);
    }

    /// <summary>
    /// Test to make sure the Info emits something when enabled.
    /// </summary>
    [Fact]
    public void Info_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Info<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
        Assert.True(invoked);
    }

    /// <summary>
    /// Test to make sure the Warn emits nothing when not enabled.
    /// </summary>
    [Fact]
    public void Warn_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Warn<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Empty(target.Logs);
        Assert.False(invoked);
    }

    /// <summary>
    /// Test to make sure the Warn emits something when enabled.
    /// </summary>
    [Fact]
    public void Warn_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Warn<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
        Assert.True(invoked);
    }

    /// <summary>
    /// Test to make sure the Error emits nothing when not enabled.
    /// </summary>
    [Fact]
    public void Error_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Error<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Empty(target.Logs);
        Assert.False(invoked);
    }

    /// <summary>
    /// Test to make sure the Error emits something when enabled.
    /// </summary>
    [Fact]
    public void Error_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Error<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
        Assert.True(invoked);
    }

    /// <summary>
    /// Test to make sure the Fatal emits something when enabled.
    /// </summary>
    [Fact]
    public void Fatal_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Fatal<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
        Assert.True(invoked);
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass2>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass2>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass2>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass2>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass2>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Debug_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug(CultureInfo.InvariantCulture, "This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Debug_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Debug_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug(new DummyObjectClass1());

        Assert.Equal(typeof(DummyObjectClass1).ToString(), target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Info_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info(CultureInfo.InvariantCulture, "This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Info_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info(new DummyObjectClass1());

        Assert.Equal(typeof(DummyObjectClass1).ToString(), target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Info_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Warn_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn(CultureInfo.InvariantCulture, "This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Warn_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Warn_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn(new DummyObjectClass1());

        Assert.Equal(typeof(DummyObjectClass1).ToString(), target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Error_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error(CultureInfo.InvariantCulture, "This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Error_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Error_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error(new DummyObjectClass1());

        Assert.Equal(typeof(DummyObjectClass1).ToString(), target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Fact]
    public void Fatal_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal(CultureInfo.InvariantCulture, "This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Fatal_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal(new DummyObjectClass1());

        Assert.Equal(typeof(DummyObjectClass1).ToString(), target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Fact]
    public void Fatal_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }
}
