// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.ApplicationInsightsV3;

/// <summary>Tracks feature usage events and exceptions with the Application Insights v3 client.</summary>
public sealed class ApplicationInsightsFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    /// <summary>The session state and telemetry sender.</summary>
    private readonly ApplicationInsightsFeatureUsageSessionCore _session;

    /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.</summary>
    /// <param name="featureName">The feature name stored with each telemetry item.</param>
    /// <param name="telemetryClient">The client that receives telemetry.</param>
    public ApplicationInsightsFeatureUsageTrackingSession(
        string featureName,
        Microsoft.ApplicationInsights.TelemetryClient telemetryClient)
        : this(new ApplicationInsightsFeatureUsageSessionCore(featureName, Guid.Empty, telemetryClient))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsFeatureUsageTrackingSession"/> class.</summary>
    /// <param name="session">The session state and telemetry sender.</param>
    private ApplicationInsightsFeatureUsageTrackingSession(ApplicationInsightsFeatureUsageSessionCore session) => _session = session;

    /// <inheritdoc />
    public Guid FeatureReference => _session.FeatureReference;

    /// <inheritdoc />
    public Guid ParentReference => _session.ParentReference;

    /// <inheritdoc />
    public string FeatureName => _session.FeatureName;

    /// <inheritdoc />
    public void Dispose() => _session.TrackEnd();

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) =>
        new ApplicationInsightsFeatureUsageTrackingSession(_session.CreateSubFeature(description));

    /// <inheritdoc />
    public void OnException(Exception exception) => _session.TrackException(exception);
}
