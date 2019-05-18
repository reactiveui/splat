// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindscape.Raygun4Net;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat
{
    /// <summary>
    /// Feature Usage Tracking integration for Raygun.
    /// </summary>
    public sealed class RaygunFeatureUsageTracking : IFeatureUsageTrackingSession<Guid>
    {
        private readonly RaygunClient _raygunClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RaygunFeatureUsageTracking"/> class.
        /// </summary>
        /// <param name="featureName">Name of the feature.</param>
        public RaygunFeatureUsageTracking(string featureName)
            : this(featureName, Guid.Empty)
        {
        }

        internal RaygunFeatureUsageTracking(
            string featureName,
            Guid parentReference)
        {
            if (string.IsNullOrWhiteSpace(featureName))
            {
                throw new ArgumentNullException(nameof(featureName));
            }

            ParentReference = parentReference;
            FeatureName = featureName;
            FeatureReference = Guid.NewGuid();

            var userCustomData = new Dictionary<string, string>
            {
                { "EventType", "FeatureUsage" },
                { "EventReference", FeatureReference.ToString() },
                { "ParentReference", parentReference.ToString() }
            };

            // keep an eye on
            // https://raygun.com/forums/thread/92182
#if NETSTANDARD2_0
            // TODO: check about settings in netstandard version
            var raygunSettings = new RaygunSettings();
            var messageBuilder = RaygunMessageBuilder.New(raygunSettings)
#else
            var messageBuilder = RaygunMessageBuilder.New
#endif
                .SetClientDetails()
                .SetEnvironmentDetails()
                .SetUserCustomData(userCustomData);
            var raygunMessage = messageBuilder.Build();
            _raygunClient.SendInBackground(raygunMessage);
        }

        /// <inheritdoc />
        public Guid ParentReference { get; }

        /// <inheritdoc />
        public Guid FeatureReference { get; }

        /// <inheritdoc />
        public string FeatureName { get; }

        /// <inheritdoc />
        public IFeatureUsageTrackingSession SubFeature(string name)
        {
            return new RaygunFeatureUsageTracking(
                name,
                FeatureReference);
        }

        /// <inheritdoc />
        public void OnException(Exception exception)
        {
            _raygunClient.SendInBackground(exception);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
