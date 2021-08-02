// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks
{
    /// <summary>
    /// Benchmarks for the current locator.
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class CurrentBenchmark
    {
        /// <summary>
        /// Setup method for when running all bench marks.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            Locator.CurrentMutable.Register(() => new ViewModel());
        }

        /// <summary>
        /// Benchamrks returning an object.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public ViewModel GetService() => (ViewModel)Locator.Current.GetService(typeof(ViewModel));

        /// <summary>
        /// Benchamrks returning an object.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public ViewModel GetServiceWithContract() => (ViewModel)Locator.Current.GetService(typeof(ViewModel), nameof(ViewModel));

        /// <summary>
        /// Benchamrks returning an object.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public ViewModel GetServiceGeneric() => Locator.Current.GetService<ViewModel>();

        /// <summary>
        /// Benchamrks returning an object.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public ViewModel GetServiceGenericWithContract() => Locator.Current.GetService<ViewModel>(nameof(ViewModel));

        /// <summary>
        /// Benchamrks returning an enumerable of objects.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public List<ViewModel> GetServices() => new(Locator.Current.GetServices<ViewModel>());

        /// <summary>
        /// Benchamrks returning an enumerable of objects.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public List<ViewModel> GetServicesWithContract() => new(Locator.Current.GetServices<ViewModel>(nameof(ViewModel)));

        /// <summary>
        /// Benchamrks returning an enumerable of objects.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public IEnumerable<ViewModel> GetServicesGeneric() => Locator.Current.GetServices<ViewModel>();

        /// <summary>
        /// Benchamrks returning an enumerable of objects.
        /// </summary>
        /// <returns>The object.</returns>
        [Benchmark]
        public IEnumerable<ViewModel> GetServicesGenericWithContract() => Locator.Current.GetServices<ViewModel>(nameof(ViewModel));
    }
}
