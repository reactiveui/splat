// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Benchmarks;

/// <summary>
/// Shared constants used across all benchmark suites.
/// </summary>
internal static class BenchmarkConstants
{
    /// <summary>
    /// Number of mutation operations for small workloads.
    /// </summary>
    public const int SmallMutationCount = 50;

    /// <summary>
    /// Number of mutation operations for medium workloads.
    /// </summary>
    public const int MediumMutationCount = 150;

    /// <summary>
    /// Number of mutation operations for large workloads.
    /// </summary>
    public const int LargeMutationCount = 300;

    /// <summary>
    /// Number of registrations in pre-populated datasets.
    /// </summary>
    public const int DataSetSize = 150;

    /// <summary>
    /// Number of iterations for retrieval operations on small datasets.
    /// </summary>
    public const int SmallIterations = 50;

    /// <summary>
    /// Number of iterations for retrieval operations on medium datasets.
    /// </summary>
    public const int MediumIterations = 100;

    /// <summary>
    /// Number of iterations for retrieval operations on large datasets.
    /// </summary>
    public const int LargeIterations = 300;

    /// <summary>
    /// First contract name used in benchmarks.
    /// </summary>
    public const string Contract1 = "Contract1";

    /// <summary>
    /// Second contract name used in benchmarks.
    /// </summary>
    public const string Contract2 = "Contract2";
}
