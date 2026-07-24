// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.ApplicationInsightsV3;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Tests the shared Application Insights v3 telemetry factory.</summary>
public sealed class ApplicationInsightsTelemetryFactoryTests
{
    /// <summary>The common property name for view and feature names.</summary>
    private const string NamePropertyName = "Name";

    /// <summary>The property name for the current feature reference.</summary>
    private const string ReferencePropertyName = "Reference";

    /// <summary>The property name for the parent feature reference.</summary>
    private const string ParentReferencePropertyName = "ParentReference";

    /// <summary>The feature name shared by root and exception telemetry tests.</summary>
    private const string SearchFeatureName = "Search";

    /// <summary>Verifies that view navigation telemetry uses the documented custom PageView event shape.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task CreateViewNavigationEvent_WithName_ReturnsPageViewEvent()
    {
        var telemetry = ApplicationInsightsTelemetryFactory.CreateViewNavigationEvent("Home");

        using (Assert.Multiple())
        {
            await Assert.That(telemetry.Name).IsEqualTo("PageView");
            await Assert.That(telemetry.Properties[NamePropertyName]).IsEqualTo("Home");
        }
    }

    /// <summary>Verifies that root feature events include feature metadata without a parent reference.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task CreateFeatureUsageEvent_WithRootFeature_ReturnsEventWithoutParentReference()
    {
        var featureReference = Guid.NewGuid();

        var telemetry = ApplicationInsightsTelemetryFactory.CreateFeatureUsageEvent(
            "Feature Usage Start",
            SearchFeatureName,
            featureReference,
            Guid.Empty);

        using (Assert.Multiple())
        {
            await Assert.That(telemetry.Name).IsEqualTo("Feature Usage Start");
            await Assert.That(telemetry.Properties[NamePropertyName]).IsEqualTo(SearchFeatureName);
            await Assert.That(telemetry.Properties[ReferencePropertyName]).IsEqualTo(featureReference.ToString());
            await Assert.That(telemetry.Properties.ContainsKey(ParentReferencePropertyName)).IsFalse();
        }
    }

    /// <summary>Verifies that child feature events include the parent feature reference.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task CreateFeatureUsageEvent_WithChildFeature_ReturnsEventWithParentReference()
    {
        var featureReference = Guid.NewGuid();
        var parentReference = Guid.NewGuid();

        var telemetry = ApplicationInsightsTelemetryFactory.CreateFeatureUsageEvent(
            "Feature Usage End",
            "Filter",
            featureReference,
            parentReference);

        using (Assert.Multiple())
        {
            await Assert.That(telemetry.Properties[NamePropertyName]).IsEqualTo("Filter");
            await Assert.That(telemetry.Properties[ReferencePropertyName]).IsEqualTo(featureReference.ToString());
            await Assert.That(telemetry.Properties[ParentReferencePropertyName]).IsEqualTo(parentReference.ToString());
        }
    }

    /// <summary>Verifies that exception telemetry carries the exception and feature metadata.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task CreateExceptionTelemetry_WithException_ReturnsExceptionTelemetryWithFeatureMetadata()
    {
        var exception = new InvalidOperationException("boom");
        var featureReference = Guid.NewGuid();

        var telemetry = ApplicationInsightsTelemetryFactory.CreateExceptionTelemetry(
            exception,
            SearchFeatureName,
            featureReference,
            Guid.Empty);

        using (Assert.Multiple())
        {
            await Assert.That(telemetry.Exception).IsSameReferenceAs(exception);
            await Assert.That(telemetry.Properties[NamePropertyName]).IsEqualTo(SearchFeatureName);
            await Assert.That(telemetry.Properties[ReferencePropertyName]).IsEqualTo(featureReference.ToString());
        }
    }
}
