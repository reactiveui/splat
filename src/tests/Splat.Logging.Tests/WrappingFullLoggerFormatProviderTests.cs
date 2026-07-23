// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests for the single-value <see cref="IFormatProvider"/> overloads of <see cref="WrappingFullLogger"/>
/// (Debug/Info/Warn/Error/Fatal). Each renders the value through
/// <see cref="string.Format(IFormatProvider, string, object?)"/> using the caller-supplied provider and
/// forwards the resulting text to the wrapped <see cref="ILogger"/> at the matching level.
/// </summary>
public class WrappingFullLoggerFormatProviderTests
{
    /// <summary>The value passed to each single-value overload.</summary>
    private const int SampleValue = 42;

    /// <summary>The text the value renders to under <see cref="CultureInfo.InvariantCulture"/>.</summary>
    private const string ExpectedText = "42";

    /// <summary>Test that the Debug single-value format-provider overload forwards the formatted text at Debug level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Debug_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, target) = CreateLogger();

        logger.Debug(CultureInfo.InvariantCulture, SampleValue);

        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(ExpectedText);
            await Assert.That(entry.logLevel).IsEqualTo(LogLevel.Debug);
        }
    }

    /// <summary>Test that the Info single-value format-provider overload forwards the formatted text at Info level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Info_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, target) = CreateLogger();

        logger.Info(CultureInfo.InvariantCulture, SampleValue);

        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(ExpectedText);
            await Assert.That(entry.logLevel).IsEqualTo(LogLevel.Info);
        }
    }

    /// <summary>Test that the Warn single-value format-provider overload forwards the formatted text at Warn level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Warn_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, target) = CreateLogger();

        logger.Warn(CultureInfo.InvariantCulture, SampleValue);

        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(ExpectedText);
            await Assert.That(entry.logLevel).IsEqualTo(LogLevel.Warn);
        }
    }

    /// <summary>Test that the Error single-value format-provider overload forwards the formatted text at Error level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Error_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, target) = CreateLogger();

        logger.Error(CultureInfo.InvariantCulture, SampleValue);

        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(ExpectedText);
            await Assert.That(entry.logLevel).IsEqualTo(LogLevel.Error);
        }
    }

    /// <summary>Test that the Fatal single-value format-provider overload forwards the formatted text at Fatal level.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_With_FormatProvider_Value_Forwards_Formatted_Text()
    {
        var (logger, target) = CreateLogger();

        logger.Fatal(CultureInfo.InvariantCulture, SampleValue);

        var entry = target.Logs.Last();
        using (Assert.Multiple())
        {
            await Assert.That(entry.message).IsEqualTo(ExpectedText);
            await Assert.That(entry.logLevel).IsEqualTo(LogLevel.Fatal);
        }
    }

    /// <summary>Creates a <see cref="WrappingFullLogger"/> over a <see cref="TextLogger"/> capture target.</summary>
    /// <returns>The wrapping logger and the capture target it forwards to.</returns>
    private static (WrappingFullLogger Logger, TextLogger Target) CreateLogger()
    {
        var target = new TextLogger { Level = LogLevel.Debug };
        return (new WrappingFullLogger(target), target);
    }
}
