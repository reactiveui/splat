// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;
using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Feature Usage Tracking integration for Exceptionless.
/// </summary>
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
        if (string.IsNullOrWhiteSpace(featureName))
        {
            throw new ArgumentNullException(nameof(featureName));
        }

        ParentReference = parentReference;
        FeatureName = featureName;
        FeatureReference = Guid.NewGuid();

        var client = ExceptionlessClient.Default;
        var eventBuilder = client.CreateFeatureUsage(featureName);

        if (!parentReference.Equals(Guid.Empty))
        {
            eventBuilder.SetEventReference(FeatureName, FeatureReference.ToString());
        }

        eventBuilder.SetReferenceId(FeatureReference.ToString());
        eventBuilder.Submit();
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
