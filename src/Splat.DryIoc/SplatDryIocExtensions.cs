// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using DryIoc;

namespace Splat.DryIoc
{
    /// <summary>
    /// Extension methods for the DryIoc adapter.
    /// </summary>
    public static class SplatDryIocExtensions
    {
        /// <summary>
        /// Initializes an instance of <see cref="DryIocDependencyResolver"/> that overrides the default <see cref="Locator"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose handled by locator.")]
        public static void UseDryIocDependencyResolver(this IContainer container) =>
            Locator.SetLocator(new DryIocDependencyResolver(container));
    }
}
