// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Splat;

/// <summary>
/// Non-generic registry for service types accessed via reflection or <see cref="Type"/> parameters.
/// Tracks which types have been registered via non-generic APIs for fallback lookups.
/// </summary>
/// <remarks>
/// Uses per-entry versioned snapshots for lock-free reads and per-entry gates for mutation.
/// </remarks>
internal static class ServiceTypeRegistry
{
#if NET9_0_OR_GREATER
    private static readonly Lock NonGenericGate = new();
#else
    private static readonly object NonGenericGate = new();
#endif

    private static readonly ConcurrentDictionary<(Type ServiceType, string Contract), ArrayHelpers.Entry<Func<object?>>> Entries = [];

    private static readonly HashSet<(Type ServiceType, string Contract)> NonGenericRegistrationSet = [];
    private static HashSet<(Type ServiceType, string Contract)> NonGenericRegistrations = [];

    /// <summary>
    /// Tracks that a type was registered via the non-generic API.
    /// </summary>
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

    /// <summary>
    /// Checks if a type has non-generic registrations.
    /// </summary>
    public static bool HasNonGenericRegistrations(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var snapshot = Volatile.Read(ref NonGenericRegistrations);
        return snapshot.Count != 0 && snapshot.Contains((serviceType, NormalizeContract(contract)));
    }

    /// <summary>
    /// Registers a factory for a service type.
    /// </summary>
    public static void Register(Type serviceType, Func<object?> factory, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        var key = (serviceType, NormalizeContract(contract));
        var entry = Entries.GetOrAdd(key, static _ => new());

        lock (entry.Gate)
        {
            entry.List.Add(factory);
            entry.Version++;
        }
    }

    /// <summary>
    /// Gets the most recent service registration (last registration wins).
    /// </summary>
    public static object? GetService(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return null;
        }

        var factories = EnsureSnapshot(entry);
        return factories.Length == 0 ? null : factories[factories.Length - 1]();
    }

    /// <summary>
    /// Gets all registered services for a type.
    /// </summary>
    public static object[] GetServices(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return [];
        }

        var factories = EnsureSnapshot(entry);
        return factories.Length == 0 ? [] : Materialize(factories);
    }

    /// <summary>
    /// Returns whether any registrations exist.
    /// </summary>
    public static bool HasRegistration(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return false;
        }

        var snapshot = Volatile.Read(ref entry.Snapshot);
        if (snapshot is not null)
        {
            return snapshot.Length != 0;
        }

        lock (entry.Gate)
        {
            return entry.List.Count != 0;
        }
    }

    /// <summary>
    /// Gets the number of registrations for a type.
    /// </summary>
    public static int GetCount(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        if (!Entries.TryGetValue((serviceType, NormalizeContract(contract)), out var entry))
        {
            return 0;
        }

        var snapshot = Volatile.Read(ref entry.Snapshot);
        if (snapshot is not null)
        {
            return snapshot.Length;
        }

        lock (entry.Gate)
        {
            return entry.List.Count;
        }
    }

    /// <summary>
    /// Removes the most recent registration.
    /// </summary>
    public static void UnregisterCurrent(Type serviceType, string? contract = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceType);

        var key = (serviceType, NormalizeContract(contract));

        if (!Entries.TryGetValue(key, out var entry))
        {
            return;
        }

        var becameEmpty = false;

        lock (entry.Gate)
        {
            var list = entry.List;
            if (list.Count == 0)
            {
                return;
            }

            list.RemoveAt(list.Count - 1);
            entry.Version++;
            becameEmpty = list.Count == 0;
        }

        if (becameEmpty)
        {
            Entries.TryRemove(key, out _);

            lock (NonGenericGate)
            {
                NonGenericRegistrationSet.Remove(key);
                PublishNonGenericSnapshot_NoThrow();
            }
        }
    }

    /// <summary>
    /// Removes all registrations for a type.
    /// </summary>
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

    /// <summary>
    /// Clears all registrations.
    /// </summary>
    public static void Clear()
    {
        Entries.Clear();

        lock (NonGenericGate)
        {
            NonGenericRegistrationSet.Clear();
            Volatile.Write(ref NonGenericRegistrations, []);
        }
    }

    /// <summary>
    /// Returns a snapshot of all registered factories for disposal.
    /// </summary>
    public static Func<object?>[] GetAllFactoriesForDisposal()
    {
        var entriesSnapshot = Entries.ToArray();

        var total = 0;
        for (var i = 0; i < entriesSnapshot.Length; i++)
        {
            var entry = entriesSnapshot[i].Value;
            lock (entry.Gate)
            {
                total += entry.List.Count;
            }
        }

        if (total == 0)
        {
            return [];
        }

        var result = new Func<object?>[total];
        var idx = 0;

        for (var i = 0; i < entriesSnapshot.Length; i++)
        {
            var entry = entriesSnapshot[i].Value;
            lock (entry.Gate)
            {
                var list = entry.List;
                for (var j = 0; j < list.Count; j++)
                {
                    result[idx++] = list[j];
                }
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Func<object?>[] EnsureSnapshot(ArrayHelpers.Entry<Func<object?>> entry)
    {
        var version = Volatile.Read(ref entry.Version);
        var snapshot = Volatile.Read(ref entry.Snapshot);

        if (snapshot is not null && Volatile.Read(ref entry.SnapshotVersion) == version)
        {
            return snapshot;
        }

        lock (entry.Gate)
        {
            snapshot = entry.Snapshot;
            var currentVersion = entry.Version;

            if (snapshot is null || entry.SnapshotVersion != currentVersion)
            {
                Func<object?>[] newSnapshot = entry.List.Count == 0 ? [] : [.. entry.List];
                entry.Snapshot = newSnapshot;
                entry.SnapshotVersion = currentVersion;
                return newSnapshot;
            }

            return snapshot;
        }
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PublishNonGenericSnapshot_NoThrow()
    {
        Volatile.Write(ref NonGenericRegistrations, [.. NonGenericRegistrationSet]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string NormalizeContract(string? contract) => contract ?? string.Empty;
}
