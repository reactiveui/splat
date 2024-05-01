// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DryIoc;

namespace Splat.DryIoc;

/// <summary>
/// Extension methods for the DryIoc adapter.
/// </summary>
public static class SplatDryIocExtensions
{
    /// <summary>
    /// Initializes an instance of <see cref="DryIocDependencyResolver"/> that overrides the default <see cref="Locator"/>.
    /// </summary>
    /// <param name="container">The container.</param>
#pragma warning disable CA2000 // Dispose objects before losing scope
    public static void UseDryIocDependencyResolver(this IContainer container) =>
        Locator.SetLocator(new DryIocDependencyResolver(container));
#pragma warning restore CA2000 // Dispose objects before losing scope
}
