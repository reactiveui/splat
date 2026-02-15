// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mindscape.Raygun4Net;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit tests for <see cref="RaygunFeatureUsageTrackingSession"/>.
/// </summary>
public static class RaygunFeatureUsageTrackingSessionTests
{
    /// <summary>
    /// Creates a <see cref="RaygunFeatureUsageTrackingSession"/> configured with an empty API key
    /// for testing purposes.
    /// </summary>
    /// <param name="featureName">The name of the feature to track.</param>
    /// <returns>A new <see cref="RaygunFeatureUsageTrackingSession"/> instance.</returns>
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

    /// <summary>
    /// Tests for the <see cref="RaygunFeatureUsageTrackingSession"/> constructor.
    /// </summary>
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }

    /// <summary>
    /// Tests for the <see cref="RaygunFeatureUsageTrackingSession.SubFeature(string)"/> method.
    /// </summary>
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }
}
