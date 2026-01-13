// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

using Splat.SimpleInjector;

namespace Splat.Builder;

/// <summary>
/// Provides a module for integrating SimpleInjector with Splat's dependency resolver infrastructure.
/// </summary>
/// <remarks>This module enables the use of a SimpleInjector container as the backing implementation for Splat's
/// dependency resolution. It is typically used to configure dependency injection in applications that leverage both
/// SimpleInjector and Splat. Thread safety and container lifetime management are determined by the provided
/// SimpleInjector container.</remarks>
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
