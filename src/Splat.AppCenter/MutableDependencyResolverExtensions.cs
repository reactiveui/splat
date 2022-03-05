// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat.AppCenter
{
    /// <summary>
    /// Exceptionless specific extensions for the Mutable Dependency Resolver.
    /// </summary>
    public static class MutableDependencyResolverExtensions
    {
        /// <summary>
        /// Simple helper to initialize AppCenter within Splat with the Application Performance Management tracking.
        /// </summary>
        /// <param name="instance">
        /// An instance of Mutable Dependency Resolver.
        /// </param>
        /// <example>
        /// <code>
        /// Locator.CurrentMutable.UseAppCenterForApplicationPerformanceMonitoring();
        /// </code>
        /// </example>
        public static void UseAppCenterForApplicationPerformanceMonitoring(this IMutableDependencyResolver instance)
        {
            Func<string, IFeatureUsageTrackingSession> featureUsageTrackingSessionFunc =
                s => new AppCenterFeatureUsageTrackingSession(s);

            instance.RegisterConstant(new FuncFeatureUsageTrackingManager(featureUsageTrackingSessionFunc), typeof(IFeatureUsageTrackingManager));
        }
    }
}
