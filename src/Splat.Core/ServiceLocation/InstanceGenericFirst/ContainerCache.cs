// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>Per-type static cache for instance-scoped containers keyed by <see cref="ResolverState"/>.</summary>
/// <remarks>
/// <para>
/// This cache is used by instance-scoped resolvers to store per-resolver registrations for <typeparamref name="T"/>.
/// The backing store is a <see cref="ConditionalWeakTable{TKey,TValue}"/> so that containers are reclaimed when the
/// associated <see cref="ResolverState"/> becomes unreachable.
/// </para>
/// <para>
/// The container delegates storage and synchronization to a per-entry <see cref="ArrayHelpers.Entry{TValue}"/>, which
/// guards a mutable list with its own gate and publishes a snapshot array for lock-free reads.
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class ContainerCache<T>
{
    /// <summary>Maps resolver state to the per-resolver container for <typeparamref name="T"/>.</summary>
    private static readonly ConditionalWeakTable<ResolverState, Container> Containers = new();

    /// <summary>Gets or creates the container for the specified resolver state.</summary>
    /// <param name="state">The resolver state used as the cache key.</param>
    /// <returns>The per-resolver <see cref="Container"/> for <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Container Get(ResolverState state)
    {
        ArgumentExceptionHelper.ThrowIfNull(state);
        return Containers.GetOrCreateValue(state);
    }

    /// <summary>Per-resolver container for registrations of <typeparamref name="T"/>.</summary>
    /// <remarks>
    /// <para>
    /// The most recent registration wins for <see cref="TryGet"/> (last registration wins).
    /// </para>
    /// <para>
    /// Thread-safety:
    /// <list type="bullet">
    ///   <item><description>Mutations and snapshot rebuilds are synchronized inside the backing <see cref="ArrayHelpers.Entry{TValue}"/>.</description></item>
    ///   <item><description>Reads are lock-free when a current snapshot is available.</description></item>
    ///   <item><description>User factories are invoked without holding any internal locks.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    internal sealed class Container
    {
        /// <summary>Registration list and snapshot/version metadata.</summary>
        private readonly ArrayHelpers.Entry<Registration<T>> _entry = new();

        /// <summary>Gets a value indicating whether the container has at least one registration.</summary>
        /// <remarks>Lock-free volatile count check.</remarks>
        internal bool HasRegistrations
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entry.HasItems;
        }

        /// <summary>Gets the number of registrations currently stored in this container.</summary>
        /// <returns>The number of registrations.</returns>
        /// <remarks>Lock-free volatile count read.</remarks>
        internal int GetCount() => _entry.Count;

        /// <summary>Attempts to resolve the most recent registration (last registration wins).</summary>
        /// <param name="instance">Receives the resolved instance when available.</param>
        /// <returns>
        /// <see langword="true"/> if a registration exists and produces a non-null instance; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method does not hold internal locks while invoking user factories.
        /// </remarks>
        internal bool TryGet([MaybeNullWhen(false)] out T instance)
        {
            // Fast exit when we already know we have no registrations.
            if (!_entry.HasItems)
            {
                instance = default;
                return false;
            }

            var registrations = _entry.GetSnapshot();
            if (registrations.Length == 0)
            {
                instance = default;
                return false;
            }

            var last = registrations[^1];

            if (last.TryGetFactory(out var factory))
            {
                instance = factory.Invoke()!;
                return instance is not null;
            }

            instance = last.GetInstance();
            return instance is not null;
        }

        /// <summary>Resolves all registrations in this container.</summary>
        /// <returns>
        /// An array of resolved instances. Returns an empty array when no registrations exist.
        /// </returns>
        /// <remarks>
        /// Factories are invoked during materialization. Exceptions are not caught and propagate to the caller.
        /// </remarks>
        internal T[] GetAll()
        {
            var registrations = _entry.GetSnapshot();
            return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
        }

        /// <summary>Registers a constant instance.</summary>
        /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
        internal void Add(T service) => _entry.Add(Registration<T>.FromInstance(service));

        /// <summary>Registers a factory delegate.</summary>
        /// <param name="factory">The factory used to produce instances.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
        internal void Add(Func<T?> factory)
        {
            ArgumentExceptionHelper.ThrowIfNull(factory);
            _entry.Add(Registration<T>.FromFactory(factory));
        }

        /// <summary>Removes the most recent registration, if any.</summary>
        internal void RemoveCurrent() => _entry.RemoveCurrent();

        /// <summary>Removes all registrations from this container.</summary>
        /// <remarks>
        /// Publishes an empty snapshot to keep the read path fast after a clear.
        /// </remarks>
        internal void Clear() => _entry.Clear();
    }
}
