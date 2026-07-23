// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

using Splat.Common.Test;
using Splat.Microsoft.Extensions.Logging;

using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="MicrosoftExtensionsLogProvider"/> class.</summary>
[NotInParallel]
public class MicrosoftExtensionsLogProviderCoverageTests
{
    /// <summary>The category name used when creating the logger under test.</summary>
    private const string Category = "MyCategory";

    /// <summary>The log state value passed to scope and write calls.</summary>
    private const string LogState = "state";

    /// <summary>Verifies the provider creates a non-null logger.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task CreateLogger_ReturnsLogger()
    {
        using var provider = new MicrosoftExtensionsLogProvider();

        var logger = provider.CreateLogger(Category);

        await Assert.That(logger).IsNotNull();
    }

    /// <summary>Verifies disposing the provider does not throw.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Dispose_DoesNotThrow()
    {
        var provider = new MicrosoftExtensionsLogProvider();

        await Assert.That(() => provider.Dispose()).ThrowsNothing();
    }

    /// <summary>Verifies the adapter logger is enabled for all levels except None.</summary>
    /// <param name="logLevel">The Microsoft log level to evaluate.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    [Arguments(MsLogLevel.Trace)]
    [Arguments(MsLogLevel.Debug)]
    [Arguments(MsLogLevel.Information)]
    [Arguments(MsLogLevel.Warning)]
    [Arguments(MsLogLevel.Error)]
    [Arguments(MsLogLevel.Critical)]
    public async Task IsEnabled_TrueForAllExceptNone(MsLogLevel logLevel)
    {
        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        await Assert.That(logger.IsEnabled(logLevel)).IsTrue();
    }

    /// <summary>Verifies the adapter logger is disabled for the None level.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task IsEnabled_FalseForNone()
    {
        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        await Assert.That(logger.IsEnabled(MsLogLevel.None)).IsFalse();
    }

    /// <summary>Verifies the adapter logger returns a non-null disposable scope.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task BeginScope_DoesNotThrow()
    {
        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        await Assert.That(() => logger.BeginScope(LogState)).ThrowsNothing();
    }

    /// <summary>Verifies a None-level write is skipped and does not forward to Splat.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Log_NoneLevel_DoesNotForward()
    {
        using var scope = new AppLocatorScope();
        var capture = RegisterCapturingLogManager();

        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        logger.Log(MsLogLevel.None, new(0), LogState, null, static (state, _) => state);

        await Assert.That(capture.Logs).IsEmpty();
    }

    /// <summary>Verifies an enabled-level write forwards the formatted message to Splat.</summary>
    /// <param name="microsoftLevel">The Microsoft log level used for the write.</param>
    /// <param name="expectedSplatLevel">The expected Splat log level after mapping.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    [Arguments(MsLogLevel.Debug, LogLevel.Debug)]
    [Arguments(MsLogLevel.Information, LogLevel.Info)]
    [Arguments(MsLogLevel.Warning, LogLevel.Warn)]
    [Arguments(MsLogLevel.Error, LogLevel.Error)]
    [Arguments(MsLogLevel.Critical, LogLevel.Fatal)]
    public async Task Log_EnabledLevel_ForwardsToSplat(MsLogLevel microsoftLevel, LogLevel expectedSplatLevel)
    {
        using var scope = new AppLocatorScope();
        var capture = RegisterCapturingLogManager();

        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        logger.Log(microsoftLevel, new(0), "hello", null, static (state, _) => state);

        await Assert.That(capture.Logs.Count).IsEqualTo(1);
        await Assert.That(capture.Logs[0].logLevel).IsEqualTo(expectedSplatLevel);
    }

    /// <summary>Verifies a null formatter throws an <see cref="ArgumentNullException"/>.</summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task Log_NullFormatter_Throws()
    {
        using var scope = new AppLocatorScope();
        _ = RegisterCapturingLogManager();

        using var provider = new MicrosoftExtensionsLogProvider();
        var logger = provider.CreateLogger(Category);

        await Assert.That(() => logger.Log(MsLogLevel.Information, new(0), LogState, null, null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Registers an <see cref="ILogManager"/> backed by a capturing logger so writes can be observed.</summary>
    /// <returns>The capturing logger used as the backing store.</returns>
    private static CapturingLogger RegisterCapturingLogManager()
    {
        var capture = new CapturingLogger();
        AppLocator.CurrentMutable.Register<ILogManager>(() => new FuncLogManager(_ => new WrappingFullLogger(capture)));
        return capture;
    }

    /// <summary>A minimal <see cref="ILogger"/> that records every write for verification.</summary>
    private sealed class CapturingLogger : ILogger
    {
        /// <summary>Gets the captured log entries.</summary>
        public List<(LogLevel logLevel, string message)> Logs { get; } = [];

        /// <inheritdoc/>
        public LogLevel Level => LogLevel.Debug;

        /// <inheritdoc/>
        public void Write(string message, LogLevel logLevel) => Logs.Add((logLevel, message));

        /// <inheritdoc/>
        public void Write(Exception exception, string message, LogLevel logLevel) => Logs.Add((logLevel, message));

        /// <inheritdoc/>
        public void Write(string message, Type type, LogLevel logLevel) => Logs.Add((logLevel, message));

        /// <inheritdoc/>
        public void Write(Exception exception, string message, Type type, LogLevel logLevel) => Logs.Add((logLevel, message));
    }
}
