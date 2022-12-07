// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for App Center Feature Usage Tracking.
/// </summary>
public static class AppCenterFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<AppCenterFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }

    /// <inheritdoc />
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<AppCenterFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override AppCenterFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }
}
