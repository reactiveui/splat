// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;

using Splat;
using Splat.Exceptionless;

namespace Splat.Builder;

/// <summary>
/// Provides a Splat module that configures logging to use Exceptionless as the logging backend.
/// </summary>
/// <remarks>This module enables integration of Exceptionless logging with Splat by registering an
/// Exceptionless-based logger in the dependency resolver. Register this module with your Splat dependency resolver to
/// route log messages to Exceptionless.</remarks>
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
