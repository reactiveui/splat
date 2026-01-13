// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

using Splat.Ninject;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Ninject dependency resolver.
/// </summary>
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
