// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>
/// Common extension helpers for registering Splat modules.
/// </summary>
public static class SplatBuilderExtensions
{
    /// <summary>
    /// Runs the provided configuration action immediately and configures the specified module using the current mutable application locator.
    /// </summary>
    /// <param name="module">The module to configure. Cannot be null.</param>
    public static void Apply(this IModule module)
    {
        ArgumentExceptionHelper.ThrowIfNull(module);

        module.Configure(AppLocator.CurrentMutable);
    }

    /// <summary>
    /// Creates a new application builder that uses the specified dependency resolver.
    /// </summary>
    /// <param name="resolver">The dependency resolver to use for resolving services and dependencies. Cannot be null.</param>
    /// <returns>An <see cref="IAppBuilder"/> instance configured with the specified dependency resolver.</returns>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver);
    }

    /// <summary>
    /// Creates a new application builder using the specified dependency resolver and applies custom configuration.
    /// </summary>
    /// <param name="resolver">The dependency resolver to use for service registration and resolution. Cannot be null.</param>
    /// <param name="configureAction">An action that configures additional registrations on the dependency resolver. Can be null if no custom
    /// configuration is required.</param>
    /// <returns>An <see cref="IAppBuilder"/> instance configured with the specified resolver and custom registrations.</returns>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver, Action<IMutableDependencyResolver> configureAction)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver).WithCustomRegistration(configureAction);
    }
}
