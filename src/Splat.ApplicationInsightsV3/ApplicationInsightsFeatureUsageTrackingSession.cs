// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.ApplicationInsightsV3;

/// <summary>Tracks feature usage events and exceptions with the Application Insights v3 client.</summary>
public sealed class ApplicationInsightsFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    /// <summary>The client used for all feature usage telemetry.</summary>
    private readonly TelemetryClient _telemetryClient;

    /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.</summary>
    /// <param name="featureName">The feature name stored with each telemetry item.</param>
    /// <param name="telemetryClient">The client that receives telemetry.</param>
    public ApplicationInsightsFeatureUsageTrackingSession(
        string featureName,
        TelemetryClient telemetryClient)
        : this(featureName, Guid.Empty, telemetryClient)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.</summary>
    /// <param name="featureName">The feature name stored with each telemetry item.</param>
    /// <param name="parentReference">The parent feature reference, or <see cref="Guid.Empty"/> for root sessions.</param>
    /// <param name="telemetryClient">The client that receives telemetry.</param>
    internal ApplicationInsightsFeatureUsageTrackingSession(
        string featureName,
        Guid parentReference,
        TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();
        ParentReference = parentReference;

        TrackEvent("Feature Usage Start");
    }

    /// <inheritdoc />
    public Guid FeatureReference { get; }

    /// <inheritdoc />
    public Guid ParentReference { get; }

    /// <inheritdoc />
    public string FeatureName { get; }

    /// <inheritdoc />
    public void Dispose() => TrackEvent("Feature Usage End");

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) =>
        new ApplicationInsightsFeatureUsageTrackingSession(
            description,
            FeatureReference,
            _telemetryClient);

    /// <inheritdoc />
    public void OnException(Exception exception)
    {
        var telemetry = new ExceptionTelemetry(exception);
        PrepareEventData(telemetry);

        _telemetryClient.TrackException(telemetry);
    }

    /// <summary>Sends a feature usage event with the current session metadata.</summary>
    /// <param name="eventName">The Application Insights event name.</param>
    private void TrackEvent(string eventName)
    {
        var eventTelemetry = new EventTelemetry(eventName);
        PrepareEventData(eventTelemetry);

        _telemetryClient.TrackEvent(eventTelemetry);
    }

    /// <summary>Adds the session metadata shared by events and exceptions.</summary>
    /// <typeparam name="TTelemetry">The telemetry shape that carries custom properties.</typeparam>
    /// <param name="eventTelemetry">The telemetry item being populated.</param>
    private void PrepareEventData<TTelemetry>(TTelemetry eventTelemetry)
        where TTelemetry : ISupportProperties
    {
        eventTelemetry.Properties.Add("Name", FeatureName);
        eventTelemetry.Properties.Add("Reference", FeatureReference.ToString());

        if (ParentReference == Guid.Empty)
        {
            return;
        }

        eventTelemetry.Properties.Add("ParentReference", ParentReference.ToString());
    }
}
