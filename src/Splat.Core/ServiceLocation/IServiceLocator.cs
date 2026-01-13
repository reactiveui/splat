// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Defines a contract for registering, retrieving, and managing service instances by type and optional contract name.
/// Enables dependency resolution and service location within an application or component.
/// </summary>
/// <remarks>The IServiceLocator interface provides methods for registering services as singletons, transients, or
/// lazy singletons, and for retrieving them by type or by type and contract name. It supports both direct retrieval and
/// safe, non-throwing attempts to resolve services. This interface is commonly used to decouple service consumers from
/// concrete implementations and to facilitate dependency injection patterns. Thread safety and lifetime management
/// depend on the specific implementation of the interface.</remarks>
public interface IServiceLocator
{
    /// <summary>
    /// Gets an instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    T GetService<T>();

    /// <summary>
    /// Gets an instance of the specified service type with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    T GetService<T>(string contract);

    /// <summary>
    /// Gets all registered service instances of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>An enumerable collection of service instances of type T. The collection is empty if no services of the specified
    /// type are registered.</returns>
    IEnumerable<T> GetServices<T>();

    /// <summary>
    /// Gets all registered service instances of the specified type that match the given contract name.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="contract">The contract name used to filter the services. Only services registered with this contract will be returned.
    /// Cannot be null.</param>
    /// <returns>An enumerable collection of service instances of type T that match the specified contract. The collection is
    /// empty if no matching services are found.</returns>
    IEnumerable<T> GetServices<T>(string contract);

    /// <summary>
    /// Attempts to retrieve a service object of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="service">When this method returns, contains the service object of type <typeparamref name="T"/> if found; otherwise, the
    /// default value for the type.</param>
    /// <returns><see langword="true"/> if a service object of type <typeparamref name="T"/> is found; otherwise, <see
    /// langword="false"/>.</returns>
    bool TryGetService<T>([MaybeNullWhen(false)] out T service);

    /// <summary>
    /// Attempts to retrieve a service of the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="contract">The contract name that identifies the service. Cannot be null.</param>
    /// <param name="service">When this method returns, contains the requested service if found; otherwise, the default value for the type.</param>
    /// <returns>true if the service was found and returned in the out parameter; otherwise, false.</returns>
    bool TryGetService<T>(string contract, [MaybeNullWhen(false)] out T service);

    /// <summary>
    /// Retrieves a lazily initialized instance of the specified service type from the dependency injection container.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve. Must have a public parameterless constructor.</typeparam>
    /// <returns>A <see cref="Lazy{T}"/> that provides access to the requested service instance. The service is created when the
    /// <see cref="Lazy{T}.Value"/> property is first accessed.</returns>
    Lazy<T> GetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>();

