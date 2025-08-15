// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;
using Splat;
using Splat.Autofac;

namespace ReactiveUI.Builder;

/// <summary>
/// Splat module for configuring the Autofac dependency resolver.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AutofacSplatModule"/> class.
/// </remarks>
/// <param name="builder">The Autofac container builder.</param>
public sealed class AutofacSplatModule(ContainerBuilder builder) : IReactiveUIModule
{
    private readonly ContainerBuilder _builder = builder ?? throw new ArgumentNullException(nameof(builder));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        var autofacResolver = _builder.UseAutofacDependencyResolver();

        // Also register the resolver instance for later retrieval if the container is built after
        _builder.RegisterInstance(autofacResolver);
    }
}
