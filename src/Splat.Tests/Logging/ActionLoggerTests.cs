// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests associated with the <see cref="ActionLogger"/> class.
/// </summary>
[TestFixture]
public class ActionLoggerTests
{
    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Test]
    public void Write_Should_Emit_Message()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;

        var logger = new ActionLogger(
            (message, level) =>
            {
                passedMessage = message;
                passedLevel = level;
            },
            null!,
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Write("This is a test.", LogLevel.Debug);

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Debug));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Debug_With_Generic_Type_Should_Emit_Message_And_Type()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Debug<DummyObjectClass1>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Debug));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass1)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Debug_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Debug<DummyObjectClass2>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Debug));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass2)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Info_With_Generic_Type_Should_Emit_Message_And_Type()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Info<DummyObjectClass1>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Info));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass1)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Info_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Info<DummyObjectClass2>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Info));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass2)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Warn_With_Generic_Type_Should_Emit_Message_And_Type()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Warn<DummyObjectClass1>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Warn));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass1)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Warn_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Warn<DummyObjectClass2>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Warn));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass2)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Error_With_Generic_Type_Should_Emit_Message_And_Type()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Error<DummyObjectClass1>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Error));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass1)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Error_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Error<DummyObjectClass2>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Error));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass2)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Fatal_With_Generic_Type_Should_Emit_Message_And_Type()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Fatal<DummyObjectClass1>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Fatal));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass1)));
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Test]
    public void Fatal_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
    {
        string? passedMessage = null;
        LogLevel? passedLevel = null;
        Type? passedType = null;

        var logger = new ActionLogger(
            null!,
            (message, type, level) =>
            {
                passedMessage = message;
                passedType = type;
                passedLevel = level;
            },
            null!,
            null!);

        var fullLogger = new WrappingFullLogger(logger);

        fullLogger.Fatal<DummyObjectClass2>("This is a test.");

        Assert.That(passedMessage, Is.EqualTo("This is a test."));
        Assert.That(passedLevel, Is.EqualTo(LogLevel.Fatal));
        Assert.That(passedType, Is.EqualTo(typeof(DummyObjectClass2)));
    }
}
