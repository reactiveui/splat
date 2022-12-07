// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

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
    public FuncFeatureUsageTrackingManager(Func<string, IFeatureUsageTrackingSession> featureUsageTrackingSessionFunc) =>
        _featureUsageTrackingSessionFunc = featureUsageTrackingSessionFunc ??
                                           throw new ArgumentNullException(nameof(featureUsageTrackingSessionFunc));

    /// <inheritdoc/>
    public IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => _featureUsageTrackingSessionFunc(featureName);
}
