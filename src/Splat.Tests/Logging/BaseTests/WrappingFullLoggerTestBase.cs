// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the wrapping full logger.
/// </summary>
public abstract class WrappingFullLoggerTestBase : AllocationFreeLoggerBaseTestBase<IFullLogger>
{
    /// <summary>
    /// Test to make sure the debug emits nothing when not enabled.
    /// </summary>
    [Fact]
    public void Debug_Disabled_Should_Not_Emit()
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
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>("This is a test.");

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Write_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("This is a test.", LogLevel.Debug);

        Assert.Equal("This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim());
    }
}
