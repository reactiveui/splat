// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="MicrosoftDependencyResolver"/>.
/// </summary>
public static class SplatMicrosoftExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    public static void UseMicrosoftDependencyResolver(this IServiceCollection serviceCollection) =>

        // Will be disposed with the InternalLocator
        AppLocator.SetLocator(new MicrosoftDependencyResolver(serviceCollection));

    /// <summary>
    /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="AppLocator"/>
    /// with a built <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// If there is already a <see cref="MicrosoftDependencyResolver"/> serving as the
    /// <see cref="AppLocator.Current"/>, it'll instead update it to use the specified
    /// <paramref name="serviceProvider"/>.
    /// </remarks>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
    public static void UseMicrosoftDependencyResolver(this IServiceProvider serviceProvider)
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
