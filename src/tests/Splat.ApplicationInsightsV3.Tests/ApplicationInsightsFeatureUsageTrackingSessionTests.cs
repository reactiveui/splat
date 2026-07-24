// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

using Splat.ApplicationInsightsV3;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Tests the Application Insights v3 feature usage adapter.</summary>
public static class ApplicationInsightsFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<ApplicationInsightsFeatureUsageTrackingSession>
    {
        /// <inheritdoc />
        protected override ApplicationInsightsFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(featureName, telemetryClient);
        }
    }

    /// <inheritdoc />
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<ApplicationInsightsFeatureUsageTrackingSession>
    {
        /// <inheritdoc />
        protected override ApplicationInsightsFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
                DisableTelemetry = true,
            };
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            return new(featureName, telemetryClient);
        }
    }

    /// <summary>Verifies the v3 feature usage call paths.</summary>
    public sealed class TelemetryEmissionTests
    {
        /// <summary>Verifies that constructing a session is accepted by the v3 SDK.</summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Test]
        public async Task Constructor_WithFeatureName_DoesNotThrow()
        {
            var client = CreateClient();
            var featureName = Guid.NewGuid().ToString();

            await Assert.That(() => new ApplicationInsightsFeatureUsageTrackingSession(featureName, client).Dispose()).ThrowsNothing();
        }

        /// <summary>Verifies that disposing a session is accepted by the v3 SDK.</summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Test]
        public async Task Dispose_WithStartedSession_DoesNotThrow()
        {
            var client = CreateClient();
            var session = new ApplicationInsightsFeatureUsageTrackingSession(Guid.NewGuid().ToString(), client);

            await Assert.That(session.Dispose).ThrowsNothing();
        }

        /// <summary>Verifies that exception tracking is accepted by the v3 SDK.</summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Test]
        public async Task OnException_WithException_DoesNotThrow()
        {
            var client = CreateClient();
            var featureName = Guid.NewGuid().ToString();
            using var session = new ApplicationInsightsFeatureUsageTrackingSession(featureName, client);
            var exception = new InvalidOperationException("boom");

            await Assert.That(() => session.OnException(exception)).ThrowsNothing();
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
