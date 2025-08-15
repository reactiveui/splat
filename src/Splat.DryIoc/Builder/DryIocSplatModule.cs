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
/// <remarks>
/// Initializes a new instance of the <see cref="DryIocSplatModule"/> class.
/// </remarks>
/// <param name="container">The DryIoc container.</param>
public sealed class DryIocSplatModule(IContainer container) : IReactiveUIModule
{
    private readonly IContainer _container = container ?? throw new ArgumentNullException(nameof(container));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseDryIocDependencyResolver();
}
