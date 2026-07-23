// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.NLog;

/// <summary>Provides extension methods for registering NLog integration with Splat using a mutable dependency resolver.</summary>
public static class MutableDependencyResolverExtensions
{
    /// <summary>Extension members for <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="instance">The mutable dependency resolver the extension members operate on.</param>
    extension(IMutableDependencyResolver instance)
    {
        /// <summary>Initializes NLog integration with Splat using the wrapping full logger pattern.</summary>
        /// <remarks>
        /// Configure NLog targets and rules before calling this method.
        /// </remarks>
        /// <example>
        /// <c>AppLocator.CurrentMutable.UseNLogWithWrappingFullLogger();</c>
        /// </example>
        public void UseNLogWithWrappingFullLogger()
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);

            var funcLogManager = new FuncLogManager(static type => new NLogLogger(LogResolver.Resolve(type)));

            instance.Register<ILogManager>(() => funcLogManager);
        }
    }
}
