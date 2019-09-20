// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Autofac;

namespace Splat.Autofac
{
    /// <summary>
    /// Extension methods for the Autofac adapter.
    /// </summary>
    public static class SplatAutofacExtensions
    {
        /// <summary>
        /// Initializes an instance of <see cref="AutofacDependencyResolver"/> that overrides the default <see cref="Locator"/>.
        /// </summary>
        /// <param name="componentContext">Autofac component context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose handled by locator.")]
        public static void UseAutofacDependencyResolver(this IComponentContext componentContext) =>
            Locator.SetLocator(new AutofacDependencyResolver(componentContext));

        /// <summary>
        /// Initializes an instance of <see cref="AutofacDependencyResolver"/> that overrides the default <see cref="Locator"/>.
        /// </summary>
        /// <param name="containerBuilder">Autofac container builder.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose handled by locator.")]
        public static void UseAutofacDependencyResolver(this ContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
            {
                throw new ArgumentNullException(nameof(containerBuilder));
            }

            Locator.SetLocator(new AutofacDependencyResolver(containerBuilder.Build()));
        }
    }
}
