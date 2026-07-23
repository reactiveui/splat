// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

using static Splat.Tests.Logging.WrappingFullLoggerTestFactory;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the generic single-value overloads of <see cref="WrappingFullLogger"/>
/// (<c>Debug/Info/Warn/Error/Fatal&lt;T&gt;(T)</c>). These short-circuit when the level is disabled or the value is
/// <see langword="null"/>, and substitute the null placeholder when the value's <see cref="object.ToString"/> returns
/// <see langword="null"/>.
/// </summary>
public class WrappingFullLoggerValueTests
{
    /// <summary>The text substituted when a value's <see cref="object.ToString"/> returns <see langword="null"/>.</summary>
    private const string NullPlaceholder = "(null)";

    /// <summary>Test that a disabled Debug level does not emit a value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Disabled_Does_Not_Emit_Value()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Debug(new DummyObjectClass1());

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Debug does not emit a null value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Null_Value_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug((DummyObjectClass1)null!);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Debug substitutes the placeholder when the value renders to null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_Null_ToString_Emits_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Debug);

        logger.Debug(new NullToStringValue());

        await AssertPlaceholder(target, LogLevel.Debug);
    }

    /// <summary>Test that a disabled Info level does not emit a value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Disabled_Does_Not_Emit_Value()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Info(new DummyObjectClass1());

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Info does not emit a null value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Null_Value_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);

        logger.Info((DummyObjectClass1)null!);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Info substitutes the placeholder when the value renders to null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_Null_ToString_Emits_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Info);

        logger.Info(new NullToStringValue());

        await AssertPlaceholder(target, LogLevel.Info);
    }

    /// <summary>Test that a disabled Warn level does not emit a value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Disabled_Does_Not_Emit_Value()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Warn(new DummyObjectClass1());

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Warn does not emit a null value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Null_Value_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);

        logger.Warn((DummyObjectClass1)null!);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Warn substitutes the placeholder when the value renders to null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_Null_ToString_Emits_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Warn);

        logger.Warn(new NullToStringValue());

        await AssertPlaceholder(target, LogLevel.Warn);
    }

    /// <summary>Test that a disabled Error level does not emit a value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Disabled_Does_Not_Emit_Value()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Error(new DummyObjectClass1());

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Error does not emit a null value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Null_Value_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);

        logger.Error((DummyObjectClass1)null!);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Error substitutes the placeholder when the value renders to null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_Null_ToString_Emits_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Error);

        logger.Error(new NullToStringValue());

        await AssertPlaceholder(target, LogLevel.Error);
    }

    /// <summary>Test that a level above Fatal does not emit a Fatal value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Disabled_Does_Not_Emit_Value()
    {
        var (logger, target) = CreateLogger(AboveFatal);

        logger.Fatal(new DummyObjectClass1());

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Fatal does not emit a null value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Null_Value_Does_Not_Emit()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Fatal((DummyObjectClass1)null!);

        await Assert.That(target.Logs).IsEmpty();
    }

    /// <summary>Test that Fatal substitutes the placeholder when the value renders to null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Null_ToString_Emits_Placeholder()
    {
        var (logger, target) = CreateLogger(LogLevel.Fatal);

        logger.Fatal(new NullToStringValue());

        await AssertPlaceholder(target, LogLevel.Fatal);
    }

    /// <summary>Asserts the most recent captured entry is the null placeholder at the expected level.</summary>
    /// <param name="target">The capture target.</param>
    /// <param name="expectedLevel">The level the entry should have been logged at.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task AssertPlaceholder(TextLogger target, LogLevel expectedLevel)
    {
        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(NullPlaceholder);
            await Assert.That(entry.logLevel).IsEqualTo(expectedLevel);
        }
    }

    /// <summary>A value whose <see cref="object.ToString"/> returns <see langword="null"/> to exercise the fallback.</summary>
    private sealed class NullToStringValue
    {
        /// <summary>Simulates a misbehaving override that returns <see langword="null"/> at runtime.</summary>
        /// <returns>Always <see langword="null"/>.</returns>
        public override string ToString() => NullText();

        /// <summary>Produces a runtime <see langword="null"/> behind a non-null declared return type.</summary>
        /// <returns>Always <see langword="null"/>.</returns>
        private static string NullText() => null!;
    }
}
