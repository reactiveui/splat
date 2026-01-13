// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Provides view tracking functionality for navigation events, enabling integration with App Center Analytics.
/// </summary>
/// <remarks>This class is intended to be used for tracking page or view navigation within an application. It
/// implements the IViewTracking interface to standardize view tracking across different analytics providers. Instances
/// of this class are typically used to report navigation events to Microsoft App Center Analytics.</remarks>
public sealed class AppCenterViewTracking : IViewTracking
{
    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name) =>
        Microsoft.AppCenter.Analytics.Analytics.TrackEvent("PageView", GetProperties(name));

    /// <summary>
    /// Creates a dictionary containing a single entry with the specified name.
    /// </summary>
    /// <param name="name">The value to associate with the "Name" key in the returned dictionary. Cannot be null.</param>
    /// <returns>A dictionary with one entry where the key is "Name" and the value is the specified name.</returns>
    private static Dictionary<string, string> GetProperties(string name) =>
        new() { { "Name", name }, };
}
