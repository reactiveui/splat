// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
