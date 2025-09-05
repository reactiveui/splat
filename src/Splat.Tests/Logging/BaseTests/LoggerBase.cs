// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Contains common tests associated with all loggers.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
[TestFixture]
public abstract class LoggerBase<T>
    where T : ILogger
{
    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Logger_Level_Debug_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Debug);

        Assert.That(logger.Level, Is.EqualTo(LogLevel.Debug));
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Logger_Level_Info_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Info);

        Assert.That(logger.Level, Is.EqualTo(LogLevel.Info));
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Logger_Level_Warn_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Warn);

        Assert.That(logger.Level, Is.EqualTo(LogLevel.Warn));
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Logger_Level_Error_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Error);

        Assert.That(logger.Level, Is.EqualTo(LogLevel.Error));
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Logger_Level_Fatal_Should_Be_correct()
    {
        var (logger, _) = GetLogger(LogLevel.Fatal);

        Assert.That(logger.Level, Is.EqualTo(LogLevel.Fatal));
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);

        logger.Write("This is a test.", LogLevel.Debug);

        Assert.That(target.Logs.Last(, Is.EqualTo("This is a test.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Info_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);

        logger.Write("This is a test.", LogLevel.Info);

        Assert.That(target.Logs.Last(, Is.EqualTo("This is a test.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Warn_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);

        logger.Write("This is a test.", LogLevel.Warn);

        Assert.That(target.Logs.Last(, Is.EqualTo("This is a test.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Error_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);

        logger.Write("This is a test.", LogLevel.Error);

        Assert.That(target.Logs.Last(, Is.EqualTo("This is a test.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Fatal_Write_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);

        logger.Write("This is a test.", LogLevel.Fatal);

        Assert.That(target.Logs.Last(, Is.EqualTo("This is a test.")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Debug);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Debug);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Info);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Info);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Warn);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Warn);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Error);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Error);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void FatalException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", LogLevel.Fatal);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Debug);
        Assert.That(target.Logs.Last(, Is.EqualTo("1")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Debug);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Info);
        Assert.That(target.Logs.Last(, Is.EqualTo("1")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Info);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Warn);
        Assert.That(target.Logs.Last(, Is.EqualTo("1")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Warn);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Error);
        Assert.That(target.Logs.Last(, Is.EqualTo("1")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorType_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Error);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void FatalType_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write("1", typeof(DummyObjectClass1), LogLevel.Fatal);
        Assert.That(target.Logs.Last(, Is.EqualTo("1")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Debug);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Debug);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void DebugTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Debug);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Info);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Info);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void InfoTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Info);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Warn);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Warn);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void WarnTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Warn);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Error);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Error);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void ErrorTypeException_Should_Not_Write_If_Higher_Level()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Error);
        Assert.That(target.Logs, Is.Empty);
    }

    /// <summary>
    /// Tests that the inner logger writes correctly.
    /// </summary>
    [Test]
    public void FatalTypeException_Should_Write_Message()
    {
        var (logger, target) = GetLogger(LogLevel.Fatal);
        logger.Write(FormatHelper.Exception, "1", typeof(DummyObjectClass1), LogLevel.Fatal);
        Assert.That(target.Logs.Last(, Is.EqualTo($"1 {FormatHelper.Exception}")).message.Trim(FormatHelper.NewLine).Trim());
    }

    /// <summary>
    /// Gets the logger to test.
    /// </summary>
    /// <param name="minimumLogLevel">The minimum log level.</param>
    /// <returns>The logger.</returns>
    protected abstract (T logger, IMockLogTarget mockTarget) GetLogger(LogLevel minimumLogLevel);
}
