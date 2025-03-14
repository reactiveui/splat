﻿// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Func based Feature Usage Tracking Manager.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FuncFeatureUsageTrackingManager"/> class.
/// </remarks>
/// <param name="featureUsageTrackingSessionFunc">
/// Factory function for a Feature Usage Tracking Session.
/// </param>
public class FuncFeatureUsageTrackingManager(Func<string, IFeatureUsageTrackingSession> featureUsageTrackingSessionFunc) : IFeatureUsageTrackingManager
{
    private readonly Func<string, IFeatureUsageTrackingSession> _featureUsageTrackingSessionFunc = featureUsageTrackingSessionFunc ??
                                           throw new ArgumentNullException(nameof(featureUsageTrackingSessionFunc));

    /// <inheritdoc/>
    public IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => _featureUsageTrackingSessionFunc(featureName);
}
