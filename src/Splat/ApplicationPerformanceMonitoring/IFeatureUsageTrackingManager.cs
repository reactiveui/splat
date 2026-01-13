// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Defines a contract for creating feature usage tracking sessions for specified features.
/// </summary>
public interface IFeatureUsageTrackingManager
{
    /// <summary>
    /// Creates a new session for tracking usage of the specified feature.
    /// </summary>
    /// <remarks>Use the returned session to record feature usage events. Disposing the session typically
    /// finalizes and submits the usage data. Multiple sessions can be created for different features as
    /// needed.</remarks>
    /// <param name="featureName">The name of the feature to track. Cannot be null or empty.</param>
    /// <returns>An object representing the feature usage tracking session. The caller is responsible for disposing the session
    /// when tracking is complete.</returns>
    IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName);
}
