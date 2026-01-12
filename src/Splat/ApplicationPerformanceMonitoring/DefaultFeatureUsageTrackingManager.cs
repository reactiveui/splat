// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Default implementation of the Feature Usage Tracking Manager.
/// </summary>
public sealed class DefaultFeatureUsageTrackingManager : FuncFeatureUsageTrackingManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultFeatureUsageTrackingManager"/> class.
    /// </summary>
    public DefaultFeatureUsageTrackingManager()
        : base(featureName => new DefaultFeatureUsageTrackingSession(featureName))
    {
    }
}
