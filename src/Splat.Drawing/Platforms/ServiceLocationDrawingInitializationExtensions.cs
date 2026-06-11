// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>Provides methods for initializing platform-specific bitmap loader services for service location.</summary>
public static class ServiceLocationDrawingInitializationExtensions
{
    /// <summary>Extension members for registering platform-specific drawing services on a resolver.</summary>
    /// <param name="resolver">The resolver the extension members operate on.</param>
    extension(IMutableDependencyResolver resolver)
    {
        /// <summary>Registers the platform bitmap loader for the current platform.</summary>
#if !IS_SHARED_NET && !NET462_OR_GREATER
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Needs to use reflection")]
#endif
        public void RegisterPlatformBitmapLoader()
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);

#if !IS_SHARED_NET
            // not supported in NET6+ library
            if (resolver.HasRegistration<IBitmapLoader>())
            {
                return;
            }

            resolver.RegisterLazySingleton(static () => new PlatformBitmapLoader(), typeof(IBitmapLoader));
#endif
        }
    }
}
