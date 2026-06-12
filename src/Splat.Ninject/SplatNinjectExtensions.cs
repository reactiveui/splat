// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Ninject;

namespace Splat.Ninject;

/// <summary>Provides extension methods for integrating Ninject with Splat by configuring the dependency resolver.</summary>
/// <remarks>This class enables the use of Ninject as the dependency resolver within Splat-based applications. It
/// is intended to be used in application startup code to replace the default dependency resolution mechanism with one
/// backed by a Ninject kernel.</remarks>
public static class SplatNinjectExtensions
{
    /// <summary>Extension members for <see cref="IKernel"/>.</summary>
    /// <param name="kernel">The Ninject kernel the extension members operate on.</param>
    extension(IKernel kernel)
    {
        /// <summary>Initializes an instance of <see cref="NinjectDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.</summary>
        public void UseNinjectDependencyResolver() =>
            AppLocator.SetLocator(new NinjectDependencyResolver(kernel));
    }
}
