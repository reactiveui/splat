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
    /// Runs the provided configuration action imediately against the current Splat Locator.
    /// </summary>
    /// <param name="module">The module to configure.</param>
    public static void Apply(this IModule module)
    {
        if (module is null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        module.Configure(AppLocator.CurrentMutable);
    }

    /// <summary>
    /// Creates the default.
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <returns>The builder instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">resolver.</exception>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver)
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver);
    }

    /// <summary>
    /// Creates the default.
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <param name="configureAction">The configuration action to apply.</param>
    /// <returns>The builder instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">resolver.</exception>
    public static IAppBuilder CreateSplatBuilder(this IMutableDependencyResolver resolver, Action<IMutableDependencyResolver> configureAction)
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        // Create a new AppBuilder instance with the provided resolver
        return new AppBuilder(resolver).WithCustomRegistration(configureAction);
    }
}
