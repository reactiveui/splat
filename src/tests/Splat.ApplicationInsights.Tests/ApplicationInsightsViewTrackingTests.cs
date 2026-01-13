// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for Application Insights Feature Usage Tracking.
/// </summary>
public static class ApplicationInsightsViewTrackingTests
{
    /// <inheritdoc/>
    [InheritsTests]
    public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<ApplicationInsightsViewTracking>
    {
        /// <inheritdoc/>
        protected override ApplicationInsightsViewTracking GetViewTracking()
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(telemetryClient);
        }
    }
}
