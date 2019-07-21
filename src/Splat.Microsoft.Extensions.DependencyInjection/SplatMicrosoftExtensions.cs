// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

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
        /// <param name="services">The container.</param>
        public static void UseMicrosoftDependencyResolver(this IServiceCollection services) =>
            Locator.SetLocator(new MicrosoftDependencyResolver(services));
    }
}
