// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="ActionLogger"/> class.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate usage of Exception for testing")]
public class ActionLoggerCoverageTests
{
    /// <summary>The message written and asserted on by the logger tests.</summary>
    private const string TestMessage = "This is a test.";

    /// <summary>Test that the Level property is settable and gettable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_Should_Be_Settable()
    {
        var logger = new ActionLogger(null!, null!, null!, null!) { Level = LogLevel.Warn };

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);
    }

    /// <summary>Test that the simple Write overload invokes the no-type action.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_Invokes_NoType_Action()
    {
        string? message = null;
        LogLevel? level = null;
        var logger = new ActionLogger(
            (m, l) =>
            {
                message = m;
                level = l;
            },
            null!,
            null!,
            null!);

        logger.Write(TestMessage, LogLevel.Info);

        using (Assert.Multiple())
        {
            await Assert.That(message).IsEqualTo(TestMessage);
            await Assert.That(level).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Test that the typed Write overload invokes the with-type action.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Message_With_Type_Invokes_WithType_Action()
    {
        string? message = null;
        Type? type = null;
        LogLevel? level = null;
        var logger = new ActionLogger(
            null!,
            (m, t, l) =>
            {
                message = m;
                type = t;
                level = l;
            },
            null!,
            null!);

        logger.Write(TestMessage, typeof(ActionLoggerCoverageTests), LogLevel.Warn);

        using (Assert.Multiple())
        {
            await Assert.That(message).IsEqualTo(TestMessage);
            await Assert.That(type).IsEqualTo(typeof(ActionLoggerCoverageTests));
            await Assert.That(level).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Test that the exception Write overload invokes the no-type-with-exception action.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_Invokes_NoType_Exception_Action()
    {
        Exception? captured = null;
        string? message = null;
        LogLevel? level = null;
        var expected = new Exception("boom");
        var logger = new ActionLogger(
            null!,
            null!,
            (e, m, l) =>
            {
                captured = e;
                message = m;
                level = l;
            },
            null!);

        logger.Write(expected, TestMessage, LogLevel.Error);

        using (Assert.Multiple())
        {
            await Assert.That(captured).IsSameReferenceAs(expected);
            await Assert.That(message).IsEqualTo(TestMessage);
            await Assert.That(level).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Test that the typed exception Write overload invokes the with-type-and-exception action.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_Exception_Message_With_Type_Invokes_WithType_Exception_Action()
    {
        Exception? captured = null;
        string? message = null;
        Type? type = null;
        LogLevel? level = null;
        var expected = new Exception("boom");
        var logger = new ActionLogger(
            null!,
            null!,
            null!,
            (e, m, t, l) =>
            {
                captured = e;
                message = m;
                type = t;
                level = l;
            });

        logger.Write(expected, TestMessage, typeof(ActionLoggerCoverageTests), LogLevel.Fatal);

        using (Assert.Multiple())
        {
            await Assert.That(captured).IsSameReferenceAs(expected);
            await Assert.That(message).IsEqualTo(TestMessage);
            await Assert.That(type).IsEqualTo(typeof(ActionLoggerCoverageTests));
            await Assert.That(level).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Test that Write overloads do not throw when the backing actions are null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_With_Null_Actions_Should_Not_Throw()
    {
        var logger = new ActionLogger(null!, null!, null!, null!);

        await Assert.That(() =>
        {
            logger.Write(TestMessage, LogLevel.Debug);
            logger.Write(TestMessage, typeof(ActionLoggerCoverageTests), LogLevel.Debug);
            logger.Write(new("boom"), TestMessage, LogLevel.Debug);
            logger.Write(new("boom"), TestMessage, typeof(ActionLoggerCoverageTests), LogLevel.Debug);
        }).ThrowsNothing();
    }
}
