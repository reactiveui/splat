// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;

using Mindscape.Raygun4Net;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Unit tests for <see cref="RaygunFeatureUsageTrackingSession"/>.</summary>
public static class RaygunFeatureUsageTrackingSessionTests
{
    /// <summary>Creates a <see cref="RaygunFeatureUsageTrackingSession"/> configured with an empty API key for testing purposes.</summary>
    /// <param name="featureName">The name of the feature to track.</param>
    /// <returns>A new <see cref="RaygunFeatureUsageTrackingSession"/> instance.</returns>
    private static RaygunFeatureUsageTrackingSession GetRaygunFeatureUsageTrackingSession(string featureName)
    {
        var apiKey = string.Empty;
        var raygunSettings = new RaygunSettings
        {
            ApiKey = apiKey,
        };

        var raygunClient = new RaygunClient(raygunSettings);

        return new(featureName, raygunClient, raygunSettings);
    }

    /// <summary>Tests for the <see cref="RaygunFeatureUsageTrackingSession"/> constructor.</summary>
    [InheritsTests]
    public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }

    /// <summary>Tests for the <see cref="RaygunFeatureUsageTrackingSession.SubFeature(string)"/> method.</summary>
    [InheritsTests]
    public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<RaygunFeatureUsageTrackingSession>
    {
        /// <inheritdoc/>
        protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName) => GetRaygunFeatureUsageTrackingSession(featureName);
    }

    /// <summary>Tests for exception reporting and disposal of <see cref="RaygunFeatureUsageTrackingSession"/>.</summary>
    public sealed class ExceptionAndDisposalTests
    {
        /// <summary>The feature name used when constructing test sessions.</summary>
        private const string FeatureName = "feature";

        /// <summary>Verifies <see cref="RaygunFeatureUsageTrackingSession.OnException(Exception)"/> reports the exception without throwing.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task OnException_ReportsWithoutThrowing()
        {
            var settings = new RaygunSettings { ApiKey = string.Empty };
            var client = new RaygunClient(settings);
            using var session = new RaygunFeatureUsageTrackingSession(FeatureName, client, settings);

            await Assert.That(() => session.OnException(new InvalidOperationException("boom"))).ThrowsNothing();
        }

        /// <summary>Verifies disposing the session does not throw.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task Dispose_DoesNotThrow()
        {
            var settings = new RaygunSettings { ApiKey = string.Empty };
            var client = new RaygunClient(settings);
            using var session = new RaygunFeatureUsageTrackingSession(FeatureName, client, settings);

            await Assert.That(session.Dispose).ThrowsNothing();
        }

        /// <summary>Verifies a faulted background send is observed inline without propagating out of <see cref="RaygunFeatureUsageTrackingSession.OnException(Exception)"/>.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task OnException_WhenBackgroundSendFaults_ObservesFaultWithoutThrowing()
        {
            var settings = new RaygunSettings { ApiKey = string.Empty };
            var client = new FaultingRaygunClient(settings);
            using var session = new RaygunFeatureUsageTrackingSession(FeatureName, client, settings);

            // The already-faulted send task runs the fault-only continuation inline; the fault is observed, not rethrown.
            await Assert.That(() => session.OnException(new InvalidOperationException("boom"))).ThrowsNothing();
        }

        /// <summary>A Raygun client whose exception send always returns an already-faulted task.</summary>
        /// <param name="settings">The settings used to configure the underlying client.</param>
        private sealed class FaultingRaygunClient(RaygunSettings settings) : RaygunClient(settings)
        {
            /// <inheritdoc />
            public override Task SendInBackground(Exception exception, IList<string>? tags = null, IDictionary? userCustomData = null, RaygunIdentifierMessage? userInfo = null) =>
                Task.FromException(new InvalidOperationException("send failed"));
        }
    }
}
