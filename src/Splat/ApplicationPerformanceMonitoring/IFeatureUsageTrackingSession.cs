// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Base interface for a feature usage tracking session.
/// </summary>
public interface IFeatureUsageTrackingSession : IDisposable
{
    /// <summary>
    /// Gets the name of the Feature being tracked.
    /// </summary>
    string FeatureName { get; }

    /// <summary>
    /// Starts a sub-feature usage tracking session on the current session.
    /// </summary>
    /// <param name="description">Description of the sub-feature.</param>
    /// <returns>The sub-feature usage tracking session.</returns>
    IFeatureUsageTrackingSession SubFeature(string description);

    /// <summary>
    /// Notify the APM toolset an exception has occured in the current tracking session.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    void OnException(Exception exception);
}
