// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides an implementation of the dependency resolver pattern using Microsoft.Extensions.DependencyInjection.
/// Supports registration, resolution, and management of services with optional contract-based (keyed) registrations.
/// </summary>
/// <remarks>This class enables integration with the Microsoft dependency injection container, allowing for both
/// programmatic and externally provided service collections or providers. Once the container is built from an
/// IServiceProvider, further modifications are not permitted. Thread safety is ensured for all registration and
/// resolution operations. Contract-based (keyed) registrations are supported if the underlying service provider
/// implements IKeyedServiceProvider. This resolver is suitable for scenarios requiring dynamic service registration and
/// resolution, as well as integration with existing Microsoft.Extensions.DependencyInjection infrastructure.</remarks>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification = "The generic parameter is the caller-supplied service type and cannot appear in the parameter list without breaking the IDependencyResolver resolution/registration contract.")]
public class MicrosoftDependencyResolver : IDependencyResolver, IAsyncDisposable
{
    /// <summary>Message thrown when a mutation is attempted after the container has been built.</summary>
    private const string ImmutableExceptionMessage = "This container has already been built and cannot be modified.";

    /// <summary>Message thrown when resolution is attempted before a service provider is available.</summary>
    private const string ServiceProviderNullMessage = "The ServiceProvider is null.";

    /// <summary>Serializes access to the service collection and built provider.</summary>
    private readonly Lock _syncLock = new();

    /// <summary>The mutable service collection used until the provider is built.</summary>
    private IServiceCollection? _serviceCollection;

    /// <summary>Indicates whether the container has been built and can no longer be mutated.</summary>
    private bool _isImmutable;

    /// <summary>The built service provider used for resolution once the container is immutable.</summary>
    private IServiceProvider? _serviceProvider;

    /// <summary>Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with a new <see cref="IServiceCollection"/>.</summary>
    public MicrosoftDependencyResolver()
        : this((IServiceCollection?)null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with an <see cref="IServiceCollection"/>.</summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    public MicrosoftDependencyResolver(IServiceCollection? services) => _serviceCollection = services ?? new ServiceCollection();

    /// <summary>Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with a configured service Provider.</summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public MicrosoftDependencyResolver(IServiceProvider serviceProvider) =>
        UpdateContainer(serviceProvider);

    /// <summary>Gets the internal Microsoft container, or builds a new one if this instance was not initialized with one.</summary>
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

    /// <summary>Updates this instance with a collection of configured services.</summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    public void UpdateContainer(IServiceCollection services)
    {
        ArgumentExceptionHelper.ThrowIfNull(services);

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            if (_serviceProvider is not null)
            {
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
            }

            _serviceCollection = services;
        }
    }

    /// <summary>Updates this instance with a configured service Provider.</summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public void UpdateContainer(IServiceProvider serviceProvider)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceProvider);

