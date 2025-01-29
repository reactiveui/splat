// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// View Tracking integration for AppCenter.
/// </summary>
public sealed class AppCenterViewTracking : IViewTracking
{
    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name) =>
        Microsoft.AppCenter.Analytics.Analytics.TrackEvent("PageView", GetProperties(name));

    private static Dictionary<string, string> GetProperties(string name) =>
        new() { { "Name", name }, };
}
