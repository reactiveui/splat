// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides extension methods for registering and managing dependency resolvers and service instances in a dependency
/// injection system.
/// </summary>
/// <remarks>These mixin methods extend dependency resolver interfaces to simplify common registration patterns,
/// such as registering constant or singleton services. They are intended to be used in scenarios where dynamic or
/// test-time configuration of dependency resolution is required.</remarks>
public static class DependencyResolverMixins
{
    /// <summary>Extension members for <see cref="IDependencyResolver"/>.</summary>
    /// <param name="resolver">The dependency resolver the extension members operate on.</param>
    extension(IDependencyResolver resolver)
    {
        /// <summary>
        /// Resolves through the specified resolver for the calling async flow, restoring the resolver the flow was
        /// using when the returned object is disposed.
        /// </summary>
        /// <remarks>Use this method to temporarily override the dependency resolver within a specific scope. The
        /// override travels with the async flow - across awaits and thread hops - and is invisible to every other
        /// flow, so tests running concurrently can each hold their own resolver without interfering. Ensure that the
        /// returned IDisposable is properly disposed to avoid leaving the flow in an inconsistent state.</remarks>
        /// <returns>An IDisposable that, when disposed, restores the resolver the flow was previously using.</returns>
        public IDisposable WithResolver() => resolver.WithResolver(true);

        /// <summary>
        /// Resolves through the specified resolver for the calling async flow, restoring the resolver the flow was
        /// using when the returned object is disposed.
        /// </summary>
        /// <remarks>Use this method to temporarily override the dependency resolver within a specific scope. The
        /// override travels with the async flow - across awaits and thread hops - and is invisible to every other
        /// flow, so tests running concurrently can each hold their own resolver without interfering. Ensure that the
        /// returned IDisposable is properly disposed to avoid leaving the flow in an inconsistent state.</remarks>
        /// <param name="suppressResolverCallback">true to suppress resolver callback changed notifications for the calling flow while the resolver is replaced; otherwise, false.</param>
        /// <returns>An IDisposable that, when disposed, restores the resolver the flow was previously using and re-enables notifications if
        /// they were suppressed.</returns>
        public IDisposable WithResolver(bool suppressResolverCallback)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);

            return AppLocator.InternalLocator.WithResolver(resolver, suppressResolverCallback);
        }
    }

    /// <summary>Extension members for <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="resolver">The mutable dependency resolver the extension members operate on.</param>
    extension(IMutableDependencyResolver resolver)
    {
        /// <summary>Registers a constant value as a service implementation for the specified service type in the dependency resolver.</summary>
        /// <remarks>After registration, requests for the specified service type will always return the provided
        /// constant value. This method is typically used to register singleton or pre-constructed instances.</remarks>
        /// <param name="value">The constant value to register as the service implementation. May be null if the service type allows null
        /// values.</param>
        /// <param name="serviceType">The type of service to associate with the constant value. Cannot be null.</param>
        public void RegisterConstant(object? value, Type serviceType)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);
            resolver.Register(() => value, serviceType);
        }

        /// <summary>
        /// Registers a constant value as a service implementation for the specified service type and contract in the
        /// dependency resolver.
        /// </summary>
        /// <remarks>After registration, all requests for the specified service type and contract will return the
        /// provided constant value. This method is typically used to register singleton or pre-constructed
        /// instances.</remarks>
        /// <param name="value">The constant value to register as the service implementation. May be null if the service type allows null
        /// values.</param>
        /// <param name="serviceType">The type of the service to associate with the constant value. Cannot be null.</param>
        /// <param name="contract">The contract name to associate with the registration. Use null or an empty string to register the service
        /// without a contract.</param>
        public void RegisterConstant(object? value, Type serviceType, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);
            resolver.Register(() => value, serviceType, contract);
        }

        /// <summary>Registers a singleton service with lazy initialization in the specified dependency resolver.</summary>
        /// <remarks>The singleton instance is created lazily and is shared for all subsequent resolutions of the
        /// specified service type. This method is thread-safe and ensures that the value factory is invoked only once, even
        /// in multithreaded scenarios.</remarks>
        /// <param name="valueFactory">A delegate that provides the value to be used as the singleton instance. The factory is invoked only once, upon
        /// first resolution.</param>
        /// <param name="serviceType">The type of the service to register as a singleton. Cannot be null.</param>
        public void RegisterLazySingleton(Func<object?> valueFactory, Type serviceType)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);
            var val = new Lazy<object?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            resolver.Register(() => val.Value, serviceType);
        }

        /// <summary>
        /// Registers a singleton service in the dependency resolver using lazy initialization. The service instance is
        /// created on first request and shared for all subsequent requests with the specified contract.
        /// </summary>
        /// <remarks>The service instance is created only once, upon the first request, and the same instance is
        /// returned for all subsequent requests with the specified contract. This method is thread-safe and ensures that
        /// the service is initialized only once, even in multithreaded scenarios.</remarks>
        /// <param name="valueFactory">A delegate that provides the instance of the service when it is first requested. Cannot be null.</param>
        /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
        /// <param name="contract">The contract string used to distinguish between multiple registrations of the same service type. Cannot be null.</param>
        public void RegisterLazySingleton(Func<object?> valueFactory, Type serviceType, string contract)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);
            var val = new Lazy<object?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            resolver.Register(() => val.Value, serviceType, contract);
        }
    }
}
