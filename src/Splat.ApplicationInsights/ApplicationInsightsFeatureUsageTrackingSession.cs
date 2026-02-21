// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.ApplicationInsights;

/// <summary>
/// Provides a feature usage tracking session that records feature usage events and exceptions to Azure Application
/// Insights using a specified telemetry client.
/// </summary>
/// <remarks>This class is typically used to track the start and end of feature usage, as well as any exceptions
/// that occur during the session. Feature usage events are sent to Application Insights when the session is created and
/// disposed. Sub-features can be tracked by creating nested sessions. This class is not thread-safe; each instance
/// should be used on a single thread.</remarks>
public sealed class ApplicationInsightsFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    /// <summary>
    /// The Application Insights telemetry client used to send events and exceptions.
    /// </summary>
    private readonly TelemetryClient _telemetryClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">The name of the feature being tracked.</param>
    /// <param name="telemetryClient">The Application Insights telemetry client instance to use for sending telemetry data.</param>
    public ApplicationInsightsFeatureUsageTrackingSession(
        string featureName,
        TelemetryClient telemetryClient)
        : this(featureName, Guid.Empty, telemetryClient)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class
    /// with a parent reference for sub-feature tracking.
    /// </summary>
    /// <param name="featureName">The name of the feature being tracked.</param>
    /// <param name="parentReference">The unique identifier of the parent feature session, or <see cref="Guid.Empty"/> if this is a top-level session.</param>
    /// <param name="telemetryClient">The Application Insights telemetry client instance to use for sending telemetry data.</param>
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

    /// <summary>
    /// Tracks a feature usage event with the specified event name to Application Insights.
    /// </summary>
    /// <param name="eventName">The name of the event to track (e.g., "Feature Usage Start" or "Feature Usage End").</param>
    private void TrackEvent(string eventName)
    {
        var eventTelemetry = new EventTelemetry(eventName);
        PrepareEventData(eventTelemetry);

        _telemetryClient.TrackEvent(eventTelemetry);
    }

    /// <summary>
    /// Populates the standard feature tracking properties on a telemetry item.
    /// </summary>
    /// <typeparam name="TTelemetry">The type of telemetry item that supports custom properties.</typeparam>
    /// <param name="eventTelemetry">The telemetry item to populate with feature name, reference, and optional parent reference properties.</param>
    private void PrepareEventData<TTelemetry>(TTelemetry eventTelemetry)
        where TTelemetry : ISupportProperties
    {
        eventTelemetry.Properties.Add("Name", FeatureName);
        eventTelemetry.Properties.Add("Reference", FeatureReference.ToString());

        if (ParentReference != Guid.Empty)
        {
            eventTelemetry.Properties.Add("ParentReference", ParentReference.ToString());
        }
    }
}
