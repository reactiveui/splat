// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests <see cref="SerilogFullLogger"/> behaviour when the underlying Serilog logger reports every level as
/// disabled (its minimum level is controlled above Fatal). The <c>Level</c> getter then falls through to its
/// Fatal default, and the deferred Fatal callbacks return without invoking their callback.
/// </summary>
public class SerilogFullLoggerDisabledLevelTests
{
    /// <summary>The message the deferred callbacks would return if they were ever invoked.</summary>
    private const string DisabledMessage = "should not be logged";

    /// <summary>Verifies the <c>Level</c> getter defaults to Fatal when no mapped level is enabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Level_WhenEveryLevelDisabled_DefaultsToFatal()
    {
        var (logger, _) = RecordingSerilogSink.CreateWithAllLevelsDisabled();

        await Assert.That(logger.Level).IsEqualTo(LogLevel.Fatal);
    }

    /// <summary>Verifies the deferred Fatal callbacks are skipped while Fatal is disabled.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Fatal_Functions_WhenDisabled_DoNotInvokeCallback()
    {
        var (logger, sink) = RecordingSerilogSink.CreateWithAllLevelsDisabled();
        var functionInvoked = false;
        var genericInvoked = false;

        logger.Fatal(() =>
        {
            functionInvoked = true;
            return DisabledMessage;
        });

        logger.Fatal<DummyObjectClass1>(() =>
        {
            genericInvoked = true;
            return DisabledMessage;
        });

        using (Assert.Multiple())
        {
            await Assert.That(functionInvoked).IsFalse();
            await Assert.That(genericInvoked).IsFalse();
            await Assert.That(sink.Count).IsEqualTo(0);
        }
    }
}
