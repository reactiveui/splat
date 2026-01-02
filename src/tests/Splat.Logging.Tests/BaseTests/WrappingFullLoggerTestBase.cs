// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the wrapping full logger.
/// </summary>
[InheritsTests]
public abstract class WrappingFullLoggerTestBase : AllocationFreeLoggerBaseTestBase<IFullLogger>
{
    /// <summary>
    /// Test to make sure the debug emits nothing when not enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Warn<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).IsEmpty();
            await Assert.That(invoked).IsFalse();
        }
    }

    /// <summary>
    /// Test to make sure the debug emits something when enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Debug<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>
    /// Test to make sure the Info emits nothing when not enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Info<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).IsEmpty();
            await Assert.That(invoked).IsFalse();
        }
    }

    /// <summary>
    /// Test to make sure the Info emits something when enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Info<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>
    /// Test to make sure the Warn emits nothing when not enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Warn<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).IsEmpty();
            await Assert.That(invoked).IsFalse();
        }
    }

    /// <summary>
    /// Test to make sure the Warn emits something when enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Warn<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>
    /// Test to make sure the Error emits nothing when not enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Disabled_Should_Not_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Error<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).IsEmpty();
            await Assert.That(invoked).IsFalse();
        }
    }

    /// <summary>
    /// Test to make sure the Error emits something when enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        var invoked = false;

        logger.Error<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>
    /// Test to make sure the Fatal emits something when enabled.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Enabled_Should_Emit()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Fatal<DummyObjectClass1>(
            () =>
            {
                invoked = true;
                return "This is a test.";
            });

        using (Assert.Multiple())
        {
            await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass1>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("This is a test.", LogLevel.Debug);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Debug<DummyObjectClass2>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass1>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Info<DummyObjectClass2>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass1>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Warn<DummyObjectClass2>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass1>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Error<DummyObjectClass2>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass1>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Fatal<DummyObjectClass2>("This is a test.");

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }
}
