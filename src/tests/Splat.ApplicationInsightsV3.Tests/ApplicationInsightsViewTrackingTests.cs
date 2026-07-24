// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

using Splat.ApplicationInsightsV3;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Tests the Application Insights v3 view tracking adapter.</summary>
public static class ApplicationInsightsViewTrackingTests
{
    /// <inheritdoc />
    [InheritsTests]
    public sealed class ConstructorMethod : BaseViewTrackingTests.ConstructorMethod<ApplicationInsightsViewTracking>
    {
        /// <inheritdoc />
        protected override ApplicationInsightsViewTracking GetViewTracking()
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(telemetryClient);
        }
    }

    /// <summary>Verifies the v3 view tracking call path.</summary>
    public sealed class NavigationTests
    {
        /// <summary>Verifies that navigating by name is accepted by the v3 SDK.</summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Test]
        public async Task OnViewNavigation_WithName_DoesNotThrow()
        {
            var client = CreateClient();
            var viewTracking = new ApplicationInsightsViewTracking(client);

            await Assert.That(() => viewTracking.OnViewNavigation("Home")).ThrowsNothing();
        }

        /// <summary>Creates a disabled v3 client with the required connection string.</summary>
        /// <returns>The configured telemetry client.</returns>
        private static TelemetryClient CreateClient()
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
                DisableTelemetry = true,
            };

            return new(telemetryConfiguration);
        }
    }
}
