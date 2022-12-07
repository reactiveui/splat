// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using Exceptionless;

using Splat.ApplicationPerformanceMonitoring;

namespace Splat;

/// <summary>
/// Exceptionless View Tracking integration.
/// </summary>
public sealed class ExceptionlessViewTracking : IViewTracking
{
    private readonly ExceptionlessClient _exceptionlessClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionlessViewTracking"/> class.
    /// </summary>
    /// <param name="exceptionlessClient">The exceptionless client to use.</param>
    public ExceptionlessViewTracking(ExceptionlessClient exceptionlessClient)
    {
        _exceptionlessClient = exceptionlessClient ?? throw new ArgumentNullException(nameof(exceptionlessClient));
    }

    /// <summary>
    /// Track a view navigation using just a name.
    /// </summary>
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
