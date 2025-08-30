// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Log4Net;

namespace Splat.Builder;

/// <summary>
/// Splat module for configuring the Log4Net logger.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Log4NetSplatModule"/> class.
/// </remarks>
public sealed class Log4NetSplatModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseLog4NetWithWrappingFullLogger();
}
