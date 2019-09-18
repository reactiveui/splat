// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using ReactiveUI;
using Splat;

namespace Splat
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder WithLogging(this IApplicationBuilder hostBuilder, IFullLogger fullLogger)
        {
            return hostBuilder;
        }

        public static IApplicationBuilder WithDependencyResolver(this IApplicationBuilder hostBuilder, IDependencyResolver dependencyResolver)
        {
            hostBuilder.SetDependencyRegistrar(dependencyResolver);
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
