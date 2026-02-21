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
    public void OnViewNavigation(string name) => OnViewNavigation(
        name,
        new Dictionary<string, string>());

    /// <summary>
    /// Track a view navigation using name and extended properties.
    /// </summary>
    /// <remarks>
    /// See https://github.com/microsoft/ApplicationInsights-dotnet/tree/main/BASE#tracking-page-views for details on underlying usage.
    /// </remarks>
    /// <param name="name">Name of the view.</param>
    /// <param name="extendedProperties">Set of extended properties to send with the event. NOTE: if you set PageName in the collection, it will be overridden using <see cref="name"/>.</param>
    public void OnViewNavigation(
        string name,
        IDictionary<string, string> extendedProperties)
    {
        // need to look at whether the standard properties of the Javascript SDK are supported in the .NET SDK (or rather the Azure Monitor when it rewrites the payload), but for now we'll just leave as minimal and allow injection by caller.
        // reference: https://github.com/microsoft/ApplicationInsights-JS/blob/b6de144e27629b2d50e05ceb3885ee51b4fa0e2b/API-reference.md
        extendedProperties ??= new Dictionary<string, string>();
        extendedProperties["PageName"] = name;

        telemetryClient.TrackEvent(
            "PageView",
            extendedProperties);
    }
}
