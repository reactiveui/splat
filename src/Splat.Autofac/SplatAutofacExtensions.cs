// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

namespace Splat.Autofac;

/// <summary>Provides extension methods for integrating Autofac with Splat's dependency resolution system.</summary>
public static class SplatAutofacExtensions
{
    /// <summary>Extension members for <see cref="ContainerBuilder"/>.</summary>
    /// <param name="builder">The Autofac container builder the extension members operate on.</param>
    extension(ContainerBuilder builder)
    {
        /// <summary>Initializes an instance of <see cref="AutofacDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.</summary>
        /// <returns>The Autofac dependency resolver.</returns>
        public AutofacDependencyResolver UseAutofacDependencyResolver()
        {
            var autofacResolver = new AutofacDependencyResolver(builder);
            AppLocator.SetLocator(autofacResolver);
            return autofacResolver;
        }
    }
}
