// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Splat;

/// <summary>
/// A set of extension methods that assist with the <see cref="IDependencyResolver"/> and <see cref="IMutableDependencyResolver"/> interfaces.
/// </summary>
public static class DependencyResolverMixins
{
    /// <summary>
    /// Override the default Dependency Resolver until the object returned
    /// is disposed.
    /// </summary>
    /// <param name="resolver">The test resolver to use.</param>
    /// <param name="suppressResolverCallback">If we should suppress the resolver callback notify.</param>
    /// <returns>A disposable which will reset the resolver back to the original.</returns>
    public static IDisposable WithResolver(this IDependencyResolver resolver, bool suppressResolverCallback = true)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        var origResolver = AppLocator.GetLocator();

        // Start suppression BEFORE changing the locator if requested
        var notificationDisposable = suppressResolverCallback ? AppLocator.SuppressResolverCallbackChangedNotifications() : ActionDisposable.Empty;

        // Now change the locator while suppression is active
        AppLocator.SetLocator(resolver);

        return new CompositeDisposable(new ActionDisposable(() => AppLocator.SetLocator(origResolver)), notificationDisposable);
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="serviceType">The type of service to register.</param>
    public static void RegisterConstant(this IMutableDependencyResolver resolver, object? value, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        resolver.Register(() => value, serviceType);
    }

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    public static void RegisterConstant(this IMutableDependencyResolver resolver, object? value, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        resolver.Register(() => value, serviceType, contract);
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="serviceType">The type of service to register.</param>
    public static void RegisterLazySingleton(this IMutableDependencyResolver resolver, Func<object?> valueFactory, Type serviceType)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        var val = new Lazy<object?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType);
    }

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <param name="resolver">The resolver to register the service type with.</param>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="serviceType">The type of service to register.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    public static void RegisterLazySingleton(this IMutableDependencyResolver resolver, Func<object?> valueFactory, Type serviceType, string contract)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);
        var val = new Lazy<object?>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, serviceType, contract);
    }
}
