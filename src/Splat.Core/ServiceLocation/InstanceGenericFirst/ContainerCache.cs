// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Per-type static cache for instance-scoped containers keyed by <see cref="ResolverState"/>.
/// </summary>
/// <remarks>
/// <para>
/// This cache is used by instance-scoped resolvers to store per-resolver registrations for <typeparamref name="T"/>.
/// The backing store is a <see cref="ConditionalWeakTable{TKey,TValue}"/> so that containers are reclaimed when the
/// associated <see cref="ResolverState"/> becomes unreachable.
/// </para>
/// <para>
/// The container uses a mutable list guarded by a per-entry gate and a published snapshot array for lock-free reads.
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContainerCache<T>
{
    /// <summary>
    /// Maps resolver state to the per-resolver container for <typeparamref name="T"/>.
    /// </summary>
    private static readonly ConditionalWeakTable<ResolverState, Container> Containers = new();

    /// <summary>
    /// Gets or creates the container for the specified resolver state.
    /// </summary>
    /// <param name="state">The resolver state used as the cache key.</param>
    /// <returns>The per-resolver <see cref="Container"/> for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Container Get(ResolverState state)
    {
        ArgumentExceptionHelper.ThrowIfNull(state);
        return Containers.GetOrCreateValue(state);
    }

    /// <summary>
    /// Per-resolver container for registrations of <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The most recent registration wins for <see cref="TryGet"/> (last registration wins).
    /// </para>
    /// <para>
    /// Thread-safety:
    /// <list type="bullet">
    ///   <item><description>Mutations are synchronized using <c>lock(_entry.Gate)</c>.</description></item>
    ///   <item><description>Reads are lock-free when a current snapshot is available.</description></item>
    ///   <item><description>User factories are invoked without holding any internal locks.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    internal sealed class Container
    {
        /// <summary>
        /// Registration list and snapshot/version metadata.
        /// </summary>
        /// <remarks>
        /// The associated <see cref="ArrayHelpers.Entry{TValue}.Gate"/> is used for synchronization.
        /// </remarks>
        private readonly ArrayHelpers.Entry<Registration<T>> _entry = new();

        /// <summary>
        /// Gets a value indicating whether the container has at least one registration.
        /// </summary>
        /// <remarks>
        /// This uses the published snapshot when available; otherwise, it falls back to checking the list under the entry gate.
        /// </remarks>
        public bool HasRegistrations
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var snapshot = Volatile.Read(ref _entry.Snapshot);
                if (snapshot is not null)
                {
                    return snapshot.Length != 0;
                }

                lock (_entry.Gate)
                {
                    return _entry.List.Count != 0;
                }
            }
        }

        /// <summary>
        /// Gets the number of registrations currently stored in this container.
        /// </summary>
        /// <returns>The number of registrations.</returns>
        /// <remarks>
        /// This uses the published snapshot length when available; otherwise, it reads the list count under the entry gate.
        /// </remarks>
        public int GetCount()
        {
            var snapshot = Volatile.Read(ref _entry.Snapshot);
            if (snapshot is not null)
            {
                return snapshot.Length;
            }

            lock (_entry.Gate)
            {
                return _entry.List.Count;
            }
        }

        /// <summary>
        /// Attempts to resolve the most recent registration (last registration wins).
        /// </summary>
        /// <param name="instance">Receives the resolved instance when available.</param>
        /// <returns>
        /// <see langword="true"/> if a registration exists and produces a non-null instance; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method does not hold internal locks while invoking user factories.
        /// </remarks>
        public bool TryGet([MaybeNullWhen(false)] out T instance)
        {
            // Fast exit when we already know we have no registrations (snapshot available and empty).
            var snapshot = Volatile.Read(ref _entry.Snapshot);
            if (snapshot is not null && snapshot.Length == 0)
            {
                instance = default;
                return false;
            }

            var registrations = EnsureSnapshot();
            if (registrations.Length == 0)
            {
                instance = default;
                return false;
            }

            var last = registrations[registrations.Length - 1];

            if (last.TryGetFactory(out var factory))
            {
                instance = factory.Invoke()!;
                return instance is not null;
            }

            instance = last.GetInstance();
            return instance is not null;
        }

        /// <summary>
        /// Resolves all registrations in this container.
        /// </summary>
        /// <returns>
        /// An array of resolved instances. Returns an empty array when no registrations exist.
        /// </returns>
        /// <remarks>
        /// Factories are invoked during materialization. Exceptions are not caught and propagate to the caller.
        /// </remarks>
        public T[] GetAll()
        {
            var registrations = EnsureSnapshot();
            return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
        }

        /// <summary>
        /// Registers a constant instance.
        /// </summary>
        /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
        public void Add(T service)
        {
            lock (_entry.Gate)
            {
                _entry.List.Add(Registration<T>.FromInstance(service));
                _entry.Version++;

                // Snapshot becomes stale; rebuilt lazily.
            }
        }

        /// <summary>
        /// Registers a factory delegate.
        /// </summary>
        /// <param name="factory">The factory used to produce instances.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
        public void Add(Func<T?> factory)
        {
            ArgumentExceptionHelper.ThrowIfNull(factory);

            lock (_entry.Gate)
            {
                _entry.List.Add(Registration<T>.FromFactory(factory));
                _entry.Version++;
            }
        }

        /// <summary>
        /// Removes the most recent registration, if any.
        /// </summary>
        public void RemoveCurrent()
        {
            lock (_entry.Gate)
            {
                var list = _entry.List;
                if (list.Count == 0)
                {
                    return;
                }

                list.RemoveAt(list.Count - 1);
                _entry.Version++;
            }
        }

        /// <summary>
        /// Removes all registrations from this container.
        /// </summary>
        /// <remarks>
        /// Publishes an empty snapshot to keep the read path fast after a clear.
        /// </remarks>
        public void Clear()
        {
            lock (_entry.Gate)
            {
                _entry.List.Clear();
                _entry.Version++;

                // Publish an empty snapshot corresponding to the current version.
                _entry.Snapshot = [];
                _entry.SnapshotVersion = _entry.Version;
            }
        }

        /// <summary>
        /// Returns a current snapshot of registrations, rebuilding it if stale.
        /// </summary>
        /// <returns>A snapshot array representing the current registration list.</returns>
        /// <remarks>
        /// Fast path is lock-free when the snapshot version matches the list version.
        /// Snapshot rebuild occurs under <c>lock(_entry.Gate)</c> and allocates an array copy of the list.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Registration<T>[] EnsureSnapshot()
        {
            var version = Volatile.Read(ref _entry.Version);
            var snapshot = Volatile.Read(ref _entry.Snapshot);

            if (snapshot is not null && Volatile.Read(ref _entry.SnapshotVersion) == version)
            {
                return snapshot;
            }

            lock (_entry.Gate)
            {
                snapshot = _entry.Snapshot;
                var currentVersion = _entry.Version;

                if (snapshot is null || _entry.SnapshotVersion != currentVersion)
                {
                    Registration<T>[] newSnapshot = _entry.List.Count == 0 ? [] : [.. _entry.List];
                    _entry.Snapshot = newSnapshot;
                    _entry.SnapshotVersion = currentVersion;
                    return newSnapshot;
                }

                return snapshot;
            }
        }
    }
}
