// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Splat.Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the DryIoc adapter.
    /// </summary>
    public static class SplatMicrosoftExtensions
    {
        /// <summary>
        /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="Locator"/>.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
        public static void UseMicrosoftDependencyResolver(this IServiceCollection serviceCollection)
        {
            var resolver = new MicrosoftDependencyResolver(serviceCollection);
            Locator.SetLocator(resolver);
            serviceCollection.AddSingleton(resolver);
        }

        /// <summary>
        /// Initializes an instance of <see cref="MicrosoftDependencyResolver"/> that overrides the default <see cref="Locator"/>
        /// with a built <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public static void UseMicrosoftDependencyResolver(this IServiceProvider serviceProvider)
        {
            var resolver = serviceProvider.GetService<MicrosoftDependencyResolver>();
            if (resolver != null)
            {
                resolver.UpdateServiceProvider(serviceProvider);
            }
            else
            {
                resolver = new MicrosoftDependencyResolver(serviceProvider);
                Locator.SetLocator(resolver);
            }
        }
    }
}
