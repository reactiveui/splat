// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents a mutable dependency resolver that allows for the registration, unregistration, and querying of service
/// factories and instances at runtime. Enables dynamic management of service lifetimes and contracts within a
/// dependency injection system.
/// </summary>
/// <remarks>Implementations of this interface support advanced scenarios such as registering services with
/// specific contracts, handling multiple registrations per service type, and reacting to service registration events
/// via callbacks. This interface is typically used to extend or modify the set of available services during application
/// execution, such as in plugin architectures or for testing purposes. Thread safety and registration order semantics
/// may vary by implementation; consult the specific resolver's documentation for details.</remarks>
public interface IMutableDependencyResolver
{
    /// <summary>
    /// Determines whether a registration exists for the specified service type.
    /// </summary>
    /// <param name="serviceType">The type of the service to check for registration. Can be null to indicate an unspecified service type.</param>
    /// <returns>true if a registration exists for the specified service type; otherwise, false.</returns>
    bool HasRegistration(Type? serviceType);

    /// <summary>
    /// Determines whether a registration exists for the specified service type and contract.
    /// </summary>
    /// <param name="serviceType">The type of the service to check for registration. Can be null to indicate a default or unspecified service
    /// type, depending on the implementation.</param>
    /// <param name="contract">An optional contract name that distinguishes between multiple registrations of the same service type. Can be
    /// null or empty to indicate the default contract.</param>
    /// <returns>true if a registration exists for the specified service type and contract; otherwise, false.</returns>
    bool HasRegistration(Type? serviceType, string? contract);

    /// <summary>
    /// Determines whether a registration exists for the specified service type.
    /// </summary>
    /// <typeparam name="T">The type of the service to check for registration.</typeparam>
    /// <returns>true if a registration for the specified service type exists; otherwise, false.</returns>
    bool HasRegistration<T>();

    /// <summary>
    /// Determines whether a registration exists for the specified service type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to check for a registration.</typeparam>
    /// <param name="contract">An optional contract name that identifies a specific registration. Can be null to check for the default
    /// registration.</param>
    /// <returns>true if a registration exists for the specified service type and contract; otherwise, false.</returns>
    bool HasRegistration<T>(string? contract);

    /// <summary>
    /// Registers a factory method for creating instances of the specified service type.
    /// </summary>
    /// <remarks>Use this method to provide custom logic for creating service instances. If serviceType is
    /// null, the registration mechanism may attempt to infer the service type from the factory delegate's return
    /// type.</remarks>
    /// <param name="factory">A delegate that returns an instance of the service. The delegate may return null if appropriate for the service.</param>
    /// <param name="serviceType">The type of the service to register. If null, the type may be inferred from the factory's return type.</param>
    void Register(Func<object?> factory, Type? serviceType);

    /// <summary>
    /// Registers a factory method for creating instances of a specified service type and contract.
    /// </summary>
    /// <remarks>Use this method to register services with custom creation logic, such as when dependencies or
    /// configuration are required at instantiation. If multiple registrations exist for the same service type and
    /// contract, the most recent registration may take precedence, depending on the container's behavior.</remarks>
    /// <param name="factory">A delegate that returns an instance of the service to register. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. If null, the type is inferred from the factory's return value.</param>
    /// <param name="contract">An optional contract name that distinguishes this registration from others of the same service type. Can be null
    /// or empty for the default contract.</param>
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
    /// Unregisters the current instance of the specified service type from the context.
    /// </summary>
    /// <remarks>If no instance of the specified service type is registered, this method has no
    /// effect.</remarks>
    /// <param name="serviceType">The type of the service to unregister. If null, the default service type is used.</param>
    void UnregisterCurrent(Type? serviceType);

    /// <summary>
    /// Unregisters the current instance of the specified service type and contract from the container.
    /// </summary>
    /// <param name="serviceType">The type of the service to unregister. If null, the default service type is used.</param>
    /// <param name="contract">An optional contract name that identifies the registration to remove. If null, the default contract is used.</param>
    void UnregisterCurrent(Type? serviceType, string? contract);

