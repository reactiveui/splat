// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Common unit tests for APM View Tracking.
/// </summary>
public static class BaseViewTrackingTests
{
    /// <summary>
    /// Unit Tests for the View Tracking Constructor.
    /// </summary>
    /// <typeparam name="TViewTracking">The type for the view tracking class to construcst.</typeparam>
    public abstract class ConstructorMethod<TViewTracking>
        where TViewTracking : IViewTracking
    {
        /// <summary>
        /// Test to make sure a view tracking session is set up correctly.
        /// </summary>
        [Fact]
        public void ReturnsInstance()
        {
            var instance = GetViewTracking();
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Gets a View Tracking Instance.
        /// </summary>
        /// <returns>View Tracking Instance.</returns>
        protected abstract TViewTracking GetViewTracking();
    }
}
