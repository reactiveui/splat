// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Contains common tests associated with all loggers.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
[NotInParallel]
public abstract class LoggerBase<T>
    where T : ILogger
{
    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logger_Level_Debug_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Debug);
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logger_Level_Info_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Info);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Info);
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logger_Level_Warn_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logger_Level_Error_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Error);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Error);
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logger_Level_Fatal_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Fatal);

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Fatal);
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("This is a test.", LogLevel.Debug);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Write("This is a test.", LogLevel.Info);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Write("This is a test.", LogLevel.Warn);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Write("This is a test.", LogLevel.Error);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Write("This is a test.", LogLevel.Fatal);

        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("This is a test.");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Debug);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Debug);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Info);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Info);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Warn);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Warn);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Error);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Error);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Fatal);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Debug);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Debug);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Info);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Info);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Warn);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Warn);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Error);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Error);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Fatal);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo("1");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Debug);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Debug);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Info);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Info);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Warn);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Warn);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Error);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Error);
        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Fatal);
        await Assert.That(target.Logs.Last().message.Trim(FormatHelper.NewLine).Trim()).IsEqualTo($"1 {FormatHelper.Exception}");
    }

    /// <summary>
    /// Gets the logger to test.
    /// </summary>
    /// <param name="minimumLogLevel">The minimum log level.</param>
    /// <returns>The logger.</returns>
    protected abstract (T logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel);
}
