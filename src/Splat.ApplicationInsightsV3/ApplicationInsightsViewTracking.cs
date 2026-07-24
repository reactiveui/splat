// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.ApplicationInsightsV3;

/// <summary>Records view navigation with the Application Insights v3 client.</summary>
/// <remarks>Application Insights v3 removed native page-view telemetry; navigation is emitted as a custom event named <c>PageView</c>.</remarks>
/// <param name="telemetryClient">The client that receives telemetry.</param>
public sealed class ApplicationInsightsViewTracking(TelemetryClient telemetryClient) : IViewTracking
{
    /// <summary>Tracks a view navigation as a custom <c>PageView</c> event.</summary>
    /// <param name="name">The view name recorded in the event properties.</param>
    public void OnViewNavigation(string name) =>
        telemetryClient.TrackEvent(ApplicationInsightsTelemetryFactory.CreateViewNavigationEvent(name));
}
