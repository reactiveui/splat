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
/// This class is not thread-safe; each instance should be used on a single thread.
/// </remarks>
/// <param name="activitySource">The OpenTelemetry ActivitySource used to create activities for tracking page views. Cannot be null.</param>
public sealed class ApplicationInsightsViewTracking(ActivitySource activitySource) : IViewTracking
{
    private Activity? _currentPageActivity;

    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name)
    {
        // Stop the previous page view activity if one exists
        _currentPageActivity?.Stop();

        // Start a new activity for the current page view
        // ActivityKind.Server ensures it appears as a request in Application Insights
        _currentPageActivity = activitySource.StartActivity(
            name: $"PageView: {name}",
            kind: ActivityKind.Server);

        if (_currentPageActivity is not null)
        {
            // Add standard HTTP semantic convention tags
            _currentPageActivity.SetTag("http.url", $"view://{name}")
                .SetTag("http.method", "GET")
                .SetTag("http.scheme", "view")
                .SetTag("url.scheme", "view")
                .SetTag("url.path", $"/{name}")

                // Add custom tags for filtering and analysis
                .SetTag("view.name", name)
                .SetTag("view.type", "page");
        }
    }
}
