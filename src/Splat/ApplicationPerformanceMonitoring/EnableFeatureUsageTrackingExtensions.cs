// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Extensions for the IEnableFeatureUsageTracking interface. This is a similar design to IEnableLogger, to allow
/// easy use and extension of classes such as ViewModels.
/// </summary>
public static class EnableFeatureUsageTrackingExtensions
{
    /// <summary>Extension members for instances that participate in feature usage tracking.</summary>
    /// <param name="instance">The feature-usage-tracking-enabled instance the extension members operate on.</param>
    extension(IEnableFeatureUsageTracking instance)
    {
        /// <summary>Creates a session for tracking usage of a specified feature on the given instance.</summary>
        /// <param name="featureName">The name of the feature to track. Cannot be null or empty.</param>
        /// <returns>An <see cref="IFeatureUsageTrackingSession"/> instance for tracking usage of the specified feature.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the feature usage tracking manager service is not available.</exception>
        public IFeatureUsageTrackingSession FeatureUsageTrackingSession(string featureName)
        {
            var featureUsageTrackingSession = AppLocator.Current.GetService<IFeatureUsageTrackingManager>();
            return featureUsageTrackingSession switch
            {
                null => throw new InvalidOperationException("Feature Usage Tracking Manager is null. This should never happen, your dependency resolver is broken"),
                _ => featureUsageTrackingSession.GetFeatureUsageTrackingSession(featureName)
            };
        }

        /// <summary>Helper for wrapping an action with a Feature Usage Tracking Session.</summary>
        /// <param name="featureName">Name of the feature.</param>
        /// <param name="action">Action to carry out.</param>
        public void WithFeatureUsageTrackingSession(
            string featureName,
            Action<IFeatureUsageTrackingSession> action)
        {
            ArgumentExceptionHelper.ThrowIfNull(action);

            using var session = instance.FeatureUsageTrackingSession(featureName);
            try
            {
                action(session);
            }
            catch (Exception exception)
            {
                session.OnException(exception);
                throw;
            }
        }
    }

    /// <summary>Extension members for an existing feature usage tracking session.</summary>
    /// <param name="instance">The feature usage tracking session the extension members operate on.</param>
    extension(IFeatureUsageTrackingSession instance)
    {
        /// <summary>Helper for wrapping an action with a sub-feature Usage Tracking Session.</summary>
        /// <param name="featureName">Name of the feature.</param>
        /// <param name="action">Action to carry out.</param>
        public void WithSubFeatureUsageTrackingSession(
            string featureName,
            Action<IFeatureUsageTrackingSession> action)
        {
            ArgumentExceptionHelper.ThrowIfNull(instance);
            ArgumentExceptionHelper.ThrowIfNull(action);

            using var session = instance.SubFeature(featureName);
            try
            {
                action(session);
            }
            catch (Exception exception)
            {
                session.OnException(exception);
                throw;
            }
        }
    }
}
