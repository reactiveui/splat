// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// An interface for interacting with a dependency resolver in a read-only fashion.
/// </summary>
public interface IReadonlyDependencyResolver
{
    /// <summary>
    /// Gets an instance of the given <paramref name="serviceType"/>. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    object? GetService(Type? serviceType);

    /// <summary>
    /// Gets an instance of the given <paramref name="serviceType"/>. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <param name="contract">A value which will retrieve only a object registered with the same contract.</param>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    object? GetService(Type? serviceType, string? contract);

    /// <summary>
    /// Gets an instance of the given <typeparamref name="T"/>. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    T? GetService<T>();

    /// <summary>
    /// Gets an instance of the given <typeparamref name="T"/>. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">A value which will retrieve only a object registered with the same contract.</param>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    T? GetService<T>(string? contract);

    /// <summary>
    /// Gets all instances of the given <paramref name="serviceType"/>. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <returns>A sequence of instances of the requested <paramref name="serviceType"/>. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    IEnumerable<object> GetServices(Type? serviceType);

    /// <summary>
    /// Gets all instances of the given <paramref name="serviceType"/>. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <param name="contract">A value which will retrieve only objects registered with the same contract.</param>
    /// <returns>A sequence of instances of the requested <paramref name="serviceType"/>. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    IEnumerable<object> GetServices(Type? serviceType, string? contract);

    /// <summary>
    /// Gets all instances of the given <typeparamref name="T"/>. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>A sequence of instances of the requested <typeparamref name="T"/>. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    IEnumerable<T> GetServices<T>();

    /// <summary>
    /// Gets all instances of the given <typeparamref name="T"/>. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">A value which will retrieve only objects registered with the same contract.</param>
    /// <returns>A sequence of instances of the requested <typeparamref name="T"/>. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    IEnumerable<T> GetServices<T>(string? contract);
}
