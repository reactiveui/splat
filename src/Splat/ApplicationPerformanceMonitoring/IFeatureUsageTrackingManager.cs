// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// A manager which will generate a <see cref="IFeatureUsageTrackingSession"/> for the specified type.
    /// </summary>
    public interface IFeatureUsageTrackingManager
    {
        /// <summary>
        /// Generate a <see cref="IFeatureUsageTrackingSession"/> for the specified type.
        /// </summary>
        /// <param name="type">The type to generate the feature usage tracking session for.</param>
        /// <returns>The <see cref="IFeatureUsageTrackingSession"/> for the specified type.</returns>
        IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(Type type);
    }
}
