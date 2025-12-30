// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for App Center Feature Usage Tracking.
/// </summary>
public static class AppCenterFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<AppCenterFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }

    /// <inheritdoc />
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<AppCenterFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }
}
