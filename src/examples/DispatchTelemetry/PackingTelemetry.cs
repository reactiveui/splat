// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.ApplicationInsights;
using Splat.ApplicationPerformanceMonitoring;

namespace DispatchTelemetry;

/// <summary>Tracks navigation to the packing desk and records label printing as a child operation.</summary>
internal static class PackingTelemetry
{
    /// <summary>Records the packing screen and returns the relationship between the two feature sessions.</summary>
    /// <param name="client">The application's configured Application Insights client.</param>
    /// <returns>The packing session ID and the parent ID recorded by the label session.</returns>
    internal static (Guid PackingId, Guid LabelParentId) PrintLabel(TelemetryClient client)
    {
        var views = new ApplicationInsightsViewTracking(client);
        views.OnViewNavigation("Packing desk");
        var sessions = new FuncFeatureUsageTrackingManager(name => new ApplicationInsightsFeatureUsageTrackingSession(name, client));
        using var packing = (IFeatureUsageTrackingSession<Guid>)sessions.GetFeatureUsageTrackingSession("Pack order");
        using var label = (IFeatureUsageTrackingSession<Guid>)packing.SubFeature("Print label");
        return (packing.FeatureReference, label.ParentReference);
    }
}