    /// <summary>
    /// Unregisters the current instance of the specified type from the context.
    /// </summary>
    /// <typeparam name="T">The type of the instance to unregister from the current context.</typeparam>
    void UnregisterCurrent<T>();

    /// <summary>
    /// Unregisters the current instance of type T associated with the specified contract, if any.
    /// </summary>
    /// <remarks>If no instance of type T is registered with the specified contract, this method has no
    /// effect.</remarks>
    /// <typeparam name="T">The type of the instance to unregister.</typeparam>
    /// <param name="contract">An optional contract name that specifies which registration to remove. If null, the default registration for
    /// type T is unregistered.</param>
    void UnregisterCurrent<T>(string? contract);

    /// <summary>
    /// Unregisters all service registrations for the specified service type.
    /// </summary>
    /// <param name="serviceType">The type of service for which to remove all registrations. If null, all service registrations are removed.</param>
    void UnregisterAll(Type? serviceType);

    /// <summary>
    /// Unregisters all service registrations that match the specified service type and contract.
    /// </summary>
    /// <param name="serviceType">The type of the service to unregister. If null, all service types are considered.</param>
    /// <param name="contract">The contract name to match when unregistering services. If null, all contracts are considered.</param>
    void UnregisterAll(Type? serviceType, string? contract);

    /// <summary>
    /// Unregisters all instances of the specified type from the registry or container.
    /// </summary>
    /// <remarks>After calling this method, no instances of type T will remain registered. Subsequent requests
    /// for T may fail or result in new registrations, depending on the container's behavior.</remarks>
    /// <typeparam name="T">The type of objects to unregister.</typeparam>
    void UnregisterAll<T>();

    /// <summary>
    /// Unregisters all instances of the specified type that are associated with the given contract.
    /// </summary>
    /// <typeparam name="T">The type of the instances to unregister.</typeparam>
    /// <param name="contract">The contract name used to identify the registrations to remove. Can be null to target registrations without a
    /// contract.</param>
    void UnregisterAll<T>(string? contract);

