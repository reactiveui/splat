// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides a global access point for dependency resolution within the application, allowing services and types to be
/// registered and retrieved at runtime.
/// </summary>
/// <remarks>AppLocator exposes static members for resolving and registering dependencies using a default or
/// custom dependency resolver. It is intended for use by both application code and libraries that require service
/// location. In most scenarios, the default resolver is sufficient and should be used unless advanced customization or
/// testing scenarios require replacing it. Thread safety and resolver change notifications are managed internally. For
/// most applications, use the Current and CurrentMutable properties to access the dependency resolver.</remarks>
public static class AppLocator
{
    static AppLocator() => InternalLocator = new();

    /// <summary>
    /// Gets the read only dependency resolver. This class is used throughout
    /// libraries for many internal operations as well as for general use
    /// by applications. If this isn't assigned on startup, a default, highly
    /// capable implementation will be used, and it is advised for most people
    /// to simply use the default implementation.
    /// </summary>
    /// <value>The dependency resolver.</value>
    public static IReadonlyDependencyResolver Current => InternalLocator.Current;

    /// <summary>
    /// Gets the mutable dependency resolver.
    /// The default resolver is also a mutable resolver, so this will be non-null.
    /// Use this to register new types on startup if you are using the default resolver.
    /// </summary>
    public static IMutableDependencyResolver CurrentMutable => InternalLocator.CurrentMutable;

    /// <summary>
    /// Gets or sets the current locator instance.
    /// Used mostly for testing purposes.
    /// </summary>
    internal static InternalLocator InternalLocator { get; set; }

    /// <summary>
    /// Gets or sets the action used to reinitialize the dependency resolver.
    /// </summary>
    /// <remarks>This property is intended for internal use to allow resetting or reconfiguring the dependency
    /// resolver during application lifetime. Modifying this property may affect how dependencies are resolved within
    /// the application.</remarks>
    internal static Action<IMutableDependencyResolver> ReInit { get; set; } = _ => { };

    /// <summary>
    /// Sets the dependency resolver to be used by the application.
    /// </summary>
    /// <remarks>Call this method at application startup to configure the global dependency resolver.
    /// Subsequent calls will replace the existing resolver.</remarks>
    /// <param name="dependencyResolver">The dependency resolver instance that provides service resolution for the application. Cannot be null.</param>
    public static void SetLocator(IDependencyResolver dependencyResolver) => InternalLocator.SetLocator(dependencyResolver);

    /// <summary>
    /// Gets the current dependency resolver instance used by the application.
    /// </summary>
    /// <remarks>The returned resolver provides access to registered services and dependencies. The same
    /// instance is returned on each call. This method is intended for advanced scenarios where direct access to the
    /// dependency resolver is required.</remarks>
    /// <returns>An <see cref="IDependencyResolver"/> representing the application's current dependency resolver.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Existing API")]
    public static IDependencyResolver GetLocator() => InternalLocator.Internal;

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

