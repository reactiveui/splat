// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using Splat.ApplicationInsights;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Unit Tests for Application Insights Feature Usage Tracking.</summary>
public static class ApplicationInsightsFeatureUsageTrackingSessionTests
{
    /// <inheritdoc />
    [InheritsTests]
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
    [InheritsTests]
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

    /// <summary>Verifies the telemetry the session actually sends to Application Insights.</summary>
    public sealed class TelemetryEmissionTests
    {
        /// <summary>Verifies that constructing a session sends a "Feature Usage Start" event tagged with the feature metadata.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Constructor_Sends_FeatureUsageStart_Event()
        {
            var (client, channel) = CapturingTelemetryChannel.CreateClient();
            var featureName = Guid.NewGuid().ToString();

            using var session = new ApplicationInsightsFeatureUsageTrackingSession(featureName, client);

            var startEvent = channel.Sent.OfType<EventTelemetry>().Single();
            using (Assert.Multiple())
            {
                await Assert.That(startEvent.Name).IsEqualTo("Feature Usage Start");
                await Assert.That(startEvent.Properties["Name"]).IsEqualTo(featureName);
                await Assert.That(startEvent.Properties["Reference"]).IsEqualTo(session.FeatureReference.ToString());
            }
        }

        /// <summary>Verifies that disposing a session sends a "Feature Usage End" event.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Dispose_Sends_FeatureUsageEnd_Event()
        {
            var (client, channel) = CapturingTelemetryChannel.CreateClient();
            var session = new ApplicationInsightsFeatureUsageTrackingSession(Guid.NewGuid().ToString(), client);

            session.Dispose();

            var endEvent = channel.Sent.OfType<EventTelemetry>().Last();
            await Assert.That(endEvent.Name).IsEqualTo("Feature Usage End");
        }

        /// <summary>Verifies that <see cref="ApplicationInsightsFeatureUsageTrackingSession.OnException(Exception)"/> sends exception telemetry carrying the feature metadata.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task OnException_Sends_ExceptionTelemetry()
        {
            var (client, channel) = CapturingTelemetryChannel.CreateClient();
            var featureName = Guid.NewGuid().ToString();
            using var session = new ApplicationInsightsFeatureUsageTrackingSession(featureName, client);
            var exception = new InvalidOperationException("boom");

            session.OnException(exception);

            var exceptionTelemetry = channel.Sent.OfType<ExceptionTelemetry>().Single();
            using (Assert.Multiple())
            {
                await Assert.That(exceptionTelemetry.Exception).IsSameReferenceAs(exception);
                await Assert.That(exceptionTelemetry.Properties["Name"]).IsEqualTo(featureName);
                await Assert.That(exceptionTelemetry.Properties["Reference"]).IsEqualTo(session.FeatureReference.ToString());
            }
        }
    }
}
