// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Threading;

namespace Splat;

/// <summary>Per-resolver state object for the instance-scoped GenericFirst dependency resolver.</summary>
/// <remarks>
/// <para>
/// Each resolver instance owns exactly one <see cref="ResolverState"/>.
/// This state object is used as the key in <see cref="ConditionalWeakTable{TKey, TValue}"/>
/// to associate resolver-specific containers and caches.
/// </para>
/// <para>
/// Resolver lifetime:
/// <list type="bullet">
///   <item><description>
///     When a resolver is disposed, its state reference is replaced.
///   </description></item>
///   <item><description>
///     Once no references remain, all associated ConditionalWeakTable entries are eligible for GC.
///   </description></item>
/// </list>
/// </para>
/// <para>
/// This type intentionally contains only minimal mutable state and does not override equality or hashing.
/// Identity semantics are relied upon by the runtime.
/// </para>
/// </remarks>
internal sealed class ResolverState
{
    /// <summary>Monotonically increasing identifier source.</summary>
    /// <remarks>
    /// Used only to generate <see cref="Id"/> values for diagnostics and tracing.
    /// </remarks>
    private static long _nextId;

    /// <summary>Backing flag for <see cref="HasAnyRegistrations"/>; <c>0</c> = none added, <c>1</c> = at least one added.</summary>
    private int _hasAnyRegistrations;

    /// <summary>Gets a value indicating whether this resolver has ever had at least one registration.</summary>
    /// <remarks>
    /// <para>
    /// The flag is written once (transition from <see langword="false"/> to <see langword="true"/>) and read frequently
    /// on hot paths to provide a fast exit when no services are registered.
    /// </para>
    /// <para>
    /// Backed by an <see cref="int"/> so the read and the one-way transition use lock-free
    /// <see cref="Volatile"/> / <see cref="Interlocked"/> operations.
    /// </para>
    /// </remarks>
    internal bool HasAnyRegistrations => Volatile.Read(ref _hasAnyRegistrations) != 0;

    /// <summary>Gets a unique identifier for this resolver state instance.</summary>
    /// <remarks>
    /// <para>
    /// This identifier is intended for diagnostics, logging, and debugging only.
    /// It is not used for equality or lookup semantics.
    /// </para>
    /// <para>
    /// Values are assigned using <see cref="Interlocked.Increment(ref long)"/> and are unique
    /// within the current process.
    /// </para>
    /// </remarks>
    internal long Id { get; } = Interlocked.Increment(ref _nextId);

    /// <summary>Marks that at least one registration has been added (one-way <c>false</c> → <c>true</c> transition).</summary>
    /// <returns><see langword="true"/> if this call performed the transition; otherwise <see langword="false"/>.</returns>
    internal bool MarkHasRegistrations() => Interlocked.CompareExchange(ref _hasAnyRegistrations, 1, 0) == 0;
}
