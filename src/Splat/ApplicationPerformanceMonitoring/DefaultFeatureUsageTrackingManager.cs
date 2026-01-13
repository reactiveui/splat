// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Provides the default implementation of a feature usage tracking manager that creates tracking sessions for features
/// using the standard session type.
/// </summary>
/// <remarks>This class is typically used when no custom tracking behavior is required. It creates a new <see
/// cref="DefaultFeatureUsageTrackingSession"/> for each feature name provided. The class is sealed and cannot be
/// inherited.</remarks>
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
