// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using SimpleInjector;

namespace Splat.SimpleInjector;

/// <summary>Provides extension methods for integrating Simple Injector with Splat's dependency resolution system.</summary>
public static class SplatSimpleInjectorExtensions
{
    /// <summary>Extension members for <see cref="Container"/>.</summary>
    /// <param name="container">The Simple Injector container the extension members operate on.</param>
    extension(Container container)
    {
        /// <summary>Initializes an instance of <see cref="SimpleInjectorDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.</summary>
        /// <param name="initializer">Initializer.</param>
        public void UseSimpleInjectorDependencyResolver(SimpleInjectorInitializer initializer) =>
            AppLocator.SetLocator(new SimpleInjectorDependencyResolver(container, initializer));
    }
}
