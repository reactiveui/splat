// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

using Splat;
using Splat.Exceptionless;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Exceptionless integration.
/// </summary>
public sealed class ExceptionlessSplatModule : IModule
{
    private readonly ExceptionlessClient _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionlessSplatModule"/> class.
    /// </summary>
    /// <param name="exceptionlessClient">The Exceptionless client.</param>
    public ExceptionlessSplatModule(ExceptionlessClient exceptionlessClient)
    {
        ArgumentExceptionHelper.ThrowIfNull(exceptionlessClient);
        _container = exceptionlessClient;
    }

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseExceptionlessWithWrappingFullLogger(_container);
}
