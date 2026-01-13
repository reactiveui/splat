// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

using Splat.DryIoc;

namespace Splat.Builder;

/// <summary>
/// Provides a module for integrating DryIoc as the dependency injection container within Splat-based applications.
/// </summary>
/// <remarks>This module enables the use of DryIoc for resolving dependencies in applications that utilize the
/// Splat library. It is typically registered with a dependency resolver during application startup to configure DryIoc
/// as the underlying container.</remarks>
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
