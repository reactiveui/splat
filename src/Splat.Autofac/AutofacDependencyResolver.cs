// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace Splat.Autofac;

/// <summary>
/// Provides an Autofac-based implementation of the IDependencyResolver interface for resolving services and managing
/// registrations within ReactiveUI applications.
/// </summary>
/// <remarks>This resolver integrates Autofac's container and lifetime scope management with ReactiveUI's
/// dependency resolution system. It is designed to support the initialization and service resolution needs of
/// ReactiveUI, including contract-based and generic service lookups. Due to Autofac 5+ containers being immutable,
/// registration and unregistration methods are intended for internal use during initialization and should not be used
/// by end-users after the container is built. To override default registrations, register your services after calling
/// InitializeReactiveUI. Thread safety is ensured for all public operations. Disposing the resolver will release
/// Autofac container resources.</remarks>
[SuppressMessage(
    "StyleSharp",
    "SST2307:A generic method's type parameter appears in no parameter, so no caller can infer it",
    Justification =
        "The generic parameter is the caller-supplied service type for these resolution/registration APIs and cannot be a method parameter without breaking the IDependencyResolver API.")]
[SuppressMessage(
    "StyleSharp",
    "SST1452:A generic type parameter is used only as a marker",
    Justification = "Generic marker API; the type parameter identifies the target and is applied via typeof(T) in the implementation.")]
public class AutofacDependencyResolver : IDependencyResolver
{
    /// <summary>The exception message thrown when an attempt is made to mutate the resolver after the lifetime scope has been set.</summary>
    private const string ContainerAlreadyBuiltMessage =
        "Container has already been built and the lifetime scope set, so it is not possible to modify it anymore.";

    /// <summary>The guidance appended to the obsolete unregister messages explaining how to override default registrations.</summary>
    private const string OverrideRegistrationGuidance =
        "Instead, simply register your service after InitializeReactiveUI to override it https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations.";

    /// <summary>The obsolete message used by the register members, which are kept for backward compatibility.</summary>
    private const string RegisterObsoleteMessage =
        "Because Autofac 5+ containers are immutable, this method should not be used by the end-user.";

    /// <summary>The obsolete message used by the unregister-current members, which are kept for backward compatibility.</summary>
    private const string UnregisterCurrentObsoleteMessage =
        $"Because Autofac 5+ containers are immutable, UnregisterCurrent method is not available anymore. {OverrideRegistrationGuidance}";

    /// <summary>The obsolete message used by the unregister-all members, which are kept for backward compatibility.</summary>
    private const string UnregisterAllObsoleteMessage =
        $"Because Autofac 5+ containers are immutable, UnregisterAll method is not available anymore. {OverrideRegistrationGuidance}";

    /// <summary>Serializes registration and lifetime-scope changes.</summary>
    private readonly Lock _lockObject = new();

    /// <summary>The builder used to accumulate registrations before the container is built.</summary>
    private readonly ContainerBuilder _builder;

    /// <summary>
    ///     The internal container, which takes care of mutability needed for ReactiveUI initialization procedure.
    ///     It is disposed of once the user sets the actual lifetime scope from which to resolve by calling SetLifetimeScope.
    /// </summary>
    private IContainer _internalContainer;

    /// <summary>The user-supplied lifetime scope from which services are resolved once set.</summary>
    private ILifetimeScope? _lifetimeScope;

    /// <summary>The lifetime scope of <see cref="_internalContainer"/> used until the user supplies their own scope.</summary>
    private ILifetimeScope _internalLifetimeScope;

    /// <summary>
    ///     Set to true, when SetLifetimeScope has been called.
    ///     Prevents mutating the ContainerBuilder or setting the lifetime again.
    /// </summary>
    private bool _lifetimeScopeSet;

    /// <summary>Initializes a new instance of the <see cref="AutofacDependencyResolver" /> class.</summary>
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

    /// <inheritdoc/>
    public T? GetService<T>() => (T?)GetService(typeof(T));

    /// <inheritdoc/>
    public T? GetService<T>(string? contract) =>
        (T?)GetService(typeof(T), contract);

    /// <inheritdoc />
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = Resolve(enumerableType);

