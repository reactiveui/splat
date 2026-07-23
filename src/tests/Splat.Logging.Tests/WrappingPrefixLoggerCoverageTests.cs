// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="WrappingPrefixLogger"/> class.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate usage of Exception for testing")]
public class WrappingPrefixLoggerCoverageTests
{
    /// <summary>The message written and asserted on by the logger tests.</summary>
    private const string TestMessage = "This is a test.";

    /// <summary>Test that the Level property reflects the inner logger's level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Should_Reflect_Inner_Logger_Level()
    {
        var inner = new TextLogger { Level = LogLevel.Warn };
        var logger = new WrappingPrefixLogger(inner, typeof(DummyObjectClass1));

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>Test that the simple Write overload applies the calling type prefix.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_Should_Apply_Prefix()
    {
        var inner = new TextLogger();
        var logger = new WrappingPrefixLogger(inner, typeof(DummyObjectClass1));

        logger.Write(TestMessage, LogLevel.Debug);

        await Assert.That(inner.Logs.Last().message).IsEqualTo($"{nameof(DummyObjectClass1)}: This is a test.");
    }

    /// <summary>Test that the exception Write overload applies the calling type prefix.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_Should_Apply_Prefix()
    {
        var inner = new TextLogger();
        var logger = new WrappingPrefixLogger(inner, typeof(DummyObjectClass1));

        logger.Write(new("boom"), TestMessage, LogLevel.Error);

        await Assert.That(inner.Logs.Last().message.StartsWith($"{nameof(DummyObjectClass1)}: This is a test.", StringComparison.Ordinal)).IsTrue();
    }

    /// <summary>Test that the typed Write overload uses the supplied type name as prefix.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_With_Type_Should_Use_Type_Prefix()
    {
        var inner = new TextLogger();
        var logger = new WrappingPrefixLogger(inner, typeof(DummyObjectClass1));

        logger.Write(TestMessage, typeof(DummyObjectClass2), LogLevel.Info);

        await Assert.That(inner.Logs.Last().message).IsEqualTo($"{nameof(DummyObjectClass2)}: This is a test.");
    }

    /// <summary>Test that the typed exception Write overload uses the supplied type name as prefix.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_With_Type_Should_Use_Type_Prefix()
    {
        var inner = new TextLogger();
        var logger = new WrappingPrefixLogger(inner, typeof(DummyObjectClass1));

        logger.Write(new("boom"), TestMessage, typeof(DummyObjectClass2), LogLevel.Fatal);

        await Assert.That(inner.Logs.Last().message.StartsWith($"{nameof(DummyObjectClass2)}: This is a test.", StringComparison.Ordinal)).IsTrue();
    }

    /// <summary>Test that the typed Write overload throws when the type argument is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_With_Null_Type_Throws()
    {
        var logger = new WrappingPrefixLogger(new TextLogger(), typeof(DummyObjectClass1));

        await Assert.That(() => logger.Write(TestMessage, null!, LogLevel.Info)).Throws<ArgumentNullException>();
    }

    /// <summary>Test that the typed exception Write overload throws when the type argument is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_With_Null_Type_Throws()
    {
        var logger = new WrappingPrefixLogger(new TextLogger(), typeof(DummyObjectClass1));

        await Assert.That(() => logger.Write(new("boom"), TestMessage, null!, LogLevel.Info)).Throws<ArgumentNullException>();
    }
}
