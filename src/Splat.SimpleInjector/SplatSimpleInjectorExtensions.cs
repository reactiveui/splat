// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>
/// Provides extension methods for integrating Simple Injector with Splat's dependency resolution system.
/// </summary>
public static class SplatSimpleInjectorExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="SimpleInjectorDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="container">Simple Injector container.</param>
    /// <param name="initializer">Initializer.</param>
    public static void UseSimpleInjectorDependencyResolver(this Container container, SimpleInjectorInitializer initializer) =>
        AppLocator.SetLocator(new SimpleInjectorDependencyResolver(container, initializer));
}
