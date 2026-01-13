// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

using Splat.Microsoft.Extensions.Logging;

namespace Splat.Builder;

/// <summary>
/// Provides a Splat module that configures logging using Microsoft.Extensions.Logging and wraps loggers for Splat
/// compatibility.
/// </summary>
/// <remarks>This module enables integration of Microsoft.Extensions.Logging with Splat's logging infrastructure.
/// Register this module with your dependency resolver to enable logging through Microsoft.Extensions.Logging in
/// Splat-based applications.</remarks>
/// <param name="loggerFactory">The Microsoft.Extensions.Logging.ILoggerFactory instance to use for creating loggers.</param>
public sealed class MicrosoftExtensionsLoggingSplatModule(ILoggerFactory loggerFactory) : IModule
{
    /// <inheritdoc />
    public void Configure(IMutableDependencyResolver resolver) => resolver.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(loggerFactory);
}
