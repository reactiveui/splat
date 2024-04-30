// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Feature Usage Tracking Client for AppCenter.
/// </summary>
public sealed class AppCenterFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppCenterFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">The name of the feature.</param>
    public AppCenterFeatureUsageTrackingSession(string featureName)
        : this(featureName, Guid.Empty)
    {
    }

    internal AppCenterFeatureUsageTrackingSession(string featureName, Guid parentReference)
    {
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();
        ParentReference = parentReference;

        TrackEvent("Feature Usage Start");
    }

    /// <inheritdoc/>
    public string FeatureName { get; }

    /// <inheritdoc/>
    public Guid FeatureReference { get; }

    /// <inheritdoc/>
    public Guid ParentReference { get; }

    /// <inheritdoc />
    public void Dispose() => TrackEvent("Feature Usage End");

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) => new AppCenterFeatureUsageTrackingSession(description, FeatureReference);

    /// <inheritdoc />
    public void OnException(Exception exception)
    {
        var properties = GetProperties();
        Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
    }

    private Dictionary<string, string> GetProperties()
    {
        var properties = new Dictionary<string, string>
        {
            { "Name", FeatureName },
            { "Reference", FeatureReference.ToString() },
        };

        if (ParentReference != Guid.Empty)
        {
            properties.Add("ParentReference", ParentReference.ToString());
        }

        return properties;
    }

    private void TrackEvent(string eventName)
    {
        var properties = GetProperties();

        Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName, properties);
    }
}
