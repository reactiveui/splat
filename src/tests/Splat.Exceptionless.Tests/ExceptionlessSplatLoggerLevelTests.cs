// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

using Exceptionless;

using Splat.Exceptionless;

namespace Splat.Tests.Logging;

/// <summary>Tests how <see cref="ExceptionlessSplatLogger"/> filters by effective level, resolves the source, and reacts to reconfiguration.</summary>
[NotInParallel]
public sealed class ExceptionlessSplatLoggerLevelTests
{
    /// <summary>The Exceptionless settings key that stores the wildcard minimum log level.</summary>
    private const string WildcardLogLevelKey = "@@log:*";

    /// <summary>The source recorded when a type does not expose a full name.</summary>
    private const string UnknownSource = "(unknown)";

    /// <summary>Verifies a write below the effective level is not submitted, while a write at or above it is.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_BelowEffectiveLevel_IsFilteredOut()
    {
        var (logger, submitted, _) = CreateLogger(global::Exceptionless.Logging.LogLevel.Warn);
        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);

        logger.Write("filtered", LogLevel.Debug);
        await Assert.That(submitted).IsEmpty();

        logger.Write("kept", LogLevel.Error);
        await Assert.That(submitted.Select(static x => x.Message)).Contains("kept");
    }

    /// <summary>Verifies writes for a type without a full name are submitted under the unknown source.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Write_WithType_LackingFullName_UsesUnknownSource()
    {
        var typeWithoutFullName = typeof(List<>).GetGenericArguments()[0];
        var (logger, submitted, _) = CreateLogger(global::Exceptionless.Logging.LogLevel.Debug);

        logger.Write("typed", typeWithoutFullName, LogLevel.Debug);
        logger.Write(new InvalidOperationException("boom"), "typed-with-exception", typeWithoutFullName, LogLevel.Debug);

        using (Assert.Multiple())
        {
            await Assert.That(submitted.Select(static x => x.Message)).Contains("typed");
            await Assert.That(submitted.Select(static x => x.Message)).Contains("typed-with-exception");
            await Assert.That(submitted.TrueForAll(static x => x.Source == UnknownSource)).IsTrue();
        }
    }

    /// <summary>Verifies the logger recomputes its effective level when the client configuration changes.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Reconfiguration_UpdatesEffectiveLevel()
    {
        var (logger, _, client) = CreateLogger(global::Exceptionless.Logging.LogLevel.Warn);
        await Assert.That(logger.Level).IsEqualTo(LogLevel.Warn);

        // Change the configured minimum level, then trigger a configuration-changed notification.
        client.Configuration.Settings[WildcardLogLevelKey] = global::Exceptionless.Logging.LogLevel.Error.ToString();
        client.Configuration.ApiKey = "reconfigured-api-key";

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Error);
    }

    /// <summary>Builds a logger whose submitted log events are captured and never sent to the network.</summary>
    /// <param name="minLevel">The minimum Exceptionless log level to configure.</param>
    /// <returns>The logger, the list of captured (source, message) pairs, and the client backing it.</returns>
    private static (ExceptionlessSplatLogger Logger, List<(string? Source, string? Message)> Submitted, ExceptionlessClient Client) CreateLogger(global::Exceptionless.Logging.LogLevel minLevel)
    {
        var client = new ExceptionlessClient();
        client.Configuration.ApiKey = "some-api-key";
        client.Configuration.SetDefaultMinLogLevel(minLevel);

        var submitted = new List<(string? Source, string? Message)>();
        client.Configuration.AddPlugin("capture", context =>
        {
            context.Cancel = true;
            submitted.Add((context.Event.Source, context.Event.Message));
        });

        var logger = new ExceptionlessSplatLogger(typeof(ExceptionlessSplatLoggerLevelTests), client);
        return (logger, submitted, client);
    }
}

#endif
