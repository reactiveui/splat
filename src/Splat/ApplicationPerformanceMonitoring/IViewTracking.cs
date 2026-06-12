// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>Defines a contract for tracking view navigation events by name.</summary>
public interface IViewTracking
{
    /// <summary>Handles navigation to a view identified by the specified name.</summary>
    /// <param name="name">The name of the view to navigate to. Cannot be null or empty.</param>
    void OnViewNavigation(string name);
}
