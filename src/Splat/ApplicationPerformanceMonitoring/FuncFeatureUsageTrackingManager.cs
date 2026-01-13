// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Provides a feature usage tracking manager that creates tracking sessions using a supplied factory function.
/// </summary>
/// <remarks>This implementation allows customization of feature usage tracking session creation by accepting a
/// factory delegate. It is useful when session instantiation logic needs to be injected or varied at runtime.</remarks>
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
