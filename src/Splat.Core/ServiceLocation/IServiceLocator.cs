// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// A generic-first service locator interface for AOT-friendly dependency resolution.
/// This interface provides only generic methods to avoid reflection and support ahead-of-time compilation.
/// </summary>
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
    /// Gets all instances of the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <returns>A collection of service instances.</returns>
    IEnumerable<T> GetServices<T>();

    /// <summary>
    /// Gets all instances of the specified service type with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <returns>A collection of service instances.</returns>
    IEnumerable<T> GetServices<T>(string contract);

    /// <summary>
    /// Tries to get an instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="service">The service instance if found; otherwise, the default value.</param>
    /// <returns><c>true</c> if the service was found; otherwise, <c>false</c>.</returns>
    bool TryGetService<T>([MaybeNullWhen(false)] out T service);

    /// <summary>
    /// Tries to get an instance of the specified service type with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <param name="service">The service instance if found; otherwise, the default value.</param>
    /// <returns><c>true</c> if the service was found; otherwise, <c>false</c>.</returns>
    bool TryGetService<T>(string contract, [MaybeNullWhen(false)] out T service);

    /// <summary>
    /// Gets a lazy instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <returns>A lazy instance of the service.</returns>
    Lazy<T> GetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>();

    /// <summary>
    /// Gets a lazy instance of the specified service type with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <returns>A lazy instance of the service.</returns>
    Lazy<T> GetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string contract);

    /// <summary>
    /// Tries to get a lazy instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="service">The lazy service instance if found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the service was found; otherwise, <c>false</c>.</returns>
    bool TryGetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>([MaybeNullWhen(false)] out Lazy<T> service);

    /// <summary>
    /// Tries to get a lazy instance of the specified service type with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <param name="service">The lazy service instance if found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the service was found; otherwise, <c>false</c>.</returns>
    bool TryGetLazyService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string contract, [MaybeNullWhen(false)] out Lazy<T> service);

    /// <summary>
    /// Checks if a service of the specified type is registered.
    /// </summary>
    /// <typeparam name="T">The service type to check.</typeparam>
    /// <returns><c>true</c> if the service is registered; otherwise, <c>false</c>.</returns>
    bool HasService<T>();

    /// <summary>
    /// Checks if a service of the specified type with a contract is registered.
    /// </summary>
    /// <typeparam name="T">The service type to check.</typeparam>
    /// <param name="contract">The contract name.</param>
    /// <returns><c>true</c> if the service is registered; otherwise, <c>false</c>.</returns>
    bool HasService<T>(string contract);

    /// <summary>
    /// Registers a transient service using a factory function.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    void AddService<T>(Func<T> instanceFactory);

    /// <summary>
    /// Registers a transient service using a factory function with a contract.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    /// <param name="contract">The contract name.</param>
    void AddService<T>(Func<T> instanceFactory, string contract);

    /// <summary>
    /// Registers a singleton service instance.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instance">The service instance.</param>
    void AddSingleton<TContract>(TContract instance)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service instance with a contract.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instance">The service instance.</param>
    /// <param name="contract">The contract name.</param>
    void AddSingleton<TContract>(TContract instance, string contract)
        where TContract : class;

    /// <summary>
    /// Registers a singleton service using a factory function.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    void AddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory);

    /// <summary>
    /// Registers a singleton service using a factory function with a contract.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    /// <param name="contract">The contract name.</param>
    void AddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract);

    /// <summary>
    /// Registers a lazy singleton service.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="lazy">The lazy service instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Lazy<TContract> lazy)
        where TContract : class;

    /// <summary>
    /// Registers a lazy singleton service using a factory function.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    /// <param name="threadSafetyMode">The thread safety mode for the lazy instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, LazyThreadSafetyMode threadSafetyMode)
        where TContract : class;

    /// <summary>
    /// Registers a lazy singleton service with a contract.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="lazy">The lazy service instance.</param>
    /// <param name="contract">The contract name.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Lazy<TContract> lazy, string contract)
        where TContract : class;

    /// <summary>
    /// Registers a lazy singleton service using a factory function with a contract.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    /// <param name="contract">The contract name.</param>
    /// <param name="threadSafetyMode">The thread safety mode for the lazy instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract, LazyThreadSafetyMode threadSafetyMode)
        where TContract : class;

    /// <summary>
    /// Registers a lazy singleton service using a factory function.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory)
        where TContract : class;

    /// <summary>
    /// Registers a lazy singleton service using a factory function with a contract.
    /// </summary>
    /// <typeparam name="TContract">The service type to register.</typeparam>
    /// <param name="instanceFactory">The factory function that creates the service instance.</param>
    /// <param name="contract">The contract name.</param>
    void AddLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TContract>(Func<TContract> instanceFactory, string contract)
        where TContract : class;
}
