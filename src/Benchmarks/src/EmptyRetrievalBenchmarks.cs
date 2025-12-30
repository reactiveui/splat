using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for retrieval operations on an empty container.
/// These benchmarks measure the "fast path" performance when no registrations exist.
/// </summary>
[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0, id: nameof(RuntimeMoniker.NativeAot10_0))]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class EmptyRetrievalBenchmarks
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
            ResolverType.Global => new GlobalGenericFirstDependencyResolver(),
            ResolverType.Instance => new InstanceGenericFirstDependencyResolver(),
            // ModernDependencyResolver needs to be instantiated.
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
    [BenchmarkCategory("Empty", "Miss")]
    public void GetService_Empty_Miss()
    {
        // Because the container is empty, this hits the fast path (e.g. HasAnyRegistrations check)
        for (var i = 0; i < Iterations; i++)
        {
            _ = _resolver.GetService<ViewModel>();
        }
    }
}
