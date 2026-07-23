// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.WrappingFullLoggerTestFactory;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the deferred (<see cref="Func{TResult}"/>) overloads of <see cref="WrappingFullLogger"/> that also carry an
/// exception (<c>Debug/Info/Warn/Error/Fatal(Exception, Func&lt;string&gt;)</c> and
/// <c>DebugException/... (Func&lt;string&gt;, Exception)</c>), plus the disabled path of the plain Fatal deferred
/// overloads. When the level is enabled the message factory is invoked and forwarded; when disabled the factory is never
/// invoked and nothing is emitted.
/// </summary>
public class WrappingFullLoggerDeferredMessageTests
{
    /// <summary>The text produced by the deferred message factory.</summary>
    private const string DeferredMessage = "deferred-detail";

    /// <summary>The exception instance forwarded through the deferred exception overloads.</summary>
    private static readonly Exception _exception = new InvalidOperationException("wrapped-failure");

    /// <summary>Test that Debug invokes the factory and forwards the exception at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);
        var invoked = false;

        logger.Debug(_exception, () => Factory(ref invoked));

        await AssertEmitted(target, LogLevel.Debug, invoked);
    }

    /// <summary>Test that a disabled Debug level neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Exception_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Debug(_exception, () => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that DebugException invokes the factory and forwards the exception at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);
        var invoked = false;

        logger.DebugException(() => Factory(ref invoked), _exception);

        await AssertEmitted(target, LogLevel.Debug, invoked);
    }

    /// <summary>Test that a disabled DebugException neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DebugException_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.DebugException(() => Factory(ref invoked), _exception);

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that Info invokes the factory and forwards the exception at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);
        var invoked = false;

        logger.Info(_exception, () => Factory(ref invoked));

        await AssertEmitted(target, LogLevel.Info, invoked);
    }

    /// <summary>Test that a disabled Info level neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Exception_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Info(_exception, () => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that InfoException invokes the factory and forwards the exception at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);
        var invoked = false;

        logger.InfoException(() => Factory(ref invoked), _exception);

        await AssertEmitted(target, LogLevel.Info, invoked);
    }

    /// <summary>Test that a disabled InfoException neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task InfoException_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.InfoException(() => Factory(ref invoked), _exception);

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that Warn invokes the factory and forwards the exception at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);
        var invoked = false;

        logger.Warn(_exception, () => Factory(ref invoked));

        await AssertEmitted(target, LogLevel.Warn, invoked);
    }

    /// <summary>Test that a disabled Warn level neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Exception_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Warn(_exception, () => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that WarnException invokes the factory and forwards the exception at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);
        var invoked = false;

        logger.WarnException(() => Factory(ref invoked), _exception);

        await AssertEmitted(target, LogLevel.Warn, invoked);
    }

    /// <summary>Test that a disabled WarnException neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WarnException_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.WarnException(() => Factory(ref invoked), _exception);

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that Error invokes the factory and forwards the exception at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);
        var invoked = false;

        logger.Error(_exception, () => Factory(ref invoked));

        await AssertEmitted(target, LogLevel.Error, invoked);
    }

    /// <summary>Test that a disabled Error level neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Exception_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Error(_exception, () => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that ErrorException invokes the factory and forwards the exception at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);
        var invoked = false;

        logger.ErrorException(() => Factory(ref invoked), _exception);

        await AssertEmitted(target, LogLevel.Error, invoked);
    }

    /// <summary>Test that a disabled ErrorException neither invokes the factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ErrorException_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.ErrorException(() => Factory(ref invoked), _exception);

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that Fatal invokes the factory and forwards the exception at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.Fatal(_exception, () => Factory(ref invoked));

        await AssertEmitted(target, LogLevel.Fatal, invoked);
    }

    /// <summary>Test that a level above Fatal neither invokes the exception factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Exception_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(AboveFatal);
        var invoked = false;

        logger.Fatal(_exception, () => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that FatalException invokes the factory and forwards the exception at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Func_Enabled_Emits()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);
        var invoked = false;

        logger.FatalException(() => Factory(ref invoked), _exception);

        await AssertEmitted(target, LogLevel.Fatal, invoked);
    }

    /// <summary>Test that a level above Fatal neither invokes the FatalException factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FatalException_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(AboveFatal);
        var invoked = false;

        logger.FatalException(() => Factory(ref invoked), _exception);

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that a level above Fatal neither invokes the plain Fatal factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(AboveFatal);
        var invoked = false;

        logger.Fatal(() => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Test that a level above Fatal neither invokes the generic Fatal factory nor emits.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Generic_Func_Disabled_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(AboveFatal);
        var invoked = false;

        logger.Fatal<DummyObjectClass1>(() => Factory(ref invoked));

        await AssertSuppressed(target, invoked);
    }

    /// <summary>Records that the deferred factory ran and returns the deferred message.</summary>
    /// <param name="invoked">Set to <see langword="true"/> when the factory is evaluated.</param>
    /// <returns>The deferred message.</returns>
    private static string Factory(ref bool invoked)
    {
        invoked = true;
        return DeferredMessage;
    }

    /// <summary>Asserts the most recent captured entry carries the deferred message at the expected level.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="expectedLevel">The level the entry should have been logged at.</param>
    /// <param name="invoked">Whether the deferred factory was evaluated.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertEmitted(TextLogger target, LogLevel expectedLevel, bool invoked)
    {
        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).Contains(DeferredMessage);
            await Assert.That(entry.logLevel).IsEqualTo(expectedLevel);
            await Assert.That(invoked).IsTrue();
        }
    }

    /// <summary>Asserts nothing was emitted and the deferred factory was not evaluated.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="invoked">Whether the deferred factory was evaluated.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertSuppressed(TextLogger target, bool invoked)
    {
        using (Assert.Multiple())
        {
            await Assert.That(target.Logs).IsEmpty();
            await Assert.That(invoked).IsFalse();
        }
    }
}
