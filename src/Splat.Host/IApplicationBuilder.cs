// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;
using Splat;

namespace Splat
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostbuilder?view=aspnetcore-2.2
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets the dependency registrar.
        /// </summary>
        /// <value>The dependency registrar.</value>
        IDependencyResolver DependencyRegistrar { get; }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>The application instance.</returns>
        IApplication Build();

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
        IApplicationBuilder ConfigureContainer(IMutableDependencyResolver containerRegistry); // TODO: make this container agnostic?

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>The application builder.</returns>
        IApplicationBuilder ConfigureServices(Action<IDependencyResolver> serviceCollection);
    }
}
