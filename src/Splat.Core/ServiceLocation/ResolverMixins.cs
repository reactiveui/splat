// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Resolver Mixins.
/// </summary>
public static class ResolverMixins
{
    /// <summary>
    /// Registers a factory for the given <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new());
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new(), contract);
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="factory">A factory method for generating a object of the specified type.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> factory)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!);
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="factory">A factory method for generating a object of the specified type.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService>(this IMutableDependencyResolver resolver, Func<TService> factory, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TImplementation" /> that will be resolved as <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type that will be instantiated.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver)
        where TImplementation : TService, new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new TImplementation());
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TImplementation" /> that will be resolved as <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type that will be instantiated.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, string contract)
        where TImplementation : TService, new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register<TService>(() => new TImplementation(), contract);
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TImplementation" /> that will be resolved as <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type returned by the factory.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="factory">A factory method for generating a object of the specified type.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, Func<TImplementation> factory)
        where TImplementation : TService
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!);
        return resolver;
    }

    /// <summary>
    /// Registers a factory for the given <typeparamref name="TImplementation" /> that will be resolved as <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type returned by the factory.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="factory">A factory method for generating a object of the specified type.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterAnd<TService, TImplementation>(this IMutableDependencyResolver resolver, Func<TImplementation> factory, string contract)
        where TImplementation : TService
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        ArgumentExceptionHelper.ThrowIfNull(factory);

        resolver.Register<TService>(() => factory()!, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd(this IMutableDependencyResolver resolver, object value, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register(() => value, serviceType);
        return resolver;
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd(this IMutableDependencyResolver resolver, object value, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        resolver.Register(() => value, serviceType, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var value = new TService();
        return resolver.RegisterAnd(() => value);
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var value = new TService();
        return resolver.RegisterAnd(() => value, contract);
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, TService value)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        return resolver.RegisterAnd(() => value);
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterConstantAnd<TService>(this IMutableDependencyResolver resolver, TService value, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        return resolver.RegisterAnd(() => value, contract);
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd(this IMutableDependencyResolver resolver, Func<object> valueFactory, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<object>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType);
        return resolver;
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd(this IMutableDependencyResolver resolver, Func<object> valueFactory, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<object>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value);
        return resolver;
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TService>(this IMutableDependencyResolver resolver, string contract)
        where TService : new()
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var val = new Lazy<TService?>(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register<TService>(() => val.Value, contract);
        return resolver;
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <returns>The resolver.</returns>
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
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="TService">The service type to register for.</typeparam>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    /// <returns>The resolver.</returns>
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
