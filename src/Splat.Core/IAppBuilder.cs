// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Defines a builder interface for configuring and constructing application instances with customizable modules,
/// services, and dependency registrations.
/// </summary>
/// <remarks>The IAppBuilder interface enables fluent configuration of application components and services before
/// creating an application instance. Implementations typically support method chaining, allowing multiple configuration
/// steps to be composed in a single statement. This interface is intended for use in application startup code to
/// register modules, configure dependency resolution, and set up core or custom services prior to building the final
/// application instance.</remarks>
public interface IAppBuilder
{
    /// <summary>
    /// Builds and returns a configured application instance.
    /// </summary>
    /// <returns>An <see cref="IAppInstance"/> representing the configured application. The returned instance is ready for use
    /// according to the builder's configuration.</returns>
    IAppInstance Build();

    /// <summary>
    /// Configures the application to use the current Splat service locator for dependency resolution within the OWIN
    /// pipeline.
    /// </summary>
    /// <remarks>Call this method to ensure that components relying on Splat's service locator can resolve
    /// dependencies during OWIN middleware execution. This is typically used when integrating Splat-based services into
    /// an OWIN application.</remarks>
    /// <returns>The <see cref="IAppBuilder"/> instance, enabling further configuration of the OWIN pipeline.</returns>
    IAppBuilder UseCurrentSplatLocator();

    /// <summary>
    /// Registers the specified module with the application builder, enabling its services and configuration within the
    /// application pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the module to register. Must implement the IModule interface.</typeparam>
    /// <param name="registrationModule">The module instance to register with the application builder. Cannot be null.</param>
    /// <returns>The current IAppBuilder instance, enabling method chaining.</returns>
    IAppBuilder UsingModule<T>(T registrationModule)
        where T : IModule;

    /// <summary>
    /// Adds the core framework services to the application builder.
    /// </summary>
    /// <returns>The current <see cref="IAppBuilder"/> instance with core services registered. This enables further configuration
    /// of the application pipeline.</returns>
    IAppBuilder WithCoreServices();

    /// <summary>
    /// Configures the dependency resolver with custom registrations using the specified action.
    /// </summary>
    /// <remarks>Use this method to add or override service registrations in the application's dependency
    /// resolver before the application is built. This is typically used to register custom services or replace default
    /// implementations.</remarks>
    /// <param name="configureAction">An action that receives the mutable dependency resolver to which custom registrations can be added. Cannot be
    /// null.</param>
    /// <returns>The current <see cref="IAppBuilder"/> instance, enabling method chaining.</returns>
    IAppBuilder WithCustomRegistration(Action<IMutableDependencyResolver> configureAction);
}
