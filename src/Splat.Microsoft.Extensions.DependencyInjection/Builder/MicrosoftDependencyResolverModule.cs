// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Microsoft dependency resolver.
/// </summary>
/// <seealso cref="IModule" />
public sealed class MicrosoftDependencyResolverModule(IServiceCollection serviceCollection) : IModule
{
    private readonly IServiceCollection _container = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseMicrosoftDependencyResolver();
}
