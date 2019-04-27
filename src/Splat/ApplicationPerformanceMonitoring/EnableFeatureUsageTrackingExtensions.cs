// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Extensions for the IEnableFeatureUsageTracking interface. This is a similar design to IEnableLogger, to allow
    /// easy use and extension of classes such as ViewModels.
    /// </summary>
    public static class EnableFeatureUsageTrackingExtensions
    {
        /// <summary>
        /// Gets a Feature Usage Tracking Sessions.
        /// </summary>
        /// <param name="instance">instance of class that uses IEnableFeatureUsageTracking.</param>
        /// <param name="featureName">Name of the feature.</param>
        /// <returns>Feature Usage Tracking Session.</returns>
        public static IFeatureUsageTrackingSession FeatureUsageTrackingSession(
            this IEnableFeatureUsageTracking instance,
            string featureName)
        {
            var featureUsageTrackingSession = Locator.Current.GetService<IFeatureUsageTrackingManager>();
            if (featureUsageTrackingSession == null)
            {
                throw new Exception("Feature Usage Tracking Manager is null. This should never happen, your dependency resolver is broken");
            }

            return featureUsageTrackingSession.GetFeatureUsageTrackingSession(featureName);
        }
    }
}
