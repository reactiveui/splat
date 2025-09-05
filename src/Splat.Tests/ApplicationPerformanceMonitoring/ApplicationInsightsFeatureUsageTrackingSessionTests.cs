// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Splat.ApplicationInsights;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for Application Insights Feature Usage Tracking.
/// </summary>
public static class ApplicationInsightsFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ApplicationInsightsFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override ApplicationInsightsFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(featureName, telemetryClient);
        }
    }

    /// <inheritdoc />
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<ApplicationInsightsFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override ApplicationInsightsFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(featureName, telemetryClient);
        }
    }
}
