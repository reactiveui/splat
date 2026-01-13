// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Represents a feature usage tracking session that reports usage and exceptions to Exceptionless using unique
/// identifiers for each feature instance.
/// </summary>
/// <remarks>This class is used to track the usage of a specific feature within an application and to report
/// exceptions that occur during the session. Each session is associated with a unique feature reference and can create
/// sub-feature sessions for hierarchical tracking. Feature usage and exceptions are automatically submitted to
/// Exceptionless upon session creation and when exceptions are reported. This class is not thread-safe and is intended
/// for use within a single logical operation or request.</remarks>
public sealed class ExceptionlessFeatureUsageTrackingSession : IFeatureUsageTrackingSession<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionlessFeatureUsageTrackingSession"/> class.
    /// </summary>
    /// <param name="featureName">Name of the feature.</param>
    public ExceptionlessFeatureUsageTrackingSession(string featureName)
        : this(featureName, Guid.Empty)
    {
    }

    internal ExceptionlessFeatureUsageTrackingSession(
        string featureName,
        Guid parentReference)
    {
        ArgumentExceptionHelper.ThrowIfNullOrWhiteSpace(featureName);

        ParentReference = parentReference;
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();

        var client = ExceptionlessClient.Default;
        var eventBuilder = client.CreateFeatureUsage(featureName);

        if (!parentReference.Equals(Guid.Empty))
        {
            eventBuilder = eventBuilder.SetEventReference(FeatureName, FeatureReference.ToString());
        }

        eventBuilder.SetReferenceId(FeatureReference.ToString()).Submit();
    }

    /// <inheritdoc />
    public Guid ParentReference { get; }

    /// <inheritdoc />
    public Guid FeatureReference { get; }

    /// <inheritdoc />
    public string FeatureName { get; }

    /// <inheritdoc />
    public IFeatureUsageTrackingSession SubFeature(string description) =>
        new ExceptionlessFeatureUsageTrackingSession(
            description,
            FeatureReference);

    /// <inheritdoc />
    public void OnException(Exception exception)
    {
        var eventBuilder = exception.ToExceptionless()
            .SetEventReference(FeatureName, ParentReference.ToString())
            .SetReferenceId(FeatureReference.ToString());

        eventBuilder.Submit();
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
