// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Host
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a logger to the application.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="fullLogger"></param>
        /// <returns></returns>
        public static IApplicationBuilder WithLogging(this IApplicationBuilder hostBuilder, IFullLogger fullLogger)
        {
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

        /// <summary>
        /// Registers the platform.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="platformOperations">The platform operations.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder RegisterPlatform(
            this IApplicationBuilder builder,
            Func<IPlatformOperations> platformOperations)
        {
            builder.DependencyRegistrar.RegisterConstant(platformOperations, typeof(IPlatformOperations));
            return builder;
        }
    }
}
