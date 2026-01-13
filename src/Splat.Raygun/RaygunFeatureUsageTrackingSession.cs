// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mindscape.Raygun4Net;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Represents a feature usage tracking session that reports feature usage events to Raygun.
/// </summary>
/// <remarks>This class is used to track the usage of a specific feature within an application and send usage
/// events to Raygun for monitoring and analytics. Each session is associated with a unique feature reference and can be
/// used to create sub-feature tracking sessions. Instances of this class are intended to be short-lived and disposed of
/// when tracking is complete.</remarks>
public sealed class RaygunFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    private readonly RaygunClient _raygunClient;
    private readonly RaygunSettings _raygunSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RaygunFeatureUsageTrackingSession"/> class for tracking usage of a specific.
    /// feature.
    /// </summary>
    /// <param name="featureName">The name of the feature to be tracked. Cannot be null or empty.</param>
    /// <param name="raygunClient">The RaygunClient instance used to send feature usage data. Cannot be null.</param>
    /// <param name="raygunSettings">The RaygunSettings instance that configures feature usage tracking. Cannot be null.</param>
    public RaygunFeatureUsageTrackingSession(
        string featureName,
        RaygunClient raygunClient,
        RaygunSettings raygunSettings)
        : this(
            featureName,
            Guid.Empty,
            raygunClient,
            raygunSettings)
    {
    }

    internal RaygunFeatureUsageTrackingSession(
        string featureName,
        Guid parentReference,
        RaygunClient raygunClient,
        RaygunSettings raygunSettings)
    {
        ArgumentExceptionHelper.ThrowIfNullOrWhiteSpace(featureName);

        _raygunClient = raygunClient;
        _raygunSettings = raygunSettings;

        ParentReference = parentReference;
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();

        var userCustomData = new Dictionary<string, string>
        {
            { "EventType", "FeatureUsage" },
            { "EventReference", FeatureReference.ToString() },
            { "ParentReference", parentReference.ToString() },
        };

        // keep an eye on
        // https://raygun.com/forums/thread/92182
        var messageBuilder = RaygunMessageBuilder.New(raygunSettings)
            .SetClientDetails()
            .SetEnvironmentDetails()
            .SetUserCustomData(userCustomData);
        var raygunMessage = messageBuilder.Build();
        _ = _raygunClient.SendInBackground(raygunMessage);
    }

    /// <inheritdoc />
    public Guid ParentReference { get; }

    /// <inheritdoc />
    public Guid FeatureReference { get; }

    /// <inheritdoc />
    public string FeatureName { get; }

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) =>
        new RaygunFeatureUsageTrackingSession(
            description,
            FeatureReference,
            _raygunClient,
            _raygunSettings);

    /// <inheritdoc />
    public void OnException(Exception exception) => _raygunClient.SendInBackground(exception);

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
