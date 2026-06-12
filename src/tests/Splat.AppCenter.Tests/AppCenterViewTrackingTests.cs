// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Unit Tests for App Center View Tracking.</summary>
public static class AppCenterViewTrackingTests
{
    /// <inheritdoc/>
    [InheritsTests]
    public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<AppCenterViewTracking>
    {
        /// <inheritdoc/>
        protected override AppCenterViewTracking GetViewTracking() => new();
    }
}
