// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Benchmarks;

/// <summary>
/// Resolver types for benchmark parameterization.
/// </summary>
public enum ResolverType
{
    /// <summary>
    /// GlobalGenericFirstDependencyResolver - static process-wide containers.
    /// </summary>
    Global,

    /// <summary>
    /// InstanceGenericFirstDependencyResolver - per-instance ConditionalWeakTable containers.
    /// </summary>
    Instance,

    /// <summary>
    /// ModernDependencyResolver - legacy dictionary-based resolver.
    /// </summary>
    Modern,
}
