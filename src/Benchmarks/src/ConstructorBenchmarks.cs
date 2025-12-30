// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for constructor-based bulk registration patterns.
/// These benchmarks measure the cost of resolver construction + bulk registration + disposal.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
public class ConstructorBenchmarks
{
    [ParamsAllValues]
    public ResolverType Resolver { get; set; }

    [Params(BenchmarkConstants.MediumMutationCount, BenchmarkConstants.LargeMutationCount)]
    public int RegistrationCount { get; set; }

    [IterationCleanup]
    public void Cleanup()
    {
        GlobalGenericFirstDependencyResolver.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    }

    [Benchmark]
    [BenchmarkCategory("BulkRegister", "Constructor")]
    public void BulkRegister_Constructor()
    {
        IDependencyResolver resolver = Resolver switch
        {
            ResolverType.Global => new GlobalGenericFirstDependencyResolver(r =>
            {
                for (var i = 0; i < RegistrationCount; i++)
                {
                    r.Register(() => new ViewModel());
                }
            }),
            ResolverType.Instance => new InstanceGenericFirstDependencyResolver(r =>
            {
                for (var i = 0; i < RegistrationCount; i++)
                {
                    r.Register(() => new ViewModel());
                }
            }),
            ResolverType.Modern => new ModernDependencyResolver(),
            _ => throw new InvalidOperationException($"Unknown resolver type: {Resolver}"),
        };

        resolver.Dispose();
    }
}
