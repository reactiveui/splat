// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Unit Tests for Application Insights View Tracking.
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
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000",
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(telemetryClient);
        }
    }

    /// <summary>
    /// Tests for the <see cref="ApplicationInsightsViewTracking.OnViewNavigation"/> method.
    /// </summary>
    public sealed class OnViewNavigationMethod
    {
        /// <summary>
        /// Verifies that tracking a page view with a valid name does not throw.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task TracksPageView()
        {
            var viewTracking = CreateViewTracking();

            viewTracking.OnViewNavigation("HomePage");

            await Assert.That(viewTracking).IsNotNull();
        }

        /// <summary>
        /// Verifies that tracking multiple consecutive page views does not throw.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task TracksMultiplePageViews()
        {
            var viewTracking = CreateViewTracking();

            viewTracking.OnViewNavigation("HomePage");
            viewTracking.OnViewNavigation("SettingsPage");
            viewTracking.OnViewNavigation("ProfilePage");

            await Assert.That(viewTracking).IsNotNull();
        }

        /// <summary>
        /// Verifies that tracking a page view with an empty name does not throw.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task TracksEmptyViewName()
        {
            var viewTracking = CreateViewTracking();

            viewTracking.OnViewNavigation(string.Empty);

            await Assert.That(viewTracking).IsNotNull();
        }

        /// <summary>
        /// Verifies that tracking a page view with a name containing special characters does not throw.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task TracksViewNameWithSpecialCharacters()
        {
            var viewTracking = CreateViewTracking();

            viewTracking.OnViewNavigation("Views/Home Page (Main)");

            await Assert.That(viewTracking).IsNotNull();
        }

        /// <summary>
        /// Creates an <see cref="ApplicationInsightsViewTracking"/> instance configured for testing.
        /// </summary>
        /// <returns>A new <see cref="ApplicationInsightsViewTracking"/> instance with telemetry disabled.</returns>
        private static ApplicationInsightsViewTracking CreateViewTracking()
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = true,
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000",
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(telemetryClient);
        }
    }
}
