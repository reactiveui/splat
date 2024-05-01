// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat.ApplicationInsights;

/// <summary>
/// Feature Usage Tracking Client for Application Insights.
/// </summary>
public sealed class ApplicationInsightsFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    private readonly TelemetryClient _telemetryClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">The name of the feature.</param>
    /// <param name="telemetryClient">The Application Insights telemetry client instance to use.</param>
    public ApplicationInsightsFeatureUsageTrackingSession(
        string featureName,
        TelemetryClient telemetryClient)
        : this(featureName, Guid.Empty, telemetryClient)
    {
    }

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

    private void TrackEvent(string eventName)
    {
        var eventTelemetry = new EventTelemetry(eventName);
        PrepareEventData(eventTelemetry);

        _telemetryClient.TrackEvent(eventTelemetry);
    }

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
