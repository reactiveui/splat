// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>Global generic container for service registrations without contracts.</summary>
/// <remarks>
/// <para>
/// This container stores registrations for <typeparamref name="T"/> as <see cref="Registration{T}"/> values.
/// Mutations are O(1) and rebuild the read snapshot lazily.
/// </para>
/// <para>
/// Thread-safety:
/// <list type="bullet">
///   <item><description>All synchronization is encapsulated in the backing <see cref="ArrayHelpers.Entry{TValue}"/>.</description></item>
///   <item><description>Reads are lock-free when a current snapshot is available.</description></item>
///   <item><description>User factories are invoked outside locks.</description></item>
/// </list>
/// </para>
/// </remarks>
/// <typeparam name="T">The service type.</typeparam>
internal static class Container<T>
{
    /// <summary>Mutable registration list and snapshot/version metadata for <typeparamref name="T"/>.</summary>
    private static readonly ArrayHelpers.Entry<Registration<T>> Entries = new();

    /// <summary>Initializes static members of the <see cref="Container{T}"/> class. Static initialization registers the clear action with the global resolver.</summary>
    static Container() => GlobalGenericFirstDependencyResolver.RegisterClearAction(Clear);

    /// <summary>Gets a value indicating whether this container has any registrations.</summary>
    /// <remarks>
    /// Uses a fast volatile count check to avoid snapshot materialization on the common empty path.
    /// </remarks>
    internal static bool HasRegistrations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Entries.HasItems;
    }

    /// <summary>Gets the number of registrations without invoking any factories.</summary>
    /// <returns>The number of registrations currently stored.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetCount() => Entries.Count;

    /// <summary>Adds a constant instance registration.</summary>
    /// <param name="service">The instance to register (may be <see langword="null"/>).</param>
    /// <remarks>
    /// This is an O(1) operation. Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    internal static void Add(T service) => Entries.Add(Registration<T>.FromInstance(service));

    /// <summary>Adds a factory registration.</summary>
    /// <param name="factory">Factory delegate used to produce instances.</param>
    /// <remarks>
    /// This is an O(1) operation. Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is <see langword="null"/>.</exception>
    internal static void Add(Func<T?> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(factory);
        Entries.Add(Registration<T>.FromFactory(factory));
    }

    /// <summary>Attempts to resolve the most recent registration (last registration wins).</summary>
    /// <param name="instance">Receives the resolved instance when available.</param>
    /// <returns>
    /// <see langword="true"/> if a registration exists and resolves to a non-null instance; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method never holds internal locks while invoking user factories.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryGet([MaybeNullWhen(false)] out T instance)
    {
        // Fast path: avoid snapshot materialization if empty.
        if (!Entries.HasItems)
        {
            instance = default;
            return false;
        }

        var registrations = Entries.GetSnapshot();
        if (registrations.Length == 0)
        {
            // Defensive: should be rare if the count is accurate, but safe under racey Clear/Remove sequences.
            instance = default;
            return false;
        }

        var last = registrations[^1];

        // Invoke user code outside locks (snapshot is immutable).
        if (last.TryGetFactory(out var factory))
        {
            instance = factory.Invoke()!;
            return instance is not null;
        }

        instance = last.GetInstance();
        return instance is not null;
    }

    /// <summary>Resolves all registrations as a materialized array.</summary>
    /// <returns>An array of materialized instances (excluding nulls). Returns an empty array when none exist.</returns>
    /// <remarks>
    /// Factories are invoked during materialization. Exceptions are not caught and propagate to the caller.
    /// </remarks>
    internal static T[] GetAll()
    {
        // Keep the empty fast path.
        if (!Entries.HasItems)
        {
            return [];
        }

        var registrations = Entries.GetSnapshot();
        return registrations.Length == 0 ? [] : ArrayHelpers.MaterializeRegistrations(registrations);
    }

    /// <summary>Removes the most recent registration, if any.</summary>
    /// <remarks>
    /// Snapshot rebuild is deferred until a read occurs.
    /// </remarks>
    internal static void RemoveCurrent() => Entries.RemoveCurrent();

    /// <summary>Clears all registrations from this container.</summary>
    /// <remarks>
    /// Publishes an empty snapshot corresponding to the new version so that subsequent reads can fast-exit.
    /// Readers will observe either the complete old snapshot or the new empty snapshot.
    /// </remarks>
    internal static void Clear() => Entries.Clear();
}
