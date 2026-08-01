// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.LoggerTestConstants;

namespace Splat.Tests.Logging;

/// <summary>Verifies the typed-argument Fatal overloads honour an inner logger that reports fatal as disabled.</summary>
/// <remarks>
/// Fatal is the highest defined level, so the only way an inner logger can report it disabled is by exposing a level
/// beyond it. A consumer implementing <see cref="ILogger"/> can do exactly that, which is what these overloads guard
/// against before they format anything.
/// </remarks>
[NotInParallel]
public sealed class WrappingFullLoggerFatalSuppressionTests
{
    /// <summary>A level above every defined one, so that even fatal is filtered out.</summary>
    private const LogLevel AboveFatal = LogLevel.Fatal + 1;

    /// <summary>Verifies none of the typed-argument Fatal overloads write when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTypedArguments_WhenFatalIsDisabled_WriteNothing()
    {
        var (logger, target) = CreateSuppressedLogger();

        logger.Fatal(Format1, Arg1);
        logger.Fatal(Format2, Arg1, Arg2);
        logger.Fatal(Format3, Arg1, Arg2, Arg3);
        logger.Fatal(Format4, Arg1, Arg2, Arg3, Arg4);
        logger.Fatal(Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        logger.Fatal(Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        logger.Fatal(Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        logger.Fatal(Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        logger.Fatal(Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        logger.Fatal(Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Verifies none of the exception-carrying Fatal overloads write when fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalTypedArgumentsWithException_WhenFatalIsDisabled_WriteNothing()
    {
        var (logger, target) = CreateSuppressedLogger();
        var exception = new InvalidOperationException("suppressed");

        logger.Fatal(exception, Format1, Arg1);
        logger.Fatal(exception, Format2, Arg1, Arg2);
        logger.Fatal(exception, Format3, Arg1, Arg2, Arg3);
        logger.Fatal(exception, Format4, Arg1, Arg2, Arg3, Arg4);
        logger.Fatal(exception, Format5, Arg1, Arg2, Arg3, Arg4, Arg5);
        logger.Fatal(exception, Format6, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        logger.Fatal(exception, Format7, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        logger.Fatal(exception, Format8, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        logger.Fatal(exception, Format9, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        logger.Fatal(exception, Format10, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Builds a logger whose inner target filters out every level, fatal included.</summary>
    /// <returns>
    /// The logger, typed as <see cref="IAllocationFreeLogger"/> so the calls bind to the typed-argument overloads
    /// rather than the params-array ones the concrete logger also carries, and the target it writes to.
    /// </returns>
    private static (IAllocationFreeLogger Logger, TextLogger Target) CreateSuppressedLogger()
    {
        var target = new TextLogger { Level = AboveFatal };
        return (new WrappingFullLogger(target), target);
    }
}
