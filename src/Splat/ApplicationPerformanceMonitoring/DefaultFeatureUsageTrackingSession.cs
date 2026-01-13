// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Provides a default implementation of a feature usage tracking session, enabling tracking and logging of feature
/// usage events within an application.
/// </summary>
/// <remarks>This class creates a new tracking session for a specified feature and supports hierarchical tracking
/// by allowing sub-feature sessions. It logs session start, exceptions, and session completion events for diagnostic
/// purposes. Instances are intended to be disposed when tracking is complete to ensure proper logging of session
/// end.</remarks>
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
        ArgumentExceptionHelper.ThrowIfNullOrWhiteSpace(featureName);

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
        this.Log().Info(
            exception,
            () => "Feature Usage Tracking Exception");

    /// <inheritdoc/>
    public void Dispose() => this.Log().Info(() => $"Feature Finish: {FeatureReference}");

    private string GetSessionStartLogMessage()
    {
        var message = $"Feature Start. Reference={FeatureReference}";
        if (ParentReference != Guid.Empty)
        {
            message += $", Parent Reference={ParentReference}";
        }

        return message;
    }
}
