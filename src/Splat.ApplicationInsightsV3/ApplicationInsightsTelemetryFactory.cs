// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights.DataContracts;

namespace Splat.ApplicationInsightsV3;

/// <summary>Creates Application Insights v3 telemetry shapes used by the Splat APM adapters.</summary>
internal static class ApplicationInsightsTelemetryFactory
{
    /// <summary>The common property name for view and feature names.</summary>
    private const string NamePropertyName = "Name";

    /// <summary>The property name for the current feature reference.</summary>
    private const string ReferencePropertyName = "Reference";

    /// <summary>The property name for the parent feature reference.</summary>
    private const string ParentReferencePropertyName = "ParentReference";

    /// <summary>The custom event name used for view tracking in Application Insights v3.</summary>
    private const string PageViewEventName = "PageView";

    /// <summary>Creates the custom event used for view navigation in Application Insights v3.</summary>
    /// <param name="name">The view name recorded in event properties.</param>
    /// <returns>The event telemetry to send.</returns>
    internal static EventTelemetry CreateViewNavigationEvent(string name)
    {
        var eventTelemetry = new EventTelemetry(PageViewEventName);
        eventTelemetry.Properties.Add(NamePropertyName, name);

        return eventTelemetry;
    }

    /// <summary>Creates a feature usage event with Splat feature metadata.</summary>
    /// <param name="eventName">The Application Insights event name.</param>
    /// <param name="featureName">The Splat feature name.</param>
    /// <param name="featureReference">The feature session reference.</param>
    /// <param name="parentReference">The parent feature reference, or <see cref="Guid.Empty"/> for root sessions.</param>
    /// <returns>The event telemetry to send.</returns>
    internal static EventTelemetry CreateFeatureUsageEvent(
        string eventName,
        string featureName,
        Guid featureReference,
        Guid parentReference)
    {
        var eventTelemetry = new EventTelemetry(eventName);
        AddFeatureProperties(eventTelemetry, featureName, featureReference, parentReference);

        return eventTelemetry;
    }

    /// <summary>Creates exception telemetry with Splat feature metadata.</summary>
    /// <param name="exception">The exception captured during the feature session.</param>
    /// <param name="featureName">The Splat feature name.</param>
    /// <param name="featureReference">The feature session reference.</param>
    /// <param name="parentReference">The parent feature reference, or <see cref="Guid.Empty"/> for root sessions.</param>
    /// <returns>The exception telemetry to send.</returns>
    internal static ExceptionTelemetry CreateExceptionTelemetry(
        Exception exception,
        string featureName,
        Guid featureReference,
        Guid parentReference)
    {
        var telemetry = new ExceptionTelemetry(exception);
        AddFeatureProperties(telemetry, featureName, featureReference, parentReference);

        return telemetry;
    }

    /// <summary>Adds the metadata shared by feature usage events and exceptions.</summary>
    /// <typeparam name="TTelemetry">The telemetry shape that carries custom properties.</typeparam>
    /// <param name="telemetry">The telemetry item being populated.</param>
    /// <param name="featureName">The Splat feature name.</param>
    /// <param name="featureReference">The feature session reference.</param>
    /// <param name="parentReference">The parent feature reference, or <see cref="Guid.Empty"/> for root sessions.</param>
    private static void AddFeatureProperties<TTelemetry>(
        TTelemetry telemetry,
        string featureName,
        Guid featureReference,
        Guid parentReference)
        where TTelemetry : ISupportProperties
    {
        telemetry.Properties.Add(NamePropertyName, featureName);
        telemetry.Properties.Add(ReferencePropertyName, featureReference.ToString());

        if (parentReference == Guid.Empty)
        {
            return;
        }

        telemetry.Properties.Add(ParentReferencePropertyName, parentReference.ToString());
    }
}
