// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="MicrosoftDependencyResolver"/>.
/// </summary>
public static class SplatMicrosoftExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="Locator"/>.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    public static void UseMicrosoftDependencyResolver(this IServiceCollection serviceCollection) =>
        Locator.SetLocator(new MicrosoftDependencyResolver(serviceCollection));

    /// <summary>
    /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="Locator"/>
    /// with a built <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// If there is already a <see cref="MicrosoftDependencyResolver"/> serving as the
    /// <see cref="Locator.Current"/>, it'll instead update it to use the specified
    /// <paramref name="serviceProvider"/>.
    /// </remarks>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
    public static void UseMicrosoftDependencyResolver(this IServiceProvider serviceProvider)
    {
        if (Locator.Current is MicrosoftDependencyResolver resolver)
        {
            resolver.UpdateContainer(serviceProvider);
        }
        else
        {
            Locator.SetLocator(new MicrosoftDependencyResolver(serviceProvider));
        }
    }
}