        return InternalLocator.RegisterResolverCallbackChanged(callback);
    }

    /// <summary>
    /// This method will prevent resolver changed notifications from happening until
    /// the returned <see cref="IDisposable"/> is disposed.
    /// </summary>
    /// <returns>A disposable which when disposed will indicate the change
    /// notification is no longer needed.</returns>
    public static IDisposable SuppressResolverCallbackChangedNotifications() => InternalLocator.SuppressResolverCallbackChangedNotifications();

    /// <summary>
    /// Indicates if the we are notifying external classes of updates to the resolver being changed.
    /// </summary>
    /// <returns>A value indicating whether the notifications are happening.</returns>
    public static bool AreResolverCallbackChangedNotificationsEnabled() => InternalLocator.AreResolverCallbackChangedNotificationsEnabled();

    // Generic-first service resolution methods for AOT compatibility

    /// <summary>
    /// Gets an instance of the given service type. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    public static T? GetService<T>() => Current.GetService<T>();

    /// <summary>
    /// Gets an instance of the given service type. Must return <c>null</c>
    /// if the service is not available (must not throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">A value which will retrieve only a object registered with the same contract.</param>
    /// <returns>The requested object, if found; <c>null</c> otherwise.</returns>
    public static T? GetService<T>(string contract) => Current.GetService<T>(contract);

    /// <summary>
    /// Gets all instances of the given service type. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>A sequence of instances of the requested service type. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public static IEnumerable<T> GetServices<T>() => Current.GetServices<T>();

    /// <summary>
    /// Gets all instances of the given service type. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">A value which will retrieve only objects registered with the same contract.</param>
    /// <returns>A sequence of instances of the requested service type. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public static IEnumerable<T> GetServices<T>(string contract) => Current.GetServices<T>(contract);

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <returns>Whether there is a registration for the type.</returns>
    public static bool HasRegistration<T>() => CurrentMutable.HasRegistration<T>();

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <param name="contract">A contract value which will indicates to only check for the registration if this contract is specified.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    public static bool HasRegistration<T>(string contract) => CurrentMutable.HasRegistration<T>(contract);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <param name="factory">The factory function which generates our object.</param>
    public static void Register<T>(Func<T?> factory) => CurrentMutable.Register(factory);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// </summary>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="contract">A contract value which will indicates to only generate the value if this contract is specified.</param>
    public static void Register<T>(Func<T?> factory, string contract) => CurrentMutable.Register(factory, contract);

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="value">The specified instance to always return.</param>
    public static void RegisterConstant<T>(T? value)
        where T : class => CurrentMutable.RegisterConstant(value);

    /// <summary>
    /// Registers a constant value which will always return the specified object instance.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="value">The specified instance to always return.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    public static void RegisterConstant<T>(T? value, string contract)
        where T : class => CurrentMutable.RegisterConstant(value, contract);

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    public static void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory)
        where T : class =>
        CurrentMutable.RegisterLazySingleton(valueFactory);

    /// <summary>
    /// Registers a lazy singleton value which will always return the specified object instance once created.
    /// The value is only generated once someone requests the service from the resolver.
    /// </summary>
    /// <typeparam name="T">The service type to register for (must be a reference type).</typeparam>
    /// <param name="valueFactory">A factory method for generating a object of the specified type.</param>
    /// <param name="contract">A contract value which will indicates to only return the value if this contract is specified.</param>
    public static void RegisterLazySingleton<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T?> valueFactory, string contract)
        where T : class =>
        CurrentMutable.RegisterLazySingleton(valueFactory, contract);

    /// <summary>
    /// Unregisters the current the value for the specified type and the optional contract.
    /// </summary>
    /// <typeparam name="T">The type of item to unregister.</typeparam>
    public static void UnregisterCurrent<T>() => CurrentMutable.UnregisterCurrent<T>();

    /// <summary>
    /// Unregisters the current the value for the specified type and the optional contract.
    /// </summary>
    /// <typeparam name="T">The type of item to unregister.</typeparam>
    /// <param name="contract">A contract which indicates to only removed the item registered with this contract.</param>
    public static void UnregisterCurrent<T>(string contract) => CurrentMutable.UnregisterCurrent<T>(contract);

    /// <summary>
    /// Unregisters the all the values for the specified type and the optional contract.
    /// </summary>
    /// <typeparam name="T">The type of items to unregister.</typeparam>
    public static void UnregisterAll<T>() => CurrentMutable.UnregisterAll<T>();

    /// <summary>
    /// Unregisters the all the values for the specified type and the optional contract.
    /// </summary>
    /// <typeparam name="T">The type of items to unregister.</typeparam>
    /// <param name="contract">A contract which indicates to only removed those items registered with this contract.</param>
    public static void UnregisterAll<T>(string contract) => CurrentMutable.UnregisterAll<T>(contract);
}
