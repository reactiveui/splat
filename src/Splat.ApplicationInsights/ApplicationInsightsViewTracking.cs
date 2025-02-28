// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// View Tracking integration for Application Insights.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApplicationInsightsViewTracking"/> class.
/// </remarks>
/// <param name="telemetryClient">The Application Insights telemetry client instance to use.</param>
public sealed class ApplicationInsightsViewTracking(TelemetryClient telemetryClient) : IViewTracking
{
    private readonly TelemetryClient _telemetryClient = telemetryClient;

    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name) => _telemetryClient.TrackPageView(name);

    /// <summary>
    /// Track a View Navigation with Extended Data.
    /// </summary>
    /// <param name="telemetry">Telemetry data.</param>
    public void OnViewNavigation(PageViewTelemetry telemetry)
    {
        _ = GetPageViewTelemetry();
        _telemetryClient.TrackPageView(telemetry);
    }

    internal static PageViewTelemetry GetPageViewTelemetry() => new();
}
