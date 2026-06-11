// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging;

/// <summary>Provides extension methods for integrating Splat logging with Microsoft.Extensions.Logging.</summary>
public static class MicrosoftExtensionsLoggingExtensions
{
    /// <summary>Extension methods for the <see cref="IMutableDependencyResolver"/>.</summary>
    /// <param name="instance">
    /// The mutable dependency resolver to register Microsoft.Extensions.Logging with.
    /// </param>
    extension(IMutableDependencyResolver instance)
    {
        /// <summary>Initializes Microsoft.Extensions.Logging integration with Splat using the wrapping full logger pattern.</summary>
        /// <remarks>
        /// Configure the logger factory providers before calling this method.
        /// </remarks>
        /// <param name="loggerFactory">
        /// The configured Microsoft.Extensions.Logging logger factory.
        /// </param>
        /// <example>
        /// <code>
        /// AppLocator.CurrentMutable.UseMicrosoftExtensionsLoggingWithWrappingFullLogger();
        /// </code>
        /// </example>
        public void UseMicrosoftExtensionsLoggingWithWrappingFullLogger(ILoggerFactory loggerFactory)
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);

            var funcLogManager = new FuncLogManager(type =>
            {
                var actualLogger = loggerFactory.CreateLogger(type.ToString());
                var miniLoggingWrapper = new MicrosoftExtensionsLoggingLogger(actualLogger);
                return new WrappingFullLogger(miniLoggingWrapper);
            });

            instance.Register<ILogManager>(() => funcLogManager);
        }
    }

    /// <summary>Provides extension methods for integrating Splat logging with Microsoft.Extensions.Logging.</summary>
    /// <param name="builder">The logging builder to configure.</param>
    extension(ILoggingBuilder builder)
    {
        /// <summary>Registers Splat as a logging provider with Microsoft.Extensions.Logging.</summary>
        /// <returns>The logging builder for chaining.</returns>
        public ILoggingBuilder AddSplat()
        {
            ArgumentExceptionHelper.ThrowIfNull(builder);

            builder.Services.AddSingleton<ILoggerProvider, MicrosoftExtensionsLogProvider>();

            return builder;
        }
    }

    /// <summary>Provides extension methods for integrating Splat logging with Microsoft.Extensions.Logging.</summary>
    /// <param name="loggerFactory">The logger factory to configure.</param>
    extension(ILoggerFactory loggerFactory)
    {
        /// <summary>Adds Splat as a logging provider to the logger factory.</summary>
        /// <returns>The logger factory for chaining.</returns>
        public ILoggerFactory AddSplat()
        {
            ArgumentExceptionHelper.ThrowIfNull(loggerFactory);

            loggerFactory.AddProvider(new MicrosoftExtensionsLogProvider());
            return loggerFactory;
        }
    }
}
