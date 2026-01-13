// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Extensions for the IEnableFeatureUsageTracking interface. This is a similar design to IEnableLogger, to allow
/// easy use and extension of classes such as ViewModels.
/// </summary>
public static class EnableFeatureUsageTrackingExtensions
{
    /// <summary>
    /// Creates a session for tracking usage of a specified feature on the given instance.
    /// </summary>
    /// <param name="instance">The object that enables feature usage tracking. Must not be null.</param>
    /// <param name="featureName">The name of the feature to track. Cannot be null or empty.</param>
    /// <returns>An <see cref="IFeatureUsageTrackingSession"/> instance for tracking usage of the specified feature.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the feature usage tracking manager service is not available.</exception>
    public static IFeatureUsageTrackingSession FeatureUsageTrackingSession(
        this IEnableFeatureUsageTracking instance,
        string featureName)
    {
        var featureUsageTrackingSession = AppLocator.Current.GetService<IFeatureUsageTrackingManager>();
        return featureUsageTrackingSession switch
        {
            null => throw new InvalidOperationException("Feature Usage Tracking Manager is null. This should never happen, your dependency resolver is broken"),
            _ => featureUsageTrackingSession.GetFeatureUsageTrackingSession(featureName)
        };
    }

    /// <summary>
    /// Helper for wrapping an action with a Feature Usage Tracking Session.
    /// </summary>
    /// <param name="instance">instance of class that uses IEnableFeatureUsageTracking.</param>
    /// <param name="featureName">Name of the feature.</param>
    /// <param name="action">Action to carry out.</param>
    public static void WithFeatureUsageTrackingSession(
        this IEnableFeatureUsageTracking instance,
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

    /// <summary>
    /// Helper for wrapping an action with a sub-feature Usage Tracking Session.
    /// </summary>
    /// <param name="instance">instance of class that uses IEnableFeatureUsageTracking.</param>
    /// <param name="featureName">Name of the feature.</param>
    /// <param name="action">Action to carry out.</param>
    public static void WithSubFeatureUsageTrackingSession(
        this IFeatureUsageTrackingSession instance,
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
