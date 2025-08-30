// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

namespace Splat.Ninject;

/// <summary>
/// Extension methods for the Ninject adapter.
/// </summary>
public static class SplatNinjectExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="NinjectDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    public static void UseNinjectDependencyResolver(this IKernel kernel) =>
        AppLocator.SetLocator(new NinjectDependencyResolver(kernel));
}
