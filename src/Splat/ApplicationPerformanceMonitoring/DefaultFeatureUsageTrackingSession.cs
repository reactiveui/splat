// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Default Feature Usage Tracking Session. Used for output when a dev chooses not to override.
/// </summary>
public sealed class DefaultFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>, IEnableLogger
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">The name of the feature.</param>
    public DefaultFeatureUsageTrackingSession(string featureName)
        : this(featureName, Guid.Empty)
    {
    }

    internal DefaultFeatureUsageTrackingSession(string featureName, Guid parentReference)
    {
        if (string.IsNullOrWhiteSpace(featureName))
        {
            throw new ArgumentNullException(nameof(featureName));
        }

        ParentReference = parentReference;
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();

        this.Log().Info(GetSessionStartLogMessage);
    }

    /// <inheritdoc />
    public Guid ParentReference { get; }

    /// <inheritdoc />
    public Guid FeatureReference { get; }

    /// <inheritdoc />
    public string FeatureName { get; }

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) => new DefaultFeatureUsageTrackingSession(description, FeatureReference);

    /// <inheritdoc />
    public void OnException(Exception exception) =>
        this.Log().InfoException(
            () => "Feature Usage Tracking Exception",
            exception);

    /// <inheritdoc/>
    public void Dispose() => this.Log().Info(() => $"Feature Finish: {FeatureReference}");

    private string GetSessionStartLogMessage()
    {
        var message =
            $"Feature Start. Reference={FeatureReference}{(ParentReference != Guid.Empty ? $", Parent Reference={ParentReference}" : null)}";
        if (ParentReference != Guid.Empty)
        {
            message += $", Parent Reference={ParentReference}";
        }

        return message;
    }
}
