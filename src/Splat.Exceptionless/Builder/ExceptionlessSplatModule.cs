// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Exceptionless;
using Splat;
using Splat.Exceptionless;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Exceptionless dependency resolver.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionlessSplatModule"/> class.
/// </remarks>
/// <param name="exceptionlessClient">The Exceptionless container.</param>
public sealed class ExceptionlessSplatModule(ExceptionlessClient exceptionlessClient) : IReactiveUIModule
{
    private readonly ExceptionlessClient _container = exceptionlessClient ?? throw new ArgumentNullException(nameof(exceptionlessClient));

    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseExceptionlessWithWrappingFullLogger(_container);
}