    /// <summary>
    /// Registers a callback to be invoked when a service of the specified type is registered or becomes available.
    /// </summary>
    /// <param name="serviceType">The type of the service to monitor for registration. Cannot be null.</param>
    /// <param name="callback">The action to invoke when the service is registered. The callback receives an IDisposable representing the
    /// service registration. Cannot be null.</param>
    /// <returns>An IDisposable that can be used to unregister the callback.</returns>
    IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback);

    /// <summary>
    /// Registers a callback to be invoked when a service of the specified type and contract is registered or
    /// unregistered.
    /// </summary>
    /// <remarks>The callback is invoked whenever a matching service is registered or unregistered. Disposing
    /// the returned IDisposable will stop further notifications.</remarks>
    /// <param name="serviceType">The type of the service to monitor for registration events. Cannot be null.</param>
    /// <param name="contract">An optional contract name that further qualifies the service type. May be null to match any contract.</param>
    /// <param name="callback">An action to invoke when the service registration changes. The callback receives an IDisposable representing the
    /// registration. Cannot be null.</param>
    /// <returns>An IDisposable that can be disposed to unregister the callback.</returns>
    IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback);

    /// <summary>
    /// Registers a callback to be invoked when a service of type T is registered, and returns a disposable object that
    /// can be used to unregister the callback.
    /// </summary>
    /// <typeparam name="T">The type of the service to monitor for registration events.</typeparam>
    /// <param name="callback">The action to invoke when a service of type T is registered. The callback receives an IDisposable that can be
    /// used to unregister the callback.</param>
    /// <returns>An IDisposable that, when disposed, unregisters the callback.</returns>
    IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback);

    /// <summary>
    /// Registers a callback to be invoked when a service of type T is registered under the specified contract.
    /// </summary>
    /// <remarks>Use this method to observe dynamic service registrations and perform actions when services
    /// become available. The callback is invoked each time a matching service is registered. Disposing the returned
    /// IDisposable will stop further notifications.</remarks>
    /// <typeparam name="T">The type of the service to monitor for registration.</typeparam>
    /// <param name="contract">The contract name to filter service registrations. If null, the callback is invoked for all contracts.</param>
    /// <param name="callback">The action to invoke when a matching service is registered. Receives an IDisposable that can be used to
    /// unregister the callback.</param>
    /// <returns>An IDisposable that, when disposed, unregisters the callback.</returns>
    IDisposable ServiceRegistrationCallback<T>(string? contract, Action<IDisposable> callback);

    /// <summary>
    /// Registers a service type and its implementation for dependency resolution.
    /// </summary>
    /// <remarks>Subsequent requests for TService will resolve to instances of TImplementation. If the service
    /// type is already registered, this method may overwrite the existing registration depending on the
    /// implementation.</remarks>
    /// <typeparam name="TService">The interface or base class type to register as a service. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to instantiate when resolving the service. Must be a reference type, implement
    /// TService, and have a public parameterless constructor.</typeparam>
    void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new();

    /// <summary>
    /// Registers a service implementation with an optional contract name for dependency resolution.
    /// </summary>
    /// <remarks>Use this method to associate a service interface or base class with a specific
    /// implementation, optionally under a contract name. This enables resolving different implementations of the same
    /// service type by contract. If multiple implementations are registered for the same service and contract, the
    /// behavior may depend on the container's resolution strategy.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to register for the service. Must be a reference type with a public
    /// parameterless constructor.</typeparam>
    /// <param name="contract">An optional contract name that distinguishes this registration from others of the same service type. Specify
    /// null to register the implementation without a contract.</param>
    void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new();

    /// <summary>
    /// Registers a constant value of the specified reference type for later retrieval or use.
    /// </summary>
    /// <typeparam name="T">The reference type of the constant value to register.</typeparam>
    /// <param name="value">The constant value to register. Can be null to represent the absence of a value.</param>
    void RegisterConstant<T>(T? value)
        where T : class;

    /// <summary>
    /// Registers a constant instance of the specified type for use in dependency resolution.
    /// </summary>
    /// <remarks>Registering a constant ensures that the same instance is returned for all requests matching
    /// the specified type and contract. If a contract is provided, the constant is associated only with that contract;
    /// otherwise, it is registered for the type without a contract.</remarks>
    /// <typeparam name="T">The type of the constant instance to register. Must be a reference type.</typeparam>
    /// <param name="value">The constant instance to register. Can be null if null values are supported by the container.</param>
    /// <param name="contract">An optional contract name that uniquely identifies the registration. Can be null to register without a contract.</param>
    void RegisterConstant<T>(T? value, string? contract)
        where T : class;

    /// <summary>
    /// Registers a singleton service of type T that is created lazily using the specified factory function.
    /// </summary>
    /// <remarks>The service instance is not created until it is first requested. Subsequent requests will
    /// return the same instance. Registering multiple lazy singletons of the same type may result in only the first
    /// registration being used, depending on the container's behavior.</remarks>
    /// <typeparam name="T">The type of the service to register. Must be a reference type with a public parameterless constructor.</typeparam>
    /// <param name="valueFactory">A function that provides the instance of type T when the service is first requested. The function may return
    /// null if no instance should be registered.</param>
    void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class;

    /// <summary>
    /// Registers a singleton service of type T that is created lazily using the specified factory method.
    /// </summary>
    /// <remarks>The singleton instance is not created until it is first requested. Subsequent requests for
    /// the service will return the same instance. Registering multiple lazy singletons with the same contract will
    /// overwrite previous registrations.</remarks>
    /// <typeparam name="T">The type of the service to register. Must be a reference type with a public parameterless constructor.</typeparam>
    /// <param name="valueFactory">A function that provides the instance of T when the singleton is first requested. May return null if a null
    /// singleton is desired.</param>
    /// <param name="contract">An optional contract name used to distinguish between multiple registrations of the same service type. If null,
    /// the default contract is used.</param>
    void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string? contract)
        where T : class;
}
