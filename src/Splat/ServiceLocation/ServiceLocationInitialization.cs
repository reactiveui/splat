// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat
{
    /// <summary>
    /// Initialization logic for Splat interacting with Dependency Resolvers.
    /// </summary>
    public static class ServiceLocationInitialization
    {
        /// <summary>
        /// Registers all the default registrations that are needed by the Splat module.
        /// </summary>
        /// <param name="resolver">The resolver to register the needed service types against.</param>
        public static void InitializeSplat(this IMutableDependencyResolver resolver)
        {
            if (resolver is null)
            {
                throw new System.ArgumentNullException(nameof(resolver));
            }

            RegisterDefaultLogManager(resolver);
            RegisterLogger(resolver);
        }

        private static void RegisterDefaultLogManager(IMutableDependencyResolver resolver)
        {
            if (!resolver.HasRegistration(typeof(ILogManager)))
            {
                resolver.Register(() => new DefaultLogManager(), typeof(ILogManager));
            }
        }

        private static void RegisterLogger(IMutableDependencyResolver resolver)
        {
            if (!resolver.HasRegistration(typeof(ILogger)))
            {
                resolver.RegisterConstant(new DebugLogger(), typeof(ILogger));
            }
        }
    }
}
