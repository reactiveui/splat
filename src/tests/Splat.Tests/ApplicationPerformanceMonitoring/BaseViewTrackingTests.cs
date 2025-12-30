// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>
/// Common unit tests for APM View Tracking.
/// </summary>
[TestFixture]
public static class BaseViewTrackingTests
{
    /// <summary>
    /// Unit Tests for the View Tracking Constructor.
    /// </summary>
    /// <typeparam name="TViewTracking">The type for the view tracking class to construcst.</typeparam>
    [TestFixture]
    public abstract class ConstructorMethod<TViewTracking>
        where TViewTracking : IViewTracking
    {
        /// <summary>
        /// Test to make sure a view tracking session is set up correctly.
        /// </summary>
        [Test]
        public void ReturnsInstance()
        {
            var instance = GetViewTracking();
            Assert.That(instance, Is.Not.Null);
        }

        /// <summary>
        /// Gets a View Tracking Instance.
        /// </summary>
        /// <returns>View Tracking Instance.</returns>
        protected abstract TViewTracking GetViewTracking();
    }
}
