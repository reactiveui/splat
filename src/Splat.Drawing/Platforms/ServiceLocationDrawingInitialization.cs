// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Provides methods for initializing platform-specific bitmap loader services for service location.
/// </summary>
public static class ServiceLocationDrawingInitialization
{
    /// <summary>
    /// Registers the platform bitmap loader for the current platform.
    /// </summary>
    /// <param name="resolver">The resolver to register against.</param>
#if !IS_SHARED_NET && !NET462_OR_GREATER
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Needs to use reflection")]
#endif
    public static void RegisterPlatformBitmapLoader(this IMutableDependencyResolver resolver)
    {
        ArgumentExceptionHelper.ThrowIfNull(resolver);

#if !IS_SHARED_NET
        // not supported in NET6+ library
        if (!resolver.HasRegistration<IBitmapLoader>())
        {
            resolver.RegisterLazySingleton(static () => new PlatformBitmapLoader(), typeof(IBitmapLoader));
        }
#endif
    }
}
