// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

namespace Splat.Ninject;

/// <summary>
/// Provides extension methods for integrating Ninject with Splat by configuring the dependency resolver.
/// </summary>
/// <remarks>This class enables the use of Ninject as the dependency resolver within Splat-based applications. It
/// is intended to be used in application startup code to replace the default dependency resolution mechanism with one
/// backed by a Ninject kernel.</remarks>
public static class SplatNinjectExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="NinjectDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    public static void UseNinjectDependencyResolver(this IKernel kernel) =>
        AppLocator.SetLocator(new NinjectDependencyResolver(kernel));
}
