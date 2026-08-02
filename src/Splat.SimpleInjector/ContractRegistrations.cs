// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.SimpleInjector;

/// <summary>Holds the service factories that were registered against a contract, keyed by service type and contract.</summary>
/// <remarks>
/// Simple Injector deliberately offers no named or keyed registrations, so a contract cannot be expressed inside the
/// container itself. Contract registrations are therefore held in this table beside the container and resolved from
/// it. Keeping them out of the container is what stops two implementations registered under different contracts from
/// silently overwriting one another, and what keeps a contract registration invisible to a contract-less lookup.
/// </remarks>
internal sealed class ContractRegistrations
{
    /// <summary>Serializes access to <see cref="_factories"/>.</summary>
    private readonly Lock _lockObject = new();

    /// <summary>The registered factories, keyed by service type and contract, held in registration order.</summary>
    private readonly Dictionary<(Type ServiceType, string Contract), List<Func<object?>>> _factories = [];

    /// <summary>Records a factory against the supplied service type and contract.</summary>
    /// <param name="serviceType">The service type the factory produces.</param>
    /// <param name="contract">The contract the registration is keyed by.</param>
    /// <param name="factory">The factory that produces the service.</param>
    internal void Add(Type serviceType, string contract, Func<object?> factory)
    {
        lock (_lockObject)
        {
#if NET6_0_OR_GREATER
            ref var registered = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(_factories, (serviceType, contract), out _);
            registered ??= [];
#else
            if (!_factories.TryGetValue((serviceType, contract), out var registered))
            {
                registered = [];
                _factories.Add((serviceType, contract), registered);
            }
#endif

            registered.Add(factory);
        }
    }

    /// <summary>Determines whether anything is registered for the supplied service type and contract.</summary>
    /// <param name="serviceType">The service type to look for.</param>
    /// <param name="contract">The contract to look for.</param>
    /// <returns><see langword="true"/> when at least one factory is registered; otherwise <see langword="false"/>.</returns>
    internal bool Contains(Type serviceType, string contract)
    {
        lock (_lockObject)
        {
            return _factories.TryGetValue((serviceType, contract), out var registered) && registered.Count > 0;
        }
    }

    /// <summary>Resolves the most recently registered factory for the supplied service type and contract.</summary>
    /// <param name="serviceType">The service type to resolve.</param>
    /// <param name="contract">The contract to resolve under.</param>
    /// <returns>The produced service, or <see langword="null"/> when nothing is registered under that contract.</returns>
    internal object? ResolveLast(Type serviceType, string contract)
    {
        var factories = Snapshot(serviceType, contract);

        return factories.Length == 0 ? null : factories[^1]();
    }

    /// <summary>Resolves every factory registered for the supplied service type and contract, in registration order.</summary>
    /// <param name="serviceType">The service type to resolve.</param>
    /// <param name="contract">The contract to resolve under.</param>
    /// <returns>The produced services; empty when nothing is registered under that contract.</returns>
    internal IEnumerable<object> ResolveAll(Type serviceType, string contract)
    {
        var factories = Snapshot(serviceType, contract);
        if (factories.Length == 0)
        {
            return [];
        }

        var services = new List<object>(factories.Length);
        foreach (var factory in factories)
        {
            var service = factory();
            if (service is not null)
            {
                services.Add(service);
            }
        }

        return services;
    }

    /// <summary>Removes every factory registered for the supplied service type and contract.</summary>
    /// <param name="serviceType">The service type to remove registrations for.</param>
    /// <param name="contract">The contract to remove registrations for.</param>
    internal void RemoveAll(Type serviceType, string contract)
    {
        lock (_lockObject)
        {
            _ = _factories.Remove((serviceType, contract));
        }
    }

    /// <summary>Copies every registration held here into <paramref name="destination"/>, preserving registration order.</summary>
    /// <param name="destination">The table the registrations are copied into.</param>
    internal void CopyTo(ContractRegistrations destination)
    {
        List<(Type ServiceType, string Contract, Func<object?> Factory)> snapshot;

        lock (_lockObject)
        {
            snapshot = new(_factories.Count);
            foreach (var entry in _factories)
            {
                foreach (var factory in entry.Value)
                {
                    snapshot.Add((entry.Key.ServiceType, entry.Key.Contract, factory));
                }
            }
        }

        foreach (var (serviceType, contract, factory) in snapshot)
        {
            destination.Add(serviceType, contract, factory);
        }
    }

    /// <summary>Takes a point-in-time copy of the factories for a key so they can be invoked outside the lock.</summary>
    /// <param name="serviceType">The service type to snapshot.</param>
    /// <param name="contract">The contract to snapshot.</param>
    /// <returns>The registered factories, or an empty array when the key is unknown.</returns>
    private Func<object?>[] Snapshot(Type serviceType, string contract)
    {
        lock (_lockObject)
        {
            return _factories.TryGetValue((serviceType, contract), out var registered) ? [.. registered] : [];
        }
    }
}
