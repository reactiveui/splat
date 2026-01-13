// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

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
        ArgumentExceptionHelper.ThrowIfNull(resolver);

        RegisterDefaultLogManager(resolver);
        RegisterLogger(resolver);
        RegisterApplicationPerformanceMonitoring(resolver);
    }

    private static void RegisterApplicationPerformanceMonitoring(IMutableDependencyResolver resolver)
    {
        if (!resolver.HasRegistration<IFeatureUsageTrackingManager>())
        {
            resolver.RegisterConstant<IFeatureUsageTrackingManager>(new DefaultFeatureUsageTrackingManager());
        }
    }

    private static void RegisterDefaultLogManager(IMutableDependencyResolver resolver)
    {
        if (!resolver.HasRegistration<ILogManager>())
        {
            resolver.Register<ILogManager>(() => new DefaultLogManager(AppLocator.Current));
        }
    }

    private static void RegisterLogger(IMutableDependencyResolver resolver)
    {
        if (!resolver.HasRegistration<ILogger>())
        {
            resolver.RegisterConstant<ILogger>(new DebugLogger());
        }
    }
}
