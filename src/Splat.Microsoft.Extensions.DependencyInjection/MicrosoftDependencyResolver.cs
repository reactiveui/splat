// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Microsoft DI implementation for <see cref="IDependencyResolver"/>.
/// </summary>
/// <seealso cref="IDependencyResolver" />
public class MicrosoftDependencyResolver : IDependencyResolver
{
    private const string ImmutableExceptionMessage = "This container has already been built and cannot be modified.";
    private readonly object _syncLock = new();
    private IServiceCollection? _serviceCollection;
    private bool _isImmutable;
    private IServiceProvider? _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with an <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    public MicrosoftDependencyResolver(IServiceCollection? services = null) => _serviceCollection = services ?? new ServiceCollection();

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with a configured service Provider.
    /// </summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public MicrosoftDependencyResolver(IServiceProvider serviceProvider) =>
        UpdateContainer(serviceProvider);

    /// <summary>
    /// Gets the internal Microsoft conainer,
    /// or build new if this instance was not initialized with one.
    /// </summary>
    protected virtual IServiceProvider? ServiceProvider
    {
        get
        {
            lock (_syncLock)
            {
                _serviceProvider ??= _serviceCollection?.BuildServiceProvider();

                return _serviceProvider;
            }
        }
    }

    /// <summary>
    /// Updates this instance with a configured service Provider.
    /// </summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public void UpdateContainer(IServiceProvider serviceProvider)
    {
#if NETSTANDARD || NETFRAMEWORK
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }
#else
        ArgumentNullException.ThrowIfNull(serviceProvider);
#endif

        lock (_syncLock)
        {
            _serviceCollection = null;
            _serviceProvider = serviceProvider;
            _isImmutable = true;
        }
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract = null) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract = null)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException("The ServiceProvider is null.");
        }

        var isNull = serviceType is null;
        serviceType ??= typeof(NullServiceType);

        IEnumerable<object> services = [];

        if (contract is null || string.IsNullOrWhiteSpace(contract))
        {
            // this is to deal with CS8613 that GetServices returns IEnumerable<object?>?
            services = ServiceProvider.GetServices(serviceType)
                .Where(a => a is not null)
                .Select(a => a!);
        }
        else if (ServiceProvider is IKeyedServiceProvider serviceProvider)
        {
            services = serviceProvider.GetKeyedServices(serviceType, contract)
                .Where(a => a is not null)
                .Select(a => a!);
        }

        if (isNull)
        {
            services = services
                .Cast<NullServiceType>()
                .Select(nst => nst.Factory()!);
        }

        return services;
    }

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract = null)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var isNull = serviceType is null;

        serviceType ??= typeof(NullServiceType);

        lock (_syncLock)
        {
            if (contract is null || string.IsNullOrWhiteSpace(contract))
            {
                _serviceCollection?.AddTransient(serviceType, _ =>
                isNull
                ? new NullServiceType(factory)
                : factory()!);
            }
            else
            {
                _serviceCollection?.AddKeyedTransient(serviceType, contract, (_, __) =>
                isNull
                ? new NullServiceType(factory)
                : factory()!);
            }

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public virtual void UnregisterCurrent(Type? serviceType, string? contract = null)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= typeof(NullServiceType);

        lock (_syncLock)
        {
            if (contract is null || string.IsNullOrWhiteSpace(contract))
            {
                var sd = _serviceCollection?.LastOrDefault(s => !s.IsKeyedService && s.ServiceType == serviceType);
                if (sd is not null)
                {
                    _serviceCollection?.Remove(sd);
                }
            }
            else
            {
                var sd = _serviceCollection?.LastOrDefault(sd => MatchesKeyedContract(serviceType, contract, sd));
                if (sd is not null)
                {
                    _serviceCollection?.Remove(sd);
                }
            }

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract - or -
    /// If the container has already been built, removes the specified contract (scope) entirely,
    /// ignoring the <paramref name="serviceType"/> argument.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">A optional value which will remove only an object registered with the same contract.</param>
    public virtual void UnregisterAll(Type? serviceType, string? contract = null)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= typeof(NullServiceType);

        lock (_syncLock)
        {
            if (_serviceCollection is null)
            {
                // required so that it gets rebuilt if not injected externally.
                _serviceProvider = null;
                return;
            }

            IEnumerable<ServiceDescriptor> sds = [];

            sds = contract is null || string.IsNullOrWhiteSpace(contract)
                ? _serviceCollection.Where(s => !s.IsKeyedService && s.ServiceType == serviceType)
                : _serviceCollection
                  .Where(sd => MatchesKeyedContract(serviceType, contract, sd));

            foreach (var sd in sds.ToList())
            {
                _serviceCollection.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            _serviceProvider = null;
        }
    }

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException();

    /// <inheritdoc/>
    public virtual bool HasRegistration(Type? serviceType, string? contract = null)
    {
        serviceType ??= typeof(NullServiceType);

        if (!_isImmutable)
        {
            return contract is null || string.IsNullOrWhiteSpace(contract)
                ? _serviceCollection?.Any(sd => !sd.IsKeyedService && sd.ServiceType == serviceType) == true
                : _serviceCollection?.Any(sd => MatchesKeyedContract(serviceType, contract, sd)) == true;
        }

        if (contract is null)
        {
            var service = _serviceProvider?.GetService(serviceType);
            return service is not null;
        }

        return _serviceProvider is IKeyedServiceProvider keyedServiceProvider
                && keyedServiceProvider.GetKeyedService(serviceType, contract) is not null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the instance.
    /// </summary>
    /// <param name="disposing">Whether or not the instance is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool MatchesKeyedContract(Type? serviceType, string contract, ServiceDescriptor sd) =>
        sd.ServiceType == serviceType
        && sd is { IsKeyedService: true, ServiceKey: string serviceKey }
        && serviceKey == contract;
}
