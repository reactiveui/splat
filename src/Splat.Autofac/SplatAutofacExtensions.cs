// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

namespace Splat.Autofac;

/// <summary>
/// Provides extension methods for integrating Autofac with Splat's dependency resolution system.
/// </summary>
public static class SplatAutofacExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="AutofacDependencyResolver"/> that overrides the default <see cref="AppLocator"/>.
    /// </summary>
    /// <param name="builder">Autofac container builder.</param>
    /// <returns>The Autofac dependency resolver.</returns>
    public static AutofacDependencyResolver UseAutofacDependencyResolver(this ContainerBuilder builder)
    {
        var autofacResolver = new AutofacDependencyResolver(builder);
        AppLocator.SetLocator(autofacResolver);
        return autofacResolver;
    }
}
