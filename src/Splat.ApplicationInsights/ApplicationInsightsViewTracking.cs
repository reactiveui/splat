// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Provides view tracking functionality that records page view navigation events using Application Insights telemetry.
/// </summary>
/// <remarks>
/// This class uses OpenTelemetry Activities with ActivityKind.Server to track view navigation in Application Insights v3.
/// Activities appear as requests in Application Insights, with the duration automatically tracked from activity start to stop.
/// The implementation follows semantic conventions for HTTP requests while adding custom tags for view-specific filtering.
/// </remarks>
/// <param name="activitySource">The OpenTelemetry ActivitySource used to create activities for tracking page views. Cannot be null.</param>
public sealed class ApplicationInsightsViewTracking(ActivitySource activitySource) : IViewTracking
{
    private readonly ActivitySource _activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));

    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name)
    {
        var tags = new KeyValuePair<string, object?>[]
        {
            new("http.url", $"view://{name}"),
            new("http.method", "GET"),
            new("http.scheme", "view"),
            new("url.scheme", "view"),
            new("url.path", $"/{name}"),
            new("view.name", name),
            new("view.type", "page")
        };

        // Start a new activity for the current page view
        // ActivityKind.Server ensures it appears as a request in Application Insights
        var activity = _activitySource.StartActivity(
            kind: ActivityKind.Server,
            tags: tags,
            name: $"PageView: {name}");

        activity?.Stop();
    }
}
