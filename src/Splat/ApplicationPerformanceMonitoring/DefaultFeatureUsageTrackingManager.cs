// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
