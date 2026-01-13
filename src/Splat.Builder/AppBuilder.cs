// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Provides a builder for configuring and constructing application dependency resolution and service registration using
/// Splat patterns.
/// </summary>
/// <remarks>The AppBuilder class enables fluent configuration of dependency resolvers, service modules, and
/// custom registrations for applications using Splat. It supports chaining of registration methods and
/// ensures that core services are registered before building the application instance. AppBuilder is typically used
/// during application startup to configure dependency injection and service location. Thread safety is not guaranteed;
/// configuration should be completed before the application is accessed from multiple threads.</remarks>
public class AppBuilder : IAppBuilder, IAppInstance
{
    private readonly List<Action<IMutableDependencyResolver>> _registrations = [];
    private Func<IMutableDependencyResolver> _resolverProvider;
    private Func<IReadonlyDependencyResolver?> _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBuilder"/> class with the specified dependency resolver and an optional.
    /// current resolver.
    /// </summary>
    /// <param name="resolver">The dependency resolver to use for registering and resolving services. Cannot be null.</param>
    /// <param name="current">An optional read-only dependency resolver to use as the current resolver. If null, only the mutable resolver is
    /// used.</param>
    public AppBuilder(IMutableDependencyResolver resolver, IReadonlyDependencyResolver? current = null)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        UsingBuilder = true;
        CurrentMutable = resolver;
        Current = current;
        _resolverProvider = () => CurrentMutable;
        _serviceProvider = () => Current;
    }

    /// <summary>
    /// Gets a value indicating whether this instance has been built.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has been built; otherwise, <c>false</c>.
    /// </value>
    public static bool HasBeenBuilt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the application builder is being used.
    /// </summary>
    /// <value>
    ///   <c>true</c> if using the application builder; otherwise, <c>false</c>.
    /// </value>
    public static bool UsingBuilder { get; private set; }

    /// <summary>
    /// Gets the current dependency resolver in use by the application.
    /// </summary>
    /// <remarks>The dependency resolver is responsible for providing service instances throughout the
    /// application's lifetime. If no resolver has been set, this property may return null.</remarks>
    public IReadonlyDependencyResolver? Current { get; private set; }

    /// <summary>
    /// Gets the current mutable dependency resolver used for registering and resolving services at runtime.
    /// </summary>
    /// <remarks>Use this property to add or override service registrations dynamically. Changes made to the
    /// resolver affect dependency resolution for subsequent requests.</remarks>
    public IMutableDependencyResolver CurrentMutable { get; }

    /// <summary>
    /// Creates a new instance of the application builder using the current mutable and immutable service locators.
    /// </summary>
    /// <remarks>Use this method to configure dependency injection and service registration for the
    /// application using the Splat framework. The returned builder is preconfigured with the application's current
    /// service locators.</remarks>
    /// <returns>An <see cref="AppBuilder"/> initialized with the current service locator configuration.</returns>
    public static AppBuilder CreateSplatBuilder() => new(AppLocator.CurrentMutable, AppLocator.Current);

    /// <summary>
    /// Resets the internal builder state to its initial values for use in unit tests.
    /// </summary>
    /// <remarks>This method is intended for test scenarios only. It should not be used in production code, as
    /// it may affect the global state shared by other components.</remarks>
    public static void ResetBuilderStateForTests()
    {
        HasBeenBuilt = false;
        UsingBuilder = false;
    }

    /// <summary>
    /// Configures the application to use the current Splat service locator (AppLocator.CurrentMutable) for dependency resolution.
    /// </summary>
    /// <remarks>This method sets the resolver and service provider delegates to use the current Splat <see
    /// cref="AppLocator"/>. Use this method when you want the application to resolve dependencies using the global
    /// Splat locator, which may be shared across different parts of the application.
    /// This is useful when configuring an external container (e.g., Autofac, DryIoc, Microsoft.Extensions.DependencyInjection)
    /// as the Splat dependency resolver prior to applying Splat registrations.</remarks>
    /// <returns>The current <see cref="IAppBuilder"/> instance. This enables method chaining.</returns>
    public IAppBuilder UseCurrentSplatLocator()
    {
        _resolverProvider = () => AppLocator.CurrentMutable;
        _serviceProvider = () => AppLocator.Current;
        return this;
    }

    /// <summary>
    /// Registers a module for application configuration using the specified module instance.
    /// </summary>
    /// <typeparam name="T">The type of the module to register. Must implement the <see cref="IModule"/> interface.</typeparam>
    /// <param name="registrationModule">The module instance to register. Cannot be null.</param>
    /// <returns>The current <see cref="IAppBuilder"/> instance to allow method chaining.</returns>
    public IAppBuilder UsingModule<T>(T registrationModule)
        where T : IModule
    {
        ArgumentExceptionHelper.ThrowIfNull(registrationModule);
        _registrations.Add(registrationModule.Configure);
        return this;
    }

    /// <summary>
    /// Adds a custom dependency registration action to the application builder.
    /// </summary>
    /// <remarks>Use this method to register additional services or override default registrations in the
    /// application's dependency resolver before the application is built.</remarks>
    /// <param name="configureAction">An action that receives an <see cref="IMutableDependencyResolver"/> and performs custom service registrations.
    /// Cannot be null.</param>
    /// <returns>The current <see cref="IAppBuilder"/> instance, enabling method chaining.</returns>
    public IAppBuilder WithCustomRegistration(Action<IMutableDependencyResolver> configureAction)
    {
        ArgumentExceptionHelper.ThrowIfNull(configureAction);

        _registrations.Add(configureAction);
        return this;
    }

    /// <summary>
    /// Adds the core framework services to the application builder.
    /// </summary>
    /// <remarks>This method is typically called during application startup to ensure that essential services
    /// required by the framework are available. It can be used in a fluent configuration chain.</remarks>
    /// <returns>The current <see cref="IAppBuilder"/> instance with core services registered.</returns>
    public virtual IAppBuilder WithCoreServices() => this;

    /// <summary>
    /// Finalizes the configuration and builds the application instance, making it ready for use.
    /// </summary>
    /// <remarks>Subsequent calls to this method after the initial build have no effect and return the same
    /// instance. After building, further modifications to the builder's configuration are not applied.</remarks>
    /// <returns>The current application instance with all configured services and registrations applied.</returns>
    public IAppInstance Build()
    {
        // If the builder has already been built, do nothing.
        if (HasBeenBuilt)
        {
            return this;
        }

        // Mark as initialized using the builder so reflection-based initialization is disabled.
        HasBeenBuilt = true;

        // Ensure core services are always registered
        WithCoreServices();

        // Apply all registrations against the current resolver source
        foreach (var registration in _registrations)
        {
            var targetResolver = _resolverProvider();
            registration(targetResolver);
        }

        Current = _serviceProvider();
        return this;
    }

    /// <summary>
    /// Retrieves the current build and builder usage state for test isolation. Used by test scopes.
    /// </summary>
    /// <returns>A tuple containing two Boolean values: <see langword="true"/> if the object has been built; otherwise, <see
    /// langword="false"/>. The second value is <see langword="true"/> if the builder is currently being used;
    /// otherwise, <see langword="false"/>.</returns>
    internal static (bool hasBeenBuilt, bool usingBuilder) GetState() =>
        (HasBeenBuilt, UsingBuilder);

    /// <summary>
    /// Restores the internal build state from the specified tuple. Use for test isolation. Used by test scopes.
    /// </summary>
    /// <param name="state">A tuple containing the values to restore for the build state. The first item indicates whether the build has
    /// been completed; the second item specifies whether the builder pattern is in use.</param>
    internal static void RestoreState((bool hasBeenBuilt, bool usingBuilder) state)
    {
        HasBeenBuilt = state.hasBeenBuilt;
        UsingBuilder = state.usingBuilder;
    }

    /// <summary>
    /// Resets the state to default for test isolation. Used by test scopes.
    /// </summary>
    /// <remarks>This method is intended for internal use to reinitialize static state. It should not be
    /// called from external code.</remarks>
    internal static void ResetState()
    {
        HasBeenBuilt = false;
        UsingBuilder = false;
    }
}
