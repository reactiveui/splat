// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;

namespace DispatchAdapters;

/// <summary>Runs the container and parcel-label examples in an isolated resolver scope.</summary>
internal static class Program
{
    /// <summary>Dispatches parcels through each container and reads a thumbnail back from PNG.</summary>
    /// <returns>Completion after the thumbnail has been decoded.</returns>
    internal static async Task Main()
    {
        using var resolver = new ModernDependencyResolver();
        using var scope = resolver.WithResolver();
        DispatchContainers.UsingAutofac();
        DispatchContainers.UsingDryIoc();
        DispatchContainers.UsingMicrosoftServices();
        DispatchContainers.UsingNinject();
        DispatchContainers.UsingSimpleInjector();
        DispatchContainers.UsingPrism();
        DispatchLogs.CheckProviders();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        await ParcelLabel.CheckThumbnailAsync(resolver, timeout.Token);
    }
}
