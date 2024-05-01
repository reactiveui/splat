// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Extensions for the IEnableFeatureUsageTracking interface. This is a similar design to IEnableLogger, to allow
/// easy use and extension of classes such as ViewModels.
/// </summary>
public static class EnableFeatureUsageTrackingExtensions
{
    /// <summary>
    /// Gets a Feature Usage Tracking Sessions.
    /// </summary>
    /// <param name="instance">instance of class that uses IEnableFeatureUsageTracking.</param>
    /// <param name="featureName">Name of the feature.</param>
    /// <returns>Feature Usage Tracking Session.</returns>
#pragma warning disable RCS1175 // Unused 'this' parameter.
    public static IFeatureUsageTrackingSession FeatureUsageTrackingSession(
        this IEnableFeatureUsageTracking instance,
        string featureName)
#pragma warning restore RCS1175 // Unused 'this' parameter.
    {
        var featureUsageTrackingSession = Locator.Current.GetService<IFeatureUsageTrackingManager>();
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
        action.ThrowArgumentNullExceptionIfNull(nameof(action));

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
    /// Helper for wrapping an action with a SubFeature Usage Tracking Session.
    /// </summary>
    /// <param name="instance">instance of class that uses IEnableFeatureUsageTracking.</param>
    /// <param name="featureName">Name of the feature.</param>
    /// <param name="action">Action to carry out.</param>
    public static void WithSubFeatureUsageTrackingSession(
        this IFeatureUsageTrackingSession instance,
        string featureName,
        Action<IFeatureUsageTrackingSession> action)
    {
        instance.ThrowArgumentNullExceptionIfNull(nameof(instance));
        action.ThrowArgumentNullExceptionIfNull(nameof(action));

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
