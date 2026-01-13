// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Represents a session for tracking the usage of a specific feature, allowing for hierarchical sub-feature tracking
/// and exception reporting.
/// </summary>
/// <remarks>Implementations of this interface are intended to be used with application performance monitoring
/// (APM) tools to record feature usage and exceptions. The session should be disposed when tracking is complete to
/// ensure that all usage data is properly finalized and reported. Sub-feature sessions can be created to track nested
/// operations within the main feature session.</remarks>
public interface IFeatureUsageTrackingSession : IDisposable
{
    /// <summary>
    /// Gets the name of the Feature being tracked.
    /// </summary>
    string FeatureName { get; }

    /// <summary>
    /// Creates a new sub-feature tracking session with the specified description.
    /// </summary>
    /// <remarks>Use sub-feature tracking sessions to record usage metrics for distinct operations or
    /// components within a larger feature. Each sub-feature session is independent and should be disposed when tracking
    /// is complete.</remarks>
    /// <param name="description">A string that describes the sub-feature to be tracked. Cannot be null or empty.</param>
    /// <returns>An <see cref="IFeatureUsageTrackingSession"/> instance for tracking usage of the specified sub-feature.</returns>
    IFeatureUsageTrackingSession SubFeature(string description);

    /// <summary>
    /// Notify the APM toolset an exception has occurred in the current tracking session.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    void OnException(Exception exception);
}
