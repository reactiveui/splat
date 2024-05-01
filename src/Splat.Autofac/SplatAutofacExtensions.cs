// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

namespace Splat.Autofac;

/// <summary>
/// Extension methods for the Autofac adapter.
/// </summary>
public static class SplatAutofacExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="AutofacDependencyResolver"/> that overrides the default <see cref="Locator"/>.
    /// </summary>
    /// <param name="builder">Autofac container builder.</param>
    /// <returns>The Autofac dependency resolver.</returns>
    public static AutofacDependencyResolver UseAutofacDependencyResolver(this ContainerBuilder builder)
    {
        var autofacResolver = new AutofacDependencyResolver(builder);
        Locator.SetLocator(autofacResolver);
        return autofacResolver;
    }
}
