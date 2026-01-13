// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the <see cref="LoggingException"/> class.
/// </summary>
public class LoggingExceptionTests
{
    /// <summary>
    /// Test that LoggingException can be constructed with message.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_With_Message_Should_Set_Message()
    {
        var exception = new LoggingException("Test message");

        await Assert.That(exception.Message).IsEqualTo("Test message");
    }

    /// <summary>
    /// Test that LoggingException can be constructed with message and inner exception.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_With_Message_And_InnerException_Should_Set_Both()
    {
        var inner = new InvalidOperationException("Inner");
        var exception = new LoggingException("Test message", inner);

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).IsEqualTo("Test message");
            await Assert.That(exception.InnerException).IsSameReferenceAs(inner);
        }
    }

    /// <summary>
    /// Test that LoggingException can be thrown and caught.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task LoggingException_Can_Be_Thrown_And_Caught()
    {
        var wasCaught = false;
        try
        {
            throw new LoggingException("Test");
        }
        catch (LoggingException ex)
        {
            wasCaught = true;
            await Assert.That(ex.Message).IsEqualTo("Test");
        }

        await Assert.That(wasCaught).IsTrue();
    }
}
