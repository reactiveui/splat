// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for retrieval operations: GetService, GetServices, HasRegistration.
/// These benchmarks use realistic pre-populated data to measure read performance.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "Benchmarking generic vs non-generic")]
public class RetrievalBenchmarks
{
    private IDependencyResolver _resolver = null!;

    [ParamsAllValues]
    public ResolverType Resolver { get; set; }

    [Params(BenchmarkConstants.SmallIterations, BenchmarkConstants.MediumIterations, BenchmarkConstants.LargeIterations)]
    public int Iterations { get; set; }

    [IterationSetup]
    public void Setup()
    {
        _resolver = Resolver switch
        {
            ResolverType.Global => new GlobalGenericFirstDependencyResolver(r =>
            {
                for (var i = 0; i < BenchmarkConstants.DataSetSize; i++)
                {
                    r.Register(() => new ViewModel());
                }

                r.Register(() => new ViewModel(), BenchmarkConstants.Contract1);
            }),
            ResolverType.Instance => new InstanceGenericFirstDependencyResolver(r =>
            {
                for (var i = 0; i < BenchmarkConstants.DataSetSize; i++)
                {
                    r.Register(() => new ViewModel());
                }

                r.Register(() => new ViewModel(), BenchmarkConstants.Contract1);
            }),
            ResolverType.Modern => CreateModernResolver(),
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

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GetService", "Generic", "Hit")]
    public void GetService_Generic_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService<ViewModel>();
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetService", "Generic", "Miss")]
    public void GetService_Generic_Miss()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService<string>();
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetService", "Type", "Hit")]
    public void GetService_Type_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService(typeof(ViewModel));
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetService", "Contract", "Hit")]
    public void GetService_Contract_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService<ViewModel>(BenchmarkConstants.Contract1);
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetService", "Contract", "Miss")]
    public void GetService_Contract_Miss()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService<ViewModel>("NonExistent");
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetServices", "Generic")]
    public void GetServices_Generic()
    {
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var _ in _resolver.GetServices<ViewModel>())
            {
                // Enumerate all
            }
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetServices", "Materialized")]
    public void GetServices_Materialized()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = new List<ViewModel>(_resolver.GetServices<ViewModel>());
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetServices", "Type")]
    public void GetServices_Type()
    {
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var _ in _resolver.GetServices(typeof(ViewModel)))
            {
                // Enumerate all
            }
        }
    }

    [Benchmark]
    [BenchmarkCategory("GetServices", "Contract")]
    public void GetServices_Contract()
    {
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var _ in _resolver.GetServices<ViewModel>(BenchmarkConstants.Contract1))
            {
                // Enumerate all
            }
        }
    }

    [Benchmark]
    [BenchmarkCategory("HasRegistration", "Generic", "Hit")]
    public void HasRegistration_Generic_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.HasRegistration<ViewModel>();
        }
    }

    [Benchmark]
    [BenchmarkCategory("HasRegistration", "Generic", "Miss")]
    public void HasRegistration_Generic_Miss()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.HasRegistration<string>();
        }
    }

    [Benchmark]
    [BenchmarkCategory("HasRegistration", "Type", "Hit")]
    public void HasRegistration_Type_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.HasRegistration(typeof(ViewModel));
        }
    }

    [Benchmark]
    [BenchmarkCategory("HasRegistration", "Contract", "Hit")]
    public void HasRegistration_Contract_Hit()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.HasRegistration<ViewModel>(BenchmarkConstants.Contract1);
        }
    }

    [Benchmark]
    [BenchmarkCategory("HasRegistration", "Contract", "Miss")]
    public void HasRegistration_Contract_Miss()
    {
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.HasRegistration<ViewModel>("NonExistent");
        }
    }

    private ModernDependencyResolver CreateModernResolver()
    {
        var resolver = new ModernDependencyResolver();
        for (var i = 0; i < BenchmarkConstants.DataSetSize; i++)
        {
            resolver.Register(() => new ViewModel());
        }

        resolver.Register(() => new ViewModel(), BenchmarkConstants.Contract1);
        return resolver;
    }
}
