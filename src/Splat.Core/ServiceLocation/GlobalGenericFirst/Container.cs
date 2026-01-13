// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Global generic container for service registrations without contracts.
/// </summary>
/// <remarks>
/// <para>
/// This container stores registrations for <typeparamref name="T"/> as <see cref="Registration{T}"/> values.
/// Mutations are O(1) and rebuild the read snapshot lazily.
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>All mutations are synchronized via <c>lock(Entries.Gate)</c>.</description></item>
///   <item><description>Reads are lock-free when a current snapshot is available.</description></item>
///   <item><description>User factories are invoked outside locks.</description></item>
/// </list>
/// </para>
/// <para>
/// A volatile count is maintained to provide a very fast "empty" check without snapshot materialization.
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class Container<T>
{
    /// <summary>
    /// Mutable registration list and snapshot/version metadata for <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The associated <see cref="ArrayHelpers.Entry{TValue}.Gate"/> is used for synchronization.
    /// </remarks>
    private static readonly ArrayHelpers.Entry<Registration<T>> Entries = new();

    /// <summary>
    /// Cached registration count for fast empty checks.
    /// </summary>
    /// <remarks>
    /// Written under <c>lock(Entries.Gate)</c> and read with a Volatile Read.
    /// </remarks>
    private static int _count;

    /// <summary>
    /// Initializes static members of the <see cref="Container{T}"/> class.
    /// Static initialization registers the clear action with the global resolver.
    /// </summary>
    static Container() => GlobalGenericFirstDependencyResolver.RegisterClearAction(Clear);

    /// <summary>
    /// Gets a value indicating whether this container has any registrations.
    /// </summary>
    /// <remarks>
    /// Uses a fast volatile count check to avoid snapshot materialization on the common empty path.
    /// </remarks>
    public static bool HasRegistrations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _count) > 0;
    }

    /// <summary>
    /// Gets the number of registrations without invoking any factories.
    /// </summary>
    /// <returns>The number of registrations currently stored.</returns>
    /// <remarks>
    /// This returns the cached count, which is updated under the entry gate on every mutation.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetCount() => Volatile.Read(ref _count);

    /// <summary>
    /// Adds a constant instance registration.
    /// </summary>
    /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
    /// <remarks>
    /// This is an O(1) operation. Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    public static void Add(T service)
    {
        lock (Entries.Gate)
        {
            Entries.List.Add(Registration<T>.FromInstance(service));
            Entries.Version++;
            Volatile.Write(ref _count, Entries.List.Count);
        }
    }

    /// <summary>
    /// Adds a factory registration.
    /// </summary>
    /// <param name="factory">Factory delegate used to produce instances.</param>
    /// <remarks>
    /// This is an O(1) operation. Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static void Add(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);

        lock (Entries.Gate)
        {
            Entries.List.Add(Registration<T>.FromFactory(factory));
            Entries.Version++;
            Volatile.Write(ref _count, Entries.List.Count);
        }
    }

    /// <summary>
    /// Attempts to resolve the most recent registration (last registration wins).
    /// </summary>
    /// <param name="instance">Receives the resolved instance when available.</param>
    /// <returns>
    /// <see langword="true"/> if a registration exists and resolves to a non-null instance; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method never holds internal locks while invoking user factories.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet([MaybeNullWhen(false)] out T instance)
    {
        // Fast path: avoid snapshot materialization if empty.
        if (Volatile.Read(ref _count) == 0)
        {
            instance = default;
            return false;
        }

        var registrations = EnsureSnapshot();
        if (registrations.Length == 0)
        {
            // Defensive: should be rare if _count is accurate, but safe under racey Clear/Remove sequences.
            instance = default;
            return false;
        }

        var last = registrations[registrations.Length - 1];

        // Invoke user code outside locks (snapshot is immutable).
        if (last.TryGetFactory(out var factory))
        {
            instance = factory.Invoke()!;
            return instance is not null;
        }

        instance = last.GetInstance();
        return instance is not null;
    }

    /// <summary>
    /// Resolves all registrations as a materialized array.
    /// </summary>
    /// <returns>An array of materialized instances (excluding nulls). Returns an empty array when none exist.</returns>
    /// <remarks>
    /// Factories are invoked during materialization. Exceptions are not caught and propagate to the caller.
    /// </remarks>
    public static T[] GetAll()
    {
        // Keep the empty fast path.
        if (Volatile.Read(ref _count) == 0)
        {
            return [];
        }

        var registrations = EnsureSnapshot();
        return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>
    /// Removes the most recent registration, if any.
    /// </summary>
    /// <remarks>
    /// Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    public static void RemoveCurrent()
    {
        lock (Entries.Gate)
        {
            var list = Entries.List;
            var count = list.Count;
            if (count == 0)
            {
                return;
            }

            list.RemoveAt(count - 1);
            Entries.Version++;
            Volatile.Write(ref _count, list.Count);

            // If we became empty, publish an empty snapshot aligned with the current version
            // so subsequent readers can fast-exit without rebuilding.
            if (list.Count == 0)
            {
                Entries.Snapshot = [];
                Entries.SnapshotVersion = Entries.Version;
            }
        }
    }

    /// <summary>
    /// Clears all registrations from this container.
    /// </summary>
    /// <remarks>
    /// Publishes an empty snapshot corresponding to the new version so that subsequent reads can fast-exit.
    /// Readers will observe either the complete old snapshot or the new empty snapshot.
    /// </remarks>
    public static void Clear()
    {
        lock (Entries.Gate)
        {
            Entries.List.Clear();
            Entries.Version++;

            Entries.Snapshot = [];
            Entries.SnapshotVersion = Entries.Version;

            Volatile.Write(ref _count, 0);
        }
    }

    /// <summary>
    /// Ensures the published snapshot is current, rebuilding it if stale.
    /// </summary>
    /// <returns>A snapshot array representing the current registration list.</returns>
    /// <remarks>
    /// Fast path is lock-free when the snapshot version matches the list version.
    /// Snapshot rebuild occurs under <c>lock(Entries.Gate)</c> and allocates an array copy of the list.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Registration<T>[] EnsureSnapshot()
    {
        var version = Volatile.Read(ref Entries.Version);
        var snapshot = Volatile.Read(ref Entries.Snapshot);

        if (snapshot is not null && Volatile.Read(ref Entries.SnapshotVersion) == version)
        {
            return snapshot;
        }

        lock (Entries.Gate)
        {
            snapshot = Entries.Snapshot;
            var currentVersion = Entries.Version;

            if (snapshot is null || Entries.SnapshotVersion != currentVersion)
            {
                Registration<T>[] newSnapshot = Entries.List.Count == 0 ? [] : [.. Entries.List];
                Entries.Snapshot = newSnapshot;
                Entries.SnapshotVersion = currentVersion;
                return newSnapshot;
            }

            return snapshot;
        }
    }
}
