// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Represents a feature usage tracking session that reports usage events and exceptions to App Center Analytics and
/// Crashes.
/// </summary>
/// <remarks>This class is used to track the start and end of a feature usage session, as well as any exceptions
/// that occur during the session. It generates unique references for each session and supports hierarchical tracking of
/// sub-features. All tracking data is sent to Microsoft App Center services. Instances of this class are not
/// thread-safe.</remarks>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="AppCenterFeatureUsageTrackingSession"/> class to track usage of a specified.
    /// feature within the application.
    /// </summary>
    /// <param name="featureName">The name of the feature to be tracked. Cannot be null or empty.</param>
    /// <param name="parentReference">The unique identifier of the parent session or context to which this feature usage is related.</param>
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