            switch (isNull)
            {
                case true when instance is IEnumerable<NullServiceType> nullService:
                    return nullService.Select(static item => item.Factory()!);
                case false when instance is not null:
                    return ((IEnumerable)instance).Cast<object>();
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
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = Resolve(enumerableType, contract);

            switch (isNull)
            {
                case true when instance is IEnumerable<NullServiceType> nullService:
                    return nullService.Select(static item => item.Factory()!);
                case false when instance is not null:
                    return ((IEnumerable)instance).Cast<object>();
            }

            return [];
        }
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>() => GetServices(typeof(T)).Cast<T>();

    /// <inheritdoc/>
    public IEnumerable<T> GetServices<T>(string? contract) =>
        GetServices(typeof(T), contract).Cast<T>();

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
            return contract is null || string.IsNullOrWhiteSpace(contract)
                ? lifeTimeScope.IsRegistered(serviceType)
                : lifeTimeScope.IsRegisteredWithName(contract, serviceType);
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
    [Obsolete(RegisterObsoleteMessage)]
    public virtual void Register(Func<object?> factory, Type? serviceType)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            // We register every ReactiveUI service twice.
            // First to the application-wide container, which we are still building.
            // Second to child lifetimes in a temporary container, that is used only to satisfy ReactiveUI dependencies.
            RegisterFactory(_builder, factory, serviceType, isNull);
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new FactoryScopeConfigurator(factory, serviceType, isNull, null).Configure);
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
    [Obsolete(RegisterObsoleteMessage)]
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            // We register every ReactiveUI service twice.
            // First to the application-wide container, which we are still building.
            // Second to child lifetimes in a temporary container, that is used only to satisfy ReactiveUI dependencies.
            RegisterFactory(_builder, factory, serviceType, isNull, contract);
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new FactoryScopeConfigurator(factory, serviceType, isNull, contract).Configure);
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void Register<T>(Func<T?> factory) =>
        Register(() => factory(), typeof(T));

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void Register<T>(Func<T?> factory, string? contract) =>
        Register(() => factory(), typeof(T), contract);

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            // Register to both the application-wide container and internal lifetime
            _ = _builder.Register(static _ => new TImplementation())
                .As<TService>()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(static internalBuilder =>
                internalBuilder.Register(static _ => new TImplementation())
                    .As<TService>()
                    .AsImplementedInterfaces());
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                Register<TService, TImplementation>();
            }
            else
            {
                var contractName = contract!;

                // Register to both the application-wide container and internal lifetime
                _ = _builder.Register(static _ => new TImplementation())
                    .Named<TService>(contractName)
                    .AsImplementedInterfaces();
                _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                    new TypeScopeConfigurator<TService, TImplementation>(contractName).Configure);
            }
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void RegisterConstant<T>(T? value)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            // Register as singleton instance to both the application-wide container and internal lifetime
            _ = _builder.RegisterInstance(value!)
                .As<T>()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new InstanceScopeConfigurator<T>(value!, null).Configure);
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void RegisterConstant<T>(T? value, string? contract)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                RegisterConstant(value);
                return;
            }

            var contractName = contract!;

            // Register as singleton instance to both the application-wide container and internal lifetime
            _ = _builder.RegisterInstance(value!)
                .Named<T>(contractName)
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new InstanceScopeConfigurator<T>(value!, contractName).Configure);
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        T>(
        Func<T?> valueFactory)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            // Register as singleton with factory to both the application-wide container and internal lifetime
            _ = _builder.Register(_ => valueFactory()!)
                .As<T>()
                .SingleInstance()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new LazySingletonScopeConfigurator<T>(valueFactory, null).Configure);
        }
    }

    /// <inheritdoc/>
    [Obsolete(RegisterObsoleteMessage)]
    public void RegisterLazySingleton<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        T>(
        Func<T?> valueFactory,
        string? contract)
        where T : class
    {
        lock (_lockObject)
        {
            if (_lifetimeScopeSet)
            {
                throw new InvalidOperationException(ContainerAlreadyBuiltMessage);
            }

            if (string.IsNullOrWhiteSpace(contract))
            {
                RegisterLazySingleton(valueFactory);
                return;
            }

            var contractName = contract!;

            // Register as singleton with factory to both the application-wide container and internal lifetime
            _ = _builder.Register(_ => valueFactory()!)
                .Named<T>(contractName)
                .SingleInstance()
                .AsImplementedInterfaces();
            _internalLifetimeScope = _internalLifetimeScope.BeginLifetimeScope(
                new LazySingletonScopeConfigurator<T>(valueFactory, contractName).Configure);
        }
    }

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterCurrent method is not available anymore.
    ///     Instead, simply
    ///     <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see>
    ///     to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <exception cref="NotSupportedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete(UnregisterCurrentObsoleteMessage)]
    public virtual void UnregisterCurrent(Type? serviceType) =>
        throw new NotSupportedException(UnregisterCurrentObsoleteMessage);

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterCurrent method is not available anymore.
    ///     Instead, simply
    ///     <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see>
    ///     to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    /// <exception cref="NotSupportedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete(UnregisterCurrentObsoleteMessage)]
    public virtual void UnregisterCurrent(Type? serviceType, string? contract) =>
        throw new NotSupportedException(UnregisterCurrentObsoleteMessage);

    /// <inheritdoc/>
    [Obsolete(UnregisterCurrentObsoleteMessage)]
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <inheritdoc/>
    [Obsolete(UnregisterCurrentObsoleteMessage)]
    public void UnregisterCurrent<T>(string? contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterAll method is not available anymore.
    ///     Instead, simply
    ///     <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see>
    ///     to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <exception cref="NotSupportedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete(UnregisterAllObsoleteMessage)]
    public virtual void UnregisterAll(Type? serviceType) =>
        throw new NotSupportedException(UnregisterAllObsoleteMessage);

    /// <summary>
    ///     Because <see href="https://autofaccn.readthedocs.io/en/latest/best-practices/#consider-a-container-as-immutable">Autofac 5+ containers are immutable</see>,
    ///     UnregisterAll method is not available anymore.
    ///     Instead, simply
    ///     <see href="https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations">register your service after InitializeReactiveUI</see>
    ///     to override it.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    /// <exception cref="NotSupportedException">This is not implemented by default.</exception>
    /// <inheritdoc />
    [Obsolete(UnregisterAllObsoleteMessage)]
    public virtual void UnregisterAll(Type? serviceType, string? contract) =>
        throw new NotSupportedException(UnregisterAllObsoleteMessage);

    /// <inheritdoc/>
    [Obsolete(UnregisterAllObsoleteMessage)]
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <inheritdoc/>
    [Obsolete(UnregisterAllObsoleteMessage)]
    public void UnregisterAll<T>(string? contract) =>
        UnregisterAll(typeof(T), contract);

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotSupportedException(
            "ServiceRegistrationCallback without contract is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc />
    public virtual IDisposable ServiceRegistrationCallback(
        Type serviceType,
        string? contract,
        Action<IDisposable> callback) =>
        throw new NotSupportedException(
            "ServiceRegistrationCallback is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
        throw new NotSupportedException(
            "ServiceRegistrationCallback without contract is not implemented in the Autofac dependency resolver.");

    /// <inheritdoc/>
    public IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback) =>
        ServiceRegistrationCallback(typeof(T), contract, callback);

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
        lock (_lockObject)
        {
            if (!disposing)
            {
                return;
            }

            _lifetimeScope?.ComponentRegistry.Dispose();
            _internalContainer?.Dispose();
        }
    }

    /// <summary>Registers the supplied factory against the given builder, optionally using a contract name.</summary>
    /// <param name="builder">The container builder to register against.</param>
    /// <param name="factory">The factory function which generates the service instance.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="isNull">Whether the original service type was <see langword="null"/>.</param>
    /// <param name="contract">The optional contract name used to disambiguate the registration.</param>
    private static void RegisterFactory(
        ContainerBuilder builder,
        Func<object?> factory,
        Type serviceType,
        bool isNull,
        string? contract = null)
    {
        var registration = builder.Register(_ =>
            isNull
                ? new NullServiceType(factory)
                : factory()!);

        if (contract is null || string.IsNullOrWhiteSpace(contract))
        {
            _ = registration.As(serviceType).AsImplementedInterfaces();
        }
        else
        {
            _ = registration.Named(contract, serviceType).AsImplementedInterfaces();
        }
    }

    /// <summary>Resolves a single service instance for the specified type from the active lifetime scope.</summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <returns>The resolved service instance, or <see langword="null"/> if no registration exists.</returns>
    private object? Resolve(Type serviceType)
    {
        var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;

        _ = lifeTimeScope.TryResolve(serviceType, out var serviceInstance);

        return serviceInstance;
    }

    /// <summary>Resolves a single service instance for the specified type and contract from the active lifetime scope.</summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <param name="contract">The optional contract name used to disambiguate the registration.</param>
    /// <returns>The resolved service instance, or <see langword="null"/> if no registration exists.</returns>
    private object? Resolve(Type serviceType, string? contract)
    {
        var lifeTimeScope = _lifetimeScope ?? _internalLifetimeScope;

        _ = contract is null || string.IsNullOrWhiteSpace(contract)
            ? lifeTimeScope.TryResolve(serviceType, out var serviceInstance)
            : lifeTimeScope.TryResolveNamed(contract, serviceType, out serviceInstance!);

        return serviceInstance;
    }

    /// <summary>
    ///     Holds the state needed to replay a factory registration against a freshly created child lifetime scope.
    ///     Storing the state on a dedicated instance lets the scope configuration be supplied as a method group,
    ///     so it does not close over any local of the calling method.
    /// </summary>
    /// <param name="factory">The factory function which generates the service instance.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="isNull">Whether the original service type was <see langword="null"/>.</param>
    /// <param name="contract">The optional contract name used to disambiguate the registration.</param>
    private sealed class FactoryScopeConfigurator(Func<object?> factory, Type serviceType, bool isNull, string? contract)
    {
        /// <summary>Applies the stored registration to the supplied child scope builder.</summary>
        /// <param name="builder">The child scope builder to register against.</param>
        public void Configure(ContainerBuilder builder) =>
            RegisterFactory(builder, factory, serviceType, isNull, contract);
    }

    /// <summary>
    ///     Holds the state needed to replay a singleton instance registration against a freshly created child
    ///     lifetime scope without closing over any local of the calling method.
    /// </summary>
    /// <typeparam name="T">The service type to register the instance as.</typeparam>
    /// <param name="value">The singleton instance to register.</param>
    /// <param name="contractName">The contract name, or <see langword="null"/> to register without a contract.</param>
    private sealed class InstanceScopeConfigurator<T>(T value, string? contractName)
        where T : class
    {
        /// <summary>Applies the stored registration to the supplied child scope builder.</summary>
        /// <param name="builder">The child scope builder to register against.</param>
        public void Configure(ContainerBuilder builder)
        {
            var registration = builder.RegisterInstance(value);
            if (contractName is null)
            {
                _ = registration.As<T>().AsImplementedInterfaces();
            }
            else
            {
                _ = registration.Named<T>(contractName).AsImplementedInterfaces();
            }
        }
    }

    /// <summary>
    ///     Holds the state needed to replay a lazy singleton factory registration against a freshly created child
    ///     lifetime scope without closing over any local of the calling method.
    /// </summary>
    /// <typeparam name="T">The service type to register the factory as.</typeparam>
    /// <param name="valueFactory">The factory that produces the singleton instance on first resolution.</param>
    /// <param name="contractName">The contract name, or <see langword="null"/> to register without a contract.</param>
    private sealed class LazySingletonScopeConfigurator<T>(Func<T?> valueFactory, string? contractName)
        where T : class
    {
        /// <summary>Applies the stored registration to the supplied child scope builder.</summary>
        /// <param name="builder">The child scope builder to register against.</param>
        public void Configure(ContainerBuilder builder)
        {
            var registration = builder.Register(_ => valueFactory()!);
            if (contractName is null)
            {
                _ = registration.As<T>().SingleInstance().AsImplementedInterfaces();
            }
            else
            {
                _ = registration.Named<T>(contractName).SingleInstance().AsImplementedInterfaces();
            }
        }
    }

    /// <summary>
    ///     Holds the state needed to replay a named type registration against a freshly created child lifetime
    ///     scope without closing over any local of the calling method.
    /// </summary>
    /// <typeparam name="TService">The service type the implementation is registered as.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to instantiate.</typeparam>
    /// <param name="contractName">The contract name used to disambiguate the registration.</param>
    private sealed class TypeScopeConfigurator<TService, TImplementation>(string contractName)
        where TService : class
        where TImplementation : class, TService, new()
    {
        /// <summary>Applies the stored registration to the supplied child scope builder.</summary>
        /// <param name="builder">The child scope builder to register against.</param>
        public void Configure(ContainerBuilder builder) =>
            _ = builder.Register(static _ => new TImplementation())
                .Named<TService>(contractName)
                .AsImplementedInterfaces();
    }
}
