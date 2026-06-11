// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.LoggerTestConstants;

namespace Splat.Tests.Logging;

/// <summary>Unit tests for the <see cref="WrappingPrefixLogger"/> class.</summary>
public class WrappingPrefixLoggerTests
{
    /// <summary>Gets the new line characters for the current environment.</summary>
    private static char[] NewLine => Environment.NewLine.ToCharArray();

    /// <summary>Test to make sure the message writes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Write_Message()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Write(TestMessage, LogLevel.Debug);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Write(TestMessage, typeof(DummyObjectClass1), LogLevel.Debug);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Debug<DummyObjectClass1>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Debug<DummyObjectClass2>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Info<DummyObjectClass1>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Info<DummyObjectClass2>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Warn<DummyObjectClass1>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Warn<DummyObjectClass2>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Error<DummyObjectClass1>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Error<DummyObjectClass2>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Fatal<DummyObjectClass1>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test to make sure the generic type parameter is passed to the logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Fatal<DummyObjectClass2>(TestMessage);

        await Assert.That(textLogger.Logs.Last().message.Trim(NewLine).Trim()).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }
}
