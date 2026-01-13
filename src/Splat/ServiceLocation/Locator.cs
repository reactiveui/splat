// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides access to the application's dependency resolver and related functionality for service location and
/// registration.
/// This is now a wrapper around <see cref="AppLocator"/> which is the actual implementation.
/// </summary>
/// <remarks>The Locator class exposes static members to retrieve or set the application's dependency resolver, as
/// well as to register callbacks for resolver changes. Most applications should use the default resolver provided by
/// this class. Advanced scenarios, such as library development or custom dependency resolution, may require interacting
/// with the mutable resolver or registering for resolver change notifications.</remarks>
public static class Locator
{
    static Locator() => AppLocator.ReInit = resolver => resolver.InitializeSplat();

    /// <summary>
    /// Gets the current dependency resolver used by the application.
    /// </summary>
    /// <remarks>Use this property to resolve application services and dependencies at runtime. The value is
    /// typically set during application startup and remains constant for the application's lifetime.
    /// If this isn't assigned on startup, a default, highly
    /// capable implementation will be used, and it is advised for most people to simply use the default implementation.</remarks>
    public static IReadonlyDependencyResolver Current => AppLocator.Current;

    /// <summary>
    /// Gets the current mutable dependency resolver for the application.
    /// </summary>
    /// <remarks>Use this property to register or override service implementations at runtime. Changes made to
    /// the resolver affect dependency resolution for the lifetime of the application. This property is typically used
    /// during application startup or for testing scenarios where services need to be replaced.</remarks>
    public static IMutableDependencyResolver CurrentMutable => AppLocator.CurrentMutable;

    /// <summary>
    /// Sets the dependency resolver to be used by the application for resolving service dependencies.
    /// </summary>
    /// <remarks>Call this method during application startup to configure the global dependency resolution
    /// strategy. Subsequent service resolutions will use the specified resolver until it is replaced.</remarks>
    /// <param name="dependencyResolver">The dependency resolver instance that provides service resolution for the application. Cannot be null.</param>
    public static void SetLocator(IDependencyResolver dependencyResolver) => AppLocator.SetLocator(dependencyResolver);

    /// <summary>
    /// Gets the full locator.
    /// Note you should use <see cref="Current"/> or <see cref="CurrentMutable"/> in most situations.
    /// </summary>
    /// <returns>The locator.</returns>
    public static IDependencyResolver GetLocator() => AppLocator.GetLocator();

    /// <summary>
    /// This method allows libraries to register themselves to be set up
    /// whenever the dependency resolver changes. Applications should avoid
    /// this method, it is usually used for libraries that depend on service
    /// location.
    /// </summary>
    /// <param name="callback">A callback that is invoked when the
    /// resolver is changed. This callback is also invoked immediately,
    /// to configure the current resolver.</param>
    /// <returns>When disposed, removes the callback. You probably can
    /// ignore this.</returns>
    public static IDisposable RegisterResolverCallbackChanged(Action callback)
    {
        ArgumentExceptionHelper.ThrowIfNull(callback);

        return AppLocator.RegisterResolverCallbackChanged(callback);
    }

    /// <summary>
    /// Temporarily suppresses notifications for resolver callback changes until the returned object is disposed.
    /// </summary>
    /// <remarks>Use this method to prevent resolver callback change notifications from being raised during a
    /// batch of operations. Notifications will resume automatically when the returned object is disposed. This method
    /// is thread-safe.</remarks>
    /// <returns>An <see cref="IDisposable"/> that, when disposed, restores resolver callback change notifications.</returns>
    public static IDisposable SuppressResolverCallbackChangedNotifications() => AppLocator.SuppressResolverCallbackChangedNotifications();

    /// <summary>
    /// Determines whether notifications are enabled when the resolver callback changes.
    /// </summary>
    /// <returns>true if resolver callback changed notifications are enabled; otherwise, false.</returns>
    public static bool AreResolverCallbackChangedNotificationsEnabled() => AppLocator.AreResolverCallbackChangedNotificationsEnabled();
}
