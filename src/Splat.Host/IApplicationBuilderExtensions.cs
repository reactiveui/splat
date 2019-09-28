// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Host
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Withes the logging.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="fullLogger">The full logger.</param>
        /// <returns>The builder.</returns
        public static IApplicationBuilder WithLogging(this IApplicationBuilder hostBuilder, IFullLogger fullLogger)
        {
            Locator.CurrentMutable.Register<IFullLogger>(() => fullLogger);
            return hostBuilder;
        }

        /// <summary>
        /// Sets the dependency resolver.
        /// </summary>
        /// <param name="hostBuilder">The application builder.</param>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The builder.</returns>
        public static IApplicationBuilder WithDependencyResolver(this IApplicationBuilder hostBuilder, IDependencyResolver dependencyResolver)
        {
            Locator.SetLocator(dependencyResolver);
            return hostBuilder;
        }
    }
}
