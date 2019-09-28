// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Splat.Host
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostbuilder?view=aspnetcore-2.2 .
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets a central location for sharing state between components during the host building process.
        /// </summary>
        IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IHostEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        IApplicationBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// Configures the application configuration.
        /// </summary>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <returns>The application builder.</returns>
        IApplicationBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configurationBuilder);

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="containerRegistry">The container registry.</param>
        /// <returns>The application builder.</returns>
        IApplicationBuilder ConfigureContainer(IDependencyResolver containerRegistry); // TODO: make this container agnostic?

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>The application builder.</returns>
        IApplicationBuilder ConfigureServices(Action<IDependencyResolver> serviceCollection);

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>The application instance.</returns>
        IApplication Build();
    }
}
