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
        /// <summary>
        /// Unit Tests for the constructor.
        /// </summary>
        public sealed class ConstructorTests : BaseFeatureUsageTrackingTests.BaseConstructorTests<RaygunFeatureUsageTrackingSession>
        {
            /// <inheritdoc/>
            protected override RaygunFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
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
        }
    }
}
