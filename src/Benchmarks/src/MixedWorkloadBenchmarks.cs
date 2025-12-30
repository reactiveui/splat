// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for mixed workload and edge case scenarios.
/// Tests realistic usage patterns and boundary conditions.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
public class MixedWorkloadBenchmarks
{
    private IDependencyResolver _resolver = null!;

    [ParamsAllValues]
    public ResolverType Resolver { get; set; }

    [IterationSetup]
    public void Setup()
    {
        _resolver = Resolver switch
        {
            ResolverType.Global => new GlobalGenericFirstDependencyResolver(),
            ResolverType.Instance => new InstanceGenericFirstDependencyResolver(),
            ResolverType.Modern => new ModernDependencyResolver(),
            _ => throw new InvalidOperationException($"Unknown resolver type: {Resolver}"),
        };
    }

    [IterationCleanup]
    public void Cleanup()
    {
        _resolver?.Dispose();
        GlobalGenericFirstDependencyResolver.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    }

    [Benchmark]
    [BenchmarkCategory("Mixed", "Realistic")]
    public void MixedWorkload_Realistic()
    {
        // Register various types
        for (var i = 0; i < 100; i++)
        {
            _resolver.Register(() => new ViewModel());
            _resolver.Register(() => new ViewModel(), BenchmarkConstants.Contract1);
            _resolver.RegisterConstant(new ViewModel());
            _resolver.RegisterLazySingleton(() => new ViewModel());
        }

        // Resolve services
        for (var i = 0; i < 500; i++)
        {
            _ = _resolver.GetService<ViewModel>();
            _ = _resolver.GetService<ViewModel>(BenchmarkConstants.Contract1);
            _ = _resolver.HasRegistration<ViewModel>();
        }

        // Enumerate all services
        for (var i = 0; i < 100; i++)
        {
            foreach (var _ in _resolver.GetServices<ViewModel>())
            {
                // Enumerate
            }
        }
    }
}
