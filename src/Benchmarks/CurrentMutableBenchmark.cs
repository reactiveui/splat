// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks
{
    /// <summary>
    /// Benchmarks for the current mutable locator.
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class CurrentMutableBenchmark
    {
        /// <summary>
        /// Setup method for when running all bench marks.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
        }

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void Register() => Locator.CurrentMutable.Register(() => new ViewModel());

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void RegisterWithContract() => Locator.CurrentMutable.Register(() => new ViewModel(), nameof(ViewModel));

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void RegisterConstant() => Locator.CurrentMutable.RegisterConstant(new ViewModel());

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void RegisterConstantWithContract() => Locator.CurrentMutable.RegisterConstant(new ViewModel(), nameof(ViewModel));

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void RegisterLazySingleton() => Locator.CurrentMutable.RegisterLazySingleton(() => new ViewModel());

        /// <summary>
        /// Benchamrks registering an object.
        /// </summary>
        [Benchmark]
        public void RegisterLazySingletonWithContract() => Locator.CurrentMutable.RegisterLazySingleton(() => new ViewModel(), nameof(ViewModel));
    }
}
