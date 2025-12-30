// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for mutation operations: Register, RegisterConstant, RegisterLazySingleton.
/// These benchmarks create clean resolvers for each iteration to measure pure mutation cost.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
public class MutationBenchmarks
{
    private IMutableDependencyResolver _resolver = null!;

    [ParamsAllValues]
    public ResolverType Resolver { get; set; }

    [Params(BenchmarkConstants.SmallMutationCount, BenchmarkConstants.MediumMutationCount, BenchmarkConstants.LargeMutationCount)]
    public int MutationCount { get; set; }

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
        (_resolver as IDisposable)?.Dispose();
        GlobalGenericFirstDependencyResolver.Clear();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    }

    [Benchmark]
    [BenchmarkCategory("Register", "Generic")]
    public void Register_Generic()
    {
        for (var i = 0; i < MutationCount; i++)
        {
            _resolver.Register(() => new ViewModel());
        }
    }

    [Benchmark]
    [BenchmarkCategory("Register", "Type")]
    public void Register_Type()
    {
        for (var i = 0; i < MutationCount; i++)
        {
            _resolver.Register(() => new ViewModel(), typeof(ViewModel));
        }
    }

    [Benchmark]
    [BenchmarkCategory("Register", "WithContract")]
    public void Register_WithContract()
    {
        for (var i = 0; i < MutationCount; i++)
        {
            _resolver.Register(() => new ViewModel(), BenchmarkConstants.Contract1);
        }
    }

    [Benchmark]
    [BenchmarkCategory("RegisterConstant")]
    public void RegisterConstant()
    {
        for (var i = 0; i < MutationCount; i++)
        {
            _resolver.RegisterConstant(new ViewModel());
        }
    }

    [Benchmark]
    [BenchmarkCategory("RegisterLazySingleton")]
    public void RegisterLazySingleton()
    {
        for (var i = 0; i < MutationCount; i++)
        {
            _resolver.RegisterLazySingleton(() => new ViewModel());
        }
    }
}
