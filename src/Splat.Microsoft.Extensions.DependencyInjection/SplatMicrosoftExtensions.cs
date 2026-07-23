// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for integrating the Microsoft dependency injection system with Splat by configuring the
/// application-wide dependency resolver.
/// </summary>
/// <remarks>These methods allow you to set or update the global dependency resolver used by Splat to leverage
/// Microsoft.Extensions.DependencyInjection. Use these extensions to enable Splat services to be resolved from your
/// application's IServiceCollection or IServiceProvider. Only one resolver can be active at a time; calling these
/// methods will replace any existing resolver.</remarks>
public static class SplatMicrosoftExtensions
{
    /// <summary>Extension members for <see cref="IServiceCollection"/>.</summary>
    /// <param name="serviceCollection">The service collection the extension members operate on.</param>
    extension(IServiceCollection serviceCollection)
    {
        /// <summary>Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.</summary>
        public void UseMicrosoftDependencyResolver() =>

            // Will be disposed with the InternalLocator
            AppLocator.SetLocator(new MicrosoftDependencyResolver(serviceCollection));
    }

    /// <summary>Extension members for <see cref="IServiceProvider"/>.</summary>
    /// <param name="serviceProvider">The service provider the extension members operate on.</param>
    extension(IServiceProvider serviceProvider)
    {
        /// <summary>Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="AppLocator"/> with a built <see cref="IServiceProvider"/>.</summary>
        /// <remarks>
        /// If there is already a <see cref="MicrosoftDependencyResolver"/> serving as the
        /// <see cref="AppLocator.Current"/>, it'll instead update it to use the specified
        /// service provider.
        /// </remarks>
        public void UseMicrosoftDependencyResolver()
        {
            if (AppLocator.Current is MicrosoftDependencyResolver resolver)
            {
                resolver.UpdateContainer(serviceProvider);
            }
            else
            {
                // Will be disposed with the InternalLocator
                AppLocator.SetLocator(new MicrosoftDependencyResolver(serviceProvider));
            }
        }
    }
}
