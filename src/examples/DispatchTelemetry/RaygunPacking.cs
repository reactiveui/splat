// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mindscape.Raygun4Net;
using Splat;

namespace DispatchTelemetry;

/// <summary>Records a label printer fault within a packing session.</summary>
internal static class RaygunPacking
{
    /// <summary>Associates the supplied printer error with the active label operation.</summary>
    /// <param name="client">The application's Raygun client.</param>
    /// <param name="settings">The settings used by the client.</param>
    /// <param name="error">The error returned by the printer.</param>
    internal static void RecordPrinterFault(RaygunClient client, RaygunSettings settings, Exception error)
    {
        using var packing = new RaygunFeatureUsageTrackingSession("Pack order", client, settings);
        using var label = packing.SubFeature("Print label");
        label.OnException(error);
    }
}
