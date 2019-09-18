// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;

namespace ReactiveUI.HostBuilder.Splat
{
    /// <summary>
    /// Application startup functions.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void Configure(IApplicationBuilder builder);

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        void ConfigureServices(IMutableDependencyResolver dependencyResolver);
    }
}
