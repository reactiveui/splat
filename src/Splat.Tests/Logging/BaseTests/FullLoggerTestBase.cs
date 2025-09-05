﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
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
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs, Is.Empty);
            Assert.That(invoked, Is.False);
        }
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
            Assert.That(invoked, Is.True);
        }
    }

    /// <summary>
    /// Test to make sure the Info emits nothing when not enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs, Is.Empty);
            Assert.That(invoked, Is.False);
        }
    }

    /// <summary>
    /// Test to make sure the Info emits something when enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
            Assert.That(invoked, Is.True);
        }
    }

    /// <summary>
    /// Test to make sure the Warn emits nothing when not enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs, Is.Empty);
            Assert.That(invoked, Is.False);
        }
    }

    /// <summary>
    /// Test to make sure the Warn emits something when enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
            Assert.That(invoked, Is.True);
        }
    }

    /// <summary>
    /// Test to make sure the Error emits nothing when not enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs, Is.Empty);
            Assert.That(invoked, Is.False);
        }
    }

    /// <summary>
    /// Test to make sure the Error emits something when enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
            Assert.That(invoked, Is.True);
        }
    }

    /// <summary>
    /// Test to make sure the Fatal emits something when enabled.
    /// </summary>
    [Test]
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
            Assert.That(invoked, Is.True);
        }
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Debug_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Debug_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass2>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Info_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass1>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Info_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass2>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Warn_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass1>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Warn_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass2>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Error_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass1>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Error_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass2>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Fatal_With_Generic_Type_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass1>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the message is passed to the logger.
    /// </summary>
    [Test]
    public void Fatal_With_Generic_Type_Should_Write_Message_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass2>("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
    public void Debug_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug(CultureInfo.InvariantCulture, "This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Debug_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Debug_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug(new DummyObjectClass1());

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo(typeof(DummyObjectClass1).ToString()));
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
    public void Info_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info(CultureInfo.InvariantCulture, "This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Info_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info(new DummyObjectClass1());

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo(typeof(DummyObjectClass1).ToString()));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Info_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Info("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
    public void Warn_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn(CultureInfo.InvariantCulture, "This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Warn_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Warn_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Warn(new DummyObjectClass1());

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo(typeof(DummyObjectClass1).ToString()));
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
    public void Error_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error(CultureInfo.InvariantCulture, "This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Error_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Error_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Error(new DummyObjectClass1());

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo(typeof(DummyObjectClass1).ToString()));
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    [Test]
    public void Fatal_Enabled_FormatProvider_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal(CultureInfo.InvariantCulture, "This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Fatal_Object_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal(new DummyObjectClass1());

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo(typeof(DummyObjectClass1).ToString()));
    }

    /// <summary>
    /// Test to make sure that when enabled debug emits values.
    /// </summary>
    [Test]
    public void Fatal_Message_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Fatal("This is a test.");

        Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim(), Is.EqualTo("This is a test."));
    }
}
