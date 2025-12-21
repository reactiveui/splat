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
public sealed class SimpleInjectorSplatModule : IModule
{
    private readonly Container _container;
    private readonly SimpleInjectorInitializer _initializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleInjectorSplatModule" /> class.
    /// </summary>
    /// <param name="container">The SimpleInjector container.</param>
    /// <param name="initializer">The SimpleInjector Initializer.</param>
    public SimpleInjectorSplatModule(Container container, SimpleInjectorInitializer initializer)
    {
        ArgumentExceptionHelper.ThrowIfNull(container);
        ArgumentExceptionHelper.ThrowIfNull(initializer);
        _container = container;
        _initializer = initializer;
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => _container.UseSimpleInjectorDependencyResolver(_initializer);
}