        lock (_syncLock)
        {
            // can be null if constructor using IServiceCollection was used.
            // and no fetch of a service was called.
            if (_serviceProvider is not null)
            {
                DisposeServiceProvider(_serviceProvider);
            }

            _serviceProvider = serviceProvider;
            _serviceCollection = null;
            _isImmutable = true;
        }
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException(ServiceProviderNullMessage);
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        var service = ServiceProvider.GetService(serviceType);
        return isNull && service is NullServiceType nullServiceType
            ? nullServiceType.Factory()
            : service;
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            return GetService(serviceType);
        }

        if (ServiceProvider is null)
        {
            throw new InvalidOperationException(ServiceProviderNullMessage);
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        var service = ServiceProvider is IKeyedServiceProvider serviceProvider
            ? serviceProvider.GetKeyedService(serviceType, contract)
            : null;

        return isNull && service is NullServiceType nullServiceType
            ? nullServiceType.Factory()
            : service;
    }

    /// <inheritdoc/>
    public T? GetService<T>() => ServiceProvider is null
        ? throw new InvalidOperationException(ServiceProviderNullMessage)
        : ServiceProvider.GetService<T>();

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        contract is null ? GetService<T>() : ServiceProvider switch
        {
            null => throw new InvalidOperationException(ServiceProviderNullMessage),
            IKeyedServiceProvider keyedServiceProvider => keyedServiceProvider.GetKeyedService<T>(contract),
            _ => default
        };

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException(ServiceProviderNullMessage);
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        // this is to deal with CS8613 that GetServices returns IEnumerable<object?>?
        IEnumerable<object> services = ServiceProvider.GetServices(serviceType)
            .Where(static a => a is not null)
            .Select(static a => a!);

        if (isNull)
        {
            services = services
                .Cast<NullServiceType>()
                .Select(static nst => nst.Factory()!);
        }

        return services;
    }

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException(ServiceProviderNullMessage);
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        IEnumerable<object> services = [];

        if (ServiceProvider is IKeyedServiceProvider serviceProvider)
        {
            services = serviceProvider.GetKeyedServices(serviceType, contract)
                .Where(static a => a is not null)
                .Select(static a => a!);
        }

        if (isNull)
        {
            services = services
                .Cast<NullServiceType>()
                .Select(static nst => nst.Factory()!);
        }

        return services;
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>() => ServiceProvider is null
        ? throw new InvalidOperationException(ServiceProviderNullMessage)
        : ServiceProvider.GetServices<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>
        contract is null ? GetServices<T>() : ServiceProvider switch
        {
            null => throw new InvalidOperationException(ServiceProviderNullMessage),
            IKeyedServiceProvider keyedServiceProvider => keyedServiceProvider.GetKeyedServices<T>(contract),
            _ => []
        };

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            _serviceCollection?.AddTransient(serviceType, _ =>
            isNull
                ? new NullServiceType(factory)
                : factory()!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc />
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            Register(factory, serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedTransient(serviceType, contract, (_, _) =>
            isNull
                ? new NullServiceType(factory)
                : factory()!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory) =>
        Register(() => factory(), typeof(T));

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract) =>
        Register(() => factory(), typeof(T), contract);

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddTransient<TService>(static _ => new TImplementation());

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (contract is null)
        {
            Register<TService, TImplementation>();
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedTransient<TService>(contract, static (_, _) => new TImplementation());

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public virtual void UnregisterCurrent(Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            var sd = _serviceCollection?.LastOrDefault(s => !s.IsKeyedService && s.ServiceType == serviceType);
            if (sd is not null)
            {
                _ = _serviceCollection?.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public virtual void UnregisterCurrent(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            UnregisterCurrent(serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            var sd = _serviceCollection?.LastOrDefault(sd => MatchesKeyedContract(serviceType, contract, sd));
            if (sd is not null)
            {
                _ = _serviceCollection?.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc />
    public virtual void UnregisterAll(Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            if (_serviceCollection is null)
            {
                // required so that it gets rebuilt if not injected externally.
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
                return;
            }

            foreach (var sd in _serviceCollection.Where(s => !s.IsKeyedService && s.ServiceType == serviceType).ToList())
            {
                _ = _serviceCollection.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract - or -
    /// If the container has already been built, removes the specified contract (scope) entirely,
    /// ignoring the <paramref name="serviceType"/> argument.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">A value which will remove only objects registered with the same contract.</param>
    public virtual void UnregisterAll(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            UnregisterAll(serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            if (_serviceCollection is null)
            {
                // required so that it gets rebuilt if not injected externally.
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
                return;
            }

            foreach (var sd in _serviceCollection.Where(sd => MatchesKeyedContract(serviceType, contract, sd)).ToList())
            {
                _ = _serviceCollection.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotSupportedException("ServiceRegistrationCallback without contract is not implemented in the Microsoft dependency resolver.");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotSupportedException("ServiceRegistrationCallback is not implemented in the Microsoft dependency resolver.");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    public virtual bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        if (!_isImmutable)
        {
            return _serviceCollection?.Any(sd => !sd.IsKeyedService && sd.ServiceType == serviceType) == true;
        }

        var service = _serviceProvider?.GetService(serviceType);
        return service is not null;
    }

    /// <inheritdoc/>
    public virtual bool HasRegistration(Type? serviceType, string? contract)
    {
        // Contract semantics: a null contract means "use the non-keyed registration path".
        // This matches Register(..., Type?, string? contract) which delegates to Register(..., Type?)
        // when contract is null, and matches expected IDependencyResolver behavior in Splat tests.
        if (contract is null)
        {
            return HasRegistration(serviceType);
        }

        serviceType ??= NullServiceType.CachedType;

        return !_isImmutable ? _serviceCollection?.Any(sd => MatchesKeyedContract(serviceType, contract, sd)) == true : _serviceProvider is IKeyedServiceProvider keyedServiceProvider
               && keyedServiceProvider.GetKeyedService(serviceType, contract) is not null;
    }

    /// <inheritdoc/>
    public bool HasRegistration<T>() => HasRegistration(typeof(T));

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddSingleton<T>(value!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterConstant(value);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedSingleton<T>(contract, value!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_syncLock)
        {
            _serviceCollection?.AddSingleton<T>(_ => lazy.Value!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton(valueFactory);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedSingleton<T>(contract, (_, _) => lazy.Value!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_serviceProvider is IAsyncDisposable d)
        {
            await d.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Disposes of the instance.</summary>
    /// <param name="disposing">Whether or not the instance is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        DisposeServiceProvider(_serviceProvider);
    }

    /// <summary>Disposes the supplied service provider if it implements <see cref="IDisposable"/>.</summary>
    /// <param name="sp">The service provider to dispose, or <see langword="null"/> to do nothing.</param>
    private static void DisposeServiceProvider(IServiceProvider? sp)
    {
        if (sp is not IDisposable d)
        {
            return;
        }

        d.Dispose();
    }

    /// <summary>Determines whether the supplied service descriptor is a keyed registration matching the service type and contract.</summary>
    /// <param name="serviceType">The service type to match.</param>
    /// <param name="contract">The contract (service key) to match.</param>
    /// <param name="sd">The service descriptor to test.</param>
    /// <returns><see langword="true"/> if the descriptor is keyed and matches both the service type and contract; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool MatchesKeyedContract(Type? serviceType, string? contract, ServiceDescriptor sd) =>
        sd.ServiceType == serviceType
        && sd is { IsKeyedService: true, ServiceKey: string serviceKey }
        && serviceKey == contract;
}
