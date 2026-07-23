// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Unit Tests for Application Insights Feature Usage Tracking.</summary>
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

    /// <summary>Verifies the page-view telemetry that navigation actually sends to Application Insights.</summary>
    public sealed class NavigationTests
    {
        /// <summary>Verifies that navigating by name sends a page view carrying that name.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task OnViewNavigation_WithName_Sends_PageView()
        {
            var (client, channel) = CapturingTelemetryChannel.CreateClient();
            var viewTracking = new ApplicationInsightsViewTracking(client);

            viewTracking.OnViewNavigation("Home");

            var pageView = channel.Sent.OfType<PageViewTelemetry>().Single();
            await Assert.That(pageView.Name).IsEqualTo("Home");
        }

        /// <summary>Verifies that navigating with supplied telemetry sends that same telemetry instance.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task OnViewNavigation_WithTelemetry_Sends_ProvidedPageView()
        {
            var (client, channel) = CapturingTelemetryChannel.CreateClient();
            var viewTracking = new ApplicationInsightsViewTracking(client);
            var telemetry = new PageViewTelemetry("Details");

            viewTracking.OnViewNavigation(telemetry);

            var pageView = channel.Sent.OfType<PageViewTelemetry>().Single();
            await Assert.That(pageView).IsSameReferenceAs(telemetry);
        }
    }
}
