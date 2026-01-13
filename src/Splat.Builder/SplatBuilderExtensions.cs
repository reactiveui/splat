// Copyright (c) 2026 ReactiveUI. All rights reserved.
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
    /// Runs the provided configuration action immediately against the current Splat AppLocator.
    /// </summary>
    /// <param name="module">The module to configure.</param>
    public static void Apply(this IModule module)
    {
        ArgumentExceptionHelper.ThrowIfNull(module);

        module.Configure(AppLocator.CurrentMutable);
    }

    /// <summary>
    /// Creates a Splat application builder using the specified dependency resolver.
    /// </summary>
    /// <param name="resolver">The mutable dependency resolver to use.</param>
    /// <returns>The builder instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when resolver is null.</exception>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver);
    }

    /// <summary>
    /// Creates a Splat application builder using the specified dependency resolver and applies a configuration action.
    /// </summary>
    /// <param name="resolver">The mutable dependency resolver to use.</param>
    /// <param name="configureAction">The configuration action to apply.</param>
    /// <returns>The builder instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when resolver is null.</exception>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver, Action<IMutableDependencyResolver> configureAction)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver).WithCustomRegistration(configureAction);
    }
}
