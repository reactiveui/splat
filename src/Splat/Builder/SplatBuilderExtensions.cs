// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Splat.Builder;

/// <summary>
/// Common extension helpers for registering Splat modules.
/// </summary>
public static class SplatBuilderExtensions
{
    /// <summary>
    /// Runs the provided configuration action imediately against the current Splat Locator.
    /// </summary>
    /// <param name="module">The module to configure.</param>
    public static void Apply(this IReactiveUIModule module)
    {
        if (module is null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        module.Configure(Locator.CurrentMutable);
    }
}
