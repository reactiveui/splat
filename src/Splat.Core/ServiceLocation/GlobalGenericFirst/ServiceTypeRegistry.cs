// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Non-generic registry for service types accessed via reflection or <see cref="Type"/> parameters.
/// Tracks which types have been registered via non-generic APIs for fallback lookups.
/// </summary>
/// <remarks>
/// Uses per-entry versioned snapshots for lock-free reads; each <see cref="ArrayHelpers.Entry{TValue}"/> encapsulates its own mutation gate.
/// </remarks>
internal static class ServiceTypeRegistry
{
    /// <summary>Serializes updates to the non-generic registration tracking sets.</summary>
    private static readonly Lock NonGenericGate = new();

    /// <summary>Per-(service type, contract) entries holding the registered non-generic factories.</summary>
    private static readonly ConcurrentDictionary<(Type ServiceType, string Contract), ArrayHelpers.Entry<Func<object?>>> Entries = [];

    /// <summary>Set of (service type, contract) pairs registered via the non-generic API; guarded by <see cref="NonGenericGate"/>.</summary>
    private static readonly HashSet<(Type ServiceType, string Contract)> NonGenericRegistrationSet = [];

    /// <summary>Snapshot view of <see cref="NonGenericRegistrationSet"/> swapped atomically for lock-free enumeration.</summary>
    private static HashSet<(Type ServiceType, string Contract)> _nonGenericRegistrations = [];

    /// <summary>Tracks that a type was registered via the non-generic API.</summary>
    /// <param name="serviceType">The service type that was registered.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static void TrackNonGenericRegistration(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var key = (serviceType, NormalizeContract(contract));

        lock (NonGenericGate)
        {
            NonGenericRegistrationSet.Add(key);
            PublishNonGenericSnapshot_NoThrow();
        }
    }

    /// <summary>Checks if a type has non-generic registrations.</summary>
    /// <param name="serviceType">The service type to test.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <returns><see langword="true"/> if a non-generic registration exists; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static bool HasNonGenericRegistrations(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var snapshot = Volatile.Read(ref _nonGenericRegistrations);
        return snapshot.Count != 0 && snapshot.Contains((serviceType, NormalizeContract(contract)));
    }

    /// <summary>Registers a factory for a service type.</summary>
    /// <param name="serviceType">The service type being registered.</param>
    /// <param name="factory">Factory delegate that produces instances.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> or <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static void Register(Type serviceType, Func<object?> factory, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var entry = Entries.GetOrAdd((serviceType, NormalizeContract(contract)), static _ => new());
        entry.Add(factory);
    }

    /// <summary>Gets the most recent service registration (last registration wins).</summary>
    /// <param name="serviceType">The service type to resolve.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <returns>The resolved instance, or <see langword="null"/> when no registration exists or the factory returns <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static object? GetService(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return null;
        }

        var factories = entry.GetSnapshot();
        return factories.Length == 0 ? null : factories[factories.Length - 1]();
    }

    /// <summary>Gets all registered services for a type.</summary>
    /// <param name="serviceType">The service type to resolve.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <returns>An array of resolved services (excluding nulls). Returns an empty array when none exist.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static object[] GetServices(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return [];
        }

        var factories = entry.GetSnapshot();
        return factories.Length == 0 ? [] : Materialize(factories);
    }

    /// <summary>Returns whether any registrations exist.</summary>
    /// <param name="serviceType">The service type to test.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <returns><see langword="true"/> if at least one registration exists; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static bool HasRegistration(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        return Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry) && entry.HasItems;
    }

    /// <summary>Gets the number of registrations for a type.</summary>
    /// <param name="serviceType">The service type to count.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <returns>The number of registrations for the type and contract.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static int GetCount(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        return Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry) ? entry.Count : 0;
    }

    /// <summary>Removes the most recent registration.</summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static void UnregisterCurrent(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var key = (serviceType, NormalizeContract(contract));

        if (!Entries.TryGetValue(key, out var entry) || !entry.RemoveCurrent())
        {
            return;
        }

        Entries.TryRemove(key, out _);

        lock (NonGenericGate)
        {
            NonGenericRegistrationSet.Remove(key);
            PublishNonGenericSnapshot_NoThrow();
        }
    }

    /// <summary>Removes all registrations for a type.</summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">Optional contract key; <see langword="null"/> maps to the default contract.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> is <see langword="null"/>.</exception>
    public static void UnregisterAll(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var key = (serviceType, NormalizeContract(contract));

        if (!Entries.TryRemove(key, out _))
        {
            return;
        }

        lock (NonGenericGate)
        {
            NonGenericRegistrationSet.Remove(key);
            PublishNonGenericSnapshot_NoThrow();
        }
    }

    /// <summary>Clears all registrations.</summary>
    public static void Clear()
    {
        Entries.Clear();

        lock (NonGenericGate)
        {
            NonGenericRegistrationSet.Clear();
            Volatile.Write(ref _nonGenericRegistrations, []);
        }
    }

    /// <summary>Returns a snapshot of all registered factories for disposal.</summary>
    /// <returns>An array containing every registered factory across all entries at the time of the call.</returns>
    public static Func<object?>[] GetAllFactoriesForDisposal()
    {
        var entriesSnapshot = Entries.ToArray();

        var factories = new List<Func<object?>>();
        for (var i = 0; i < entriesSnapshot.Length; i++)
        {
            entriesSnapshot[i].Value.CopyItemsTo(factories);
        }

        return factories.Count == 0 ? [] : [.. factories];
    }

    /// <summary>Materializes an array of factories, invoking each factory once and excluding null results.</summary>
    /// <param name="factories">Factories to invoke.</param>
    /// <returns>An array of non-null results.</returns>
    private static object[] Materialize(Func<object?>[] factories)
    {
        var tmp = new object[factories.Length];
        var idx = 0;

        for (var i = 0; i < factories.Length; i++)
        {
            var value = factories[i]();
            if (value is not null)
            {
                tmp[idx++] = value;
            }
        }

        if (idx == 0)
        {
            return [];
        }

        if (idx == tmp.Length)
        {
            return tmp;
        }

        var result = new object[idx];
        Array.Copy(tmp, result, idx);
        return result;
    }

    /// <summary>Publishes a new immutable snapshot from the tracked non-generic registration set.</summary>
    /// <remarks>The caller must hold <see cref="NonGenericGate"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PublishNonGenericSnapshot_NoThrow() =>
        Volatile.Write(ref _nonGenericRegistrations, [.. NonGenericRegistrationSet]);

    /// <summary>Normalizes a contract key for internal storage and lookup.</summary>
    /// <param name="contract">Contract string, possibly <see langword="null"/>.</param>
    /// <returns>The normalized contract key. Never <see langword="null"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string NormalizeContract(string? contract) => contract ?? string.Empty;
}
