using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Splat.ApplicationInsights;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for Application Insights Feature Usage Tracking.
    /// </summary>
    public static class ApplicationInsightsFeatureUsageTrackingSessionTests
    {
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ApplicationInsightsFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override ApplicationInsightsFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                var telemetryConfiguration = new TelemetryConfiguration
                {
                    DisableTelemetry = true
                };
                var telemetryClient = new TelemetryClient(telemetryConfiguration);

                return new ApplicationInsightsFeatureUsageTrackingSession(featureName, telemetryClient);
            }
        }
    }
}
