// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Func based Feature Usage Tracking Manager.
/// </summary>
public class FuncFeatureUsageTrackingManager : IFeatureUsageTrackingManager
{
    private readonly Func<string, IFeatureUsageTrackingSession> _featureUsageTrackingSessionFunc;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncFeatureUsageTrackingManager"/> class.
    /// </summary>
    /// <param name="featureUsageTrackingSessionFunc">
    /// Factory function for a Feature Usage Tracking Session.
    /// </param>
    public FuncFeatureUsageTrackingManager(Func<string, IFeatureUsageTrackingSession> featureUsageTrackingSessionFunc)
    {
        ArgumentExceptionHelper.ThrowIfNull(featureUsageTrackingSessionFunc);
        _featureUsageTrackingSessionFunc = featureUsageTrackingSessionFunc;
    }

    /// <inheritdoc/>
    public IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => _featureUsageTrackingSessionFunc(featureName);
}
