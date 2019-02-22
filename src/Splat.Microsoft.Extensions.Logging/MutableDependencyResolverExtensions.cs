// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Splat.Microsoft.Extensions.Logging
{
    /// <summary>
    /// Microsoft.Extensions.Logging specific extensions for the Mutable Dependency Resolver.
    /// </summary>
    public static class MutableDependencyResolverExtensions
    {
        /// <summary>
        /// Simple helper to initialize Microsoft.Extensions.Logging within Splat with the Wrapping Full Logger.
        /// </summary>
        /// <remarks>
        /// You should configure Microsoft.Extensions.Logging prior to calling this method.
        /// </remarks>
        /// <param name="instance">
        /// An instance of Mutable Dependency Resolver.
        /// </param>
        /// <param name="loggerFactory">
        /// An instance of the Microsoft.Extensions.Logging Logger Factory.
        /// </param>
        /// <example>
        /// <code>
        /// Locator.CurrentMutable.UseMicrosoftExtensionsLoggingWithWrappingFullLogger();
        /// </code>
        /// </example>
        public static void UseMicrosoftExtensionsLoggingWithWrappingFullLogger(
            this IMutableDependencyResolver instance,
            ILoggerFactory loggerFactory)
        {
            var funcLogManager = new FuncLogManager(type =>
            {
                var actualLogger = loggerFactory.CreateLogger(type.ToString());
                var miniLoggingWrapper = new MicrosoftExtensionsLoggingLogger(actualLogger);
                return new WrappingFullLogger(miniLoggingWrapper);
            });

            instance.RegisterConstant(funcLogManager, typeof(ILogManager));
        }
    }
}
