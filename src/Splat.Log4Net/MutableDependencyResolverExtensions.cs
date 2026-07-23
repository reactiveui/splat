// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Log4Net;

/// <summary>Provides extension methods for registering Log4Net integration with Splat using the wrapping full logger pattern.</summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>Extension members for <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="instance">The mutable dependency resolver the extension members operate on.</param>
    extension(IMutableDependencyResolver instance)
    {
        /// <summary>Initializes Log4Net integration with Splat using the wrapping full logger pattern.</summary>
        /// <remarks>
        /// Configure Log4Net appenders and configuration before calling this method.
        /// </remarks>
        /// <example>
        /// <c>AppLocator.CurrentMutable.UseLog4NetWithWrappingFullLogger();</c>
        /// </example>
        public void UseLog4NetWithWrappingFullLogger()
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);
            var funcLogManager = new FuncLogManager(static type => new WrappingFullLogger(new Log4NetLogger(LogResolver.Resolve(type))));

            instance.RegisterConstant<ILogManager>(funcLogManager);
        }
    }
}
