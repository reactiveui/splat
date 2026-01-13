// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Log4Net;

namespace Splat.Builder;

/// <summary>
/// Provides a Splat module that configures logging to use log4net with full logger wrapping.
/// </summary>
/// <remarks>This module is intended for use with Splat's dependency resolver infrastructure. Register this module
/// to enable log4net-based logging throughout your application. This module is typically used in applications that
/// require integration between Splat and log4net for consistent logging behavior.</remarks>
public sealed class Log4NetSplatModule() : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseLog4NetWithWrappingFullLogger();
}
