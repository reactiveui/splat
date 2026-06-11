// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder;

/// <summary>Common extension helpers for registering Splat modules.</summary>
public static class SplatBuilderExtensions
{
    /// <summary>Common extension helpers for configuring Splat modules.</summary>
    /// <param name="module">The module to configure. Cannot be null.</param>
    extension(IModule module)
    {
        /// <summary>
        /// Runs the provided configuration action immediately and configures the specified module using the current mutable application locator.
        /// </summary>
        public void Apply()
        {
            ArgumentExceptionHelper.ThrowIfNull(module);

            module.Configure(AppLocator.CurrentMutable);
        }
    }

    /// <summary>Common extension helpers for creating Splat application builders.</summary>
    /// <param name="resolver">The dependency resolver to use for resolving services and dependencies. Cannot be null.</param>
    extension(IMutableDependencyResolver resolver)
    {
        /// <summary>Creates a new application builder that uses the specified dependency resolver.</summary>
        /// <returns>An <see cref="IAppBuilder"/> instance configured with the specified dependency resolver.</returns>
        public IAppBuilder CreateSplatBuilder()
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);

            // Create a new AppBuilder instance with the provided resolver
            return new AppBuilder(resolver);
        }

        /// <summary>Creates a new application builder using the specified dependency resolver and applies custom configuration.</summary>
        /// <param name="configureAction">An action that configures additional registrations on the dependency resolver. Can be null if no custom
        /// configuration is required.</param>
        /// <returns>An <see cref="IAppBuilder"/> instance configured with the specified resolver and custom registrations.</returns>
        public IAppBuilder CreateSplatBuilder(Action<IMutableDependencyResolver> configureAction)
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);

            // Create a new AppBuilder instance with the provided resolver
            return new AppBuilder(resolver).WithCustomRegistration(configureAction);
        }
    }
}