    /// <summary>
    /// Retrieves a lazily initialized service of the specified type associated with the given contract name.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve. Must have a public parameterless constructor.</typeparam>
    /// <param name="contract">The contract name that identifies the service to retrieve. Cannot be null.</param>
    /// <returns>A <see cref="Lazy{T}"/> instance that provides access to the requested service. The service is created when the
    /// <see cref="Lazy{T}.Value"/> property is first accessed.</returns>
    Lazy<T> GetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string contract);

    /// <summary>
    /// Attempts to retrieve a lazily initialized service of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve. Must have a public parameterless constructor.</typeparam>
    /// <param name="service">When this method returns, contains a Lazy{T} instance for the requested service if found; otherwise, null.</param>
    /// <returns>true if the service was found and assigned to the out parameter; otherwise, false.</returns>
    bool TryGetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>([MaybeNullWhen(false)] out Lazy<T> service);

    /// <summary>
    /// Attempts to retrieve a lazily initialized service of the specified type and contract.
    /// </summary>
    /// <remarks>Use this method to attempt to obtain a service without throwing an exception if the service
    /// is not available. The service is created only when the Lazy{T} is evaluated.</remarks>
    /// <typeparam name="T">The type of the service to retrieve. Must have a public parameterless constructor.</typeparam>
    /// <param name="contract">The contract name that uniquely identifies the requested service. Cannot be null.</param>
    /// <param name="service">When this method returns, contains a Lazy{T} instance for the requested service if found; otherwise, null.</param>
    /// <returns>true if a matching service is found and assigned to the out parameter; otherwise, false.</returns>
    bool TryGetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string contract, [MaybeNullWhen(false)] out Lazy<T> service);

    /// <summary>
    /// Determines whether a service of the specified type is available from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to check for availability.</typeparam>
    /// <returns>true if a service of type T is available; otherwise, false.</returns>
    bool HasService<T>();

    /// <summary>
    /// Determines whether a service of the specified type and contract is available from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of the service to check for.</typeparam>
    /// <param name="contract">The contract name that identifies the service. Cannot be null.</param>
    /// <returns>true if a service of type T with the specified contract is available; otherwise, false.</returns>
    bool HasService<T>(string contract);

    /// <summary>
    /// Registers a transient service of the specified type using the provided factory function.
    /// </summary>
    /// <remarks>Use this method to register services that require custom instantiation logic or dependencies
    /// not handled by default constructors. The factory function should not return null.</remarks>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="instanceFactory">A function that returns an instance of the service type. This function is called each time an instance is
    /// requested.</param>
    void AddService<T>(Func<T> instanceFactory);

    /// <summary>
    /// Registers a transient service implementation with the specified contract, using the provided factory to create instances
    /// of the service type.
    /// </summary>
    /// <remarks>Use this method to associate a service implementation with a specific contract, allowing for
    /// multiple implementations of the same service type to be registered under different contracts.</remarks>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="instanceFactory">A factory function that creates instances of the service type. Cannot be null.</param>
    /// <param name="contract">The contract name under which the service is registered. Cannot be null or empty.</param>
    void AddService<T>(Func<T> instanceFactory, string contract);

    /// <summary>
    /// Registers the specified instance as a singleton service for the given contract type.
    /// </summary>
    /// <remarks>Use this method to provide a specific instance that will be shared across all consumers of
    /// the contract type. The same instance is returned each time the service is requested.</remarks>
    /// <typeparam name="TContract">The contract type of the service to register. Must be a reference type.</typeparam>
    /// <param name="instance">The instance to use for the singleton service. This instance will be returned for all requests of the specified
    /// contract type. Cannot be null.</param>
    void AddSingleton<TContract>(TContract instance)
        where TContract : class;

    /// <summary>
    /// Registers the specified instance as a singleton for the given contract type and contract name.
    /// </summary>
    /// <remarks>Subsequent requests for the specified contract type and contract name will return the same
    /// instance. This method is typically used to provide a pre-constructed or externally managed singleton to the
    /// dependency injection container.</remarks>
    /// <typeparam name="TContract">The contract type to associate with the singleton instance. Must be a reference type.</typeparam>
    /// <param name="instance">The instance to register as a singleton. This instance will be returned for all requests matching the specified
    /// contract type and contract name.</param>
    /// <param name="contract">The contract name used to distinguish this registration. Cannot be null or empty.</param>
    void AddSingleton<TContract>(TContract instance, string contract)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service of the specified contract type using the provided factory function.
    /// </summary>
    /// <remarks>Subsequent requests for the service will return the same instance created by the factory.
    /// This method is typically used to register services that should have a single shared instance for the
    /// application's lifetime.</remarks>
    /// <typeparam name="TContract">The type of the service to register. The type must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A function that returns an instance of the service to be registered as a singleton. This factory is called once
    /// to create the singleton instance.</param>
    void AddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory);

    /// <summary>
    /// Registers a singleton service of the specified contract type using the provided factory function and associates
    /// it with the given contract name.
    /// </summary>
    /// <remarks>Subsequent requests for the specified contract will return the same singleton instance
    /// created by the factory. This method is typically used to register services that should have a single shared
    /// instance for the application's lifetime.</remarks>
    /// <typeparam name="TContract">The type of the service to register. The type must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A function that creates an instance of the service to be registered as a singleton. This factory is called once
    /// to create the singleton instance.</param>
    /// <param name="contract">The name of the contract to associate with the registered singleton service. Cannot be null or empty.</param>
    void AddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract);

    /// <summary>
    /// Registers a lazily initialized singleton instance for the specified contract type.
    /// </summary>
    /// <remarks>This method allows deferred creation of the singleton instance. The instance will be created
    /// only when first requested from the container. Subsequent requests will return the same instance.</remarks>
    /// <typeparam name="TContract">The contract type to associate with the singleton instance. Must have a public parameterless constructor.</typeparam>
    /// <param name="lazy">A lazily initialized instance of the contract type to register as a singleton. Cannot be null.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Lazy<TContract> lazy)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service of the specified contract type, where the instance is created lazily using the
    /// provided factory and with the specified thread safety mode.
    /// </summary>
    /// <remarks>The singleton instance is not created until it is first requested. The thread safety behavior
    /// during instance creation is determined by the specified thread safety mode. This method is useful for deferring
    /// expensive object creation or for services that should be instantiated only when needed.</remarks>
    /// <typeparam name="TContract">The contract type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A delegate that creates an instance of the contract type when the singleton is first requested. Cannot be null.</param>
    /// <param name="threadSafetyMode">Specifies the thread safety mode to use when creating the singleton instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, LazyThreadSafetyMode threadSafetyMode)
        where TContract : class;

    /// <summary>
    /// Registers a lazily initialized singleton instance for the specified contract type and contract name.
    /// </summary>
    /// <remarks>Use this method to register a singleton that is created only when first requested, which can
    /// improve startup performance or defer expensive initialization. Subsequent requests for the contract will return
    /// the same instance.</remarks>
    /// <typeparam name="TContract">The contract type to associate with the singleton instance. Must have a public parameterless constructor.</typeparam>
    /// <param name="lazy">A lazily initialized instance of the contract type to register as a singleton. Cannot be null.</param>
    /// <param name="contract">The contract name used to identify the singleton registration. Cannot be null or empty.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Lazy<TContract> lazy, string contract)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service of the specified contract type that is created lazily using the provided factory
    /// and thread safety mode.
    /// </summary>
    /// <remarks>The singleton instance is not created until it is first requested. The specified thread
    /// safety mode determines how concurrent access is handled during instance creation. Registering multiple
    /// singletons with the same contract name may result in unexpected behavior.</remarks>
    /// <typeparam name="TContract">The type of the contract to register as a singleton. Must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A delegate that creates an instance of the contract type when the singleton is first requested. Cannot be null.</param>
    /// <param name="contract">The unique contract name used to identify the singleton registration. Cannot be null or empty.</param>
    /// <param name="threadSafetyMode">Specifies the thread safety mode to use when creating the singleton instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract, LazyThreadSafetyMode threadSafetyMode)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service of the specified contract type using a factory method. The service instance is
    /// created lazily upon first request and reused for subsequent requests.
    /// </summary>
    /// <remarks>Use this method to defer the creation of a singleton service until it is actually needed.
    /// This can improve startup performance and resource usage if the service is expensive to create or may not always
    /// be required.</remarks>
    /// <typeparam name="TContract">The contract type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A factory method that creates an instance of the service. The factory is invoked only once, when the service is
    /// first requested.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service of the specified contract type using a factory method. The service instance is
    /// created lazily upon first request and reused for subsequent requests.
    /// </summary>
    /// <remarks>Use this method to register services that should be instantiated only when needed and shared
    /// as a singleton for the lifetime of the container. The same instance will be returned for all requests with the
    /// specified contract name.</remarks>
    /// <typeparam name="TContract">The contract type of the service to register. Must have a public parameterless constructor.</typeparam>
    /// <param name="instanceFactory">A factory method that creates an instance of the contract type. The factory is invoked only once, when the
    /// service is first requested.</param>
    /// <param name="contract">The unique contract name used to identify the service registration. Cannot be null or empty.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract)
        where TContract : class;
}
