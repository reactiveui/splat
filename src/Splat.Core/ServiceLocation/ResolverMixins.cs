// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides extension methods for registering services, factories, and singleton instances with an
/// IMutableDependencyResolver in a fluent manner.
/// </summary>
/// <remarks>These mixin methods simplify the registration of services, factories, and singleton or constant
/// values with dependency resolvers. They support both generic and non-generic registrations, as well as contract-based
/// registrations for advanced scenarios. All methods return the resolver instance to enable fluent chaining of multiple
/// registrations.</remarks>
public static class ResolverMixins
{
    /// <summary>
    /// Registers the specified service type with the dependency resolver using its parameterless constructor, and
    /// returns the resolver for chaining.
    /// </summary>
    /// <remarks>This method is useful for registering simple services that can be constructed using a
    /// parameterless constructor. It enables fluent registration of multiple services in a single statement.</remarks>
    /// <typeparam name="TService">The type of service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver to which the service type is registered. Cannot be null.</param>
    /// <returns>The same dependency resolver instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new());
        return resolver;
    }

    /// <summary>
    /// Registers a service of type TService with the specified contract using its default constructor, and returns the
    /// resolver for chaining.
    /// </summary>
    /// <remarks>This method is useful for fluent registration of services that can be constructed using a
    /// parameterless constructor. If a service with the same type and contract is already registered, this registration
    /// may override the previous one depending on the resolver's implementation.</remarks>
    /// <typeparam name="TService">The type of service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service. Cannot be null.</param>
    /// <param name="contract">An optional contract string that distinguishes this registration from others of the same service type. Can be
    /// null or empty for the default contract.</param>
    /// <returns>The same IMutableDependencyResolver instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new(), contract);
        return resolver;
    }

    /// <summary>
    /// Registers a factory method for the specified service type and returns the dependency resolver to enable fluent
    /// chaining.
    /// </summary>
    /// <remarks>This method is intended for use in fluent configuration scenarios, allowing multiple service
    /// registrations to be chained together.</remarks>
    /// <typeparam name="TService">The type of service to register with the dependency resolver.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service factory. Cannot be null.</param>
    /// <param name="factory">A delegate that creates instances of the service type. Cannot be null.</param>
    /// <returns>The same dependency resolver instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!);
        return resolver;
    }

    /// <summary>
    /// Registers a factory method for the specified service type and contract, and returns the dependency resolver to
    /// enable fluent configuration.
    /// </summary>
    /// <remarks>This method enables fluent registration of services by returning the original resolver
    /// instance. If a contract is specified, the service is registered under that contract; otherwise, it is registered
    /// as the default implementation.</remarks>
    /// <typeparam name="TService">The type of service to register with the dependency resolver.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service factory. Cannot be null.</param>
    /// <param name="factory">A function that creates instances of the service type. Cannot be null.</param>
    /// <param name="contract">An optional contract string that distinguishes this registration from others of the same service type. May be
    /// null or empty to indicate the default contract.</param>
    /// <returns>The same dependency resolver instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> factory, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a service implementation with the dependency resolver and returns the resolver for chaining.
    /// </summary>
    /// <remarks>This method registers <typeparamref name="TImplementation"/> as the implementation for
    /// <typeparamref name="TService"/> using a parameterless constructor. Subsequent calls to resolve <typeparamref
    /// name="TService"/> will create new instances of <typeparamref name="TImplementation"/>.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete type that implements <typeparamref name="TService"/>. Must have a parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service. Cannot be null.</param>
    /// <returns>The same <see cref="IMutableDependencyResolver"/> instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver)
        where TImplementation : TService, new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new TImplementation());
        return resolver;
    }

    /// <summary>
    /// Registers a service implementation with the specified contract and returns the dependency resolver to enable
    /// fluent configuration.
    /// </summary>
    /// <remarks>This method registers <typeparamref name="TImplementation"/> as an implementation of
    /// <typeparamref name="TService"/> using a parameterless constructor. It is intended for use in fluent registration
    /// scenarios.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete type that implements <typeparamref name="TService"/>. Must have a parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service implementation. Cannot be null.</param>
    /// <param name="contract">The contract name used to distinguish this registration. May be null or empty to indicate the default contract.</param>
    /// <returns>The same dependency resolver instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, string contract)
        where TImplementation : TService, new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new TImplementation(), contract);
        return resolver;
    }

    /// <summary>
    /// Registers a factory method for creating instances of the specified implementation type as the given service
    /// type, and returns the dependency resolver to enable fluent configuration.
    /// </summary>
    /// <remarks>This method is intended for fluent registration of services and implementations. The factory
    /// method is invoked each time an instance of the service is requested.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete type that implements or derives from <typeparamref name="TService"/>.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service.</param>
    /// <param name="factory">A factory method that creates instances of <typeparamref name="TImplementation"/>. Cannot be null.</param>
    /// <returns>The same <see cref="IMutableDependencyResolver"/> instance, to allow for method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, Func<TImplementation> factory)
        where TImplementation : TService
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!);
        return resolver;
    }

    /// <summary>
    /// Registers a factory method for a service implementation with the specified contract and returns the dependency
    /// resolver for chaining.
    /// </summary>
    /// <remarks>This method is intended for fluent registration of services. The implementation type must be
    /// assignable to the service type.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete type that implements the service.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service.</param>
    /// <param name="factory">A factory method that creates instances of the implementation type. Cannot be null.</param>
    /// <param name="contract">An optional contract string used to distinguish between multiple registrations of the same service type. May be
    /// null or empty.</param>
    /// <returns>The same dependency resolver instance, to support method chaining.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, Func<TImplementation> factory, string contract)
        where TImplementation : TService
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a constant instance for the specified service type in the dependency resolver and returns the resolver
    /// for chaining.
    /// </summary>
    /// <remarks>This method is useful for registering singleton or pre-constructed instances with the
    /// dependency resolver. Subsequent resolutions of the specified service type will return the provided constant
    /// instance.</remarks>
    /// <param name="resolver">The dependency resolver in which to register the constant instance. Cannot be null.</param>
    /// <param name="value">The constant instance to associate with the specified service type. May be null if the service type allows null
    /// values.</param>
    /// <param name="serviceType">The type of service to associate with the constant instance. This type will be used to resolve the registered
    /// value.</param>
    /// <returns>The same dependency resolver instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd(this IMutableDependencyResolver resolver, object value, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register(() => value, serviceType);
        return resolver;
    }

    /// <summary>
    /// Registers a constant value for the specified service type and contract in the dependency resolver, and returns
    /// the resolver to enable method chaining.
    /// </summary>
    /// <remarks>This method is intended for use with dependency injection scenarios where a specific instance
    /// should always be returned for a given service type and contract. The registered value will be returned for all
    /// subsequent resolutions of the specified service type and contract.</remarks>
    /// <param name="resolver">The dependency resolver in which to register the constant value. Cannot be null.</param>
    /// <param name="value">The constant value to register for the specified service type and contract. May be null if the service type
    /// allows null values.</param>
    /// <param name="serviceType">The type of service to associate with the registered value. Cannot be null.</param>
    /// <param name="contract">An optional contract name to distinguish between multiple registrations of the same service type. May be null or
    /// empty.</param>
    /// <returns>The same <see cref="IMutableDependencyResolver"/> instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd(this IMutableDependencyResolver resolver, object value, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register(() => value, serviceType, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a single, shared instance of the specified service type in the dependency resolver and returns the
    /// resolver for chaining.
    /// </summary>
    /// <remarks>This method creates a single instance of the service type and registers it as a constant,
    /// ensuring that all resolutions of the service return the same object. This is useful for registering stateless or
    /// singleton services.</remarks>
    /// <typeparam name="TService">The type of service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service instance. Cannot be null.</param>
    /// <returns>The same dependency resolver instance, to support method chaining.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var value = new TService();
        return resolver.RegisterAnd(() => value);
    }

    /// <summary>
    /// Registers a singleton instance of the specified service type and returns the resolver to enable method chaining.
    /// </summary>
    /// <remarks>This method creates a single instance of the specified service type using its parameterless
    /// constructor and registers it as a constant for the given contract. Subsequent resolutions of the service will
    /// return the same instance.</remarks>
    /// <typeparam name="TService">The type of service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service instance. Cannot be null.</param>
    /// <param name="contract">An optional contract string used to distinguish between multiple registrations of the same service type. May be
    /// null or empty.</param>
    /// <returns>The same dependency resolver instance, to allow for fluent registration of additional services.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var value = new TService();
        return resolver.RegisterAnd(() => value, contract);
    }

    /// <summary>
    /// Registers a constant instance of the specified service type and returns the resolver for chaining.
    /// </summary>
    /// <remarks>This method is typically used to register singleton or pre-constructed service instances. The
    /// registered value will be returned for all requests for the specified service type.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the service. Cannot be null.</param>
    /// <param name="value">The constant instance of the service to register. This instance will always be returned for the specified
    /// service type.</param>
    /// <returns>The same dependency resolver instance, to support method chaining.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, TService value)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        return resolver.RegisterAnd(() => value);
    }

    /// <summary>
    /// Registers a constant instance of the specified service type with the dependency resolver and returns the
    /// resolver for chaining.
    /// </summary>
    /// <remarks>Use this method to register a singleton or constant value for a service type, ensuring that
    /// the same instance is returned for all requests matching the specified contract.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="resolver">The dependency resolver to which the service instance will be registered. Cannot be null.</param>
    /// <param name="value">The constant instance of the service to register. This instance will always be returned for the specified
    /// contract and service type.</param>
    /// <param name="contract">An optional contract string used to distinguish between multiple registrations of the same service type. Can be
    /// null or empty.</param>
    /// <returns>The same dependency resolver instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, TService value, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        return resolver.RegisterAnd(() => value, contract);
    }

    /// <summary>
    /// Registers a singleton service with lazy initialization and returns the dependency resolver for chaining.
    /// </summary>
    /// <remarks>The service instance is created on the first request using the specified factory and is
    /// shared for all subsequent requests. This method is useful for registering expensive or stateful services that
    /// should be instantiated only once.</remarks>
    /// <param name="resolver">The dependency resolver in which to register the singleton service. Cannot be null.</param>
    /// <param name="valueFactory">A delegate that provides the instance of the service when first requested. The factory is invoked only once, and
    /// the same instance is returned for subsequent requests.</param>
    /// <param name="serviceType">The type of the service to register as a singleton. Cannot be null.</param>
    /// <returns>The same dependency resolver instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd(this IMutableDependencyResolver resolver, Func<object> valueFactory, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<object>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType);
        return resolver;
    }

    /// <summary>
    /// Registers a singleton service in the dependency resolver using a factory method that is lazily evaluated, and
    /// returns the resolver to allow for method chaining.
    /// </summary>
    /// <remarks>The service instance is created the first time it is requested, and the same instance is
    /// returned for all subsequent requests. This method is thread-safe and ensures that the factory is invoked only
    /// once, even in multithreaded scenarios.</remarks>
    /// <param name="resolver">The dependency resolver in which to register the singleton service. Cannot be null.</param>
    /// <param name="valueFactory">A delegate that provides the instance of the service when first requested. The factory is invoked only once, and
    /// the same instance is returned for subsequent requests.</param>
    /// <param name="serviceType">The type of the service to register. This specifies the contract type that consumers will request.</param>
    /// <param name="contract">An optional contract name used to distinguish between multiple registrations of the same service type. May be
    /// null or empty for the default contract.</param>
    /// <returns>The same dependency resolver instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd(this IMutableDependencyResolver resolver, Func<object> valueFactory, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<object>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a lazily instantiated singleton of type TService with the specified dependency resolver and returns
    /// the resolver for chaining.
    /// </summary>
    /// <remarks>The singleton instance of TService is created on first request and reused for subsequent
    /// resolutions. This method is useful for registering services that are expensive to create or should be
    /// instantiated only once per resolver.</remarks>
    /// <typeparam name="TService">The type of service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver with which to register the singleton service. Cannot be null.</param>
    /// <returns>The same dependency resolver instance, to support method chaining.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value);
        return resolver;
    }

    /// <summary>
    /// Registers a singleton service of type TService using lazy initialization and returns the dependency resolver for
    /// chaining.
    /// </summary>
    /// <remarks>The service instance is created on first request and shared for all subsequent requests. This
    /// method is useful for registering services that are expensive to create or should only be instantiated once per
    /// resolver.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the singleton service. Cannot be null.</param>
    /// <param name="contract">An optional contract string used to distinguish between multiple registrations of the same service type.</param>
    /// <returns>The same dependency resolver instance, enabling method chaining.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a singleton service of type TService with the resolver using a factory method, and returns the
    /// resolver to enable method chaining.
    /// </summary>
    /// <remarks>The singleton instance of TService is created lazily and only once, upon the first request.
    /// Subsequent requests will return the same instance. This method is intended for use in fluent registration
    /// scenarios.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the singleton service. Cannot be null.</param>
    /// <param name="valueFactory">A factory method used to create the singleton instance of TService. Cannot be null.</param>
    /// <returns>The same IMutableDependencyResolver instance, to allow for fluent configuration.</returns>
