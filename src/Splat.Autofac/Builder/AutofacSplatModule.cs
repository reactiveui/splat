// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

using Splat.Autofac;

namespace Splat.Builder;

/// <summary>
/// Provides an Autofac module that integrates Autofac with Splat's dependency resolver system.
/// </summary>
/// <remarks>This module enables the use of Autofac as the dependency resolver for Splat-based applications. It is
/// typically used to configure dependency injection in applications that leverage both Autofac and Splat. The module
/// should be initialized with a valid Autofac ContainerBuilder instance before configuring the resolver.</remarks>
public sealed class AutofacSplatModule : IModule
{
    private readonly ContainerBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutofacSplatModule"/> class.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    public AutofacSplatModule(ContainerBuilder builder)
    {
        ArgumentExceptionHelper.ThrowIfNull(builder);
        _builder = builder;
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver)
    {
        var autofacResolver = _builder.UseAutofacDependencyResolver();

        // Also register the resolver instance for later retrieval if the container is built after
        _builder.RegisterInstance(autofacResolver);
    }
}
