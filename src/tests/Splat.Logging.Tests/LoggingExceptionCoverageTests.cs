// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>Coverage tests for the <see cref="LoggingException"/> class.</summary>
public class LoggingExceptionCoverageTests
{
    /// <summary>Test that the parameterless constructor produces an exception with no inner exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Default_Constructor_Should_Create_Exception()
    {
        var exception = new LoggingException();

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsNotNull();
            await Assert.That(exception.InnerException).IsNull();
        }
    }

    /// <summary>Test that the message constructor sets the message and leaves no inner exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Message_Constructor_Should_Set_Message_And_No_Inner()
    {
        const string message = "logging failed";
        var exception = new LoggingException(message);

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).IsEqualTo(message);
            await Assert.That(exception.InnerException).IsNull();
        }
    }

    /// <summary>Test that the message and inner constructor sets both values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Message_And_Inner_Constructor_Should_Set_Both()
    {
        const string message = "logging failed";
        var inner = new InvalidOperationException("inner");
        var exception = new LoggingException(message, inner);

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).IsEqualTo(message);
            await Assert.That(exception.InnerException).IsSameReferenceAs(inner);
        }
    }
}
