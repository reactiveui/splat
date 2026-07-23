// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !ANDROID

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Unit Tests for Exceptionless Feature Usage Tracking.</summary>
public static class ExceptionlessFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />>
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ExceptionlessFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override ExceptionlessFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }

    /// <inheritdoc />>
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<ExceptionlessFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override ExceptionlessFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => new(featureName);
    }
}

#endif
