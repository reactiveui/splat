// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Provides extension methods for registering the default service implementations required by the Splat module with a
/// dependency resolver.
/// </summary>
public static class ServiceLocationInitializationExtensions
{
    /// <summary>Extension members for registering Splat's default service implementations on a resolver.</summary>
    /// <param name="resolver">The resolver the extension members operate on.</param>
    extension(IMutableDependencyResolver resolver)
    {
        /// <summary>Registers all the default registrations that are needed by the Splat module.</summary>
        public void InitializeSplat()
        {
            ArgumentExceptionHelper.ThrowIfNull(resolver);

            RegisterDefaultLogManager(resolver);
            RegisterLogger(resolver);
            RegisterApplicationPerformanceMonitoring(resolver);
        }
    }

    /// <summary>
    /// Registers the default application performance monitoring services with the specified dependency resolver if they
    /// are not already registered.
    /// </summary>
    /// <remarks>This method ensures that an implementation of IFeatureUsageTrackingManager is available in
    /// the dependency resolver. If no registration exists, a default implementation is registered. Call this method
    /// during application startup to enable feature usage tracking.</remarks>
    /// <param name="resolver">The dependency resolver to which the application performance monitoring services will be registered. Cannot be
    /// null.</param>
    private static void RegisterApplicationPerformanceMonitoring(IMutableDependencyResolver resolver)
    {
        if (resolver.HasRegistration<IFeatureUsageTrackingManager>())
        {
            return;
        }

        resolver.RegisterConstant<IFeatureUsageTrackingManager>(new DefaultFeatureUsageTrackingManager());
    }

    /// <summary>
    /// Registers the default log manager implementation with the specified dependency resolver if no log manager is
    /// already registered.
    /// </summary>
    /// <remarks>This method ensures that an ILogManager implementation is available for dependency
    /// resolution. If a log manager is already registered, this method does nothing.</remarks>
    /// <param name="resolver">The dependency resolver to which the default log manager will be registered. Cannot be null.</param>
    private static void RegisterDefaultLogManager(IMutableDependencyResolver resolver)
    {
        if (resolver.HasRegistration<ILogManager>())
        {
            return;
        }

        resolver.Register<ILogManager>(() => new DefaultLogManager(AppLocator.Current));
    }

    /// <summary>Registers a default logger implementation with the specified dependency resolver if no logger is already registered.</summary>
    /// <remarks>This method ensures that an ILogger implementation is available for dependency resolution. If
    /// a logger is already registered, this method does nothing.</remarks>
    /// <param name="resolver">The dependency resolver to which the logger implementation will be registered. Cannot be null.</param>
    private static void RegisterLogger(IMutableDependencyResolver resolver)
    {
        if (resolver.HasRegistration<ILogger>())
        {
            return;
        }

        resolver.RegisterConstant<ILogger>(new DebugLogger());
    }
}
