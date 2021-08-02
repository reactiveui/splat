// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Splat;

namespace Splat.Benchmarks
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable

    /// <summary>
    /// Benchmarks for the service locator.
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class LocatorBenchmark
    {
        private DependencyResolver _dependencyResolver;

        /// <summary>
        /// Setup method for when running all bench marks.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            _dependencyResolver = new DependencyResolver();
        }

        /// <summary>
        /// Benchmarks getting the current locator.
        /// </summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public IReadonlyDependencyResolver Current() => Locator.Current;

        /// <summary>
        /// Benchmarks getting the current mutable locator.
        /// </summary>
        /// <returns>The dependency resolver.</returns>
        [Benchmark]
        public IMutableDependencyResolver CurrentMutable() => Locator.CurrentMutable;

        /// <summary>
        /// Benchmarks setting the dependency resolver.
        /// </summary>
        [Benchmark]
        public void SetLocator() => Locator.SetLocator(_dependencyResolver);

        /// <summary>
        /// Benchmarks setting the call back for changes.
        /// </summary>
        /// <returns>A disposable.</returns>
        [Benchmark]
        public IDisposable RegisterResolverCallbackChanged() => Locator.RegisterResolverCallbackChanged(() =>
        {
            var foo = "bar";
        });

        /// <summary>
        /// Suppresses the resolver callback changed notifications.
        /// </summary>
        /// <returns>A disposable.</returns>
        [Benchmark]
        public IDisposable SuppressResolverCallbackChangedNotifications() => Locator.SuppressResolverCallbackChangedNotifications();

        /// <summary>
        /// Ares the resolver callback changed notifications enabled.
        /// </summary>
        /// <returns>Whether the change notifications are enabled.</returns>
        [Benchmark]
        public bool AreResolverCallbackChangedNotificationsEnabled() => Locator.AreResolverCallbackChangedNotificationsEnabled();
    }

#pragma warning restore CA1001 // Types that own disposable fields should be disposable
}
