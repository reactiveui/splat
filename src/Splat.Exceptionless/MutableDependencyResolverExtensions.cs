// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

namespace Splat.Exceptionless;

/// <summary>Provides extension methods for registering Exceptionless integration with Splat using a wrapping full logger pattern.</summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>Extension methods for the <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="instance">The mutable dependency resolver to register Exceptionless with.</param>
    extension(IMutableDependencyResolver instance)
    {
        /// <summary>Initializes Exceptionless integration with Splat using the wrapping full logger pattern.</summary>
        /// <remarks>
        /// Configure Exceptionless client before calling this method.
        /// </remarks>
        /// <param name="exceptionlessClient">The configured Exceptionless client instance.</param>
        /// <example>
        /// <c>AppLocator.CurrentMutable.UseExceptionlessWithWrappingFullLogger(exception);</c>
        /// </example>
        public void UseExceptionlessWithWrappingFullLogger(ExceptionlessClient exceptionlessClient)
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);

            var funcLogManager = new FuncLogManager(type =>
            {
                var miniLoggingWrapper = new ExceptionlessSplatLogger(type, exceptionlessClient);
                return new WrappingFullLogger(miniLoggingWrapper);
            });

            instance.RegisterConstant<ILogManager>(funcLogManager);
        }
    }
}
