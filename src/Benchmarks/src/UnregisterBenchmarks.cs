// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for unregister operations.
/// Pre-populates resolvers to measure only unregistration cost.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
public class UnregisterBenchmarks
{
    private IMutableDependencyResolver _resolver = null!;

    [ParamsAllValues]
    public ResolverType Resolver { get; set; }

    [Params(BenchmarkConstants.SmallMutationCount, BenchmarkConstants.MediumMutationCount, BenchmarkConstants.LargeMutationCount)]
    public int RegistrationCount { get; set; }

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

        for (var i = 0; i < RegistrationCount; i++)
        {
            _resolver.Register(() => new ViewModel());
        }
    }

    [IterationCleanup]
    public void Cleanup()
    {
        (_resolver as IDisposable)?.Dispose();
        GlobalGenericFirstDependencyResolver.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    }

    [Benchmark]
    [BenchmarkCategory("Unregister", "Current")]
    public void UnregisterCurrent()
    {
        for (var i = 0; i < RegistrationCount; i++)
        {
            _resolver.UnregisterCurrent<ViewModel>();
        }
    }

    [Benchmark]
    [BenchmarkCategory("Unregister", "All")]
    public void UnregisterAll()
    {
        for (var i = 0; i < RegistrationCount; i++)
        {
            _resolver.Register(() => new ViewModel());
            _resolver.UnregisterAll<ViewModel>();
        }
    }
}
