#if !WINDOWS_UWP

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for Application Insights Feature Usage Tracking.
    /// </summary>
    public static class ApplicationInsightsViewTrackingTests
    {
        /// <inheritdoc/>
        public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<ApplicationInsightsViewTracking>
        {
            /// <inheritdoc/>
            protected override ApplicationInsightsViewTracking GetViewTracking()
            {
                var telemetryConfiguration = new TelemetryConfiguration
                {
                    DisableTelemetry = true
                };
                var telemetryClient = new TelemetryClient(telemetryConfiguration);

                return new ApplicationInsightsViewTracking(telemetryClient);
            }
        }
    }
}

#endif
