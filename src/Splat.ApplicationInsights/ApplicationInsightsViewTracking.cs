// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Provides view tracking functionality that records page view navigation events using Application Insights telemetry.
/// </summary>
/// <remarks>This class is typically used to integrate view navigation tracking into applications that utilize
/// Application Insights for telemetry. It implements the IViewTracking interface to standardize view tracking across
/// different telemetry providers.</remarks>
/// <param name="telemetryClient">The Application Insights telemetry client used to send page view tracking data. Cannot be null.</param>
public sealed class ApplicationInsightsViewTracking(TelemetryClient telemetryClient) : IViewTracking
{
    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name) => telemetryClient.TrackPageView(name);

    /// <summary>
    /// Track a View Navigation with Extended Data.
    /// </summary>
    /// <param name="telemetry">Telemetry data.</param>
    public void OnViewNavigation(PageViewTelemetry telemetry)
    {
        _ = GetPageViewTelemetry();
        telemetryClient.TrackPageView(telemetry);
    }

    internal static PageViewTelemetry GetPageViewTelemetry() => new();
}
