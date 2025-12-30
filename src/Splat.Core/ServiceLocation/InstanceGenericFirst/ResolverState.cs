// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Per-resolver state object for instance-scoped GenericFirst resolver.
/// Used as key in ConditionalWeakTable to maintain per-resolver containers.
/// </summary>
internal sealed class ResolverState
{
    /// <summary>
    /// Tracks whether this resolver has ever had any registrations.
    /// 0 = False (never registered), 1 = True (has registrations).
    /// Using int allows for atomic Volatile.Read/Write without locking.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Used with concurrency operators, needs to be a field and public")]
    public int HasAnyRegistrations;

    private static long _nextId;

    /// <summary>
    /// Gets a unique identifier for this resolver instance.
    /// </summary>
    public long Id { get; } = Interlocked.Increment(ref _nextId);
}
