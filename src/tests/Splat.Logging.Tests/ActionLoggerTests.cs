// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

public class ActionLoggerTests
{
    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Should_Emit_Message()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Emit_Message_And_Type()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Debug);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass1));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Debug);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass2));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Emit_Message_And_Type()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Info);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass1));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Info);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass2));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Emit_Message_And_Type()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Warn);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass1));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Warn);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass2));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Emit_Message_And_Type()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Error);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass1));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Error);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass2));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Emit_Message_And_Type()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Fatal);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass1));
        }
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_Generic_Type_Should_Emit_Message_And_Type_Provided()
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

        using (Assert.Multiple())
        {
            await Assert.That(passedMessage).IsEqualTo("This is a test.");
            await Assert.That(passedLevel).IsEqualTo(LogLevel.Fatal);
            await Assert.That(passedType).IsEqualTo(typeof(DummyObjectClass2));
        }
    }
}
