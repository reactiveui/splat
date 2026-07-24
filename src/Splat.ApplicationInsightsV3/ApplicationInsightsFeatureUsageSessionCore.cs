// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;

namespace Splat.ApplicationInsightsV3;

/// <summary>Owns the mutable work of an Application Insights v3 feature usage session.</summary>
internal sealed class ApplicationInsightsFeatureUsageSessionCore
{
    /// <summary>The event name emitted when a session starts.</summary>
    private const string FeatureUsageStartEventName = "Feature Usage Start";

    /// <summary>The event name emitted when a session ends.</summary>
    private const string FeatureUsageEndEventName = "Feature Usage End";

    /// <summary>The Application Insights client that receives feature telemetry.</summary>
    private readonly TelemetryClient _telemetryClient;

    /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageSessionCore"/> class.</summary>
    /// <param name="featureName">The Splat feature name.</param>
    /// <param name="parentReference">The parent feature reference, or <see cref="Guid.Empty"/> for a root session.</param>
    /// <param name="telemetryClient">The client that receives telemetry.</param>
    internal ApplicationInsightsFeatureUsageSessionCore(
        string featureName,
        Guid parentReference,
        TelemetryClient telemetryClient)
    {
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();
        ParentReference = parentReference;
        _telemetryClient = telemetryClient;

        TrackEvent(FeatureUsageStartEventName);
    }

    /// <summary>Gets the current feature session reference.</summary>
    internal Guid FeatureReference { get; }

    /// <summary>Gets the parent feature session reference.</summary>
    internal Guid ParentReference { get; }

    /// <summary>Gets the current feature name.</summary>
    internal string FeatureName { get; }

    /// <summary>Creates a child feature usage session.</summary>
    /// <param name="description">The child feature name.</param>
    /// <returns>The child feature usage session core.</returns>
    internal ApplicationInsightsFeatureUsageSessionCore CreateSubFeature(string description) =>
        new(description, FeatureReference, _telemetryClient);

    /// <summary>Tracks the end of this feature usage session.</summary>
    internal void TrackEnd() => TrackEvent(FeatureUsageEndEventName);

    /// <summary>Tracks an exception that occurred during this feature usage session.</summary>
    /// <param name="exception">The exception captured during the session.</param>
    internal void TrackException(Exception exception) =>
        _telemetryClient.TrackException(
            ApplicationInsightsTelemetryFactory.CreateExceptionTelemetry(
                exception,
                FeatureName,
                FeatureReference,
                ParentReference));

    /// <summary>Tracks a feature usage event for this session.</summary>
    /// <param name="eventName">The Application Insights event name.</param>
    private void TrackEvent(string eventName) =>
        _telemetryClient.TrackEvent(
            ApplicationInsightsTelemetryFactory.CreateFeatureUsageEvent(
                eventName,
                FeatureName,
                FeatureReference,
                ParentReference));
}
