// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>Provides view tracking functionality by recording view navigation events using an Exceptionless client.</summary>
public sealed class ExceptionlessViewTracking : IViewTracking
{
    /// <summary>The Exceptionless client that view navigation events are submitted to.</summary>
    private readonly ExceptionlessClient _exceptionlessClient;

    /// <summary>Initializes a new instance of the <see cref="ExceptionlessViewTracking"/> class.</summary>
    /// <param name="exceptionlessClient">The exceptionless client to use.</param>
    public ExceptionlessViewTracking(ExceptionlessClient exceptionlessClient)
    {
        ArgumentExceptionHelper.ThrowIfNull(exceptionlessClient);
        _exceptionlessClient = exceptionlessClient;
    }

    /// <summary>Track a view navigation using just a name.</summary>
    /// <param name="name">Name of the view.</param>
    public void OnViewNavigation(string name)
    {
        // need to consider whether to just use feature event
        // and tag it with view specific properties.
        var eventBuilder = _exceptionlessClient
            .CreateEvent()
            .SetType("PageView")
            .SetMessage(name);

        eventBuilder.Submit();
    }
}
