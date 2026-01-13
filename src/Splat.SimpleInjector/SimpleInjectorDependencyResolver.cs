// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>
/// Simple Injector implementation for <see cref="IMutableDependencyResolver"/>.
/// </summary>
/// <seealso cref="IDependencyResolver" />
public class SimpleInjectorDependencyResolver : IDependencyResolver
{
    private readonly Container _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="initializer">The initializer.</param>
    public SimpleInjectorDependencyResolver(Container container, SimpleInjectorInitializer initializer)
    {
        ArgumentExceptionHelper.ThrowIfNull(container);
        ArgumentExceptionHelper.ThrowIfNull(initializer);

        _container = container;
        RegisterFactories(initializer);
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public object? GetService(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        try
        {
            var registration = _container.GetRegistration(serviceType);
            if (registration is not null)
            {
                return registration.GetInstance();
            }

            var registers = _container.GetAllInstances(serviceType);
            return registers.LastOrDefault()!;
        }
        catch
        {
            return default;
        }
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public object? GetService(Type? serviceType, string? contract) =>

        // SimpleInjector doesn't natively support contracts, so we treat contract-based calls the same as non-contract
        GetService(serviceType);

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public IEnumerable<object> GetServices(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;
        try
        {
            return _container.GetAllInstances(serviceType);
        }
        catch
        {
            var registration = _container.GetRegistration(serviceType);
            return registration switch
            {
                not null => [registration.GetInstance()],
                _ => Array.Empty<object>()
            };
        }
    }

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We provide a different registration instead")]
    public IEnumerable<object> GetServices(Type? serviceType, string? contract) =>

        // SimpleInjector doesn't natively support contracts, so we treat contract-based calls the same as non-contract
        GetServices(serviceType);

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        return _container.GetCurrentRegistrations().Any(x => x.ServiceType == serviceType);
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract) =>

        // SimpleInjector doesn't natively support contracts, so we treat contract-based calls the same as non-contract
        HasRegistration(serviceType);

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType)
    {
        // The function does nothing because there should be no registration called on this object.
        // Anyway, AppLocator.SetLocator performs some unnecessary registrations.
    }

    /// <inheritdoc />
    public void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        // The function does nothing because there should be no registration called on this object.
        // Anyway, AppLocator.SetLocator performs some unnecessary registrations.
    }

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType) => throw new NotImplementedException("UnregisterCurrent is not supported in the SimpleInjector dependency resolver. SimpleInjector does not support removing individual registrations after they have been added.");

    /// <inheritdoc />
    public void UnregisterCurrent(Type? serviceType, string? contract) => throw new NotImplementedException("UnregisterCurrent with contract is not supported in the SimpleInjector dependency resolver. SimpleInjector does not support contracts or removing individual registrations after they have been added.");

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType) => throw new NotImplementedException("UnregisterAll is not supported in the SimpleInjector dependency resolver. SimpleInjector does not support removing registrations after they have been added.");

    /// <inheritdoc />
    public void UnregisterAll(Type? serviceType, string? contract) => throw new NotImplementedException("UnregisterAll with contract is not supported in the SimpleInjector dependency resolver. SimpleInjector does not support contracts or removing registrations after they have been added.");

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) => throw new NotImplementedException("ServiceRegistrationCallback is not supported in the SimpleInjector dependency resolver. SimpleInjector does not provide a mechanism for service registration callbacks.");

    /// <inheritdoc />
    public IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) => throw new NotImplementedException("ServiceRegistrationCallback with contract is not supported in the SimpleInjector dependency resolver. SimpleInjector does not support contracts or service registration callbacks.");

    /// <inheritdoc/>
    public T? GetService<T>() =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        (T?)GetService(typeof(T));

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>() =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        GetServices(typeof(T)).Cast<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        GetServices(typeof(T), contract).Cast<T>();

    /// <inheritdoc/>
    public bool HasRegistration<T>() =>
        HasRegistration(typeof(T));

    /// <inheritdoc/>
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory) =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        Register(() => factory(), typeof(T));

    /// <inheritdoc/>
    public void Register<T>(Func<T?> factory, string? contract) =>

        // SimpleInjector's generic methods require class constraint, so we always use the non-generic version
        Register(() => factory(), typeof(T), contract);

    /// <inheritdoc/>
    public void UnregisterCurrent<T>() =>
        UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc/>
    public void UnregisterAll<T>() =>
        UnregisterAll(typeof(T));

    /// <inheritdoc/>
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), callback);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new() =>
        _container.Register<TService, TImplementation>();

    /// <inheritdoc/>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new() =>
        Register(() => (TService)new TImplementation(), contract);

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        _container.RegisterInstance<T>(value);
    }

    /// <inheritdoc/>
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(value);

        Register(() => value, typeof(T), contract);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        _container.RegisterSingleton<T>(() => valueFactory()!);
    }

    /// <inheritdoc/>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        ArgumentExceptionHelper.ThrowIfNull(valueFactory);

        var lazy = new Lazy<T?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        Register(() => lazy.Value, typeof(T), contract);
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
        if (disposing)
        {
            _container.Dispose();
        }
    }

    private void RegisterFactories(SimpleInjectorInitializer initializer)
    {
        foreach (var typeFactories in initializer.RegisteredFactories)
        {
            _container.Collection.Register(
                typeFactories.Key,
                typeFactories.Value.Select(n =>
                    new TransientSimpleInjectorRegistration(_container, typeFactories.Key, n)));
        }
    }
}
