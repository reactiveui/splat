// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

namespace Splat.DryIoc;

/// <summary>Provides extension methods for integrating DryIoc as the dependency resolver within Splat-based applications.</summary>
public static class SplatDryIocExtensions
{
    /// <summary>Extension methods for the DryIoc container.</summary>
    /// <param name="container">The container.</param>
    extension(IContainer container)
    {
        /// <summary>Initializes an instance of <see cref="DryIocDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.</summary>
        public void UseDryIocDependencyResolver() =>
            AppLocator.SetLocator(new DryIocDependencyResolver(container));
    }
}
