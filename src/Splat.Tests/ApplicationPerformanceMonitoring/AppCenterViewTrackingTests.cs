// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for App Center View Tracking.
/// </summary>
public static class AppCenterViewTrackingTests
{
    /// <inheritdoc/>
    public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<AppCenterViewTracking>
    {
        /// <inheritdoc/>
        protected override AppCenterViewTracking GetViewTracking() => new();
    }
}
