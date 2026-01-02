// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a dependency resolver where types can be registered after setup.
/// </summary>
public interface IMutableDependencyResolver
{
    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <param name="serviceType">The type to check for registration.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    bool HasRegistration(Type? serviceType);

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <param name="serviceType">The type to check for registration.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    bool HasRegistration(Type? serviceType, string? contract);

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <returns>Whether there is a registration for the type.</returns>
    bool HasRegistration<T>();

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    bool HasRegistration<T>(string? contract);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    void Register(Func<object?> factory, Type? serviceType);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    void Register(Func<object?> factory, Type? serviceType, string? contract);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <remarks>
    /// <para>
    /// This generic method is preferred over the non-generic <see cref="Register(Func{object}, Type)"/>
    /// method for better performance and type safety. It enables optimizations in resolvers like
    /// <see cref="GlobalGenericFirstDependencyResolver"/> which use static generic containers for zero-cost service resolution.
    /// </para>
    /// </remarks>
    void Register<T>(Func<T?> factory);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <remarks>
    /// <para>
    /// This generic method is preferred over the non-generic <see cref="Register(Func{object}, Type, string)"/>
    /// method for better performance and type safety. It enables optimizations in resolvers like
    /// <see cref="GlobalGenericFirstDependencyResolver"/> which use static generic containers for zero-cost service resolution.
    /// </para>
    /// </remarks>
    void Register<T>(Func<T?> factory, string? contract);

    /// <summary>
    /// Unregisters the current item based on the specified type.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    void UnregisterCurrent(Type? serviceType);

    /// <summary>
    /// Unregisters the current item based on the specified type and contract.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The contract value, which will only remove the value associated with the contract.</param>
    void UnregisterCurrent(Type? serviceType, string? contract);

    /// <summary>
    /// Unregisters the current item based on the specified type.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    void UnregisterCurrent<T>();

    /// <summary>
    /// Unregisters the current item based on the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    /// <param name="contract">The contract value, which will only remove the value associated with the contract.</param>
    void UnregisterCurrent<T>(string? contract);

    /// <summary>
    /// Unregisters all the values associated with the specified type.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    void UnregisterAll(Type? serviceType);

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The contract value, which will only remove the value associated with the contract.</param>
    void UnregisterAll(Type? serviceType, string? contract);

    /// <summary>
    /// Unregisters all the values associated with the specified type.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    void UnregisterAll<T>();

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    /// <param name="contract">The contract value, which will only remove the value associated with the contract.</param>
    void UnregisterAll<T>(string? contract);

    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="callback">The callback which will be called when the specified service type is registered.</param>
    IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback);

    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type
    /// and contract is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <param name="callback">The callback which will be called when the specified service type and contract are registered.</param>
    IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback);

    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="callback">The callback which will be called when the specified service type is registered.</param>
    IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback);

    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type
    /// and contract is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <param name="callback">The callback which will be called when the specified service type and contract are registered.</param>
    IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback);

    /// <summary>
    /// Registers a factory for a service type that will be registered as a different type.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class, must be a reference type).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type (must be a reference type, have a parameterless constructor, and implement TService).</typeparam>
    void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new();

    /// <summary>
    /// Registers a factory for a service type that will be registered as a different type.
    /// </summary>
    /// <typeparam name="TService">The service type to register as (interface or base class, must be a reference type).</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type (must be a reference type, have a parameterless constructor, and implement TService).</typeparam>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new();

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="value">The specified instance to always return.</param>
    void RegisterConstant<T>(T? value)
        where T : class;

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    void RegisterConstant<T>(T? value, string? contract)
        where T : class;

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class;

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class;
}
