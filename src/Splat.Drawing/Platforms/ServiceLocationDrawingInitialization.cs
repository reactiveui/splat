// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat;

/// <summary>
/// Provides service location for the Splat.Drawing packages.
/// </summary>
public static class ServiceLocationDrawingInitialization
{
    /// <summary>
    /// Registers the platform bitmap loader for the current platform.
    /// </summary>
    /// <param name="resolver">The resolver to register against.</param>
    public static void RegisterPlatformBitmapLoader(this IMutableDependencyResolver resolver)
    {
        resolver.ThrowArgumentNullExceptionIfNull(nameof(resolver));

#if !IS_SHARED_NET
        // not supported in netstandard or NET6 library
        if (!resolver.HasRegistration(typeof(IBitmapLoader)))
        {
            resolver.RegisterLazySingleton(static () => new PlatformBitmapLoader(), typeof(IBitmapLoader));
        }
#endif
    }
}
