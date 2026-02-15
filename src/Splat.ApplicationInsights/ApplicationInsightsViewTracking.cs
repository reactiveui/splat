// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Provides view tracking functionality that records page view navigation events using Application Insights telemetry.
/// </summary>
/// <remarks>This class is typically used to integrate view navigation tracking into applications that utilize
/// Application Insights for telemetry. It implements the IViewTracking interface to standardize view tracking across
/// different telemetry providers. In Application Insights v3, page views are tracked as custom events since the
/// PageViewTelemetry type has been removed.</remarks>
/// <param name="telemetryClient">The Application Insights telemetry client used to send page view tracking data. Cannot be null.</param>
public sealed class ApplicationInsightsViewTracking(TelemetryClient telemetryClient) : IViewTracking
{
    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name) =>
        telemetryClient.TrackEvent(
            "PageView",
            new Dictionary<string, string> { ["Name"] = name });
}
