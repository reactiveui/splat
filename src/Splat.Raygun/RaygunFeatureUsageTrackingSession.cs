// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mindscape.Raygun4Net;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Feature Usage Tracking integration for Raygun.
/// </summary>
public sealed class RaygunFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    private readonly RaygunClient _raygunClient;
    private readonly RaygunSettings _raygunSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RaygunFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">Name of the feature.</param>
    /// <param name="raygunClient">Raygun client instance.</param>
    /// <param name="raygunSettings">Raygun settings instance.</param>
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
        if (string.IsNullOrWhiteSpace(featureName))
        {
            throw new ArgumentNullException(nameof(featureName));
        }

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
        _raygunClient.SendInBackground(raygunMessage);
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
