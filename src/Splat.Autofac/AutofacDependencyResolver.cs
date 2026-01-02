// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

using Autofac;

namespace Splat.Autofac;

/// <summary>
/// Autofac implementation for <see cref="IDependencyResolver"/>.
/// </summary>
public class AutofacDependencyResolver : IDependencyResolver
{
    private readonly object _lockObject = new();
    private readonly ContainerBuilder _builder;

    /// <summary>
    ///     The internal container, which takes care of mutability needed for ReactiveUI initialization procedure.
    ///     It is disposed of once the user sets the actual lifetime scope from which to resolve by calling SetLifetimeScope.
    /// </summary>
    private IContainer _internalContainer;

    private ILifetimeScope? _lifetimeScope;

    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "_internalLifetimeScope will be disposed, because it is a child of _internalContainer")]
    private ILifetimeScope _internalLifetimeScope;

    /// <summary>
    ///     Set to true, when SetLifetimeScope has been called.
    ///     Prevents mutating the ContainerBuilder or setting the lifetime again.
    /// </summary>
    private bool _lifetimeScopeSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutofacDependencyResolver" /> class.
    /// </summary>
    /// <param name="builder">Autofac container builder.</param>
    public AutofacDependencyResolver(ContainerBuilder builder)
    {
        _builder = builder;

        _internalContainer = new ContainerBuilder().Build();
        _internalLifetimeScope = _internalContainer.BeginLifetimeScope();
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var result = Resolve(serviceType);
            return isNull ? (result as NullServiceType)?.Factory() : result;
        }
    }

    /// <inheritdoc />
    public virtual object? GetService(Type? serviceType, string? contract)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var result = Resolve(serviceType, contract);
            return isNull ? (result as NullServiceType)?.Factory() : result;
        }
    }

    /// <summary>
    ///     Sets the lifetime scope which will be used to resolve ReactiveUI services.
    ///     It should be set after Autofac application-wide container is built.
    /// </summary>
    /// <param name="lifetimeScope">Lifetime scope, which will be used to resolve ReactiveUI services.</param>
    public void SetLifetimeScope(ILifetimeScope lifetimeScope)
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Lifetime scope of the Autofac resolver has already been set");
            }

            _lifetimeScopeSet = true;
            _lifetimeScope = lifetimeScope;

            // We dispose on the internal container, since it and its many child lifetime scopes are not needed anymore.
            _internalContainer.Dispose();
            _internalContainer = new ContainerBuilder().Build();
            _internalLifetimeScope = _internalContainer.BeginLifetimeScope();
        }
    }

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            try
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                var instance = Resolve(enumerableType);

                switch (isNull)
                {
                    case true when instance is IEnumerable<NullServiceType> nullService:
                        return nullService.Select(item => item.Factory()!);
                    case false when instance is not null:
                        return ((IEnumerable)instance).Cast<object>();
                }
            }
            finally
            {
                // no op
            }

            return [];
        }
    }

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            try
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                var instance = Resolve(enumerableType, contract);

                switch (isNull)
                {
                    case true when instance is IEnumerable<NullServiceType> nullService:
                        return nullService.Select(item => item.Factory()!);
                    case false when instance is not null:
                        return ((IEnumerable)instance).Cast<object>();
                }
            }
            finally
            {
                // no op
            }

            return [];
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;
            return lifeTimeScope.IsRegistered(serviceType);
        }
    }

    /// <inheritdoc />
    public bool HasRegistration(Type? serviceType, string? contract)
    {
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;
            return contract is null || string.IsNullOrWhiteSpace(contract) ?
                lifeTimeScope.IsRegistered(serviceType) :
                lifeTimeScope.IsRegisteredWithName(contract, serviceType);
        }
    }

    /// <inheritdoc />
    public bool HasRegistration<T>() => HasRegistration(typeof(T));

    /// <inheritdoc />
    public bool HasRegistration<T>(string? contract) =>
        HasRegistration(typeof(T), contract);

    /// <summary>
    ///     Important: Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     this method should not be used by the end-user.
    ///     It is still needed to satisfy ReactiveUI initialization procedure.
    ///     Register a function with the resolver which will generate a object
    ///     for the specified service type.
    ///     Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public virtual void Register(Func<object?> factory, Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            // We register every ReactiveUI service twice.
            // First to the application-wide container, which we are still building.
            // Second to child lifetimes in a temporary container, that is used only to satisfy ReactiveUI dependencies.
            _builder.Register(_ =>
                isNull
                    ? new NullServiceType(factory)
                    : factory()!)
                .As(serviceType)
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.Register(_ =>
                    isNull
                        ? new NullServiceType(factory)
                        : factory()!)
                    .As(serviceType)
                    .AsImplementedInterfaces());
        }
    }

    /// <summary>
    ///     Important: Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     this method should not be used by the end-user.
    ///     It is still needed to satisfy ReactiveUI initialization procedure.
    ///     Register a function with the resolver which will generate a object
    ///     for the specified service type.
    ///     A contract is registered which will indicate
    ///     that registration will only work with that contract.
    ///     Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="contract">A contract value which indicates to only generate the value if this contract is specified.</param>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            // We register every ReactiveUI service twice.
            // First to the application-wide container, which we are still building.
            // Second to child lifetimes in a temporary container, that is used only to satisfy ReactiveUI dependencies.
            if (contract is null || string.IsNullOrWhiteSpace(contract))
            {
                _builder.Register(_ =>
                    isNull
                        ? new NullServiceType(factory)
                        : factory()!)
                    .As(serviceType)
                    .AsImplementedInterfaces();
                _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                    internalBuilder.Register(_ =>
                        isNull
                            ? new NullServiceType(factory)
                            : factory()!)
                        .As(serviceType)
                        .AsImplementedInterfaces());
            }
            else
            {
                _builder.Register(_ =>
                    isNull
                        ? new NullServiceType(factory)
                        : factory()!)
                    .Named(contract, serviceType)
                    .AsImplementedInterfaces();
                _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                    internalBuilder.Register(_ =>
                        isNull
                            ? new NullServiceType(factory)
                            : factory()!)
                        .Named(contract, serviceType)
                        .AsImplementedInterfaces());
            }
        }
    }

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterCurrent method is not available anymore.
    ///     Instead, simply <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see> to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <exception cref="NotImplementedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public virtual void UnregisterCurrent(Type? serviceType) =>
        throw new NotImplementedException("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
                                          "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.");

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterCurrent method is not available anymore.
    ///     Instead, simply <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see> to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    /// <exception cref="NotImplementedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public virtual void UnregisterCurrent(Type? serviceType, string? contract) =>
        throw new NotImplementedException("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
                                          "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.");

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterAll method is not available anymore.
    ///     Instead, simply <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see> to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <exception cref="NotImplementedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public virtual void UnregisterAll(Type? serviceType) =>
        throw new NotImplementedException("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
                                          "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.");

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterAll method is not available anymore.
    ///     Instead, simply <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see> to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    /// <exception cref="NotImplementedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public virtual void UnregisterAll(Type? serviceType, string? contract) =>
        throw new NotImplementedException("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
                                          "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback without contract is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc/>
    public T? GetService<T>() => (T?)GetService(typeof(T));

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>() => GetServices(typeof(T)).Cast<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>
        GetServices(typeof(T), contract).Cast<T>();

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void Register<T>(Func<T?> factory) =>
        Register(() => factory(), typeof(T));

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void Register<T>(Func<T?> factory, string? contract) =>
        Register(() => factory(), typeof(T), contract);

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. " +
              "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.")]
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback without contract is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            // Register to both the application-wide container and internal lifetime
            _builder.RegisterType<TImplementation>()
                .As<TService>()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.RegisterType<TImplementation>()
                    .As<TService>()
                    .AsImplementedInterfaces());
        }
    }

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
               Register<TService, TImplementation>();
            }
            else
            {
                // Register to both the application-wide container and internal lifetime
                _builder.RegisterType<TImplementation>()
                    .Named<TService>(contract!)
                    .AsImplementedInterfaces();
                _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                    internalBuilder.RegisterType<TImplementation>()
                        .Named<TService>(contract!)
                        .AsImplementedInterfaces());
            }
        }
    }

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            // Register as singleton instance to both the application-wide container and internal lifetime
            _builder.RegisterInstance(value!)
                .As<T>()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.RegisterInstance(value!)
                    .As<T>()
                    .AsImplementedInterfaces());
        }
    }

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                RegisterConstant<T>(value);
                return;
            }

            // Register as singleton instance to both the application-wide container and internal lifetime
            _builder.RegisterInstance(value!)
                .Named<T>(contract!)
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.RegisterInstance(value!)
                    .Named<T>(contract!)
                    .AsImplementedInterfaces());
        }
    }

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            // Register as singleton with factory to both the application-wide container and internal lifetime
            _builder.Register(_ => valueFactory()!)
                .As<T>()
                .SingleInstance()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.Register(_ => valueFactory()!)
                    .As<T>()
                    .SingleInstance()
                    .AsImplementedInterfaces());
        }
    }

    /// <inheritdoc/>
    [Obsolete("Because Autofac 5+ containers are immutable, this method should not be used by the end-user.")]
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException("Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.");
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                RegisterLazySingleton<T>(valueFactory);
                return;
            }

            // Register as singleton with factory to both the application-wide container and internal lifetime
            _builder.Register(_ => valueFactory()!)
                .Named<T>(contract!)
                .SingleInstance()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(internalBuilder =>
                internalBuilder.Register(_ => valueFactory()!)
                    .Named<T>(contract!)
                    .SingleInstance()
                    .AsImplementedInterfaces());
        }
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
        lock (_lockObject)
        {
            if (disposing)
            {
                _lifetimeScope?.ComponentRegistry.Dispose();
                _internalContainer?.Dispose();
            }
        }
    }

    private object? Resolve(Type serviceType)
    {
        object serviceInstance;

        var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;

        lifeTimeScope.TryResolve(serviceType, out serviceInstance!);

        return serviceInstance;
    }

    private object? Resolve(Type serviceType, string? contract)
    {
        object serviceInstance;

        var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;

        _ = contract is null || string.IsNullOrWhiteSpace(contract)
            ? lifeTimeScope.TryResolve(serviceType, out serviceInstance!)
            : lifeTimeScope.TryResolveNamed(contract, serviceType, out serviceInstance!);

        return serviceInstance;
    }
}
