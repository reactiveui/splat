// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

using Splat.Ninject;

namespace Splat.Builder;

/// <summary>
/// Provides a Ninject-based implementation of the Splat dependency injection module for configuring dependency
/// resolution.
/// </summary>
/// <remarks>This module enables integration between Ninject and Splat by registering Ninject as the dependency
/// resolver. Use this module to allow Splat-based components to resolve dependencies from a Ninject
/// container.</remarks>
public sealed class NinjectSplatModule : IModule
{
    private readonly IKernel _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="NinjectSplatModule"/> class.
    /// </summary>
    /// <param name="kernel">The Ninject container.</param>
    public NinjectSplatModule(IKernel kernel)
    {
        ArgumentExceptionHelper.ThrowIfNull(kernel);
        _container = kernel;
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseNinjectDependencyResolver();
}
