// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>An in-memory <see cref="ITelemetryChannel"/> that records every telemetry item sent through it.</summary>
internal sealed class CapturingTelemetryChannel : ITelemetryChannel
{
    /// <inheritdoc />
    public bool? DeveloperMode { get; set; }

    /// <inheritdoc />
    public string EndpointAddress { get; set; } = string.Empty;

    /// <summary>Gets the telemetry items that were sent to the channel, in send order.</summary>
    internal List<ITelemetry> Sent { get; } = [];

    /// <inheritdoc />
    public void Send(ITelemetry item) => Sent.Add(item);

    /// <inheritdoc />
    public void Flush()
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <summary>Builds a <see cref="TelemetryClient"/> that routes all telemetry to a fresh capturing channel.</summary>
    /// <returns>The telemetry client and the channel that captures what it sends.</returns>
    internal static (TelemetryClient Client, CapturingTelemetryChannel Channel) CreateClient()
    {
        var channel = new CapturingTelemetryChannel();
        var configuration = new TelemetryConfiguration
        {
            ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
            TelemetryChannel = channel,
        };

        return (new TelemetryClient(configuration), channel);
    }
}