#if NET6_0_OR_GREATER
    public static IMutableDependencyResolver RegisterLazySingletonAnd<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TService>(this IMutableDependencyResolver resolver, Func<TService> valueFactory)
#else
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> valueFactory)
#endif
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => valueFactory()!, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value);
        return resolver;
    }

    /// <summary>
    /// Registers a singleton service of type TService using a lazy value factory and returns the resolver for chaining.
    /// </summary>
    /// <remarks>The service instance is created lazily and only once, upon the first request. Subsequent
    /// requests return the same instance. This method is useful for registering expensive or stateful services that
    /// should be instantiated only when needed.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="resolver">The dependency resolver in which to register the singleton service. Cannot be null.</param>
    /// <param name="valueFactory">A function that creates an instance of TService when first requested. Cannot be null.</param>
    /// <param name="contract">An optional contract string used to distinguish between multiple registrations of the same service type. May be
    /// null.</param>
    /// <returns>The same IMutableDependencyResolver instance, enabling method chaining.</returns>
#if NET6_0_OR_GREATER
    public static IMutableDependencyResolver RegisterLazySingletonAnd<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TService>(this IMutableDependencyResolver resolver, Func<TService> valueFactory, string contract)
#else
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> valueFactory, string contract)
#endif
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => valueFactory()!, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value, contract);
        return resolver;
    }
}
