// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

using Splat.DryIoc;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the DryIoc dependency resolver.
/// </summary>
public sealed class DryIocSplatModule : IModule
{
    private readonly IContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocSplatModule"/> class.
    /// </summary>
    /// <param name="container">The DryIoc container.</param>
    public DryIocSplatModule(IContainer container)
    {
        ArgumentExceptionHelper.ThrowIfNull(container);
        _container = container;
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseDryIocDependencyResolver();
}
