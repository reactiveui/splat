// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;
using Splat.SimpleInjector;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the SimpleInjector dependency resolver.
/// </summary>
/// <seealso cref="IModule" />
/// <remarks>
/// Initializes a new instance of the <see cref="SimpleInjectorSplatModule" /> class.
/// </remarks>
/// <param name="container">The SimpleInjector container.</param>
/// <param name="initializer">The SimpleInjector Initializer.</param>
public sealed class SimpleInjectorSplatModule(Container container, SimpleInjectorInitializer initializer) : IModule
{
    private readonly Container _container = container ?? throw new ArgumentNullException(nameof(container));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseSimpleInjectorDependencyResolver(initializer);
}
