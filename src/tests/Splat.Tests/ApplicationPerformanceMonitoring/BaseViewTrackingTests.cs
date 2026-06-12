// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Base class containing shared view tracking tests.</summary>
public static class BaseViewTrackingTests
{
    /// <summary>Tests for the view tracking constructor.</summary>
    /// <typeparam name="TViewTracking">The type of view tracking under test.</typeparam>
    public abstract class ConstructorMethod<TViewTracking>
        where TViewTracking : class, IViewTracking
    {
        /// <summary>Test to make sure a view tracking session is set up correctly.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Test]
        public async Task ReturnsInstance()
        {
            var instance = GetViewTracking();
            await Assert.That(instance).IsNotNull();
        }

        /// <summary>Gets a View Tracking Instance.</summary>
        /// <returns>View Tracking Instance.</returns>
        protected abstract TViewTracking GetViewTracking();
    }
}
