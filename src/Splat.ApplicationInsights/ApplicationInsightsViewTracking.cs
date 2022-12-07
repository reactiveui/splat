﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// View Tracking integration for Application Insights.
/// </summary>
public sealed class ApplicationInsightsViewTracking : IViewTracking
{
    private readonly TelemetryClient _telemetryClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationInsightsViewTracking"/> class.
    /// </summary>
    /// <param name="telemetryClient">The Application Insights telemetry client instance to use.</param>
    public ApplicationInsightsViewTracking(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name)
    {
        _telemetryClient.TrackPageView(name);
    }

    /// <summary>
    /// Track a View Navigation with Extended Data.
    /// </summary>
    /// <param name="telemetry">Telemetry data.</param>
    public void OnViewNavigation(PageViewTelemetry telemetry)
    {
        var pageViewTelemetry = GetPageViewTelemetry();
        _telemetryClient.TrackPageView(telemetry);
    }

    internal static PageViewTelemetry GetPageViewTelemetry()
    {
        var result = new PageViewTelemetry();
        return result;
    }
}
