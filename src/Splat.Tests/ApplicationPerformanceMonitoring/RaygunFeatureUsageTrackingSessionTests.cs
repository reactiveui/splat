using System;
using System.Collections.Generic;
using System.Text;
using Mindscape.Raygun4Net;

namespace Splat.Tests.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Unit Tests for Raygun Feature Usage Tracking.
    /// </summary>
    public static class RaygunFeatureUsageTrackingSessionTests
    {
        private static RaygunFeatureUsageTrackingSession GetRaygunFeatureUsageTrackingSession(string featureName)
        {
            var apiKey = string.Empty;
            var raygunSettings = new RaygunSettings
            {
                ApiKey = apiKey
            };

#if NETSTANDARD2_0
            var raygunClient = new RaygunClient(raygunSettings);
#else
            var raygunClient = new RaygunClient(apiKey);
#endif

            return new RaygunFeatureUsageTrackingSession(featureName, raygunClient, raygunSettings);
        }

        /// <inheritdoc />>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return GetRaygunFeatureUsageTrackingSession(featureName);
            }
        }

        /// <inheritdoc />>
        public sealed class SubFeatureMethodTests : BaseFeatureUsageTrackingTests.BaseSubFeatureMethodTests<RaygunFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
            {
                return GetRaygunFeatureUsageTrackingSession(featureName);
            }
        }
    }
}
