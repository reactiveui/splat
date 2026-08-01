// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Splat.Benchmarks;

/// <summary>
/// Benchmarks for the ambient service-location entry points that sit in front of every resolution:
/// <c>AppLocator.Current</c>, <c>AppLocator.CurrentMutable</c> and <c>AppLocator.GetLocator()</c>.
/// The "WithinScope" variants measure the same accessors while a <c>WithResolver</c> scope is installed.
/// </summary>
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Benchmarks its fine")]
public class AppLocatorAccessBenchmarks
{
    private InstanceGenericFirstDependencyResolver _scopedResolver = null!;

    [Params(BenchmarkConstants.LargeIterations)]
    public int Iterations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        AppLocator.CurrentMutable.Register(() => new ViewModel());

        _scopedResolver = new InstanceGenericFirstDependencyResolver();
        _scopedResolver.Register(() => new ViewModel());
    }

    [GlobalCleanup]
    public void Cleanup() => _scopedResolver.Dispose();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Current")]
    public int Current()
    {
        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.Current is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("CurrentMutable")]
    public int CurrentMutable()
    {
        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.CurrentMutable is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("GetLocator")]
    public int GetLocator()
    {
        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.GetLocator() is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("GetService")]
    public int GetService()
    {
        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.GetService<ViewModel>() is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("Current", "WithinScope")]
    public int Current_WithinResolverScope()
    {
        using var scope = _scopedResolver.WithResolver();

        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.Current is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("GetService", "WithinScope")]
    public int GetService_WithinResolverScope()
    {
        using var scope = _scopedResolver.WithResolver();

        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            accumulator += AppLocator.GetService<ViewModel>() is null ? 0 : 1;
        }

        return accumulator;
    }

    [Benchmark]
    [BenchmarkCategory("WithResolver")]
    public int WithResolver_EnterAndExit()
    {
        var accumulator = 0;
        for (var i = 0; i < Iterations; i++)
        {
            using var scope = _scopedResolver.WithResolver();
            accumulator += AppLocator.Current is null ? 0 : 1;
        }

        return accumulator;
    }
}
