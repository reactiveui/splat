// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

namespace Splat.DryIoc;

/// <summary>
/// Extension methods for the DryIoc adapter.
/// </summary>
public static class SplatDryIocExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="DryIocDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="container">The container.</param>
    public static void UseDryIocDependencyResolver(this IContainer container) =>
        AppLocator.SetLocator(new DryIocDependencyResolver(container));
}
