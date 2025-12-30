// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mindscape.Raygun4Net;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for Raygun Feature Usage Tracking.
/// </summary>
public static class RaygunFeatureUsageTrackingSessionTests
{
    private static RaygunFeatureUsageTrackingSession GetRaygunFeatureUsageTrackingSession(string featureName)
    {
        var apiKey = string.Empty;
        var raygunSettings = new RaygunSettings
        {
            ApiKey = apiKey,
        };

        var raygunClient = new RaygunClient(raygunSettings);

        return new(featureName, raygunClient, raygunSettings);
    }

    /// <inheritdoc />>
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }

    /// <inheritdoc />>
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }
}
