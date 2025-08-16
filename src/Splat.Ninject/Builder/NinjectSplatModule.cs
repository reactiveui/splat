// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;
using Splat.Ninject;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Ninject dependency resolver.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NinjectSplatModule"/> class.
/// </remarks>
/// <param name="kernel">The Ninject container.</param>
public sealed class NinjectSplatModule(IKernel kernel) : IModule
{
    private readonly IKernel _container = kernel ?? throw new ArgumentNullException(nameof(kernel));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseNinjectDependencyResolver();
}
