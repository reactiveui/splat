// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Threading;

namespace Splat;

/// <summary>
/// Per-resolver state object for the instance-scoped GenericFirst dependency resolver.
/// </summary>
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
    /// <summary>
    /// Indicates whether this resolver has ever had at least one registration.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Values:
    /// <list type="bullet">
    ///   <item><description><c>0</c> — No registrations have ever been added.</description></item>
    ///   <item><description><c>1</c> — At least one registration has been added.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This field is written once (transition from 0 → 1) and read frequently on hot paths
    /// to provide a fast exit when no services are registered.
    /// </para>
    /// <para>
    /// An <see cref="int"/> is used to allow Volatile read and write without locking.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "Accessed using Volatile operations for hot-path performance.")]
    public int HasAnyRegistrations;

    /// <summary>
    /// Monotonically increasing identifier source.
    /// </summary>
    /// <remarks>
    /// Used only to generate <see cref="Id"/> values for diagnostics and tracing.
    /// </remarks>
    private static long _nextId;

    /// <summary>
    /// Gets a unique identifier for this resolver state instance.
    /// </summary>
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
    public long Id { get; } = Interlocked.Increment(ref _nextId);
}
