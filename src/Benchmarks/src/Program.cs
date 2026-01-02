// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Running;

namespace Splat.Benchmarks;

/// <summary>
/// Class which hosts the main entry point into the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point into the benchmarking application.
    /// </summary>
    /// <param name="args">Arguments from the command line.</param>
    public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}
